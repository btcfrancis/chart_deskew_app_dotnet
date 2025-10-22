namespace ChartDeskewApp.UI.Controls;

/// <summary>
/// Custom PictureBox control with enhanced image handling capabilities
/// </summary>
public partial class ImageViewer : PictureBox
{
  private Image? _currentImage;
  private bool _isLoading;

  public ImageViewer()
  {
    InitializeComponent();
    this.SizeMode = PictureBoxSizeMode.CenterImage;
    this.BackColor = Color.White;
    this.BorderStyle = BorderStyle.FixedSingle;
  }

  /// <summary>
  /// Loads an image asynchronously with proper disposal of previous image
  /// </summary>
  public async Task LoadImageAsync(Image image)
  {
    if (_isLoading) return;

    _isLoading = true;

    try
    {
      // Dispose of previous image
      _currentImage?.Dispose();

      // Load new image on background thread
      await Task.Run(() =>
      {
        _currentImage = new Bitmap(image);
      });

      // Update UI on main thread
      this.Invoke(() =>
      {
        this.Image = _currentImage;
        this.Refresh();
      });
    }
    finally
    {
      _isLoading = false;
    }
  }

  /// <summary>
  /// Clears the current image
  /// </summary>
  public void ClearImage()
  {
    _currentImage?.Dispose();
    _currentImage = null;
    this.Image = null;
    this.Refresh();
  }

  /// <summary>
  /// Gets the current image as a byte array
  /// </summary>
  public async Task<byte[]?> GetImageAsBytesAsync()
  {
    if (_currentImage == null) return null;

    using var stream = new MemoryStream();
    await Task.Run(() => _currentImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png));
    return stream.ToArray();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      _currentImage?.Dispose();
    }
    base.Dispose(disposing);
  }
}