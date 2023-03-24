using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class ConnectionsPage : OptionsPage
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
            numRDPConTimeout = new MrngNumericUpDown();
            lblRDPConTimeout = new MrngLabel();
            lblRdpReconnectionCount = new MrngLabel();
            numRdpReconnectionCount = new MrngNumericUpDown();
            chkSingleClickOnConnectionOpensIt = new MrngCheckBox();
            chkHostnameLikeDisplayName = new MrngCheckBox();
            chkSingleClickOnOpenedConnectionSwitchesToIt = new MrngCheckBox();
            lblAutoSave1 = new MrngLabel();
            numAutoSave = new MrngNumericUpDown();
            pnlConfirmCloseConnection = new System.Windows.Forms.Panel();
            lblClosingConnections = new MrngLabel();
            radCloseWarnAll = new MrngRadioButton();
            radCloseWarnMultiple = new MrngRadioButton();
            radCloseWarnExit = new MrngRadioButton();
            radCloseWarnNever = new MrngRadioButton();
            chkSaveConnectionsAfterEveryEdit = new MrngCheckBox();
            chkUseFilterSearch = new MrngCheckBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            chkPlaceSearchBarAboveConnectionTree = new MrngCheckBox();
            chkConnectionTreeTrackActiveConnection = new MrngCheckBox();
            chkDoNotTrimUsername = new MrngCheckBox();
            ((System.ComponentModel.ISupportInitialize)numRDPConTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRdpReconnectionCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numAutoSave).BeginInit();
            pnlConfirmCloseConnection.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // numRDPConTimeout
            // 
            numRDPConTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRDPConTimeout.Location = new System.Drawing.Point(274, 29);
            numRDPConTimeout.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            numRDPConTimeout.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            numRDPConTimeout.Name = "numRDPConTimeout";
            numRDPConTimeout.Size = new System.Drawing.Size(53, 22);
            numRDPConTimeout.TabIndex = 1;
            numRDPConTimeout.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // lblRDPConTimeout
            // 
            lblRDPConTimeout.Dock = System.Windows.Forms.DockStyle.Top;
            lblRDPConTimeout.Location = new System.Drawing.Point(3, 26);
            lblRDPConTimeout.Name = "lblRDPConTimeout";
            lblRDPConTimeout.Size = new System.Drawing.Size(265, 26);
            lblRDPConTimeout.TabIndex = 0;
            lblRDPConTimeout.Text = "RDP Connection Timeout";
            lblRDPConTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRdpReconnectionCount
            // 
            lblRdpReconnectionCount.Dock = System.Windows.Forms.DockStyle.Top;
            lblRdpReconnectionCount.Location = new System.Drawing.Point(3, 0);
            lblRdpReconnectionCount.Name = "lblRdpReconnectionCount";
            lblRdpReconnectionCount.Size = new System.Drawing.Size(265, 26);
            lblRdpReconnectionCount.TabIndex = 0;
            lblRdpReconnectionCount.Text = "RDP Reconnection Count";
            lblRdpReconnectionCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numRdpReconnectionCount
            // 
            numRdpReconnectionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRdpReconnectionCount.Location = new System.Drawing.Point(274, 3);
            numRdpReconnectionCount.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numRdpReconnectionCount.Name = "numRdpReconnectionCount";
            numRdpReconnectionCount.Size = new System.Drawing.Size(53, 22);
            numRdpReconnectionCount.TabIndex = 1;
            numRdpReconnectionCount.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // chkSingleClickOnConnectionOpensIt
            // 
            chkSingleClickOnConnectionOpensIt._mice = MrngCheckBox.MouseState.OUT;
            chkSingleClickOnConnectionOpensIt.AutoSize = true;
            chkSingleClickOnConnectionOpensIt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSingleClickOnConnectionOpensIt.Location = new System.Drawing.Point(3, 3);
            chkSingleClickOnConnectionOpensIt.Name = "chkSingleClickOnConnectionOpensIt";
            chkSingleClickOnConnectionOpensIt.Size = new System.Drawing.Size(206, 17);
            chkSingleClickOnConnectionOpensIt.TabIndex = 0;
            chkSingleClickOnConnectionOpensIt.Text = "Single click on connection opens it";
            chkSingleClickOnConnectionOpensIt.UseVisualStyleBackColor = true;
            // 
            // chkHostnameLikeDisplayName
            // 
            chkHostnameLikeDisplayName._mice = MrngCheckBox.MouseState.OUT;
            chkHostnameLikeDisplayName.AutoSize = true;
            chkHostnameLikeDisplayName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkHostnameLikeDisplayName.Location = new System.Drawing.Point(3, 72);
            chkHostnameLikeDisplayName.Name = "chkHostnameLikeDisplayName";
            chkHostnameLikeDisplayName.Size = new System.Drawing.Size(355, 17);
            chkHostnameLikeDisplayName.TabIndex = 2;
            chkHostnameLikeDisplayName.Text = "Set hostname like display name when creating new connections";
            chkHostnameLikeDisplayName.UseVisualStyleBackColor = true;
            // 
            // chkSingleClickOnOpenedConnectionSwitchesToIt
            // 
            chkSingleClickOnOpenedConnectionSwitchesToIt._mice = MrngCheckBox.MouseState.OUT;
            chkSingleClickOnOpenedConnectionSwitchesToIt.AutoSize = true;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSingleClickOnOpenedConnectionSwitchesToIt.Location = new System.Drawing.Point(3, 26);
            chkSingleClickOnOpenedConnectionSwitchesToIt.Name = "chkSingleClickOnOpenedConnectionSwitchesToIt";
            chkSingleClickOnOpenedConnectionSwitchesToIt.Size = new System.Drawing.Size(492, 17);
            chkSingleClickOnOpenedConnectionSwitchesToIt.TabIndex = 1;
            chkSingleClickOnOpenedConnectionSwitchesToIt.Text = Language.SingleClickOnOpenConnectionSwitchesToIt;
            chkSingleClickOnOpenedConnectionSwitchesToIt.UseVisualStyleBackColor = true;
            // 
            // lblAutoSave1
            // 
            lblAutoSave1.Dock = System.Windows.Forms.DockStyle.Top;
            lblAutoSave1.Location = new System.Drawing.Point(3, 52);
            lblAutoSave1.Name = "lblAutoSave1";
            lblAutoSave1.Size = new System.Drawing.Size(265, 26);
            lblAutoSave1.TabIndex = 0;
            lblAutoSave1.Text = "Auto Save  in Minutes (0 means disabled)";
            lblAutoSave1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numAutoSave
            // 
            numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numAutoSave.Location = new System.Drawing.Point(274, 55);
            numAutoSave.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numAutoSave.Name = "numAutoSave";
            numAutoSave.Size = new System.Drawing.Size(53, 22);
            numAutoSave.TabIndex = 1;
            // 
            // pnlConfirmCloseConnection
            // 
            pnlConfirmCloseConnection.Controls.Add(lblClosingConnections);
            pnlConfirmCloseConnection.Controls.Add(radCloseWarnAll);
            pnlConfirmCloseConnection.Controls.Add(radCloseWarnMultiple);
            pnlConfirmCloseConnection.Controls.Add(radCloseWarnExit);
            pnlConfirmCloseConnection.Controls.Add(radCloseWarnNever);
            pnlConfirmCloseConnection.Location = new System.Drawing.Point(3, 270);
            pnlConfirmCloseConnection.Name = "pnlConfirmCloseConnection";
            pnlConfirmCloseConnection.Size = new System.Drawing.Size(604, 137);
            pnlConfirmCloseConnection.TabIndex = 6;
            // 
            // lblClosingConnections
            // 
            lblClosingConnections.AutoSize = true;
            lblClosingConnections.Location = new System.Drawing.Point(3, 12);
            lblClosingConnections.Name = "lblClosingConnections";
            lblClosingConnections.Size = new System.Drawing.Size(147, 13);
            lblClosingConnections.TabIndex = 0;
            lblClosingConnections.Text = "When closing connections:";
            // 
            // radCloseWarnAll
            // 
            radCloseWarnAll.AutoSize = true;
            radCloseWarnAll.BackColor = System.Drawing.Color.Transparent;
            radCloseWarnAll.Location = new System.Drawing.Point(16, 34);
            radCloseWarnAll.Name = "radCloseWarnAll";
            radCloseWarnAll.Size = new System.Drawing.Size(209, 17);
            radCloseWarnAll.TabIndex = 1;
            radCloseWarnAll.TabStop = true;
            radCloseWarnAll.Text = "Warn me when closing connections";
            radCloseWarnAll.UseVisualStyleBackColor = false;
            // 
            // radCloseWarnMultiple
            // 
            radCloseWarnMultiple.AutoSize = true;
            radCloseWarnMultiple.BackColor = System.Drawing.Color.Transparent;
            radCloseWarnMultiple.Location = new System.Drawing.Point(16, 57);
            radCloseWarnMultiple.Name = "radCloseWarnMultiple";
            radCloseWarnMultiple.Size = new System.Drawing.Size(279, 17);
            radCloseWarnMultiple.TabIndex = 2;
            radCloseWarnMultiple.TabStop = true;
            radCloseWarnMultiple.Text = "Warn me only when closing multiple connections";
            radCloseWarnMultiple.UseVisualStyleBackColor = false;
            // 
            // radCloseWarnExit
            // 
            radCloseWarnExit.AutoSize = true;
            radCloseWarnExit.BackColor = System.Drawing.Color.Transparent;
            radCloseWarnExit.Location = new System.Drawing.Point(16, 80);
            radCloseWarnExit.Name = "radCloseWarnExit";
            radCloseWarnExit.Size = new System.Drawing.Size(233, 17);
            radCloseWarnExit.TabIndex = 3;
            radCloseWarnExit.TabStop = true;
            radCloseWarnExit.Text = "Warn me only when exiting mRemoteNG";
            radCloseWarnExit.UseVisualStyleBackColor = false;
            // 
            // radCloseWarnNever
            // 
            radCloseWarnNever.AutoSize = true;
            radCloseWarnNever.BackColor = System.Drawing.Color.Transparent;
            radCloseWarnNever.Location = new System.Drawing.Point(16, 103);
            radCloseWarnNever.Name = "radCloseWarnNever";
            radCloseWarnNever.Size = new System.Drawing.Size(246, 17);
            radCloseWarnNever.TabIndex = 4;
            radCloseWarnNever.TabStop = true;
            radCloseWarnNever.Text = "Do not warn me when closing connections";
            radCloseWarnNever.UseVisualStyleBackColor = false;
            // 
            // chkSaveConnectionsAfterEveryEdit
            // 
            chkSaveConnectionsAfterEveryEdit._mice = MrngCheckBox.MouseState.OUT;
            chkSaveConnectionsAfterEveryEdit.AutoSize = true;
            chkSaveConnectionsAfterEveryEdit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSaveConnectionsAfterEveryEdit.Location = new System.Drawing.Point(3, 95);
            chkSaveConnectionsAfterEveryEdit.Name = "chkSaveConnectionsAfterEveryEdit";
            chkSaveConnectionsAfterEveryEdit.Size = new System.Drawing.Size(194, 17);
            chkSaveConnectionsAfterEveryEdit.TabIndex = 7;
            chkSaveConnectionsAfterEveryEdit.Text = "Save connections after every edit";
            chkSaveConnectionsAfterEveryEdit.UseVisualStyleBackColor = true;
            // 
            // chkUseFilterSearch
            // 
            chkUseFilterSearch._mice = MrngCheckBox.MouseState.OUT;
            chkUseFilterSearch.AutoSize = true;
            chkUseFilterSearch.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseFilterSearch.Location = new System.Drawing.Point(3, 118);
            chkUseFilterSearch.Name = "chkUseFilterSearch";
            chkUseFilterSearch.Size = new System.Drawing.Size(230, 17);
            chkUseFilterSearch.TabIndex = 8;
            chkUseFilterSearch.Text = "Filter search matches in connection tree";
            chkUseFilterSearch.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            tableLayoutPanel2.Controls.Add(numRdpReconnectionCount, 1, 0);
            tableLayoutPanel2.Controls.Add(numAutoSave, 1, 2);
            tableLayoutPanel2.Controls.Add(lblRdpReconnectionCount, 0, 0);
            tableLayoutPanel2.Controls.Add(lblAutoSave1, 0, 2);
            tableLayoutPanel2.Controls.Add(lblRDPConTimeout, 0, 1);
            tableLayoutPanel2.Controls.Add(numRDPConTimeout, 1, 1);
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 185);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.Size = new System.Drawing.Size(604, 79);
            tableLayoutPanel2.TabIndex = 9;
            // 
            // chkPlaceSearchBarAboveConnectionTree
            // 
            chkPlaceSearchBarAboveConnectionTree._mice = MrngCheckBox.MouseState.OUT;
            chkPlaceSearchBarAboveConnectionTree.AutoSize = true;
            chkPlaceSearchBarAboveConnectionTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkPlaceSearchBarAboveConnectionTree.Location = new System.Drawing.Point(3, 141);
            chkPlaceSearchBarAboveConnectionTree.Name = "chkPlaceSearchBarAboveConnectionTree";
            chkPlaceSearchBarAboveConnectionTree.Size = new System.Drawing.Size(226, 17);
            chkPlaceSearchBarAboveConnectionTree.TabIndex = 8;
            chkPlaceSearchBarAboveConnectionTree.Text = "Place search bar above connection tree";
            chkPlaceSearchBarAboveConnectionTree.UseVisualStyleBackColor = true;
            // 
            // chkConnectionTreeTrackActiveConnection
            // 
            chkConnectionTreeTrackActiveConnection._mice = MrngCheckBox.MouseState.OUT;
            chkConnectionTreeTrackActiveConnection.AutoSize = true;
            chkConnectionTreeTrackActiveConnection.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkConnectionTreeTrackActiveConnection.Location = new System.Drawing.Point(3, 49);
            chkConnectionTreeTrackActiveConnection.Name = "chkConnectionTreeTrackActiveConnection";
            chkConnectionTreeTrackActiveConnection.Size = new System.Drawing.Size(262, 17);
            chkConnectionTreeTrackActiveConnection.TabIndex = 10;
            chkConnectionTreeTrackActiveConnection.Text = "Track active connection in the connection tree";
            chkConnectionTreeTrackActiveConnection.UseVisualStyleBackColor = true;
            // 
            // chkDoNotTrimUsername
            // 
            chkDoNotTrimUsername._mice = MrngCheckBox.MouseState.OUT;
            chkDoNotTrimUsername.AutoSize = true;
            chkDoNotTrimUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkDoNotTrimUsername.Location = new System.Drawing.Point(3, 165);
            chkDoNotTrimUsername.Name = "chkDoNotTrimUsername";
            chkDoNotTrimUsername.Size = new System.Drawing.Size(143, 17);
            chkDoNotTrimUsername.TabIndex = 11;
            chkDoNotTrimUsername.Text = "Do not trim usernames";
            chkDoNotTrimUsername.UseVisualStyleBackColor = true;
            // 
            // ConnectionsPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(chkDoNotTrimUsername);
            Controls.Add(chkConnectionTreeTrackActiveConnection);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(chkPlaceSearchBarAboveConnectionTree);
            Controls.Add(chkUseFilterSearch);
            Controls.Add(chkSaveConnectionsAfterEveryEdit);
            Controls.Add(chkSingleClickOnConnectionOpensIt);
            Controls.Add(chkHostnameLikeDisplayName);
            Controls.Add(chkSingleClickOnOpenedConnectionSwitchesToIt);
            Controls.Add(pnlConfirmCloseConnection);
            Name = "ConnectionsPage";
            Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)numRDPConTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRdpReconnectionCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numAutoSave).EndInit();
            pnlConfirmCloseConnection.ResumeLayout(false);
            pnlConfirmCloseConnection.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        internal Controls.MrngLabel lblRdpReconnectionCount;
        internal MrngCheckBox chkSingleClickOnConnectionOpensIt;
        internal MrngCheckBox chkHostnameLikeDisplayName;
        internal MrngCheckBox chkSingleClickOnOpenedConnectionSwitchesToIt;
        internal Controls.MrngLabel lblAutoSave1;
        internal Controls.MrngNumericUpDown numAutoSave;
        internal System.Windows.Forms.Panel pnlConfirmCloseConnection;
        internal Controls.MrngLabel lblClosingConnections;
        internal Controls.MrngRadioButton radCloseWarnAll;
        internal Controls.MrngRadioButton radCloseWarnMultiple;
        internal Controls.MrngRadioButton radCloseWarnExit;
        internal Controls.MrngRadioButton radCloseWarnNever;
        internal Controls.MrngNumericUpDown numRDPConTimeout;
        internal Controls.MrngLabel lblRDPConTimeout;
        internal Controls.MrngNumericUpDown numRdpReconnectionCount;
        internal MrngCheckBox chkSaveConnectionsAfterEveryEdit;
        private MrngCheckBox chkUseFilterSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MrngCheckBox chkPlaceSearchBarAboveConnectionTree;
        private MrngCheckBox chkConnectionTreeTrackActiveConnection;
        private MrngCheckBox chkDoNotTrimUsername;
    }
}
