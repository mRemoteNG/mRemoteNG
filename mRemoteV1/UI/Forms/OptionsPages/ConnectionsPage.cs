using mRemoteNG.Config;
using System;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class ConnectionsPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;

        public ConnectionsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.Root_Icon;
        }

        public override string PageName
        {
            get => Language.strConnections;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            chkSingleClickOnConnectionOpensIt.Text = Language.strSingleClickOnConnectionOpensIt;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.strSingleClickOnOpenConnectionSwitchesToIt;
            chkConnectionTreeTrackActiveConnection.Text = Language.strTrackActiveConnectionInConnectionTree;
            chkHostnameLikeDisplayName.Text = Language.strSetHostnameLikeDisplayName;
            chkSaveConnectionsAfterEveryEdit.Text = Language.SaveConnectionsAfterEveryEdit;
            chkUseFilterSearch.Text = Language.FilterSearchMatchesInConnectionTree;
            chkPlaceSearchBarAboveConnectionTree.Text = Language.PlaceSearchBarAboveConnectionTree;
            chkDoNotTrimUsername.Text = Language.DoNotTrimUsername;

            lblRdpReconnectionCount.Text = Language.strRdpReconnectCount;
            lblRDPConTimeout.Text = Language.strRDPOverallConnectionTimeout;
            lblAutoSave1.Text = Language.strAutoSaveEvery;

            lblClosingConnections.Text = Language.strLabelClosingConnections;
            radCloseWarnAll.Text = Language.strRadioCloseWarnAll;
            radCloseWarnMultiple.Text = Language.strRadioCloseWarnMultiple;
            radCloseWarnExit.Text = Language.strRadioCloseWarnExit;
            radCloseWarnNever.Text = Language.strRadioCloseWarnNever;
        }

        public override void LoadSettings()
        {
            chkSingleClickOnConnectionOpensIt.Checked = Settings.Default.SingleClickOnConnectionOpensIt;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Checked = Settings.Default.SingleClickSwitchesToOpenConnection;
            chkConnectionTreeTrackActiveConnection.Checked = Settings.Default.TrackActiveConnectionInConnectionTree;
            chkHostnameLikeDisplayName.Checked = Settings.Default.SetHostnameLikeDisplayName;
            chkSaveConnectionsAfterEveryEdit.Checked = Settings.Default.SaveConnectionsAfterEveryEdit;
            chkUseFilterSearch.Checked = Settings.Default.UseFilterSearch;
            chkPlaceSearchBarAboveConnectionTree.Checked = Settings.Default.PlaceSearchBarAboveConnectionTree;
            chkDoNotTrimUsername.Checked = Settings.Default.DoNotTrimUsername;

            numRdpReconnectionCount.Value = Convert.ToDecimal(Settings.Default.RdpReconnectionCount);
            numRDPConTimeout.Value = Convert.ToDecimal(Settings.Default.ConRDPOverallConnectionTimeout);
            numAutoSave.Value = Convert.ToDecimal(Settings.Default.AutoSaveEveryMinutes);

            switch (Settings.Default.ConfirmCloseConnection)
            {
                case (int)ConfirmCloseEnum.Never:
                    radCloseWarnNever.Checked = true;
                    break;
                case (int)ConfirmCloseEnum.Exit:
                    radCloseWarnExit.Checked = true;
                    break;
                case (int)ConfirmCloseEnum.Multiple:
                    radCloseWarnMultiple.Checked = true;
                    break;
                default:
                    radCloseWarnAll.Checked = true;
                    break;
            }
        }

        public override void SaveSettings()
        {
            Settings.Default.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
            Settings.Default.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
            Settings.Default.TrackActiveConnectionInConnectionTree = chkConnectionTreeTrackActiveConnection.Checked;
            Settings.Default.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;
            Settings.Default.SaveConnectionsAfterEveryEdit = chkSaveConnectionsAfterEveryEdit.Checked;
            Settings.Default.UseFilterSearch = chkUseFilterSearch.Checked;
            Settings.Default.PlaceSearchBarAboveConnectionTree = chkPlaceSearchBarAboveConnectionTree.Checked;
            Settings.Default.DoNotTrimUsername = chkDoNotTrimUsername.Checked;

            Settings.Default.RdpReconnectionCount = (int)numRdpReconnectionCount.Value;
            Settings.Default.ConRDPOverallConnectionTimeout = (int)numRDPConTimeout.Value;
            Settings.Default.AutoSaveEveryMinutes = (int)numAutoSave.Value;
            if (Settings.Default.AutoSaveEveryMinutes > 0)
            {
                _frmMain.tmrAutoSave.Interval = Settings.Default.AutoSaveEveryMinutes * 60000;
                _frmMain.tmrAutoSave.Enabled = true;
            }
            else
            {
                _frmMain.tmrAutoSave.Enabled = false;
            }

            if (radCloseWarnAll.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.All;
            }

            if (radCloseWarnMultiple.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Multiple;
            }

            if (radCloseWarnExit.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Exit;
            }

            if (radCloseWarnNever.Checked)
            {
                Settings.Default.ConfirmCloseConnection = (int)ConfirmCloseEnum.Never;
            }
        }
    }
}