using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for circle detection operations
/// </summary>
public interface ICircleDetector
{
  /// <summary>
  /// Detects circles in an image
  /// </summary>
  Task<List<CircleDetection>> DetectCirclesAsync(Bitmap image);

  /// <summary>
  /// Finds the best circle from a list of detections
  /// </summary>
  Task<CircleDetection?> FindBestCircleAsync(List<CircleDetection> circles);
}