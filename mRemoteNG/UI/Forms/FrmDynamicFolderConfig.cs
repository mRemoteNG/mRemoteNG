using System;
using System.Windows.Forms;
using mRemoteNG.Container;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmDynamicFolderConfig : Form
    {
        private ContainerInfo _container;

        private Label lblSourceType;
        private ComboBox cboSourceType;
        private Label lblSourceValue;
        private TextBox txtSourceValue;
        private Label lblRefreshInterval;
        private NumericUpDown numRefreshInterval;
        private Button btnOk;
        private Button btnCancel;

        public FrmDynamicFolderConfig(ContainerInfo container)
        {
            _container = container;
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.lblSourceType = new System.Windows.Forms.Label();
            this.cboSourceType = new System.Windows.Forms.ComboBox();
            this.lblSourceValue = new System.Windows.Forms.Label();
            this.txtSourceValue = new System.Windows.Forms.TextBox();
            this.lblRefreshInterval = new System.Windows.Forms.Label();
            this.numRefreshInterval = new System.Windows.Forms.NumericUpDown();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRefreshInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSourceType
            // 
            this.lblSourceType.AutoSize = true;
            this.lblSourceType.Location = new System.Drawing.Point(12, 15);
            this.lblSourceType.Name = "lblSourceType";
            this.lblSourceType.Size = new System.Drawing.Size(71, 13);
            this.lblSourceType.TabIndex = 0;
            this.lblSourceType.Text = "Source Type:";
            // 
            // cboSourceType
            // 
            this.cboSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceType.FormattingEnabled = true;
            this.cboSourceType.Location = new System.Drawing.Point(110, 12);
            this.cboSourceType.Name = "cboSourceType";
            this.cboSourceType.Size = new System.Drawing.Size(260, 21);
            this.cboSourceType.TabIndex = 1;
            this.cboSourceType.SelectedIndexChanged += new System.EventHandler(this.cboSourceType_SelectedIndexChanged);
            // 
            // lblSourceValue
            // 
            this.lblSourceValue.AutoSize = true;
            this.lblSourceValue.Location = new System.Drawing.Point(12, 45);
            this.lblSourceValue.Name = "lblSourceValue";
            this.lblSourceValue.Size = new System.Drawing.Size(92, 13);
            this.lblSourceValue.TabIndex = 2;
            this.lblSourceValue.Text = "Path / Query:";
            // 
            // txtSourceValue
            // 
            this.txtSourceValue.Location = new System.Drawing.Point(110, 42);
            this.txtSourceValue.Name = "txtSourceValue";
            this.txtSourceValue.Size = new System.Drawing.Size(260, 20);
            this.txtSourceValue.TabIndex = 3;
            // 
            // lblRefreshInterval
            // 
            this.lblRefreshInterval.AutoSize = true;
            this.lblRefreshInterval.Location = new System.Drawing.Point(12, 75);
            this.lblRefreshInterval.Name = "lblRefreshInterval";
            this.lblRefreshInterval.Size = new System.Drawing.Size(92, 13);
            this.lblRefreshInterval.TabIndex = 4;
            this.lblRefreshInterval.Text = "Refresh (minutes):";
            // 
            // numRefreshInterval
            // 
            this.numRefreshInterval.Location = new System.Drawing.Point(110, 72);
            this.numRefreshInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numRefreshInterval.Name = "numRefreshInterval";
            this.numRefreshInterval.Size = new System.Drawing.Size(80, 20);
            this.numRefreshInterval.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(214, 100);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(295, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmDynamicFolderConfig
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 136);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.numRefreshInterval);
            this.Controls.Add(this.lblRefreshInterval);
            this.Controls.Add(this.txtSourceValue);
            this.Controls.Add(this.lblSourceValue);
            this.Controls.Add(this.cboSourceType);
            this.Controls.Add(this.lblSourceType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDynamicFolderConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Dynamic Folder";
            ((System.ComponentModel.ISupportInitialize)(this.numRefreshInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoadSettings()
        {
            cboSourceType.DataSource = Enum.GetValues<DynamicSourceType>();
            cboSourceType.SelectedItem = _container.DynamicSource;
            txtSourceValue.Text = _container.DynamicSourceValue;
            numRefreshInterval.Value = _container.DynamicRefreshInterval;
            
            UpdateUi();
        }

        private void UpdateUi()
        {
            bool isEnabled = cboSourceType.SelectedItem is DynamicSourceType typed && typed != DynamicSourceType.None;
            txtSourceValue.Enabled = isEnabled;
            numRefreshInterval.Enabled = isEnabled;
        }

        private void cboSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUi();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _container.DynamicSource = cboSourceType.SelectedItem is DynamicSourceType typed ? typed : DynamicSourceType.None;
            _container.DynamicSourceValue = txtSourceValue.Text;
            _container.DynamicRefreshInterval = (int)numRefreshInterval.Value;
            
            // Re-schedule
            if (_container.DynamicSource != DynamicSourceType.None)
            {
                App.Runtime.DynamicFolderManager.ScheduleRefresh(_container);
            }
            else
            {
                App.Runtime.DynamicFolderManager.UnscheduleRefresh(_container);
            }
        }
    }
}
