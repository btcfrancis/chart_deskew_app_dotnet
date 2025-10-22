namespace ChartDeskewApp.UI.Dialogs;

/// <summary>
/// Custom OpenFileDialog for image files with proper filtering
/// </summary>
public class OpenImageDialog : IDisposable
{
  private readonly OpenFileDialog _dialog;

  public OpenImageDialog()
  {
    _dialog = new OpenFileDialog
    {
      Title = "Select Chart Image",
      Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.tiff;*.gif|" +
                 "JPEG Files|*.jpg;*.jpeg|" +
                 "PNG Files|*.png|" +
                 "Bitmap Files|*.bmp|" +
                 "TIFF Files|*.tiff|" +
                 "GIF Files|*.gif|" +
                 "All Files|*.*",
      FilterIndex = 1,
      Multiselect = false,
      CheckFileExists = true,
      CheckPathExists = true
    };
  }

  public string FileName => _dialog.FileName;
  public DialogResult ShowDialog() => _dialog.ShowDialog();

  public void Dispose()
  {
    _dialog?.Dispose();
  }
}