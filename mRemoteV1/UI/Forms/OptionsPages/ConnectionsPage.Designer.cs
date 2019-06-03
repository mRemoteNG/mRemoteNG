

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public sealed partial class ConnectionsPage : OptionsPage
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
            this.numRDPConTimeout = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.lblRDPConTimeout = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblRdpReconnectionCount = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.numRdpReconnectionCount = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.chkSingleClickOnConnectionOpensIt = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkHostnameLikeDisplayName = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkSingleClickOnOpenedConnectionSwitchesToIt = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.lblAutoSave1 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.numAutoSave = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.lblClosingConnections = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.chkUseFilterSearch = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.tableLayoutPanelRDPTimeout = new System.Windows.Forms.TableLayoutPanel();
            this.chkPlaceSearchBarAboveConnectionTree = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkConnectionTreeTrackActiveConnection = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkDoNotTrimUsername = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelBackupFile = new System.Windows.Forms.TableLayoutPanel();
            this.lblConnectionsBackupFrequency = new System.Windows.Forms.Label();
            this.cmbConnectionBackupFrequency = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelConnectionsWarning = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxConnectionWarning = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).BeginInit();
            this.tableLayoutPanelRDPTimeout.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelBackupFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBackups)).BeginInit();
            this.tableLayoutPanelConnectionsWarning.SuspendLayout();
            this.SuspendLayout();
            // 
            // numRDPConTimeout
            // 
            this.numRDPConTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRDPConTimeout.Location = new System.Drawing.Point(270, 29);
            this.numRDPConTimeout.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numRDPConTimeout.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numRDPConTimeout.Name = "numRDPConTimeout";
            this.numRDPConTimeout.Size = new System.Drawing.Size(53, 22);
            this.numRDPConTimeout.TabIndex = 1;
            this.numRDPConTimeout.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblRDPConTimeout
            // 
            this.lblRDPConTimeout.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRDPConTimeout.Location = new System.Drawing.Point(3, 26);
            this.lblRDPConTimeout.Name = "lblRDPConTimeout";
            this.lblRDPConTimeout.Size = new System.Drawing.Size(261, 26);
            this.lblRDPConTimeout.TabIndex = 0;
            this.lblRDPConTimeout.Text = "RDP Connection Timeout";
            this.lblRDPConTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRdpReconnectionCount
            // 
            this.lblRdpReconnectionCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRdpReconnectionCount.Location = new System.Drawing.Point(3, 0);
            this.lblRdpReconnectionCount.Name = "lblRdpReconnectionCount";
            this.lblRdpReconnectionCount.Size = new System.Drawing.Size(261, 26);
            this.lblRdpReconnectionCount.TabIndex = 0;
            this.lblRdpReconnectionCount.Text = "RDP Reconnection Count";
            this.lblRdpReconnectionCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numRdpReconnectionCount
            // 
            this.numRdpReconnectionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRdpReconnectionCount.Location = new System.Drawing.Point(270, 3);
            this.numRdpReconnectionCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numRdpReconnectionCount.Name = "numRdpReconnectionCount";
            this.numRdpReconnectionCount.Size = new System.Drawing.Size(53, 22);
            this.numRdpReconnectionCount.TabIndex = 1;
            this.numRdpReconnectionCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkSingleClickOnConnectionOpensIt
            // 
            this.chkSingleClickOnConnectionOpensIt._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkSingleClickOnConnectionOpensIt.AutoSize = true;
            this.chkSingleClickOnConnectionOpensIt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSingleClickOnConnectionOpensIt.Location = new System.Drawing.Point(3, 3);
            this.chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt";
            this.chkSingleClickOnConnectionOpensIt.Size = new System.Drawing.Size(206, 17);
            this.chkSingleClickOnConnectionOpensIt.TabIndex = 0;
            this.chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it";
            this.chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = true;
            // 
            // chkHostnameLikeDisplayName
            // 
            this.chkHostnameLikeDisplayName._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkHostnameLikeDisplayName.AutoSize = true;
            this.chkHostnameLikeDisplayName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHostnameLikeDisplayName.Location = new System.Drawing.Point(3, 72);
            this.chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName";
            this.chkHostnameLikeDisplayName.Size = new System.Drawing.Size(355, 17);
            this.chkHostnameLikeDisplayName.TabIndex = 2;
            this.chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections";
            this.chkHostnameLikeDisplayName.UseVisualStyleBackColor = true;
            // 
            // chkSingleClickOnOpenedConnectionSwitchesToIt
            // 
            this.chkSingleClickOnOpenedConnectionSwitchesToIt._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = true;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Location = new System.Drawing.Point(3, 26);
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt";
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Size = new System.Drawing.Size(490, 17);
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 1;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Text = global::mRemoteNG.Language.strSingleClickOnOpenConnectionSwitchesToIt;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = true;
            // 
            // lblAutoSave1
            // 
            this.lblAutoSave1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAutoSave1.Location = new System.Drawing.Point(3, 52);
            this.lblAutoSave1.Name = "lblAutoSave1";
            this.lblAutoSave1.Size = new System.Drawing.Size(261, 26);
            this.lblAutoSave1.TabIndex = 0;
            this.lblAutoSave1.Text = "Auto Save  in Minutes (0 means disabled)";
            this.lblAutoSave1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numAutoSave
            // 
            this.numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAutoSave.Location = new System.Drawing.Point(270, 55);
            this.numAutoSave.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numAutoSave.Name = "numAutoSave";
            this.numAutoSave.Size = new System.Drawing.Size(53, 22);
            this.numAutoSave.TabIndex = 1;
            // 
            // lblClosingConnections
            // 
            this.lblClosingConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClosingConnections.AutoSize = true;
            this.lblClosingConnections.Location = new System.Drawing.Point(3, 0);
            this.lblClosingConnections.Name = "lblClosingConnections";
            this.lblClosingConnections.Size = new System.Drawing.Size(203, 28);
            this.lblClosingConnections.TabIndex = 0;
            this.lblClosingConnections.Text = "When closing connections, warn me...";
            this.lblClosingConnections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkUseFilterSearch
            // 
            this.chkUseFilterSearch._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseFilterSearch.AutoSize = true;
            this.chkUseFilterSearch.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseFilterSearch.Location = new System.Drawing.Point(3, 95);
            this.chkUseFilterSearch.Name = "chkUseFilterSearch";
            this.chkUseFilterSearch.Size = new System.Drawing.Size(230, 17);
            this.chkUseFilterSearch.TabIndex = 8;
            this.chkUseFilterSearch.Text = "Filter search matches in connection tree";
            this.chkUseFilterSearch.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRDPTimeout
            // 
            this.tableLayoutPanelRDPTimeout.ColumnCount = 2;
            this.tableLayoutPanelRDPTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelRDPTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanelRDPTimeout.Controls.Add(this.numRdpReconnectionCount, 1, 0);
            this.tableLayoutPanelRDPTimeout.Controls.Add(this.numAutoSave, 1, 2);
            this.tableLayoutPanelRDPTimeout.Controls.Add(this.lblRdpReconnectionCount, 0, 0);
            this.tableLayoutPanelRDPTimeout.Controls.Add(this.lblAutoSave1, 0, 2);
            this.tableLayoutPanelRDPTimeout.Controls.Add(this.lblRDPConTimeout, 0, 1);
            this.tableLayoutPanelRDPTimeout.Controls.Add(this.numRDPConTimeout, 1, 1);
            this.tableLayoutPanelRDPTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRDPTimeout.Location = new System.Drawing.Point(3, 164);
            this.tableLayoutPanelRDPTimeout.Name = "tableLayoutPanelRDPTimeout";
            this.tableLayoutPanelRDPTimeout.RowCount = 3;
            this.tableLayoutPanelRDPTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelRDPTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelRDPTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanelRDPTimeout.Size = new System.Drawing.Size(594, 79);
            this.tableLayoutPanelRDPTimeout.TabIndex = 9;
            // 
            // chkPlaceSearchBarAboveConnectionTree
            // 
            this.chkPlaceSearchBarAboveConnectionTree._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkPlaceSearchBarAboveConnectionTree.AutoSize = true;
            this.chkPlaceSearchBarAboveConnectionTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPlaceSearchBarAboveConnectionTree.Location = new System.Drawing.Point(3, 118);
            this.chkPlaceSearchBarAboveConnectionTree.Name = "chkPlaceSearchBarAboveConnectionTree";
            this.chkPlaceSearchBarAboveConnectionTree.Size = new System.Drawing.Size(226, 17);
            this.chkPlaceSearchBarAboveConnectionTree.TabIndex = 8;
            this.chkPlaceSearchBarAboveConnectionTree.Text = "Place search bar above connection tree";
            this.chkPlaceSearchBarAboveConnectionTree.UseVisualStyleBackColor = true;
            // 
            // chkConnectionTreeTrackActiveConnection
            // 
            this.chkConnectionTreeTrackActiveConnection._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkConnectionTreeTrackActiveConnection.AutoSize = true;
            this.chkConnectionTreeTrackActiveConnection.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkConnectionTreeTrackActiveConnection.Location = new System.Drawing.Point(3, 49);
            this.chkConnectionTreeTrackActiveConnection.Name = "chkConnectionTreeTrackActiveConnection";
            this.chkConnectionTreeTrackActiveConnection.Size = new System.Drawing.Size(261, 17);
            this.chkConnectionTreeTrackActiveConnection.TabIndex = 10;
            this.chkConnectionTreeTrackActiveConnection.Text = "Track active connection in the connection tree";
            this.chkConnectionTreeTrackActiveConnection.UseVisualStyleBackColor = true;
            // 
            // chkDoNotTrimUsername
            // 
            this.chkDoNotTrimUsername._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkDoNotTrimUsername.AutoSize = true;
            this.chkDoNotTrimUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDoNotTrimUsername.Location = new System.Drawing.Point(3, 141);
            this.chkDoNotTrimUsername.Name = "chkDoNotTrimUsername";
            this.chkDoNotTrimUsername.Size = new System.Drawing.Size(143, 17);
            this.chkDoNotTrimUsername.TabIndex = 11;
            this.chkDoNotTrimUsername.Text = "Do not trim usernames";
            this.chkDoNotTrimUsername.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelBackupFile, 0, 9);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelConnectionsWarning, 0, 8);
            this.tableLayoutPanelMain.Controls.Add(this.chkSingleClickOnConnectionOpensIt, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelRDPTimeout, 0, 7);
            this.tableLayoutPanelMain.Controls.Add(this.chkDoNotTrimUsername, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.chkSingleClickOnOpenedConnectionSwitchesToIt, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.chkConnectionTreeTrackActiveConnection, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.chkPlaceSearchBarAboveConnectionTree, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.chkHostnameLikeDisplayName, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.chkUseFilterSearch, 0, 4);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 10;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(630, 412);
            this.tableLayoutPanelMain.TabIndex = 12;
            // 
            // tableLayoutPanelBackupFile
            // 
            this.tableLayoutPanelBackupFile.ColumnCount = 4;
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblConnectionsBackupFrequency, 0, 0);
            this.tableLayoutPanelBackupFile.Controls.Add(this.cmbConnectionBackupFrequency, 1, 0);
            this.tableLayoutPanelBackupFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBackupFile.Location = new System.Drawing.Point(3, 283);
            this.tableLayoutPanelBackupFile.Name = "tableLayoutPanelBackupFile";
            this.tableLayoutPanelBackupFile.RowCount = 4;
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.Size = new System.Drawing.Size(594, 179);
            this.tableLayoutPanelBackupFile.TabIndex = 15;
            // 
            // lblConnectionsBackupFrequency
            // 
            this.lblConnectionsBackupFrequency.AutoSize = true;
            this.lblConnectionsBackupFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionsBackupFrequency.Location = new System.Drawing.Point(3, 0);
            this.lblConnectionsBackupFrequency.Name = "lblConnectionsBackupFrequency";
            this.lblConnectionsBackupFrequency.Size = new System.Drawing.Size(186, 27);
            this.lblConnectionsBackupFrequency.TabIndex = 14;
            this.lblConnectionsBackupFrequency.Text = "Connection Backup Frequency";
            this.lblConnectionsBackupFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbConnectionBackupFrequency
            // 
            this.cmbConnectionBackupFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectionBackupFrequency.FormattingEnabled = true;
            this.cmbConnectionBackupFrequency.Location = new System.Drawing.Point(195, 3);
            this.cmbConnectionBackupFrequency.Name = "cmbConnectionBackupFrequency";
            this.cmbConnectionBackupFrequency.Size = new System.Drawing.Size(160, 21);
            this.cmbConnectionBackupFrequency.TabIndex = 13;
            // 
            // tableLayoutPanelConnectionsWarning
            // 
            this.tableLayoutPanelConnectionsWarning.ColumnCount = 3;
            this.tableLayoutPanelConnectionsWarning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelConnectionsWarning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelConnectionsWarning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelConnectionsWarning.Controls.Add(this.lblClosingConnections, 0, 0);
            this.tableLayoutPanelConnectionsWarning.Controls.Add(this.comboBoxConnectionWarning, 1, 0);
            this.tableLayoutPanelConnectionsWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelConnectionsWarning.Location = new System.Drawing.Point(3, 249);
            this.tableLayoutPanelConnectionsWarning.Name = "tableLayoutPanelConnectionsWarning";
            this.tableLayoutPanelConnectionsWarning.RowCount = 1;
            this.tableLayoutPanelConnectionsWarning.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelConnectionsWarning.Size = new System.Drawing.Size(594, 28);
            this.tableLayoutPanelConnectionsWarning.TabIndex = 13;
            // 
            // comboBoxConnectionWarning
            // 
            this.comboBoxConnectionWarning.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnectionWarning.FormattingEnabled = true;
            this.comboBoxConnectionWarning.Location = new System.Drawing.Point(212, 3);
            this.comboBoxConnectionWarning.Name = "comboBoxConnectionWarning";
            this.comboBoxConnectionWarning.Size = new System.Drawing.Size(199, 21);
            this.comboBoxConnectionWarning.TabIndex = 1;
            // 
            // ConnectionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "ConnectionsPage";
            this.Size = new System.Drawing.Size(630, 412);
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).EndInit();
            this.tableLayoutPanelRDPTimeout.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tableLayoutPanelBackupFile.ResumeLayout(false);
            this.tableLayoutPanelBackupFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBackups)).EndInit();
            this.tableLayoutPanelConnectionsWarning.ResumeLayout(false);
            this.tableLayoutPanelConnectionsWarning.PerformLayout();
            this.ResumeLayout(false);

		}
		internal Controls.Base.NGLabel lblRdpReconnectionCount;
		internal Controls.Base.NGCheckBox chkSingleClickOnConnectionOpensIt;
		internal Controls.Base.NGCheckBox chkHostnameLikeDisplayName;
		internal Controls.Base.NGCheckBox chkSingleClickOnOpenedConnectionSwitchesToIt;
		internal Controls.Base.NGLabel lblAutoSave1;
		internal Controls.Base.NGNumericUpDown numAutoSave;
		internal Controls.Base.NGLabel lblClosingConnections;
        internal Controls.Base.NGNumericUpDown numRDPConTimeout;
        internal Controls.Base.NGLabel lblRDPConTimeout;
        internal Controls.Base.NGNumericUpDown numRdpReconnectionCount;
        private Controls.Base.NGCheckBox chkUseFilterSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRDPTimeout;
        private Controls.Base.NGCheckBox chkPlaceSearchBarAboveConnectionTree;
        private Controls.Base.NGCheckBox chkConnectionTreeTrackActiveConnection;
        private Controls.Base.NGCheckBox chkDoNotTrimUsername;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ComboBox cmbConnectionBackupFrequency;
        private System.Windows.Forms.Label lblConnectionsBackupFrequency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBackupFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelConnectionsWarning;
        private System.Windows.Forms.ComboBox comboBoxConnectionWarning;
        private System.Windows.Forms.NumericUpDown numMaxBackups;
    }
}
