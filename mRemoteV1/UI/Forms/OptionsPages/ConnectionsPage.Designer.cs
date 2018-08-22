

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionsPage));
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
            this.pnlRdpReconnectionCount = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRdpConnectionTimeout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkSaveConnectionsAfterEveryEdit = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkUseFilterSearch = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).BeginInit();
            this.pnlConfirmCloseConnection.SuspendLayout();
            this.pnlRdpReconnectionCount.SuspendLayout();
            this.pnlRdpConnectionTimeout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numRDPConTimeout
            // 
            this.numRDPConTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRDPConTimeout.Location = new System.Drawing.Point(270, 3);
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
            this.numRDPConTimeout.Size = new System.Drawing.Size(53, 20);
            this.numRDPConTimeout.TabIndex = 1;
            this.numRDPConTimeout.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblRDPConTimeout
            // 
            this.lblRDPConTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRDPConTimeout.Location = new System.Drawing.Point(3, 0);
            this.lblRDPConTimeout.Name = "lblRDPConTimeout";
            this.lblRDPConTimeout.Size = new System.Drawing.Size(261, 26);
            this.lblRDPConTimeout.TabIndex = 0;
            this.lblRDPConTimeout.Text = "RDP Connection Timeout";
            this.lblRDPConTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRdpReconnectionCount
            // 
            this.lblRdpReconnectionCount.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.numRdpReconnectionCount.Size = new System.Drawing.Size(53, 20);
            this.numRdpReconnectionCount.TabIndex = 1;
            this.numRdpReconnectionCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkSingleClickOnConnectionOpensIt
            // 
            this.chkSingleClickOnConnectionOpensIt._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkSingleClickOnConnectionOpensIt.AutoSize = true;
            this.chkSingleClickOnConnectionOpensIt.Location = new System.Drawing.Point(3, 3);
            this.chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt";
            this.chkSingleClickOnConnectionOpensIt.Size = new System.Drawing.Size(191, 17);
            this.chkSingleClickOnConnectionOpensIt.TabIndex = 0;
            this.chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it";
            this.chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = true;
            // 
            // chkHostnameLikeDisplayName
            // 
            this.chkHostnameLikeDisplayName._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkHostnameLikeDisplayName.AutoSize = true;
            this.chkHostnameLikeDisplayName.Location = new System.Drawing.Point(3, 49);
            this.chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName";
            this.chkHostnameLikeDisplayName.Size = new System.Drawing.Size(328, 17);
            this.chkHostnameLikeDisplayName.TabIndex = 2;
            this.chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections";
            this.chkHostnameLikeDisplayName.UseVisualStyleBackColor = true;
            // 
            // chkSingleClickOnOpenedConnectionSwitchesToIt
            // 
            this.chkSingleClickOnOpenedConnectionSwitchesToIt._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = true;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Location = new System.Drawing.Point(3, 26);
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt";
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Size = new System.Drawing.Size(457, 17);
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 1;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Text = global::mRemoteNG.Language.strSingleClickOnOpenConnectionSwitchesToIt;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = true;
            // 
            // lblAutoSave1
            // 
            this.lblAutoSave1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutoSave1.Location = new System.Drawing.Point(3, 0);
            this.lblAutoSave1.Name = "lblAutoSave1";
            this.lblAutoSave1.Size = new System.Drawing.Size(261, 26);
            this.lblAutoSave1.TabIndex = 0;
            this.lblAutoSave1.Text = "Auto Save  in Minutes (0 means disabled)";
            this.lblAutoSave1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numAutoSave
            // 
            this.numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAutoSave.Location = new System.Drawing.Point(270, 3);
            this.numAutoSave.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numAutoSave.Name = "numAutoSave";
            this.numAutoSave.Size = new System.Drawing.Size(53, 20);
            this.numAutoSave.TabIndex = 1;
            // 
            // pnlConfirmCloseConnection
            // 
            this.pnlConfirmCloseConnection.Controls.Add(this.lblClosingConnections);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnAll);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnMultiple);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnExit);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnNever);
            this.pnlConfirmCloseConnection.Location = new System.Drawing.Point(3, 214);
            this.pnlConfirmCloseConnection.Name = "pnlConfirmCloseConnection";
            this.pnlConfirmCloseConnection.Size = new System.Drawing.Size(595, 137);
            this.pnlConfirmCloseConnection.TabIndex = 6;
            // 
            // lblClosingConnections
            // 
            this.lblClosingConnections.AutoSize = true;
            this.lblClosingConnections.Location = new System.Drawing.Point(3, 12);
            this.lblClosingConnections.Name = "lblClosingConnections";
            this.lblClosingConnections.Size = new System.Drawing.Size(136, 13);
            this.lblClosingConnections.TabIndex = 0;
            this.lblClosingConnections.Text = "When closing connections:";
            // 
            // radCloseWarnAll
            // 
            this.radCloseWarnAll.AutoSize = true;
            this.radCloseWarnAll.Location = new System.Drawing.Point(16, 34);
            this.radCloseWarnAll.Name = "radCloseWarnAll";
            this.radCloseWarnAll.Size = new System.Drawing.Size(194, 17);
            this.radCloseWarnAll.TabIndex = 1;
            this.radCloseWarnAll.TabStop = true;
            this.radCloseWarnAll.Text = "Warn me when closing connections";
            this.radCloseWarnAll.UseVisualStyleBackColor = true;
            // 
            // radCloseWarnMultiple
            // 
            this.radCloseWarnMultiple.AutoSize = true;
            this.radCloseWarnMultiple.Location = new System.Drawing.Point(16, 57);
            this.radCloseWarnMultiple.Name = "radCloseWarnMultiple";
            this.radCloseWarnMultiple.Size = new System.Drawing.Size(254, 17);
            this.radCloseWarnMultiple.TabIndex = 2;
            this.radCloseWarnMultiple.TabStop = true;
            this.radCloseWarnMultiple.Text = "Warn me only when closing multiple connections";
            this.radCloseWarnMultiple.UseVisualStyleBackColor = true;
            // 
            // radCloseWarnExit
            // 
            this.radCloseWarnExit.AutoSize = true;
            this.radCloseWarnExit.Location = new System.Drawing.Point(16, 80);
            this.radCloseWarnExit.Name = "radCloseWarnExit";
            this.radCloseWarnExit.Size = new System.Drawing.Size(216, 17);
            this.radCloseWarnExit.TabIndex = 3;
            this.radCloseWarnExit.TabStop = true;
            this.radCloseWarnExit.Text = "Warn me only when exiting mRemoteNG";
            this.radCloseWarnExit.UseVisualStyleBackColor = true;
            // 
            // radCloseWarnNever
            // 
            this.radCloseWarnNever.AutoSize = true;
            this.radCloseWarnNever.Location = new System.Drawing.Point(16, 103);
            this.radCloseWarnNever.Name = "radCloseWarnNever";
            this.radCloseWarnNever.Size = new System.Drawing.Size(226, 17);
            this.radCloseWarnNever.TabIndex = 4;
            this.radCloseWarnNever.TabStop = true;
            this.radCloseWarnNever.Text = "Do not warn me when closing connections";
            this.radCloseWarnNever.UseVisualStyleBackColor = true;
            // 
            // pnlRdpReconnectionCount
            // 
            this.pnlRdpReconnectionCount.ColumnCount = 2;
            this.pnlRdpReconnectionCount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.pnlRdpReconnectionCount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.pnlRdpReconnectionCount.Controls.Add(this.lblRdpReconnectionCount, 0, 0);
            this.pnlRdpReconnectionCount.Controls.Add(this.numRdpReconnectionCount, 1, 0);
            this.pnlRdpReconnectionCount.Location = new System.Drawing.Point(4, 118);
            this.pnlRdpReconnectionCount.Name = "pnlRdpReconnectionCount";
            this.pnlRdpReconnectionCount.RowCount = 1;
            this.pnlRdpReconnectionCount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlRdpReconnectionCount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlRdpReconnectionCount.Size = new System.Drawing.Size(595, 26);
            this.pnlRdpReconnectionCount.TabIndex = 3;
            // 
            // pnlRdpConnectionTimeout
            // 
            this.pnlRdpConnectionTimeout.ColumnCount = 2;
            this.pnlRdpConnectionTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.pnlRdpConnectionTimeout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.pnlRdpConnectionTimeout.Controls.Add(this.numRDPConTimeout, 1, 0);
            this.pnlRdpConnectionTimeout.Controls.Add(this.lblRDPConTimeout, 0, 0);
            this.pnlRdpConnectionTimeout.Location = new System.Drawing.Point(4, 150);
            this.pnlRdpConnectionTimeout.Name = "pnlRdpConnectionTimeout";
            this.pnlRdpConnectionTimeout.RowCount = 1;
            this.pnlRdpConnectionTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlRdpConnectionTimeout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlRdpConnectionTimeout.Size = new System.Drawing.Size(595, 26);
            this.pnlRdpConnectionTimeout.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.Controls.Add(this.numAutoSave, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblAutoSave1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 182);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(595, 26);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // chkSaveConnectionsAfterEveryEdit
            // 
            this.chkSaveConnectionsAfterEveryEdit._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkSaveConnectionsAfterEveryEdit.AutoSize = true;
            this.chkSaveConnectionsAfterEveryEdit.Location = new System.Drawing.Point(3, 72);
            this.chkSaveConnectionsAfterEveryEdit.Name = "chkSaveConnectionsAfterEveryEdit";
            this.chkSaveConnectionsAfterEveryEdit.Size = new System.Drawing.Size(185, 17);
            this.chkSaveConnectionsAfterEveryEdit.TabIndex = 7;
            this.chkSaveConnectionsAfterEveryEdit.Text = "Save connections after every edit";
            this.chkSaveConnectionsAfterEveryEdit.UseVisualStyleBackColor = true;
            // 
            // chkUseFilterSearch
            // 
            this.chkUseFilterSearch._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkUseFilterSearch.AutoSize = true;
            this.chkUseFilterSearch.Location = new System.Drawing.Point(4, 95);
            this.chkUseFilterSearch.Name = "chkUseFilterSearch";
            this.chkUseFilterSearch.Size = new System.Drawing.Size(214, 17);
            this.chkUseFilterSearch.TabIndex = 8;
            this.chkUseFilterSearch.Text = "Filter search matches in connection tree";
            this.chkUseFilterSearch.UseVisualStyleBackColor = true;
            // 
            // ConnectionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkUseFilterSearch);
            this.Controls.Add(this.chkSaveConnectionsAfterEveryEdit);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pnlRdpConnectionTimeout);
            this.Controls.Add(this.pnlRdpReconnectionCount);
            this.Controls.Add(this.chkSingleClickOnConnectionOpensIt);
            this.Controls.Add(this.chkHostnameLikeDisplayName);
            this.Controls.Add(this.chkSingleClickOnOpenedConnectionSwitchesToIt);
            this.Controls.Add(this.pnlConfirmCloseConnection);
            this.Name = "ConnectionsPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).EndInit();
            this.pnlConfirmCloseConnection.ResumeLayout(false);
            this.pnlConfirmCloseConnection.PerformLayout();
            this.pnlRdpReconnectionCount.ResumeLayout(false);
            this.pnlRdpConnectionTimeout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TableLayoutPanel pnlRdpReconnectionCount;
        private System.Windows.Forms.TableLayoutPanel pnlRdpConnectionTimeout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal Controls.Base.NGCheckBox chkSaveConnectionsAfterEveryEdit;
        private Controls.Base.NGCheckBox chkUseFilterSearch;
    }
}
