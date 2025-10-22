namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Represents the result of chart analysis including center, radii, and confidence metrics
/// </summary>
public class ChartAnalysisResult
{
  /// <summary>
  /// Center point of the detected chart
  /// </summary>
  public Point Center { get; set; }

  /// <summary>
  /// Radius of the inner ring (0% mark)
  /// </summary>
  public int InnerRadius { get; set; }

  /// <summary>
  /// Radius of the outer ring (100% mark)
  /// </summary>
  public int OuterRadius { get; set; }

  /// <summary>
  /// Confidence score for the analysis (0.0 to 1.0)
  /// </summary>
  public double Confidence { get; set; }

  /// <summary>
  /// Indicates whether the analysis is valid
  /// </summary>
  public bool IsValid { get; set; }

  /// <summary>
  /// Additional metadata about the analysis
  /// </summary>
  public AnalysisMetadata Metadata { get; set; } = new();

  /// <summary>
  /// Calculates the radius difference between outer and inner rings
  /// </summary>
  public int RadiusDifference => OuterRadius - InnerRadius;

  /// <summary>
  /// Calculates the aspect ratio of the detected chart
  /// </summary>
  public double AspectRatio => OuterRadius > 0 ? (double)InnerRadius / OuterRadius : 0;

  public override string ToString()
  {
    return $"Center: {Center}, Inner: {InnerRadius}px, Outer: {OuterRadius}px, Confidence: {Confidence:P1}";
  }
}