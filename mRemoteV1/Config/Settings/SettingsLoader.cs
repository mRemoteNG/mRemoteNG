using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using System.Threading;
using System.Globalization;
using mRemoteNG.Themes;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.App.Info;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Config.Settings
{
    public class SettingsLoader
	{
		private frmMain _MainForm;
        private LayoutSettingsLoader _layoutSettingsLoader;
        private ExternalAppsLoader _externalAppsLoader;

        public frmMain MainForm
		{
			get { return _MainForm; }
			set { _MainForm = value; }
		}
		
        
		public SettingsLoader(frmMain MainForm)
		{
            _MainForm = MainForm;
            _layoutSettingsLoader = new LayoutSettingsLoader(_MainForm);
            _externalAppsLoader = new ExternalAppsLoader(_MainForm);
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
						
				if (mRemoteNG.Settings.Default.ResetToolbars == true)
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
            mRemoteNG.Settings.Default.ConDefaultPassword = Security.Crypt.Decrypt(mRemoteNG.Settings.Default.ConDefaultPassword, GeneralAppInfo.EncryptionKey);
        }

        private static void SetAlwaysShowPanelTabs()
        {
            if (mRemoteNG.Settings.Default.AlwaysShowPanelTabs)
                frmMain.Default.pnlDock.DocumentStyle = DocumentStyle.DockingWindow;
        }

        private static void SetTheme()
        {
            ThemeManager.LoadTheme(mRemoteNG.Settings.Default.ThemeName);
        }

        private static void SetSupportedCulture()
        {
            SupportedCultures.InstantiateSingleton();
            if (mRemoteNG.Settings.Default.OverrideUICulture != "" && SupportedCultures.IsNameSupported(mRemoteNG.Settings.Default.OverrideUICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(mRemoteNG.Settings.Default.OverrideUICulture);
                Runtime.Log.InfoFormat("Override Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName);
            }
        }

        private void SetApplicationWindowPositionAndSize()
        {
            _MainForm.WindowState = FormWindowState.Normal;
            if (mRemoteNG.Settings.Default.MainFormState == FormWindowState.Normal)
            {
                if (!mRemoteNG.Settings.Default.MainFormLocation.IsEmpty)
                    _MainForm.Location = mRemoteNG.Settings.Default.MainFormLocation;
                if (!mRemoteNG.Settings.Default.MainFormSize.IsEmpty)
                    _MainForm.Size = mRemoteNG.Settings.Default.MainFormSize;
            }
            else
            {
                if (!mRemoteNG.Settings.Default.MainFormRestoreLocation.IsEmpty)
                    _MainForm.Location = mRemoteNG.Settings.Default.MainFormRestoreLocation;
                if (!mRemoteNG.Settings.Default.MainFormRestoreSize.IsEmpty)
                    _MainForm.Size = mRemoteNG.Settings.Default.MainFormRestoreSize;
            }

            if (mRemoteNG.Settings.Default.MainFormState == FormWindowState.Maximized)
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

            _MainForm.Location = newBounds.Location;
        }

        private void SetAutoSave()
        {
            if (mRemoteNG.Settings.Default.AutoSaveEveryMinutes > 0)
            {
                _MainForm.tmrAutoSave.Interval = Convert.ToInt32(mRemoteNG.Settings.Default.AutoSaveEveryMinutes * 60000);
                _MainForm.tmrAutoSave.Enabled = true;
            }
        }

        private void SetKioskMode()
        {
            if (mRemoteNG.Settings.Default.MainFormKiosk == true)
            {
                _MainForm.Fullscreen.Value = true;
                _MainForm.mMenViewFullscreen.Checked = true;
            }
        }

        private static void SetShowSystemTrayIcon()
        {
            if (mRemoteNG.Settings.Default.ShowSystemTrayIcon)
                Runtime.NotificationAreaIcon = new Tools.Controls.NotificationAreaIcon();
        }

        private static void SetPuttyPath()
        {
            if (mRemoteNG.Settings.Default.UseCustomPuttyPath)
                PuttyBase.PuttyPath = Convert.ToString(mRemoteNG.Settings.Default.CustomPuttyPath);
            else
                PuttyBase.PuttyPath = GeneralAppInfo.PuttyPath;
        }

        private static void EnsureSettingsAreSavedInNewestVersion()
        {
            if (mRemoteNG.Settings.Default.DoUpgrade)
                UpgradeSettingsVersion();
        }
        private static void UpgradeSettingsVersion()
        {
            try
            {
                mRemoteNG.Settings.Default.Upgrade();
            }
            catch (Exception ex)
            {
                Runtime.Log.Error("Settings.Upgrade() failed" + Environment.NewLine + ex.Message);
            }
            mRemoteNG.Settings.Default.DoUpgrade = false;

            // Clear pending update flag
            // This is used for automatic updates, not for settings migration, but it
            // needs to be cleared here because we know that we just updated.
            mRemoteNG.Settings.Default.UpdatePending = false;
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
			if (mRemoteNG.Settings.Default.QuickyTBLocation.X > mRemoteNG.Settings.Default.ExtAppsTBLocation.X)
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
			ToolStripPanelFromString(Convert.ToString(mRemoteNG.Settings.Default.QuickyTBParentDock)).Join(MainForm.tsQuickConnect, mRemoteNG.Settings.Default.QuickyTBLocation);
			MainForm.tsQuickConnect.Visible = Convert.ToBoolean(mRemoteNG.Settings.Default.QuickyTBVisible);
		}
		
		private void AddDynamicPanels()
		{
			ToolStripPanelFromString(Convert.ToString(mRemoteNG.Settings.Default.ExtAppsTBParentDock)).Join(MainForm.tsExternalTools, mRemoteNG.Settings.Default.ExtAppsTBLocation);
			MainForm.tsExternalTools.Visible = Convert.ToBoolean(mRemoteNG.Settings.Default.ExtAppsTBVisible);
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
            _layoutSettingsLoader.LoadPanelsFromXML();
		}
		
		public void LoadExternalAppsFromXML()
		{
            _externalAppsLoader.LoadExternalAppsFromXML();
        }
        #endregion
	}
}