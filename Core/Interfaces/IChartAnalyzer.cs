using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for chart analysis operations
/// </summary>
public interface IChartAnalyzer
{
  /// <summary>
  /// Analyzes a chart to find center and rings
  /// </summary>
  Task<ChartAnalysisResult> AnalyzeChartAsync(Bitmap image);

  /// <summary>
  /// Detects the center of the chart
  /// </summary>
  Task<Point> DetectCenterAsync(Bitmap image);

  /// <summary>
  /// Detects inner and outer rings
  /// </summary>
  Task<(int innerRadius, int outerRadius)> DetectRingsAsync(Bitmap image, Point center);
}