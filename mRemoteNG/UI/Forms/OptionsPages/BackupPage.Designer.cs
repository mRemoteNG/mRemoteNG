

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class BackupPage : OptionsPage
    {

        //UserControl overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer
        private System.ComponentModel.Container components = null;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            tableLayoutPanelBackupFile = new System.Windows.Forms.TableLayoutPanel();
            lblBackupType = new System.Windows.Forms.Label();
            lblBackupEnable = new System.Windows.Forms.Label();
            lblMakeBackup = new System.Windows.Forms.Label();
            lblConnectionsBackupMaxCount = new System.Windows.Forms.Label();
            numMaxBackups = new System.Windows.Forms.NumericUpDown();
            pnlBackupEnable = new System.Windows.Forms.Panel();
            rbBackupEnableEnable = new System.Windows.Forms.RadioButton();
            rbBackupEnableDisable = new System.Windows.Forms.RadioButton();
            pnlBackupType = new System.Windows.Forms.Panel();
            radioButton2 = new System.Windows.Forms.RadioButton();
            radioButton1 = new System.Windows.Forms.RadioButton();
            lblConnectionsBackupPath = new System.Windows.Forms.Label();
            txtConnectionsBackupPath = new System.Windows.Forms.TextBox();
            btnBrowsePath = new System.Windows.Forms.Button();
            lblBackupNameFormat = new System.Windows.Forms.Label();
            txtBackupNameFormat = new System.Windows.Forms.TextBox();
            lblBacupPageShowInOptionsMenu = new System.Windows.Forms.Label();
            pnlShowForUser = new System.Windows.Forms.Panel();
            cbBacupPageInOptionMenu = new System.Windows.Forms.CheckBox();
            lblACL = new System.Windows.Forms.Label();
            plBackupEnable = new System.Windows.Forms.Panel();
            cbBackupEnableACL = new System.Windows.Forms.ComboBox();
            plBackupType = new System.Windows.Forms.Panel();
            cbBackupTypeACL = new System.Windows.Forms.ComboBox();
            plBackupFrequency = new System.Windows.Forms.Panel();
            cbBackupFrequencyACL = new System.Windows.Forms.ComboBox();
            plBackupNumber = new System.Windows.Forms.Panel();
            cbBackupNumberACL = new System.Windows.Forms.ComboBox();
            plBackupNameFormat = new System.Windows.Forms.Panel();
            cbBackupNameFormatACL = new System.Windows.Forms.ComboBox();
            plBackupLocation = new System.Windows.Forms.Panel();
            cbBackupLocationACL = new System.Windows.Forms.ComboBox();
            plMakeBackup = new System.Windows.Forms.Panel();
            cbMakeBackupOnSave = new System.Windows.Forms.CheckBox();
            cbMakeBackupOnEdit = new System.Windows.Forms.CheckBox();
            cbMakeBackupOnExit = new System.Windows.Forms.CheckBox();
            panel1 = new System.Windows.Forms.Panel();
            tableLayoutPanelBackupFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxBackups).BeginInit();
            pnlBackupEnable.SuspendLayout();
            pnlBackupType.SuspendLayout();
            pnlShowForUser.SuspendLayout();
            plBackupEnable.SuspendLayout();
            plBackupType.SuspendLayout();
            plBackupFrequency.SuspendLayout();
            plBackupNumber.SuspendLayout();
            plBackupNameFormat.SuspendLayout();
            plBackupLocation.SuspendLayout();
            plMakeBackup.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelBackupFile
            // 
            tableLayoutPanelBackupFile.ColumnCount = 4;
            tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanelBackupFile.Controls.Add(lblBackupType, 1, 2);
            tableLayoutPanelBackupFile.Controls.Add(lblBackupEnable, 1, 1);
            tableLayoutPanelBackupFile.Controls.Add(lblMakeBackup, 1, 3);
            tableLayoutPanelBackupFile.Controls.Add(lblConnectionsBackupMaxCount, 1, 4);
            tableLayoutPanelBackupFile.Controls.Add(numMaxBackups, 2, 4);
            tableLayoutPanelBackupFile.Controls.Add(pnlBackupEnable, 2, 1);
            tableLayoutPanelBackupFile.Controls.Add(pnlBackupType, 2, 2);
            tableLayoutPanelBackupFile.Controls.Add(lblConnectionsBackupPath, 1, 6);
            tableLayoutPanelBackupFile.Controls.Add(txtConnectionsBackupPath, 2, 6);
            tableLayoutPanelBackupFile.Controls.Add(btnBrowsePath, 3, 6);
            tableLayoutPanelBackupFile.Controls.Add(lblBackupNameFormat, 1, 5);
            tableLayoutPanelBackupFile.Controls.Add(txtBackupNameFormat, 2, 5);
            tableLayoutPanelBackupFile.Controls.Add(lblBacupPageShowInOptionsMenu, 1, 0);
            tableLayoutPanelBackupFile.Controls.Add(pnlShowForUser, 2, 0);
            tableLayoutPanelBackupFile.Controls.Add(lblACL, 0, 0);
            tableLayoutPanelBackupFile.Controls.Add(plBackupEnable, 0, 1);
            tableLayoutPanelBackupFile.Controls.Add(plBackupType, 0, 2);
            tableLayoutPanelBackupFile.Controls.Add(plBackupFrequency, 0, 3);
            tableLayoutPanelBackupFile.Controls.Add(plBackupNumber, 0, 4);
            tableLayoutPanelBackupFile.Controls.Add(plBackupNameFormat, 0, 5);
            tableLayoutPanelBackupFile.Controls.Add(plBackupLocation, 0, 6);
            tableLayoutPanelBackupFile.Controls.Add(plMakeBackup, 2, 3);
            tableLayoutPanelBackupFile.Controls.Add(panel1, 3, 0);
            tableLayoutPanelBackupFile.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelBackupFile.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanelBackupFile.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelBackupFile.Name = "tableLayoutPanelBackupFile";
            tableLayoutPanelBackupFile.RowCount = 8;
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelBackupFile.Size = new System.Drawing.Size(761, 633);
            tableLayoutPanelBackupFile.TabIndex = 15;
            // 
            // lblBackupType
            // 
            lblBackupType.AutoSize = true;
            lblBackupType.Dock = System.Windows.Forms.DockStyle.Fill;
            lblBackupType.Location = new System.Drawing.Point(103, 57);
            lblBackupType.Name = "lblBackupType";
            lblBackupType.Size = new System.Drawing.Size(165, 30);
            lblBackupType.TabIndex = 20;
            lblBackupType.Text = "Backup type";
            lblBackupType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBackupEnable
            // 
            lblBackupEnable.AutoSize = true;
            lblBackupEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            lblBackupEnable.Location = new System.Drawing.Point(103, 27);
            lblBackupEnable.Name = "lblBackupEnable";
            lblBackupEnable.Size = new System.Drawing.Size(165, 30);
            lblBackupEnable.TabIndex = 6;
            lblBackupEnable.Text = "Backups for connection data";
            lblBackupEnable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMakeBackup
            // 
            lblMakeBackup.AutoSize = true;
            lblMakeBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            lblMakeBackup.Location = new System.Drawing.Point(103, 87);
            lblMakeBackup.Name = "lblMakeBackup";
            lblMakeBackup.Size = new System.Drawing.Size(165, 30);
            lblMakeBackup.TabIndex = 14;
            lblMakeBackup.Text = "Make a backup";
            lblMakeBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConnectionsBackupMaxCount
            // 
            lblConnectionsBackupMaxCount.AutoSize = true;
            lblConnectionsBackupMaxCount.Dock = System.Windows.Forms.DockStyle.Fill;
            lblConnectionsBackupMaxCount.Location = new System.Drawing.Point(103, 117);
            lblConnectionsBackupMaxCount.Name = "lblConnectionsBackupMaxCount";
            lblConnectionsBackupMaxCount.Size = new System.Drawing.Size(165, 30);
            lblConnectionsBackupMaxCount.TabIndex = 13;
            lblConnectionsBackupMaxCount.Text = "Maximum number of backups";
            lblConnectionsBackupMaxCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numMaxBackups
            // 
            numMaxBackups.Location = new System.Drawing.Point(274, 120);
            numMaxBackups.Name = "numMaxBackups";
            numMaxBackups.Size = new System.Drawing.Size(34, 22);
            numMaxBackups.TabIndex = 1;
            numMaxBackups.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlBackupEnable
            // 
            pnlBackupEnable.Controls.Add(rbBackupEnableEnable);
            pnlBackupEnable.Controls.Add(rbBackupEnableDisable);
            pnlBackupEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBackupEnable.Location = new System.Drawing.Point(274, 30);
            pnlBackupEnable.Name = "pnlBackupEnable";
            pnlBackupEnable.Size = new System.Drawing.Size(217, 24);
            pnlBackupEnable.TabIndex = 0;
            // 
            // rbBackupEnableEnable
            // 
            rbBackupEnableEnable.AutoSize = true;
            rbBackupEnableEnable.Checked = true;
            rbBackupEnableEnable.Location = new System.Drawing.Point(4, 3);
            rbBackupEnableEnable.Name = "rbBackupEnableEnable";
            rbBackupEnableEnable.Size = new System.Drawing.Size(60, 17);
            rbBackupEnableEnable.TabIndex = 3;
            rbBackupEnableEnable.TabStop = true;
            rbBackupEnableEnable.Text = "Enable";
            rbBackupEnableEnable.UseVisualStyleBackColor = true;
            // 
            // rbBackupEnableDisable
            // 
            rbBackupEnableDisable.AutoSize = true;
            rbBackupEnableDisable.Location = new System.Drawing.Point(70, 3);
            rbBackupEnableDisable.Name = "rbBackupEnableDisable";
            rbBackupEnableDisable.Size = new System.Drawing.Size(63, 17);
            rbBackupEnableDisable.TabIndex = 4;
            rbBackupEnableDisable.Text = "Disable";
            rbBackupEnableDisable.UseVisualStyleBackColor = true;
            rbBackupEnableDisable.CheckedChanged += rbBackupEnableDisable_CheckedChanged;
            // 
            // pnlBackupType
            // 
            pnlBackupType.Controls.Add(radioButton2);
            pnlBackupType.Controls.Add(radioButton1);
            pnlBackupType.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBackupType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pnlBackupType.Location = new System.Drawing.Point(274, 60);
            pnlBackupType.Name = "pnlBackupType";
            pnlBackupType.Size = new System.Drawing.Size(217, 24);
            pnlBackupType.TabIndex = 27;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Enabled = false;
            radioButton2.Location = new System.Drawing.Point(70, 3);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(53, 17);
            radioButton2.TabIndex = 1;
            radioButton2.Text = "to DB";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new System.Drawing.Point(4, 3);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(57, 17);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "to File";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // lblConnectionsBackupPath
            // 
            lblConnectionsBackupPath.AutoSize = true;
            lblConnectionsBackupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            lblConnectionsBackupPath.Location = new System.Drawing.Point(103, 177);
            lblConnectionsBackupPath.Name = "lblConnectionsBackupPath";
            lblConnectionsBackupPath.Size = new System.Drawing.Size(165, 31);
            lblConnectionsBackupPath.TabIndex = 13;
            lblConnectionsBackupPath.Text = "Backup folder";
            lblConnectionsBackupPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConnectionsBackupPath
            // 
            txtConnectionsBackupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            txtConnectionsBackupPath.Location = new System.Drawing.Point(274, 182);
            txtConnectionsBackupPath.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            txtConnectionsBackupPath.Name = "txtConnectionsBackupPath";
            txtConnectionsBackupPath.Size = new System.Drawing.Size(217, 22);
            txtConnectionsBackupPath.TabIndex = 3;
            // 
            // btnBrowsePath
            // 
            btnBrowsePath.AutoSize = true;
            btnBrowsePath.Location = new System.Drawing.Point(497, 180);
            btnBrowsePath.Name = "btnBrowsePath";
            btnBrowsePath.Size = new System.Drawing.Size(94, 25);
            btnBrowsePath.TabIndex = 4;
            btnBrowsePath.Text = "Browse";
            btnBrowsePath.UseVisualStyleBackColor = true;
            btnBrowsePath.Click += ButtonBrowsePath_Click;
            // 
            // lblBackupNameFormat
            // 
            lblBackupNameFormat.AutoSize = true;
            lblBackupNameFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            lblBackupNameFormat.Location = new System.Drawing.Point(103, 147);
            lblBackupNameFormat.Name = "lblBackupNameFormat";
            lblBackupNameFormat.Size = new System.Drawing.Size(165, 30);
            lblBackupNameFormat.TabIndex = 33;
            lblBackupNameFormat.Text = "Backup file name format";
            lblBackupNameFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBackupNameFormat
            // 
            txtBackupNameFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            txtBackupNameFormat.Location = new System.Drawing.Point(274, 152);
            txtBackupNameFormat.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            txtBackupNameFormat.Name = "txtBackupNameFormat";
            txtBackupNameFormat.Size = new System.Drawing.Size(217, 22);
            txtBackupNameFormat.TabIndex = 2;
            // 
            // lblBacupPageShowInOptionsMenu
            // 
            lblBacupPageShowInOptionsMenu.AutoSize = true;
            lblBacupPageShowInOptionsMenu.BackColor = System.Drawing.Color.Salmon;
            lblBacupPageShowInOptionsMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            lblBacupPageShowInOptionsMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblBacupPageShowInOptionsMenu.Location = new System.Drawing.Point(100, 0);
            lblBacupPageShowInOptionsMenu.Margin = new System.Windows.Forms.Padding(0);
            lblBacupPageShowInOptionsMenu.Name = "lblBacupPageShowInOptionsMenu";
            lblBacupPageShowInOptionsMenu.Size = new System.Drawing.Size(171, 27);
            lblBacupPageShowInOptionsMenu.TabIndex = 0;
            lblBacupPageShowInOptionsMenu.Text = "Page control in Options menu";
            lblBacupPageShowInOptionsMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblBacupPageShowInOptionsMenu.Visible = false;
            // 
            // pnlShowForUser
            // 
            pnlShowForUser.BackColor = System.Drawing.Color.Salmon;
            pnlShowForUser.Controls.Add(cbBacupPageInOptionMenu);
            pnlShowForUser.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlShowForUser.Location = new System.Drawing.Point(271, 0);
            pnlShowForUser.Margin = new System.Windows.Forms.Padding(0);
            pnlShowForUser.Name = "pnlShowForUser";
            pnlShowForUser.Size = new System.Drawing.Size(223, 27);
            pnlShowForUser.TabIndex = 0;
            pnlShowForUser.Visible = false;
            // 
            // cbBacupPageInOptionMenu
            // 
            cbBacupPageInOptionMenu.AutoSize = true;
            cbBacupPageInOptionMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cbBacupPageInOptionMenu.Location = new System.Drawing.Point(7, 5);
            cbBacupPageInOptionMenu.Name = "cbBacupPageInOptionMenu";
            cbBacupPageInOptionMenu.Size = new System.Drawing.Size(104, 19);
            cbBacupPageInOptionMenu.TabIndex = 1;
            cbBacupPageInOptionMenu.Text = "Show for user";
            cbBacupPageInOptionMenu.UseVisualStyleBackColor = true;
            // 
            // lblACL
            // 
            lblACL.AutoSize = true;
            lblACL.BackColor = System.Drawing.Color.Salmon;
            lblACL.Dock = System.Windows.Forms.DockStyle.Fill;
            lblACL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblACL.Location = new System.Drawing.Point(0, 0);
            lblACL.Margin = new System.Windows.Forms.Padding(0);
            lblACL.Name = "lblACL";
            lblACL.Size = new System.Drawing.Size(100, 27);
            lblACL.TabIndex = 0;
            lblACL.Text = "ACL for user";
            lblACL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblACL.Visible = false;
            // 
            // plBackupEnable
            // 
            plBackupEnable.BackColor = System.Drawing.Color.Salmon;
            plBackupEnable.Controls.Add(cbBackupEnableACL);
            plBackupEnable.Location = new System.Drawing.Point(0, 27);
            plBackupEnable.Margin = new System.Windows.Forms.Padding(0);
            plBackupEnable.Name = "plBackupEnable";
            plBackupEnable.Size = new System.Drawing.Size(100, 30);
            plBackupEnable.TabIndex = 0;
            plBackupEnable.Visible = false;
            // 
            // cbBackupEnableACL
            // 
            cbBackupEnableACL.BackColor = System.Drawing.Color.Salmon;
            cbBackupEnableACL.Cursor = System.Windows.Forms.Cursors.Hand;
            cbBackupEnableACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBackupEnableACL.FormattingEnabled = true;
            cbBackupEnableACL.Location = new System.Drawing.Point(3, 5);
            cbBackupEnableACL.MaxDropDownItems = 3;
            cbBackupEnableACL.Name = "cbBackupEnableACL";
            cbBackupEnableACL.Size = new System.Drawing.Size(94, 21);
            cbBackupEnableACL.TabIndex = 2;
            cbBackupEnableACL.Visible = false;
            // 
            // plBackupType
            // 
            plBackupType.BackColor = System.Drawing.Color.Salmon;
            plBackupType.Controls.Add(cbBackupTypeACL);
            plBackupType.Location = new System.Drawing.Point(0, 57);
            plBackupType.Margin = new System.Windows.Forms.Padding(0);
            plBackupType.Name = "plBackupType";
            plBackupType.Size = new System.Drawing.Size(100, 30);
            plBackupType.TabIndex = 0;
            plBackupType.Visible = false;
            // 
            // cbBackupTypeACL
            // 
            cbBackupTypeACL.Cursor = System.Windows.Forms.Cursors.Hand;
            cbBackupTypeACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBackupTypeACL.FormattingEnabled = true;
            cbBackupTypeACL.Location = new System.Drawing.Point(3, 5);
            cbBackupTypeACL.MaxDropDownItems = 3;
            cbBackupTypeACL.Name = "cbBackupTypeACL";
            cbBackupTypeACL.Size = new System.Drawing.Size(94, 21);
            cbBackupTypeACL.TabIndex = 0;
            cbBackupTypeACL.Visible = false;
            // 
            // plBackupFrequency
            // 
            plBackupFrequency.BackColor = System.Drawing.Color.Salmon;
            plBackupFrequency.Controls.Add(cbBackupFrequencyACL);
            plBackupFrequency.Location = new System.Drawing.Point(0, 87);
            plBackupFrequency.Margin = new System.Windows.Forms.Padding(0);
            plBackupFrequency.Name = "plBackupFrequency";
            plBackupFrequency.Size = new System.Drawing.Size(100, 30);
            plBackupFrequency.TabIndex = 0;
            plBackupFrequency.Visible = false;
            // 
            // cbBackupFrequencyACL
            // 
            cbBackupFrequencyACL.Cursor = System.Windows.Forms.Cursors.Hand;
            cbBackupFrequencyACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBackupFrequencyACL.FormattingEnabled = true;
            cbBackupFrequencyACL.Location = new System.Drawing.Point(3, 5);
            cbBackupFrequencyACL.MaxDropDownItems = 3;
            cbBackupFrequencyACL.Name = "cbBackupFrequencyACL";
            cbBackupFrequencyACL.Size = new System.Drawing.Size(94, 21);
            cbBackupFrequencyACL.TabIndex = 0;
            cbBackupFrequencyACL.Visible = false;
            // 
            // plBackupNumber
            // 
            plBackupNumber.BackColor = System.Drawing.Color.Salmon;
            plBackupNumber.Controls.Add(cbBackupNumberACL);
            plBackupNumber.Location = new System.Drawing.Point(0, 117);
            plBackupNumber.Margin = new System.Windows.Forms.Padding(0);
            plBackupNumber.Name = "plBackupNumber";
            plBackupNumber.Size = new System.Drawing.Size(100, 30);
            plBackupNumber.TabIndex = 0;
            plBackupNumber.Visible = false;
            // 
            // cbBackupNumberACL
            // 
            cbBackupNumberACL.Cursor = System.Windows.Forms.Cursors.Hand;
            cbBackupNumberACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBackupNumberACL.FormattingEnabled = true;
            cbBackupNumberACL.Location = new System.Drawing.Point(3, 5);
            cbBackupNumberACL.MaxDropDownItems = 3;
            cbBackupNumberACL.Name = "cbBackupNumberACL";
            cbBackupNumberACL.Size = new System.Drawing.Size(94, 21);
            cbBackupNumberACL.TabIndex = 0;
            cbBackupNumberACL.Visible = false;
            // 
            // plBackupNameFormat
            // 
            plBackupNameFormat.BackColor = System.Drawing.Color.Salmon;
            plBackupNameFormat.Controls.Add(cbBackupNameFormatACL);
            plBackupNameFormat.Location = new System.Drawing.Point(0, 147);
            plBackupNameFormat.Margin = new System.Windows.Forms.Padding(0);
            plBackupNameFormat.Name = "plBackupNameFormat";
            plBackupNameFormat.Size = new System.Drawing.Size(100, 30);
            plBackupNameFormat.TabIndex = 0;
            plBackupNameFormat.Visible = false;
            // 
            // cbBackupNameFormatACL
            // 
            cbBackupNameFormatACL.Cursor = System.Windows.Forms.Cursors.Hand;
            cbBackupNameFormatACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBackupNameFormatACL.FormattingEnabled = true;
            cbBackupNameFormatACL.Location = new System.Drawing.Point(3, 5);
            cbBackupNameFormatACL.MaxDropDownItems = 3;
            cbBackupNameFormatACL.Name = "cbBackupNameFormatACL";
            cbBackupNameFormatACL.Size = new System.Drawing.Size(94, 21);
            cbBackupNameFormatACL.TabIndex = 0;
            cbBackupNameFormatACL.Visible = false;
            // 
            // plBackupLocation
            // 
            plBackupLocation.BackColor = System.Drawing.Color.Salmon;
            plBackupLocation.Controls.Add(cbBackupLocationACL);
            plBackupLocation.Location = new System.Drawing.Point(0, 177);
            plBackupLocation.Margin = new System.Windows.Forms.Padding(0);
            plBackupLocation.Name = "plBackupLocation";
            plBackupLocation.Size = new System.Drawing.Size(100, 30);
            plBackupLocation.TabIndex = 0;
            plBackupLocation.Visible = false;
            // 
            // cbBackupLocationACL
            // 
            cbBackupLocationACL.Cursor = System.Windows.Forms.Cursors.Hand;
            cbBackupLocationACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbBackupLocationACL.FormattingEnabled = true;
            cbBackupLocationACL.Location = new System.Drawing.Point(3, 5);
            cbBackupLocationACL.MaxDropDownItems = 3;
            cbBackupLocationACL.Name = "cbBackupLocationACL";
            cbBackupLocationACL.Size = new System.Drawing.Size(94, 21);
            cbBackupLocationACL.TabIndex = 0;
            cbBackupLocationACL.Visible = false;
            // 
            // plMakeBackup
            // 
            plMakeBackup.Controls.Add(cbMakeBackupOnSave);
            plMakeBackup.Controls.Add(cbMakeBackupOnEdit);
            plMakeBackup.Controls.Add(cbMakeBackupOnExit);
            plMakeBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            plMakeBackup.Location = new System.Drawing.Point(274, 90);
            plMakeBackup.Name = "plMakeBackup";
            plMakeBackup.Size = new System.Drawing.Size(217, 24);
            plMakeBackup.TabIndex = 34;
            // 
            // cbMakeBackupOnSave
            // 
            cbMakeBackupOnSave.AutoSize = true;
            cbMakeBackupOnSave.Location = new System.Drawing.Point(141, 3);
            cbMakeBackupOnSave.Name = "cbMakeBackupOnSave";
            cbMakeBackupOnSave.Size = new System.Drawing.Size(67, 17);
            cbMakeBackupOnSave.TabIndex = 2;
            cbMakeBackupOnSave.Text = "On save";
            cbMakeBackupOnSave.UseVisualStyleBackColor = true;
            // 
            // cbMakeBackupOnEdit
            // 
            cbMakeBackupOnEdit.AutoSize = true;
            cbMakeBackupOnEdit.Location = new System.Drawing.Point(70, 3);
            cbMakeBackupOnEdit.Name = "cbMakeBackupOnEdit";
            cbMakeBackupOnEdit.Size = new System.Drawing.Size(65, 17);
            cbMakeBackupOnEdit.TabIndex = 1;
            cbMakeBackupOnEdit.Text = "On edit";
            cbMakeBackupOnEdit.UseVisualStyleBackColor = true;
            // 
            // cbMakeBackupOnExit
            // 
            cbMakeBackupOnExit.AutoSize = true;
            cbMakeBackupOnExit.Location = new System.Drawing.Point(4, 4);
            cbMakeBackupOnExit.Name = "cbMakeBackupOnExit";
            cbMakeBackupOnExit.Size = new System.Drawing.Size(63, 17);
            cbMakeBackupOnExit.TabIndex = 0;
            cbMakeBackupOnExit.Text = "On exit";
            cbMakeBackupOnExit.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.Salmon;
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(494, 0);
            panel1.Margin = new System.Windows.Forms.Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(267, 27);
            panel1.TabIndex = 35;
            // 
            // BackupPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(tableLayoutPanelBackupFile);
            Name = "BackupPage";
            Size = new System.Drawing.Size(761, 633);
            tableLayoutPanelBackupFile.ResumeLayout(false);
            tableLayoutPanelBackupFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxBackups).EndInit();
            pnlBackupEnable.ResumeLayout(false);
            pnlBackupEnable.PerformLayout();
            pnlBackupType.ResumeLayout(false);
            pnlBackupType.PerformLayout();
            pnlShowForUser.ResumeLayout(false);
            pnlShowForUser.PerformLayout();
            plBackupEnable.ResumeLayout(false);
            plBackupType.ResumeLayout(false);
            plBackupFrequency.ResumeLayout(false);
            plBackupNumber.ResumeLayout(false);
            plBackupNameFormat.ResumeLayout(false);
            plBackupLocation.ResumeLayout(false);
            plMakeBackup.ResumeLayout(false);
            plMakeBackup.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBackupFile;
        private System.Windows.Forms.TextBox txtConnectionsBackupPath;
        private System.Windows.Forms.Label lblConnectionsBackupPath;
        private System.Windows.Forms.Label lblMakeBackup;
        private System.Windows.Forms.Label lblConnectionsBackupMaxCount;
        private System.Windows.Forms.Button btnBrowsePath;
        private System.Windows.Forms.NumericUpDown numMaxBackups;
        private System.Windows.Forms.Label lblACL;
        private System.Windows.Forms.Label lblBackupType;
        private System.Windows.Forms.Label lblBackupEnable;
        private System.Windows.Forms.ComboBox cbBackupEnableACL;
        private System.Windows.Forms.Panel pnlBackupEnable;
        private System.Windows.Forms.RadioButton rbBackupEnableEnable;
        private System.Windows.Forms.RadioButton rbBackupEnableDisable;
        private System.Windows.Forms.Panel pnlBackupType;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox cbBackupFrequencyACL;
        private System.Windows.Forms.ComboBox cbBackupTypeACL;
        private System.Windows.Forms.ComboBox cbBackupNumberACL;
        private System.Windows.Forms.ComboBox cbBackupNameFormatACL;
        private System.Windows.Forms.ComboBox cbBackupLocationACL;
        private System.Windows.Forms.Label lblBackupNameFormat;
        private System.Windows.Forms.TextBox txtBackupNameFormat;
        private System.Windows.Forms.Label lblBacupPageShowInOptionsMenu;
        private System.Windows.Forms.CheckBox cbBacupPageInOptionMenu;
        private System.Windows.Forms.Panel pnlShowForUser;
        private System.Windows.Forms.Panel plBackupEnable;
        private System.Windows.Forms.Panel plBackupType;
        private System.Windows.Forms.Panel plBackupFrequency;
        private System.Windows.Forms.Panel plBackupNumber;
        private System.Windows.Forms.Panel plBackupNameFormat;
        private System.Windows.Forms.Panel plBackupLocation;
        private System.Windows.Forms.Panel plMakeBackup;
        private System.Windows.Forms.CheckBox cbMakeBackupOnSave;
        private System.Windows.Forms.CheckBox cbMakeBackupOnEdit;
        private System.Windows.Forms.CheckBox cbMakeBackupOnExit;
        private System.Windows.Forms.Panel panel1;
    }
}