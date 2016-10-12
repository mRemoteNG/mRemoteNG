using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Themes;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Config.Settings
{
    public class SettingsLoader
    {
        private readonly ExternalAppsLoader _externalAppsLoader;
        private readonly LayoutSettingsLoader _layoutSettingsLoader;


        public SettingsLoader(frmMain mainForm)
        {
            MainForm = mainForm;
            _layoutSettingsLoader = new LayoutSettingsLoader(MainForm);
            _externalAppsLoader = new ExternalAppsLoader(MainForm);
        }

        public frmMain MainForm { get; set; }

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
            mRemoteNG.Settings.Default.ConDefaultPassword =
                cryptographyProvider.Decrypt(mRemoteNG.Settings.Default.ConDefaultPassword, Runtime.EncryptionKey);
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
            if ((mRemoteNG.Settings.Default.OverrideUICulture != "") &&
                SupportedCultures.IsNameSupported(mRemoteNG.Settings.Default.OverrideUICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(mRemoteNG.Settings.Default.OverrideUICulture);
                Logger.Instance.InfoFormat("Override Culture: {0}/{1}", Thread.CurrentThread.CurrentUICulture.Name,
                    Thread.CurrentThread.CurrentUICulture.NativeName);
            }
        }

        private void SetApplicationWindowPositionAndSize()
        {
            MainForm.WindowState = FormWindowState.Normal;
            if (mRemoteNG.Settings.Default.MainFormState == FormWindowState.Normal)
            {
                if (!mRemoteNG.Settings.Default.MainFormLocation.IsEmpty)
                    MainForm.Location = mRemoteNG.Settings.Default.MainFormLocation;
                if (!mRemoteNG.Settings.Default.MainFormSize.IsEmpty)
                    MainForm.Size = mRemoteNG.Settings.Default.MainFormSize;
            }
            else
            {
                if (!mRemoteNG.Settings.Default.MainFormRestoreLocation.IsEmpty)
                    MainForm.Location = mRemoteNG.Settings.Default.MainFormRestoreLocation;
                if (!mRemoteNG.Settings.Default.MainFormRestoreSize.IsEmpty)
                    MainForm.Size = mRemoteNG.Settings.Default.MainFormRestoreSize;
            }

            if (mRemoteNG.Settings.Default.MainFormState == FormWindowState.Maximized)
                MainForm.WindowState = FormWindowState.Maximized;

            // Make sure the form is visible on the screen
            const int minHorizontal = 300;
            const int minVertical = 150;
            var screenBounds = Screen.FromHandle(MainForm.Handle).Bounds;
            var newBounds = MainForm.Bounds;

            if (newBounds.Right < screenBounds.Left + minHorizontal)
                newBounds.X = screenBounds.Left + minHorizontal - newBounds.Width;
            if (newBounds.Left > screenBounds.Right - minHorizontal)
                newBounds.X = screenBounds.Right - minHorizontal;
            if (newBounds.Bottom < screenBounds.Top + minVertical)
                newBounds.Y = screenBounds.Top + minVertical - newBounds.Height;
            if (newBounds.Top > screenBounds.Bottom - minVertical)
                newBounds.Y = screenBounds.Bottom - minVertical;

            MainForm.Location = newBounds.Location;
        }

        private void SetAutoSave()
        {
            if (mRemoteNG.Settings.Default.AutoSaveEveryMinutes > 0)
            {
                MainForm.tmrAutoSave.Interval = Convert.ToInt32(mRemoteNG.Settings.Default.AutoSaveEveryMinutes*60000);
                MainForm.tmrAutoSave.Enabled = true;
            }
        }

        private void SetKioskMode()
        {
            if (mRemoteNG.Settings.Default.MainFormKiosk)
            {
                MainForm.Fullscreen.Value = true;
                MainForm.mMenViewFullscreen.Checked = true;
            }
        }

        private static void SetShowSystemTrayIcon()
        {
            if (mRemoteNG.Settings.Default.ShowSystemTrayIcon)
                Runtime.NotificationAreaIcon = new Controls.NotificationAreaIcon();
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
            ToolStripPanelFromString(Convert.ToString(mRemoteNG.Settings.Default.QuickyTBParentDock))
                .Join(MainForm.tsQuickConnect, mRemoteNG.Settings.Default.QuickyTBLocation);
            MainForm.tsQuickConnect.Visible = Convert.ToBoolean(mRemoteNG.Settings.Default.QuickyTBVisible);
        }

        private void AddDynamicPanels()
        {
            ToolStripPanelFromString(Convert.ToString(mRemoteNG.Settings.Default.ExtAppsTBParentDock))
                .Join(MainForm.tsExternalTools, mRemoteNG.Settings.Default.ExtAppsTBLocation);
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