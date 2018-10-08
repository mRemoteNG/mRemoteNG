using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using System.Threading;
using System.Globalization;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.Config.Settings
{
    public class SettingsLoader
	{
        private readonly ExternalAppsLoader _externalAppsLoader;
        private readonly MessageCollector _messageCollector;
	    private readonly MenuStrip _mainMenu;
        private readonly QuickConnectToolStrip _quickConnectToolStrip;
        private readonly ExternalToolsToolStrip _externalToolsToolStrip;
	    private readonly MultiSshToolStrip _multiSshToolStrip;

        private FrmMain MainForm { get; }


	    public SettingsLoader(
            FrmMain mainForm, 
            MessageCollector messageCollector, 
            QuickConnectToolStrip quickConnectToolStrip, 
            ExternalToolsToolStrip externalToolsToolStrip,
            MultiSshToolStrip multiSshToolStrip,
            MenuStrip mainMenu)
		{
            if (mainForm == null)
                throw new ArgumentNullException(nameof(mainForm));
            if (messageCollector == null)
                throw new ArgumentNullException(nameof(messageCollector));
            if (quickConnectToolStrip == null)
                throw new ArgumentNullException(nameof(quickConnectToolStrip));
            if (externalToolsToolStrip == null)
                throw new ArgumentNullException(nameof(externalToolsToolStrip));
            if (multiSshToolStrip == null)
                throw new ArgumentNullException(nameof(multiSshToolStrip));
		    if (mainMenu == null)
		        throw new ArgumentNullException(nameof(mainMenu));

            MainForm = mainForm;
	        _messageCollector = messageCollector;
	        _quickConnectToolStrip = quickConnectToolStrip;
	        _externalToolsToolStrip = externalToolsToolStrip;
		    _multiSshToolStrip = multiSshToolStrip;
		    _mainMenu = mainMenu;
		    _externalAppsLoader = new ExternalAppsLoader(MainForm, messageCollector, _externalToolsToolStrip);
        }
        
        #region Public Methods
        public void LoadSettings()
		{
			try
			{
                EnsureSettingsAreSavedInNewestVersion();

                SetSupportedCulture(); 
                SetApplicationWindowPositionAndSize();
                SetKioskMode();

                SetPuttyPath();
                SetShowSystemTrayIcon();
                SetAutoSave();
				LoadExternalAppsFromXml();
                SetAlwaysShowPanelTabs();
						
				if (mRemoteNG.Settings.Default.ResetToolbars)
                    SetToolbarsDefault();
				else
                    LoadToolbarsFromSettings();
			}
			catch (Exception ex)
			{
                _messageCollector.AddExceptionMessage("Loading settings failed", ex);
			}
		}

        private static void SetAlwaysShowPanelTabs()
        {
            if (mRemoteNG.Settings.Default.AlwaysShowPanelTabs)
                FrmMain.Default.pnlDock.DocumentStyle = DocumentStyle.DockingWindow;
        }

 

        private void SetSupportedCulture()
        {
            if (mRemoteNG.Settings.Default.OverrideUICulture == "" ||
                !SupportedCultures.IsNameSupported(mRemoteNG.Settings.Default.OverrideUICulture)) return;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(mRemoteNG.Settings.Default.OverrideUICulture);
            _messageCollector.AddMessage(MessageClass.InformationMsg, $"Override Culture: {Thread.CurrentThread.CurrentUICulture.Name}/{Thread.CurrentThread.CurrentUICulture.NativeName}", true);
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
            {
                MainForm.WindowState = FormWindowState.Maximized;
            }

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
            if (mRemoteNG.Settings.Default.AutoSaveEveryMinutes <= 0) return;
            MainForm.tmrAutoSave.Interval = mRemoteNG.Settings.Default.AutoSaveEveryMinutes * 60000;
            MainForm.tmrAutoSave.Enabled = true;
        }

        private void SetKioskMode()
        {
            if (!mRemoteNG.Settings.Default.MainFormKiosk) return;
            MainForm.Fullscreen.Value = true;
        }

        private static void SetShowSystemTrayIcon()
        {
            if (mRemoteNG.Settings.Default.ShowSystemTrayIcon)
                Runtime.NotificationAreaIcon = new NotificationAreaIcon();
        }

        private static void SetPuttyPath()
        {
            PuttyBase.PuttyPath = mRemoteNG.Settings.Default.UseCustomPuttyPath ? mRemoteNG.Settings.Default.CustomPuttyPath : GeneralAppInfo.PuttyPath;
        }

        private void EnsureSettingsAreSavedInNewestVersion()
        {
            if (mRemoteNG.Settings.Default.DoUpgrade)
                UpgradeSettingsVersion();
        }

        private void UpgradeSettingsVersion()
        {
            try
            {
                mRemoteNG.Settings.Default.Save();
                mRemoteNG.Settings.Default.Upgrade();
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("Settings.Upgrade() failed", ex);
            }
            mRemoteNG.Settings.Default.DoUpgrade = false;

            // Clear pending update flag
            // This is used for automatic updates, not for settings migration, but it
            // needs to be cleared here because we know that we just updated.
            mRemoteNG.Settings.Default.UpdatePending = false;
        }

	    private void SetToolbarsDefault()
		{
			ToolStripPanelFromString("top").Join(_quickConnectToolStrip, new Point(300, 0));
            _quickConnectToolStrip.Visible = true;
			ToolStripPanelFromString("bottom").Join(_externalToolsToolStrip, new Point(3, 0));
            _externalToolsToolStrip.Visible = false;
		}

	    private void LoadToolbarsFromSettings()
		{
            ResetAllToolbarLocations();
		    AddMainMenuPanel();
            AddExternalAppsPanel();
		    AddQuickConnectPanel();
		    AddMultiSshPanel();
        }

        /// <summary>
        /// This prevents odd positioning issues due to toolbar load order.
        /// Since all toolbars start in this temp panel, no toolbar load
        /// can be blocked by pre-existing toolbars.
        /// </summary>
	    private void ResetAllToolbarLocations()
	    {
	        var tempToolStrip = new ToolStripPanel();
            tempToolStrip.Join(_mainMenu);
	        tempToolStrip.Join(_quickConnectToolStrip);
	        tempToolStrip.Join(_externalToolsToolStrip);
	        tempToolStrip.Join(_multiSshToolStrip);
        }

	    private void AddMainMenuPanel()
	    {
	        SetToolstripGripStyle(_mainMenu);
            var toolStripPanel = ToolStripPanelFromString("top");
	        toolStripPanel.Join(_mainMenu, new Point(3, 0));
        }

		private void AddQuickConnectPanel()
		{
		    SetToolstripGripStyle(_quickConnectToolStrip);
            _quickConnectToolStrip.Visible = mRemoteNG.Settings.Default.QuickyTBVisible;
            var toolStripPanel = ToolStripPanelFromString(mRemoteNG.Settings.Default.QuickyTBParentDock);
            toolStripPanel.Join(_quickConnectToolStrip, mRemoteNG.Settings.Default.QuickyTBLocation);
		}
		
		private void AddExternalAppsPanel()
		{
		    SetToolstripGripStyle(_externalToolsToolStrip);
            _externalToolsToolStrip.Visible = mRemoteNG.Settings.Default.ExtAppsTBVisible;
            var toolStripPanel = ToolStripPanelFromString(mRemoteNG.Settings.Default.ExtAppsTBParentDock);
            toolStripPanel.Join(_externalToolsToolStrip, mRemoteNG.Settings.Default.ExtAppsTBLocation);
		}

	    private void AddMultiSshPanel()
	    {
	        SetToolstripGripStyle(_multiSshToolStrip);
	        _multiSshToolStrip.Visible = mRemoteNG.Settings.Default.MultiSshToolbarVisible;
            var toolStripPanel = ToolStripPanelFromString(mRemoteNG.Settings.Default.MultiSshToolbarParentDock);
            toolStripPanel.Join(_multiSshToolStrip, mRemoteNG.Settings.Default.MultiSshToolbarLocation);
	    }

	    private void SetToolstripGripStyle(ToolStrip toolbar)
	    {
	        toolbar.GripStyle = mRemoteNG.Settings.Default.LockToolbars
	            ? ToolStripGripStyle.Hidden
	            : ToolStripGripStyle.Visible;
        }
		
		private ToolStripPanel ToolStripPanelFromString(string panel)
		{
			switch (panel.ToLower())
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

	    private void LoadExternalAppsFromXml()
		{
            _externalAppsLoader.LoadExternalAppsFromXML();
        }
        #endregion
	}
}