using System.Drawing.Drawing2D;
using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for geometric calculation operations
/// </summary>
public interface IGeometryHelper
{
  /// <summary>
  /// Calculates the distance between two points
  /// </summary>
  /// <param name="point1">First point</param>
  /// <param name="point2">Second point</param>
  /// <returns>Distance between the points</returns>
  double CalculateDistance(Point point1, Point point2);

  /// <summary>
  /// Calculates the distance between two points using double precision
  /// </summary>
  /// <param name="x1">X coordinate of first point</param>
  /// <param name="y1">Y coordinate of first point</param>
  /// <param name="x2">X coordinate of second point</param>
  /// <param name="y2">Y coordinate of second point</param>
  /// <returns>Distance between the points</returns>
  double CalculateDistance(double x1, double y1, double x2, double y2);

  /// <summary>
  /// Calculates the center point of a circle from three points on its circumference
  /// </summary>
  /// <param name="point1">First point on circle</param>
  /// <param name="point2">Second point on circle</param>
  /// <param name="point3">Third point on circle</param>
  /// <returns>Center point of the circle, or null if points are collinear</returns>
  Point? CalculateCircleCenter(Point point1, Point point2, Point point3);

  /// <summary>
  /// Calculates the radius of a circle given its center and a point on the circumference
  /// </summary>
  /// <param name="center">Center of the circle</param>
  /// <param name="pointOnCircle">Point on the circle's circumference</param>
  /// <returns>Radius of the circle</returns>
  double CalculateRadius(Point center, Point pointOnCircle);

  /// <summary>
  /// Calculates the angle between two vectors in radians
  /// </summary>
  /// <param name="vector1">First vector</param>
  /// <param name="vector2">Second vector</param>
  /// <returns>Angle in radians</returns>
  double CalculateAngle(Vector2D vector1, Vector2D vector2);

  /// <summary>
  /// Calculates the angle between two points relative to a center point
  /// </summary>
  /// <param name="center">Center point</param>
  /// <param name="point1">First point</param>
  /// <param name="point2">Second point</param>
  /// <returns>Angle in radians</returns>
  double CalculateAngle(Point center, Point point1, Point point2);

  /// <summary>
  /// Rotates a point around a center point by a specified angle
  /// </summary>
  /// <param name="point">Point to rotate</param>
  /// <param name="center">Center of rotation</param>
  /// <param name="angleRadians">Angle in radians</param>
  /// <returns>Rotated point</returns>
  Point RotatePoint(Point point, Point center, double angleRadians);

  /// <summary>
  /// Calculates the skew angle of an ellipse or oval shape
  /// </summary>
  /// <param name="points">Points defining the shape</param>
  /// <returns>Skew angle in radians</returns>
  double CalculateSkewAngle(List<Point> points);

  /// <summary>
  /// Calculates the aspect ratio of a shape defined by points
  /// </summary>
  /// <param name="points">Points defining the shape</param>
  /// <returns>Aspect ratio (width/height)</returns>
  double CalculateAspectRatio(List<Point> points);

  /// <summary>
  /// Determines if three points are collinear (lie on the same line)
  /// </summary>
  /// <param name="point1">First point</param>
  /// <param name="point2">Second point</param>
  /// <param name="point3">Third point</param>
  /// <param name="tolerance">Tolerance for collinearity check</param>
  /// <returns>True if points are collinear within tolerance</returns>
  bool AreCollinear(Point point1, Point point2, Point point3, double tolerance = 1e-6);

  /// <summary>
  /// Calculates the perpendicular distance from a point to a line
  /// </summary>
  /// <param name="point">Point to measure distance from</param>
  /// <param name="lineStart">Start point of the line</param>
  /// <param name="lineEnd">End point of the line</param>
  /// <returns>Perpendicular distance</returns>
  double CalculatePerpendicularDistance(Point point, Point lineStart, Point lineEnd);

  /// <summary>
  /// Calculates the centroid (center of mass) of a set of points
  /// </summary>
  /// <param name="points">Collection of points</param>
  /// <returns>Centroid point</returns>
  Point CalculateCentroid(IEnumerable<Point> points);

  /// <summary>
  /// Calculates the bounding rectangle of a set of points
  /// </summary>
  /// <param name="points">Collection of points</param>
  /// <returns>Bounding rectangle</returns>
  Rectangle CalculateBoundingRectangle(IEnumerable<Point> points);

  /// <summary>
  /// Calculates the circularity of a shape (how close it is to a perfect circle)
  /// </summary>
  /// <param name="points">Points defining the shape</param>
  /// <returns>Circularity value (1.0 = perfect circle, lower values = less circular)</returns>
  double CalculateCircularity(List<Point> points);

  /// <summary>
  /// Calculates the standard deviation of distances from center to points
  /// </summary>
  /// <param name="center">Center point</param>
  /// <param name="points">Points to measure</param>
  /// <returns>Standard deviation of distances</returns>
  double CalculateRadialStandardDeviation(Point center, IEnumerable<Point> points);

  /// <summary>
  /// Calculates the transformation matrix for deskewing
  /// </summary>
  /// <param name="skewAngle">Angle to correct</param>
  /// <param name="center">Center of transformation</param>
  /// <returns>Transformation matrix</returns>
  Matrix CalculateDeskewMatrix(double skewAngle, Point center);

  /// <summary>
  /// Applies a transformation matrix to a point
  /// </summary>
  /// <param name="point">Point to transform</param>
  /// <param name="matrix">Transformation matrix</param>
  /// <returns>Transformed point</returns>
  Point TransformPoint(Point point, Matrix matrix);
}

/// <summary>
/// Simple 2D vector structure for geometric calculations
/// </summary>
public struct Vector2D
{
  public double X { get; set; }
  public double Y { get; set; }

  public Vector2D(double x, double y)
  {
    X = x;
    Y = y;
  }

  public double Magnitude => Math.Sqrt(X * X + Y * Y);

  public Vector2D Normalize()
  {
    var magnitude = Magnitude;
    if (magnitude == 0) return new Vector2D(0, 0);
    return new Vector2D(X / magnitude, Y / magnitude);
  }

  public static Vector2D operator -(Vector2D a, Vector2D b)
  {
    return new Vector2D(a.X - b.X, a.Y - b.Y);
  }

  public static Vector2D operator +(Vector2D a, Vector2D b)
  {
    return new Vector2D(a.X + b.X, a.Y + b.Y);
  }

  public static Vector2D operator *(Vector2D vector, double scalar)
  {
    return new Vector2D(vector.X * scalar, vector.Y * scalar);
  }
}