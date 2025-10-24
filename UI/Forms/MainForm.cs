using ChartDeskewApp.Core.Interfaces;
using ChartDeskewApp.UI.Controls;
using ChartDeskewApp.UI.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace ChartDeskewApp.UI.Forms;

public partial class MainForm : Form
{
  private readonly IImageProcessor _imageProcessor;
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

    // Set splitter to 50/50 split
    this.Load += (s, e) =>
    {
      splitContainer.SplitterDistance = splitContainer.Width / 2;
    };

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

    btnProcessImage.Enabled = false;
    progressBar.Visible = true;

    var contours = await _imageProcessor.DetectRingsAsync(_currentImageData);
    var drawnImage = await _imageProcessor.DeskewImageAsync(_currentImageData);
    await LoadImageToViewer(correctedImageViewer, drawnImage);

    var contourImage = await _imageProcessor.DrawContoursAsync(_currentImageData, contours);
    await LoadImageToViewer(originalImageViewer, contourImage);

    btnProcessImage.Enabled = true;
    progressBar.Visible = false;
  }

  private void BtnSaveCorrected_Click(object? sender, EventArgs e)
  {
    if (_currentImageData == null) return;

    using var dialog = new SaveImageDialog();
    if (dialog.ShowDialog() == DialogResult.OK)
    {
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