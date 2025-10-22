using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Services;

/// <summary>
/// Chart analysis service implementation
/// </summary>
public class ChartAnalyzer : IChartAnalyzer
{
  private readonly ICircleDetector _circleDetector;
  private readonly IGeometryHelper _geometryHelper;

  public ChartAnalyzer(ICircleDetector circleDetector, IGeometryHelper geometryHelper)
  {
    _circleDetector = circleDetector ?? throw new ArgumentNullException(nameof(circleDetector));
    _geometryHelper = geometryHelper ?? throw new ArgumentNullException(nameof(geometryHelper));
  }

  public async Task<ChartAnalysisResult> AnalyzeChartAsync(Bitmap image)
  {
    var startTime = DateTime.Now;

    try
    {
      // Detect circles in the image
      var circles = await _circleDetector.DetectCirclesAsync(image);

      if (!circles.Any())
      {
        return new ChartAnalysisResult
        {
          IsValid = false,
          Confidence = 0.0,
          Metadata = new AnalysisMetadata
          {
            ProcessingTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds,
            CirclesDetected = 0,
            ImageSize = image.Size
          }
        };
      }

      // Find the best circle (outer boundary)
      var bestCircle = await _circleDetector.FindBestCircleAsync(circles);
      if (bestCircle == null)
      {
        return new ChartAnalysisResult
        {
          IsValid = false,
          Confidence = 0.0,
          Metadata = new AnalysisMetadata
          {
            ProcessingTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds,
            CirclesDetected = circles.Count,
            ImageSize = image.Size
          }
        };
      }

      // Detect center and rings
      var center = await DetectCenterAsync(image);
      var (innerRadius, outerRadius) = await DetectRingsAsync(image, center);

      // Calculate confidence
      var confidence = CalculateConfidence(bestCircle, center, innerRadius, outerRadius);

      var result = new ChartAnalysisResult
      {
        Center = center,
        InnerRadius = innerRadius,
        OuterRadius = outerRadius,
        Confidence = confidence,
        IsValid = confidence > 0.5,
        Metadata = new AnalysisMetadata
        {
          ProcessingTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds,
          CirclesDetected = circles.Count,
          ImageSize = image.Size
        }
      };

      return result;
    }
    catch (Exception ex)
    {
      return new ChartAnalysisResult
      {
        IsValid = false,
        Confidence = 0.0,
        Metadata = new AnalysisMetadata
        {
          ProcessingTimeMs = (long)(DateTime.Now - startTime).TotalMilliseconds,
          ImageSize = image.Size,
          Notes = $"Analysis failed: {ex.Message}"
        }
      };
    }
  }

  public async Task<Point> DetectCenterAsync(Bitmap image)
  {
    return await Task.Run(() =>
    {
      // Simplified center detection - use image center for now
      // In a real implementation, this would analyze the image to find the actual center
      return new Point(image.Width / 2, image.Height / 2);
    });
  }

  public async Task<(int innerRadius, int outerRadius)> DetectRingsAsync(Bitmap image, Point center)
  {
    return await Task.Run(() =>
    {
      // Simplified ring detection
      // In a real implementation, this would analyze radial intensity patterns
      var maxRadius = Math.Min(image.Width, image.Height) / 2;
      var outerRadius = (int)(maxRadius * 0.9);
      var innerRadius = (int)(maxRadius * 0.1);

      return (innerRadius, outerRadius);
    });
  }

  private double CalculateConfidence(CircleDetection bestCircle, Point center, int innerRadius, int outerRadius)
  {
    var centerDistance = _geometryHelper.CalculateDistance(bestCircle.Center, center);
    var radiusRatio = (double)innerRadius / outerRadius;

    // Confidence based on circle quality, center alignment, and reasonable radius ratio
    var centerConfidence = Math.Max(0, 1.0 - centerDistance / 100.0);
    var radiusConfidence = radiusRatio > 0.1 && radiusRatio < 0.9 ? 1.0 : 0.5;

    return (bestCircle.Quality * 0.4 + centerConfidence * 0.3 + radiusConfidence * 0.3);
  }
}