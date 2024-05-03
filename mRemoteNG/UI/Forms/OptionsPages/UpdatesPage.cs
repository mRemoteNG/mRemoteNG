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
using mRemoteNG.Config.Settings.Registry;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class UpdatesPage
    {
        #region Private Fields

        private AppUpdater _appUpdate;
        private readonly OptRegistryUpdatesPage _RegistrySettings = new();
        private bool _pageEnabled = true;

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

            lblUpdateAdminInfo.Text = Language.OptionsAdminInfo;
        }

        public override void LoadSettings()
        {
            if (UpdatesForbidden())
                return;

            chkCheckForUpdatesOnStartup.Checked = Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup;
            if (!_RegistrySettings.CheckForUpdatesFrequencyDays.IsKeyPresent)
                cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;

            if (CommonRegistrySettings.AllowCheckForUpdatesAutomatical)
            {
                cboUpdateCheckFrequency.Items.Clear();
                int nDaily = cboUpdateCheckFrequency.Items.Add(Language.Daily);
                int nWeekly = cboUpdateCheckFrequency.Items.Add(Language.Weekly);
                int nMonthly = cboUpdateCheckFrequency.Items.Add(Language.Monthly);
                if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays < 1)
                {
                    chkCheckForUpdatesOnStartup.Checked = false;
                    cboUpdateCheckFrequency.SelectedIndex = nDaily;
                } // Daily
                else
                {
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
                            int nCustom =
                                cboUpdateCheckFrequency.Items.Add(string.Format(Language.UpdateFrequencyCustom, Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays));
                            cboUpdateCheckFrequency.SelectedIndex = nCustom;
                            break;
                    }
                }
            }

            int stable = cboReleaseChannel.Items.Add(UpdateChannelInfo.STABLE);
            int beta = cboReleaseChannel.Items.Add(UpdateChannelInfo.PREVIEW);
            int dev = cboReleaseChannel.Items.Add(UpdateChannelInfo.NIGHTLY);
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
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            txtProxyPassword.Text =
                cryptographyProvider.Decrypt(Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass, Runtime.EncryptionKey);

            btnTestProxy.Enabled = Properties.OptionsUpdatesPage.Default.UpdateUseProxy;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            if (UpdatesForbidden())
                return;

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
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = cryptographyProvider.Encrypt(txtProxyPassword.Text, Runtime.EncryptionKey);
        }

        public override void LoadRegistrySettings()
        {
            if (UpdatesForbidden())
                return;

            if (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
                DisableControl(chkCheckForUpdatesOnStartup);
                DisableControl(cboUpdateCheckFrequency);
            }

            if (_RegistrySettings.CheckForUpdatesFrequencyDays.IsKeyPresent && _RegistrySettings.CheckForUpdatesFrequencyDays.Value >= 1)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = _RegistrySettings.CheckForUpdatesFrequencyDays.Value;
                DisableControl(cboUpdateCheckFrequency);
            }

            btnUpdateCheckNow.Enabled = CommonRegistrySettings.AllowCheckForUpdatesManual;

            if (_RegistrySettings.UpdateChannel.IsKeyPresent)
            {
                Properties.OptionsUpdatesPage.Default.UpdateChannel = _RegistrySettings.UpdateChannel.Value;
                DisableControl(cboReleaseChannel);
            }

            if (_RegistrySettings.UseProxyForUpdates.IsKeyPresent)
            {
                Properties.OptionsUpdatesPage.Default.UpdateUseProxy = _RegistrySettings.UseProxyForUpdates.Value;
                DisableControl(chkUseProxyForAutomaticUpdates);

                if (_RegistrySettings.UseProxyForUpdates.Value == false)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = "";
                    Properties.OptionsUpdatesPage.Default.UpdateProxyPort = 80;
                }

                if (_RegistrySettings.ProxyAddress.IsKeyPresent && _RegistrySettings.UseProxyForUpdates.Value == true)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = _RegistrySettings.ProxyAddress.Value;
                    DisableControl(txtProxyAddress);
                }

                if (_RegistrySettings.ProxyPort.IsKeyPresent && _RegistrySettings.UseProxyForUpdates.Value == true)
                {
                    // only set value if not is 0 to prevent errors..
                    if (_RegistrySettings.ProxyPort.Value >= 1)
                        Properties.OptionsUpdatesPage.Default.UpdateProxyPort = _RegistrySettings.ProxyPort.Value;

                    DisableControl(numProxyPort);
                }
            }

            if (_RegistrySettings.UseProxyAuthentication.IsKeyPresent)
            {
                Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = _RegistrySettings.UseProxyAuthentication.Value;
                DisableControl(chkUseProxyAuthentication);

                if (_RegistrySettings.UseProxyAuthentication.Value == false)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = "";
                    //Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = "";
                }

                if (_RegistrySettings.ProxyAuthUser.IsKeyPresent && _RegistrySettings.UseProxyAuthentication.Value == true)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = _RegistrySettings.ProxyAuthUser.Value;
                    DisableControl(txtProxyUsername);
                }

                /*if (_RegistrySettings.ProxyAuthPass.IsProvided && _RegistrySettings.UseProxyAuthentication.Value == true)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = _RegistrySettings.ProxyAuthPass;
                    DisableControl(txtProxyPassword);
                }*/
            }



            lblUpdateAdminInfo.Visible = ShowAdministratorInfo();
        }

        public override bool ShowAdministratorInfo()
        {
            return !CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                || !CommonRegistrySettings.AllowCheckForUpdatesManual
                || _RegistrySettings.CheckForUpdatesFrequencyDays.IsKeyPresent
                || _RegistrySettings.UpdateChannel.IsKeyPresent
                || _RegistrySettings.UseProxyForUpdates.IsKeyPresent
                || _RegistrySettings.ProxyAddress.IsKeyPresent
                || _RegistrySettings.ProxyPort.IsKeyPresent
                || _RegistrySettings.UseProxyAuthentication.IsKeyPresent
                || _RegistrySettings.ProxyAuthUser.IsKeyPresent;
        }

        #endregion

        #region Event Handlers

        private void chkCheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (!_RegistrySettings.CheckForUpdatesFrequencyDays.IsKeyPresent)
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
                if (!_RegistrySettings.UseProxyForUpdates.IsKeyPresent)
                    chkUseProxyAuthentication.Enabled = true;

                if (chkUseProxyAuthentication.Checked && !_RegistrySettings.UseProxyAuthentication.IsKeyPresent)
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

        private bool UpdatesForbidden()
        {
            bool forbidden = !CommonRegistrySettings.AllowCheckForUpdates
                || (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                && !CommonRegistrySettings.AllowCheckForUpdatesManual);

            if (forbidden && _pageEnabled)
                DisablePage();

            return forbidden;
        }

        public override void DisablePage()
        {
            chkCheckForUpdatesOnStartup.Checked = false;
            chkCheckForUpdatesOnStartup.Enabled = false;
            cboUpdateCheckFrequency.Enabled = false;
            btnUpdateCheckNow.Enabled = false;
            cboReleaseChannel.Enabled = false;

            chkUseProxyForAutomaticUpdates.Checked = false;
            chkUseProxyForAutomaticUpdates.Enabled = false;

            tblProxyBasic.Enabled = false;
            txtProxyAddress.Enabled = false;
            txtProxyAddress.ReadOnly = true;
            numProxyPort.Enabled = false;

            chkUseProxyAuthentication.Checked = false;
            chkUseProxyAuthentication.Enabled = false;

            tblProxyAuthentication.Enabled = false;
            txtProxyUsername.Enabled = false;
            txtProxyUsername.ReadOnly = true;
            txtProxyPassword.Enabled = false;
            txtProxyPassword.ReadOnly = true;
            btnTestProxy.Enabled = false;

            lblUpdateAdminInfo.Visible = true;

            _pageEnabled = false;
        }

        #endregion
    }
}