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
                try { base.Dispose(disposing); }
                catch (System.NullReferenceException) { /* finalizer-safe: Control.ContextMenuStrip may be null on non-STA thread */ }
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
            chkWatchConnectionFile = new MrngCheckBox();
            chkDoubleClickOpensNewConnection = new MrngCheckBox();
            chkDefaultInheritance = new MrngCheckBox();
            chkDisableTreeDragAndDrop = new MrngCheckBox();
            pnlOptions = new System.Windows.Forms.Panel();
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)numRDPConTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRdpReconnectionCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numAutoSave).BeginInit();
            pnlConfirmCloseConnection.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            pnlOptions.SuspendLayout();
            SuspendLayout();
            // 
            // numRDPConTimeout
            // 
            numRDPConTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRDPConTimeout.Location = new System.Drawing.Point(277, 29);
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
            lblRDPConTimeout.Size = new System.Drawing.Size(268, 26);
            lblRDPConTimeout.TabIndex = 0;
            lblRDPConTimeout.Text = "RDP Connection Timeout";
            lblRDPConTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRdpReconnectionCount
            // 
            lblRdpReconnectionCount.Dock = System.Windows.Forms.DockStyle.Top;
            lblRdpReconnectionCount.Location = new System.Drawing.Point(3, 0);
            lblRdpReconnectionCount.Name = "lblRdpReconnectionCount";
            lblRdpReconnectionCount.Size = new System.Drawing.Size(268, 26);
            lblRdpReconnectionCount.TabIndex = 0;
            lblRdpReconnectionCount.Text = "RDP Reconnection Count";
            lblRdpReconnectionCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numRdpReconnectionCount
            // 
            numRdpReconnectionCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRdpReconnectionCount.Location = new System.Drawing.Point(277, 3);
            numRdpReconnectionCount.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
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
            chkSingleClickOnConnectionOpensIt.Location = new System.Drawing.Point(6, 3);
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
            chkHostnameLikeDisplayName.Location = new System.Drawing.Point(6, 72);
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
            chkSingleClickOnOpenedConnectionSwitchesToIt.Location = new System.Drawing.Point(6, 26);
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
            lblAutoSave1.Size = new System.Drawing.Size(268, 26);
            lblAutoSave1.TabIndex = 0;
            lblAutoSave1.Text = "Auto Save  in Minutes (0 means disabled)";
            lblAutoSave1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numAutoSave
            // 
            numAutoSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numAutoSave.Location = new System.Drawing.Point(277, 55);
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
            pnlConfirmCloseConnection.Dock = System.Windows.Forms.DockStyle.Top;
            pnlConfirmCloseConnection.Location = new System.Drawing.Point(0, 343);
            pnlConfirmCloseConnection.Name = "pnlConfirmCloseConnection";
            pnlConfirmCloseConnection.Size = new System.Drawing.Size(610, 133);
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
            chkSaveConnectionsAfterEveryEdit.Location = new System.Drawing.Point(6, 95);
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
            chkUseFilterSearch.Location = new System.Drawing.Point(6, 118);
            chkUseFilterSearch.Name = "chkUseFilterSearch";
            chkUseFilterSearch.Size = new System.Drawing.Size(230, 17);
            chkUseFilterSearch.TabIndex = 8;
            chkUseFilterSearch.Text = "Filter search matches in connection tree";
            chkUseFilterSearch.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(numRdpReconnectionCount, 1, 0);
            tableLayoutPanel2.Controls.Add(numAutoSave, 1, 2);
            tableLayoutPanel2.Controls.Add(lblRdpReconnectionCount, 0, 0);
            tableLayoutPanel2.Controls.Add(lblAutoSave1, 0, 2);
            tableLayoutPanel2.Controls.Add(lblRDPConTimeout, 0, 1);
            tableLayoutPanel2.Controls.Add(numRDPConTimeout, 1, 1);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel2.Location = new System.Drawing.Point(0, 264);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanel2.Size = new System.Drawing.Size(610, 79);
            tableLayoutPanel2.TabIndex = 9;
            // 
            // chkPlaceSearchBarAboveConnectionTree
            // 
            chkPlaceSearchBarAboveConnectionTree._mice = MrngCheckBox.MouseState.OUT;
            chkPlaceSearchBarAboveConnectionTree.AutoSize = true;
            chkPlaceSearchBarAboveConnectionTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkPlaceSearchBarAboveConnectionTree.Location = new System.Drawing.Point(6, 141);
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
            chkConnectionTreeTrackActiveConnection.Location = new System.Drawing.Point(6, 49);
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
            chkDoNotTrimUsername.Location = new System.Drawing.Point(6, 165);
            chkDoNotTrimUsername.Name = "chkDoNotTrimUsername";
            chkDoNotTrimUsername.Size = new System.Drawing.Size(143, 17);
            chkDoNotTrimUsername.TabIndex = 11;
            chkDoNotTrimUsername.Text = "Do not trim usernames";
            chkDoNotTrimUsername.UseVisualStyleBackColor = true;
            // 
            // chkWatchConnectionFile
            // 
            chkWatchConnectionFile._mice = MrngCheckBox.MouseState.OUT;
            chkWatchConnectionFile.AutoSize = true;
            chkWatchConnectionFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkWatchConnectionFile.Location = new System.Drawing.Point(6, 188);
            chkWatchConnectionFile.Name = "chkWatchConnectionFile";
            chkWatchConnectionFile.Size = new System.Drawing.Size(243, 17);
            chkWatchConnectionFile.TabIndex = 12;
            chkWatchConnectionFile.Text = "Watch connection file for external changes";
            chkWatchConnectionFile.UseVisualStyleBackColor = true;
            // 
            // chkDoubleClickOpensNewConnection
            // 
            chkDoubleClickOpensNewConnection._mice = MrngCheckBox.MouseState.OUT;
            chkDoubleClickOpensNewConnection.AutoSize = true;
            chkDoubleClickOpensNewConnection.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkDoubleClickOpensNewConnection.Location = new System.Drawing.Point(6, 211);
            chkDoubleClickOpensNewConnection.Name = "chkDoubleClickOpensNewConnection";
            chkDoubleClickOpensNewConnection.Size = new System.Drawing.Size(243, 17);
            chkDoubleClickOpensNewConnection.TabIndex = 13;
            chkDoubleClickOpensNewConnection.Text = "Double click opens duplicate connection";
            chkDoubleClickOpensNewConnection.UseVisualStyleBackColor = true;
            //
            // chkDefaultInheritance
            //
            chkDefaultInheritance._mice = MrngCheckBox.MouseState.OUT;
            chkDefaultInheritance.AutoSize = true;
            chkDefaultInheritance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkDefaultInheritance.Location = new System.Drawing.Point(6, 234);
            chkDefaultInheritance.Name = "chkDefaultInheritance";
            chkDefaultInheritance.Size = new System.Drawing.Size(350, 17);
            chkDefaultInheritance.TabIndex = 14;
            chkDefaultInheritance.Text = "New connections inherit all properties from parent by default";
            chkDefaultInheritance.UseVisualStyleBackColor = true;
            //
            // chkDisableTreeDragAndDrop
            //
            chkDisableTreeDragAndDrop._mice = MrngCheckBox.MouseState.OUT;
            chkDisableTreeDragAndDrop.AutoSize = true;
            chkDisableTreeDragAndDrop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkDisableTreeDragAndDrop.Location = new System.Drawing.Point(6, 257);
            chkDisableTreeDragAndDrop.Name = "chkDisableTreeDragAndDrop";
            chkDisableTreeDragAndDrop.Size = new System.Drawing.Size(300, 17);
            chkDisableTreeDragAndDrop.TabIndex = 15;
            chkDisableTreeDragAndDrop.Text = "Disable drag and drop in the connection tree";
            chkDisableTreeDragAndDrop.UseVisualStyleBackColor = true;
            //
            // pnlOptions
            // 
            pnlOptions.Controls.Add(chkSingleClickOnConnectionOpensIt);
            pnlOptions.Controls.Add(chkDoNotTrimUsername);
            pnlOptions.Controls.Add(chkWatchConnectionFile);
            pnlOptions.Controls.Add(chkDoubleClickOpensNewConnection);
            pnlOptions.Controls.Add(chkDisableTreeDragAndDrop);
            pnlOptions.Controls.Add(chkDefaultInheritance);
            pnlOptions.Controls.Add(chkSingleClickOnOpenedConnectionSwitchesToIt);
            pnlOptions.Controls.Add(chkConnectionTreeTrackActiveConnection);
            pnlOptions.Controls.Add(chkHostnameLikeDisplayName);
            pnlOptions.Controls.Add(chkSaveConnectionsAfterEveryEdit);
            pnlOptions.Controls.Add(chkPlaceSearchBarAboveConnectionTree);
            pnlOptions.Controls.Add(chkUseFilterSearch);
            pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlOptions.Location = new System.Drawing.Point(0, 30);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new System.Drawing.Size(610, 280);
            pnlOptions.TabIndex = 12;
            // 
            // lblRegistrySettingsUsedInfo
            // 
            lblRegistrySettingsUsedInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            lblRegistrySettingsUsedInfo.Dock = System.Windows.Forms.DockStyle.Top;
            lblRegistrySettingsUsedInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            lblRegistrySettingsUsedInfo.Location = new System.Drawing.Point(0, 0);
            lblRegistrySettingsUsedInfo.Name = "lblRegistrySettingsUsedInfo";
            lblRegistrySettingsUsedInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            lblRegistrySettingsUsedInfo.Size = new System.Drawing.Size(610, 30);
            lblRegistrySettingsUsedInfo.TabIndex = 13;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.Visible = false;
            // 
            // ConnectionsPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(pnlConfirmCloseConnection);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(pnlOptions);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Name = "ConnectionsPage";
            Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)numRDPConTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRdpReconnectionCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numAutoSave).EndInit();
            pnlConfirmCloseConnection.ResumeLayout(false);
            pnlConfirmCloseConnection.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            pnlOptions.ResumeLayout(false);
            pnlOptions.PerformLayout();
            ResumeLayout(false);
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
        private MrngCheckBox chkWatchConnectionFile;
        private MrngCheckBox chkDoubleClickOpensNewConnection;
        private MrngCheckBox chkDefaultInheritance;
        private MrngCheckBox chkDisableTreeDragAndDrop;
        internal System.Windows.Forms.Panel pnlOptions;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
    }
}
