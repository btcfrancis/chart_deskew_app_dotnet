using ChartDeskewApp.Core.Services;
using ChartDeskewApp.Core.Models;
using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.UI.Controls;
using ChartDeskewApp.UI.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace ChartDeskewApp.UI.Forms;

public partial class MainForm : Form
{
  private readonly IImageProcessor _imageProcessor;
  private ChartAnalysisResult? _currentAnalysis;
  private byte[]? _currentImageData;

  public MainForm(IServiceProvider serviceProvider)
  {
    InitializeComponent();
    _imageProcessor = serviceProvider.GetRequiredService<IImageProcessor>();
    InitializeUI();
  }

  private void InitializeUI()
  {
    // Set up event handlers
    btnOpenImage.Click += BtnOpenImage_Click;
    btnSaveCorrected.Click += BtnSaveCorrected_Click;
    btnProcessImage.Click += BtnProcessImage_Click;

    // Initialize status
    UpdateStatus("Ready to load image");
  }

  private async void BtnOpenImage_Click(object? sender, EventArgs e)
  {
    using var dialog = new OpenImageDialog();
    if (dialog.ShowDialog() == DialogResult.OK)
    {
      try
      {
        _currentImageData = await File.ReadAllBytesAsync(dialog.FileName);
        await LoadImageToViewer(originalImageViewer, _currentImageData);
        UpdateStatus($"Loaded: {Path.GetFileName(dialog.FileName)}");
        btnProcessImage.Enabled = true;
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error loading image: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }

  private async void BtnProcessImage_Click(object? sender, EventArgs e)
  {
    if (_currentImageData == null) return;

    try
    {
      btnProcessImage.Enabled = false;
      progressBar.Visible = true;
      UpdateStatus("Analyzing chart...");

      // Analyze the chart
      _currentAnalysis = await _imageProcessor.AnalyzeChartAsync(_currentImageData);

      if (_currentAnalysis.IsValid)
      {
        UpdateStatus($"Analysis complete. Confidence: {_currentAnalysis.Confidence:P1}");

        // Deskew the image
        UpdateStatus("Deskewing image...");
        var deskewedData = await _imageProcessor.DeskewImageAsync(_currentImageData, _currentAnalysis);

        // Display the result
        await LoadImageToViewer(correctedImageViewer, deskewedData);

        // Update status with analysis details
        statusPanel.UpdateAnalysis(_currentAnalysis);
        btnSaveCorrected.Enabled = true;
      }
      else
      {
        UpdateStatus("Chart analysis failed. Please try a different image.");
        MessageBox.Show("Could not detect a valid circular chart in the image.",
            "Analysis Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Error processing image: {ex.Message}", "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
      UpdateStatus("Processing failed");
    }
    finally
    {
      btnProcessImage.Enabled = true;
      progressBar.Visible = false;
    }
  }

  private async void BtnSaveCorrected_Click(object? sender, EventArgs e)
  {
    if (_currentImageData == null || _currentAnalysis == null) return;

    using var dialog = new SaveImageDialog();
    if (dialog.ShowDialog() == DialogResult.OK)
    {
      try
      {
        var deskewedData = await _imageProcessor.DeskewImageAsync(_currentImageData, _currentAnalysis);
        await File.WriteAllBytesAsync(dialog.FileName, deskewedData);
        UpdateStatus($"Saved: {Path.GetFileName(dialog.FileName)}");
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error saving image: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }

  private async Task LoadImageToViewer(ImageViewer viewer, byte[] imageData)
  {
    using var stream = new MemoryStream(imageData);
    var image = Image.FromStream(stream);
    await viewer.LoadImageAsync(image);
  }

  private void UpdateStatus(string message)
  {
    statusPanel.UpdateStatus(message);
  }
}