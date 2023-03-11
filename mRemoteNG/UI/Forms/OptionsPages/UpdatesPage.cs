using System;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.App.Update;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class UpdatesPage
    {
        #region Private Fields

        private AppUpdater _appUpdate;

        #endregion

        public UpdatesPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.RunUpdate_16x);
        }

        #region Public Methods

        public override string PageName
        {
            get => Language.Updates;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblUpdatesExplanation.Text = Language.UpdateCheck;

            chkCheckForUpdatesOnStartup.Text = Language.CheckForUpdatesOnStartup;
            btnUpdateCheckNow.Text = Language.CheckNow;

            groupBoxReleaseChannel.Text = Language.ReleaseChannel;
            lblReleaseChannelExplanation.Text = Language.ReleaseChannelExplanation;

            chkUseProxyForAutomaticUpdates.Text = Language.CheckboxUpdateUseProxy;
            lblProxyAddress.Text = Language.Address;
            lblProxyPort.Text = Language.Port;

            chkUseProxyAuthentication.Text = Language.CheckboxProxyAuthentication;
            lblProxyUsername.Text = Language.Username;
            lblProxyPassword.Text = Language.Password;

            btnTestProxy.Text = Language.TestProxy;
        }

        public override void LoadSettings()
        {
            chkCheckForUpdatesOnStartup.Checked = Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup;
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;
            cboUpdateCheckFrequency.Items.Clear();
            var nDaily = cboUpdateCheckFrequency.Items.Add(Language.Daily);
            var nWeekly = cboUpdateCheckFrequency.Items.Add(Language.Weekly);
            var nMonthly = cboUpdateCheckFrequency.Items.Add(Language.Monthly);
            if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays < 1)
            {
                chkCheckForUpdatesOnStartup.Checked = false;
                cboUpdateCheckFrequency.SelectedIndex = nDaily;
            } // Daily
            else
                switch (Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays)
                {
                    case 1:
                        cboUpdateCheckFrequency.SelectedIndex = nDaily;
                        break;
                    case 7:
                        cboUpdateCheckFrequency.SelectedIndex = nWeekly;
                        break;
                    case 31:
                        cboUpdateCheckFrequency.SelectedIndex = nMonthly;
                        break;
                    default:
                        var nCustom =
                            cboUpdateCheckFrequency.Items.Add(string.Format(Language.UpdateFrequencyCustom, Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays));
                        cboUpdateCheckFrequency.SelectedIndex = nCustom;
                        break;
                }

            var stable = cboReleaseChannel.Items.Add(UpdateChannelInfo.STABLE);
            var beta = cboReleaseChannel.Items.Add(UpdateChannelInfo.PREVIEW);
            var dev = cboReleaseChannel.Items.Add(UpdateChannelInfo.NIGHTLY);
            switch (Properties.OptionsUpdatesPage.Default.UpdateChannel)
            {
                case UpdateChannelInfo.STABLE:
                    cboReleaseChannel.SelectedIndex = stable;
                    break;
                case UpdateChannelInfo.PREVIEW:
                    cboReleaseChannel.SelectedIndex = beta;
                    break;
                case UpdateChannelInfo.NIGHTLY:
                    cboReleaseChannel.SelectedIndex = dev;
                    break;
                default:
                    cboReleaseChannel.SelectedIndex = stable;
                    break;
            }

            chkUseProxyForAutomaticUpdates.Checked = Properties.OptionsUpdatesPage.Default.UpdateUseProxy;
            tblProxyBasic.Enabled = Properties.OptionsUpdatesPage.Default.UpdateUseProxy;
            txtProxyAddress.Text = Properties.OptionsUpdatesPage.Default.UpdateProxyAddress;
            numProxyPort.Value = Convert.ToDecimal(Properties.OptionsUpdatesPage.Default.UpdateProxyPort);

            chkUseProxyAuthentication.Checked = Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication;
            tblProxyAuthentication.Enabled = Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication;
            txtProxyUsername.Text = Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            txtProxyPassword.Text =
                cryptographyProvider.Decrypt(Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass, Runtime.EncryptionKey);

            btnTestProxy.Enabled = Properties.OptionsUpdatesPage.Default.UpdateUseProxy;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked;
            if (cboUpdateCheckFrequency.SelectedItem.ToString() == Language.Daily)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 1;
            }
            else if (cboUpdateCheckFrequency.SelectedItem.ToString() == Language.Weekly)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 7;
            }
            else if (cboUpdateCheckFrequency.SelectedItem.ToString() == Language.Monthly)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 31;
            }

            Properties.OptionsUpdatesPage.Default.UpdateChannel = cboReleaseChannel.Text;

            Properties.OptionsUpdatesPage.Default.UpdateUseProxy = chkUseProxyForAutomaticUpdates.Checked;
            Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = txtProxyAddress.Text;
            Properties.OptionsUpdatesPage.Default.UpdateProxyPort = (int)numProxyPort.Value;

            Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = chkUseProxyAuthentication.Checked;
            Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = txtProxyUsername.Text;
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = cryptographyProvider.Encrypt(txtProxyPassword.Text, Runtime.EncryptionKey);
        }

        #endregion

        #region Event Handlers

        private void chkCheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;
        }

        private void btnUpdateCheckNow_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.Update);
        }

        private void chkUseProxyForAutomaticUpdates_CheckedChanged(object sender, EventArgs e)
        {
            tblProxyBasic.Enabled = chkUseProxyForAutomaticUpdates.Checked;
            btnTestProxy.Enabled = chkUseProxyForAutomaticUpdates.Checked;

            if (chkUseProxyForAutomaticUpdates.Checked)
            {
                chkUseProxyAuthentication.Enabled = true;

                if (chkUseProxyAuthentication.Checked)
                {
                    tblProxyAuthentication.Enabled = true;
                }
            }
            else
            {
                chkUseProxyAuthentication.Enabled = false;
                tblProxyAuthentication.Enabled = false;
            }
        }

        private async void btnTestProxy_Click(object sender, EventArgs e)
        {
            if (_appUpdate != null)
            {
                if (_appUpdate.IsGetUpdateInfoRunning)
                {
                    return;
                }
            }

            _appUpdate = new AppUpdater();
            //_appUpdate.Load += _appUpdate.Update_Load;
            _appUpdate.SetProxySettings(chkUseProxyForAutomaticUpdates.Checked, txtProxyAddress.Text,
                                        (int)numProxyPort.Value, chkUseProxyAuthentication.Checked,
                                        txtProxyUsername.Text,
                                        txtProxyPassword.Text);
            try
            {
                btnTestProxy.Enabled = false;
                btnTestProxy.Text = Language.OptionsProxyTesting;

                await _appUpdate.GetUpdateInfoAsync();

                btnTestProxy.Enabled = true;
                btnTestProxy.Text = Language.TestProxy;
                CTaskDialog.ShowCommandBox(this, Application.ProductName, Language.ProxyTestSucceeded, "",
                    Language._Ok, false);
            }
            catch (Exception ex)
            {
                CTaskDialog.ShowCommandBox(this, Application.ProductName, Language.ProxyTestFailed,
                    MiscTools.GetExceptionMessageRecursive(ex), "", "", "", Language._Ok,
                    false,
                    ESysIcons.Error,
                    0);
            }

        }

        private void chkUseProxyAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseProxyForAutomaticUpdates.Checked)
            {
                tblProxyAuthentication.Enabled = chkUseProxyAuthentication.Checked;
            }
        }

        #endregion
    }
}