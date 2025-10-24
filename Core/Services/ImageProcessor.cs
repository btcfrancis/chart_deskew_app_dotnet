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
    if (area < 10000) // skip very small contours and less than centeric black hole area
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

  public async Task<List<OpenCvSharp.Point[]>> DetectContoursAsync(byte[] imageData)
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
          result.Add(Cv2.ConvexHull(contour));
      }

      // Clean up resources
      img.Dispose();
      blurred.Dispose();
      binary.Dispose();

      return result;
    });
  }

  public async Task<byte[]> DrawContoursAsync(byte[] imageData)
  {
    // Detect contours first
    var contours = await DetectContoursAsync(imageData);

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
  }
}