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
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Config.Settings
{
    public class SettingsLoader
	{
		private frmMain _mainForm;
        private readonly LayoutSettingsLoader _layoutSettingsLoader;
        private readonly ExternalAppsLoader _externalAppsLoader;

        public frmMain MainForm
		{
			get { return _mainForm; }
			set { _mainForm = value; }
		}
		
        
		public SettingsLoader(frmMain mainForm)
		{
            _mainForm = mainForm;
            _layoutSettingsLoader = new LayoutSettingsLoader(_mainForm);
            _externalAppsLoader = new ExternalAppsLoader(_mainForm);
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
				LoadPanelsFromXml();
				LoadExternalAppsFromXml();
                SetAlwaysShowPanelTabs();
						
				if (mRemoteNG.Settings.Default.ResetToolbars)
                    SetToolbarsDefault();
				else
                    LoadToolbarsFromSettings();
			}
			catch (Exception ex)
			{
                Logger.Instance.Error("Loading settings failed" + Environment.NewLine + ex.Message);
			}
		}

        private static void SetConDefaultPassword()
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            mRemoteNG.Settings.Default.ConDefaultPassword = cryptographyProvider.Decrypt(mRemoteNG.Settings.Default.ConDefaultPassword, Runtime.EncryptionKey);
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
            if (mRemoteNG.Settings.Default.OverrideUICulture != "" && SupportedCultures.IsNameSupported(mRemoteNG.Settings.Default.OverrideUICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(mRemoteNG.Settings.Default.OverrideUICulture);
                Logger.Instance.InfoFormat("Override Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.NativeName);
            }
        }

        private void SetApplicationWindowPositionAndSize()
        {
            _mainForm.WindowState = FormWindowState.Normal;
            if (mRemoteNG.Settings.Default.MainFormState == FormWindowState.Normal)
            {
                if (!mRemoteNG.Settings.Default.MainFormLocation.IsEmpty)
                    _mainForm.Location = mRemoteNG.Settings.Default.MainFormLocation;
                if (!mRemoteNG.Settings.Default.MainFormSize.IsEmpty)
                    _mainForm.Size = mRemoteNG.Settings.Default.MainFormSize;
            }
            else
            {
                if (!mRemoteNG.Settings.Default.MainFormRestoreLocation.IsEmpty)
                    _mainForm.Location = mRemoteNG.Settings.Default.MainFormRestoreLocation;
                if (!mRemoteNG.Settings.Default.MainFormRestoreSize.IsEmpty)
                    _mainForm.Size = mRemoteNG.Settings.Default.MainFormRestoreSize;
            }

            if (mRemoteNG.Settings.Default.MainFormState == FormWindowState.Maximized)
            {
                _mainForm.WindowState = FormWindowState.Maximized;
            }

            // Make sure the form is visible on the screen
            const int minHorizontal = 300;
            const int minVertical = 150;
            var screenBounds = Screen.FromHandle(_mainForm.Handle).Bounds;
            var newBounds = _mainForm.Bounds;

            if (newBounds.Right < screenBounds.Left + minHorizontal)
                newBounds.X = screenBounds.Left + minHorizontal - newBounds.Width;
            if (newBounds.Left > screenBounds.Right - minHorizontal)
                newBounds.X = screenBounds.Right - minHorizontal;
            if (newBounds.Bottom < screenBounds.Top + minVertical)
                newBounds.Y = screenBounds.Top + minVertical - newBounds.Height;
            if (newBounds.Top > screenBounds.Bottom - minVertical)
                newBounds.Y = screenBounds.Bottom - minVertical;

            _mainForm.Location = newBounds.Location;
        }

        private void SetAutoSave()
        {
            if (mRemoteNG.Settings.Default.AutoSaveEveryMinutes > 0)
            {
                _mainForm.tmrAutoSave.Interval = Convert.ToInt32(mRemoteNG.Settings.Default.AutoSaveEveryMinutes * 60000);
                _mainForm.tmrAutoSave.Enabled = true;
            }
        }

        private void SetKioskMode()
        {
            if (mRemoteNG.Settings.Default.MainFormKiosk)
            {
                _mainForm.Fullscreen.Value = true;
                _mainForm.mMenViewFullscreen.Checked = true;
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
                Logger.Instance.Error("Settings.Upgrade() failed" + Environment.NewLine + ex.Message);
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
		
		public void LoadPanelsFromXml()
		{
            _layoutSettingsLoader.LoadPanelsFromXml();
		}
		
		public void LoadExternalAppsFromXml()
		{
            _externalAppsLoader.LoadExternalAppsFromXML();
        }
        #endregion
	}
}