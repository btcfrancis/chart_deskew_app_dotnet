namespace ChartDeskewApp.UI.Dialogs;

/// <summary>
/// Custom SaveFileDialog for saving corrected images
/// </summary>
public class SaveImageDialog : IDisposable
{
  private readonly SaveFileDialog _dialog;

  public SaveImageDialog()
  {
    _dialog = new SaveFileDialog
    {
      Title = "Save Corrected Image",
      Filter = "PNG Files|*.png|" +
                 "JPEG Files|*.jpg;*.jpeg|" +
                 "Bitmap Files|*.bmp|" +
                 "TIFF Files|*.tiff|" +
                 "All Files|*.*",
      FilterIndex = 1,
      DefaultExt = "png",
      AddExtension = true,
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