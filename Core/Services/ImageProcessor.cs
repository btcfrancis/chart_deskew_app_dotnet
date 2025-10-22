using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.Core.Models;

namespace ChartDeskewApp.Core.Services;

/// <summary>
/// Main image processing service implementation
/// </summary>
public class ImageProcessor : IImageProcessor
{
  private readonly IChartAnalyzer _chartAnalyzer;
  private readonly IDeskewAlgorithm _deskewAlgorithm;

  public ImageProcessor(IChartAnalyzer chartAnalyzer, IDeskewAlgorithm deskewAlgorithm)
  {
    _chartAnalyzer = chartAnalyzer ?? throw new ArgumentNullException(nameof(chartAnalyzer));
    _deskewAlgorithm = deskewAlgorithm ?? throw new ArgumentNullException(nameof(deskewAlgorithm));
  }

  public async Task<ChartAnalysisResult> AnalyzeChartAsync(byte[] imageData)
  {
    using var stream = new MemoryStream(imageData);
    using var image = new Bitmap(stream);

    return await _chartAnalyzer.AnalyzeChartAsync(image);
  }

  public async Task<byte[]> DeskewImageAsync(byte[] imageData, ChartAnalysisResult analysis)
  {
    using var stream = new MemoryStream(imageData);
    using var originalImage = new Bitmap(stream);
    using var deskewedImage = await _deskewAlgorithm.DeskewImageAsync(originalImage, analysis);

    using var outputStream = new MemoryStream();
    deskewedImage.Save(outputStream, System.Drawing.Imaging.ImageFormat.Png);
    return outputStream.ToArray();
  }

  public async Task<byte[]> DeskewImageFromFileAsync(string filePath)
  {
    var imageData = await File.ReadAllBytesAsync(filePath);
    var analysis = await AnalyzeChartAsync(imageData);
    return await DeskewImageAsync(imageData, analysis);
  }
}