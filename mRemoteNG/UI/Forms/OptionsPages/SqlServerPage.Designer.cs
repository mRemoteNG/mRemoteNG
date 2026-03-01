using mRemoteNG.UI.Controls;
using System;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class SqlServerPage : OptionsPage
    {

        //UserControl overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing) => base.Dispose(disposing);

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlServerPage));
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            lblSectionName = new System.Windows.Forms.Label();
            pnlServerBlock = new System.Windows.Forms.Panel();
            tabCtrlSQL = new System.Windows.Forms.TabControl();
            tabPageProfiles = new System.Windows.Forms.TabPage();
            lstProfiles = new System.Windows.Forms.ListBox();
            btnLoadProfile = new MrngButton();
            btnSaveProfile = new MrngButton();
            btnDeleteProfile = new MrngButton();
            txtProfileName = new MrngTextBox();
            lblProfileName = new MrngLabel();
            tabPage1 = new System.Windows.Forms.TabPage();
            pnlSQLCon = new System.Windows.Forms.TableLayoutPanel();
            chkSQLReadOnly = new System.Windows.Forms.CheckBox();
            chkShowDatabasePickerOnStartup = new System.Windows.Forms.CheckBox();
            lblShowDatabasePickerOnStartup = new MrngLabel();
            txtSQLAuthType = new MrngComboBox();
            lblSQLAuthType = new MrngLabel();
            lblSQLReadOnly = new MrngLabel();
            lblSQLType = new MrngLabel();
            txtSQLType = new MrngComboBox();
            lblSQLServer = new MrngLabel();
            lblSQLUsername = new MrngLabel();
            lblSQLPassword = new MrngLabel();
            txtSQLServer = new MrngTextBox();
            txtSQLPassword = new MrngTextBox();
            txtSQLUsername = new MrngTextBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            txtSQLDatabaseName = new MrngTextBox();
            lblSQLDatabaseName = new MrngLabel();
            numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            mrngLabel19 = new MrngLabel();
            mrngComboBox4 = new MrngComboBox();
            mrngLabel18 = new MrngLabel();
            mrngLabel1 = new MrngLabel();
            mrngLabel9 = new MrngLabel();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            tabPage3 = new System.Windows.Forms.TabPage();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            mrngCheckBox5 = new MrngCheckBox();
            mrngLabel7 = new MrngLabel();
            mrngTextBox5 = new MrngTextBox();
            mrngLabel3 = new MrngLabel();
            mrngComboBox2 = new MrngComboBox();
            mrngCheckBox2 = new MrngCheckBox();
            mrngCheckBox4 = new MrngCheckBox();
            mrngCheckBox3 = new MrngCheckBox();
            mrngComboBox3 = new MrngComboBox();
            mrngLabel11 = new MrngLabel();
            mrngLabel12 = new MrngLabel();
            mrngLabel13 = new MrngLabel();
            mrngLabel14 = new MrngLabel();
            mrngLabel15 = new MrngLabel();
            mrngTextBox8 = new MrngTextBox();
            mrngLabel16 = new MrngLabel();
            mrngLabel17 = new MrngLabel();
            mrngTextBox11 = new MrngTextBox();
            tabPage4 = new System.Windows.Forms.TabPage();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            DCMSetuptxtmandatory4 = new System.Windows.Forms.Label();
            DCMSetuptxtmandatory3 = new System.Windows.Forms.Label();
            DCMSetuptxtmandatory2 = new System.Windows.Forms.Label();
            DCMSetuptxtmandatory1 = new System.Windows.Forms.Label();
            DCMSetuptxtuserpwd = new MrngTextBox();
            DCMSetuptxtuser = new MrngTextBox();
            DCMSetuplbluserpwd = new System.Windows.Forms.Label();
            DCMSetuplbluser = new System.Windows.Forms.Label();
            DCMSetuptxtdbname = new MrngTextBox();
            DCMSetuplbldbname = new System.Windows.Forms.Label();
            DCMSetuptxtadmpwd = new MrngTextBox();
            DCMSetuplbladminpwd = new System.Windows.Forms.Label();
            DCMSetuptxtadmuser = new MrngTextBox();
            DCMSetupRdBtnC = new System.Windows.Forms.RadioButton();
            DCMSetupRdBtnV = new System.Windows.Forms.RadioButton();
            DCMSetuplblschema = new System.Windows.Forms.Label();
            DCMSetupddschema = new System.Windows.Forms.ComboBox();
            DCMSetuplbladminuser = new System.Windows.Forms.Label();
            imgConnectionStatus = new System.Windows.Forms.PictureBox();
            lblTestConnectionResults = new MrngLabel();
            btnTestConnection = new MrngButton();
            btnExpandOptions = new MrngButton();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            mrngTextBox2 = new MrngTextBox();
            mrngLabel4 = new MrngLabel();
            mrngLabel5 = new MrngLabel();
            mrngTextBox1 = new MrngTextBox();
            mrngLabel6 = new MrngLabel();
            mrngTextBox4 = new MrngTextBox();
            label1 = new System.Windows.Forms.Label();
            picboxLogo = new System.Windows.Forms.PictureBox();
            chkUseSQLServer = new System.Windows.Forms.CheckBox();
            frmtoolTip = new System.Windows.Forms.ToolTip(components);
            pnlServerBlock.SuspendLayout();
            tabCtrlSQL.SuspendLayout();
            tabPage1.SuspendLayout();
            pnlSQLCon.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            tabPage3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tabPage4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgConnectionStatus).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picboxLogo).BeginInit();
            SuspendLayout();
            // 
            // lblRegistrySettingsUsedInfo
            // 
            lblRegistrySettingsUsedInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            lblRegistrySettingsUsedInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            lblRegistrySettingsUsedInfo.ForeColor = System.Drawing.Color.IndianRed;
            lblRegistrySettingsUsedInfo.Location = new System.Drawing.Point(3, 46);
            lblRegistrySettingsUsedInfo.Name = "lblRegistrySettingsUsedInfo";
            lblRegistrySettingsUsedInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            lblRegistrySettingsUsedInfo.Size = new System.Drawing.Size(1128, 30);
            lblRegistrySettingsUsedInfo.TabIndex = 24;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSectionName
            // 
            lblSectionName.BackColor = System.Drawing.SystemColors.ControlLight;
            lblSectionName.Dock = System.Windows.Forms.DockStyle.Top;
            lblSectionName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            lblSectionName.ForeColor = System.Drawing.SystemColors.ControlText;
            lblSectionName.Location = new System.Drawing.Point(0, 0);
            lblSectionName.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
            lblSectionName.Name = "lblSectionName";
            lblSectionName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            lblSectionName.Size = new System.Drawing.Size(656, 46);
            lblSectionName.TabIndex = 26;
            lblSectionName.Text = "Database Connection Manager";
            // 
            // pnlServerBlock
            // 
            pnlServerBlock.BackColor = System.Drawing.SystemColors.ControlLight;
            pnlServerBlock.Controls.Add(tabCtrlSQL);
            pnlServerBlock.Controls.Add(imgConnectionStatus);
            pnlServerBlock.Controls.Add(lblTestConnectionResults);
            pnlServerBlock.Controls.Add(btnTestConnection);
            pnlServerBlock.Controls.Add(btnExpandOptions);
            pnlServerBlock.Controls.Add(tableLayoutPanel1);
            pnlServerBlock.Controls.Add(label1);
            pnlServerBlock.Controls.Add(picboxLogo);
            pnlServerBlock.Location = new System.Drawing.Point(15, 120);
            pnlServerBlock.Name = "pnlServerBlock";
            pnlServerBlock.Size = new System.Drawing.Size(492, 324);
            pnlServerBlock.TabIndex = 27;
            pnlServerBlock.Visible = false;
            // 
            // tabCtrlSQL
            // 
            tabCtrlSQL.Controls.Add(tabPageProfiles);
            tabCtrlSQL.Controls.Add(tabPage1);
            tabCtrlSQL.Controls.Add(tabPage2);
            tabCtrlSQL.Controls.Add(tabPage3);
            tabCtrlSQL.Controls.Add(tabPage4);
            tabCtrlSQL.Location = new System.Drawing.Point(8, 3);
            tabCtrlSQL.Name = "tabCtrlSQL";
            tabCtrlSQL.SelectedIndex = 0;
            tabCtrlSQL.Size = new System.Drawing.Size(481, 277);
            tabCtrlSQL.TabIndex = 33;
            tabCtrlSQL.Visible = false;
            // 
            // tabPageProfiles
            // 
            tabPageProfiles.Controls.Add(lstProfiles);
            tabPageProfiles.Controls.Add(lblProfileName);
            tabPageProfiles.Controls.Add(txtProfileName);
            tabPageProfiles.Controls.Add(btnLoadProfile);
            tabPageProfiles.Controls.Add(btnSaveProfile);
            tabPageProfiles.Controls.Add(btnDeleteProfile);
            tabPageProfiles.Location = new System.Drawing.Point(4, 22);
            tabPageProfiles.Name = "tabPageProfiles";
            tabPageProfiles.Padding = new System.Windows.Forms.Padding(3);
            tabPageProfiles.Size = new System.Drawing.Size(473, 251);
            tabPageProfiles.TabIndex = 4;
            tabPageProfiles.Text = "Profiles";
            tabPageProfiles.UseVisualStyleBackColor = true;
            // 
            // lstProfiles
            // 
            lstProfiles.FormattingEnabled = true;
            lstProfiles.Location = new System.Drawing.Point(6, 6);
            lstProfiles.Name = "lstProfiles";
            lstProfiles.Size = new System.Drawing.Size(200, 238);
            lstProfiles.TabIndex = 0;
            // 
            // lblProfileName
            // 
            lblProfileName.AutoSize = true;
            lblProfileName.Location = new System.Drawing.Point(212, 6);
            lblProfileName.Name = "lblProfileName";
            lblProfileName.Size = new System.Drawing.Size(70, 13);
            lblProfileName.TabIndex = 1;
            lblProfileName.Text = "Profile Name:";
            // 
            // txtProfileName
            // 
            txtProfileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtProfileName.Location = new System.Drawing.Point(215, 22);
            txtProfileName.Name = "txtProfileName";
            txtProfileName.Size = new System.Drawing.Size(200, 20);
            txtProfileName.TabIndex = 2;
            // 
            // btnLoadProfile
            // 
            btnLoadProfile.Location = new System.Drawing.Point(215, 48);
            btnLoadProfile.Name = "btnLoadProfile";
            btnLoadProfile.Size = new System.Drawing.Size(75, 23);
            btnLoadProfile.TabIndex = 3;
            btnLoadProfile.Text = "Load";
            btnLoadProfile.UseVisualStyleBackColor = true;
            // 
            // btnSaveProfile
            // 
            btnSaveProfile.Location = new System.Drawing.Point(296, 48);
            btnSaveProfile.Name = "btnSaveProfile";
            btnSaveProfile.Size = new System.Drawing.Size(75, 23);
            btnSaveProfile.TabIndex = 4;
            btnSaveProfile.Text = "Save";
            btnSaveProfile.UseVisualStyleBackColor = true;
            // 
            // btnDeleteProfile
            // 
            btnDeleteProfile.Location = new System.Drawing.Point(377, 48);
            btnDeleteProfile.Name = "btnDeleteProfile";
            btnDeleteProfile.Size = new System.Drawing.Size(75, 23);
            btnDeleteProfile.TabIndex = 5;
            btnDeleteProfile.Text = "Delete";
            btnDeleteProfile.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(pnlSQLCon);
            tabPage1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(473, 251);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Server & Credentials";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnlSQLCon
            // 
            pnlSQLCon.ColumnCount = 2;
            pnlSQLCon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            pnlSQLCon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            pnlSQLCon.Controls.Add(chkSQLReadOnly, 1, 7);
            pnlSQLCon.Controls.Add(lblShowDatabasePickerOnStartup, 0, 8);
            pnlSQLCon.Controls.Add(chkShowDatabasePickerOnStartup, 1, 8);
            pnlSQLCon.Controls.Add(txtSQLAuthType, 1, 3);
            pnlSQLCon.Controls.Add(lblSQLAuthType, 0, 3);
            pnlSQLCon.Controls.Add(lblSQLReadOnly, 0, 7);
            pnlSQLCon.Controls.Add(lblSQLType, 0, 0);
            pnlSQLCon.Controls.Add(txtSQLType, 1, 0);
            pnlSQLCon.Controls.Add(lblSQLServer, 0, 1);
            pnlSQLCon.Controls.Add(lblSQLUsername, 0, 4);
            pnlSQLCon.Controls.Add(lblSQLPassword, 0, 5);
            pnlSQLCon.Controls.Add(txtSQLServer, 1, 1);
            pnlSQLCon.Controls.Add(txtSQLPassword, 1, 5);
            pnlSQLCon.Controls.Add(txtSQLUsername, 1, 4);
            pnlSQLCon.Location = new System.Drawing.Point(-3, 9);
            pnlSQLCon.Name = "pnlSQLCon";
            pnlSQLCon.RowCount = 9;
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            pnlSQLCon.RowStyles.Add(new System.Windows.Forms.RowStyle());
            pnlSQLCon.Size = new System.Drawing.Size(458, 223);
            pnlSQLCon.TabIndex = 23;
            // 
            // chkSQLReadOnly
            // 
            chkSQLReadOnly.Anchor = System.Windows.Forms.AnchorStyles.Left;
            chkSQLReadOnly.AutoSize = true;
            chkSQLReadOnly.Location = new System.Drawing.Point(163, 188);
            chkSQLReadOnly.Name = "chkSQLReadOnly";
            chkSQLReadOnly.Size = new System.Drawing.Size(15, 14);
            chkSQLReadOnly.TabIndex = 24;
            chkSQLReadOnly.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkSQLReadOnly.UseVisualStyleBackColor = true;
            // 
            // chkShowDatabasePickerOnStartup
            // 
            chkShowDatabasePickerOnStartup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            chkShowDatabasePickerOnStartup.AutoSize = true;
            chkShowDatabasePickerOnStartup.Location = new System.Drawing.Point(163, 214);
            chkShowDatabasePickerOnStartup.Name = "chkShowDatabasePickerOnStartup";
            chkShowDatabasePickerOnStartup.Size = new System.Drawing.Size(15, 14);
            chkShowDatabasePickerOnStartup.TabIndex = 25;
            chkShowDatabasePickerOnStartup.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            chkShowDatabasePickerOnStartup.UseVisualStyleBackColor = true;
            // 
            // lblShowDatabasePickerOnStartup
            // 
            lblShowDatabasePickerOnStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            lblShowDatabasePickerOnStartup.Location = new System.Drawing.Point(3, 208);
            lblShowDatabasePickerOnStartup.Name = "lblShowDatabasePickerOnStartup";
            lblShowDatabasePickerOnStartup.Size = new System.Drawing.Size(154, 26);
            lblShowDatabasePickerOnStartup.TabIndex = 26;
            lblShowDatabasePickerOnStartup.Text = "Show picker on startup:";
            lblShowDatabasePickerOnStartup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLAuthType
            // 
            txtSQLAuthType._mice = MrngComboBox.MouseState.HOVER;
            txtSQLAuthType.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLAuthType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            txtSQLAuthType.FormattingEnabled = true;
            txtSQLAuthType.Items.AddRange(new object[] { "Windows Authentication", "SQL Server Authentication", "Microsoft Entra MFA", "Microsoft Entra Password", "Microsoft Entra Integrated", "Microsoft Entra Service Principal", "Microsoft Entra Managed Identity", "Microsoft Entra Default" });
            txtSQLAuthType.Location = new System.Drawing.Point(163, 81);
            txtSQLAuthType.Name = "txtSQLAuthType";
            txtSQLAuthType.Size = new System.Drawing.Size(292, 21);
            txtSQLAuthType.TabIndex = 24;
            txtSQLAuthType.SelectedIndexChanged += txtSQLAuthType_SelectedIndexChanged;
            // 
            // lblSQLAuthType
            // 
            lblSQLAuthType.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLAuthType.Location = new System.Drawing.Point(3, 78);
            lblSQLAuthType.Name = "lblSQLAuthType";
            lblSQLAuthType.Size = new System.Drawing.Size(154, 26);
            lblSQLAuthType.TabIndex = 23;
            lblSQLAuthType.Text = "Autentifcation:";
            lblSQLAuthType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSQLReadOnly
            // 
            lblSQLReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLReadOnly.Location = new System.Drawing.Point(3, 182);
            lblSQLReadOnly.Name = "lblSQLReadOnly";
            lblSQLReadOnly.Size = new System.Drawing.Size(154, 26);
            lblSQLReadOnly.TabIndex = 22;
            lblSQLReadOnly.Text = "Access for Read Only:";
            lblSQLReadOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            frmtoolTip.SetToolTip(lblSQLReadOnly, "Data from db will be loaded but not saved");
            // 
            // lblSQLType
            // 
            lblSQLType.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLType.Location = new System.Drawing.Point(3, 0);
            lblSQLType.Name = "lblSQLType";
            lblSQLType.Size = new System.Drawing.Size(154, 26);
            lblSQLType.TabIndex = 20;
            lblSQLType.Text = "Database Platform:";
            lblSQLType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLType
            // 
            txtSQLType._mice = MrngComboBox.MouseState.HOVER;
            txtSQLType.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            txtSQLType.FormattingEnabled = true;
            txtSQLType.Items.AddRange(new object[] { "MSSQL - developed by Microsoft", "MySQL - developed by Oracle" });
            txtSQLType.Location = new System.Drawing.Point(163, 3);
            txtSQLType.Name = "txtSQLType";
            txtSQLType.Size = new System.Drawing.Size(292, 21);
            txtSQLType.TabIndex = 21;
            // 
            // lblSQLServer
            // 
            lblSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLServer.Location = new System.Drawing.Point(3, 26);
            lblSQLServer.Name = "lblSQLServer";
            lblSQLServer.Size = new System.Drawing.Size(154, 26);
            lblSQLServer.TabIndex = 3;
            lblSQLServer.Text = "Server name or IP:";
            lblSQLServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSQLUsername
            // 
            lblSQLUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLUsername.Location = new System.Drawing.Point(3, 104);
            lblSQLUsername.Name = "lblSQLUsername";
            lblSQLUsername.Size = new System.Drawing.Size(154, 26);
            lblSQLUsername.TabIndex = 7;
            lblSQLUsername.Text = "Username:";
            lblSQLUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSQLPassword
            // 
            lblSQLPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLPassword.Location = new System.Drawing.Point(3, 130);
            lblSQLPassword.Name = "lblSQLPassword";
            lblSQLPassword.Size = new System.Drawing.Size(154, 26);
            lblSQLPassword.TabIndex = 9;
            lblSQLPassword.Text = "Password:";
            lblSQLPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSQLServer
            // 
            txtSQLServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLServer.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLServer.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            txtSQLServer.Location = new System.Drawing.Point(163, 29);
            txtSQLServer.Name = "txtSQLServer";
            txtSQLServer.Size = new System.Drawing.Size(292, 22);
            txtSQLServer.TabIndex = 4;
            // 
            // txtSQLPassword
            // 
            txtSQLPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            txtSQLPassword.Location = new System.Drawing.Point(163, 133);
            txtSQLPassword.Name = "txtSQLPassword";
            txtSQLPassword.Size = new System.Drawing.Size(292, 22);
            txtSQLPassword.TabIndex = 10;
            txtSQLPassword.UseSystemPasswordChar = true;
            // 
            // txtSQLUsername
            // 
            txtSQLUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            txtSQLUsername.Location = new System.Drawing.Point(163, 107);
            txtSQLUsername.Name = "txtSQLUsername";
            txtSQLUsername.Size = new System.Drawing.Size(292, 22);
            txtSQLUsername.TabIndex = 8;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tabPage2.Location = new System.Drawing.Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(473, 251);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Connection Properties";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(txtSQLDatabaseName, 1, 0);
            tableLayoutPanel2.Controls.Add(lblSQLDatabaseName, 0, 0);
            tableLayoutPanel2.Controls.Add(numericUpDown3, 1, 4);
            tableLayoutPanel2.Controls.Add(numericUpDown2, 1, 3);
            tableLayoutPanel2.Controls.Add(mrngLabel19, 0, 2);
            tableLayoutPanel2.Controls.Add(mrngComboBox4, 1, 1);
            tableLayoutPanel2.Controls.Add(mrngLabel18, 0, 1);
            tableLayoutPanel2.Controls.Add(mrngLabel1, 0, 3);
            tableLayoutPanel2.Controls.Add(mrngLabel9, 0, 4);
            tableLayoutPanel2.Controls.Add(numericUpDown1, 1, 2);
            tableLayoutPanel2.Location = new System.Drawing.Point(-3, 9);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new System.Drawing.Size(458, 191);
            tableLayoutPanel2.TabIndex = 23;
            // 
            // txtSQLDatabaseName
            // 
            txtSQLDatabaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSQLDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSQLDatabaseName.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            txtSQLDatabaseName.Location = new System.Drawing.Point(163, 3);
            txtSQLDatabaseName.Name = "txtSQLDatabaseName";
            txtSQLDatabaseName.Size = new System.Drawing.Size(292, 22);
            txtSQLDatabaseName.TabIndex = 32;
            // 
            // lblSQLDatabaseName
            // 
            lblSQLDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSQLDatabaseName.Location = new System.Drawing.Point(3, 0);
            lblSQLDatabaseName.Name = "lblSQLDatabaseName";
            lblSQLDatabaseName.Size = new System.Drawing.Size(154, 26);
            lblSQLDatabaseName.TabIndex = 31;
            lblSQLDatabaseName.Text = "Database name:";
            lblSQLDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new System.Drawing.Point(163, 107);
            numericUpDown3.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new System.Drawing.Size(120, 22);
            numericUpDown3.TabIndex = 30;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new System.Drawing.Point(163, 81);
            numericUpDown2.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new System.Drawing.Size(120, 22);
            numericUpDown2.TabIndex = 29;
            numericUpDown2.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // mrngLabel19
            // 
            mrngLabel19.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel19.Location = new System.Drawing.Point(3, 52);
            mrngLabel19.Name = "mrngLabel19";
            mrngLabel19.Size = new System.Drawing.Size(154, 26);
            mrngLabel19.TabIndex = 27;
            mrngLabel19.Text = "Network packet size (bytes):";
            mrngLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngComboBox4
            // 
            mrngComboBox4._mice = MrngComboBox.MouseState.HOVER;
            mrngComboBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngComboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            mrngComboBox4.FormattingEnabled = true;
            mrngComboBox4.Items.AddRange(new object[] { "<default>", "Named Pipes", "Shared Memory", "TCP/IP" });
            mrngComboBox4.Location = new System.Drawing.Point(163, 29);
            mrngComboBox4.Name = "mrngComboBox4";
            mrngComboBox4.Size = new System.Drawing.Size(292, 21);
            mrngComboBox4.TabIndex = 26;
            // 
            // mrngLabel18
            // 
            mrngLabel18.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel18.Location = new System.Drawing.Point(3, 26);
            mrngLabel18.Name = "mrngLabel18";
            mrngLabel18.Size = new System.Drawing.Size(154, 26);
            mrngLabel18.TabIndex = 25;
            mrngLabel18.Text = "Network protocol:";
            mrngLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel1
            // 
            mrngLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel1.Location = new System.Drawing.Point(3, 78);
            mrngLabel1.Name = "mrngLabel1";
            mrngLabel1.Size = new System.Drawing.Size(154, 26);
            mrngLabel1.TabIndex = 23;
            mrngLabel1.Text = "Connection time-out (s):";
            mrngLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel9
            // 
            mrngLabel9.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel9.Location = new System.Drawing.Point(3, 104);
            mrngLabel9.Name = "mrngLabel9";
            mrngLabel9.Size = new System.Drawing.Size(154, 26);
            mrngLabel9.TabIndex = 7;
            mrngLabel9.Text = "Execution time-out (s):";
            mrngLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new System.Drawing.Point(163, 55);
            numericUpDown1.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(120, 22);
            numericUpDown1.TabIndex = 28;
            numericUpDown1.Value = new decimal(new int[] { 4096, 0, 0, 0 });
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(tableLayoutPanel3);
            tabPage3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tabPage3.Location = new System.Drawing.Point(4, 22);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(473, 251);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Security";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(mrngCheckBox5, 1, 8);
            tableLayoutPanel3.Controls.Add(mrngLabel7, 0, 8);
            tableLayoutPanel3.Controls.Add(mrngTextBox5, 1, 7);
            tableLayoutPanel3.Controls.Add(mrngLabel3, 0, 7);
            tableLayoutPanel3.Controls.Add(mrngComboBox2, 1, 6);
            tableLayoutPanel3.Controls.Add(mrngCheckBox2, 1, 5);
            tableLayoutPanel3.Controls.Add(mrngCheckBox4, 1, 1);
            tableLayoutPanel3.Controls.Add(mrngCheckBox3, 1, 0);
            tableLayoutPanel3.Controls.Add(mrngComboBox3, 1, 3);
            tableLayoutPanel3.Controls.Add(mrngLabel11, 0, 3);
            tableLayoutPanel3.Controls.Add(mrngLabel12, 0, 6);
            tableLayoutPanel3.Controls.Add(mrngLabel13, 0, 0);
            tableLayoutPanel3.Controls.Add(mrngLabel14, 0, 1);
            tableLayoutPanel3.Controls.Add(mrngLabel15, 0, 2);
            tableLayoutPanel3.Controls.Add(mrngTextBox8, 1, 2);
            tableLayoutPanel3.Controls.Add(mrngLabel16, 0, 4);
            tableLayoutPanel3.Controls.Add(mrngLabel17, 0, 5);
            tableLayoutPanel3.Controls.Add(mrngTextBox11, 1, 4);
            tableLayoutPanel3.Location = new System.Drawing.Point(-3, 9);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 10;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel3.Size = new System.Drawing.Size(458, 233);
            tableLayoutPanel3.TabIndex = 23;
            // 
            // mrngCheckBox5
            // 
            mrngCheckBox5._mice = MrngCheckBox.MouseState.OUT;
            mrngCheckBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            mrngCheckBox5.AutoSize = true;
            mrngCheckBox5.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngCheckBox5.Location = new System.Drawing.Point(163, 211);
            mrngCheckBox5.Name = "mrngCheckBox5";
            mrngCheckBox5.Size = new System.Drawing.Size(15, 20);
            mrngCheckBox5.TabIndex = 33;
            mrngCheckBox5.UseVisualStyleBackColor = true;
            // 
            // mrngLabel7
            // 
            mrngLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel7.Location = new System.Drawing.Point(3, 208);
            mrngLabel7.Name = "mrngLabel7";
            mrngLabel7.Size = new System.Drawing.Size(154, 26);
            mrngLabel7.TabIndex = 32;
            mrngLabel7.Text = "Trust server certificate:";
            mrngLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngTextBox5
            // 
            mrngTextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mrngTextBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngTextBox5.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngTextBox5.Location = new System.Drawing.Point(163, 185);
            mrngTextBox5.Name = "mrngTextBox5";
            mrngTextBox5.Size = new System.Drawing.Size(292, 22);
            mrngTextBox5.TabIndex = 31;
            // 
            // mrngLabel3
            // 
            mrngLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel3.Location = new System.Drawing.Point(3, 182);
            mrngLabel3.Name = "mrngLabel3";
            mrngLabel3.Size = new System.Drawing.Size(154, 26);
            mrngLabel3.TabIndex = 30;
            mrngLabel3.Text = "Host name in certificate:";
            mrngLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngComboBox2
            // 
            mrngComboBox2._mice = MrngComboBox.MouseState.HOVER;
            mrngComboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            mrngComboBox2.FormattingEnabled = true;
            mrngComboBox2.Items.AddRange(new object[] { "Optional", "Mandatory", "Strict (SQL Server 2022 and Azure SQL)" });
            mrngComboBox2.Location = new System.Drawing.Point(163, 159);
            mrngComboBox2.Name = "mrngComboBox2";
            mrngComboBox2.Size = new System.Drawing.Size(292, 21);
            mrngComboBox2.TabIndex = 29;
            // 
            // mrngCheckBox2
            // 
            mrngCheckBox2._mice = MrngCheckBox.MouseState.OUT;
            mrngCheckBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            mrngCheckBox2.AutoSize = true;
            mrngCheckBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngCheckBox2.Location = new System.Drawing.Point(163, 133);
            mrngCheckBox2.Name = "mrngCheckBox2";
            mrngCheckBox2.Size = new System.Drawing.Size(15, 20);
            mrngCheckBox2.TabIndex = 27;
            mrngCheckBox2.UseVisualStyleBackColor = true;
            // 
            // mrngCheckBox4
            // 
            mrngCheckBox4._mice = MrngCheckBox.MouseState.OUT;
            mrngCheckBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            mrngCheckBox4.AutoSize = true;
            mrngCheckBox4.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngCheckBox4.Location = new System.Drawing.Point(163, 29);
            mrngCheckBox4.Name = "mrngCheckBox4";
            mrngCheckBox4.Size = new System.Drawing.Size(15, 20);
            mrngCheckBox4.TabIndex = 26;
            mrngCheckBox4.UseVisualStyleBackColor = true;
            // 
            // mrngCheckBox3
            // 
            mrngCheckBox3._mice = MrngCheckBox.MouseState.OUT;
            mrngCheckBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            mrngCheckBox3.AutoSize = true;
            mrngCheckBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngCheckBox3.Location = new System.Drawing.Point(163, 3);
            mrngCheckBox3.Name = "mrngCheckBox3";
            mrngCheckBox3.Size = new System.Drawing.Size(15, 20);
            mrngCheckBox3.TabIndex = 25;
            mrngCheckBox3.UseVisualStyleBackColor = true;
            // 
            // mrngComboBox3
            // 
            mrngComboBox3._mice = MrngComboBox.MouseState.HOVER;
            mrngComboBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            mrngComboBox3.FormattingEnabled = true;
            mrngComboBox3.Items.AddRange(new object[] { "None", "Host Guardian Service", "Microsoft Azure Attestation" });
            mrngComboBox3.Location = new System.Drawing.Point(163, 81);
            mrngComboBox3.Name = "mrngComboBox3";
            mrngComboBox3.Size = new System.Drawing.Size(292, 21);
            mrngComboBox3.TabIndex = 24;
            // 
            // mrngLabel11
            // 
            mrngLabel11.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel11.Location = new System.Drawing.Point(3, 78);
            mrngLabel11.Name = "mrngLabel11";
            mrngLabel11.Size = new System.Drawing.Size(154, 26);
            mrngLabel11.TabIndex = 23;
            mrngLabel11.Text = "Protocol:";
            mrngLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel12
            // 
            mrngLabel12.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel12.Location = new System.Drawing.Point(3, 156);
            mrngLabel12.Name = "mrngLabel12";
            mrngLabel12.Size = new System.Drawing.Size(154, 26);
            mrngLabel12.TabIndex = 22;
            mrngLabel12.Text = "Encryption:";
            mrngLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel13
            // 
            mrngLabel13.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel13.Location = new System.Drawing.Point(3, 0);
            mrngLabel13.Name = "mrngLabel13";
            mrngLabel13.Size = new System.Drawing.Size(154, 26);
            mrngLabel13.TabIndex = 20;
            mrngLabel13.Text = "Enable Always Encrypted:";
            mrngLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel14
            // 
            mrngLabel14.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel14.Location = new System.Drawing.Point(3, 26);
            mrngLabel14.Name = "mrngLabel14";
            mrngLabel14.Size = new System.Drawing.Size(154, 26);
            mrngLabel14.TabIndex = 3;
            mrngLabel14.Text = "Enable secure enclaves:";
            mrngLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel15
            // 
            mrngLabel15.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel15.Location = new System.Drawing.Point(3, 52);
            mrngLabel15.Name = "mrngLabel15";
            mrngLabel15.Size = new System.Drawing.Size(154, 26);
            mrngLabel15.TabIndex = 5;
            mrngLabel15.Text = "Enclave attestation:";
            mrngLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngTextBox8
            // 
            mrngTextBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mrngTextBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngTextBox8.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngTextBox8.Location = new System.Drawing.Point(163, 55);
            mrngTextBox8.Name = "mrngTextBox8";
            mrngTextBox8.Size = new System.Drawing.Size(292, 22);
            mrngTextBox8.TabIndex = 6;
            // 
            // mrngLabel16
            // 
            mrngLabel16.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel16.Location = new System.Drawing.Point(3, 104);
            mrngLabel16.Name = "mrngLabel16";
            mrngLabel16.Size = new System.Drawing.Size(154, 26);
            mrngLabel16.TabIndex = 7;
            mrngLabel16.Text = "URL:";
            mrngLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel17
            // 
            mrngLabel17.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel17.Location = new System.Drawing.Point(3, 130);
            mrngLabel17.Name = "mrngLabel17";
            mrngLabel17.Size = new System.Drawing.Size(154, 26);
            mrngLabel17.TabIndex = 9;
            mrngLabel17.Text = "Enable MARS:";
            mrngLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngTextBox11
            // 
            mrngTextBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mrngTextBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngTextBox11.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngTextBox11.Location = new System.Drawing.Point(163, 107);
            mrngTextBox11.Name = "mrngTextBox11";
            mrngTextBox11.Size = new System.Drawing.Size(292, 22);
            mrngTextBox11.TabIndex = 8;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(tableLayoutPanel4);
            tabPage4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tabPage4.Location = new System.Drawing.Point(4, 22);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3);
            tabPage4.Size = new System.Drawing.Size(473, 251);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Setup";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel4.Controls.Add(DCMSetuptxtmandatory4, 2, 4);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtmandatory3, 2, 3);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtmandatory2, 2, 2);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtmandatory1, 2, 1);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtuserpwd, 1, 6);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtuser, 1, 5);
            tableLayoutPanel4.Controls.Add(DCMSetuplbluserpwd, 0, 6);
            tableLayoutPanel4.Controls.Add(DCMSetuplbluser, 0, 5);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtdbname, 1, 4);
            tableLayoutPanel4.Controls.Add(DCMSetuplbldbname, 0, 4);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtadmpwd, 1, 3);
            tableLayoutPanel4.Controls.Add(DCMSetuplbladminpwd, 0, 3);
            tableLayoutPanel4.Controls.Add(DCMSetuptxtadmuser, 1, 2);
            tableLayoutPanel4.Controls.Add(DCMSetupRdBtnC, 1, 0);
            tableLayoutPanel4.Controls.Add(DCMSetupRdBtnV, 0, 0);
            tableLayoutPanel4.Controls.Add(DCMSetuplblschema, 0, 1);
            tableLayoutPanel4.Controls.Add(DCMSetupddschema, 1, 1);
            tableLayoutPanel4.Controls.Add(DCMSetuplbladminuser, 0, 2);
            tableLayoutPanel4.Location = new System.Drawing.Point(8, 6);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 8;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.Size = new System.Drawing.Size(452, 225);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // DCMSetuptxtmandatory4
            // 
            DCMSetuptxtmandatory4.Anchor = System.Windows.Forms.AnchorStyles.None;
            DCMSetuptxtmandatory4.AutoSize = true;
            DCMSetuptxtmandatory4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            DCMSetuptxtmandatory4.Location = new System.Drawing.Point(435, 126);
            DCMSetuptxtmandatory4.Name = "DCMSetuptxtmandatory4";
            DCMSetuptxtmandatory4.Size = new System.Drawing.Size(14, 17);
            DCMSetuptxtmandatory4.TabIndex = 44;
            DCMSetuptxtmandatory4.Text = "*";
            // 
            // DCMSetuptxtmandatory3
            // 
            DCMSetuptxtmandatory3.Anchor = System.Windows.Forms.AnchorStyles.None;
            DCMSetuptxtmandatory3.AutoSize = true;
            DCMSetuptxtmandatory3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            DCMSetuptxtmandatory3.Location = new System.Drawing.Point(435, 96);
            DCMSetuptxtmandatory3.Name = "DCMSetuptxtmandatory3";
            DCMSetuptxtmandatory3.Size = new System.Drawing.Size(14, 17);
            DCMSetuptxtmandatory3.TabIndex = 43;
            DCMSetuptxtmandatory3.Text = "*";
            // 
            // DCMSetuptxtmandatory2
            // 
            DCMSetuptxtmandatory2.Anchor = System.Windows.Forms.AnchorStyles.None;
            DCMSetuptxtmandatory2.AutoSize = true;
            DCMSetuptxtmandatory2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            DCMSetuptxtmandatory2.Location = new System.Drawing.Point(435, 66);
            DCMSetuptxtmandatory2.Name = "DCMSetuptxtmandatory2";
            DCMSetuptxtmandatory2.Size = new System.Drawing.Size(14, 17);
            DCMSetuptxtmandatory2.TabIndex = 42;
            DCMSetuptxtmandatory2.Text = "*";
            // 
            // DCMSetuptxtmandatory1
            // 
            DCMSetuptxtmandatory1.Anchor = System.Windows.Forms.AnchorStyles.None;
            DCMSetuptxtmandatory1.AutoSize = true;
            DCMSetuptxtmandatory1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            DCMSetuptxtmandatory1.Location = new System.Drawing.Point(435, 36);
            DCMSetuptxtmandatory1.Name = "DCMSetuptxtmandatory1";
            DCMSetuptxtmandatory1.Size = new System.Drawing.Size(14, 17);
            DCMSetuptxtmandatory1.TabIndex = 41;
            DCMSetuptxtmandatory1.Text = "*";
            // 
            // DCMSetuptxtuserpwd
            // 
            DCMSetuptxtuserpwd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            DCMSetuptxtuserpwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DCMSetuptxtuserpwd.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            DCMSetuptxtuserpwd.Location = new System.Drawing.Point(219, 184);
            DCMSetuptxtuserpwd.Name = "DCMSetuptxtuserpwd";
            DCMSetuptxtuserpwd.Size = new System.Drawing.Size(210, 22);
            DCMSetuptxtuserpwd.TabIndex = 38;
            frmtoolTip.SetToolTip(DCMSetuptxtuserpwd, "If provided will be saved");
            DCMSetuptxtuserpwd.UseSystemPasswordChar = true;
            DCMSetuptxtuserpwd.Visible = false;
            // 
            // DCMSetuptxtuser
            // 
            DCMSetuptxtuser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            DCMSetuptxtuser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DCMSetuptxtuser.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            DCMSetuptxtuser.Location = new System.Drawing.Point(219, 154);
            DCMSetuptxtuser.Name = "DCMSetuptxtuser";
            DCMSetuptxtuser.Size = new System.Drawing.Size(210, 22);
            DCMSetuptxtuser.TabIndex = 37;
            frmtoolTip.SetToolTip(DCMSetuptxtuser, "If provided will be saved and grant write permissions to db");
            DCMSetuptxtuser.UseSystemPasswordChar = true;
            DCMSetuptxtuser.Visible = false;
            // 
            // DCMSetuplbluserpwd
            // 
            DCMSetuplbluserpwd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            DCMSetuplbluserpwd.AutoSize = true;
            DCMSetuplbluserpwd.Location = new System.Drawing.Point(111, 188);
            DCMSetuplbluserpwd.Name = "DCMSetuplbluserpwd";
            DCMSetuplbluserpwd.Size = new System.Drawing.Size(102, 13);
            DCMSetuplbluserpwd.TabIndex = 36;
            DCMSetuplbluserpwd.Text = "db user password:";
            DCMSetuplbluserpwd.Visible = false;
            // 
            // DCMSetuplbluser
            // 
            DCMSetuplbluser.Anchor = System.Windows.Forms.AnchorStyles.Right;
            DCMSetuplbluser.AutoSize = true;
            DCMSetuplbluser.Location = new System.Drawing.Point(164, 158);
            DCMSetuplbluser.Name = "DCMSetuplbluser";
            DCMSetuplbluser.Size = new System.Drawing.Size(49, 13);
            DCMSetuplbluser.TabIndex = 34;
            DCMSetuplbluser.Text = "db user:";
            frmtoolTip.SetToolTip(DCMSetuplbluser, "With write permissions");
            DCMSetuplbluser.Visible = false;
            // 
            // DCMSetuptxtdbname
            // 
            DCMSetuptxtdbname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DCMSetuptxtdbname.Dock = System.Windows.Forms.DockStyle.Fill;
            DCMSetuptxtdbname.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            DCMSetuptxtdbname.Location = new System.Drawing.Point(219, 123);
            DCMSetuptxtdbname.Name = "DCMSetuptxtdbname";
            DCMSetuptxtdbname.Size = new System.Drawing.Size(210, 22);
            DCMSetuptxtdbname.TabIndex = 33;
            // 
            // DCMSetuplbldbname
            // 
            DCMSetuplbldbname.Anchor = System.Windows.Forms.AnchorStyles.Right;
            DCMSetuplbldbname.AutoSize = true;
            DCMSetuplbldbname.Location = new System.Drawing.Point(123, 128);
            DCMSetuplbldbname.Name = "DCMSetuplbldbname";
            DCMSetuplbldbname.Size = new System.Drawing.Size(90, 13);
            DCMSetuplbldbname.TabIndex = 14;
            DCMSetuplbldbname.Text = "Database name:";
            // 
            // DCMSetuptxtadmpwd
            // 
            DCMSetuptxtadmpwd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            DCMSetuptxtadmpwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DCMSetuptxtadmpwd.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            DCMSetuptxtadmpwd.Location = new System.Drawing.Point(219, 94);
            DCMSetuptxtadmpwd.Name = "DCMSetuptxtadmpwd";
            DCMSetuptxtadmpwd.Size = new System.Drawing.Size(210, 22);
            DCMSetuptxtadmpwd.TabIndex = 13;
            frmtoolTip.SetToolTip(DCMSetuptxtadmpwd, "Will be used but not saved");
            DCMSetuptxtadmpwd.UseSystemPasswordChar = true;
            // 
            // DCMSetuplbladminpwd
            // 
            DCMSetuplbladminpwd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            DCMSetuplbladminpwd.AutoSize = true;
            DCMSetuplbladminpwd.Location = new System.Drawing.Point(100, 98);
            DCMSetuplbladminpwd.Name = "DCMSetuplbladminpwd";
            DCMSetuplbladminpwd.Size = new System.Drawing.Size(113, 13);
            DCMSetuplbladminpwd.TabIndex = 12;
            DCMSetuplbladminpwd.Text = "db admin password:";
            // 
            // DCMSetuptxtadmuser
            // 
            DCMSetuptxtadmuser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            DCMSetuptxtadmuser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DCMSetuptxtadmuser.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            DCMSetuptxtadmuser.Location = new System.Drawing.Point(219, 64);
            DCMSetuptxtadmuser.Name = "DCMSetuptxtadmuser";
            DCMSetuptxtadmuser.Size = new System.Drawing.Size(210, 22);
            DCMSetuptxtadmuser.TabIndex = 11;
            frmtoolTip.SetToolTip(DCMSetuptxtadmuser, "Will be used but not saved");
            DCMSetuptxtadmuser.UseSystemPasswordChar = true;
            // 
            // DCMSetupRdBtnC
            // 
            DCMSetupRdBtnC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            DCMSetupRdBtnC.AutoSize = true;
            DCMSetupRdBtnC.Location = new System.Drawing.Point(219, 3);
            DCMSetupRdBtnC.Name = "DCMSetupRdBtnC";
            DCMSetupRdBtnC.Size = new System.Drawing.Size(136, 24);
            DCMSetupRdBtnC.TabIndex = 1;
            DCMSetupRdBtnC.TabStop = true;
            DCMSetupRdBtnC.Text = "Create table structure";
            DCMSetupRdBtnC.UseVisualStyleBackColor = true;
            DCMSetupRdBtnC.CheckedChanged += DCMSetupRdBtnC_CheckedChanged;
            // 
            // DCMSetupRdBtnV
            // 
            DCMSetupRdBtnV.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            DCMSetupRdBtnV.AutoSize = true;
            DCMSetupRdBtnV.Checked = true;
            DCMSetupRdBtnV.Location = new System.Drawing.Point(81, 3);
            DCMSetupRdBtnV.Name = "DCMSetupRdBtnV";
            DCMSetupRdBtnV.Size = new System.Drawing.Size(132, 24);
            DCMSetupRdBtnV.TabIndex = 0;
            DCMSetupRdBtnV.TabStop = true;
            DCMSetupRdBtnV.Text = "Verify table structure";
            DCMSetupRdBtnV.UseVisualStyleBackColor = true;
            DCMSetupRdBtnV.CheckedChanged += DCMSetupRdBtnV_CheckedChanged;
            // 
            // DCMSetuplblschema
            // 
            DCMSetuplblschema.Anchor = System.Windows.Forms.AnchorStyles.Right;
            DCMSetuplblschema.AutoSize = true;
            DCMSetuplblschema.Location = new System.Drawing.Point(122, 38);
            DCMSetuplblschema.Name = "DCMSetuplblschema";
            DCMSetuplblschema.Size = new System.Drawing.Size(91, 13);
            DCMSetuplblschema.TabIndex = 2;
            DCMSetuplblschema.Text = "Choose schema:";
            // 
            // DCMSetupddschema
            // 
            DCMSetupddschema.Anchor = System.Windows.Forms.AnchorStyles.Left;
            DCMSetupddschema.FormattingEnabled = true;
            DCMSetupddschema.Location = new System.Drawing.Point(219, 34);
            DCMSetupddschema.Name = "DCMSetupddschema";
            DCMSetupddschema.Size = new System.Drawing.Size(210, 21);
            DCMSetupddschema.TabIndex = 3;
            // 
            // DCMSetuplbladminuser
            // 
            DCMSetuplbladminuser.Anchor = System.Windows.Forms.AnchorStyles.Right;
            DCMSetuplbladminuser.AutoSize = true;
            DCMSetuplbladminuser.Location = new System.Drawing.Point(127, 68);
            DCMSetuplbladminuser.Name = "DCMSetuplbladminuser";
            DCMSetuplbladminuser.Size = new System.Drawing.Size(86, 13);
            DCMSetuplbladminuser.TabIndex = 4;
            DCMSetuplbladminuser.Text = "db admin User:";
            frmtoolTip.SetToolTip(DCMSetuplbladminuser, "With write permissions");
            // 
            // imgConnectionStatus
            // 
            imgConnectionStatus.Image = Properties.Resources.F1Help_16x;
            imgConnectionStatus.Location = new System.Drawing.Point(243, 286);
            imgConnectionStatus.Name = "imgConnectionStatus";
            imgConnectionStatus.Size = new System.Drawing.Size(16, 16);
            imgConnectionStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            imgConnectionStatus.TabIndex = 32;
            imgConnectionStatus.TabStop = false;
            // 
            // lblTestConnectionResults
            // 
            lblTestConnectionResults.AutoSize = true;
            lblTestConnectionResults.Location = new System.Drawing.Point(17, 289);
            lblTestConnectionResults.Name = "lblTestConnectionResults";
            lblTestConnectionResults.Size = new System.Drawing.Size(124, 13);
            lblTestConnectionResults.TabIndex = 31;
            lblTestConnectionResults.Text = "Test connection details";
            // 
            // btnTestConnection
            // 
            btnTestConnection._mice = MrngButton.MouseState.OUT;
            btnTestConnection.Location = new System.Drawing.Point(265, 286);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new System.Drawing.Size(109, 25);
            btnTestConnection.TabIndex = 30;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            // 
            // btnExpandOptions
            // 
            btnExpandOptions._mice = MrngButton.MouseState.OUT;
            btnExpandOptions.Location = new System.Drawing.Point(380, 286);
            btnExpandOptions.Name = "btnExpandOptions";
            btnExpandOptions.Size = new System.Drawing.Size(109, 25);
            btnExpandOptions.TabIndex = 29;
            btnExpandOptions.Text = "Advanced >>";
            btnExpandOptions.UseVisualStyleBackColor = true;
            btnExpandOptions.Click += btnExpandOptions_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(mrngTextBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(mrngLabel4, 0, 0);
            tableLayoutPanel1.Controls.Add(mrngLabel5, 0, 1);
            tableLayoutPanel1.Controls.Add(mrngTextBox1, 1, 1);
            tableLayoutPanel1.Controls.Add(mrngLabel6, 0, 2);
            tableLayoutPanel1.Controls.Add(mrngTextBox4, 1, 2);
            tableLayoutPanel1.Enabled = false;
            tableLayoutPanel1.Location = new System.Drawing.Point(17, 148);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel1.Size = new System.Drawing.Size(458, 81);
            tableLayoutPanel1.TabIndex = 28;
            // 
            // mrngTextBox2
            // 
            mrngTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mrngTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngTextBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngTextBox2.Location = new System.Drawing.Point(163, 3);
            mrngTextBox2.Name = "mrngTextBox2";
            mrngTextBox2.Size = new System.Drawing.Size(292, 22);
            mrngTextBox2.TabIndex = 24;
            // 
            // mrngLabel4
            // 
            mrngLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel4.Location = new System.Drawing.Point(3, 0);
            mrngLabel4.Name = "mrngLabel4";
            mrngLabel4.Size = new System.Drawing.Size(154, 26);
            mrngLabel4.TabIndex = 23;
            mrngLabel4.Text = "Server name or IP:";
            mrngLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngLabel5
            // 
            mrngLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel5.Location = new System.Drawing.Point(3, 26);
            mrngLabel5.Name = "mrngLabel5";
            mrngLabel5.Size = new System.Drawing.Size(154, 26);
            mrngLabel5.TabIndex = 5;
            mrngLabel5.Text = "Database name:";
            mrngLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngTextBox1
            // 
            mrngTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mrngTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngTextBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngTextBox1.Location = new System.Drawing.Point(163, 29);
            mrngTextBox1.Name = "mrngTextBox1";
            mrngTextBox1.Size = new System.Drawing.Size(292, 22);
            mrngTextBox1.TabIndex = 6;
            // 
            // mrngLabel6
            // 
            mrngLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngLabel6.Location = new System.Drawing.Point(3, 52);
            mrngLabel6.Name = "mrngLabel6";
            mrngLabel6.Size = new System.Drawing.Size(154, 26);
            mrngLabel6.TabIndex = 7;
            mrngLabel6.Text = "Username:";
            mrngLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mrngTextBox4
            // 
            mrngTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mrngTextBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            mrngTextBox4.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            mrngTextBox4.Location = new System.Drawing.Point(163, 55);
            mrngTextBox4.Name = "mrngTextBox4";
            mrngTextBox4.Size = new System.Drawing.Size(292, 22);
            mrngTextBox4.TabIndex = 8;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            label1.BackColor = System.Drawing.SystemColors.ControlLight;
            label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
            label1.ForeColor = System.Drawing.SystemColors.ControlText;
            label1.Location = new System.Drawing.Point(163, 69);
            label1.Margin = new System.Windows.Forms.Padding(10, 10, 3, 0);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            label1.Size = new System.Drawing.Size(275, 46);
            label1.TabIndex = 27;
            label1.Text = "Our Prod DB";
            label1.Visible = false;
            // 
            // picboxLogo
            // 
            picboxLogo.BackColor = System.Drawing.SystemColors.Control;
            picboxLogo.Image = (System.Drawing.Image)resources.GetObject("picboxLogo.Image");
            picboxLogo.Location = new System.Drawing.Point(47, 39);
            picboxLogo.Margin = new System.Windows.Forms.Padding(10);
            picboxLogo.Name = "picboxLogo";
            picboxLogo.Padding = new System.Windows.Forms.Padding(5);
            picboxLogo.Size = new System.Drawing.Size(94, 76);
            picboxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            picboxLogo.TabIndex = 0;
            picboxLogo.TabStop = false;
            // 
            // chkUseSQLServer
            // 
            chkUseSQLServer.AutoSize = true;
            chkUseSQLServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
            chkUseSQLServer.Location = new System.Drawing.Point(15, 89);
            chkUseSQLServer.Name = "chkUseSQLServer";
            chkUseSQLServer.Size = new System.Drawing.Size(240, 25);
            chkUseSQLServer.TabIndex = 34;
            chkUseSQLServer.Text = "Enable SQL Server Integration:";
            chkUseSQLServer.UseVisualStyleBackColor = true;
            chkUseSQLServer.CheckedChanged += chkUseSQLServer_CheckedChanged;
            // 
            // SqlServerPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(chkUseSQLServer);
            Controls.Add(pnlServerBlock);
            Controls.Add(lblSectionName);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "SqlServerPage";
            Size = new System.Drawing.Size(656, 490);
            Load += SqlServerPage_Load;
            pnlServerBlock.ResumeLayout(false);
            pnlServerBlock.PerformLayout();
            tabCtrlSQL.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            pnlSQLCon.ResumeLayout(false);
            pnlSQLCon.PerformLayout();
            tabPage2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            tabPage3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgConnectionStatus).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picboxLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
        internal System.Windows.Forms.Label lblSectionName;
        private System.Windows.Forms.Panel pnlServerBlock;
        private System.Windows.Forms.PictureBox picboxLogo;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal MrngLabel mrngLabel5;
        internal MrngTextBox mrngTextBox1;
        internal MrngLabel mrngLabel6;
        internal MrngTextBox mrngTextBox4;
        internal MrngTextBox mrngTextBox2;
        internal MrngLabel mrngLabel4;
        private MrngButton btnTestConnection;
        private MrngButton btnExpandOptions;
        private System.Windows.Forms.TabControl tabCtrlSQL;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel pnlSQLCon;
        private MrngComboBox txtSQLAuthType;
        internal MrngLabel lblSQLAuthType;
        internal MrngLabel lblSQLType;
        private MrngComboBox txtSQLType;
        internal MrngLabel lblSQLServer;
        internal MrngLabel lblSQLUsername;
        internal MrngLabel lblSQLPassword;
        internal MrngTextBox txtSQLServer;
        internal MrngTextBox txtSQLPassword;
        internal MrngTextBox txtSQLUsername;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        internal MrngLabel mrngLabel1;
        internal MrngLabel mrngLabel9;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MrngComboBox mrngComboBox3;
        internal MrngLabel mrngLabel11;
        internal MrngLabel mrngLabel15;
        internal MrngTextBox mrngTextBox8;
        internal MrngLabel mrngLabel16;
        internal MrngLabel mrngLabel17;
        internal MrngTextBox mrngTextBox11;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox imgConnectionStatus;
        private MrngLabel lblTestConnectionResults;
        internal MrngLabel mrngLabel13;
        internal MrngLabel mrngLabel14;
        private MrngCheckBox mrngCheckBox4;
        private MrngCheckBox mrngCheckBox3;
        private MrngComboBox mrngComboBox2;
        private MrngCheckBox mrngCheckBox2;
        internal MrngLabel mrngLabel12;
        private MrngCheckBox mrngCheckBox5;
        internal MrngLabel mrngLabel7;
        internal MrngTextBox mrngTextBox5;
        internal MrngLabel mrngLabel3;
        private System.Windows.Forms.CheckBox chkSQLReadOnly;
        private System.Windows.Forms.CheckBox chkUseSQLServer;
        internal MrngLabel mrngLabel19;
        private MrngComboBox mrngComboBox4;
        internal MrngLabel mrngLabel18;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        internal MrngTextBox txtSQLDatabaseName;
        internal MrngLabel lblSQLDatabaseName;
        internal MrngLabel lblSQLReadOnly;
        internal MrngLabel lblShowDatabasePickerOnStartup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton DCMSetupRdBtnC;
        private System.Windows.Forms.RadioButton DCMSetupRdBtnV;
        internal MrngTextBox DCMSetuptxtadmuser;
        private System.Windows.Forms.Label DCMSetuplblschema;
        private System.Windows.Forms.ComboBox DCMSetupddschema;
        private System.Windows.Forms.Label DCMSetuplbladminuser;
        private System.Windows.Forms.ToolTip frmtoolTip;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label DCMSetuplbladminpwd;
        internal MrngTextBox DCMSetuptxtadmpwd;
        internal MrngTextBox DCMSetuptxtdbname;
        private System.Windows.Forms.Label DCMSetuplbldbname;
        internal MrngTextBox DCMSetuptxtuserpwd;
        internal MrngTextBox DCMSetuptxtuser;
        private System.Windows.Forms.Label DCMSetuplbluserpwd;
        private System.Windows.Forms.Label DCMSetuplbluser;
        private System.Windows.Forms.Label DCMSetuptxtmandatory4;
        private System.Windows.Forms.Label DCMSetuptxtmandatory3;
        private System.Windows.Forms.Label DCMSetuptxtmandatory2;
        private System.Windows.Forms.Label DCMSetuptxtmandatory1;
        private System.Windows.Forms.CheckBox chkShowDatabasePickerOnStartup;
        private System.Windows.Forms.TabPage tabPageProfiles;
        private System.Windows.Forms.ListBox lstProfiles;
        private MrngButton btnLoadProfile;
        private MrngButton btnSaveProfile;
        private MrngButton btnDeleteProfile;
        private MrngTextBox txtProfileName;
        private MrngLabel lblProfileName;
    }
}
