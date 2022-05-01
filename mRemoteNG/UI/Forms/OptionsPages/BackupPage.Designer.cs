

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class BackupPage : OptionsPage
	{

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
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
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.tableLayoutPanelBackupFile = new System.Windows.Forms.TableLayoutPanel();
            this.lblBackupType = new System.Windows.Forms.Label();
            this.lblBackupEnable = new System.Windows.Forms.Label();
            this.lblMakeBackup = new System.Windows.Forms.Label();
            this.lblConnectionsBackupMaxCount = new System.Windows.Forms.Label();
            this.numMaxBackups = new System.Windows.Forms.NumericUpDown();
            this.pnlBackupEnable = new System.Windows.Forms.Panel();
            this.rbBackupEnableEnable = new System.Windows.Forms.RadioButton();
            this.rbBackupEnableDisable = new System.Windows.Forms.RadioButton();
            this.pnlBackupType = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.lblConnectionsBackupPath = new System.Windows.Forms.Label();
            this.txtConnectionsBackupPath = new System.Windows.Forms.TextBox();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.lblBackupNameFormat = new System.Windows.Forms.Label();
            this.txtBackupNameFormat = new System.Windows.Forms.TextBox();
            this.lblBacupPageShowInOptionsMenu = new System.Windows.Forms.Label();
            this.pnlShowForUser = new System.Windows.Forms.Panel();
            this.cbBacupPageInOptionMenu = new System.Windows.Forms.CheckBox();
            this.lblACL = new System.Windows.Forms.Label();
            this.plBackupEnable = new System.Windows.Forms.Panel();
            this.cbBackupEnableACL = new System.Windows.Forms.ComboBox();
            this.plBackupType = new System.Windows.Forms.Panel();
            this.cbBackupTypeACL = new System.Windows.Forms.ComboBox();
            this.plBackupFrequency = new System.Windows.Forms.Panel();
            this.cbBackupFrequencyACL = new System.Windows.Forms.ComboBox();
            this.plBackupNumber = new System.Windows.Forms.Panel();
            this.cbBackupNumberACL = new System.Windows.Forms.ComboBox();
            this.plBackupNameFormat = new System.Windows.Forms.Panel();
            this.cbBackupNameFormatACL = new System.Windows.Forms.ComboBox();
            this.plBackupLocation = new System.Windows.Forms.Panel();
            this.cbBackupLocationACL = new System.Windows.Forms.ComboBox();
            this.plMakeBackup = new System.Windows.Forms.Panel();
            this.cbMakeBackupOnSave = new System.Windows.Forms.CheckBox();
            this.cbMakeBackupOnEdit = new System.Windows.Forms.CheckBox();
            this.cbMakeBackupOnExit = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanelBackupFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBackups)).BeginInit();
            this.pnlBackupEnable.SuspendLayout();
            this.pnlBackupType.SuspendLayout();
            this.pnlShowForUser.SuspendLayout();
            this.plBackupEnable.SuspendLayout();
            this.plBackupType.SuspendLayout();
            this.plBackupFrequency.SuspendLayout();
            this.plBackupNumber.SuspendLayout();
            this.plBackupNameFormat.SuspendLayout();
            this.plBackupLocation.SuspendLayout();
            this.plMakeBackup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBackupFile
            // 
            this.tableLayoutPanelBackupFile.ColumnCount = 4;
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblBackupType, 1, 2);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblBackupEnable, 1, 1);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblMakeBackup, 1, 3);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblConnectionsBackupMaxCount, 1, 4);
            this.tableLayoutPanelBackupFile.Controls.Add(this.numMaxBackups, 2, 4);
            this.tableLayoutPanelBackupFile.Controls.Add(this.pnlBackupEnable, 2, 1);
            this.tableLayoutPanelBackupFile.Controls.Add(this.pnlBackupType, 2, 2);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblConnectionsBackupPath, 1, 6);
            this.tableLayoutPanelBackupFile.Controls.Add(this.txtConnectionsBackupPath, 2, 6);
            this.tableLayoutPanelBackupFile.Controls.Add(this.btnBrowsePath, 3, 6);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblBackupNameFormat, 1, 5);
            this.tableLayoutPanelBackupFile.Controls.Add(this.txtBackupNameFormat, 2, 5);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblBacupPageShowInOptionsMenu, 1, 0);
            this.tableLayoutPanelBackupFile.Controls.Add(this.pnlShowForUser, 2, 0);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblACL, 0, 0);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plBackupEnable, 0, 1);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plBackupType, 0, 2);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plBackupFrequency, 0, 3);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plBackupNumber, 0, 4);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plBackupNameFormat, 0, 5);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plBackupLocation, 0, 6);
            this.tableLayoutPanelBackupFile.Controls.Add(this.plMakeBackup, 2, 3);
            this.tableLayoutPanelBackupFile.Controls.Add(this.panel1, 3, 0);
            this.tableLayoutPanelBackupFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBackupFile.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanelBackupFile.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBackupFile.Name = "tableLayoutPanelBackupFile";
            this.tableLayoutPanelBackupFile.RowCount = 8;
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.Size = new System.Drawing.Size(761, 633);
            this.tableLayoutPanelBackupFile.TabIndex = 15;
            // 
            // lblBackupType
            // 
            this.lblBackupType.AutoSize = true;
            this.lblBackupType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackupType.Location = new System.Drawing.Point(103, 57);
            this.lblBackupType.Name = "lblBackupType";
            this.lblBackupType.Size = new System.Drawing.Size(165, 30);
            this.lblBackupType.TabIndex = 20;
            this.lblBackupType.Text = "Backup type";
            this.lblBackupType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBackupEnable
            // 
            this.lblBackupEnable.AutoSize = true;
            this.lblBackupEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackupEnable.Location = new System.Drawing.Point(103, 27);
            this.lblBackupEnable.Name = "lblBackupEnable";
            this.lblBackupEnable.Size = new System.Drawing.Size(165, 30);
            this.lblBackupEnable.TabIndex = 6;
            this.lblBackupEnable.Text = "Backups for connection data";
            this.lblBackupEnable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMakeBackup
            // 
            this.lblMakeBackup.AutoSize = true;
            this.lblMakeBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMakeBackup.Location = new System.Drawing.Point(103, 87);
            this.lblMakeBackup.Name = "lblMakeBackup";
            this.lblMakeBackup.Size = new System.Drawing.Size(165, 30);
            this.lblMakeBackup.TabIndex = 14;
            this.lblMakeBackup.Text = "Make a backup";
            this.lblMakeBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConnectionsBackupMaxCount
            // 
            this.lblConnectionsBackupMaxCount.AutoSize = true;
            this.lblConnectionsBackupMaxCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionsBackupMaxCount.Location = new System.Drawing.Point(103, 117);
            this.lblConnectionsBackupMaxCount.Name = "lblConnectionsBackupMaxCount";
            this.lblConnectionsBackupMaxCount.Size = new System.Drawing.Size(165, 30);
            this.lblConnectionsBackupMaxCount.TabIndex = 13;
            this.lblConnectionsBackupMaxCount.Text = "Maximum number of backups";
            this.lblConnectionsBackupMaxCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numMaxBackups
            // 
            this.numMaxBackups.Location = new System.Drawing.Point(274, 120);
            this.numMaxBackups.Name = "numMaxBackups";
            this.numMaxBackups.Size = new System.Drawing.Size(34, 22);
            this.numMaxBackups.TabIndex = 1;
            this.numMaxBackups.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlBackupEnable
            // 
            this.pnlBackupEnable.Controls.Add(this.rbBackupEnableEnable);
            this.pnlBackupEnable.Controls.Add(this.rbBackupEnableDisable);
            this.pnlBackupEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackupEnable.Location = new System.Drawing.Point(274, 30);
            this.pnlBackupEnable.Name = "pnlBackupEnable";
            this.pnlBackupEnable.Size = new System.Drawing.Size(217, 24);
            this.pnlBackupEnable.TabIndex = 0;
            // 
            // rbBackupEnableEnable
            // 
            this.rbBackupEnableEnable.AutoSize = true;
            this.rbBackupEnableEnable.Checked = true;
            this.rbBackupEnableEnable.Location = new System.Drawing.Point(4, 3);
            this.rbBackupEnableEnable.Name = "rbBackupEnableEnable";
            this.rbBackupEnableEnable.Size = new System.Drawing.Size(60, 17);
            this.rbBackupEnableEnable.TabIndex = 3;
            this.rbBackupEnableEnable.TabStop = true;
            this.rbBackupEnableEnable.Text = "Enable";
            this.rbBackupEnableEnable.UseVisualStyleBackColor = true;
            // 
            // rbBackupEnableDisable
            // 
            this.rbBackupEnableDisable.AutoSize = true;
            this.rbBackupEnableDisable.Location = new System.Drawing.Point(70, 3);
            this.rbBackupEnableDisable.Name = "rbBackupEnableDisable";
            this.rbBackupEnableDisable.Size = new System.Drawing.Size(63, 17);
            this.rbBackupEnableDisable.TabIndex = 4;
            this.rbBackupEnableDisable.Text = "Disable";
            this.rbBackupEnableDisable.UseVisualStyleBackColor = true;
            this.rbBackupEnableDisable.CheckedChanged += new System.EventHandler(this.rbBackupEnableDisable_CheckedChanged);
            // 
            // pnlBackupType
            // 
            this.pnlBackupType.Controls.Add(this.radioButton2);
            this.pnlBackupType.Controls.Add(this.radioButton1);
            this.pnlBackupType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackupType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlBackupType.Location = new System.Drawing.Point(274, 60);
            this.pnlBackupType.Name = "pnlBackupType";
            this.pnlBackupType.Size = new System.Drawing.Size(217, 24);
            this.pnlBackupType.TabIndex = 27;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(70, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(53, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "to DB";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(4, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(57, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "to File";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // lblConnectionsBackupPath
            // 
            this.lblConnectionsBackupPath.AutoSize = true;
            this.lblConnectionsBackupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionsBackupPath.Location = new System.Drawing.Point(103, 177);
            this.lblConnectionsBackupPath.Name = "lblConnectionsBackupPath";
            this.lblConnectionsBackupPath.Size = new System.Drawing.Size(165, 31);
            this.lblConnectionsBackupPath.TabIndex = 13;
            this.lblConnectionsBackupPath.Text = "Backup folder";
            this.lblConnectionsBackupPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConnectionsBackupPath
            // 
            this.txtConnectionsBackupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConnectionsBackupPath.Location = new System.Drawing.Point(274, 182);
            this.txtConnectionsBackupPath.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txtConnectionsBackupPath.Name = "txtConnectionsBackupPath";
            this.txtConnectionsBackupPath.Size = new System.Drawing.Size(217, 22);
            this.txtConnectionsBackupPath.TabIndex = 3;
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.AutoSize = true;
            this.btnBrowsePath.Location = new System.Drawing.Point(497, 180);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(94, 25);
            this.btnBrowsePath.TabIndex = 4;
            this.btnBrowsePath.Text = "Browse";
            this.btnBrowsePath.UseVisualStyleBackColor = true;
            this.btnBrowsePath.Click += new System.EventHandler(this.ButtonBrowsePath_Click);
            // 
            // lblBackupNameFormat
            // 
            this.lblBackupNameFormat.AutoSize = true;
            this.lblBackupNameFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBackupNameFormat.Location = new System.Drawing.Point(103, 147);
            this.lblBackupNameFormat.Name = "lblBackupNameFormat";
            this.lblBackupNameFormat.Size = new System.Drawing.Size(165, 30);
            this.lblBackupNameFormat.TabIndex = 33;
            this.lblBackupNameFormat.Text = "Backup file name format";
            this.lblBackupNameFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBackupNameFormat
            // 
            this.txtBackupNameFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBackupNameFormat.Location = new System.Drawing.Point(274, 152);
            this.txtBackupNameFormat.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txtBackupNameFormat.Name = "txtBackupNameFormat";
            this.txtBackupNameFormat.Size = new System.Drawing.Size(217, 22);
            this.txtBackupNameFormat.TabIndex = 2;
            // 
            // lblBacupPageShowInOptionsMenu
            // 
            this.lblBacupPageShowInOptionsMenu.AutoSize = true;
            this.lblBacupPageShowInOptionsMenu.BackColor = System.Drawing.Color.Salmon;
            this.lblBacupPageShowInOptionsMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBacupPageShowInOptionsMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblBacupPageShowInOptionsMenu.Location = new System.Drawing.Point(100, 0);
            this.lblBacupPageShowInOptionsMenu.Margin = new System.Windows.Forms.Padding(0);
            this.lblBacupPageShowInOptionsMenu.Name = "lblBacupPageShowInOptionsMenu";
            this.lblBacupPageShowInOptionsMenu.Size = new System.Drawing.Size(171, 27);
            this.lblBacupPageShowInOptionsMenu.TabIndex = 0;
            this.lblBacupPageShowInOptionsMenu.Text = "Page control in Options menu";
            this.lblBacupPageShowInOptionsMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBacupPageShowInOptionsMenu.Visible = false;
            // 
            // pnlShowForUser
            // 
            this.pnlShowForUser.BackColor = System.Drawing.Color.Salmon;
            this.pnlShowForUser.Controls.Add(this.cbBacupPageInOptionMenu);
            this.pnlShowForUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlShowForUser.Location = new System.Drawing.Point(271, 0);
            this.pnlShowForUser.Margin = new System.Windows.Forms.Padding(0);
            this.pnlShowForUser.Name = "pnlShowForUser";
            this.pnlShowForUser.Size = new System.Drawing.Size(223, 27);
            this.pnlShowForUser.TabIndex = 0;
            this.pnlShowForUser.Visible = false;
            // 
            // cbBacupPageInOptionMenu
            // 
            this.cbBacupPageInOptionMenu.AutoSize = true;
            this.cbBacupPageInOptionMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbBacupPageInOptionMenu.Location = new System.Drawing.Point(7, 5);
            this.cbBacupPageInOptionMenu.Name = "cbBacupPageInOptionMenu";
            this.cbBacupPageInOptionMenu.Size = new System.Drawing.Size(104, 19);
            this.cbBacupPageInOptionMenu.TabIndex = 1;
            this.cbBacupPageInOptionMenu.Text = "Show for user";
            this.cbBacupPageInOptionMenu.UseVisualStyleBackColor = true;
            // 
            // lblACL
            // 
            this.lblACL.AutoSize = true;
            this.lblACL.BackColor = System.Drawing.Color.Salmon;
            this.lblACL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblACL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblACL.Location = new System.Drawing.Point(0, 0);
            this.lblACL.Margin = new System.Windows.Forms.Padding(0);
            this.lblACL.Name = "lblACL";
            this.lblACL.Size = new System.Drawing.Size(100, 27);
            this.lblACL.TabIndex = 0;
            this.lblACL.Text = "ACL for user";
            this.lblACL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblACL.Visible = false;
            // 
            // plBackupEnable
            // 
            this.plBackupEnable.BackColor = System.Drawing.Color.Salmon;
            this.plBackupEnable.Controls.Add(this.cbBackupEnableACL);
            this.plBackupEnable.Location = new System.Drawing.Point(0, 27);
            this.plBackupEnable.Margin = new System.Windows.Forms.Padding(0);
            this.plBackupEnable.Name = "plBackupEnable";
            this.plBackupEnable.Size = new System.Drawing.Size(100, 30);
            this.plBackupEnable.TabIndex = 0;
            this.plBackupEnable.Visible = false;
            // 
            // cbBackupEnableACL
            // 
            this.cbBackupEnableACL.BackColor = System.Drawing.Color.Salmon;
            this.cbBackupEnableACL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbBackupEnableACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackupEnableACL.FormattingEnabled = true;
            this.cbBackupEnableACL.Location = new System.Drawing.Point(3, 5);
            this.cbBackupEnableACL.MaxDropDownItems = 3;
            this.cbBackupEnableACL.Name = "cbBackupEnableACL";
            this.cbBackupEnableACL.Size = new System.Drawing.Size(94, 21);
            this.cbBackupEnableACL.TabIndex = 2;
            this.cbBackupEnableACL.Visible = false;
            // 
            // plBackupType
            // 
            this.plBackupType.BackColor = System.Drawing.Color.Salmon;
            this.plBackupType.Controls.Add(this.cbBackupTypeACL);
            this.plBackupType.Location = new System.Drawing.Point(0, 57);
            this.plBackupType.Margin = new System.Windows.Forms.Padding(0);
            this.plBackupType.Name = "plBackupType";
            this.plBackupType.Size = new System.Drawing.Size(100, 30);
            this.plBackupType.TabIndex = 0;
            this.plBackupType.Visible = false;
            // 
            // cbBackupTypeACL
            // 
            this.cbBackupTypeACL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbBackupTypeACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackupTypeACL.FormattingEnabled = true;
            this.cbBackupTypeACL.Location = new System.Drawing.Point(3, 5);
            this.cbBackupTypeACL.MaxDropDownItems = 3;
            this.cbBackupTypeACL.Name = "cbBackupTypeACL";
            this.cbBackupTypeACL.Size = new System.Drawing.Size(94, 21);
            this.cbBackupTypeACL.TabIndex = 0;
            this.cbBackupTypeACL.Visible = false;
            // 
            // plBackupFrequency
            // 
            this.plBackupFrequency.BackColor = System.Drawing.Color.Salmon;
            this.plBackupFrequency.Controls.Add(this.cbBackupFrequencyACL);
            this.plBackupFrequency.Location = new System.Drawing.Point(0, 87);
            this.plBackupFrequency.Margin = new System.Windows.Forms.Padding(0);
            this.plBackupFrequency.Name = "plBackupFrequency";
            this.plBackupFrequency.Size = new System.Drawing.Size(100, 30);
            this.plBackupFrequency.TabIndex = 0;
            this.plBackupFrequency.Visible = false;
            // 
            // cbBackupFrequencyACL
            // 
            this.cbBackupFrequencyACL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbBackupFrequencyACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackupFrequencyACL.FormattingEnabled = true;
            this.cbBackupFrequencyACL.Location = new System.Drawing.Point(3, 5);
            this.cbBackupFrequencyACL.MaxDropDownItems = 3;
            this.cbBackupFrequencyACL.Name = "cbBackupFrequencyACL";
            this.cbBackupFrequencyACL.Size = new System.Drawing.Size(94, 21);
            this.cbBackupFrequencyACL.TabIndex = 0;
            this.cbBackupFrequencyACL.Visible = false;
            // 
            // plBackupNumber
            // 
            this.plBackupNumber.BackColor = System.Drawing.Color.Salmon;
            this.plBackupNumber.Controls.Add(this.cbBackupNumberACL);
            this.plBackupNumber.Location = new System.Drawing.Point(0, 117);
            this.plBackupNumber.Margin = new System.Windows.Forms.Padding(0);
            this.plBackupNumber.Name = "plBackupNumber";
            this.plBackupNumber.Size = new System.Drawing.Size(100, 30);
            this.plBackupNumber.TabIndex = 0;
            this.plBackupNumber.Visible = false;
            // 
            // cbBackupNumberACL
            // 
            this.cbBackupNumberACL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbBackupNumberACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackupNumberACL.FormattingEnabled = true;
            this.cbBackupNumberACL.Location = new System.Drawing.Point(3, 5);
            this.cbBackupNumberACL.MaxDropDownItems = 3;
            this.cbBackupNumberACL.Name = "cbBackupNumberACL";
            this.cbBackupNumberACL.Size = new System.Drawing.Size(94, 21);
            this.cbBackupNumberACL.TabIndex = 0;
            this.cbBackupNumberACL.Visible = false;
            // 
            // plBackupNameFormat
            // 
            this.plBackupNameFormat.BackColor = System.Drawing.Color.Salmon;
            this.plBackupNameFormat.Controls.Add(this.cbBackupNameFormatACL);
            this.plBackupNameFormat.Location = new System.Drawing.Point(0, 147);
            this.plBackupNameFormat.Margin = new System.Windows.Forms.Padding(0);
            this.plBackupNameFormat.Name = "plBackupNameFormat";
            this.plBackupNameFormat.Size = new System.Drawing.Size(100, 30);
            this.plBackupNameFormat.TabIndex = 0;
            this.plBackupNameFormat.Visible = false;
            // 
            // cbBackupNameFormatACL
            // 
            this.cbBackupNameFormatACL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbBackupNameFormatACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackupNameFormatACL.FormattingEnabled = true;
            this.cbBackupNameFormatACL.Location = new System.Drawing.Point(3, 5);
            this.cbBackupNameFormatACL.MaxDropDownItems = 3;
            this.cbBackupNameFormatACL.Name = "cbBackupNameFormatACL";
            this.cbBackupNameFormatACL.Size = new System.Drawing.Size(94, 21);
            this.cbBackupNameFormatACL.TabIndex = 0;
            this.cbBackupNameFormatACL.Visible = false;
            // 
            // plBackupLocation
            // 
            this.plBackupLocation.BackColor = System.Drawing.Color.Salmon;
            this.plBackupLocation.Controls.Add(this.cbBackupLocationACL);
            this.plBackupLocation.Location = new System.Drawing.Point(0, 177);
            this.plBackupLocation.Margin = new System.Windows.Forms.Padding(0);
            this.plBackupLocation.Name = "plBackupLocation";
            this.plBackupLocation.Size = new System.Drawing.Size(100, 30);
            this.plBackupLocation.TabIndex = 0;
            this.plBackupLocation.Visible = false;
            // 
            // cbBackupLocationACL
            // 
            this.cbBackupLocationACL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbBackupLocationACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackupLocationACL.FormattingEnabled = true;
            this.cbBackupLocationACL.Location = new System.Drawing.Point(3, 5);
            this.cbBackupLocationACL.MaxDropDownItems = 3;
            this.cbBackupLocationACL.Name = "cbBackupLocationACL";
            this.cbBackupLocationACL.Size = new System.Drawing.Size(94, 21);
            this.cbBackupLocationACL.TabIndex = 0;
            this.cbBackupLocationACL.Visible = false;
            // 
            // plMakeBackup
            // 
            this.plMakeBackup.Controls.Add(this.cbMakeBackupOnSave);
            this.plMakeBackup.Controls.Add(this.cbMakeBackupOnEdit);
            this.plMakeBackup.Controls.Add(this.cbMakeBackupOnExit);
            this.plMakeBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plMakeBackup.Location = new System.Drawing.Point(274, 90);
            this.plMakeBackup.Name = "plMakeBackup";
            this.plMakeBackup.Size = new System.Drawing.Size(217, 24);
            this.plMakeBackup.TabIndex = 34;
            // 
            // cbMakeBackupOnSave
            // 
            this.cbMakeBackupOnSave.AutoSize = true;
            this.cbMakeBackupOnSave.Location = new System.Drawing.Point(141, 3);
            this.cbMakeBackupOnSave.Name = "cbMakeBackupOnSave";
            this.cbMakeBackupOnSave.Size = new System.Drawing.Size(67, 17);
            this.cbMakeBackupOnSave.TabIndex = 2;
            this.cbMakeBackupOnSave.Text = "On save";
            this.cbMakeBackupOnSave.UseVisualStyleBackColor = true;
            // 
            // cbMakeBackupOnEdit
            // 
            this.cbMakeBackupOnEdit.AutoSize = true;
            this.cbMakeBackupOnEdit.Location = new System.Drawing.Point(70, 3);
            this.cbMakeBackupOnEdit.Name = "cbMakeBackupOnEdit";
            this.cbMakeBackupOnEdit.Size = new System.Drawing.Size(65, 17);
            this.cbMakeBackupOnEdit.TabIndex = 1;
            this.cbMakeBackupOnEdit.Text = "On edit";
            this.cbMakeBackupOnEdit.UseVisualStyleBackColor = true;
            // 
            // cbMakeBackupOnExit
            // 
            this.cbMakeBackupOnExit.AutoSize = true;
            this.cbMakeBackupOnExit.Location = new System.Drawing.Point(4, 4);
            this.cbMakeBackupOnExit.Name = "cbMakeBackupOnExit";
            this.cbMakeBackupOnExit.Size = new System.Drawing.Size(63, 17);
            this.cbMakeBackupOnExit.TabIndex = 0;
            this.cbMakeBackupOnExit.Text = "On exit";
            this.cbMakeBackupOnExit.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Salmon;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(494, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 27);
            this.panel1.TabIndex = 35;
            // 
            // BackupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanelBackupFile);
            this.Name = "BackupPage";
            this.Size = new System.Drawing.Size(761, 633);
            this.tableLayoutPanelBackupFile.ResumeLayout(false);
            this.tableLayoutPanelBackupFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBackups)).EndInit();
            this.pnlBackupEnable.ResumeLayout(false);
            this.pnlBackupEnable.PerformLayout();
            this.pnlBackupType.ResumeLayout(false);
            this.pnlBackupType.PerformLayout();
            this.pnlShowForUser.ResumeLayout(false);
            this.pnlShowForUser.PerformLayout();
            this.plBackupEnable.ResumeLayout(false);
            this.plBackupType.ResumeLayout(false);
            this.plBackupFrequency.ResumeLayout(false);
            this.plBackupNumber.ResumeLayout(false);
            this.plBackupNameFormat.ResumeLayout(false);
            this.plBackupLocation.ResumeLayout(false);
            this.plMakeBackup.ResumeLayout(false);
            this.plMakeBackup.PerformLayout();
            this.ResumeLayout(false);

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