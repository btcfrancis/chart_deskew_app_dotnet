namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Quality metrics for chart analysis
/// </summary>
public class QualityMetrics
{
  /// <summary>
  /// Overall quality score (0.0 to 1.0)
  /// </summary>
  public double OverallScore { get; set; }

  /// <summary>
  /// Circularity score (how close to perfect circle)
  /// </summary>
  public double CircularityScore { get; set; }

  /// <summary>
  /// Edge clarity score
  /// </summary>
  public double EdgeClarityScore { get; set; }

  /// <summary>
  /// Contrast score
  /// </summary>
  public double ContrastScore { get; set; }

  /// <summary>
  /// Symmetry score
  /// </summary>
  public double SymmetryScore { get; set; }

  /// <summary>
  /// Calculates the weighted average of all quality scores
  /// </summary>
  public void CalculateOverallScore()
  {
    OverallScore = (CircularityScore * 0.3 + EdgeClarityScore * 0.3 +
                   ContrastScore * 0.2 + SymmetryScore * 0.2);
  }

  public override string ToString()
  {
    return $"Overall: {OverallScore:P1}, Circularity: {CircularityScore:P1}, " +
           $"Edges: {EdgeClarityScore:P1}, Contrast: {ContrastScore:P1}";
  }
}