﻿using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using mRemoteNG.Config.Connections;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class ConnectionsPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
        private List<DropdownList> _connectionWarning;

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
            
            _connectionWarning = new List<DropdownList>
            {
                { new DropdownList((int)ConfirmCloseEnum.Never, Language.strRadioCloseWarnNever)},
                { new DropdownList((int)ConfirmCloseEnum.Exit, Language.strRadioCloseWarnExit)},
                { new DropdownList((int)ConfirmCloseEnum.Multiple, Language.strRadioCloseWarnMultiple)},
                { new DropdownList((int)ConfirmCloseEnum.All, Language.strRadioCloseWarnAll)}
            };

            comboBoxConnectionWarning.DataSource = _connectionWarning;
            comboBoxConnectionWarning.DisplayMember = "DisplayString";
            comboBoxConnectionWarning.ValueMember = "Index";

            chkSingleClickOnConnectionOpensIt.Text = Language.strSingleClickOnConnectionOpensIt;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.strSingleClickOnOpenConnectionSwitchesToIt;
            chkConnectionTreeTrackActiveConnection.Text = Language.strTrackActiveConnectionInConnectionTree;
            chkHostnameLikeDisplayName.Text = Language.strSetHostnameLikeDisplayName;
            chkUseFilterSearch.Text = Language.FilterSearchMatchesInConnectionTree;
            chkPlaceSearchBarAboveConnectionTree.Text = Language.PlaceSearchBarAboveConnectionTree;
            chkDoNotTrimUsername.Text = Language.DoNotTrimUsername;

            lblRdpReconnectionCount.Text = Language.strRdpReconnectCount;
            lblRDPConTimeout.Text = Language.strRDPOverallConnectionTimeout;
            lblAutoSave1.Text = Language.strAutoSaveEvery;
            ngLabel1.Text = Language.strLabelClosingConnections;

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
            numAutoSave.Value = Convert.ToDecimal(Settings.Default.AutoSaveEveryMinutes);

            comboBoxConnectionWarning.SelectedValue = Settings.Default.ConfirmCloseConnection;

            if (Settings.Default.SaveConnectionsFrequency == (int)ConnectionsBackupFrequencyEnum.Unassigned)
            {
                if (Settings.Default.SaveConnectionsAfterEveryEdit)
                {
                    Settings.Default.SaveConnectionsFrequency = (int)ConnectionsBackupFrequencyEnum.OnEdit;
                }
                else if (Settings.Default.SaveConsOnExit)
                {
                    Settings.Default.SaveConnectionsFrequency = (int)ConnectionsBackupFrequencyEnum.OnExit;
                }
                else
                {
                    Settings.Default.SaveConnectionsFrequency = (int)ConnectionsBackupFrequencyEnum.Never;
                }
            }
        }

        public override void SaveSettings()
        {
            Settings.Default.SingleClickOnConnectionOpensIt = chkSingleClickOnConnectionOpensIt.Checked;
            Settings.Default.SingleClickSwitchesToOpenConnection = chkSingleClickOnOpenedConnectionSwitchesToIt.Checked;
            Settings.Default.TrackActiveConnectionInConnectionTree = chkConnectionTreeTrackActiveConnection.Checked;
            Settings.Default.SetHostnameLikeDisplayName = chkHostnameLikeDisplayName.Checked;

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

            Settings.Default.ConfirmCloseConnection = (int)comboBoxConnectionWarning.SelectedValue;
        }
    }
}