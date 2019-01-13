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

        private FlowLayoutPanel flpScreenshots;
        private VisualStudioToolStripExtender vsToolStripExtender;
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();

        private void InitializeComponent()
		{
            components = new System.ComponentModel.Container();
            flpScreenshots = new FlowLayoutPanel();
            msMain = new MenuStrip();
            mMenFile = new ToolStripMenuItem();
            mMenFileSaveAll = new ToolStripMenuItem();
            mMenFileRemoveAll = new ToolStripMenuItem();
            cMenScreenshot = new ContextMenuStrip(components);
            cMenScreenshotCopy = new ToolStripMenuItem();
            cMenScreenshotSave = new ToolStripMenuItem();
            dlgSaveSingleImage = new SaveFileDialog();
            dlgSaveAllImages = new FolderBrowserDialog();
            msMain.SuspendLayout();
            cMenScreenshot.SuspendLayout();
            SuspendLayout();
            // 
            // flpScreenshots
            // 
            flpScreenshots.Anchor = AnchorStyles.Top | AnchorStyles.Bottom 
                                                     | AnchorStyles.Left 
                                                     | AnchorStyles.Right;
            flpScreenshots.AutoScroll = true;
            flpScreenshots.Location = new Point(0, 26);
            flpScreenshots.Name = "flpScreenshots";
            flpScreenshots.Size = new Size(542, 296);
            flpScreenshots.TabIndex = 0;
            // 
            // msMain
            // 
            msMain.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            msMain.Items.AddRange(new ToolStripItem[] {
            mMenFile});
            msMain.Location = new Point(0, 0);
            msMain.Name = "msMain";
            msMain.RenderMode = ToolStripRenderMode.Professional;
            msMain.Size = new Size(542, 24);
            msMain.TabIndex = 1;
            msMain.Text = "MenuStrip1";
            // 
            // mMenFile
            // 
            mMenFile.DropDownItems.AddRange(new ToolStripItem[] {
            mMenFileSaveAll,
            mMenFileRemoveAll});
            mMenFile.Image = Resources.File;
            mMenFile.Name = "mMenFile";
            mMenFile.Size = new Size(53, 20);
            mMenFile.Text = "&File";
            mMenFile.DropDownOpening += mMenFile_DropDownOpening;
            // 
            // mMenFileSaveAll
            // 
            mMenFileSaveAll.Image = Resources.Screenshot_Save;
            mMenFileSaveAll.Name = "mMenFileSaveAll";
            mMenFileSaveAll.Size = new Size(130, 22);
            mMenFileSaveAll.Text = "Save All";
            mMenFileSaveAll.Click += mMenFileSaveAll_Click;
            // 
            // mMenFileRemoveAll
            // 
            mMenFileRemoveAll.Image = Resources.Screenshot_Delete;
            mMenFileRemoveAll.Name = "mMenFileRemoveAll";
            mMenFileRemoveAll.Size = new Size(130, 22);
            mMenFileRemoveAll.Text = "Remove All";
            mMenFileRemoveAll.Click += mMenFileRemoveAll_Click;
            // 
            // cMenScreenshot
            // 
            cMenScreenshot.Items.AddRange(new ToolStripItem[] {
            cMenScreenshotCopy,
            cMenScreenshotSave});
            cMenScreenshot.Name = "cMenScreenshot";
            cMenScreenshot.Size = new Size(103, 48);
            // 
            // cMenScreenshotCopy
            // 
            cMenScreenshotCopy.Image = Resources.Screenshot_Copy;
            cMenScreenshotCopy.Name = "cMenScreenshotCopy";
            cMenScreenshotCopy.Size = new Size(102, 22);
            cMenScreenshotCopy.Text = "Copy";
            cMenScreenshotCopy.Click += cMenScreenshotCopy_Click;
            // 
            // cMenScreenshotSave
            // 
            cMenScreenshotSave.Image = Resources.Screenshot_Save;
            cMenScreenshotSave.Name = "cMenScreenshotSave";
            cMenScreenshotSave.Size = new Size(102, 22);
            cMenScreenshotSave.Text = "Save";
            cMenScreenshotSave.Click += cMenScreenshotSave_Click;
            // 
            // dlgSaveSingleImage
            // 
            dlgSaveSingleImage.Filter = "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" +
    "le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" +
    "twork Graphics File (.png)|*.png";
            dlgSaveSingleImage.FilterIndex = 4;
            // 
            // ScreenshotManagerWindow
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(542, 323);
            Controls.Add(flpScreenshots);
            Controls.Add(msMain);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HideOnClose = true;
            Icon = Resources.Screenshot_Icon;
            MainMenuStrip = msMain;
            Name = "ScreenshotManagerWindow";
            TabText = "Screenshots";
            Text = "Screenshots";
            Load += ScreenshotManager_Load;
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            cMenScreenshot.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

		}
        #endregion
				
        #region Form Stuff
		private void ScreenshotManager_Load(object sender, EventArgs e)
		{
            ApplyTheme();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
		}

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive) return;
            base.ApplyTheme();
            vsToolStripExtender = new VisualStudioToolStripExtender(components)
            {
                DefaultRenderer = _toolStripProfessionalRenderer
            };
            vsToolStripExtender.SetStyle(cMenScreenshot, ThemeManager.getInstance().ActiveTheme.Version, ThemeManager.getInstance().ActiveTheme.Theme);
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
