using System;
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
        private OptRegistryUpdatesPage pageRegSettingsInstance;

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
            InitialiseCheckForUpdatesOnStartupComboBox();
            InitialiseReleaseChannelComboBox();

            // Checks updates are generaly disallowed
            if (UpdatesForbidden())
                return; // not required to continue

            chkCheckForUpdatesOnStartup.Checked = Properties.OptionsUpdatesPage.Default.CheckForUpdatesOnStartup;

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

            // Checks updates are generaly disallowed
            if (UpdatesForbidden())
                return; // not required to continue

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
            Type settingsType = typeof(OptRegistryUpdatesPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistryUpdatesPage;

            RegistryLoader.Cleanup(settingsType);

            // Checks updates are generaly disallowed.
            if (UpdatesForbidden())
            {
                DisableControl(chkCheckForUpdatesOnStartup);
                DisableControl(cboUpdateCheckFrequency);
                DisableControl(btnUpdateCheckNow);
                DisableControl(cboReleaseChannel);
                DisableControl(chkUseProxyForAutomaticUpdates);
                DisableControl(tblProxyBasic);
                DisableControl(txtProxyAddress);
                DisableControl(numProxyPort);
                DisableControl(chkUseProxyAuthentication);
                DisableControl(tblProxyAuthentication);
                DisableControl(txtProxyUsername);
                DisableControl(txtProxyPassword);
                DisableControl(btnTestProxy);

                lblRegistrySettingsUsedInfo.Visible = true;
                return; // not required to continue
            }

            // Disable the option "check for updates on startup" if auto updates is disallowed.
            if (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical)
            {
                DisableControl(chkCheckForUpdatesOnStartup);
                DisableControl(cboUpdateCheckFrequency);
            }

            // Set Update Check button state based on manual update allow state.
            btnUpdateCheckNow.Enabled = CommonRegistrySettings.AllowCheckForUpdatesManual;

            // Disable "CheckForUpdatesFrequencyDays" based on auto update allow state.
            if (pageRegSettingsInstance.CheckForUpdatesFrequencyDays.IsSet && CommonRegistrySettings.AllowCheckForUpdatesAutomatical)
            {
                DisableControl(chkCheckForUpdatesOnStartup);
                DisableControl(cboUpdateCheckFrequency);
            }

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.UpdateChannel.IsSet)
                DisableControl(cboReleaseChannel);

            if (pageRegSettingsInstance.UseProxyForUpdates.IsSet)
                DisableControl(chkUseProxyForAutomaticUpdates);

            if (pageRegSettingsInstance.ProxyAddress.IsSet)
                DisableControl(txtProxyAddress);

            if (pageRegSettingsInstance.ProxyPort.IsSet)
                DisableControl(numProxyPort);

            if (pageRegSettingsInstance.UseProxyAuthentication.IsSet)
                DisableControl(chkUseProxyAuthentication);

            if (pageRegSettingsInstance.ProxyAuthUser.IsSet)
                DisableControl(txtProxyUsername);

            if (pageRegSettingsInstance.ProxyAuthPass.IsSet)
                DisableControl(txtProxyPassword);

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        #endregion

        #region Event Handlers

        private void chkCheckForUpdatesOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (pageRegSettingsInstance?.CheckForUpdatesFrequencyDays?.IsSet == false)
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
            bool useProxyAuthRegIsSet = pageRegSettingsInstance.UseProxyAuthentication?.IsSet ?? false;
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
        /// Checks if updates are forbidden based on registry settings.
        /// </summary>
        private static bool UpdatesForbidden()
        {
            return !CommonRegistrySettings.AllowCheckForUpdates
                || (!CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                && !CommonRegistrySettings.AllowCheckForUpdatesManual);
        }

        /// <summary>
        /// Checks if specific registry settings related to updates page are used.
        /// </summary>
        private bool ShowRegistrySettingsUsedInfo()
        {
            return !CommonRegistrySettings.AllowCheckForUpdatesAutomatical
                || !CommonRegistrySettings.AllowCheckForUpdatesManual
                || pageRegSettingsInstance.CheckForUpdatesFrequencyDays.IsSet
                || pageRegSettingsInstance.UpdateChannel.IsSet
                || pageRegSettingsInstance.UseProxyForUpdates.IsSet
                || pageRegSettingsInstance.ProxyAddress.IsSet
                || pageRegSettingsInstance.ProxyPort.IsSet
                || pageRegSettingsInstance.UseProxyAuthentication.IsSet
                || pageRegSettingsInstance.ProxyAuthUser.IsSet
                || pageRegSettingsInstance.ProxyAuthPass.IsSet;
        }

        #endregion
    }
}