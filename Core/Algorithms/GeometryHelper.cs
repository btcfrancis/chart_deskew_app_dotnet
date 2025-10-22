using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.Core.Models;
using System.Drawing.Drawing2D;

namespace ChartDeskewApp.Core.Algorithms;

/// <summary>
/// Implementation of geometric calculation operations
/// </summary>
public class GeometryHelper : IGeometryHelper
{
  public double CalculateDistance(Point point1, Point point2)
  {
    return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
  }

  public double CalculateDistance(double x1, double y1, double x2, double y2)
  {
    return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
  }

  public Point? CalculateCircleCenter(Point point1, Point point2, Point point3)
  {
    if (AreCollinear(point1, point2, point3))
      return null;

    var ax = point1.X; var ay = point1.Y;
    var bx = point2.X; var by = point2.Y;
    var cx = point3.X; var cy = point3.Y;

    var d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
    if (Math.Abs(d) < 1e-10) return null;

    var ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
    var uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;

    return new Point((int)Math.Round((double)ux), (int)Math.Round((double)uy));
  }

  public double CalculateRadius(Point center, Point pointOnCircle)
  {
    return CalculateDistance(center, pointOnCircle);
  }

  public double CalculateAngle(Vector2D vector1, Vector2D vector2)
  {
    var dot = vector1.X * vector2.X + vector1.Y * vector2.Y;
    var mag1 = vector1.Magnitude;
    var mag2 = vector2.Magnitude;

    if (mag1 == 0 || mag2 == 0) return 0;

    var cos = dot / (mag1 * mag2);
    cos = Math.Max(-1, Math.Min(1, cos)); // Clamp to [-1, 1]
    return Math.Acos(cos);
  }

  public double CalculateAngle(Point center, Point point1, Point point2)
  {
    var v1 = new Vector2D(point1.X - center.X, point1.Y - center.Y);
    var v2 = new Vector2D(point2.X - center.X, point2.Y - center.Y);
    return CalculateAngle(v1, v2);
  }

  public Point RotatePoint(Point point, Point center, double angleRadians)
  {
    var cos = Math.Cos(angleRadians);
    var sin = Math.Sin(angleRadians);

    var dx = point.X - center.X;
    var dy = point.Y - center.Y;

    var newX = dx * cos - dy * sin + center.X;
    var newY = dx * sin + dy * cos + center.Y;

    return new Point((int)Math.Round(newX), (int)Math.Round(newY));
  }

  public double CalculateSkewAngle(List<Point> points)
  {
    if (points.Count < 3) return 0;

    var angles = new List<double>();
    for (int i = 0; i < points.Count - 2; i++)
    {
      var angle = CalculateAngle(points[i], points[i + 1], points[i + 2]);
      angles.Add(angle);
    }

    return angles.Average();
  }

  public double CalculateAspectRatio(List<Point> points)
  {
    if (points.Count < 2) return 1.0;

    var bounds = CalculateBoundingRectangle(points);
    return (double)bounds.Width / bounds.Height;
  }

  public bool AreCollinear(Point point1, Point point2, Point point3, double tolerance = 1e-6)
  {
    var area = Math.Abs((point2.X - point1.X) * (point3.Y - point1.Y) - (point3.X - point1.X) * (point2.Y - point1.Y)) / 2.0;
    return area < tolerance;
  }

  public double CalculatePerpendicularDistance(Point point, Point lineStart, Point lineEnd)
  {
    var A = point.X - lineStart.X;
    var B = point.Y - lineStart.Y;
    var C = lineEnd.X - lineStart.X;
    var D = lineEnd.Y - lineStart.Y;

    var dot = A * C + B * D;
    var lenSq = C * C + D * D;

    if (lenSq == 0) return CalculateDistance(point, lineStart);

    var param = dot / lenSq;
    var xx = lineStart.X + param * C;
    var yy = lineStart.Y + param * D;

    return CalculateDistance(point, new Point((int)xx, (int)yy));
  }

  public Point CalculateCentroid(IEnumerable<Point> points)
  {
    var pointList = points.ToList();
    if (pointList.Count == 0) return Point.Empty;

    var sumX = pointList.Sum(p => p.X);
    var sumY = pointList.Sum(p => p.Y);

    return new Point(sumX / pointList.Count, sumY / pointList.Count);
  }

  public Rectangle CalculateBoundingRectangle(IEnumerable<Point> points)
  {
    var pointList = points.ToList();
    if (pointList.Count == 0) return Rectangle.Empty;

    var minX = pointList.Min(p => p.X);
    var maxX = pointList.Max(p => p.X);
    var minY = pointList.Min(p => p.Y);
    var maxY = pointList.Max(p => p.Y);

    return new Rectangle(minX, minY, maxX - minX, maxY - minY);
  }

  public double CalculateCircularity(List<Point> points)
  {
    if (points.Count < 3) return 0;

    var centroid = CalculateCentroid(points);
    var distances = points.Select(p => CalculateDistance(centroid, p)).ToList();
    var avgDistance = distances.Average();
    var variance = distances.Select(d => Math.Pow(d - avgDistance, 2)).Average();
    var stdDev = Math.Sqrt(variance);

    return avgDistance > 0 ? 1.0 - (stdDev / avgDistance) : 0;
  }

  public double CalculateRadialStandardDeviation(Point center, IEnumerable<Point> points)
  {
    var distances = points.Select(p => CalculateDistance(center, p)).ToList();
    var avg = distances.Average();
    var variance = distances.Select(d => Math.Pow(d - avg, 2)).Average();
    return Math.Sqrt(variance);
  }

  public Matrix CalculateDeskewMatrix(double skewAngle, Point center)
  {
    var matrix = new Matrix();
    matrix.Translate(-center.X, -center.Y);
    matrix.Rotate((float)(-skewAngle * 180 / Math.PI));
    matrix.Translate(center.X, center.Y);
    return matrix;
  }

  public Point TransformPoint(Point point, Matrix matrix)
  {
    var points = new PointF[] { point };
    matrix.TransformPoints(points);
    return new Point((int)Math.Round(points[0].X), (int)Math.Round(points[0].Y));
  }
}