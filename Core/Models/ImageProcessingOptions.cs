namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Options for image processing operations
/// </summary>
public class ImageProcessingOptions
{
  /// <summary>
  /// Minimum circle radius for detection
  /// </summary>
  public int MinRadius { get; set; } = 10;

  /// <summary>
  /// Maximum circle radius for detection
  /// </summary>
  public int MaxRadius { get; set; } = 1000;

  /// <summary>
  /// Minimum confidence threshold for circle detection
  /// </summary>
  public double MinConfidence { get; set; } = 0.5;

  /// <summary>
  /// Edge detection threshold
  /// </summary>
  public double EdgeThreshold { get; set; } = 50.0;

  /// <summary>
  /// Gaussian blur radius for preprocessing
  /// </summary>
  public double BlurRadius { get; set; } = 1.0;

  /// <summary>
  /// Whether to apply histogram equalization
  /// </summary>
  public bool ApplyHistogramEqualization { get; set; } = false;

  /// <summary>
  /// Whether to apply noise reduction
  /// </summary>
  public bool ApplyNoiseReduction { get; set; } = true;

  /// <summary>
  /// Maximum processing time in milliseconds
  /// </summary>
  public int MaxProcessingTimeMs { get; set; } = 30000;

  /// <summary>
  /// Creates default processing options
  /// </summary>
  public static ImageProcessingOptions Default => new();

  /// <summary>
  /// Creates high-quality processing options
  /// </summary>
  public static ImageProcessingOptions HighQuality => new()
  {
    MinConfidence = 0.7,
    EdgeThreshold = 30.0,
    BlurRadius = 0.5,
    ApplyHistogramEqualization = true,
    ApplyNoiseReduction = true,
    MaxProcessingTimeMs = 60000
  };

  public override string ToString()
  {
    return $"Radius: {MinRadius}-{MaxRadius}, Confidence: {MinConfidence:P1}, " +
           $"Threshold: {EdgeThreshold}";
  }
}