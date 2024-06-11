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

            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            // Checks the combination of the following registry settings:
            //   1. AllowCheckForUpdates is false.
            //   2. AllowCheckForUpdatesAutomatical and AllowCheckForUpdatesManual are false.
            //   3. Disable page and stop processing at this point.
            if (UpdatesForbidden())
                return;

            chkCheckForUpdatesOnStartup.Checked = Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup;

            InitialiseCheckForUpdatesOnStartupComboBox();
            InitialiseReleaseChannelComboBox();

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

            // Checks the combination of the following registry settings:
            //   1. AllowCheckForUpdates is false.
            //   2. AllowCheckForUpdatesAutomatical and AllowCheckForUpdatesManual are false.
            //   3. Disable page and stop processing at this point.
            if (UpdatesForbidden())
                return;

            Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = chkCheckForUpdatesOnStartup.Checked;

            string UpdateCheckFrequency = cboUpdateCheckFrequency.SelectedItem?.ToString();
            if (UpdateCheckFrequency == Language.Never)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
            }
            if (UpdateCheckFrequency == Language.Daily)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 1;
            }
            else if (UpdateCheckFrequency == Language.Weekly)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = 7;
            }
            else if (UpdateCheckFrequency == Language.Monthly)
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
            // Checks the combination of the following registry settings:
            //   1. AllowCheckForUpdates is false.
            //   2. AllowCheckForUpdatesAutomatical and AllowCheckForUpdatesManual are false.
            //   3. Disable page and stop processing at this point.
            if (UpdatesForbidden())
                return;

            // AllowCheckForUpdatesAutomatical reg setting:
            //  1. Allowed/default: true; Disabled: false.
            //  2. Disable the option "check for updates on startup".
            if (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical)
            {
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
                DisableControl(chkCheckForUpdatesOnStartup);
                DisableControl(cboUpdateCheckFrequency);
            }

            // CheckForUpdatesFrequencyDays reg setting:
            //  1. Is 0 or less, than CheckForUpdatesOnStartup will be disabled.
            //  2. Is Valid than set CheckForUpdatesFrequencyDays option based on value.
            if (CommonRegistrySettings.AllowCheckForUpdatesAutomatical && _RegistrySettings.CheckForUpdatesFrequencyDays.IsSet)
            {
                if (_RegistrySettings.CheckForUpdatesFrequencyDays.Value < 0)
                {
                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;
                    DisableControl(chkCheckForUpdatesOnStartup);
                }
                else if (_RegistrySettings.CheckForUpdatesFrequencyDays.IsValid)
                {
                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = true;
                    DisableControl(chkCheckForUpdatesOnStartup);

                    Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays = _RegistrySettings.CheckForUpdatesFrequencyDays.Value;
                    DisableControl(cboUpdateCheckFrequency);
                }
            }

            // Enable or disable the "Update Check" button if allowed or not.
            btnUpdateCheckNow.Enabled = CommonRegistrySettings.AllowCheckForUpdatesManual;

            // UpdateChannel reg setting: set UpdateChannel option based on value
            if (_RegistrySettings.UpdateChannel.IsValid)
            {
                Properties.OptionsUpdatesPage.Default.UpdateChannel = _RegistrySettings.UpdateChannel.Value;
                DisableControl(cboReleaseChannel);
            }

            // UseProxyForUpdates reg setting: set UseProxyForUpdates option based on value
            //  1. Continues with the options checks for "ProxyAddress" and "ProxyPort"
            //  2. Moved on to the "UseProxyAuthentication" options if true
            if (_RegistrySettings.UseProxyForUpdates.IsSet)
            {
                bool UseProxy = _RegistrySettings.UseProxyForUpdates.Value;
                Properties.OptionsUpdatesPage.Default.UpdateUseProxy = UseProxy;
                DisableControl(chkUseProxyForAutomaticUpdates);

                // If the proxy is not used, reset the proxy address, port, and authentication settings to defaults.
                if (!UseProxy)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = "";
                    Properties.OptionsUpdatesPage.Default.UpdateProxyPort = 80;
                    Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = false;
                }

                // ProxyAddress reg setting: set ProxyAddress option based on value
                if (_RegistrySettings.ProxyAddress.IsSet && UseProxy)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAddress = _RegistrySettings.ProxyAddress.Value;
                    DisableControl(txtProxyAddress);
                }

                // ProxyPort reg setting: set ProxyPort option based on value
                if (_RegistrySettings.ProxyPort.IsSet && UseProxy)
                {
                    _RegistrySettings.ProxyPort.SetValidation((int)numProxyPort.Minimum, (int)numProxyPort.Maximum);
                    if (_RegistrySettings.ProxyPort.IsValid)
                    {
                        Properties.OptionsUpdatesPage.Default.UpdateProxyPort = _RegistrySettings.ProxyPort.Value;
                        DisableControl(numProxyPort);
                    }
                }
            }

            // UseProxyAuthentication reg setting: set UseProxyAuthentication option based on value
            //  1. Only applied when UpdateUseProxy is true
            //  2. Continues with the options checks for "ProxyAuthUser" and "ProxyAuthPass"
            if (Properties.OptionsUpdatesPage.Default.UpdateUseProxy && _RegistrySettings.UseProxyAuthentication.IsSet)
            {
                bool UseProxyAuth = _RegistrySettings.UseProxyAuthentication.Value;
                Properties.OptionsUpdatesPage.Default.UpdateProxyUseAuthentication = UseProxyAuth;
                DisableControl(chkUseProxyAuthentication);

                // If proxy authentication is not used, reset the proxy authentication username and password to defaults.
                if (!UseProxyAuth)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = "";
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = "";
                }

                // ProxyAuthUser reg setting: set ProxyAuthUser option based on value
                if (_RegistrySettings.ProxyAuthUser.IsSet && UseProxyAuth)
                {
                    Properties.OptionsUpdatesPage.Default.UpdateProxyAuthUser = _RegistrySettings.ProxyAuthUser.Value;
                    DisableControl(txtProxyUsername);
                }

                // ProxyAuthPass reg setting:
                //  1. Test decription works to prevents potential issues
                //  2. Set ProxyAuthPass option based on value
                if (_RegistrySettings.ProxyAuthPass.IsSet && UseProxyAuth)
                {
                    // Prevents potential issues when using UpdateProxyAuthPass later.
                    try
                    {
                        LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                        string decryptedPassword;
                        string ProxyAuthPass = _RegistrySettings.ProxyAuthPass.Value;
                        decryptedPassword = cryptographyProvider.Decrypt(ProxyAuthPass, Runtime.EncryptionKey);

                        Properties.OptionsUpdatesPage.Default.UpdateProxyAuthPass = ProxyAuthPass;
                        DisableControl(txtProxyPassword);
                    }
                    catch
                    {
                        // Fire-and-forget: The password in the registry is not encrypted.
                    }
                }
            }

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        public override bool ShowRegistrySettingsUsedInfo()
        {
            return !CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                || !CommonRegistrySettings.AllowCheckForUpdatesManual
                || _RegistrySettings.CheckForUpdatesFrequencyDays.IsSet
                || _RegistrySettings.UpdateChannel.IsValid
                || _RegistrySettings.UseProxyForUpdates.IsSet
                || _RegistrySettings.ProxyAddress.IsSet
                || _RegistrySettings.ProxyPort.IsValid
                || _RegistrySettings.UseProxyAuthentication.IsSet
                || _RegistrySettings.ProxyAuthUser.IsSet
                || _RegistrySettings.ProxyAuthPass.IsSet;
        }

        #endregion

        #region Event Handlers

        private void chkCheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (!_RegistrySettings.CheckForUpdatesFrequencyDays.IsValid)
                cboUpdateCheckFrequency.Enabled = chkCheckForUpdatesOnStartup.Checked;

            InitialiseCheckForUpdatesOnStartupComboBox();
        }

        private void btnUpdateCheckNow_Click(object sender, EventArgs e)
        {
            Windows.Show(WindowType.Update);
        }

        private void chkUseProxyForAutomaticUpdates_CheckedChanged(object sender, EventArgs e)
        {
            // Enables/disables proxy settings controls
            bool useProxy = chkUseProxyForAutomaticUpdates.Checked;
            tblProxyBasic.Enabled = useProxy;
            btnTestProxy.Enabled = useProxy;

            // Enables/disables proxy authentication controls
            bool useProxyAuth = chkUseProxyAuthentication.Checked;
            bool useProxyAuthRegIsSet = _RegistrySettings.UseProxyAuthentication.IsSet;
            chkUseProxyAuthentication.Enabled = useProxy && !useProxyAuthRegIsSet;
            tblProxyAuthentication.Enabled = useProxy && useProxyAuth && !useProxyAuthRegIsSet;
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
                tblProxyAuthentication.Enabled = chkUseProxyAuthentication.Checked;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the update check frequency ComboBox.
        /// </summary>
        /// <remarks>
        /// Clears existing items, adds default options (Daily, Weekly, Monthly), and sets the selected option
        /// based on the saved frequency. If the saved frequency is less than 1, adds and selects "Never".
        /// </remarks>
        private void InitialiseCheckForUpdatesOnStartupComboBox()
        {
            cboUpdateCheckFrequency.Items.Clear();
            int nDaily = cboUpdateCheckFrequency.Items.Add(Language.Daily);
            int nWeekly = cboUpdateCheckFrequency.Items.Add(Language.Weekly);
            int nMonthly = cboUpdateCheckFrequency.Items.Add(Language.Monthly);
            if (Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays < 1)
            {
                chkCheckForUpdatesOnStartup.Checked = false;
                Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup = false;

                int nNever = cboUpdateCheckFrequency.Items.Add(Language.Never);
                cboUpdateCheckFrequency.SelectedIndex = nNever;
            }
            else if (!chkCheckForUpdatesOnStartup.Checked)
            {
                int nNever = cboUpdateCheckFrequency.Items.Add(Language.Never);
                cboUpdateCheckFrequency.SelectedIndex = nNever;
            }
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


        /// <summary>
        /// Initializes the release channel ComboBox
        /// </summary>
        /// <remarks>
        /// Set available options (STABLE, PREVIEW, NIGHTLY) and selects the appropriate channel based on saved settings
        /// </remarks>
        private void InitialiseReleaseChannelComboBox()
        {
            cboReleaseChannel.Items.Clear();
            int stable = cboReleaseChannel.Items.Add(UpdateChannelInfo.STABLE);
            int beta = cboReleaseChannel.Items.Add(UpdateChannelInfo.PREVIEW);
            int dev = cboReleaseChannel.Items.Add(UpdateChannelInfo.NIGHTLY);
            cboReleaseChannel.SelectedIndex = Properties.OptionsUpdatesPage.Default.UpdateChannel switch
            {
                UpdateChannelInfo.STABLE => stable,
                UpdateChannelInfo.PREVIEW => beta,
                UpdateChannelInfo.NIGHTLY => dev,
                _ => stable,
            };
        }

        /// <summary>
        /// Determines if updates are forbidden based on registry settings.
        /// </summary>
        /// <returns>
        /// True if updates are forbidden; otherwise, false.
        /// </returns>
        private bool UpdatesForbidden()
        {
            bool disablePage = !CommonRegistrySettings.AllowCheckForUpdates
                || (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                && !CommonRegistrySettings.AllowCheckForUpdatesManual);

            if (disablePage)
            {
                DisablePage();

                // Ensure the UI (CheckFrequency ComboBox)  appears correct when disabled
                cboUpdateCheckFrequency.Items.Clear();
                int nNever = cboUpdateCheckFrequency.Items.Add(Language.Never);
                cboUpdateCheckFrequency.SelectedIndex = nNever;

                // Ensure the UI (ReleaseChannel ComboBox) appears correct when disabled
                cboReleaseChannel.Items.Clear();
                int stable = cboReleaseChannel.Items.Add(UpdateChannelInfo.STABLE);
                cboReleaseChannel.SelectedIndex = stable;
            }

            return disablePage;
        }

        /// <summary>
        /// Disables all controls on the page related to update settings and proxy configurations.
        /// </summary>
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

            lblRegistrySettingsUsedInfo.Visible = true;
        }

        #endregion
    }
}