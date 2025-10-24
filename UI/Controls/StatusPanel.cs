using System.Drawing;

namespace ChartDeskewApp.UI.Controls;

/// <summary>
/// Custom control for displaying status information and analysis results
/// </summary>
public partial class StatusPanel : UserControl
{
  private string _statusMessage = "Ready";

  public StatusPanel()
  {
    InitializeComponent();
    SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
  }

  /// <summary>
  /// Updates the status message
  /// </summary>
  public void UpdateStatus(string message)
  {
    _statusMessage = message;
    Invalidate();
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);

    var g = e.Graphics;
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

    // Background
    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), ClientRectangle);

    // Border
    g.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200)), 0, 0, Width - 1, Height - 1);

    var font = new Font("Segoe UI", 9);
    var boldFont = new Font("Segoe UI", 9, FontStyle.Bold);
    var brush = new SolidBrush(Color.Black);

    int y = 10;

    // Status message
    g.DrawString("Status:", boldFont, brush, 10, y);
    g.DrawString(_statusMessage, font, brush, 70, y);
    y += 20;


    font.Dispose();
    boldFont.Dispose();
    brush.Dispose();
  }
}