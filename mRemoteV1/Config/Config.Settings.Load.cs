using System;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using System.Xml;


namespace mRemoteNG.Config.Settings
{
	public class Load
	{
        #region Public Properties
		private frmMain _MainForm;
        public frmMain MainForm
		{
			get
			{
				return this._MainForm;
			}
			set
			{
				this._MainForm = value;
			}
		}
        #endregion
				
        #region Public Methods
		public Load(frmMain MainForm)
		{
			this._MainForm = MainForm;
		}
				
		public void LoadSettings()
		{
			try
			{
				// Migrate settings from previous version
				if (My.Settings.Default.DoUpgrade)
				{
					try
					{
						My.Settings.Default.Upgrade();
					}
					catch (Exception ex)
					{
                        Runtime.Log.Error("My.Settings.Upgrade() failed" + Constants.vbNewLine + ex.Message);
					}
					My.Settings.Default.DoUpgrade = false;
							
					// Clear pending update flag
					// This is used for automatic updates, not for settings migration, but it
					// needs to be cleared here because we know that we just updated.
					My.Settings.Default.UpdatePending = false;
				}
						
				App.SupportedCultures.InstantiateSingleton();
				if (!(My.Settings.Default.OverrideUICulture == "") && App.SupportedCultures.IsNameSupported(System.Convert.ToString(My.Settings.Default.OverrideUICulture)))
				{
					System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(System.Convert.ToString(My.Settings.Default.OverrideUICulture));
					Runtime.Log.InfoFormat("Override Culture: {0}/{1}", System.Threading.Thread.CurrentThread.CurrentUICulture.Name, System.Threading.Thread.CurrentThread.CurrentUICulture.NativeName);
				}
						
				Themes.ThemeManager.LoadTheme(System.Convert.ToString(My.Settings.Default.ThemeName));
						
				this._MainForm.WindowState = FormWindowState.Normal;
				if (My.Settings.Default.MainFormState == FormWindowState.Normal)
				{
					if (!My.Settings.Default.MainFormLocation.IsEmpty)
					{
						this._MainForm.Location = My.Settings.Default.MainFormLocation;
					}
					if (!My.Settings.Default.MainFormSize.IsEmpty)
					{
						this._MainForm.Size = My.Settings.Default.MainFormSize;
					}
				}
				else
				{
					if (!My.Settings.Default.MainFormRestoreLocation.IsEmpty)
					{
						this._MainForm.Location = My.Settings.Default.MainFormRestoreLocation;
					}
					if (!My.Settings.Default.MainFormRestoreSize.IsEmpty)
					{
						this._MainForm.Size = My.Settings.Default.MainFormRestoreSize;
					}
				}
				if (My.Settings.Default.MainFormState == FormWindowState.Maximized)
				{
					this._MainForm.WindowState = FormWindowState.Maximized;
				}
						
				// Make sure the form is visible on the screen
				const int minHorizontal = 300;
				const int minVertical = 150;
				System.Drawing.Rectangle screenBounds = Screen.FromHandle(this._MainForm.Handle).Bounds;
				System.Drawing.Rectangle newBounds = this._MainForm.Bounds;
						
				if (newBounds.Right < screenBounds.Left + minHorizontal)
				{
					newBounds.X = screenBounds.Left + minHorizontal - newBounds.Width;
				}
				if (newBounds.Left > screenBounds.Right - minHorizontal)
				{
					newBounds.X = screenBounds.Right - minHorizontal;
				}
				if (newBounds.Bottom < screenBounds.Top + minVertical)
				{
					newBounds.Y = screenBounds.Top + minVertical - newBounds.Height;
				}
				if (newBounds.Top > screenBounds.Bottom - minVertical)
				{
					newBounds.Y = screenBounds.Bottom - minVertical;
				}
						
				this._MainForm.Location = newBounds.Location;
						
				if (My.Settings.Default.MainFormKiosk == true)
				{
					this._MainForm.Fullscreen.Value = true;
					this._MainForm.mMenViewFullscreen.Checked = true;
				}
						
				if (My.Settings.Default.UseCustomPuttyPath)
				{
					Connection.Protocol.PuttyBase.PuttyPath = System.Convert.ToString(My.Settings.Default.CustomPuttyPath);
				}
				else
				{
					Connection.Protocol.PuttyBase.PuttyPath = App.Info.General.PuttyPath;
				}
						
				if (My.Settings.Default.ShowSystemTrayIcon)
				{
					App.Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
				}
						
				if (My.Settings.Default.AutoSaveEveryMinutes > 0)
				{
					this._MainForm.tmrAutoSave.Interval = System.Convert.ToInt32(My.Settings.Default.AutoSaveEveryMinutes * 60000);
					this._MainForm.tmrAutoSave.Enabled = true;
				}
						
				My.Settings.Default.ConDefaultPassword = Security.Crypt.Decrypt(System.Convert.ToString(My.Settings.Default.ConDefaultPassword), App.Info.General.EncryptionKey);
						
				this.LoadPanelsFromXML();
				this.LoadExternalAppsFromXML();
						
				if (My.Settings.Default.AlwaysShowPanelTabs)
				{
					frmMain.Default.pnlDock.DocumentStyle = DocumentStyle.DockingWindow;
				}
						
				if (My.Settings.Default.ResetToolbars == false)
				{
					LoadToolbarsFromSettings();
				}
				else
				{
					SetToolbarsDefault();
				}
			}
			catch (Exception ex)
			{
                Runtime.Log.Error("Loading settings failed" + Constants.vbNewLine + ex.Message);
			}
		}
				
		public void SetToolbarsDefault()
		{
			ToolStripPanelFromString("top").Join(MainForm.tsQuickConnect, new Point(300, 0));
			MainForm.tsQuickConnect.Visible = true;
			ToolStripPanelFromString("bottom").Join(MainForm.tsExternalTools, new Point(3, 0));
			MainForm.tsExternalTools.Visible = false;
		}
				
		public void LoadToolbarsFromSettings()
		{
			if (My.Settings.Default.QuickyTBLocation.X > My.Settings.Default.ExtAppsTBLocation.X)
			{
				AddDynamicPanels();
				AddStaticPanels();
			}
			else
			{
				AddStaticPanels();
				AddDynamicPanels();
			}
		}
				
		private void AddStaticPanels()
		{
			ToolStripPanelFromString(System.Convert.ToString(My.Settings.Default.QuickyTBParentDock)).Join(MainForm.tsQuickConnect, My.Settings.Default.QuickyTBLocation);
			MainForm.tsQuickConnect.Visible = System.Convert.ToBoolean(My.Settings.Default.QuickyTBVisible);
		}
				
		private void AddDynamicPanels()
		{
			ToolStripPanelFromString(System.Convert.ToString(My.Settings.Default.ExtAppsTBParentDock)).Join(MainForm.tsExternalTools, My.Settings.Default.ExtAppsTBLocation);
			MainForm.tsExternalTools.Visible = System.Convert.ToBoolean(My.Settings.Default.ExtAppsTBVisible);
		}
				
		private ToolStripPanel ToolStripPanelFromString(string Panel)
		{
			switch (Panel.ToLower())
			{
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
			try
			{
                Runtime.Windows.treePanel = null;
                Runtime.Windows.configPanel = null;
                Runtime.Windows.errorsPanel = null;
						
				while (MainForm.pnlDock.Contents.Count > 0)
				{
                    WeifenLuo.WinFormsUI.Docking.DockContent dc = (WeifenLuo.WinFormsUI.Docking.DockContent)MainForm.pnlDock.Contents[0];
					dc.Close();
				}

                Runtime.Startup.CreatePanels();
						
				string oldPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName + "\\" + App.Info.Settings.LayoutFileName;
				string newPath = App.Info.Settings.SettingsPath + "\\" + App.Info.Settings.LayoutFileName;
				if (File.Exists(newPath))
				{
					MainForm.pnlDock.LoadFromXml(newPath, GetContentFromPersistString);
                #if !PORTABLE
				}
				else if (File.Exists(oldPath))
				{
					MainForm.pnlDock.LoadFromXml(oldPath, GetContentFromPersistString);
                #endif
				}
				else
				{
                    Runtime.Startup.SetDefaultLayout();
				}
			}
			catch (Exception ex)
			{
                Runtime.Log.Error("LoadPanelsFromXML failed" + Constants.vbNewLine + ex.Message);
			}
		}
				
		public void LoadExternalAppsFromXML()
		{
			string oldPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName + "\\" + App.Info.Settings.ExtAppsFilesName;
			string newPath = App.Info.Settings.SettingsPath + "\\" + App.Info.Settings.ExtAppsFilesName;
			XmlDocument xDom = new XmlDocument();
			if (File.Exists(newPath))
			{
				xDom.Load(newPath);
            #if !PORTABLE
			}
			else if (File.Exists(oldPath))
			{
				xDom.Load(oldPath);
            #endif
			}
			else
			{
				return;
			}
					
			foreach (XmlElement xEl in xDom.DocumentElement.ChildNodes)
			{
				Tools.ExternalTool extA = new Tools.ExternalTool();
				extA.DisplayName = xEl.Attributes["DisplayName"].Value;
				extA.FileName = xEl.Attributes["FileName"].Value;
				extA.Arguments = xEl.Attributes["Arguments"].Value;
						
				if (xEl.HasAttribute("WaitForExit"))
				{
					extA.WaitForExit = bool.Parse(xEl.Attributes["WaitForExit"].Value);
				}
						
				if (xEl.HasAttribute("TryToIntegrate"))
				{
					extA.TryIntegrate = bool.Parse(xEl.Attributes["TryToIntegrate"].Value);
				}

                Runtime.ExternalTools.Add(extA);
			}
					
			MainForm.SwitchToolBarText(System.Convert.ToBoolean(My.Settings.Default.ExtAppsTBShowText));
					
			xDom = null;
					
			frmMain.Default.AddExternalToolsToToolBar();
		}
        #endregion
				
        #region Private Methods
		private IDockContent GetContentFromPersistString(string persistString)
		{
			// pnlLayout.xml persistence XML fix for refactoring to mRemoteNG
			if (persistString.StartsWith("mRemote."))
			{
				persistString = persistString.Replace("mRemote.", "mRemoteNG.");
			}
					
			try
			{
				if (persistString == typeof(UI.Window.Config).ToString())
				{
                    return Runtime.Windows.configPanel;
				}
						
				if (persistString == typeof(UI.Window.Tree).ToString())
				{
                    return Runtime.Windows.treePanel;
				}
						
				if (persistString == typeof(UI.Window.ErrorsAndInfos).ToString())
				{
                    return Runtime.Windows.errorsPanel;
				}
						
				if (persistString == typeof(UI.Window.Sessions).ToString())
				{
                    return Runtime.Windows.sessionsPanel;
				}
						
				if (persistString == typeof(UI.Window.ScreenshotManager).ToString())
				{
                    return Runtime.Windows.screenshotPanel;
				}
			}
			catch (Exception ex)
			{
                Runtime.Log.Error("GetContentFromPersistString failed" + Constants.vbNewLine + ex.Message);
			}
					
			return null;
		}
        #endregion
	}
}
