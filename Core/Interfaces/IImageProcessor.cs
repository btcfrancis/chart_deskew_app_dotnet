using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for image processing operations
/// </summary>
public interface IImageProcessor
{
  /// <summary>
  /// Analyzes a chart image and returns analysis results
  /// </summary>
  Task<ChartAnalysisResult> AnalyzeChartAsync(byte[] imageData);

  /// <summary>
  /// Deskews an image based on analysis results
  /// </summary>
  Task<byte[]> DeskewImageAsync(byte[] imageData, ChartAnalysisResult analysis);

  /// <summary>
  /// Processes an image from file path
  /// </summary>
  Task<byte[]> DeskewImageFromFileAsync(string filePath);
}