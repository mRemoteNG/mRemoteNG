using System;
using mRemoteNG.Config.Connections;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class ConnectionsPage
    {
        #region Private Fields
        private OptRegistryConnectionsPage pageRegSettingsInstance;
        private readonly FrmMain _frmMain = FrmMain.Default;

        // never used, added: Jun 15, 2024
        //private List<DropdownList> _connectionWarning;  

        #endregion

        public ConnectionsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.ASPWebSite_16x);

            /* 
             * Comments added: Jun 15, 2024 
             * These settings are not used on the settings page. It doesn't matter if they are set or not; nothing happens:
             * 1) chkSaveConnectionsAfterEveryEdit: never used
             * 2) pnlConfirmCloseConnection: seems to be unfinished. _connectionWarning or other corresponding settings are not available.
            */
            chkSaveConnectionsAfterEveryEdit.Visible = false; // Temporary hide control, never used, added: Jun 15, 2024 
            pnlConfirmCloseConnection.Visible = false; // Temporary hide control, never used, added: Jun 15, 2024 
        }

        public override string PageName
        {
            get => Language.Connections;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            /* 
             * Comments added: Jun 15, 2024
             * 
             * Seems to be unfinished or old
             */
            /*_connectionWarning = new List<DropdownList>
            {
                { new DropdownList((int)ConfirmCloseEnum.Never, Language.RadioCloseWarnMultiple)},
                { new DropdownList((int)ConfirmCloseEnum.Exit, Language.RadioCloseWarnExit)},
                { new DropdownList((int)ConfirmCloseEnum.Multiple, Language.RadioCloseWarnMultiple)},
                { new DropdownList((int)ConfirmCloseEnum.All, Language._CloseWarnAll)}
            };*/

            //comboBoxConnectionWarning.DataSource = _connectionWarning;
            //comboBoxConnectionWarning.DisplayMember = "DisplayString";
            //comboBoxConnectionWarning.ValueMember = "Index";

            chkSingleClickOnConnectionOpensIt.Text = Language.SingleClickOnConnectionOpensIt;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.SingleClickOnOpenConnectionSwitchesToIt;
            chkConnectionTreeTrackActiveConnection.Text = Language.TrackActiveConnectionInConnectionTree;
            chkHostnameLikeDisplayName.Text = Language.SetHostnameLikeDisplayName;
            chkUseFilterSearch.Text = Language.FilterSearchMatchesInConnectionTree;
            chkPlaceSearchBarAboveConnectionTree.Text = Language.PlaceSearchBarAboveConnectionTree;
            chkDoNotTrimUsername.Text = Language.DoNotTrimUsername;

            lblRdpReconnectionCount.Text = Language.RdpReconnectCount;
            lblRDPConTimeout.Text = Language.RdpOverallConnectionTimeout;
            lblAutoSave1.Text = Language.AutoSaveEvery;
            //ngLabel1.Text = Language.strLabelClosingConnections;

            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkSingleClickOnConnectionOpensIt.Checked = Settings.Default.SingleClickOnConnectionOpensIt;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = Settings.Default.SingleClickSwitchesToOpenConnection;
            chkConnectionTreeTrackActiveConnection.Checked = Settings.Default.TrackActiveConnectionInConnectionTree;
            chkHostnameLikeDisplayName.Checked = Settings.Default.SetHostnameLikeDisplayName;

            chkUseFilterSearch.Checked = Settings.Default.UseFilterSearch;
            chkPlaceSearchBarAboveConnectionTree.Checked = Settings.Default.PlaceSearchBarAboveConnectionTree;
            chkDoNotTrimUsername.Checked = Settings.Default.DoNotTrimUsername;

            numRdpReconnectionCount.Value = Convert.ToDecimal(Settings.Default.RdpReconnectionCount);
            numRDPConTimeout.Value = Convert.ToDecimal(Settings.Default.ConRDPOverallConnectionTimeout);
            numAutoSave.Value = Convert.ToDecimal(Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes);

            //comboBoxConnectionWarning.SelectedValue = Settings.Default.ConfirmCloseConnection;

            if (Properties.OptionsBackupPage.Default.SaveConnectionsFrequency == (int)ConnectionsBackupFrequencyEnum.Unassigned)
            {
                if (Properties.OptionsBackupPage.Default.SaveConnectionsAfterEveryEdit)
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int)ConnectionsBackupFrequencyEnum.OnEdit;
                }
                else if (Properties.OptionsBackupPage.Default.SaveConsOnExit)
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int)ConnectionsBackupFrequencyEnum.OnExit;
                }
                else
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int)ConnectionsBackupFrequencyEnum.Never;
                }
            }
        }

        public override void SaveSettings()
        {
            Properties.Settings.Default.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
            Properties.Settings.Default.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
            Properties.Settings.Default.TrackActiveConnectionInConnectionTree = chkConnectionTreeTrackActiveConnection.Checked;
            Properties.Settings.Default.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;

            Properties.Settings.Default.UseFilterSearch = chkUseFilterSearch.Checked;
            Properties.Settings.Default.PlaceSearchBarAboveConnectionTree = chkPlaceSearchBarAboveConnectionTree.Checked;
            Properties.Settings.Default.DoNotTrimUsername = chkDoNotTrimUsername.Checked;

            Properties.Settings.Default.RdpReconnectionCount = (int)numRdpReconnectionCount.Value;
            Properties.Settings.Default.ConRDPOverallConnectionTimeout = (int)numRDPConTimeout.Value;
            Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes = (int)numAutoSave.Value;
            if (Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes > 0)
            {
                _frmMain.tmrAutoSave.Interval = Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes * 60000;
                _frmMain.tmrAutoSave.Enabled = true;
            }
            else
            {
                _frmMain.tmrAutoSave.Enabled = false;
            }

            //Settings.Default.ConfirmCloseConnection = (int)comboBoxConnectionWarning.SelectedValue;
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistryConnectionsPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryConnectionsPage;

            RegistryLoader.Cleanup(settingsType);

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.SingleClickOnConnectionOpensIt.IsSet)
                DisableControl(chkSingleClickOnConnectionOpensIt);

            if (pageRegSettingsInstance.SingleClickSwitchesToOpenConnection.IsSet)
                DisableControl(chkSingleClickOnOpenedConnectionSwitchesToIt);

            if (pageRegSettingsInstance.TrackActiveConnectionInConnectionTree.IsSet)
                DisableControl(chkConnectionTreeTrackActiveConnection);

            if (pageRegSettingsInstance.SetHostnameLikeDisplayName.IsSet)
                DisableControl(chkHostnameLikeDisplayName);

            if (pageRegSettingsInstance.UseFilterSearch.IsSet)
                DisableControl(chkUseFilterSearch);

            if (pageRegSettingsInstance.PlaceSearchBarAboveConnectionTree.IsSet)
                DisableControl(chkPlaceSearchBarAboveConnectionTree);

            if (pageRegSettingsInstance.DoNotTrimUsername.IsSet)
                DisableControl(chkDoNotTrimUsername);

            if (pageRegSettingsInstance.RdpReconnectionCount.IsSet)
                DisableControl(numRdpReconnectionCount);

            if (pageRegSettingsInstance.ConRDPOverallConnectionTimeout.IsSet)
                DisableControl(numRDPConTimeout);

            if (pageRegSettingsInstance.AutoSaveEveryMinutes.IsSet)
                DisableControl(numAutoSave);

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        /// <summary>
        /// Checks if specific registry settings related to appearence page are used.
        /// </summary>
        public bool ShowRegistrySettingsUsedInfo()
        {
            return pageRegSettingsInstance.SingleClickOnConnectionOpensIt.IsSet
                || pageRegSettingsInstance.SingleClickSwitchesToOpenConnection.IsSet
                || pageRegSettingsInstance.TrackActiveConnectionInConnectionTree.IsSet
                || pageRegSettingsInstance.SetHostnameLikeDisplayName.IsSet
                || pageRegSettingsInstance.UseFilterSearch.IsSet
                || pageRegSettingsInstance.PlaceSearchBarAboveConnectionTree.IsSet
                || pageRegSettingsInstance.DoNotTrimUsername.IsSet
                || pageRegSettingsInstance.RdpReconnectionCount.IsSet
                || pageRegSettingsInstance.ConRDPOverallConnectionTimeout.IsSet
                || pageRegSettingsInstance.AutoSaveEveryMinutes.IsSet;
        }
    }
}