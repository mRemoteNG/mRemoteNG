using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using System.IO;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.UI.Window
{
	public class ScreenshotManagerWindow : BaseWindow
	{
        #region Form Init
		internal MenuStrip msMain;
		internal ToolStripMenuItem mMenFile;
		internal ToolStripMenuItem mMenFileSaveAll;
		internal ToolStripMenuItem mMenFileRemoveAll;
		internal ContextMenuStrip cMenScreenshot;
		private System.ComponentModel.Container components = null;
		internal ToolStripMenuItem cMenScreenshotCopy;
		internal ToolStripMenuItem cMenScreenshotSave;
		internal SaveFileDialog dlgSaveSingleImage;
		internal FolderBrowserDialog dlgSaveAllImages;
		internal FlowLayoutPanel flpScreenshots;
				
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			Load += new EventHandler(ScreenshotManager_Load);
			flpScreenshots = new FlowLayoutPanel();
			msMain = new MenuStrip();
			mMenFile = new ToolStripMenuItem();
			mMenFile.DropDownOpening += new EventHandler(mMenFile_DropDownOpening);
			mMenFileSaveAll = new ToolStripMenuItem();
			mMenFileSaveAll.Click += new EventHandler(mMenFileSaveAll_Click);
			mMenFileRemoveAll = new ToolStripMenuItem();
			mMenFileRemoveAll.Click += new EventHandler(mMenFileRemoveAll_Click);
			cMenScreenshot = new ContextMenuStrip(components);
			cMenScreenshotCopy = new ToolStripMenuItem();
			cMenScreenshotCopy.Click += new EventHandler(cMenScreenshotCopy_Click);
			cMenScreenshotSave = new ToolStripMenuItem();
			cMenScreenshotSave.Click += new EventHandler(cMenScreenshotSave_Click);
			dlgSaveSingleImage = new SaveFileDialog();
			dlgSaveAllImages = new FolderBrowserDialog();
			msMain.SuspendLayout();
			cMenScreenshot.SuspendLayout();
			SuspendLayout();
			//
			//flpScreenshots
			//
			flpScreenshots.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                | AnchorStyles.Left)
                | AnchorStyles.Right;
			flpScreenshots.AutoScroll = true;
			flpScreenshots.Location = new Point(0, 26);
			flpScreenshots.Name = "flpScreenshots";
			flpScreenshots.Size = new Size(542, 296);
			flpScreenshots.TabIndex = 0;
			//
			//msMain
			//
			msMain.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
			msMain.Items.AddRange(new ToolStripItem[] {mMenFile});
			msMain.Location = new Point(0, 0);
			msMain.Name = "msMain";
			msMain.RenderMode = ToolStripRenderMode.Professional;
			msMain.Size = new Size(542, 24);
			msMain.TabIndex = 1;
			msMain.Text = "MenuStrip1";
			//
			//mMenFile
			//
			mMenFile.DropDownItems.AddRange(new ToolStripItem[] {mMenFileSaveAll, mMenFileRemoveAll});
			mMenFile.Image = Resources.File;
			mMenFile.Name = "mMenFile";
			mMenFile.Size = new Size(51, 20);
			mMenFile.Text = "&File";
			//
			//mMenFileSaveAll
			//
			mMenFileSaveAll.Image = Resources.Screenshot_Save;
			mMenFileSaveAll.Name = "mMenFileSaveAll";
			mMenFileSaveAll.Size = new Size(128, 22);
			mMenFileSaveAll.Text = "Save All";
			//
			//mMenFileRemoveAll
			//
			mMenFileRemoveAll.Image = Resources.Screenshot_Delete;
			mMenFileRemoveAll.Name = "mMenFileRemoveAll";
			mMenFileRemoveAll.Size = new Size(128, 22);
			mMenFileRemoveAll.Text = "Remove All";
			//
			//cMenScreenshot
			//
			cMenScreenshot.Items.AddRange(new ToolStripItem[] {cMenScreenshotCopy, cMenScreenshotSave});
			cMenScreenshot.Name = "cMenScreenshot";
			cMenScreenshot.Size = new Size(100, 48);
			//
			//cMenScreenshotCopy
			//
			cMenScreenshotCopy.Image = Resources.Screenshot_Copy;
			cMenScreenshotCopy.Name = "cMenScreenshotCopy";
			cMenScreenshotCopy.Size = new Size(99, 22);
			cMenScreenshotCopy.Text = "Copy";
			//
			//cMenScreenshotSave
			//
			cMenScreenshotSave.Image = Resources.Screenshot_Save;
			cMenScreenshotSave.Name = "cMenScreenshotSave";
			cMenScreenshotSave.Size = new Size(99, 22);
			cMenScreenshotSave.Text = "Save";
			//
			//dlgSaveSingleImage
			//
			dlgSaveSingleImage.Filter = "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" + 
				"le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" + 
				"twork Graphics File (.png)|*.png";
			dlgSaveSingleImage.FilterIndex = 4;
			//
			//ScreenshotManager
			//
			ClientSize = new Size(542, 323);
			Controls.Add(flpScreenshots);
			Controls.Add(msMain);
			HideOnClose = true;
			Icon = Resources.Screenshot_Icon;
			MainMenuStrip = msMain;
			Name = "ScreenshotManager";
			TabText = "Screenshots";
			Text = "Screenshots";
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
			ApplyLanguage();
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

		public ScreenshotManagerWindow(DockContent panel)
		{
			WindowType = WindowType.ScreenshotManager;
			DockPnl = panel;
			InitializeComponent();
		}
				
		public void AddScreenshot(Image Screenshot)
		{
			try
			{
				PictureBox nPB = new PictureBox();
				nPB.MouseDown += pbScreenshot_MouseDown;
						
				nPB.Parent = flpScreenshots;
				nPB.SizeMode = PictureBoxSizeMode.StretchImage;
				nPB.BorderStyle = BorderStyle.FixedSingle;
				nPB.ContextMenuStrip = cMenScreenshot;
				nPB.Image = Screenshot;
				nPB.Size = new Size(100, 100); //New Size((Screenshot.Width / 100) * 20, (Screenshot.Height / 100) * 20)
				nPB.Show();
						
				Button nBtn = new Button();
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
				Image mImage = sender.Image;
						
				Form nForm = new Form();
				nForm.StartPosition = FormStartPosition.CenterParent;
				nForm.ShowInTaskbar = false;
				nForm.ShowIcon = false;
				nForm.MaximizeBox = false;
				nForm.MinimizeBox = false;
				nForm.Width = mImage.Width + 2;
				nForm.Height = mImage.Height + 2;
				nForm.FormBorderStyle = FormBorderStyle.None;
						
				PictureBox nPB = new PictureBox();
				nPB.Parent = nForm;
				nPB.BorderStyle = BorderStyle.FixedSingle;
				nPB.Location = new Point(0, 0);
				nPB.SizeMode = PictureBoxSizeMode.AutoSize;
				nPB.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
				nPB.Image = mImage;
				nPB.ContextMenuStrip = cMenScreenshot;
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
				int pCount = 1;
						
				if (dlgSaveAllImages.ShowDialog() == DialogResult.OK)
				{
					foreach (string fPath in Directory.GetFiles(dlgSaveAllImages.SelectedPath, "Screenshot_*", SearchOption.TopDirectoryOnly))
					{
						FileInfo f = new FileInfo(fPath);
								
						string fCount = f.Name;
						fCount = fCount.Replace(f.Extension, "");
						fCount = fCount.Replace("Screenshot_", "");
								
						pCount = (int) (double.Parse(fCount) + 1);
					}
							
					foreach (Control ctrl in flpScreenshots.Controls)
					{
						if (ctrl is PictureBox)
						{
							(ctrl as PictureBox).Image.Save(dlgSaveAllImages.SelectedPath + "\\Screenshot_" + Tools.MiscTools.LeadingZero(Convert.ToString(pCount)) +".png", System.Drawing.Imaging.ImageFormat.Png);
							pCount++;
						}
					}
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
				if (dlgSaveSingleImage.ShowDialog() == DialogResult.OK)
				{
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
