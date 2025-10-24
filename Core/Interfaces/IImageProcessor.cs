using OpenCvSharp;

namespace ChartDeskewApp.Core.Interfaces;

/// <summary>
/// Interface for image processing operations
/// </summary>
public interface IImageProcessor
{
  Task<List<OpenCvSharp.Point[]>> DetectRingsAsync(byte[] imageData);
  Task<byte[]> DeskewImageAsync(byte[] imageData);
  Task<byte[]> DrawContoursAsync(byte[] imageData, List<OpenCvSharp.Point[]> contours);
}