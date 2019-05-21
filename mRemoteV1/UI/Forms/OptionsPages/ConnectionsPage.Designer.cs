

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
            this.pnlConfirmCloseConnection = new System.Windows.Forms.Panel();
            this.lblClosingConnections = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.radCloseWarnAll = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radCloseWarnMultiple = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radCloseWarnExit = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.radCloseWarnNever = new mRemoteNG.UI.Controls.Base.NGRadioButton();
            this.chkSaveConnectionsAfterEveryEdit = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkUseFilterSearch = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.tableLayoutPanelRDPTimeout = new System.Windows.Forms.TableLayoutPanel();
            this.chkPlaceSearchBarAboveConnectionTree = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkConnectionTreeTrackActiveConnection = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkDoNotTrimUsername = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).BeginInit();
            this.pnlConfirmCloseConnection.SuspendLayout();
            this.tableLayoutPanelRDPTimeout.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
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
            // pnlConfirmCloseConnection
            // 
            this.pnlConfirmCloseConnection.Controls.Add(this.lblClosingConnections);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnAll);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnMultiple);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnExit);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnNever);
            this.pnlConfirmCloseConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfirmCloseConnection.Location = new System.Drawing.Point(3, 249);
            this.pnlConfirmCloseConnection.Name = "pnlConfirmCloseConnection";
            this.pnlConfirmCloseConnection.Size = new System.Drawing.Size(594, 137);
            this.pnlConfirmCloseConnection.TabIndex = 6;
            // 
            // lblClosingConnections
            // 
            this.lblClosingConnections.AutoSize = true;
            this.lblClosingConnections.Location = new System.Drawing.Point(3, 12);
            this.lblClosingConnections.Name = "lblClosingConnections";
            this.lblClosingConnections.Size = new System.Drawing.Size(147, 13);
            this.lblClosingConnections.TabIndex = 0;
            this.lblClosingConnections.Text = "When closing connections:";
            // 
            // radCloseWarnAll
            // 
            this.radCloseWarnAll.AutoSize = true;
            this.radCloseWarnAll.BackColor = System.Drawing.Color.Transparent;
            this.radCloseWarnAll.Location = new System.Drawing.Point(16, 34);
            this.radCloseWarnAll.Name = "radCloseWarnAll";
            this.radCloseWarnAll.Size = new System.Drawing.Size(209, 17);
            this.radCloseWarnAll.TabIndex = 1;
            this.radCloseWarnAll.TabStop = true;
            this.radCloseWarnAll.Text = "Warn me when closing connections";
            this.radCloseWarnAll.UseVisualStyleBackColor = false;
            // 
            // radCloseWarnMultiple
            // 
            this.radCloseWarnMultiple.AutoSize = true;
            this.radCloseWarnMultiple.BackColor = System.Drawing.Color.Transparent;
            this.radCloseWarnMultiple.Location = new System.Drawing.Point(16, 57);
            this.radCloseWarnMultiple.Name = "radCloseWarnMultiple";
            this.radCloseWarnMultiple.Size = new System.Drawing.Size(279, 17);
            this.radCloseWarnMultiple.TabIndex = 2;
            this.radCloseWarnMultiple.TabStop = true;
            this.radCloseWarnMultiple.Text = "Warn me only when closing multiple connections";
            this.radCloseWarnMultiple.UseVisualStyleBackColor = false;
            // 
            // radCloseWarnExit
            // 
            this.radCloseWarnExit.AutoSize = true;
            this.radCloseWarnExit.BackColor = System.Drawing.Color.Transparent;
            this.radCloseWarnExit.Location = new System.Drawing.Point(16, 80);
            this.radCloseWarnExit.Name = "radCloseWarnExit";
            this.radCloseWarnExit.Size = new System.Drawing.Size(233, 17);
            this.radCloseWarnExit.TabIndex = 3;
            this.radCloseWarnExit.TabStop = true;
            this.radCloseWarnExit.Text = "Warn me only when exiting mRemoteNG";
            this.radCloseWarnExit.UseVisualStyleBackColor = false;
            // 
            // radCloseWarnNever
            // 
            this.radCloseWarnNever.AutoSize = true;
            this.radCloseWarnNever.BackColor = System.Drawing.Color.Transparent;
            this.radCloseWarnNever.Location = new System.Drawing.Point(16, 103);
            this.radCloseWarnNever.Name = "radCloseWarnNever";
            this.radCloseWarnNever.Size = new System.Drawing.Size(246, 17);
            this.radCloseWarnNever.TabIndex = 4;
            this.radCloseWarnNever.TabStop = true;
            this.radCloseWarnNever.Text = "Do not warn me when closing connections";
            this.radCloseWarnNever.UseVisualStyleBackColor = false;
            // 
            // chkSaveConnectionsAfterEveryEdit
            // 
            this.chkSaveConnectionsAfterEveryEdit._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkSaveConnectionsAfterEveryEdit.AutoSize = true;
            this.chkSaveConnectionsAfterEveryEdit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSaveConnectionsAfterEveryEdit.Location = new System.Drawing.Point(3, 95);
            this.chkSaveConnectionsAfterEveryEdit.Name = "chkSaveConnectionsAfterEveryEdit";
            this.chkSaveConnectionsAfterEveryEdit.Size = new System.Drawing.Size(194, 17);
            this.chkSaveConnectionsAfterEveryEdit.TabIndex = 7;
            this.chkSaveConnectionsAfterEveryEdit.Text = "Save connections after every edit";
            this.chkSaveConnectionsAfterEveryEdit.UseVisualStyleBackColor = true;
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
            this.tableLayoutPanelMain.Controls.Add(this.chkSingleClickOnConnectionOpensIt, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pnlConfirmCloseConnection, 0, 8);
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
            this.tableLayoutPanelMain.RowCount = 11;
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
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(621, 591);
            this.tableLayoutPanelMain.TabIndex = 12;
            // 
            // ConnectionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "ConnectionsPage";
            this.Size = new System.Drawing.Size(621, 591);
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).EndInit();
            this.pnlConfirmCloseConnection.ResumeLayout(false);
            this.pnlConfirmCloseConnection.PerformLayout();
            this.tableLayoutPanelRDPTimeout.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

		}
		internal Controls.Base.NGLabel lblRdpReconnectionCount;
		internal Controls.Base.NGCheckBox chkSingleClickOnConnectionOpensIt;
		internal Controls.Base.NGCheckBox chkHostnameLikeDisplayName;
		internal Controls.Base.NGCheckBox chkSingleClickOnOpenedConnectionSwitchesToIt;
		internal Controls.Base.NGLabel lblAutoSave1;
		internal Controls.Base.NGNumericUpDown numAutoSave;
		internal System.Windows.Forms.Panel pnlConfirmCloseConnection;
		internal Controls.Base.NGLabel lblClosingConnections;
		internal Controls.Base.NGRadioButton radCloseWarnAll;
		internal Controls.Base.NGRadioButton radCloseWarnMultiple;
		internal Controls.Base.NGRadioButton radCloseWarnExit;
		internal Controls.Base.NGRadioButton radCloseWarnNever;
        internal Controls.Base.NGNumericUpDown numRDPConTimeout;
        internal Controls.Base.NGLabel lblRDPConTimeout;
        internal Controls.Base.NGNumericUpDown numRdpReconnectionCount;
        internal Controls.Base.NGCheckBox chkSaveConnectionsAfterEveryEdit;
        private Controls.Base.NGCheckBox chkUseFilterSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRDPTimeout;
        private Controls.Base.NGCheckBox chkPlaceSearchBarAboveConnectionTree;
        private Controls.Base.NGCheckBox chkConnectionTreeTrackActiveConnection;
        private Controls.Base.NGCheckBox chkDoNotTrimUsername;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
    }
}
