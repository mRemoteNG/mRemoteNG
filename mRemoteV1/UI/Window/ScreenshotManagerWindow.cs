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
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileSaveAll;
		internal System.Windows.Forms.ToolStripMenuItem mMenFileRemoveAll;
		internal System.Windows.Forms.ContextMenuStrip cMenScreenshot;
		private System.ComponentModel.Container components = null;
		internal System.Windows.Forms.ToolStripMenuItem cMenScreenshotCopy;
		internal System.Windows.Forms.ToolStripMenuItem cMenScreenshotSave;
		internal System.Windows.Forms.SaveFileDialog dlgSaveSingleImage;
		internal System.Windows.Forms.FolderBrowserDialog dlgSaveAllImages;
		internal System.Windows.Forms.FlowLayoutPanel flpScreenshots;
				
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Load += new System.EventHandler(ScreenshotManager_Load);
			this.flpScreenshots = new System.Windows.Forms.FlowLayoutPanel();
			this.msMain = new System.Windows.Forms.MenuStrip();
			this.mMenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFile.DropDownOpening += new System.EventHandler(this.mMenFile_DropDownOpening);
			this.mMenFileSaveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileSaveAll.Click += new System.EventHandler(this.mMenFileSaveAll_Click);
			this.mMenFileRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mMenFileRemoveAll.Click += new System.EventHandler(this.mMenFileRemoveAll_Click);
			this.cMenScreenshot = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cMenScreenshotCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenScreenshotCopy.Click += new System.EventHandler(this.cMenScreenshotCopy_Click);
			this.cMenScreenshotSave = new System.Windows.Forms.ToolStripMenuItem();
			this.cMenScreenshotSave.Click += new System.EventHandler(this.cMenScreenshotSave_Click);
			this.dlgSaveSingleImage = new System.Windows.Forms.SaveFileDialog();
			this.dlgSaveAllImages = new System.Windows.Forms.FolderBrowserDialog();
			this.msMain.SuspendLayout();
			this.cMenScreenshot.SuspendLayout();
			this.SuspendLayout();
			//
			//flpScreenshots
			//
			this.flpScreenshots.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.flpScreenshots.AutoScroll = true;
			this.flpScreenshots.Location = new System.Drawing.Point(0, 26);
			this.flpScreenshots.Name = "flpScreenshots";
			this.flpScreenshots.Size = new System.Drawing.Size(542, 296);
			this.flpScreenshots.TabIndex = 0;
			//
			//msMain
			//
			this.msMain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenFile});
			this.msMain.Location = new System.Drawing.Point(0, 0);
			this.msMain.Name = "msMain";
			this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.msMain.Size = new System.Drawing.Size(542, 24);
			this.msMain.TabIndex = 1;
			this.msMain.Text = "MenuStrip1";
			//
			//mMenFile
			//
			this.mMenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mMenFileSaveAll, this.mMenFileRemoveAll});
			this.mMenFile.Image = Resources.File;
			this.mMenFile.Name = "mMenFile";
			this.mMenFile.Size = new System.Drawing.Size(51, 20);
			this.mMenFile.Text = "&File";
			//
			//mMenFileSaveAll
			//
			this.mMenFileSaveAll.Image = Resources.Screenshot_Save;
			this.mMenFileSaveAll.Name = "mMenFileSaveAll";
			this.mMenFileSaveAll.Size = new System.Drawing.Size(128, 22);
			this.mMenFileSaveAll.Text = "Save All";
			//
			//mMenFileRemoveAll
			//
			this.mMenFileRemoveAll.Image = Resources.Screenshot_Delete;
			this.mMenFileRemoveAll.Name = "mMenFileRemoveAll";
			this.mMenFileRemoveAll.Size = new System.Drawing.Size(128, 22);
			this.mMenFileRemoveAll.Text = "Remove All";
			//
			//cMenScreenshot
			//
			this.cMenScreenshot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.cMenScreenshotCopy, this.cMenScreenshotSave});
			this.cMenScreenshot.Name = "cMenScreenshot";
			this.cMenScreenshot.Size = new System.Drawing.Size(100, 48);
			//
			//cMenScreenshotCopy
			//
			this.cMenScreenshotCopy.Image = Resources.Screenshot_Copy;
			this.cMenScreenshotCopy.Name = "cMenScreenshotCopy";
			this.cMenScreenshotCopy.Size = new System.Drawing.Size(99, 22);
			this.cMenScreenshotCopy.Text = "Copy";
			//
			//cMenScreenshotSave
			//
			this.cMenScreenshotSave.Image = Resources.Screenshot_Save;
			this.cMenScreenshotSave.Name = "cMenScreenshotSave";
			this.cMenScreenshotSave.Size = new System.Drawing.Size(99, 22);
			this.cMenScreenshotSave.Text = "Save";
			//
			//dlgSaveSingleImage
			//
			this.dlgSaveSingleImage.Filter = "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" + 
				"le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" + 
				"twork Graphics File (.png)|*.png";
			this.dlgSaveSingleImage.FilterIndex = 4;
			//
			//ScreenshotManager
			//
			this.ClientSize = new System.Drawing.Size(542, 323);
			this.Controls.Add(this.flpScreenshots);
			this.Controls.Add(this.msMain);
			this.HideOnClose = true;
			this.Icon = Resources.Screenshot_Icon;
			this.MainMenuStrip = this.msMain;
			this.Name = "ScreenshotManager";
			this.TabText = "Screenshots";
			this.Text = "Screenshots";
			this.msMain.ResumeLayout(false);
			this.msMain.PerformLayout();
			this.cMenScreenshot.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
					
		}
        #endregion
				
        #region Form Stuff
		private void ScreenshotManager_Load(object sender, System.EventArgs e)
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
		public ScreenshotManagerWindow(DockContent Panel)
		{
			this.WindowType = WindowType.ScreenshotManager;
			this.DockPnl = Panel;
			this.InitializeComponent();
		}
				
		public void AddScreenshot(Image Screenshot)
		{
			try
			{
				PictureBox nPB = new PictureBox();
				nPB.MouseDown += this.pbScreenshot_MouseDown;
						
				nPB.Parent = this.flpScreenshots;
				nPB.SizeMode = PictureBoxSizeMode.StretchImage;
				nPB.BorderStyle = BorderStyle.FixedSingle;
				nPB.ContextMenuStrip = this.cMenScreenshot;
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
						
				this.Show(frmMain.Default.pnlDock);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AddScreenshot (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region Private Methods
		private void pbScreenshot_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.cMenScreenshot.Tag = sender;
					
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.OpenScreenshot((System.Windows.Forms.PictureBox)sender);
			}
		}
				
		private void pbScreenshotOpen_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.CloseOpenedScreenshot((System.Windows.Forms.Form)((Control)sender).Parent);
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
				nForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
						
				PictureBox nPB = new PictureBox();
				nPB.Parent = nForm;
				nPB.BorderStyle = BorderStyle.FixedSingle;
				nPB.Location = new Point(0, 0);
				nPB.SizeMode = PictureBoxSizeMode.AutoSize;
				nPB.Anchor = (System.Windows.Forms.AnchorStyles) (AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
				nPB.Image = mImage;
				nPB.ContextMenuStrip = this.cMenScreenshot;
				nPB.Show();
						
				nPB.MouseDown += this.pbScreenshotOpen_MouseDown;
						
				nForm.ShowDialog();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "OpenScreenshot (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void btnCloseScreenshot_Click(System.Object sender, System.EventArgs e)
		{
			try
			{
				((Control)sender).Parent.Dispose();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "btnCloseScreenshot_Click (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void mMenFileRemoveAll_Click(System.Object sender, System.EventArgs e)
		{
			this.RemoveAllImages();
		}
				
		private void RemoveAllImages()
		{
			this.flpScreenshots.Controls.Clear();
		}
				
		private void mMenFileSaveAll_Click(System.Object sender, System.EventArgs e)
		{
			this.SaveAllImages();
		}
				
		private void SaveAllImages()
		{
			try
			{
				int pCount = 1;
						
				if (this.dlgSaveAllImages.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					foreach (string fPath in Directory.GetFiles(this.dlgSaveAllImages.SelectedPath, "Screenshot_*", SearchOption.TopDirectoryOnly))
					{
						FileInfo f = new FileInfo(fPath);
								
						string fCount = f.Name;
						fCount = fCount.Replace(f.Extension, "");
						fCount = fCount.Replace("Screenshot_", "");
								
						pCount = (int) (double.Parse(fCount) + 1);
					}
							
					foreach (System.Windows.Forms.Control ctrl in this.flpScreenshots.Controls)
					{
						if (ctrl is PictureBox)
						{
							(ctrl as PictureBox).Image.Save(this.dlgSaveAllImages.SelectedPath + "\\Screenshot_" + Tools.MiscTools.LeadingZero(Convert.ToString(pCount)) +".png", System.Drawing.Imaging.ImageFormat.Png);
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
				
		private void cMenScreenshotCopy_Click(System.Object sender, System.EventArgs e)
		{
			this.CopyImageToClipboard();
		}
				
		private void CopyImageToClipboard()
		{
			try
			{
				Clipboard.SetImage((cMenScreenshot.Tag as PictureBox).Image);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CopyImageToClipboard (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void cMenScreenshotSave_Click(System.Object sender, System.EventArgs e)
		{
			this.SaveSingleImage();
		}
				
		private void SaveSingleImage()
		{
			try
			{
				if (this.dlgSaveSingleImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					switch (this.dlgSaveSingleImage.FileName.Substring(this.dlgSaveSingleImage.FileName.LastIndexOf(".") + 1).ToLower())
					{
						case "gif":
							(this.cMenScreenshot.Tag as PictureBox).Image.Save(this.dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Gif);
							break;
						case "jpeg":
							(this.cMenScreenshot.Tag as PictureBox).Image.Save(this.dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
							break;
						case "jpg":
							(this.cMenScreenshot.Tag as PictureBox).Image.Save(this.dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
							break;
						case "png":
							(this.cMenScreenshot.Tag as PictureBox).Image.Save(this.dlgSaveSingleImage.FileName, System.Drawing.Imaging.ImageFormat.Png);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveSingleImage (UI.Window.ScreenshotManager) failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void mMenFile_DropDownOpening(object sender, System.EventArgs e)
		{
			if (this.flpScreenshots.Controls.Count < 1)
			{
				this.mMenFileSaveAll.Enabled = false;
				this.mMenFileRemoveAll.Enabled = false;
			}
			else
			{
				this.mMenFileSaveAll.Enabled = true;
				this.mMenFileRemoveAll.Enabled = true;
			}
		}
        #endregion
	}
}
