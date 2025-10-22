namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Represents a detected circle with its properties
/// </summary>
public class CircleDetection
{
  /// <summary>
  /// Center point of the detected circle
  /// </summary>
  public Point Center { get; set; }

  /// <summary>
  /// Radius of the detected circle
  /// </summary>
  public int Radius { get; set; }

  /// <summary>
  /// Confidence score for this detection (0.0 to 1.0)
  /// </summary>
  public double Confidence { get; set; }

  /// <summary>
  /// Quality score based on edge strength and circularity
  /// </summary>
  public double Quality { get; set; }

  /// <summary>
  /// Number of edge points that contributed to this detection
  /// </summary>
  public int EdgePointCount { get; set; }

  /// <summary>
  /// Standard deviation of distances from center to edge points
  /// </summary>
  public double RadialStandardDeviation { get; set; }

  /// <summary>
  /// Calculates the area of the circle
  /// </summary>
  public double Area => Math.PI * Radius * Radius;

  /// <summary>
  /// Calculates the circumference of the circle
  /// </summary>
  public double Circumference => 2 * Math.PI * Radius;

  public override string ToString()
  {
    return $"Center: {Center}, Radius: {Radius}px, Confidence: {Confidence:P1}";
  }

  public override bool Equals(object? obj)
  {
    if (obj is CircleDetection other)
    {
      return Center == other.Center && Radius == other.Radius;
    }
    return false;
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(Center, Radius);
  }
}