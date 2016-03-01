using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App.Runtime;
using System.IO;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public class ScreenshotManager : UI.Window.Base
		{

			#region "Form Init"
			internal System.Windows.Forms.MenuStrip msMain;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFile;
			internal System.Windows.Forms.ToolStripMenuItem mMenFile {
				get { return withEventsField_mMenFile; }
				set {
					if (withEventsField_mMenFile != null) {
						withEventsField_mMenFile.DropDownOpening -= mMenFile_DropDownOpening;
					}
					withEventsField_mMenFile = value;
					if (withEventsField_mMenFile != null) {
						withEventsField_mMenFile.DropDownOpening += mMenFile_DropDownOpening;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileSaveAll;
			internal System.Windows.Forms.ToolStripMenuItem mMenFileSaveAll {
				get { return withEventsField_mMenFileSaveAll; }
				set {
					if (withEventsField_mMenFileSaveAll != null) {
						withEventsField_mMenFileSaveAll.Click -= mMenFileSaveAll_Click;
					}
					withEventsField_mMenFileSaveAll = value;
					if (withEventsField_mMenFileSaveAll != null) {
						withEventsField_mMenFileSaveAll.Click += mMenFileSaveAll_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_mMenFileRemoveAll;
			internal System.Windows.Forms.ToolStripMenuItem mMenFileRemoveAll {
				get { return withEventsField_mMenFileRemoveAll; }
				set {
					if (withEventsField_mMenFileRemoveAll != null) {
						withEventsField_mMenFileRemoveAll.Click -= mMenFileRemoveAll_Click;
					}
					withEventsField_mMenFileRemoveAll = value;
					if (withEventsField_mMenFileRemoveAll != null) {
						withEventsField_mMenFileRemoveAll.Click += mMenFileRemoveAll_Click;
					}
				}
			}
			internal System.Windows.Forms.ContextMenuStrip cMenScreenshot;
			private System.ComponentModel.IContainer components;
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenScreenshotCopy;
			internal System.Windows.Forms.ToolStripMenuItem cMenScreenshotCopy {
				get { return withEventsField_cMenScreenshotCopy; }
				set {
					if (withEventsField_cMenScreenshotCopy != null) {
						withEventsField_cMenScreenshotCopy.Click -= cMenScreenshotCopy_Click;
					}
					withEventsField_cMenScreenshotCopy = value;
					if (withEventsField_cMenScreenshotCopy != null) {
						withEventsField_cMenScreenshotCopy.Click += cMenScreenshotCopy_Click;
					}
				}
			}
			private System.Windows.Forms.ToolStripMenuItem withEventsField_cMenScreenshotSave;
			internal System.Windows.Forms.ToolStripMenuItem cMenScreenshotSave {
				get { return withEventsField_cMenScreenshotSave; }
				set {
					if (withEventsField_cMenScreenshotSave != null) {
						withEventsField_cMenScreenshotSave.Click -= cMenScreenshotSave_Click;
					}
					withEventsField_cMenScreenshotSave = value;
					if (withEventsField_cMenScreenshotSave != null) {
						withEventsField_cMenScreenshotSave.Click += cMenScreenshotSave_Click;
					}
				}
			}
			internal System.Windows.Forms.SaveFileDialog dlgSaveSingleImage;
			internal System.Windows.Forms.FolderBrowserDialog dlgSaveAllImages;

			internal System.Windows.Forms.FlowLayoutPanel flpScreenshots;
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
				//flpScreenshots
				//
				this.flpScreenshots.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
				this.flpScreenshots.AutoScroll = true;
				this.flpScreenshots.Location = new System.Drawing.Point(0, 26);
				this.flpScreenshots.Name = "flpScreenshots";
				this.flpScreenshots.Size = new System.Drawing.Size(542, 296);
				this.flpScreenshots.TabIndex = 0;
				//
				//msMain
				//
				this.msMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
				this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.mMenFile });
				this.msMain.Location = new System.Drawing.Point(0, 0);
				this.msMain.Name = "msMain";
				this.msMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
				this.msMain.Size = new System.Drawing.Size(542, 24);
				this.msMain.TabIndex = 1;
				this.msMain.Text = "MenuStrip1";
				//
				//mMenFile
				//
				this.mMenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.mMenFileSaveAll,
					this.mMenFileRemoveAll
				});
				this.mMenFile.Image = global::mRemoteNG.My.Resources.Resources.File;
				this.mMenFile.Name = "mMenFile";
				this.mMenFile.Size = new System.Drawing.Size(51, 20);
				this.mMenFile.Text = "&File";
				//
				//mMenFileSaveAll
				//
				this.mMenFileSaveAll.Image = global::mRemoteNG.My.Resources.Resources.Screenshot_Save;
				this.mMenFileSaveAll.Name = "mMenFileSaveAll";
				this.mMenFileSaveAll.Size = new System.Drawing.Size(128, 22);
				this.mMenFileSaveAll.Text = "Save All";
				//
				//mMenFileRemoveAll
				//
				this.mMenFileRemoveAll.Image = global::mRemoteNG.My.Resources.Resources.Screenshot_Delete;
				this.mMenFileRemoveAll.Name = "mMenFileRemoveAll";
				this.mMenFileRemoveAll.Size = new System.Drawing.Size(128, 22);
				this.mMenFileRemoveAll.Text = "Remove All";
				//
				//cMenScreenshot
				//
				this.cMenScreenshot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
					this.cMenScreenshotCopy,
					this.cMenScreenshotSave
				});
				this.cMenScreenshot.Name = "cMenScreenshot";
				this.cMenScreenshot.Size = new System.Drawing.Size(100, 48);
				//
				//cMenScreenshotCopy
				//
				this.cMenScreenshotCopy.Image = global::mRemoteNG.My.Resources.Resources.Screenshot_Copy;
				this.cMenScreenshotCopy.Name = "cMenScreenshotCopy";
				this.cMenScreenshotCopy.Size = new System.Drawing.Size(99, 22);
				this.cMenScreenshotCopy.Text = "Copy";
				//
				//cMenScreenshotSave
				//
				this.cMenScreenshotSave.Image = global::mRemoteNG.My.Resources.Resources.Screenshot_Save;
				this.cMenScreenshotSave.Name = "cMenScreenshotSave";
				this.cMenScreenshotSave.Size = new System.Drawing.Size(99, 22);
				this.cMenScreenshotSave.Text = "Save";
				//
				//dlgSaveSingleImage
				//
				this.dlgSaveSingleImage.Filter = "Graphics Interchange Format File (.gif)|*.gif|Joint Photographic Experts Group Fi" + "le (.jpeg)|*.jpeg|Joint Photographic Experts Group File (.jpg)|*.jpg|Portable Ne" + "twork Graphics File (.png)|*.png";
				this.dlgSaveSingleImage.FilterIndex = 4;
				//
				//ScreenshotManager
				//
				this.ClientSize = new System.Drawing.Size(542, 323);
				this.Controls.Add(this.flpScreenshots);
				this.Controls.Add(this.msMain);
				this.HideOnClose = true;
				this.Icon = global::mRemoteNG.My.Resources.Resources.Screenshot_Icon;
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

			#region "Form Stuff"
			private void ScreenshotManager_Load(object sender, System.EventArgs e)
			{
				ApplyLanguage();
			}

			private void ApplyLanguage()
			{
				mMenFile.Text = mRemoteNG.My.Language.strMenuFile;
				mMenFileSaveAll.Text = mRemoteNG.My.Language.strSaveAll;
				mMenFileRemoveAll.Text = mRemoteNG.My.Language.strRemoveAll;
				cMenScreenshotCopy.Text = mRemoteNG.My.Language.strMenuCopy;
				cMenScreenshotSave.Text = mRemoteNG.My.Language.strSave;
				dlgSaveSingleImage.Filter = mRemoteNG.My.Language.strSaveImageFilter;
				TabText = mRemoteNG.My.Language.strScreenshots;
				Text = mRemoteNG.My.Language.strScreenshots;
			}
			#endregion

			#region "Public Methods"
			public ScreenshotManager(DockContent Panel)
			{
				Load += ScreenshotManager_Load;
				this.WindowType = Type.ScreenshotManager;
				this.DockPnl = Panel;
				this.InitializeComponent();
			}

			public void AddScreenshot(Image Screenshot)
			{
				try {
					PictureBox nPB = new PictureBox();
					var _with1 = nPB;
					_with1.MouseDown += this.pbScreenshot_MouseDown;

					_with1.Parent = this.flpScreenshots;
					_with1.SizeMode = PictureBoxSizeMode.StretchImage;
					_with1.BorderStyle = BorderStyle.FixedSingle;
					_with1.ContextMenuStrip = this.cMenScreenshot;
					_with1.Image = Screenshot;
					_with1.Size = new Size(100, 100);
					//New Size((Screenshot.Width / 100) * 20, (Screenshot.Height / 100) * 20)
					_with1.Show();

					Button nBtn = new Button();
					var _with2 = nBtn;
					_with2.Click += btnCloseScreenshot_Click;

					_with2.Parent = nPB;
					_with2.FlatStyle = FlatStyle.Flat;
					_with2.Text = "×";
					_with2.Size = new Size(22, 22);
					_with2.Location = new Point(nPB.Width - _with2.Width, -1);
					_with2.Show();

					this.Show(My.MyProject.Forms.frmMain.pnlDock);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "AddScreenshot (UI.Window.ScreenshotManager) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "Private Methods"
			private void pbScreenshot_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
			{
				this.cMenScreenshot.Tag = sender;

				if (e.Button == System.Windows.Forms.MouseButtons.Left) {
					this.OpenScreenshot(sender);
				}
			}

			private void pbScreenshotOpen_MouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs e)
			{
				if (e.Button == System.Windows.Forms.MouseButtons.Left) {
					this.CloseOpenedScreenshot(sender.Parent);
				}
			}

			private void CloseOpenedScreenshot(Form form)
			{
				form.Close();
			}

			private void OpenScreenshot(PictureBox sender)
			{
				try {
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
					nPB.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top;
					nPB.Image = mImage;
					nPB.ContextMenuStrip = this.cMenScreenshot;
					nPB.Show();

					nPB.MouseDown += this.pbScreenshotOpen_MouseDown;

					nForm.ShowDialog();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "OpenScreenshot (UI.Window.ScreenshotManager) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void btnCloseScreenshot_Click(System.Object sender, System.EventArgs e)
			{
				try {
					sender.Parent.Dispose();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "btnCloseScreenshot_Click (UI.Window.ScreenshotManager) failed" + Constants.vbNewLine + ex.Message, true);
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
				try {
					int pCount = 1;

					if (this.dlgSaveAllImages.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						foreach (string fPath in Directory.GetFiles(this.dlgSaveAllImages.SelectedPath, "Screenshot_*", SearchOption.TopDirectoryOnly)) {
							FileInfo f = new FileInfo(fPath);

							string fCount = f.Name;
							fCount = fCount.Replace(f.Extension, "");
							fCount = fCount.Replace("Screenshot_", "");

							pCount = fCount + 1;
						}

						foreach (System.Windows.Forms.Control ctrl in this.flpScreenshots.Controls) {
							if (ctrl is PictureBox) {
								(ctrl as PictureBox).Image.Save(this.dlgSaveAllImages.SelectedPath + "\\Screenshot_" + mRemoteNG.Tools.Misc.LeadingZero(pCount) + ".png", System.Drawing.Imaging.ImageFormat.Png);
								pCount += 1;
							}
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SaveAllImages (UI.Window.ScreenshotManager) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void cMenScreenshotCopy_Click(System.Object sender, System.EventArgs e)
			{
				this.CopyImageToClipboard();
			}

			private void CopyImageToClipboard()
			{
				try {
					Clipboard.SetImage((cMenScreenshot.Tag as PictureBox).Image);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "CopyImageToClipboard (UI.Window.ScreenshotManager) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void cMenScreenshotSave_Click(System.Object sender, System.EventArgs e)
			{
				this.SaveSingleImage();
			}

			private void SaveSingleImage()
			{
				try {
					if (this.dlgSaveSingleImage.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						switch (Strings.LCase(this.dlgSaveSingleImage.FileName.Substring(this.dlgSaveSingleImage.FileName.LastIndexOf(".") + 1))) {
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
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SaveSingleImage (UI.Window.ScreenshotManager) failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void mMenFile_DropDownOpening(object sender, System.EventArgs e)
			{
				if (this.flpScreenshots.Controls.Count < 1) {
					this.mMenFileSaveAll.Enabled = false;
					this.mMenFileRemoveAll.Enabled = false;
				} else {
					this.mMenFileSaveAll.Enabled = true;
					this.mMenFileRemoveAll.Enabled = true;
				}
			}
			#endregion

		}
	}
}
