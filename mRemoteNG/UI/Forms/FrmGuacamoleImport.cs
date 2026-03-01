using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Container;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public class FrmGuacamoleImport : Form
    {
        private ContainerInfo _destinationContainer;
        private ComboBox cmbType;
        private TextBox txtHost;
        private TextBox txtPort;
        private TextBox txtDatabase;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnImport;
        private Button btnCancel;

        public FrmGuacamoleImport(ContainerInfo destinationContainer)
        {
            _destinationContainer = destinationContainer;
            InitializeComponent();
            ApplyTheme();
            ApplyLanguage();
        }

        private void InitializeComponent()
        {
            this.Text = "Import from Guacamole";
            this.Size = new Size(400, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.Padding = new Padding(10);
            table.ColumnCount = 2;
            table.RowCount = 7;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Type
            table.Controls.Add(new Label { Text = "Database Type:", AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Right }, 0, 0);
            cmbType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbType.Items.Add("MySQL");
            cmbType.Items.Add("SQL Server");
            cmbType.SelectedIndex = 0;
            cmbType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            table.Controls.Add(cmbType, 1, 0);

            // Host
            table.Controls.Add(new Label { Text = "Host:", AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Right }, 0, 1);
            txtHost = new TextBox { Text = "localhost", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(txtHost, 1, 1);

            // Port
            table.Controls.Add(new Label { Text = "Port:", AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Right }, 0, 2);
            txtPort = new TextBox { Text = "3306", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(txtPort, 1, 2);

            // Database
            table.Controls.Add(new Label { Text = "Database Name:", AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Right }, 0, 3);
            txtDatabase = new TextBox { Text = "guacamole_db", Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(txtDatabase, 1, 3);

            // Username
            table.Controls.Add(new Label { Text = "Username:", AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Right }, 0, 4);
            txtUsername = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(txtUsername, 1, 4);

            // Password
            table.Controls.Add(new Label { Text = "Password:", AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Right }, 0, 5);
            txtPassword = new TextBox { PasswordChar = '*', Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(txtPassword, 1, 5);

            // Buttons
            var btnPanel = new FlowLayoutPanel();
            btnPanel.FlowDirection = FlowDirection.RightToLeft;
            btnPanel.Dock = DockStyle.Fill;
            
            btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel };
            btnImport = new Button { Text = "Import", DialogResult = DialogResult.OK };
            btnImport.Click += BtnImport_Click;

            btnPanel.Controls.Add(btnCancel);
            btnPanel.Controls.Add(btnImport);

            table.Controls.Add(btnPanel, 1, 6);

            this.Controls.Add(table);
            this.AcceptButton = btnImport;
            this.CancelButton = btnCancel;
        }

        private void ApplyTheme()
        {
            // Basic theming logic similar to other forms
            if (ThemeManager.getInstance().ActiveAndExtended)
            {
                var palette = ThemeManager.getInstance().ActiveTheme.ExtendedPalette;
                if (palette != null)
                {
                    this.BackColor = palette.getColor("Dialog_Background");
                    this.ForeColor = palette.getColor("Dialog_Foreground");
                }
            }
        }

        private static void ApplyLanguage()
        {
            // TODO: Use Language resources if available for generic terms
            // For now hardcoded English as fallback or specific labels
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                IDatabaseConnector? connector = null;
                string host = txtHost.Text;
                if (!string.IsNullOrEmpty(txtPort.Text))
                {
                    host += ":" + txtPort.Text;
                }

                if (string.Equals(cmbType.SelectedItem?.ToString(), "MySQL", StringComparison.Ordinal))
                {
                    connector = new MySqlDatabaseConnector(host, txtDatabase.Text, txtUsername.Text, txtPassword.Text);
                }
                else if (string.Equals(cmbType.SelectedItem?.ToString(), "SQL Server", StringComparison.Ordinal))
                {
                    connector = new MSSqlDatabaseConnector(host, txtDatabase.Text, txtUsername.Text, txtPassword.Text);
                }

                if (connector != null)
                {
                    Import.ImportFromGuacamole(connector, _destinationContainer);
                    MessageBox.Show("Import successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Don't close so user can retry
                this.DialogResult = DialogResult.None;
            }
        }
    }
}
