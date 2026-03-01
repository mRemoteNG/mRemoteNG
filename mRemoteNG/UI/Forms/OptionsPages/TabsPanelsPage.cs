using mRemoteNG.App;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Tabs;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class TabsPanelsPage
    {
        #region Private Fields

        private OptRegistryTabsPanelsPage? pageRegSettingsInstance;
        private Font? _selectedConnectionTabFont;

        #endregion

        public TabsPanelsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Tab_16x);
        }

        public override string PageName
        {
            get => Language.TabsAndPanels.Replace("&&", "&", StringComparison.Ordinal);
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkAlwaysShowPanelTabs.Text = Language.AlwaysShowPanelTabs;
            chkAlwaysShowConnectionTabs.Text = Language.AlwaysShowConnectionTabs;
            chkOpenNewTabRightOfSelected.Text = Language.OpenNewTabRight;
            chkShowLogonInfoOnTabs.Text = Language.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Text = Language.ShowProtocolOnTabs;
            chkShowFolderPathOnTabs.Text = Language.ShowFolderPathOnTabs;
            chkIdentifyQuickConnectTabs.Text = Language.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Text = Language.DoubleClickTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Text = Language.AlwaysShowPanelSelection;
            chkCreateEmptyPanelOnStart.Text = Language.CreateEmptyPanelOnStartUp;
            chkBindConnectionsAndConfigPanels.Text = "Bind Connections and Config panels together when auto-hidden";
            chkLockPanels.Text = "Lock panels";
            chkDoNotRestoreOnRdpMinimize.Text = Language.DoNotRestoreOnRdpMinimize;
            chkAutoClosePanelOnLastTabClose.Text = "Auto close panel after closing the last tab";
            chkMinimizePanelsOnConnect.Text = "Auto-hide Connections/Config panels when a connection opens";
            chkKeepTabsOpenAfterDisconnect.Text = "Keep tabs open after disconnecting";
            chkUseCustomConnectionTabColor.Text = "Use custom connection tab color";
            chkUseCustomConnectionTabFont.Text = "Use custom connection tab font";
            btnSelectConnectionTabColor.Text = "Select...";
            btnSelectConnectionTabFont.Text = "Select...";
            lblPanelName.Text = $@"{Language.PanelName}:";
            lblSplitterSize.Text = "Splitter size:";
            lblDockPadding.Text = "Border size:";

            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkAlwaysShowPanelTabs.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs;
            chkAlwaysShowConnectionTabs.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs;

            /* 
             * Comment added: June 16, 2024
             * Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected nerver used
             *  Set Visible = false
            */
            //chkOpenNewTabRightOfSelected.Checked = Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected;
            chkOpenNewTabRightOfSelected.Visible = false;

            chkShowLogonInfoOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs;
            chkShowProtocolOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs;
            chkShowFolderPathOnTabs.Checked = Properties.OptionsTabsPanelsPage.Default.ShowFolderPathOnTabs;
            chkIdentifyQuickConnectTabs.Checked = Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs;
            chkDoubleClickClosesTab.Checked = Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt;
            chkAlwaysShowPanelSelectionDlg.Checked = Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg;
            chkCreateEmptyPanelOnStart.Checked = Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp;
            chkBindConnectionsAndConfigPanels.Checked = Properties.OptionsTabsPanelsPage.Default.BindConnectionsAndConfigPanels;
            chkLockPanels.Checked = Properties.OptionsTabsPanelsPage.Default.LockPanels;
            chkDoNotRestoreOnRdpMinimize.Checked = Properties.OptionsTabsPanelsPage.Default.DoNotRestoreOnRdpMinimize;
            chkAutoClosePanelOnLastTabClose.Checked = Properties.OptionsTabsPanelsPage.Default.AutoClosePanelOnLastTabClose;
            chkMinimizePanelsOnConnect.Checked = Properties.OptionsTabsPanelsPage.Default.MinimizePanelsOnConnect;
            chkKeepTabsOpenAfterDisconnect.Checked = Properties.OptionsTabsPanelsPage.Default.KeepTabsOpenAfterDisconnect;
            txtBoxPanelName.Text = Properties.OptionsTabsPanelsPage.Default.StartUpPanelName;
            nudSplitterSize.Value = Properties.OptionsTabsPanelsPage.Default.SplitterSize;
            nudDockPadding.Value = Properties.OptionsTabsPanelsPage.Default.DockPadding;

            LoadConnectionTabAppearanceSettings();
            UpdatePanelNameTextBox();
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelTabs = chkAlwaysShowPanelTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.AlwaysShowConnectionTabs = chkAlwaysShowConnectionTabs.Checked;

            // Defer the ShowHidePanelTabs call to avoid corrupting the Options window
            // This ensures the call happens after the Options window is closed
            if (FrmMain.IsCreated)
            {
                FrmMain.Default.BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                {
                    FrmMain.Default.ShowHidePanelTabs();
                    FrmMain.ShowHideConnectionTabs();
                    FrmMain.Default.SetPanelLock();
                }));
            }

            /* 
             * Comment added: June 16, 2024
             * Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected nerver used
            */
            //Properties.OptionsTabsPanelsPage.Default.OpenTabsRightOfSelected = chkOpenNewTabRightOfSelected.Checked;

            Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs = chkShowLogonInfoOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.ShowProtocolOnTabs = chkShowProtocolOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.ShowFolderPathOnTabs = chkShowFolderPathOnTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.IdentifyQuickConnectTabs = chkIdentifyQuickConnectTabs.Checked;
            Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt = chkDoubleClickClosesTab.Checked;
            Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg = chkAlwaysShowPanelSelectionDlg.Checked;
            Properties.OptionsTabsPanelsPage.Default.CreateEmptyPanelOnStartUp = chkCreateEmptyPanelOnStart.Checked;
            Properties.OptionsTabsPanelsPage.Default.BindConnectionsAndConfigPanels = chkBindConnectionsAndConfigPanels.Checked;
            Properties.OptionsTabsPanelsPage.Default.LockPanels = chkLockPanels.Checked;
            Properties.OptionsTabsPanelsPage.Default.DoNotRestoreOnRdpMinimize = chkDoNotRestoreOnRdpMinimize.Checked;
            Properties.OptionsTabsPanelsPage.Default.AutoClosePanelOnLastTabClose = chkAutoClosePanelOnLastTabClose.Checked;
            Properties.OptionsTabsPanelsPage.Default.StartUpPanelName = txtBoxPanelName.Text;
            Properties.OptionsTabsPanelsPage.Default.SplitterSize = (int)nudSplitterSize.Value;
            Properties.OptionsTabsPanelsPage.Default.DockPadding = (int)nudDockPadding.Value;

            Properties.OptionsTabsPanelsPage.Default.KeepTabsOpenAfterDisconnect = chkKeepTabsOpenAfterDisconnect.Checked;

            Properties.OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor = chkUseCustomConnectionTabColor.Checked;
            Properties.OptionsTabsPanelsPage.Default.ConnectionTabColor = chkUseCustomConnectionTabColor.Checked
                ? txtConnectionTabColor.Text.Trim()
                : string.Empty;

            bool useCustomFont = chkUseCustomConnectionTabFont.Checked && _selectedConnectionTabFont != null;
            Properties.OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont = useCustomFont;
            Properties.OptionsTabsPanelsPage.Default.ConnectionTabFontName = useCustomFont
                ? _selectedConnectionTabFont!.Name
                : string.Empty;
            Properties.OptionsTabsPanelsPage.Default.ConnectionTabFontSize = useCustomFont
                ? _selectedConnectionTabFont!.SizeInPoints
                : 0f;

            ConnectionTabAppearanceSettings.ResetCache();
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistryTabsPanelsPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryTabsPanelsPage;

            // If registry settings don't exist, create a default instance to prevent null reference exceptions
            if (pageRegSettingsInstance == null)
            {
                pageRegSettingsInstance = new OptRegistryTabsPanelsPage();
                Logger.Instance.Log?.Debug("[TabsPanelsPage.LoadRegistrySettings] pageRegSettingsInstance was null, created default instance");
            }

            RegistryLoader.Cleanup(settingsType);

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.AlwaysShowPanelTabs.IsSet)
                DisableControl(chkAlwaysShowPanelTabs);

            if (pageRegSettingsInstance.ShowLogonInfoOnTabs.IsSet)
                DisableControl(chkShowLogonInfoOnTabs);

            if (pageRegSettingsInstance.ShowProtocolOnTabs.IsSet)
                DisableControl(chkShowProtocolOnTabs);

            if (pageRegSettingsInstance.IdentifyQuickConnectTabs.IsSet)
                DisableControl(chkIdentifyQuickConnectTabs);

            if (pageRegSettingsInstance.DoubleClickOnTabClosesIt.IsSet)
                DisableControl(chkDoubleClickClosesTab);

            if (pageRegSettingsInstance.AlwaysShowPanelSelectionDlg.IsSet)
                DisableControl(chkAlwaysShowPanelSelectionDlg);

            if (pageRegSettingsInstance.CreateEmptyPanelOnStartUp.IsSet)
                DisableControl(chkCreateEmptyPanelOnStart);

            if (pageRegSettingsInstance.StartUpPanelName.IsSet)
                DisableControl(txtBoxPanelName);

            if (pageRegSettingsInstance.BindConnectionsAndConfigPanels.IsSet)
                DisableControl(chkBindConnectionsAndConfigPanels);

            if (pageRegSettingsInstance.AutoClosePanelOnLastTabClose.IsSet)
                DisableControl(chkAutoClosePanelOnLastTabClose);

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        /// <summary>
        /// Checks if specific registry settings related to appearence page are used.
        /// </summary>
        private bool ShowRegistrySettingsUsedInfo()
        {
            if (pageRegSettingsInstance == null)
                return false;

            return pageRegSettingsInstance.AlwaysShowPanelTabs.IsSet
                || pageRegSettingsInstance.ShowLogonInfoOnTabs.IsSet
                || pageRegSettingsInstance.ShowProtocolOnTabs.IsSet
                || pageRegSettingsInstance.IdentifyQuickConnectTabs.IsSet
                || pageRegSettingsInstance.DoubleClickOnTabClosesIt.IsSet
                || pageRegSettingsInstance.AlwaysShowPanelSelectionDlg.IsSet
                || pageRegSettingsInstance.CreateEmptyPanelOnStartUp.IsSet
                || pageRegSettingsInstance.StartUpPanelName.IsSet
                || pageRegSettingsInstance.BindConnectionsAndConfigPanels.IsSet
                || pageRegSettingsInstance.AutoClosePanelOnLastTabClose.IsSet;
        }

        private void UpdatePanelNameTextBox()
        {
            if (pageRegSettingsInstance == null || !pageRegSettingsInstance.StartUpPanelName.IsSet)
                txtBoxPanelName.Enabled = chkCreateEmptyPanelOnStart.Checked;
        }

        private void LoadConnectionTabAppearanceSettings()
        {
            chkUseCustomConnectionTabColor.Checked = Properties.OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor;
            txtConnectionTabColor.Text = Properties.OptionsTabsPanelsPage.Default.ConnectionTabColor ?? string.Empty;
            UpdateTabColorControlsState();

            chkUseCustomConnectionTabFont.Checked = Properties.OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont;

            _selectedConnectionTabFont?.Dispose();
            _selectedConnectionTabFont = CreateTabFontFromSettings();
            txtConnectionTabFont.Text = FormatFontDisplay(_selectedConnectionTabFont);
            UpdateTabFontControlsState();
        }

        private static Font? CreateTabFontFromSettings()
        {
            string fontName = Properties.OptionsTabsPanelsPage.Default.ConnectionTabFontName?.Trim() ?? string.Empty;
            float fontSize = Properties.OptionsTabsPanelsPage.Default.ConnectionTabFontSize;

            if (string.IsNullOrWhiteSpace(fontName) || fontSize <= 0)
                return null;

            try
            {
                return new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Point);
            }
            catch
            {
                return null;
            }
        }

        private static string FormatFontDisplay(Font? font)
        {
            return font == null
                ? string.Empty
                : $"{font.Name}, {font.SizeInPoints:0.##} pt";
        }

        private static Color? TryParseColor(string? colorValue)
        {
            string value = colorValue?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                ColorConverter converter = new();
                object? converted = converter.ConvertFromString(value);
                return converted is Color color && !color.IsEmpty
                    ? color
                    : null;
            }
            catch
            {
                return null;
            }
        }

        private static string FormatColorForSettings(Color color)
        {
            return color.IsNamedColor
                ? color.Name
                : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        private void UpdateTabColorControlsState()
        {
            bool enabled = chkUseCustomConnectionTabColor.Checked;
            txtConnectionTabColor.Enabled = enabled;
            btnSelectConnectionTabColor.Enabled = enabled;
        }

        private void UpdateTabFontControlsState()
        {
            bool enabled = chkUseCustomConnectionTabFont.Checked;
            txtConnectionTabFont.Enabled = enabled;
            btnSelectConnectionTabFont.Enabled = enabled;
        }

        private void chkUseCustomConnectionTabColor_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTabColorControlsState();
        }

        private void btnSelectConnectionTabColor_Click(object sender, EventArgs e)
        {
            using ColorDialog colorDialog = new()
            {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                Color = TryParseColor(txtConnectionTabColor.Text) ?? Color.DodgerBlue
            };

            if (colorDialog.ShowDialog(this) != DialogResult.OK)
                return;

            txtConnectionTabColor.Text = FormatColorForSettings(colorDialog.Color);
        }

        private void chkUseCustomConnectionTabFont_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTabFontControlsState();
        }

        private void btnSelectConnectionTabFont_Click(object sender, EventArgs e)
        {
            using FontDialog fontDialog = new()
            {
                ShowEffects = false,
                FontMustExist = true,
                MinSize = 6,
                MaxSize = 72,
                Font = _selectedConnectionTabFont ?? Font
            };

            if (fontDialog.ShowDialog(this) != DialogResult.OK)
                return;

            _selectedConnectionTabFont?.Dispose();
            _selectedConnectionTabFont = (Font)fontDialog.Font.Clone();
            txtConnectionTabFont.Text = FormatFontDisplay(_selectedConnectionTabFont);
        }

        private void chkCreateEmptyPanelOnStart_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdatePanelNameTextBox();
        }
    }
}