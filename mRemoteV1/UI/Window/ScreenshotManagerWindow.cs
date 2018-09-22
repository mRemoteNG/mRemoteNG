using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using System.IO;
using mRemoteNG.UI.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Window
{
	public class ScreenshotManagerWindow : BaseWindow
	{
        #region Form Init
		internal MenuStrip msMain;
	    private ToolStripMenuItem mMenFile;
	    private ToolStripMenuItem mMenFileSaveAll;
	    private ToolStripMenuItem mMenFileRemoveAll;
		internal ContextMenuStrip cMenScreenshot;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem cMenScreenshotCopy;
	    private ToolStripMenuItem cMenScreenshotSave;
		internal SaveFileDialog dlgSaveSingleImage;
		internal FolderBrowserDialog dlgSaveAllImages;

		internal FlowLayoutPanel flpScreenshots;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.flpScreenshots = new System.Windows.Forms.FlowLayoutPanel();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenFileRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenScreenshot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cMenScreenshotCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cMenScreenshotSave = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgSaveSingleImage = new System.Windows.Forms.SaveFileDialog();
            this.dlgSaveAllImages = new System.Windows.Forms.FolderBrowserDialog();
            this.msMain.SuspendLayout();
            this.cMenScreenshot.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpScreenshots
            // 
            this.flpScreenshots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpScreenshots.AutoScroll = true;
            this.flpScreenshots.Location = new System.Drawing.Point(0, 26);
            this.flpScreenshots.Name = "flpScreenshots";
            this.flpScreenshots.Size = new System.Drawing.Size(542, 296);
            this.flpScreenshots.TabIndex = 0;
            // 
            // msMain
            // 
            this.msMain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenFile});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.msMain.Size = new System.Drawing.Size(542, 24);
            this.msMain.TabIndex = 1;
            this.msMain.Text = "MenuStrip1";
            // 
            // mMenFile
            // 
            this.mMenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mMenFileSaveAll,
            this.mMenFileRemoveAll});
            this.mMenFile.Image = global::mRemoteNG.Resources.File;
            this.mMenFile.Name = "mMenFile";
            this.mMenFile.Size = new System.Drawing.Size(53, 20);
            this.mMenFile.Text = "&File";
            this.mMenFile.DropDownOpening += new System.EventHandler(this.mMenFile_DropDownOpening);
            // 
            // mMenFileSaveAll
            // 
            this.mMenFileSaveAll.Image = global::mRemoteNG.Resources.Screenshot_Save;
            this.mMenFileSaveAll.Name = "mMenFileSaveAll";
            this.mMenFileSaveAll.Size = new System.Drawing.Size(130, 22);
            this.mMenFileSaveAll.Text = "Save All";
            this.mMenFileSaveAll.Click += new System.EventHandler(this.mMenFileSaveAll_Click);
            // 
            // mMenFileRemoveAll
            // 
            this.mMenFileRemoveAll.Image = global::mRemoteNG.Resources.Screenshot_Delete;
            this.mMenFileRemoveAll.Name = "mMenFileRemoveAll";
            this.mMenFileRemoveAll.Size = new System.Drawing.Size(130, 22);
            this.mMenFileRemoveAll.Text = "Remove All";
            this.mMenFileRemoveAll.Click += new System.EventHandler(this.mMenFileRemoveAll_Click);
            // 
            // cMenScreenshot
            // 
            this.cMenScreenshot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cMenScreenshotCopy,
            this.cMenScreenshotSave});
            this.cMenScreenshot.Name = "cMenScreenshot";
            this.cMenScreenshot.Size = new System.Drawing.Size(103, 48);
            // 
            // cMenScreenshotCopy
            // 
            this.cMenScreenshotCopy.Image = global::mRemoteNG.Resources.Screenshot_Copy;
            this.cMenScreenshotCopy.Name = "cMenScreenshotCopy";
            this.cMenScreenshotCopy.Size = new System.Drawing.Size(102, 22);
            this.cMenScreenshotCopy.Text = "Copy";
            this.cMenScreenshotCopy.Click += new System.EventHandler(this.cMenScreenshotCopy_Click);
            // 
            // cMenScreenshotSave
            // 
            this.cMenScreenshotSave.Image = global::mRemoteNG.Resources.Screenshot_Save;
            this.cMenScreenshotSave.Name = "cMenScreenshotSave";
            this.cMenScreenshotSave.Size = new System.Drawing.Size(102, 22);
            this.cMenScreenshotSave.Text = "Save";
            this.cMenScreenshotSave.Click += new System.EventHandler(this.cMenScreenshotSave_Click);
            // 
            // dlgSaveSingleImage
            // 
            this.dlgSaveSingleImage.Filter = "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" +
    "le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" +
    "twork Graphics File (.png)|*.png";
            this.dlgSaveSingleImage.FilterIndex = 4;
            // 
            // ScreenshotManagerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(542, 323);
            this.Controls.Add(this.flpScreenshots);
            this.Controls.Add(this.msMain);
            this.HideOnClose = true;
            this.Icon = global::mRemoteNG.Resources.Screenshot_Icon;
            this.MainMenuStrip = this.msMain;
            this.Name = "ScreenshotManagerWindow";
            this.TabText = "Screenshots";
            this.Text = "Screenshots";
            this.Load += new System.EventHandler(this.ScreenshotManager_Load);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.cMenScreenshot.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion
				
        #region Form Stuff
		private void ScreenshotManager_Load(object sender, EventArgs e)
		{
            ApplyTheme();
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
		}

        private new void ApplyTheme()
        {
            if (ThemeManager.getInstance().ThemingActive)
            {
                base.ApplyTheme(); 
                this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
                vsToolStripExtender.DefaultRenderer = _toolStripProfessionalRenderer;
                vsToolStripExtender.SetStyle(cMenScreenshot, ThemeManager.getInstance().ActiveTheme.Version, ThemeManager.getInstance().ActiveTheme.Theme);
            }
        }
        private void ApplyLanguage()
		{
			mMenFile.Text = Language.strMenuFile;
			mMenFileSaveAll.Text = Language.strSaveAll;
			mMenFileRemoveAll.Text = Language.strRemoveAll;
			cMenScreenshotCopy.Text = Language.strMenuCopy;
			cMenScreenshotSave.Text = Language.strSave;
			dlgSaveSingleImage.Filter = Language.strSaveImageFilter;
			TabText = Language.strScreenshots;
			Text = Language.strScreenshots;
		}
        #endregion
				
        #region Public Methods

	    public ScreenshotManagerWindow() : this(new DockContent())
	    {
	    }

		internal ScreenshotManagerWindow(DockContent panel)
		{
			WindowType = WindowType.ScreenshotManager;
			DockPnl = panel;
			InitializeComponent();
		}
				
		public void AddScreenshot(Image Screenshot)
		{
			try
			{
				var nPB = new PictureBox();
				nPB.MouseDown += pbScreenshot_MouseDown;
						
				nPB.Parent = flpScreenshots;
				nPB.SizeMode = PictureBoxSizeMode.StretchImage;
				nPB.BorderStyle = BorderStyle.FixedSingle;
				nPB.ContextMenuStrip = cMenScreenshot;
				nPB.Image = Screenshot;
				nPB.Size = new Size(100, 100); //New Size((Screenshot.Width / 100) * 20, (Screenshot.Height / 100) * 20)
				nPB.Show();
						
				var nBtn = new Button();
				nBtn.Click += btnCloseScreenshot_Click;
						
				nBtn.Parent = nPB;
				nBtn.FlatStyle = FlatStyle.Flat;
				nBtn.Text = "×";
				nBtn.Size = new Size(22, 22);
				nBtn.Location = new Point(nPB.Width - nBtn.Width, -1);
				nBtn.Show();
						
				Show(FrmMain.Default.pnlDock);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddScreenshot (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Private Methods
		private void pbScreenshot_MouseDown(object sender, MouseEventArgs e)
		{
			cMenScreenshot.Tag = sender;
					
			if (e.Button == MouseButtons.Left)
			{
				OpenScreenshot((PictureBox)sender);
			}
		}
				
		private void pbScreenshotOpen_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				CloseOpenedScreenshot((Form)((PictureBox)sender).Parent);
			}
		}
				
		private void CloseOpenedScreenshot(Form form)
		{
			form.Close();
		}
				
		private void OpenScreenshot(PictureBox sender)
		{
			try
			{
				var mImage = sender.Image;

			    var nForm = new Form
			    {
			        StartPosition = FormStartPosition.CenterParent,
			        ShowInTaskbar = false,
			        ShowIcon = false,
			        MaximizeBox = false,
			        MinimizeBox = false,
			        Width = mImage.Width + 2,
			        Height = mImage.Height + 2,
			        FormBorderStyle = FormBorderStyle.None
			    };

			    var nPB = new PictureBox
			    {
			        Parent = nForm,
			        BorderStyle = BorderStyle.FixedSingle,
			        Location = new Point(0, 0),
			        SizeMode = PictureBoxSizeMode.AutoSize,
			        Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top,
			        Image = mImage,
			        ContextMenuStrip = cMenScreenshot
			    };
			    nPB.Show();
						
				nPB.MouseDown += pbScreenshotOpen_MouseDown;
						
				nForm.ShowDialog();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "OpenScreenshot (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void btnCloseScreenshot_Click(object sender, EventArgs e)
		{
			try
			{
				((PictureBox)sender).Parent.Dispose();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "btnCloseScreenshot_Click (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void mMenFileRemoveAll_Click(object sender, EventArgs e)
		{
			RemoveAllImages();
		}
				
		private void RemoveAllImages()
		{
			flpScreenshots.Controls.Clear();
		}
				
		private void mMenFileSaveAll_Click(object sender, EventArgs e)
		{
			SaveAllImages();
		}
				
		private void SaveAllImages()
		{
			try
			{
				var pCount = 1;

			    if (dlgSaveAllImages.ShowDialog() != DialogResult.OK) return;
			    foreach (var fPath in Directory.GetFiles(dlgSaveAllImages.SelectedPath, "Screenshot_*", SearchOption.TopDirectoryOnly))
			    {
			        var f = new FileInfo(fPath);
								
			        var fCount = f.Name;
			        fCount = fCount.Replace(f.Extension, "");
			        fCount = fCount.Replace("Screenshot_", "");
								
			        pCount = (int) (double.Parse(fCount) + 1);
			    }
							
			    foreach (Control ctrl in flpScreenshots.Controls)
			    {
			        if (!(ctrl is PictureBox)) continue;
			        (ctrl as PictureBox).Image.Save(dlgSaveAllImages.SelectedPath + "\\Screenshot_" + Tools.MiscTools.LeadingZero(Convert.ToString(pCount)) +".png", System.Drawing.Imaging.ImageFormat.Png);
			        pCount++;
			    }
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveAllImages (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void cMenScreenshotCopy_Click(object sender, EventArgs e)
		{
			CopyImageToClipboard();
		}
				
		private void CopyImageToClipboard()
		{
			try
			{
				Clipboard.SetImage(((PictureBox) cMenScreenshot.Tag).Image);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CopyImageToClipboard (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void cMenScreenshotSave_Click(object sender, EventArgs e)
		{
			SaveSingleImage();
		}
				
		private void SaveSingleImage()
		{
			try
			{
			    if (dlgSaveSingleImage.ShowDialog() != DialogResult.OK) return;
			    // ReSharper disable once SwitchStatementMissingSomeCases
			    switch (dlgSaveSingleImage.FileName.Substring(dlgSaveSingleImage.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower())
			    {
			        case "gif":
			            ((PictureBox) cMenScreenshot.Tag).Image.Save(dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Gif);
			            break;
			        case "jpeg":
			            ((PictureBox) cMenScreenshot.Tag).Image.Save(dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
			            break;
			        case "jpg":
			            ((PictureBox) cMenScreenshot.Tag).Image.Save(dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
			            break;
			        case "png":
			            ((PictureBox) cMenScreenshot.Tag).Image.Save(dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Png);
			            break;
			    }
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveSingleImage (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void mMenFile_DropDownOpening(object sender, EventArgs e)
		{
			if (flpScreenshots.Controls.Count < 1)
			{
				mMenFileSaveAll.Enabled = false;
				mMenFileRemoveAll.Enabled = false;
			}
			else
			{
				mMenFileSaveAll.Enabled = true;
				mMenFileRemoveAll.Enabled = true;
			}
		}
        #endregion
	}
}
