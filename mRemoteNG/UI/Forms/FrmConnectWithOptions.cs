using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public class FrmConnectWithOptions : Form
    {
        private PropertyGrid _propertyGrid = null!;
        private Button _btnConnect = null!;
        private Button _btnCancel = null!;
        private ConnectionInfo _connectionInfo = null!;

        public ConnectionInfo ConnectionInfo => _connectionInfo;

        public FrmConnectWithOptions(ConnectionInfo originalConnection)
        {
            InitializeComponent();
            _connectionInfo = CreateFlattenedClone(originalConnection);
            _propertyGrid.SelectedObject = _connectionInfo;
            this.Text = Language.ConnectWithOptions + " - " + originalConnection.Name;
        }

        private void InitializeComponent()
        {
            _propertyGrid = new PropertyGrid();
            _btnConnect = new Button();
            _btnCancel = new Button();
            
            Panel bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 40;

            SuspendLayout();
            bottomPanel.SuspendLayout();

            // PropertyGrid
            _propertyGrid.Dock = DockStyle.Fill;
            _propertyGrid.ToolbarVisible = false;
            _propertyGrid.PropertySort = PropertySort.Categorized;

            // Buttons
            _btnConnect.Text = Language.Connect;
            _btnConnect.DialogResult = DialogResult.OK;
            _btnConnect.AutoSize = true;
            _btnConnect.Location = new Point(180, 8);

            _btnCancel.Text = Language._Cancel;
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.AutoSize = true;
            _btnCancel.Location = new Point(270, 8);

            bottomPanel.Controls.Add(_btnConnect);
            bottomPanel.Controls.Add(_btnCancel);

            // Form
            ClientSize = new Size(360, 500);
            Controls.Add(_propertyGrid);
            Controls.Add(bottomPanel);
            
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AcceptButton = _btnConnect;
            CancelButton = _btnCancel;
            
            bottomPanel.ResumeLayout(false);
            bottomPanel.PerformLayout();
            ResumeLayout(false);
        }

        private static ConnectionInfo CreateFlattenedClone(ConnectionInfo original)
        {
            var clone = new ConnectionInfo();
            // Copy properties resolving inheritance
            foreach (PropertyInfo prop in typeof(ConnectionInfo).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite) continue;
                if (string.Equals(prop.Name, "Parent", StringComparison.Ordinal) ||
                    string.Equals(prop.Name, "Inheritance", StringComparison.Ordinal) ||
                    string.Equals(prop.Name, "OpenConnections", StringComparison.Ordinal) ||
                    string.Equals(prop.Name, "ConstantID", StringComparison.Ordinal) ||
                    string.Equals(prop.Name, "TreeNode", StringComparison.Ordinal) ||
                    string.Equals(prop.Name, "IsContainer", StringComparison.Ordinal) ||
                    string.Equals(prop.Name, "IsRoot", StringComparison.Ordinal)) continue;

                try
                {
                    object? val = prop.GetValue(original, null);
                    prop.SetValue(clone, val, null);
                }
                catch (Exception)
                {
                    _ = 0; // Ignore errors copying specific properties
                }
            }
            
            clone.Name = original.Name;
            return clone;
        }
    }
}
