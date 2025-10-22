using System.Drawing.Drawing2D;

namespace ChartDeskewApp.Core.Models;

/// <summary>
/// Parameters for image deskewing operations
/// </summary>
public class DeskewParameters
{
  /// <summary>
  /// Center point for the transformation
  /// </summary>
  public Point Center { get; set; }

  /// <summary>
  /// Angle to correct in radians
  /// </summary>
  public double SkewAngle { get; set; }

  /// <summary>
  /// Scale factor for the transformation
  /// </summary>
  public double Scale { get; set; } = 1.0;

  /// <summary>
  /// Translation offset in X direction
  /// </summary>
  public double TranslationX { get; set; } = 0.0;

  /// <summary>
  /// Translation offset in Y direction
  /// </summary>
  public double TranslationY { get; set; } = 0.0;

  /// <summary>
  /// Interpolation method to use: "Bilinear", "Bicubic", etc.
  /// </summary>
  public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.Bicubic;

  /// <summary>
  /// Quality of the transformation.
  /// </summary>
  public CompositingQuality CompositingQuality { get; set; } = CompositingQuality.HighQuality;

  /// <summary>
  /// Smoothing mode for the transformation.
  /// </summary>
  public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.HighQuality;

  /// <summary>
  /// Creates a copy of these parameters
  /// </summary>
  public DeskewParameters Clone()
  {
    return new DeskewParameters
    {
      Center = Center,
      SkewAngle = SkewAngle,
      Scale = Scale,
      TranslationX = TranslationX,
      TranslationY = TranslationY,
      InterpolationMode = InterpolationMode,
      CompositingQuality = CompositingQuality,
      SmoothingMode = SmoothingMode
    };
  }

  public override string ToString()
  {
    return $"Center: {Center}, Angle: {SkewAngle:F3}rad, Scale: {Scale:F2}";
  }
}