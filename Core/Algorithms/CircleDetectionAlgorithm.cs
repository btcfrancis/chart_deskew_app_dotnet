using OpenCvSharp;

using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Algorithms;

/// <summary>
/// Implementation of circle detection using Hough Circle Transform
/// </summary>
public class CircleDetectionAlgorithm : ICircleDetector
{
  private readonly IGeometryHelper _geometryHelper;

  public CircleDetectionAlgorithm(IGeometryHelper geometryHelper)
  {
    _geometryHelper = geometryHelper ?? throw new ArgumentNullException(nameof(geometryHelper));
  }

  public async Task<List<CircleDetection>> DetectCirclesAsync(Bitmap image)
  {
    return await Task.Run(() =>
    {
      using var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(image);
      using var gray = new Mat();

      // Convert to grayscale
      Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

      // Apply Gaussian blur
      Cv2.GaussianBlur(gray, gray, new OpenCvSharp.Size(9, 9), 2, 2);

      // Detect circles using Hough Circle Transform
      var circles = Cv2.HoughCircles(
              gray,
              HoughModes.Gradient,
              1,
              gray.Rows / 8,
              param1: 50,
              param2: 30,
              minRadius: 10,
              maxRadius: Math.Min(gray.Width, gray.Height) / 2
          );

      var detections = new List<CircleDetection>();

      if (circles != null)
      {
        foreach (var circle in circles)
        {
          var detection = new CircleDetection
          {
            Center = new Point((int)circle.Center.X, (int)circle.Center.Y),
            Radius = (int)circle.Radius,
            Confidence = 1.0, // Hough circles don't provide confidence
            Quality = CalculateCircleQuality(gray, circle),
            EdgePointCount = CountEdgePoints(gray, circle),
            RadialStandardDeviation = CalculateRadialStdDev(gray, circle)
          };

          detections.Add(detection);
        }
      }

      return detections.OrderByDescending(d => d.Quality).ToList();
    });
  }

  public async Task<CircleDetection?> FindBestCircleAsync(List<CircleDetection> circles)
  {
    if (!circles.Any()) return null;

    return await Task.FromResult(circles
        .OrderByDescending(c => c.Quality)
        .ThenByDescending(c => c.Confidence)
        .FirstOrDefault());
  }

  private double CalculateCircleQuality(Mat image, Vec3f circle)
  {
    var center = new Point((int)circle.Center.X, (int)circle.Center.Y);
    var radius = (int)circle.Radius;

    if (center.X - radius < 0 || center.Y - radius < 0 ||
        center.X + radius >= image.Width || center.Y + radius >= image.Height)
      return 0;

    // Sample points on the circle circumference
    var samplePoints = new List<Point>();
    for (int i = 0; i < 360; i += 5)
    {
      var angle = i * Math.PI / 180;
      var x = (int)(center.X + radius * Math.Cos(angle));
      var y = (int)(center.Y + radius * Math.Sin(angle));
      samplePoints.Add(new Point(x, y));
    }

    // Calculate edge strength
    var edgeStrengths = samplePoints
        .Where(p => p.X >= 0 && p.Y >= 0 && p.X < image.Width && p.Y < image.Height)
        .Select(p => image.At<byte>(p.Y, p.X))
        .ToList();

    if (!edgeStrengths.Any()) return 0;

    var avgEdgeStrength = edgeStrengths.Average();
    var variance = edgeStrengths.Select(s => Math.Pow(s - avgEdgeStrength, 2)).Average();
    var stdDev = Math.Sqrt(variance);

    // Quality is based on edge strength and consistency
    return Math.Min(1.0, avgEdgeStrength / 255.0) * (1.0 - stdDev / 255.0);
  }

  private int CountEdgePoints(Mat image, Vec3f circle)
  {
    var center = new Point((int)circle.Center.X, (int)circle.Center.Y);
    var radius = (int)circle.Radius;
    var count = 0;

    for (int i = 0; i < 360; i += 2)
    {
      var angle = i * Math.PI / 180;
      var x = (int)(center.X + radius * Math.Cos(angle));
      var y = (int)(center.Y + radius * Math.Sin(angle));

      if (x >= 0 && y >= 0 && x < image.Width && y < image.Height)
      {
        if (image.At<byte>(y, x) > 128) // Threshold for edge detection
          count++;
      }
    }

    return count;
  }

  private double CalculateRadialStdDev(Mat image, Vec3f circle)
  {
    var center = new Point((int)circle.Center.X, (int)circle.Center.Y);
    var radius = (int)circle.Radius;
    var distances = new List<double>();

    for (int i = 0; i < 360; i += 5)
    {
      var angle = i * Math.PI / 180;
      var x = (int)(center.X + radius * Math.Cos(angle));
      var y = (int)(center.Y + radius * Math.Sin(angle));

      if (x >= 0 && y >= 0 && x < image.Width && y < image.Height)
      {
        var distance = _geometryHelper.CalculateDistance(center, new Point(x, y));
        distances.Add(distance);
      }
    }

    if (distances.Count == 0) return 0;

    var avg = distances.Average();
    var variance = distances.Select(d => Math.Pow(d - avg, 2)).Average();
    return Math.Sqrt(variance);
  }
}