using mRemoteNG.Config;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Security;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class BackupPage
    {
        private readonly FrmMain _frmMain = FrmMain.Default;
        private List<DropdownList> _connectionBackup;

        public BackupPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.DocumentsFolder_16x);
        }

        public override string PageName
        {
            get => Language.strBackup;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            buttonBrowsePath.Text = Language.strBrowse;

            _connectionBackup = new List<DropdownList>
            {
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Never,
                        Language.ConnectionsBackupFrequencyNever)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.OnEdit,
                        Language.ConnectionsBackupFrequencyOnEdit)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.OnExit,
                        Language.ConnectionsBackupFrequencyOnExit)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Daily,
                        Language.ConnectionsBackupFrequencyDaily)
                },
                {
                    new DropdownList((int) ConnectionsBackupFrequencyEnum.Weekly,
                        Language.ConnectionsBackupFrequencyWeekly)
                },
                //{ new DropdownList( (int)ConnectionsBackupFrequencyEnum.Custom, Language.ConnectionsBackupFrequencyCustom)}
            };

            cmbConnectionBackupFrequency.DataSource = _connectionBackup;
            cmbConnectionBackupFrequency.DisplayMember = "DisplayString";
            cmbConnectionBackupFrequency.ValueMember = "Index";

            lblConnectionsBackupFrequency.Text = Language.strConnectionBackupFrequency;
            lblConnectionsBackupMaxCount.Text = Language.strConnectionsBackupMaxCount;
            lblConnectionsBackupPath.Text = Language.strConnectionsBackupPath;
        }

        public override void LoadSettings()
        {
            numMaxBackups.Value = Convert.ToDecimal(Properties.OptionsBackupPage.Default.BackupFileKeepCount);

            if (Properties.OptionsBackupPage.Default.SaveConnectionsFrequency == (int) ConnectionsBackupFrequencyEnum.Unassigned)
            {
                if (Properties.OptionsBackupPage.Default.SaveConnectionsAfterEveryEdit)
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.OnEdit;
                }
                else if (Properties.OptionsBackupPage.Default.SaveConsOnExit)
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.OnExit;
                }
                else
                {
                    Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) ConnectionsBackupFrequencyEnum.Never;
                }
            }

            cmbConnectionBackupFrequency.SelectedValue = Properties.OptionsBackupPage.Default.SaveConnectionsFrequency;
            textBoxConnectionBackupPath.Text = Properties.OptionsBackupPage.Default.CustomConsPath;

        }

        public override void SaveSettings()
        {
            Properties.OptionsBackupPage.Default.BackupFileKeepCount = (int) numMaxBackups.Value;

            if (Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes > 0)
            {
                _frmMain.tmrAutoSave.Interval = Properties.OptionsBackupPage.Default.AutoSaveEveryMinutes * 60000;
                _frmMain.tmrAutoSave.Enabled = true;
            }
            else
            {
                _frmMain.tmrAutoSave.Enabled = false;
            }

            Properties.OptionsBackupPage.Default.SaveConnectionsFrequency = (int) cmbConnectionBackupFrequency.SelectedValue;

            if (textBoxConnectionBackupPath.Text.Trim().Length <= 0)
            {
                Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = false;
                Properties.OptionsBackupPage.Default.CustomConsPath = String.Empty;
            }
            else
            {
                var newFileName = textBoxConnectionBackupPath.Text;

                Runtime.ConnectionsService.SaveConnections(Runtime.ConnectionsService.ConnectionTreeModel, false,
                    new SaveFilter(), newFileName);

                if (newFileName == Runtime.ConnectionsService.GetDefaultStartupConnectionFileName())
                {
                    Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = false;
                }
                else
                {
                    Properties.OptionsBackupPage.Default.LoadConsFromCustomLocation = true;
                    Properties.OptionsBackupPage.Default.CustomConsPath = newFileName;
                }
            }

            //Obsolete. Set to false
            Properties.OptionsBackupPage.Default.SaveConnectionsAfterEveryEdit = false;
            Properties.OptionsBackupPage.Default.SaveConsOnExit = false;
        }

        private void ButtonBrowsePath_Click(object sender, EventArgs e)
        {
            var saveDialog = DialogFactory.ConnectionsSaveAsDialog();

            var dialogResult = saveDialog.ShowDialog(this);

            textBoxConnectionBackupPath.Text = dialogResult == DialogResult.OK ? saveDialog.FileName : string.Empty;
        }
    }
}