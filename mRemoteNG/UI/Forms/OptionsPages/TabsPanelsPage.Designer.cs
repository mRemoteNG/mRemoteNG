using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class TabsPanelsPage : OptionsPage
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
            chkAlwaysShowPanelTabs = new MrngCheckBox();
            chkAlwaysShowConnectionTabs = new MrngCheckBox();
            chkIdentifyQuickConnectTabs = new MrngCheckBox();
            chkOpenNewTabRightOfSelected = new MrngCheckBox();
            chkAlwaysShowPanelSelectionDlg = new MrngCheckBox();
            chkShowLogonInfoOnTabs = new MrngCheckBox();
            chkDoubleClickClosesTab = new MrngCheckBox();
            chkShowProtocolOnTabs = new MrngCheckBox();
            chkCreateEmptyPanelOnStart = new MrngCheckBox();
            chkBindConnectionsAndConfigPanels = new MrngCheckBox();
            chkShowFolderPathOnTabs = new MrngCheckBox();
            txtBoxPanelName = new MrngTextBox();
            lblPanelName = new MrngLabel();
            nudSplitterSize = new MrngNumericUpDown();
            lblSplitterSize = new MrngLabel();
            nudDockPadding = new MrngNumericUpDown();
            lblDockPadding = new MrngLabel();
            chkLockPanels = new MrngCheckBox();
            chkDoNotRestoreOnRdpMinimize = new MrngCheckBox();
            chkAutoClosePanelOnLastTabClose = new MrngCheckBox();
            chkUseCustomConnectionTabColor = new MrngCheckBox();
            txtConnectionTabColor = new MrngTextBox();
            btnSelectConnectionTabColor = new MrngButton();
            chkMinimizePanelsOnConnect = new MrngCheckBox();
            chkKeepTabsOpenAfterDisconnect = new MrngCheckBox();
            chkUseCustomConnectionTabFont = new MrngCheckBox();
            txtConnectionTabFont = new MrngTextBox();
            btnSelectConnectionTabFont = new MrngButton();
            pnlOptions = new System.Windows.Forms.Panel();
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            pnlOptions.SuspendLayout();
            SuspendLayout();
            // 
            // chkAlwaysShowPanelTabs
            // 
            chkAlwaysShowPanelTabs._mice = MrngCheckBox.MouseState.OUT;
            chkAlwaysShowPanelTabs.AutoSize = true;
            chkAlwaysShowPanelTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkAlwaysShowPanelTabs.Location = new System.Drawing.Point(3, 3);
            chkAlwaysShowPanelTabs.Name = "chkAlwaysShowPanelTabs";
            chkAlwaysShowPanelTabs.Size = new System.Drawing.Size(149, 17);
            chkAlwaysShowPanelTabs.TabIndex = 0;
            chkAlwaysShowPanelTabs.Text = "Always show panel tabs";
            chkAlwaysShowPanelTabs.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysShowConnectionTabs
            // 
            chkAlwaysShowConnectionTabs._mice = MrngCheckBox.MouseState.OUT;
            chkAlwaysShowConnectionTabs.AutoSize = true;
            chkAlwaysShowConnectionTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkAlwaysShowConnectionTabs.Location = new System.Drawing.Point(3, 26);
            chkAlwaysShowConnectionTabs.Name = "chkAlwaysShowConnectionTabs";
            chkAlwaysShowConnectionTabs.Size = new System.Drawing.Size(178, 17);
            chkAlwaysShowConnectionTabs.TabIndex = 0;
            chkAlwaysShowConnectionTabs.Text = "Always show connection tabs";
            chkAlwaysShowConnectionTabs.UseVisualStyleBackColor = true;
            // 
            // chkIdentifyQuickConnectTabs
            // 
            chkIdentifyQuickConnectTabs._mice = MrngCheckBox.MouseState.OUT;
            chkIdentifyQuickConnectTabs.AutoSize = true;
            chkIdentifyQuickConnectTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkIdentifyQuickConnectTabs.Location = new System.Drawing.Point(3, 95);
            chkIdentifyQuickConnectTabs.Name = "chkIdentifyQuickConnectTabs";
            chkIdentifyQuickConnectTabs.Size = new System.Drawing.Size(315, 17);
            chkIdentifyQuickConnectTabs.TabIndex = 4;
            chkIdentifyQuickConnectTabs.Text = Language.IdentifyQuickConnectTabs;
            chkIdentifyQuickConnectTabs.UseVisualStyleBackColor = true;
            // 
            // chkOpenNewTabRightOfSelected
            // 
            chkOpenNewTabRightOfSelected._mice = MrngCheckBox.MouseState.OUT;
            chkOpenNewTabRightOfSelected.AutoSize = true;
            chkOpenNewTabRightOfSelected.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkOpenNewTabRightOfSelected.Location = new System.Drawing.Point(3, 49);
            chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected";
            chkOpenNewTabRightOfSelected.Size = new System.Drawing.Size(309, 17);
            chkOpenNewTabRightOfSelected.TabIndex = 1;
            chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab";
            chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysShowPanelSelectionDlg
            // 
            chkAlwaysShowPanelSelectionDlg._mice = MrngCheckBox.MouseState.OUT;
            chkAlwaysShowPanelSelectionDlg.AutoSize = true;
            chkAlwaysShowPanelSelectionDlg.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkAlwaysShowPanelSelectionDlg.Location = new System.Drawing.Point(3, 141);
            chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg";
            chkAlwaysShowPanelSelectionDlg.Size = new System.Drawing.Size(347, 17);
            chkAlwaysShowPanelSelectionDlg.TabIndex = 6;
            chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins";
            chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = true;
            // 
            // chkShowLogonInfoOnTabs
            // 
            chkShowLogonInfoOnTabs._mice = MrngCheckBox.MouseState.OUT;
            chkShowLogonInfoOnTabs.AutoSize = true;
            chkShowLogonInfoOnTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowLogonInfoOnTabs.Location = new System.Drawing.Point(3, 26);
            chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs";
            chkShowLogonInfoOnTabs.Size = new System.Drawing.Size(226, 17);
            chkShowLogonInfoOnTabs.TabIndex = 2;
            chkShowLogonInfoOnTabs.Text = "Show logon information on tab names";
            chkShowLogonInfoOnTabs.UseVisualStyleBackColor = true;
            // 
            // chkDoubleClickClosesTab
            // 
            chkDoubleClickClosesTab._mice = MrngCheckBox.MouseState.OUT;
            chkDoubleClickClosesTab.AutoSize = true;
            chkDoubleClickClosesTab.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkDoubleClickClosesTab.Location = new System.Drawing.Point(3, 118);
            chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab";
            chkDoubleClickClosesTab.Size = new System.Drawing.Size(170, 17);
            chkDoubleClickClosesTab.TabIndex = 5;
            chkDoubleClickClosesTab.Text = "Double click on tab closes it";
            chkDoubleClickClosesTab.UseVisualStyleBackColor = true;
            //
            // chkShowProtocolOnTabs
            //
            chkShowProtocolOnTabs._mice = MrngCheckBox.MouseState.OUT;
            chkShowProtocolOnTabs.AutoSize = true;
            chkShowProtocolOnTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowProtocolOnTabs.Location = new System.Drawing.Point(3, 49);
            chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs";
            chkShowProtocolOnTabs.Size = new System.Drawing.Size(180, 17);
            chkShowProtocolOnTabs.TabIndex = 3;
            chkShowProtocolOnTabs.Text = "Show protocols on tab names";
            chkShowProtocolOnTabs.UseVisualStyleBackColor = true;
            //
            // chkShowFolderPathOnTabs
            //
            chkShowFolderPathOnTabs._mice = MrngCheckBox.MouseState.OUT;
            chkShowFolderPathOnTabs.AutoSize = true;
            chkShowFolderPathOnTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowFolderPathOnTabs.Location = new System.Drawing.Point(3, 72);
            chkShowFolderPathOnTabs.Name = "chkShowFolderPathOnTabs";
            chkShowFolderPathOnTabs.Size = new System.Drawing.Size(250, 17);
            chkShowFolderPathOnTabs.TabIndex = 10;
            chkShowFolderPathOnTabs.Text = "Show folder path on tab names";
            chkShowFolderPathOnTabs.UseVisualStyleBackColor = true;
            // 
            // chkCreateEmptyPanelOnStart
            // 
            chkCreateEmptyPanelOnStart._mice = MrngCheckBox.MouseState.OUT;
            chkCreateEmptyPanelOnStart.AutoSize = true;
            chkCreateEmptyPanelOnStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkCreateEmptyPanelOnStart.Location = new System.Drawing.Point(3, 164);
            chkCreateEmptyPanelOnStart.Name = "chkCreateEmptyPanelOnStart";
            chkCreateEmptyPanelOnStart.Size = new System.Drawing.Size(271, 17);
            chkCreateEmptyPanelOnStart.TabIndex = 7;
            chkCreateEmptyPanelOnStart.Text = "Create an empty panel when mRemoteNG starts";
            chkCreateEmptyPanelOnStart.UseVisualStyleBackColor = true;
            chkCreateEmptyPanelOnStart.CheckedChanged += chkCreateEmptyPanelOnStart_CheckedChanged;
            // 
            // chkBindConnectionsAndConfigPanels
            // 
            chkBindConnectionsAndConfigPanels._mice = MrngCheckBox.MouseState.OUT;
            chkBindConnectionsAndConfigPanels.AutoSize = true;
            chkBindConnectionsAndConfigPanels.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkBindConnectionsAndConfigPanels.Location = new System.Drawing.Point(3, 233);
            chkBindConnectionsAndConfigPanels.Name = "chkBindConnectionsAndConfigPanels";
            chkBindConnectionsAndConfigPanels.Size = new System.Drawing.Size(350, 17);
            chkBindConnectionsAndConfigPanels.TabIndex = 9;
            chkBindConnectionsAndConfigPanels.Text = "Bind Connections and Config panels together when auto-hidden";
            chkBindConnectionsAndConfigPanels.UseVisualStyleBackColor = true;
            // 
            // txtBoxPanelName
            // 
            txtBoxPanelName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtBoxPanelName.Location = new System.Drawing.Point(35, 200);
            txtBoxPanelName.Name = "txtBoxPanelName";
            txtBoxPanelName.Size = new System.Drawing.Size(213, 22);
            txtBoxPanelName.TabIndex = 8;
            // 
            // lblPanelName
            // 
            lblPanelName.AutoSize = true;
            lblPanelName.Location = new System.Drawing.Point(32, 184);
            lblPanelName.Name = "lblPanelName";
            lblPanelName.Size = new System.Drawing.Size(69, 13);
            lblPanelName.TabIndex = 9;
            lblPanelName.Text = "Panel name:";
            // 
            // lblSplitterSize
            // 
            lblSplitterSize.AutoSize = true;
            lblSplitterSize.Location = new System.Drawing.Point(3, 260);
            lblSplitterSize.Name = "lblSplitterSize";
            lblSplitterSize.Size = new System.Drawing.Size(66, 13);
            lblSplitterSize.TabIndex = 11;
            lblSplitterSize.Text = "Splitter size:";
            // 
            // nudSplitterSize
            // 
            nudSplitterSize.Location = new System.Drawing.Point(80, 258);
            nudSplitterSize.Name = "nudSplitterSize";
            nudSplitterSize.Size = new System.Drawing.Size(50, 22);
            nudSplitterSize.TabIndex = 12;
            nudSplitterSize.Minimum = 3;
            nudSplitterSize.Maximum = 20;
            //
            // lblDockPadding
            //
            lblDockPadding.AutoSize = true;
            lblDockPadding.Location = new System.Drawing.Point(3, 285);
            lblDockPadding.Name = "lblDockPadding";
            lblDockPadding.Size = new System.Drawing.Size(73, 13);
            lblDockPadding.TabIndex = 23;
            lblDockPadding.Text = "Border size:";
            //
            // nudDockPadding
            //
            nudDockPadding.Location = new System.Drawing.Point(80, 283);
            nudDockPadding.Name = "nudDockPadding";
            nudDockPadding.Size = new System.Drawing.Size(50, 22);
            nudDockPadding.TabIndex = 24;
            nudDockPadding.Minimum = 0;
            nudDockPadding.Maximum = 10;
            //
            // chkLockPanels
            //
            chkLockPanels._mice = MrngCheckBox.MouseState.OUT;
            chkLockPanels.AutoSize = true;
            chkLockPanels.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLockPanels.Location = new System.Drawing.Point(3, 310);
            chkLockPanels.Name = "chkLockPanels";
            chkLockPanels.Size = new System.Drawing.Size(86, 17);
            chkLockPanels.TabIndex = 13;
            chkLockPanels.Text = "Lock panels";
            chkLockPanels.UseVisualStyleBackColor = true;
            // 
            // chkDoNotRestoreOnRdpMinimize
            // 
            chkDoNotRestoreOnRdpMinimize._mice = MrngCheckBox.MouseState.OUT;
            chkDoNotRestoreOnRdpMinimize.AutoSize = true;
            chkDoNotRestoreOnRdpMinimize.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkDoNotRestoreOnRdpMinimize.Location = new System.Drawing.Point(3, 333);
            chkDoNotRestoreOnRdpMinimize.Name = "chkDoNotRestoreOnRdpMinimize";
            chkDoNotRestoreOnRdpMinimize.Size = new System.Drawing.Size(199, 17);
            chkDoNotRestoreOnRdpMinimize.TabIndex = 14;
            chkDoNotRestoreOnRdpMinimize.Text = "Do not restore on RDP minimize";
            chkDoNotRestoreOnRdpMinimize.UseVisualStyleBackColor = true;
            // 
            // chkAutoClosePanelOnLastTabClose
            // 
            chkAutoClosePanelOnLastTabClose._mice = MrngCheckBox.MouseState.OUT;
            chkAutoClosePanelOnLastTabClose.AutoSize = true;
            chkAutoClosePanelOnLastTabClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkAutoClosePanelOnLastTabClose.Location = new System.Drawing.Point(3, 356);
            chkAutoClosePanelOnLastTabClose.Name = "chkAutoClosePanelOnLastTabClose";
            chkAutoClosePanelOnLastTabClose.Size = new System.Drawing.Size(258, 17);
            chkAutoClosePanelOnLastTabClose.TabIndex = 15;
            chkAutoClosePanelOnLastTabClose.Text = "Auto close panel after closing the last tab";
            chkAutoClosePanelOnLastTabClose.UseVisualStyleBackColor = true;
            //
            // chkMinimizePanelsOnConnect
            //
            chkMinimizePanelsOnConnect._mice = MrngCheckBox.MouseState.OUT;
            chkMinimizePanelsOnConnect.AutoSize = true;
            chkMinimizePanelsOnConnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkMinimizePanelsOnConnect.Location = new System.Drawing.Point(3, 379);
            chkMinimizePanelsOnConnect.Name = "chkMinimizePanelsOnConnect";
            chkMinimizePanelsOnConnect.Size = new System.Drawing.Size(300, 17);
            chkMinimizePanelsOnConnect.TabIndex = 16;
            chkMinimizePanelsOnConnect.Text = "Auto-hide Connections/Config panels when a connection opens";
            chkMinimizePanelsOnConnect.UseVisualStyleBackColor = true;
            //
            // chkKeepTabsOpenAfterDisconnect
            //
            chkKeepTabsOpenAfterDisconnect._mice = MrngCheckBox.MouseState.OUT;
            chkKeepTabsOpenAfterDisconnect.AutoSize = true;
            chkKeepTabsOpenAfterDisconnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkKeepTabsOpenAfterDisconnect.Location = new System.Drawing.Point(3, 402);
            chkKeepTabsOpenAfterDisconnect.Name = "chkKeepTabsOpenAfterDisconnect";
            chkKeepTabsOpenAfterDisconnect.Size = new System.Drawing.Size(300, 17);
            chkKeepTabsOpenAfterDisconnect.TabIndex = 25;
            chkKeepTabsOpenAfterDisconnect.Text = "Keep tabs open after disconnecting";
            chkKeepTabsOpenAfterDisconnect.UseVisualStyleBackColor = true;
            //
            // chkUseCustomConnectionTabColor
            //
            chkUseCustomConnectionTabColor._mice = MrngCheckBox.MouseState.OUT;
            chkUseCustomConnectionTabColor.AutoSize = true;
            chkUseCustomConnectionTabColor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseCustomConnectionTabColor.Location = new System.Drawing.Point(3, 425);
            chkUseCustomConnectionTabColor.Name = "chkUseCustomConnectionTabColor";
            chkUseCustomConnectionTabColor.Size = new System.Drawing.Size(178, 17);
            chkUseCustomConnectionTabColor.TabIndex = 17;
            chkUseCustomConnectionTabColor.Text = "Use custom connection tab color";
            chkUseCustomConnectionTabColor.UseVisualStyleBackColor = true;
            chkUseCustomConnectionTabColor.CheckedChanged += chkUseCustomConnectionTabColor_CheckedChanged;
            //
            // txtConnectionTabColor
            //
            txtConnectionTabColor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtConnectionTabColor.Location = new System.Drawing.Point(25, 448);
            txtConnectionTabColor.Name = "txtConnectionTabColor";
            txtConnectionTabColor.ReadOnly = true;
            txtConnectionTabColor.Size = new System.Drawing.Size(120, 22);
            txtConnectionTabColor.TabIndex = 18;
            //
            // btnSelectConnectionTabColor
            //
            btnSelectConnectionTabColor._mice = MrngButton.MouseState.OUT;
            btnSelectConnectionTabColor.Location = new System.Drawing.Point(151, 447);
            btnSelectConnectionTabColor.Name = "btnSelectConnectionTabColor";
            btnSelectConnectionTabColor.Size = new System.Drawing.Size(75, 23);
            btnSelectConnectionTabColor.TabIndex = 19;
            btnSelectConnectionTabColor.Text = "Select...";
            btnSelectConnectionTabColor.UseVisualStyleBackColor = true;
            btnSelectConnectionTabColor.Click += btnSelectConnectionTabColor_Click;
            //
            // chkUseCustomConnectionTabFont
            //
            chkUseCustomConnectionTabFont._mice = MrngCheckBox.MouseState.OUT;
            chkUseCustomConnectionTabFont.AutoSize = true;
            chkUseCustomConnectionTabFont.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseCustomConnectionTabFont.Location = new System.Drawing.Point(3, 477);
            chkUseCustomConnectionTabFont.Name = "chkUseCustomConnectionTabFont";
            chkUseCustomConnectionTabFont.Size = new System.Drawing.Size(175, 17);
            chkUseCustomConnectionTabFont.TabIndex = 20;
            chkUseCustomConnectionTabFont.Text = "Use custom connection tab font";
            chkUseCustomConnectionTabFont.UseVisualStyleBackColor = true;
            chkUseCustomConnectionTabFont.CheckedChanged += chkUseCustomConnectionTabFont_CheckedChanged;
            //
            // txtConnectionTabFont
            //
            txtConnectionTabFont.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtConnectionTabFont.Location = new System.Drawing.Point(25, 500);
            txtConnectionTabFont.Name = "txtConnectionTabFont";
            txtConnectionTabFont.ReadOnly = true;
            txtConnectionTabFont.Size = new System.Drawing.Size(220, 22);
            txtConnectionTabFont.TabIndex = 21;
            //
            // btnSelectConnectionTabFont
            //
            btnSelectConnectionTabFont._mice = MrngButton.MouseState.OUT;
            btnSelectConnectionTabFont.Location = new System.Drawing.Point(251, 499);
            btnSelectConnectionTabFont.Name = "btnSelectConnectionTabFont";
            btnSelectConnectionTabFont.Size = new System.Drawing.Size(75, 23);
            btnSelectConnectionTabFont.TabIndex = 22;
            btnSelectConnectionTabFont.Text = "Select...";
            btnSelectConnectionTabFont.UseVisualStyleBackColor = true;
            btnSelectConnectionTabFont.Click += btnSelectConnectionTabFont_Click;
            // 
            // pnlOptions
            // 
            pnlOptions.Controls.Add(chkAlwaysShowPanelTabs);
            pnlOptions.Controls.Add(lblPanelName);
            pnlOptions.Controls.Add(chkShowFolderPathOnTabs);
            pnlOptions.Controls.Add(chkShowProtocolOnTabs);
            pnlOptions.Controls.Add(txtBoxPanelName);
            pnlOptions.Controls.Add(chkDoubleClickClosesTab);
            pnlOptions.Controls.Add(chkCreateEmptyPanelOnStart);
            pnlOptions.Controls.Add(chkBindConnectionsAndConfigPanels);
            pnlOptions.Controls.Add(chkShowLogonInfoOnTabs);
            pnlOptions.Controls.Add(chkAlwaysShowPanelSelectionDlg);
            pnlOptions.Controls.Add(chkAlwaysShowConnectionTabs);
            pnlOptions.Controls.Add(chkOpenNewTabRightOfSelected);
            pnlOptions.Controls.Add(chkIdentifyQuickConnectTabs);
            pnlOptions.Controls.Add(nudSplitterSize);
            pnlOptions.Controls.Add(lblSplitterSize);
            pnlOptions.Controls.Add(nudDockPadding);
            pnlOptions.Controls.Add(lblDockPadding);
            pnlOptions.Controls.Add(chkLockPanels);
            pnlOptions.Controls.Add(chkDoNotRestoreOnRdpMinimize);
            pnlOptions.Controls.Add(chkAutoClosePanelOnLastTabClose);
            pnlOptions.Controls.Add(chkMinimizePanelsOnConnect);
            pnlOptions.Controls.Add(chkKeepTabsOpenAfterDisconnect);
            pnlOptions.Controls.Add(chkUseCustomConnectionTabColor);
            pnlOptions.Controls.Add(txtConnectionTabColor);
            pnlOptions.Controls.Add(btnSelectConnectionTabColor);
            pnlOptions.Controls.Add(chkUseCustomConnectionTabFont);
            pnlOptions.Controls.Add(txtConnectionTabFont);
            pnlOptions.Controls.Add(btnSelectConnectionTabFont);
            pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlOptions.Location = new System.Drawing.Point(0, 30);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new System.Drawing.Size(610, 531);
            pnlOptions.TabIndex = 10;
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
            lblRegistrySettingsUsedInfo.TabIndex = 11;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.Visible = false;
            // 
            // TabsPanelsPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(pnlOptions);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Name = "TabsPanelsPage";
            Size = new System.Drawing.Size(610, 561);
            pnlOptions.ResumeLayout(false);
            pnlOptions.PerformLayout();
            ResumeLayout(false);
        }

        internal MrngCheckBox chkAlwaysShowPanelTabs;
        internal MrngCheckBox chkAlwaysShowConnectionTabs;
        internal MrngCheckBox chkIdentifyQuickConnectTabs;
        internal MrngCheckBox chkOpenNewTabRightOfSelected;
        internal MrngCheckBox chkAlwaysShowPanelSelectionDlg;
        internal MrngCheckBox chkShowLogonInfoOnTabs;
        internal MrngCheckBox chkDoubleClickClosesTab;
        internal MrngCheckBox chkShowProtocolOnTabs;
        private MrngCheckBox chkCreateEmptyPanelOnStart;
        private MrngCheckBox chkBindConnectionsAndConfigPanels;
        internal MrngCheckBox chkShowFolderPathOnTabs;
        private Controls.MrngTextBox txtBoxPanelName;
        private Controls.MrngLabel lblPanelName;
        private System.Windows.Forms.Panel pnlOptions;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
        internal Controls.MrngNumericUpDown nudSplitterSize;
        internal Controls.MrngLabel lblSplitterSize;
        internal Controls.MrngNumericUpDown nudDockPadding;
        internal Controls.MrngLabel lblDockPadding;
        internal MrngCheckBox chkLockPanels;
        internal MrngCheckBox chkDoNotRestoreOnRdpMinimize;
        internal MrngCheckBox chkAutoClosePanelOnLastTabClose;
        private MrngCheckBox chkUseCustomConnectionTabColor;
        private Controls.MrngTextBox txtConnectionTabColor;
        private MrngButton btnSelectConnectionTabColor;
        private MrngCheckBox chkMinimizePanelsOnConnect;
        private MrngCheckBox chkKeepTabsOpenAfterDisconnect;
        private MrngCheckBox chkUseCustomConnectionTabFont;
        private Controls.MrngTextBox txtConnectionTabFont;
        private MrngButton btnSelectConnectionTabFont;
    }
}
