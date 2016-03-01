using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App.Runtime;
using System.Xml;
using System.Environment;

namespace mRemoteNG.Config
{
	namespace Settings
	{
		public class Load
		{
			#region "Public Properties"
			private frmMain _MainForm;
			public frmMain MainForm {
				get { return this._MainForm; }
				set { this._MainForm = value; }
			}
			#endregion

			#region "Public Methods"
			public Load(frmMain MainForm)
			{
				this._MainForm = MainForm;
			}

			public void Load()
			{
				try {
					var _with1 = this._MainForm;
					// Migrate settings from previous version
					if (mRemoteNG.My.Settings.DoUpgrade) {
						try {
							mRemoteNG.My.Settings.Upgrade();
						} catch (Exception ex) {
							mRemoteNG.App.Runtime.Log.Error("My.Settings.Upgrade() failed" + Constants.vbNewLine + ex.Message);
						}
						mRemoteNG.My.Settings.DoUpgrade = false;

						// Clear pending update flag
						// This is used for automatic updates, not for settings migration, but it
						// needs to be cleared here because we know that we just updated.
						mRemoteNG.My.Settings.UpdatePending = false;
					}

					mRemoteNG.App.SupportedCultures.InstantiateSingleton();
					if (!string.IsNullOrEmpty(mRemoteNG.My.Settings.OverrideUICulture) & mRemoteNG.App.SupportedCultures.IsNameSupported(mRemoteNG.My.Settings.OverrideUICulture)) {
						System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(mRemoteNG.My.Settings.OverrideUICulture);
						mRemoteNG.App.Runtime.Log.InfoFormat("Override Culture: {0}/{1}", System.Threading.Thread.CurrentThread.CurrentUICulture.Name, System.Threading.Thread.CurrentThread.CurrentUICulture.NativeName);
					}

					mRemoteNG.Themes.ThemeManager.LoadTheme(mRemoteNG.My.Settings.ThemeName);

					_with1.WindowState = FormWindowState.Normal;
					if (mRemoteNG.My.Settings.MainFormState == FormWindowState.Normal) {
						if (!mRemoteNG.My.Settings.MainFormLocation.IsEmpty)
							_with1.Location = mRemoteNG.My.Settings.MainFormLocation;
						if (!mRemoteNG.My.Settings.MainFormSize.IsEmpty)
							_with1.Size = mRemoteNG.My.Settings.MainFormSize;
					} else {
						if (!mRemoteNG.My.Settings.MainFormRestoreLocation.IsEmpty)
							_with1.Location = mRemoteNG.My.Settings.MainFormRestoreLocation;
						if (!mRemoteNG.My.Settings.MainFormRestoreSize.IsEmpty)
							_with1.Size = mRemoteNG.My.Settings.MainFormRestoreSize;
					}
					if (mRemoteNG.My.Settings.MainFormState == FormWindowState.Maximized) {
						_with1.WindowState = FormWindowState.Maximized;
					}

					// Make sure the form is visible on the screen
					const int minHorizontal = 300;
					const int minVertical = 150;
					System.Drawing.Rectangle screenBounds = Screen.FromHandle(_with1.Handle).Bounds;
					System.Drawing.Rectangle newBounds = _with1.Bounds;

					if (newBounds.Right < screenBounds.Left + minHorizontal) {
						newBounds.X = screenBounds.Left + minHorizontal - newBounds.Width;
					}
					if (newBounds.Left > screenBounds.Right - minHorizontal) {
						newBounds.X = screenBounds.Right - minHorizontal;
					}
					if (newBounds.Bottom < screenBounds.Top + minVertical) {
						newBounds.Y = screenBounds.Top + minVertical - newBounds.Height;
					}
					if (newBounds.Top > screenBounds.Bottom - minVertical) {
						newBounds.Y = screenBounds.Bottom - minVertical;
					}

					_with1.Location = newBounds.Location;

					if (mRemoteNG.My.Settings.MainFormKiosk == true) {
						_with1.Fullscreen.Value = true;
						_with1.mMenViewFullscreen.Checked = true;
					}

					if (mRemoteNG.My.Settings.UseCustomPuttyPath) {
						mRemoteNG.Connection.Protocol.PuttyBase.PuttyPath = mRemoteNG.My.Settings.CustomPuttyPath;
					} else {
						mRemoteNG.Connection.Protocol.PuttyBase.PuttyPath = mRemoteNG.App.Info.General.PuttyPath;
					}

					if (mRemoteNG.My.Settings.ShowSystemTrayIcon) {
						mRemoteNG.App.Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
					}

					if (mRemoteNG.My.Settings.AutoSaveEveryMinutes > 0) {
						_with1.tmrAutoSave.Interval = mRemoteNG.My.Settings.AutoSaveEveryMinutes * 60000;
						_with1.tmrAutoSave.Enabled = true;
					}

					mRemoteNG.My.Settings.ConDefaultPassword = mRemoteNG.Security.Crypt.Decrypt(mRemoteNG.My.Settings.ConDefaultPassword, mRemoteNG.App.Info.General.EncryptionKey);

					this.LoadPanelsFromXML();
					this.LoadExternalAppsFromXML();

					if (mRemoteNG.My.Settings.AlwaysShowPanelTabs) {
						mRemoteNG.My.MyProject.Forms.frmMain.pnlDock.DocumentStyle = DocumentStyle.DockingWindow;
					}

					if (mRemoteNG.My.Settings.ResetToolbars == false) {
						LoadToolbarsFromSettings();
					} else {
						SetToolbarsDefault();
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.Log.Error("Loading settings failed" + Constants.vbNewLine + ex.Message);
				}
			}

			public void SetToolbarsDefault()
			{
				var _with2 = MainForm;
				ToolStripPanelFromString("top").Join(_with2.tsQuickConnect, new Point(300, 0));
				_with2.tsQuickConnect.Visible = true;
				ToolStripPanelFromString("bottom").Join(_with2.tsExternalTools, new Point(3, 0));
				_with2.tsExternalTools.Visible = false;
			}

			public void LoadToolbarsFromSettings()
			{
				var _with3 = this.MainForm;
				if (mRemoteNG.My.Settings.QuickyTBLocation.X > mRemoteNG.My.Settings.ExtAppsTBLocation.X) {
					AddDynamicPanels();
					AddStaticPanels();
				} else {
					AddStaticPanels();
					AddDynamicPanels();
				}
			}

			private void AddStaticPanels()
			{
				var _with4 = MainForm;
				ToolStripPanelFromString(mRemoteNG.My.Settings.QuickyTBParentDock).Join(_with4.tsQuickConnect, mRemoteNG.My.Settings.QuickyTBLocation);
				_with4.tsQuickConnect.Visible = mRemoteNG.My.Settings.QuickyTBVisible;
			}

			private void AddDynamicPanels()
			{
				var _with5 = MainForm;
				ToolStripPanelFromString(mRemoteNG.My.Settings.ExtAppsTBParentDock).Join(_with5.tsExternalTools, mRemoteNG.My.Settings.ExtAppsTBLocation);
				_with5.tsExternalTools.Visible = mRemoteNG.My.Settings.ExtAppsTBVisible;
			}

			private ToolStripPanel ToolStripPanelFromString(string Panel)
			{
				switch (Strings.LCase(Panel)) {
					case "top":
						return MainForm.tsContainer.TopToolStripPanel;
					case "bottom":
						return MainForm.tsContainer.BottomToolStripPanel;
					case "left":
						return MainForm.tsContainer.LeftToolStripPanel;
					case "right":
						return MainForm.tsContainer.RightToolStripPanel;
					default:
						return MainForm.tsContainer.TopToolStripPanel;
				}
			}

			public void LoadPanelsFromXML()
			{
				try {
					var _with6 = MainForm;
					Windows.treePanel = null;
					Windows.configPanel = null;
					Windows.errorsPanel = null;

					while (_with6.pnlDock.Contents.Count > 0) {
						WeifenLuo.WinFormsUI.Docking.DockContent dc = _with6.pnlDock.Contents(0);
						dc.Close();
					}

					Startup.CreatePanels();

					string oldPath = Environment.GetFolderPath(SpecialFolder.LocalApplicationData) + "\\" + mRemoteNG.My.MyProject.Application.Info.ProductName + "\\" + mRemoteNG.App.Info.Settings.LayoutFileName;
					string newPath = mRemoteNG.App.Info.Settings.SettingsPath + "\\" + mRemoteNG.App.Info.Settings.LayoutFileName;
					if (File.Exists(newPath)) {
						_with6.pnlDock.LoadFromXml(newPath, GetContentFromPersistString);
						#if Not PORTABLE
					} else if (File.Exists(oldPath)) {
						_with6.pnlDock.LoadFromXml(oldPath, GetContentFromPersistString);
						#endif
					} else {
						Startup.SetDefaultLayout();
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.Log.Error("LoadPanelsFromXML failed" + Constants.vbNewLine + ex.Message);
				}
			}

			public void LoadExternalAppsFromXML()
			{
				string oldPath = Environment.GetFolderPath(SpecialFolder.LocalApplicationData) + "\\" + mRemoteNG.My.MyProject.Application.Info.ProductName + "\\" + mRemoteNG.App.Info.Settings.ExtAppsFilesName;
				string newPath = mRemoteNG.App.Info.Settings.SettingsPath + "\\" + mRemoteNG.App.Info.Settings.ExtAppsFilesName;
				XmlDocument xDom = new XmlDocument();
				if (File.Exists(newPath)) {
					xDom.Load(newPath);
					#if Not PORTABLE
				} else if (File.Exists(oldPath)) {
					xDom.Load(oldPath);
					#endif
				} else {
					return;
				}

				foreach (XmlElement xEl in xDom.DocumentElement.ChildNodes) {
					Tools.ExternalTool extA = new Tools.ExternalTool();
					extA.DisplayName = xEl.Attributes["DisplayName"].Value;
					extA.FileName = xEl.Attributes["FileName"].Value;
					extA.Arguments = xEl.Attributes["Arguments"].Value;

					if (xEl.HasAttribute("WaitForExit")) {
						extA.WaitForExit = xEl.Attributes["WaitForExit"].Value;
					}

					if (xEl.HasAttribute("TryToIntegrate")) {
						extA.TryIntegrate = xEl.Attributes["TryToIntegrate"].Value;
					}

					mRemoteNG.App.Runtime.ExternalTools.Add(extA);
				}

				MainForm.SwitchToolBarText(mRemoteNG.My.Settings.ExtAppsTBShowText);

				xDom = null;

				My.MyProject.Forms.frmMain.AddExternalToolsToToolBar();
			}
			#endregion

			#region "Private Methods"
			private IDockContent GetContentFromPersistString(string persistString)
			{
				// pnlLayout.xml persistence XML fix for refactoring to mRemoteNG
				if ((persistString.StartsWith("mRemote."))) {
					persistString = persistString.Replace("mRemote.", "mRemoteNG.");
				}

				try {
					if (persistString == typeof(UI.Window.Config).ToString()) {
						return Windows.configPanel;
					}

					if (persistString == typeof(UI.Window.Tree).ToString()) {
						return Windows.treePanel;
					}

					if (persistString == typeof(UI.Window.ErrorsAndInfos).ToString()) {
						return Windows.errorsPanel;
					}

					if (persistString == typeof(UI.Window.Sessions).ToString()) {
						return Windows.sessionsPanel;
					}

					if (persistString == typeof(UI.Window.ScreenshotManager).ToString()) {
						return Windows.screenshotPanel;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.Log.Error("GetContentFromPersistString failed" + Constants.vbNewLine + ex.Message);
				}

				return null;
			}
			#endregion
		}
	}
}
