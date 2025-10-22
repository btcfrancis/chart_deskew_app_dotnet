namespace ChartDeskewApp.UI.Forms;

partial class MainForm
{
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.toolStrip = new System.Windows.Forms.ToolStrip();
    this.btnOpenImage = new System.Windows.Forms.ToolStripButton();
    this.btnProcessImage = new System.Windows.Forms.ToolStripButton();
    this.btnSaveCorrected = new System.Windows.Forms.ToolStripButton();
    this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
    this.splitContainer = new System.Windows.Forms.SplitContainer();
    this.lblOriginal = new System.Windows.Forms.Label();
    this.originalImageViewer = new ChartDeskewApp.UI.Controls.ImageViewer();
    this.lblCorrected = new System.Windows.Forms.Label();
    this.correctedImageViewer = new ChartDeskewApp.UI.Controls.ImageViewer();
    this.statusPanel = new ChartDeskewApp.UI.Controls.StatusPanel();
    this.progressBar = new System.Windows.Forms.ProgressBar();

    this.toolStrip.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
    this.splitContainer.Panel1.SuspendLayout();
    this.splitContainer.Panel2.SuspendLayout();
    this.splitContainer.SuspendLayout();
    this.SuspendLayout();

    // 
    // toolStrip
    // 
    this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenImage,
            this.btnProcessImage,
            this.btnSaveCorrected,
            this.toolStripSeparator1});
    this.toolStrip.Location = new System.Drawing.Point(0, 0);
    this.toolStrip.Name = "toolStrip";
    this.toolStrip.Size = new System.Drawing.Size(1200, 25);
    this.toolStrip.TabIndex = 0;
    this.toolStrip.Text = "toolStrip1";

    // 
    // btnOpenImage
    // 
    this.btnOpenImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
    this.btnOpenImage.Name = "btnOpenImage";
    this.btnOpenImage.Size = new System.Drawing.Size(75, 22);
    this.btnOpenImage.Text = "Open Image";

    // 
    // btnProcessImage
    // 
    this.btnProcessImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
    this.btnProcessImage.Enabled = false;
    this.btnProcessImage.Name = "btnProcessImage";
    this.btnProcessImage.Size = new System.Drawing.Size(85, 22);
    this.btnProcessImage.Text = "Process Image";

    // 
    // btnSaveCorrected
    // 
    this.btnSaveCorrected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
    this.btnSaveCorrected.Enabled = false;
    this.btnSaveCorrected.Name = "btnSaveCorrected";
    this.btnSaveCorrected.Size = new System.Drawing.Size(95, 22);
    this.btnSaveCorrected.Text = "Save Corrected";

    // 
    // toolStripSeparator1
    // 
    this.toolStripSeparator1.Name = "toolStripSeparator1";
    this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);

    // 
    // splitContainer
    // 
    this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
    this.splitContainer.Name = "splitContainer";
    this.splitContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
    this.splitContainer.SplitterDistance = 50;
    this.splitContainer.SplitterIncrement = 1;
    this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.None;
    this.splitContainer.Panel1MinSize = 100;
    this.splitContainer.Panel2MinSize = 100;
    this.splitContainer.SplitterWidth = 1;
    this.splitContainer.TabIndex = 1;

    // 
    // splitContainer.Panel1
    // 
    this.splitContainer.Panel1.Controls.Add(this.lblOriginal);
    this.splitContainer.Panel1.Controls.Add(this.originalImageViewer);

    // 
    // splitContainer.Panel2
    // 
    this.splitContainer.Panel2.Controls.Add(this.lblCorrected);
    this.splitContainer.Panel2.Controls.Add(this.correctedImageViewer);

    // 
    // lblOriginal
    // 
    this.lblOriginal.AutoSize = true;
    this.lblOriginal.BackColor = System.Drawing.Color.Transparent;
    this.lblOriginal.Location = new System.Drawing.Point(10, 10);
    this.lblOriginal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
    this.lblOriginal.Name = "lblOriginal";
    this.lblOriginal.TabIndex = 0;
    this.lblOriginal.Text = "Original Image";

    // 
    // originalImageViewer
    // 
    this.originalImageViewer.BackColor = System.Drawing.Color.White;
    this.originalImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    this.originalImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
    this.originalImageViewer.Name = "originalImageViewer";
    this.originalImageViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
    this.originalImageViewer.TabIndex = 1;
    this.originalImageViewer.TabStop = false;

    // 
    // lblCorrected
    // 
    this.lblCorrected.AutoSize = true;
    this.lblCorrected.BackColor = System.Drawing.Color.Transparent;
    this.lblCorrected.Location = new System.Drawing.Point(10, 10);
    this.lblCorrected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
    this.lblCorrected.Name = "lblCorrected";
    this.lblCorrected.TabIndex = 0;
    this.lblCorrected.Text = "Output Image";

    // 
    // correctedImageViewer
    // 
    this.correctedImageViewer.BackColor = System.Drawing.Color.White;
    this.correctedImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    this.correctedImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
    this.correctedImageViewer.Name = "correctedImageViewer";
    this.correctedImageViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
    this.correctedImageViewer.TabIndex = 1;
    this.correctedImageViewer.TabStop = false;

    // 
    // statusPanel
    // 
    this.statusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
    this.statusPanel.Name = "statusPanel";
    this.statusPanel.TabIndex = 2;

    // 
    // progressBar
    // 
    this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
    this.progressBar.Name = "progressBar";
    this.progressBar.Size = new System.Drawing.Size(200, 23);
    this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
    this.progressBar.TabIndex = 3;
    this.progressBar.Visible = false;

    // 
    // MainForm
    // 
    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    this.Controls.Add(this.progressBar);
    this.Controls.Add(this.splitContainer);
    this.Controls.Add(this.statusPanel);
    this.Controls.Add(this.toolStrip);
    this.Name = "MainForm";
    this.Text = "Chart Deskewer";
    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

    this.toolStrip.ResumeLayout(false);
    this.toolStrip.PerformLayout();
    this.splitContainer.Panel1.ResumeLayout(false);
    this.splitContainer.Panel1.PerformLayout();
    this.splitContainer.Panel2.ResumeLayout(false);
    this.splitContainer.Panel2.PerformLayout();
    ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
    this.splitContainer.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  #endregion

  private System.Windows.Forms.ToolStrip toolStrip;
  private System.Windows.Forms.ToolStripButton btnOpenImage;
  private System.Windows.Forms.ToolStripButton btnProcessImage;
  private System.Windows.Forms.ToolStripButton btnSaveCorrected;
  private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
  private System.Windows.Forms.SplitContainer splitContainer;
  private System.Windows.Forms.Label lblOriginal;
  private ChartDeskewApp.UI.Controls.ImageViewer originalImageViewer;
  private System.Windows.Forms.Label lblCorrected;
  private ChartDeskewApp.UI.Controls.ImageViewer correctedImageViewer;
  private ChartDeskewApp.UI.Controls.StatusPanel statusPanel;
  private System.Windows.Forms.ProgressBar progressBar;
}