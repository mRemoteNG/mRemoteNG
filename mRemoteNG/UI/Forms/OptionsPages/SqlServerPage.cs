using System;
using System.Globalization;
using mRemoteNG.App;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Properties;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Resources.Language;
using System.Drawing;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.UI.Controls;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class SqlServerPage
    {
        #region Private Fields
        private OptRegistrySqlServerPage pageRegSettingsInstance;
        private NumericUpDown numSQLReloadInterval;
        private MrngLabel lblSQLReloadInterval;

        private static readonly (string TypeKey, string DisplayName)[] SqlTypeOptions =
        {
            (DatabaseConnectorFactory.MsSqlType, "MSSQL - developed by Microsoft"),
            (DatabaseConnectorFactory.MySqlType, "MySQL - developed by Oracle"),
            (DatabaseConnectorFactory.OdbcType, "ODBC - Open Database Connectivity")
        };
        #endregion

        public SqlServerPage()
        {
            InitializeComponent();
            InitializeSqlTypeSelector();
            InitializeReloadIntervalControl();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.SQLDatabase_16x);
            pageRegSettingsInstance = new OptRegistrySqlServerPage(); // Initialize the field to avoid nullability issues

            // Event subscriptions for Profile Management
            btnLoadProfile.Click += btnLoadProfile_Click;
            btnSaveProfile.Click += btnSaveProfile_Click;
            btnDeleteProfile.Click += btnDeleteProfile_Click;
            lstProfiles.SelectedIndexChanged += lstProfiles_SelectedIndexChanged;
            LoadProfilesList();
        }

        private void InitializeReloadIntervalControl()
        {
            lblSQLReloadInterval = new MrngLabel();
            lblSQLReloadInterval.Dock = DockStyle.Fill;
            lblSQLReloadInterval.TextAlign = ContentAlignment.MiddleRight;
            lblSQLReloadInterval.Text = "Reload interval (s):";

            numSQLReloadInterval = new NumericUpDown();
            numSQLReloadInterval.Minimum = 5;
            numSQLReloadInterval.Maximum = 3600;
            numSQLReloadInterval.Value = 30;
            numSQLReloadInterval.Dock = DockStyle.Fill;

            pnlSQLCon.Controls.Add(lblSQLReloadInterval, 0, 6);
            pnlSQLCon.Controls.Add(numSQLReloadInterval, 1, 6);
        }

        public override string PageName
        {
            get => Language.SQLServer.TrimEnd(':');
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            //lblExperimental.Text = Language.Experimental.ToUpper();
            //lblSQLInfo.Text = Language.SQLInfo;

            chkUseSQLServer.Text = Language.UseSQLServer;
            //lblSQLServer.Text = Language.Hostname;
            lblSQLDatabaseName.Text = Language.Database;
            lblSQLUsername.Text = Language.Username;
            lblSQLPassword.Text = Language.Password;
            lblSQLReadOnly.Text = Language.ReadOnly;
            lblShowDatabasePickerOnStartup.Text = "Show picker on startup:";
            btnTestConnection.Text = Language.TestConnection;
            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkUseSQLServer.Checked = Properties.OptionsDBsPage.Default.UseSQLServer;
            txtSQLType.Text = GetSqlTypeDisplayName(Properties.OptionsDBsPage.Default.SQLServerType);
            txtSQLServer.Text = Properties.OptionsDBsPage.Default.SQLHost;
            txtSQLDatabaseName.Text = Properties.OptionsDBsPage.Default.SQLDatabaseName;
            txtSQLUsername.Text = Properties.OptionsDBsPage.Default.SQLUser;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            txtSQLPassword.Text = cryptographyProvider.Decrypt(Properties.OptionsDBsPage.Default.SQLPass, Runtime.EncryptionKey);
            chkSQLReadOnly.Checked = Properties.OptionsDBsPage.Default.SQLReadOnly;
            chkShowDatabasePickerOnStartup.Checked = Properties.OptionsDBsPage.Default.ShowDatabasePickerOnStartup;

            string savedAuthType = Properties.OptionsDBsPage.Default.SQLAuthType;
            int authIndex = txtSQLAuthType.FindStringExact(savedAuthType);
            txtSQLAuthType.SelectedIndex = authIndex >= 0 ? authIndex : 0;

            lblTestConnectionResults.Text = "";
        }

        public override void SaveSettings()
        {
            base.SaveSettings();
            bool sqlServerWasPreviouslyEnabled = Properties.OptionsDBsPage.Default.UseSQLServer;

            Properties.OptionsDBsPage.Default.UseSQLServer = chkUseSQLServer.Checked;
            Properties.OptionsDBsPage.Default.SQLServerType = DatabaseConnectorFactory.NormalizeType(txtSQLType.Text);
            Properties.OptionsDBsPage.Default.SQLHost = txtSQLServer.Text;
            Properties.OptionsDBsPage.Default.SQLDatabaseName = txtSQLDatabaseName.Text;
            Properties.OptionsDBsPage.Default.SQLUser = txtSQLUsername.Text;
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            Properties.OptionsDBsPage.Default.SQLPass = cryptographyProvider.Encrypt(txtSQLPassword.Text, Runtime.EncryptionKey);
            Properties.OptionsDBsPage.Default.SQLReadOnly = chkSQLReadOnly.Checked;
            Properties.OptionsDBsPage.Default.ShowDatabasePickerOnStartup = chkShowDatabasePickerOnStartup.Checked;
            Properties.OptionsDBsPage.Default.SQLAuthType = txtSQLAuthType.SelectedItem?.ToString() ?? "Windows Authentication";

            if (Properties.OptionsDBsPage.Default.UseSQLServer)
            {
                try
                {
                    ReinitializeSqlUpdater();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Language.ErrorConnectionListSaveFailed + Environment.NewLine + ex.Message, Language.Errors, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!Properties.OptionsDBsPage.Default.UseSQLServer && sqlServerWasPreviouslyEnabled)
                DisableSql();
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistrySqlServerPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);

            // Ensure settings is not null before assignment
            if (settings is OptRegistrySqlServerPage registrySettings)
            {
                pageRegSettingsInstance = registrySettings;
            }
            else
            {
                pageRegSettingsInstance = new OptRegistrySqlServerPage(); // Initialize with a new instance instead of null
                return; // Exit early if settings is null
            }

            RegistryLoader.Cleanup(settingsType);

            // Skip validation of SQL Server registry settings if not set in the registry.
            if (!pageRegSettingsInstance.UseSQLServer.IsSet)
                return;

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = true;
            DisableControl(chkUseSQLServer);

            // End validation of SQL Server registry settings if UseSQLServer is false.
            if (!Properties.OptionsDBsPage.Default.UseSQLServer)
                return;

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.SQLServerType.IsSet)
                DisableControl(txtSQLType);

            if (pageRegSettingsInstance.SQLHost.IsSet)
                DisableControl(txtSQLServer);

            if (pageRegSettingsInstance.SQLDatabaseName.IsSet)
                DisableControl(txtSQLDatabaseName);

            if (pageRegSettingsInstance.SQLUser.IsSet)
                DisableControl(txtSQLUsername);

            if (pageRegSettingsInstance.SQLPassword.IsSet)
                DisableControl(txtSQLPassword);

            if (pageRegSettingsInstance.SQLReadOnly.IsSet)
                DisableControl(chkSQLReadOnly);
        }

        private static void ReinitializeSqlUpdater()
        {
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Dispose();
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
            Runtime.ConnectionsService.LoadConnections(true, false, "");
        }

        private static void DisableSql()
        {
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer?.Dispose();
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = null!;
            Runtime.LoadConnections(true);
        }

        private void InitializeSqlTypeSelector()
        {
            txtSQLType.Items.Clear();
            txtSQLType.Items.AddRange(SqlTypeOptions.Select(option => option.DisplayName).Cast<object>().ToArray());
        }

        private static string GetSqlTypeDisplayName(string sqlType)
        {
            string normalizedType = DatabaseConnectorFactory.NormalizeType(sqlType);
            foreach ((string typeKey, string displayName) in SqlTypeOptions)
            {
                if (string.Equals(typeKey, normalizedType, StringComparison.OrdinalIgnoreCase))
                    return displayName;
            }

            return SqlTypeOptions[0].DisplayName;
        }

        private void chkUseSQLServer_CheckedChanged(object sender, EventArgs e)
        {
            toggleSQLPageControls(chkUseSQLServer.Checked);
        }

        /// <summary>
        /// Enable or disable SQL connection page controls based on SQL server settings availability.
        /// Controls are enabled if corresponding registry settings are not set, allowing user interaction
        /// when SQL server usage is enabled.
        /// </summary>
        /// <param name="useSQLServer">Flag indicating whether SQL server functionality is enabled.</param>
        private void toggleSQLPageControls(bool useSQLServer)
        {
            if (!chkUseSQLServer.Enabled) return;
            pnlServerBlock.Visible = useSQLServer;
            pnlServerBlock.Enabled = useSQLServer;
        }

        private void btnExpandOptions_Click(object sender, EventArgs e)
        {
            if (string.Equals(btnExpandOptions.Text, "Advanced >>", StringComparison.Ordinal))
            {
                btnExpandOptions.Text = "<< Simple";
                tabCtrlSQL.Visible = true;
            }
            else
            {
                btnExpandOptions.Text = "Advanced >>";
                tabCtrlSQL.Visible = false;
            }
        }

        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            string type = DatabaseConnectorFactory.NormalizeType(txtSQLType.Text);
            string server = txtSQLServer.Text;
            string database = txtSQLDatabaseName.Text;
            string username = txtSQLUsername.Text;
            string password = txtSQLPassword.Text;
            string authType = txtSQLAuthType.SelectedItem?.ToString() ?? DatabaseConnectorFactory.WindowsAuthentication;

            lblTestConnectionResults.Text = Language.TestingConnection;
            imgConnectionStatus.Image = Properties.Resources.Loading_Spinner;
            btnTestConnection.Enabled = false;

            ConnectionTestResult connectionTestResult = await DatabaseConnectionTester.TestConnectivity(type, server, database, username, password, authType);

            btnTestConnection.Enabled = true;

            switch (connectionTestResult)
            {
                case ConnectionTestResult.ConnectionSucceded:
                    UpdateConnectionImage(true);
                    lblTestConnectionResults.Text = Language.ConnectionSuccessful;
                    break;
                case ConnectionTestResult.ServerNotAccessible:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text =
                        BuildTestFailedMessage(string.Format(CultureInfo.CurrentCulture, Language.ServerNotAccessible, server));
                    break;
                case ConnectionTestResult.CredentialsRejected:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text =
                        BuildTestFailedMessage(string.Format(CultureInfo.CurrentCulture, Language.LoginFailedForUser, username));
                    break;
                case ConnectionTestResult.UnknownDatabase:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text =
                        BuildTestFailedMessage(string.Format(CultureInfo.CurrentCulture, Language.DatabaseNotAvailable, database));
                    break;
                case ConnectionTestResult.UnknownError:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(Language.RdpErrorUnknown);
                    break;
                default:
                    UpdateConnectionImage(false);
                    lblTestConnectionResults.Text = BuildTestFailedMessage(Language.RdpErrorUnknown);
                    break;
            }
        }

        private void UpdateConnectionImage(bool connectionSuccess)
        {
            imgConnectionStatus.Image = connectionSuccess ? Properties.Resources.Test_16x : Properties.Resources.LogError_16x;
        }

        private static string BuildTestFailedMessage(string specificMessage)
        {
            return Language.ConnectionOpenFailed + Environment.NewLine + specificMessage;
        }

        private void txtSQLAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure SelectedItem is not null before accessing it
            if (txtSQLAuthType.SelectedItem != null)
            {
                // Get the selected value
                string? selectedValue = txtSQLAuthType.SelectedItem.ToString();

                // Check the selected value and call appropriate action
                if (string.Equals(selectedValue, "Windows Authentication", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User name:";
                    lblSQLUsername.Enabled = false;
                    txtSQLUsername.Enabled = false;
                    txtSQLUsername.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    lblSQLPassword.Visible = false;
                    txtSQLPassword.Visible = false;
                }
                else if (string.Equals(selectedValue, "SQL Server Authentication", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "login:";
                    lblSQLUsername.Enabled = true;
                    txtSQLUsername.Enabled = true;
                    txtSQLUsername.Text = "";
                    lblSQLPassword.Visible = true;
                    txtSQLPassword.Visible = true;
                }
                else if (string.Equals(selectedValue, "Microsoft Entra MFA", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User name:";
                    lblSQLUsername.Enabled = true;
                    txtSQLUsername.Enabled = true;
                    txtSQLUsername.Text = "";
                    lblSQLPassword.Visible = false;
                    txtSQLPassword.Visible = false;
                }
                else if (string.Equals(selectedValue, "Microsoft Entra Password", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User name:";
                    lblSQLUsername.Enabled = true;
                    txtSQLUsername.Enabled = true;
                    txtSQLUsername.Text = "";
                    lblSQLPassword.Visible = true;
                    txtSQLPassword.Visible = true;
                }
                else if (string.Equals(selectedValue, "Microsoft Entra Integrated", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User name:";
                    lblSQLUsername.Enabled = false;
                    txtSQLUsername.Enabled = false;
                    txtSQLUsername.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    lblSQLPassword.Visible = false;
                    txtSQLPassword.Visible = false;
                }
                else if (string.Equals(selectedValue, "Microsoft Entra Service Principal", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User name:";
                    lblSQLUsername.Enabled = true;
                    txtSQLUsername.Enabled = true;
                    txtSQLUsername.Text = "";
                    lblSQLPassword.Visible = true;
                    txtSQLPassword.Visible = true;
                }
                else if (string.Equals(selectedValue, "Microsoft Entra Managed Identity", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User assigned identity:";
                    lblSQLUsername.Enabled = true;
                    txtSQLUsername.Enabled = true;
                    txtSQLUsername.Text = "";
                    lblSQLPassword.Visible = false;
                    txtSQLPassword.Visible = false;
                }
                else if (string.Equals(selectedValue, "Microsoft Entra Default", StringComparison.Ordinal))
                {
                    lblSQLUsername.Text = "User name:";
                    lblSQLUsername.Enabled = true;
                    txtSQLUsername.Enabled = true;
                    txtSQLUsername.Text = "";
                    lblSQLPassword.Visible = false;
                    txtSQLPassword.Visible = false;
                }
                else
                {
                    // Handle other values or do nothing
                    Console.WriteLine("No matching option.");
                }
            }
        }

        private void DCMSetupRdBtnV_CheckedChanged(object sender, EventArgs e)
        {
            if (DCMSetupRdBtnV.Checked)
            {
                DCMSetuplbluser.Visible = false;
                DCMSetuptxtuser.Visible = false;
                DCMSetuplbluserpwd.Visible = false;
                DCMSetuptxtuserpwd.Visible = false;
            }
        }

        private void DCMSetupRdBtnC_CheckedChanged(object sender, EventArgs e)
        {
            if (DCMSetupRdBtnC.Checked)
            {
                DCMSetuplbluser.Visible = true;
                DCMSetuptxtuser.Visible = true;
                DCMSetuplbluserpwd.Visible = true;
                DCMSetuptxtuserpwd.Visible = true;
            }
        }

        private void SqlServerPage_Load(object sender, EventArgs e)
        {
            // Initial load
            RefreshSchemaFiles();

            // Attach the DropDown event handler
            DCMSetupddschema.DropDown += DCMSetupddschema_DropDown;
        }

        private void DCMSetupddschema_DropDown(object? sender, EventArgs e)
        {
            // Refresh files each time dropdown is opened
            RefreshSchemaFiles();
        }

        private void RefreshSchemaFiles()
        {
            try
            {
                // Store the currently selected item
                string? currentSelection = DCMSetupddschema.SelectedValue?.ToString();

                // Get the application's running directory
                string schemasFolder = Path.Combine(Application.StartupPath, "Schemas");

                // Check if Schemas folder exists
                if (!Directory.Exists(schemasFolder))
                {
                    DCMSetupddschema.DataSource = null;
                    DCMSetupddschema.Items.Clear();
                    DCMSetupddschema.Items.Add("Schemas folder not found");
                    return;
                }

                // Get all files matching the pattern
                var schemaFiles = Directory.GetFiles(schemasFolder, "mremoteng_confcons_*.xsd");

                if (schemaFiles.Length == 0)
                {
                    DCMSetupddschema.DataSource = null;
                    DCMSetupddschema.Items.Clear();
                    DCMSetupddschema.Items.Add("No schema files found");
                    return;
                }

                // Extract version numbers and sort
                var filesWithVersions = schemaFiles
                    .Select(file => new
                    {
                        FilePath = file,
                        FileName = Path.GetFileName(file),
                        Version = ExtractVersionNumber(file)
                    })
                    .OrderByDescending(x => x.Version)
                    .ToList();

                // Add files to ComboBox
                DCMSetupddschema.DataSource = filesWithVersions;
                DCMSetupddschema.DisplayMember = "FileName";
                DCMSetupddschema.ValueMember = "FilePath";

                // Try to restore the previous selection if it still exists
                if (!string.IsNullOrEmpty(currentSelection))
                {
                    var itemToSelect = filesWithVersions.FirstOrDefault(x => x.FilePath == currentSelection);
                    if (itemToSelect != null)
                    {
                        DCMSetupddschema.SelectedValue = itemToSelect.FilePath;
                    }
                    else if (filesWithVersions.Count > 0)
                    {
                        // Select the highest version if previous selection no longer exists
                        DCMSetupddschema.SelectedIndex = 0;
                    }
                }
                else if (filesWithVersions.Count > 0)
                {
                    // Select the highest version if there was no previous selection
                    DCMSetupddschema.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                DCMSetupddschema.DataSource = null;
                DCMSetupddschema.Items.Clear();
                DCMSetupddschema.Items.Add($"Error: {ex.Message}");
            }
        }

        private static Version ExtractVersionNumber(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            var match = Regex.Match(fileName, @"mremoteng_confcons_v(\d+)_(\d+)");

            if (match.Success && match.Groups.Count == 3)
            {
                int major = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                int minor = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                return new Version(major, minor);
            }

            return new Version(0, 0);
        }

        #region Profile Management

        private void LoadProfilesList()
        {
            lstProfiles.Items.Clear();
            foreach (var profile in DatabaseProfileManager.Profiles)
            {
                lstProfiles.Items.Add(profile);
            }
            lstProfiles.DisplayMember = "Name";
        }

        private void lstProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProfiles.SelectedItem is DatabaseProfile profile)
            {
                txtProfileName.Text = profile.Name;
            }
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            if (lstProfiles.SelectedItem is DatabaseProfile profile)
            {
                DatabaseProfileManager.ApplyProfileToSettings(profile);
                LoadSettings(); // Refresh UI with new settings
                MessageBox.Show($"Profile '{profile.Name}' loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a profile to load.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            string profileName = txtProfileName.Text.Trim();
            if (string.IsNullOrEmpty(profileName))
            {
                MessageBox.Show("Please enter a profile name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save current settings to properties first (handled by SaveSettings call usually, but here we read from UI controls)
            // But wait, SaveSettings() persists to properties. We should probably update properties from UI first before creating profile?
            // Or just read from UI controls directly.
            
            // To be safe and consistent, let's update properties from UI controls first (in-memory, not necessarily saved to disk yet)
            // But SaveSettings() method does exactly this.
            // However, SaveSettings() is 'override' and called by the Options dialog logic when OK/Apply is clicked.
            // Here we want to capture current UI state into a profile.
            
            // Let's manually create profile from UI controls to avoid modifying global settings if user cancels.
            // Actually, the requirement is "Pick database on logon".
            // So saving a profile should probably just save the current configuration on screen.
            
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            var profile = new DatabaseProfile
            {
                Name = profileName,
                Type = DatabaseConnectorFactory.NormalizeType(txtSQLType.Text),
                Host = txtSQLServer.Text,
                DatabaseName = txtSQLDatabaseName.Text,
                Username = txtSQLUsername.Text,
                EncryptedPassword = cryptographyProvider.Encrypt(txtSQLPassword.Text, Runtime.EncryptionKey),
                ReadOnly = chkSQLReadOnly.Checked,
                AuthType = txtSQLAuthType.SelectedItem?.ToString() ?? "Windows Authentication"
            };

            DatabaseProfileManager.AddProfile(profile);
            LoadProfilesList();
            MessageBox.Show($"Profile '{profileName}' saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (lstProfiles.SelectedItem is DatabaseProfile profile)
            {
                if (MessageBox.Show($"Are you sure you want to delete profile '{profile.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DatabaseProfileManager.RemoveProfile(profile.Name);
                    LoadProfilesList();
                    txtProfileName.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please select a profile to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}