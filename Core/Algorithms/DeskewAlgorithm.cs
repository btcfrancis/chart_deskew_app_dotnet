using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.Core.Models;
using System.Drawing.Drawing2D;

namespace ChartDeskewApp.Core.Algorithms;

/// <summary>
/// Implementation of image deskewing operations
/// </summary>
public class DeskewAlgorithm : IDeskewAlgorithm
{
  private readonly IGeometryHelper _geometryHelper;

  public DeskewAlgorithm(IGeometryHelper geometryHelper)
  {
    _geometryHelper = geometryHelper ?? throw new ArgumentNullException(nameof(geometryHelper));
  }

  public async Task<Bitmap> DeskewImageAsync(Bitmap source, ChartAnalysisResult analysis)
  {
    return await Task.Run(() =>
    {
      if (!analysis.IsValid)
        throw new InvalidOperationException("Cannot deskew image with invalid analysis result");

      var parameters = new DeskewParameters
      {
        Center = analysis.Center,
        SkewAngle = CalculateSkewAngle(analysis),
        Scale = 1.0,
        InterpolationMode = InterpolationMode.HighQualityBicubic,
        CompositingQuality = CompositingQuality.HighQuality,
        SmoothingMode = SmoothingMode.HighQuality
      };

      return DeskewImageWithParameters(source, parameters);
    });
  }

  public async Task<double> CalculateConfidenceAsync(Bitmap original, Bitmap deskewed)
  {
    return await Task.Run(() =>
    {
      // Calculate confidence based on circularity improvement
      var originalCircularity = CalculateImageCircularity(original);
      var deskewedCircularity = CalculateImageCircularity(deskewed);

      var improvement = deskewedCircularity - originalCircularity;
      return Math.Max(0, Math.Min(1, improvement + 0.5)); // Normalize to 0-1 range
    });
  }

  private double CalculateSkewAngle(ChartAnalysisResult analysis)
  {
    // For now, return 0 as we need to implement actual skew detection
    // This would typically involve analyzing the ellipse properties
    return 0.0;
  }

  private Bitmap DeskewImageWithParameters(Bitmap source, DeskewParameters parameters)
  {
    var result = new Bitmap(source.Width, source.Height);

    using (var g = Graphics.FromImage(result))
    {
      g.InterpolationMode = parameters.InterpolationMode;
      g.CompositingQuality = parameters.CompositingQuality;
      g.SmoothingMode = parameters.SmoothingMode;

      var matrix = _geometryHelper.CalculateDeskewMatrix(parameters.SkewAngle, parameters.Center);
      g.Transform = matrix;

      g.DrawImage(source, 0, 0, source.Width, source.Height);
    }

    return result;
  }

  private double CalculateImageCircularity(Bitmap image)
  {
    // Simplified circularity calculation
    // In a real implementation, this would analyze the image for circular shapes
    var center = new Point(image.Width / 2, image.Height / 2);
    var radius = Math.Min(image.Width, image.Height) / 2;

    // Sample points around the center
    var points = new List<Point>();
    for (int i = 0; i < 360; i += 10)
    {
      var angle = i * Math.PI / 180;
      var x = (int)(center.X + radius * Math.Cos(angle));
      var y = (int)(center.Y + radius * Math.Sin(angle));
      points.Add(new Point(x, y));
    }

    return _geometryHelper.CalculateCircularity(points);
  }
}