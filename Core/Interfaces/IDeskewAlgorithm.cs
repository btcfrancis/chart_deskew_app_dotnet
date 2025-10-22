using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for image deskewing operations
/// </summary>
public interface IDeskewAlgorithm
{
  /// <summary>
  /// Deskews an image based on analysis parameters
  /// </summary>
  Task<Bitmap> DeskewImageAsync(Bitmap source, ChartAnalysisResult analysis);

  /// <summary>
  /// Calculates confidence score for deskewing result
  /// </summary>
  Task<double> CalculateConfidenceAsync(Bitmap original, Bitmap deskewed);
}