using OpenCvSharp;

using ChartDeskewApp.Core.Interfaces;

namespace ChartDeskewApp.Core.Services;

/// <summary>
/// Main image processing service implementation
/// </summary>
public class ImageProcessor : IImageProcessor
{

  public ImageProcessor()
  {
  }

  /// <summary>
  /// Checks if a given contour is likely to be circular based on area, convex hull, and shape metrics.
  /// </summary>
  /// <param name="contour">The contour to evaluate.</param>
  /// <returns>True if the contour is likely circular, false otherwise.</returns>
  private bool IsContourCircular(OpenCvSharp.Point[] contour)
  {
    if (contour == null || contour.Length < 5)
      return false;

    // Calculate area of the contour
    double area = Cv2.ContourArea(contour);
    if (area < 5000) // skip very small contours and less than centeric black hole area
      return false;

    // Calculate area of the convex hull
    var hull = Cv2.ConvexHull(contour);
    double hullArea = Cv2.ContourArea(hull);
    if (hullArea < 1e-7) // avoid division by zero
      return false;

    // Compare areas: for circles, area ≈ convex hull area
    double areaRatio = area / hullArea;

    // Calculate perimeter and circularity
    // double perimeter = Cv2.ArcLength(contour, true);
    // if (perimeter == 0)
    //   return false;
    // double circularity = 4 * Math.PI * area / (perimeter * perimeter);

    // Optionally, fit an ellipse and check axes ratio
    if (contour.Length >= 5)
    {
      var ellipse = Cv2.FitEllipse(contour);
      double axisRatio = ellipse.Size.Width / ellipse.Size.Height;
      if (axisRatio < 1.0) axisRatio = 1.0 / axisRatio; // treat ratios <1 as 1/ratio
      // Accept slightly squashed, but mostly round
      if (axisRatio > 1.2)
        return false;
    }

    // Typical values for circular objects:
    // areaRatio ≈ 1, circularity ≈ 1
    // return areaRatio > 0.8 && areaRatio < 1.2 && circularity > 0.75;
    return areaRatio > 0.9 && areaRatio < 1.1;
  }

  /// <summary>
  /// Comprehensive contour refinement for clean arc points
  /// </summary>

  /// <summary>
  /// Removes unnecessary points: interior points, noise, and sticks
  /// </summary>
  private (double rotationAngle, double scaleX, double scaleY) CalculateCircularizationParams(List<OpenCvSharp.Point[]> contours)
  {
    if (contours.Count == 0) return (0, 1, 1);

    var ellipseData = new List<(double angle, double majorAxis, double minorAxis)>();

    // Fit ellipses and collect parameters
    foreach (var contour in contours)
    {
      if (contour.Length < 5) continue;

      try
      {
        var ellipse = Cv2.FitEllipse(contour);
        double majorAxis = Math.Max(ellipse.Size.Width, ellipse.Size.Height);
        double minorAxis = Math.Min(ellipse.Size.Width, ellipse.Size.Height);

        ellipseData.Add((ellipse.Angle, majorAxis, minorAxis));
      }
      catch { }
    }

    if (ellipseData.Count == 0) return (0, 1, 1);

    // Calculate weighted averages
    double totalWeight = ellipseData.Sum(x => x.majorAxis * x.minorAxis);
    double avgAngle = ellipseData.Sum(x => x.angle * x.majorAxis * x.minorAxis) / totalWeight;
    double avgMajorAxis = ellipseData.Sum(x => x.majorAxis * x.majorAxis * x.minorAxis) / totalWeight;
    double avgMinorAxis = ellipseData.Sum(x => x.minorAxis * x.majorAxis * x.minorAxis) / totalWeight;

    // Calculate transformation parameters
    double rotationAngle = -avgAngle;
    double aspectRatio = avgMajorAxis / avgMinorAxis;

    // Determine which axis to scale
    double scaleX = 1.0;
    double scaleY = 1.0;

    if (Math.Abs(avgAngle) < 45) // Ellipse is wider than tall
    {
      scaleX = aspectRatio;
    }
    else // Ellipse is taller than wide
    {
      scaleY = aspectRatio;
    }

    return (rotationAngle, scaleX, scaleY);
  }

  public async Task<List<OpenCvSharp.Point[]>> DetectRingsAsync(byte[] imageData)
  {
    return await Task.Run(() =>
    {
      // Convert byte array to OpenCvSharp Mat
      Mat img = Mat.FromImageData(imageData, ImreadModes.Grayscale);

      // Apply median blur to reduce noise and help with circular detection
      Mat blurred = new Mat();
      Cv2.MedianBlur(img, blurred, 5);

      // Use adaptive threshold for potentially variable lighting in circular grid images
      Mat binary = new Mat();
      Cv2.AdaptiveThreshold(
        blurred,
        binary,
        255,
        AdaptiveThresholdTypes.GaussianC,
        ThresholdTypes.BinaryInv,   // invert: circles become white on black bg
        15,
        2
      );

      // Detect contours using connected components (including nested/ringed structure)
      OpenCvSharp.Point[][] contours;
      HierarchyIndex[] hierarchy;
      Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

      // Filter for ellipses using multiple criteria
      var result = new List<OpenCvSharp.Point[]>();
      for (int i = 0; i < contours.Length; i++)
      {
        var contour = contours[i];
        if (IsContourCircular(contour))
        {
          result.Add(contour);
        }
      }

      // Clean up resources
      img.Dispose();
      blurred.Dispose();
      binary.Dispose();

      return result;
    });
  }


  /// <summary>
  /// Applies deskew transformation to make ellipses circular
  /// </summary>
  private async Task<byte[]> DeskewToCircularAsync(byte[] imageData)
  {
    return await Task.Run(async () =>
    {
      // Detect rings
      var contours = await DetectRingsAsync(imageData);

      // Calculate circularization parameters
      var (rotationAngle, scaleX, scaleY) = CalculateCircularizationParams(contours);
      Console.WriteLine($"Rotation Angle: {rotationAngle}, Scale X: {scaleX}, Scale Y: {scaleY}");

      // Load original image
      Mat originalImg = Mat.FromImageData(imageData, ImreadModes.Color);
      Mat deskewedImg = new Mat();

      // Get image center
      var center = new OpenCvSharp.Point2f(originalImg.Width / 2, originalImg.Height / 2);

      // Step 1: Rotate to align with axes
      var rotationMatrix = Cv2.GetRotationMatrix2D(center, rotationAngle, 1.0);
      Mat rotatedImg = new Mat();
      Cv2.WarpAffine(originalImg, rotatedImg, rotationMatrix, originalImg.Size());

      // Step 2: Scale to make circular
      var srcPoints = new OpenCvSharp.Point2f[] {
          new OpenCvSharp.Point2f(0, 0),
          new OpenCvSharp.Point2f(1, 0),
          new OpenCvSharp.Point2f(0, 1)
      };

      var dstPoints = new OpenCvSharp.Point2f[] {
        new OpenCvSharp.Point2f(0, 0),
        new OpenCvSharp.Point2f((float)scaleX, 0),
        new OpenCvSharp.Point2f(0, (float)scaleY)
    };

      var scaleMatrix = Cv2.GetAffineTransform(srcPoints, dstPoints);

      Cv2.WarpAffine(rotatedImg, deskewedImg, scaleMatrix, originalImg.Size());

      // Convert back to byte array
      byte[] result = deskewedImg.ImEncode(".png");

      // Clean up
      originalImg.Dispose();
      rotatedImg.Dispose();
      deskewedImg.Dispose();
      rotationMatrix.Dispose();
      scaleMatrix.Dispose();

      return result;
    });
  }

  public async Task<byte[]> DrawContoursAsync(byte[] imageData, List<OpenCvSharp.Point[]> contours)
  {
    return await Task.Run(() =>
    {
      Console.WriteLine($"Contours: {contours.Count}");

      // Decode the original image for drawing (preferably in color)
      // Clone the original image so that the original one is not updated
      Mat originalImg = Mat.FromImageData(imageData, ImreadModes.Color);
      Mat imgColor = originalImg.Clone();
      originalImg.Dispose();

      // Draw contours in blue
      Scalar blueColor = new Scalar(255, 0, 0); // BGR for blue
      Cv2.DrawContours(
          image: imgColor,
          contours: contours,
          contourIdx: -1, // draw all contours
          color: blueColor,
          thickness: 2
      );

      // Encode the result image (Mat) back to a byte array (e.g., PNG format)
      byte[] outputImage = imgColor.ImEncode(".png");
      imgColor.Dispose();

      return outputImage;
    });
  }

  public async Task<byte[]> DeskewImageAsync(byte[] image)
  {
    return await Task.Run(async () =>
    {
      var contours = await this.DetectRingsAsync(image);
      var deskewedImage = await this.DeskewToCircularAsync(image);

      var drawnImage = await this.DrawContoursAsync(image, contours);
      return deskewedImage;
    });
  }
}