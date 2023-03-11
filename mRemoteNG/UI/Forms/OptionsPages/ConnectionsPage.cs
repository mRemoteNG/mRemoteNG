using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using mRemoteNG.Config.Connections;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class ConnectionsPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
		private List<DropdownList> _connectionWarning;

        public ConnectionsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.ASPWebSite_16x);
        }

        public override string PageName
        {
            get => Language.Connections;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
			
			_connectionWarning = new List<DropdownList>
            {
                { new DropdownList((int)ConfirmCloseEnum.Never, Language.RadioCloseWarnMultiple)},
                { new DropdownList((int)ConfirmCloseEnum.Exit, Language.RadioCloseWarnExit)},
                { new DropdownList((int)ConfirmCloseEnum.Multiple, Language.RadioCloseWarnMultiple)},
                { new DropdownList((int)ConfirmCloseEnum.All, Language._CloseWarnAll)}
            };

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
    }
}