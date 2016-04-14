using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using System.Xml;
using System.Threading;
using System.Globalization;
using mRemoteNG.Themes;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Window;


namespace mRemoteNG.Config.Settings
{
	public class SettingsLoader
	{
		private frmMain _MainForm;

        public frmMain MainForm
		{
			get { return this._MainForm; }
			set { this._MainForm = value; }
		}
		
        
		public SettingsLoader(frmMain MainForm)
		{
			this._MainForm = MainForm;
		}
        
        #region Public Methods
        public void LoadSettings()
		{
			try
			{
                EnsureSettingsAreSavedInNewestVersion();

                SetSupportedCulture();

                SetTheme();
                SetApplicationWindowPositionAndSize();
                SetKioskMode();

                SetPuttyPath();
                SetShowSystemTrayIcon();
                SetAutoSave();
                SetConDefaultPassword();
				LoadPanelsFromXML();
				LoadExternalAppsFromXML();
                SetAlwaysShowPanelTabs();
						
				if (My.Settings.Default.ResetToolbars == true)
                    SetToolbarsDefault();
				else
                    LoadToolbarsFromSettings();
			}
			catch (Exception ex)
			{
                Runtime.Log.Error("Loading settings failed" + Environment.NewLine + ex.Message);
			}
		}

        private static void SetConDefaultPassword()
        {
            My.Settings.Default.ConDefaultPassword = Security.Crypt.Decrypt(My.Settings.Default.ConDefaultPassword, GeneralAppInfo.EncryptionKey);
        }

        private static void SetAlwaysShowPanelTabs()
        {
            if (My.Settings.Default.AlwaysShowPanelTabs)
            {
                frmMain.Default.pnlDock.DocumentStyle = DocumentStyle.DockingWindow;
            }
        }

        private static void SetTheme()
        {
            ThemeManager.LoadTheme(My.Settings.Default.ThemeName);
        }

        private static void SetSupportedCulture()
        {
            SupportedCultures.InstantiateSingleton();
            if (!(My.Settings.Default.OverrideUICulture == "") && SupportedCultures.IsNameSupported(My.Settings.Default.OverrideUICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(My.Settings.Default.OverrideUICulture);
                Runtime.Log.InfoFormat("Override Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName);
            }
        }

        private void SetApplicationWindowPositionAndSize()
        {
            _MainForm.WindowState = FormWindowState.Normal;
            if (My.Settings.Default.MainFormState == FormWindowState.Normal)
            {
                if (!My.Settings.Default.MainFormLocation.IsEmpty)
                    _MainForm.Location = My.Settings.Default.MainFormLocation;
                if (!My.Settings.Default.MainFormSize.IsEmpty)
                    _MainForm.Size = My.Settings.Default.MainFormSize;
            }
            else
            {
                if (!My.Settings.Default.MainFormRestoreLocation.IsEmpty)
                    _MainForm.Location = My.Settings.Default.MainFormRestoreLocation;
                if (!My.Settings.Default.MainFormRestoreSize.IsEmpty)
                    _MainForm.Size = My.Settings.Default.MainFormRestoreSize;
            }

            if (My.Settings.Default.MainFormState == FormWindowState.Maximized)
            {
                _MainForm.WindowState = FormWindowState.Maximized;
            }

            // Make sure the form is visible on the screen
            const int minHorizontal = 300;
            const int minVertical = 150;
            Rectangle screenBounds = Screen.FromHandle(_MainForm.Handle).Bounds;
            Rectangle newBounds = _MainForm.Bounds;

            if (newBounds.Right < screenBounds.Left + minHorizontal)
                newBounds.X = screenBounds.Left + minHorizontal - newBounds.Width;
            if (newBounds.Left > screenBounds.Right - minHorizontal)
                newBounds.X = screenBounds.Right - minHorizontal;
            if (newBounds.Bottom < screenBounds.Top + minVertical)
                newBounds.Y = screenBounds.Top + minVertical - newBounds.Height;
            if (newBounds.Top > screenBounds.Bottom - minVertical)
                newBounds.Y = screenBounds.Bottom - minVertical;

            this._MainForm.Location = newBounds.Location;
        }

        private void SetAutoSave()
        {
            if (My.Settings.Default.AutoSaveEveryMinutes > 0)
            {
                this._MainForm.tmrAutoSave.Interval = Convert.ToInt32(My.Settings.Default.AutoSaveEveryMinutes * 60000);
                this._MainForm.tmrAutoSave.Enabled = true;
            }
        }

        private void SetKioskMode()
        {
            if (My.Settings.Default.MainFormKiosk == true)
            {
                _MainForm.Fullscreen.Value = true;
                _MainForm.mMenViewFullscreen.Checked = true;
            }
        }

        private static void SetShowSystemTrayIcon()
        {
            if (My.Settings.Default.ShowSystemTrayIcon)
                Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
        }

        private static void SetPuttyPath()
        {
            if (My.Settings.Default.UseCustomPuttyPath)
                PuttyBase.PuttyPath = Convert.ToString(My.Settings.Default.CustomPuttyPath);
            else
                PuttyBase.PuttyPath = GeneralAppInfo.PuttyPath;
        }

        private static void EnsureSettingsAreSavedInNewestVersion()
        {
            if (My.Settings.Default.DoUpgrade)
                UpgradeSettingsVersion();
        }
        private static void UpgradeSettingsVersion()
        {
            try
            {
                My.Settings.Default.Upgrade();
            }
            catch (Exception ex)
            {
                Runtime.Log.Error("My.Settings.Upgrade() failed" + Environment.NewLine + ex.Message);
            }
            My.Settings.Default.DoUpgrade = false;

            // Clear pending update flag
            // This is used for automatic updates, not for settings migration, but it
            // needs to be cleared here because we know that we just updated.
            My.Settings.Default.UpdatePending = false;
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
			ToolStripPanelFromString(Convert.ToString(My.Settings.Default.QuickyTBParentDock)).Join(MainForm.tsQuickConnect, My.Settings.Default.QuickyTBLocation);
			MainForm.tsQuickConnect.Visible = Convert.ToBoolean(My.Settings.Default.QuickyTBVisible);
		}
		
		private void AddDynamicPanels()
		{
			ToolStripPanelFromString(Convert.ToString(My.Settings.Default.ExtAppsTBParentDock)).Join(MainForm.tsExternalTools, My.Settings.Default.ExtAppsTBLocation);
			MainForm.tsExternalTools.Visible = Convert.ToBoolean(My.Settings.Default.ExtAppsTBVisible);
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
                Windows.treePanel = null;
                Windows.configPanel = null;
                Windows.errorsPanel = null;
						
				while (MainForm.pnlDock.Contents.Count > 0)
				{
                    WeifenLuo.WinFormsUI.Docking.DockContent dc = (WeifenLuo.WinFormsUI.Docking.DockContent)MainForm.pnlDock.Contents[0];
					dc.Close();
				}

                Startup.CreatePanels();
						
				string oldPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName + "\\" + App.Info.SettingsFileInfo.LayoutFileName;
				string newPath = App.Info.SettingsFileInfo.SettingsPath + "\\" + App.Info.SettingsFileInfo.LayoutFileName;
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
                    Startup.SetDefaultLayout();
				}
			}
			catch (Exception ex)
			{
                Runtime.Log.Error("LoadPanelsFromXML failed" + Environment.NewLine + ex.Message);
			}
		}
		
		public void LoadExternalAppsFromXML()
		{
			string oldPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName + "\\" + App.Info.SettingsFileInfo.ExtAppsFilesName;
			string newPath = App.Info.SettingsFileInfo.SettingsPath + "\\" + App.Info.SettingsFileInfo.ExtAppsFilesName;
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
		
		private IDockContent GetContentFromPersistString(string persistString)
		{
			// pnlLayout.xml persistence XML fix for refactoring to mRemoteNG
			if (persistString.StartsWith("mRemote."))
				persistString = persistString.Replace("mRemote.", "mRemoteNG.");
					
			try
			{
				if (persistString == typeof(ConfigWindow).ToString())
                    return Windows.configPanel;
						
				if (persistString == typeof(ConnectionTreeWindow).ToString())
                    return Windows.treePanel;
						
				if (persistString == typeof(ErrorAndInfoWindow).ToString())
                    return Windows.errorsPanel;
						
				if (persistString == typeof(SessionsWindow).ToString())
                    return Windows.sessionsPanel;
						
				if (persistString == typeof(ScreenshotManagerWindow).ToString())
                    return Windows.screenshotPanel;
			}
			catch (Exception ex)
			{
                Runtime.Log.Error("GetContentFromPersistString failed" + Environment.NewLine + ex.Message);
			}
					
			return null;
		}
	}
}