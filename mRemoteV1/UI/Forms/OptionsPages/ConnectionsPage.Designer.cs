

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class ConnectionsPage : OptionsPage
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
            this.pnlRdpReconnectionCount = new System.Windows.Forms.Panel();
            this.numRDPConTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblRDPConTimeout = new System.Windows.Forms.Label();
            this.lblRdpReconnectionCount = new System.Windows.Forms.Label();
            this.numRdpReconnectionCount = new System.Windows.Forms.NumericUpDown();
            this.chkSingleClickOnConnectionOpensIt = new System.Windows.Forms.CheckBox();
            this.chkHostnameLikeDisplayName = new System.Windows.Forms.CheckBox();
            this.chkSingleClickOnOpenedConnectionSwitchesToIt = new System.Windows.Forms.CheckBox();
            this.pnlAutoSave = new System.Windows.Forms.Panel();
            this.lblAutoSave1 = new System.Windows.Forms.Label();
            this.numAutoSave = new System.Windows.Forms.NumericUpDown();
            this.lblAutoSave2 = new System.Windows.Forms.Label();
            this.pnlConfirmCloseConnection = new System.Windows.Forms.Panel();
            this.lblClosingConnections = new System.Windows.Forms.Label();
            this.radCloseWarnAll = new System.Windows.Forms.RadioButton();
            this.radCloseWarnMultiple = new System.Windows.Forms.RadioButton();
            this.radCloseWarnExit = new System.Windows.Forms.RadioButton();
            this.radCloseWarnNever = new System.Windows.Forms.RadioButton();
            this.pnlRdpReconnectionCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).BeginInit();
            this.pnlAutoSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).BeginInit();
            this.pnlConfirmCloseConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRdpReconnectionCount
            // 
            this.pnlRdpReconnectionCount.Controls.Add(this.numRDPConTimeout);
            this.pnlRdpReconnectionCount.Controls.Add(this.lblRDPConTimeout);
            this.pnlRdpReconnectionCount.Controls.Add(this.lblRdpReconnectionCount);
            this.pnlRdpReconnectionCount.Controls.Add(this.numRdpReconnectionCount);
            this.pnlRdpReconnectionCount.Location = new System.Drawing.Point(3, 69);
            this.pnlRdpReconnectionCount.Name = "pnlRdpReconnectionCount";
            this.pnlRdpReconnectionCount.Size = new System.Drawing.Size(596, 29);
            this.pnlRdpReconnectionCount.TabIndex = 10;
            // 
            // numRDPConTimeout
            // 
            this.numRDPConTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRDPConTimeout.Location = new System.Drawing.Point(369, 7);
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
            this.numRDPConTimeout.TabIndex = 3;
            this.numRDPConTimeout.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblRDPConTimeout
            // 
            this.lblRDPConTimeout.Location = new System.Drawing.Point(226, 9);
            this.lblRDPConTimeout.Name = "lblRDPConTimeout";
            this.lblRDPConTimeout.Size = new System.Drawing.Size(137, 13);
            this.lblRDPConTimeout.TabIndex = 2;
            this.lblRDPConTimeout.Text = "RDP Connection Timeout";
            this.lblRDPConTimeout.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRdpReconnectionCount
            // 
            this.lblRdpReconnectionCount.Location = new System.Drawing.Point(3, 9);
            this.lblRdpReconnectionCount.Name = "lblRdpReconnectionCount";
            this.lblRdpReconnectionCount.Size = new System.Drawing.Size(139, 13);
            this.lblRdpReconnectionCount.TabIndex = 0;
            this.lblRdpReconnectionCount.Text = "RDP Reconnection Count";
            this.lblRdpReconnectionCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numRdpReconnectionCount
            // 
            this.numRdpReconnectionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRdpReconnectionCount.Location = new System.Drawing.Point(145, 6);
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
            this.chkSingleClickOnConnectionOpensIt.AutoSize = true;
            this.chkSingleClickOnConnectionOpensIt.Location = new System.Drawing.Point(3, 0);
            this.chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt";
            this.chkSingleClickOnConnectionOpensIt.Size = new System.Drawing.Size(191, 17);
            this.chkSingleClickOnConnectionOpensIt.TabIndex = 7;
            this.chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it";
            this.chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = true;
            // 
            // chkHostnameLikeDisplayName
            // 
            this.chkHostnameLikeDisplayName.AutoSize = true;
            this.chkHostnameLikeDisplayName.Location = new System.Drawing.Point(3, 46);
            this.chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName";
            this.chkHostnameLikeDisplayName.Size = new System.Drawing.Size(328, 17);
            this.chkHostnameLikeDisplayName.TabIndex = 9;
            this.chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections";
            this.chkHostnameLikeDisplayName.UseVisualStyleBackColor = true;
            // 
            // chkSingleClickOnOpenedConnectionSwitchesToIt
            // 
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = true;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Location = new System.Drawing.Point(3, 23);
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt";
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Size = new System.Drawing.Size(457, 17);
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 8;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.Text = global::mRemoteNG.Language.strSingleClickOnOpenConnectionSwitchesToIt;
            this.chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = true;
            // 
            // pnlAutoSave
            // 
            this.pnlAutoSave.Controls.Add(this.lblAutoSave1);
            this.pnlAutoSave.Controls.Add(this.numAutoSave);
            this.pnlAutoSave.Controls.Add(this.lblAutoSave2);
            this.pnlAutoSave.Location = new System.Drawing.Point(3, 104);
            this.pnlAutoSave.Name = "pnlAutoSave";
            this.pnlAutoSave.Size = new System.Drawing.Size(596, 29);
            this.pnlAutoSave.TabIndex = 11;
            // 
            // lblAutoSave1
            // 
            this.lblAutoSave1.Location = new System.Drawing.Point(6, 9);
            this.lblAutoSave1.Name = "lblAutoSave1";
            this.lblAutoSave1.Size = new System.Drawing.Size(288, 13);
            this.lblAutoSave1.TabIndex = 0;
            this.lblAutoSave1.Text = "Auto Save every:";
            this.lblAutoSave1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numAutoSave
            // 
            this.numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAutoSave.Location = new System.Drawing.Point(300, 7);
            this.numAutoSave.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numAutoSave.Name = "numAutoSave";
            this.numAutoSave.Size = new System.Drawing.Size(53, 20);
            this.numAutoSave.TabIndex = 1;
            // 
            // lblAutoSave2
            // 
            this.lblAutoSave2.AutoSize = true;
            this.lblAutoSave2.Location = new System.Drawing.Point(359, 9);
            this.lblAutoSave2.Name = "lblAutoSave2";
            this.lblAutoSave2.Size = new System.Drawing.Size(135, 13);
            this.lblAutoSave2.TabIndex = 2;
            this.lblAutoSave2.Text = "Minutes (0 means disabled)";
            // 
            // pnlConfirmCloseConnection
            // 
            this.pnlConfirmCloseConnection.Controls.Add(this.lblClosingConnections);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnAll);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnMultiple);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnExit);
            this.pnlConfirmCloseConnection.Controls.Add(this.radCloseWarnNever);
            this.pnlConfirmCloseConnection.Location = new System.Drawing.Point(3, 139);
            this.pnlConfirmCloseConnection.Name = "pnlConfirmCloseConnection";
            this.pnlConfirmCloseConnection.Size = new System.Drawing.Size(596, 137);
            this.pnlConfirmCloseConnection.TabIndex = 13;
            // 
            // lblClosingConnections
            // 
            this.lblClosingConnections.AutoSize = true;
            this.lblClosingConnections.Location = new System.Drawing.Point(3, 9);
            this.lblClosingConnections.Name = "lblClosingConnections";
            this.lblClosingConnections.Size = new System.Drawing.Size(136, 13);
            this.lblClosingConnections.TabIndex = 0;
            this.lblClosingConnections.Text = "When closing connections:";
            // 
            // radCloseWarnAll
            // 
            this.radCloseWarnAll.AutoSize = true;
            this.radCloseWarnAll.Location = new System.Drawing.Point(16, 31);
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
            this.radCloseWarnMultiple.Location = new System.Drawing.Point(16, 54);
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
            this.radCloseWarnExit.Location = new System.Drawing.Point(16, 77);
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
            this.radCloseWarnNever.Location = new System.Drawing.Point(16, 100);
            this.radCloseWarnNever.Name = "radCloseWarnNever";
            this.radCloseWarnNever.Size = new System.Drawing.Size(226, 17);
            this.radCloseWarnNever.TabIndex = 4;
            this.radCloseWarnNever.TabStop = true;
            this.radCloseWarnNever.Text = "Do not warn me when closing connections";
            this.radCloseWarnNever.UseVisualStyleBackColor = true;
            // 
            // ConnectionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlRdpReconnectionCount);
            this.Controls.Add(this.chkSingleClickOnConnectionOpensIt);
            this.Controls.Add(this.chkHostnameLikeDisplayName);
            this.Controls.Add(this.chkSingleClickOnOpenedConnectionSwitchesToIt);
            this.Controls.Add(this.pnlAutoSave);
            this.Controls.Add(this.pnlConfirmCloseConnection);
            this.Name = "ConnectionsPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.pnlRdpReconnectionCount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRDPConTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRdpReconnectionCount)).EndInit();
            this.pnlAutoSave.ResumeLayout(false);
            this.pnlAutoSave.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSave)).EndInit();
            this.pnlConfirmCloseConnection.ResumeLayout(false);
            this.pnlConfirmCloseConnection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.Panel pnlRdpReconnectionCount;
		internal System.Windows.Forms.Label lblRdpReconnectionCount;
		internal System.Windows.Forms.CheckBox chkSingleClickOnConnectionOpensIt;
		internal System.Windows.Forms.CheckBox chkHostnameLikeDisplayName;
		internal System.Windows.Forms.CheckBox chkSingleClickOnOpenedConnectionSwitchesToIt;
		internal System.Windows.Forms.Panel pnlAutoSave;
		internal System.Windows.Forms.Label lblAutoSave1;
		internal System.Windows.Forms.NumericUpDown numAutoSave;
		internal System.Windows.Forms.Label lblAutoSave2;
		internal System.Windows.Forms.Panel pnlConfirmCloseConnection;
		internal System.Windows.Forms.Label lblClosingConnections;
		internal System.Windows.Forms.RadioButton radCloseWarnAll;
		internal System.Windows.Forms.RadioButton radCloseWarnMultiple;
		internal System.Windows.Forms.RadioButton radCloseWarnExit;
		internal System.Windows.Forms.RadioButton radCloseWarnNever;
        internal System.Windows.Forms.NumericUpDown numRDPConTimeout;
        internal System.Windows.Forms.Label lblRDPConTimeout;
        internal System.Windows.Forms.NumericUpDown numRdpReconnectionCount;
    }
}
