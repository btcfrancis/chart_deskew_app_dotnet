using OpenCvSharp;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for image processing operations
/// </summary>
public interface IImageProcessor
{
  Task<List<OpenCvSharp.Point[]>> DetectContoursAsync(byte[] imageData);
  Task<byte[]> DrawContoursAsync(byte[] imageData);
  // Task<byte[]> DetectCentericRingAsync(byte[] imageData);
}