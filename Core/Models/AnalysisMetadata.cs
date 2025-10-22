namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Additional metadata about chart analysis
/// </summary>
public class AnalysisMetadata
{
  /// <summary>
  /// Processing time in milliseconds
  /// </summary>
  public long ProcessingTimeMs { get; set; }

  /// <summary>
  /// Number of circles detected during analysis
  /// </summary>
  public int CirclesDetected { get; set; }

  /// <summary>
  /// Number of edge points found
  /// </summary>
  public int EdgePointsFound { get; set; }

  /// <summary>
  /// Image dimensions
  /// </summary>
  public Size ImageSize { get; set; }

  /// <summary>
  /// Algorithm version used for analysis
  /// </summary>
  public string AlgorithmVersion { get; set; } = "1.0.0";

  /// <summary>
  /// Timestamp when analysis was performed
  /// </summary>
  public DateTime AnalysisTimestamp { get; set; } = DateTime.Now;

  /// <summary>
  /// Additional notes about the analysis
  /// </summary>
  public string Notes { get; set; } = string.Empty;

  /// <summary>
  /// Quality metrics
  /// </summary>
  public QualityMetrics Quality { get; set; } = new();

  public override string ToString()
  {
    return $"Processed in {ProcessingTimeMs}ms, {CirclesDetected} circles, {EdgePointsFound} edges";
  }
}