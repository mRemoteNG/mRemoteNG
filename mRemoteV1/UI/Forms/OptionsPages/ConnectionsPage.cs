using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class ConnectionsPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
        private List<DropdownList> _connectionWarning;
        private List<DropdownList> _connectionBackup;

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

            buttonBrowsePath.Text = Language.strBrowse;

            _connectionWarning = new List<DropdownList>
            {
                { new DropdownList( (int)ConfirmCloseEnum.Never, Language.ConnectionWarningNever)},
                { new DropdownList ((int)ConfirmCloseEnum.Exit, Language.ConnectionWarningExit)},
                { new DropdownList ((int)ConfirmCloseEnum.Multiple, Language.ConnectionWarningMultiple)},
                { new DropdownList ((int)ConfirmCloseEnum.All, Language.ConnectionWarningAll)}
            };

            comboBoxConnectionWarning.DataSource = _connectionWarning;
            comboBoxConnectionWarning.DisplayMember = "DisplayString";
            comboBoxConnectionWarning.ValueMember = "Index";

            _connectionBackup = new List<DropdownList>
            {
                { new DropdownList ((int)ConnectionsBackupFrequencyEnum.Never, Language.ConnectionsBackupFrequencyNever)}
                ,{ new DropdownList ((int)ConnectionsBackupFrequencyEnum.OnEdit, Language.ConnectionsBackupFrequencyOnEdit)}
                ,{ new DropdownList ((int)ConnectionsBackupFrequencyEnum.OnExit, Language.ConnectionsBackupFrequencyOnExit)}
                ,{ new DropdownList ((int)ConnectionsBackupFrequencyEnum.Daily, Language.ConnectionsBackupFrequencyDaily)}
                ,{ new DropdownList ((int)ConnectionsBackupFrequencyEnum.Weekly, Language.ConnectionsBackupFrequencyWeekly)}
                //,{ new DropdownList( (int)ConnectionsBackupFrequencyEnum.Custom, Language.ConnectionsBackupFrequencyCustom)}
            };

            cmbConnectionBackupFrequency.DataSource = _connectionBackup;
            cmbConnectionBackupFrequency.DisplayMember = "DisplayString";
            cmbConnectionBackupFrequency.ValueMember = "Index";

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
            lblConnectionsBackupFrequency.Text = Language.strConnectionBackupFrequency;
            lblConnectionsBackupMaxCount.Text = Language.strConnectionsBackupMaxCount;
            lblConnectionsBackupPath.Text = Language.strConnectionsBackupPath;

            lblClosingConnections.Text = Language.strLabelClosingConnections;

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

            numMaxBackups.Value = Convert.ToDecimal(Settings.Default.BackupFileKeepCount);

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

            cmbConnectionBackupFrequency.SelectedValue = Settings.Default.SaveConnectionsFrequency;
            textBoxConnectionBackupPath.Text = Settings.Default.CustomConsPath;

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
            Settings.Default.BackupFileKeepCount = (int)numMaxBackups.Value;

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
            Settings.Default.SaveConnectionsFrequency = (int)cmbConnectionBackupFrequency.SelectedValue;

            if (textBoxConnectionBackupPath.Text.Trim().Length <= 0)
            {
                Settings.Default.LoadConsFromCustomLocation = false;
                Settings.Default.CustomConsPath = String.Empty;
            }
            else
            {
                var newFileName = textBoxConnectionBackupPath.Text;

                Runtime.ConnectionsService.SaveConnections(Runtime.ConnectionsService.ConnectionTreeModel, false,
                    new SaveFilter(), newFileName);

                if (newFileName == Runtime.ConnectionsService.GetDefaultStartupConnectionFileName())
                {
                    Settings.Default.LoadConsFromCustomLocation = false;
                }
                else
                {
                    Settings.Default.LoadConsFromCustomLocation = true;
                    Settings.Default.CustomConsPath = newFileName;
                }
            }

            //Obsolete. Set to false
            Settings.Default.SaveConnectionsAfterEveryEdit = false;
            Settings.Default.SaveConsOnExit = false;
        }

        private void ButtonBrowsePath_Click(object sender, EventArgs e)
        {
            var saveDialog = DialogFactory.ConnectionsSaveAsDialog();

            var dialogResult = saveDialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                textBoxConnectionBackupPath.Text = saveDialog.FileName;
            }
            else
            {
                textBoxConnectionBackupPath.Text = String.Empty;
            }
        }
    }

    internal class DropdownList
    {
        public int Index { get; set; }
        public string DisplayString { get; set; }

        public DropdownList(int argIndex, string argDisplay)
        {
            Index = argIndex;
            DisplayString = argDisplay;
        }
    }
}