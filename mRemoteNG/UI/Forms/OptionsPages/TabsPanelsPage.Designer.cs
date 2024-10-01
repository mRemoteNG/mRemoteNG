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
            chkAlwaysShowPanelTabs = new MrngCheckBox();
            chkAlwaysShowConnectionTabs = new MrngCheckBox();
            chkIdentifyQuickConnectTabs = new MrngCheckBox();
            chkOpenNewTabRightOfSelected = new MrngCheckBox();
            chkAlwaysShowPanelSelectionDlg = new MrngCheckBox();
            chkShowLogonInfoOnTabs = new MrngCheckBox();
            chkDoubleClickClosesTab = new MrngCheckBox();
            chkShowProtocolOnTabs = new MrngCheckBox();
            chkCreateEmptyPanelOnStart = new MrngCheckBox();
            txtBoxPanelName = new MrngTextBox();
            lblPanelName = new MrngLabel();
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
            chkAlwaysShowConnectionTabs.Visible = false;
            // 
            // chkIdentifyQuickConnectTabs
            // 
            chkIdentifyQuickConnectTabs._mice = MrngCheckBox.MouseState.OUT;
            chkIdentifyQuickConnectTabs.AutoSize = true;
            chkIdentifyQuickConnectTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkIdentifyQuickConnectTabs.Location = new System.Drawing.Point(3, 118);
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
            chkAlwaysShowPanelSelectionDlg.Location = new System.Drawing.Point(3, 164);
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
            chkShowLogonInfoOnTabs.Location = new System.Drawing.Point(3, 72);
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
            chkDoubleClickClosesTab.Location = new System.Drawing.Point(3, 141);
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
            chkShowProtocolOnTabs.Location = new System.Drawing.Point(3, 95);
            chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs";
            chkShowProtocolOnTabs.Size = new System.Drawing.Size(180, 17);
            chkShowProtocolOnTabs.TabIndex = 3;
            chkShowProtocolOnTabs.Text = "Show protocols on tab names";
            chkShowProtocolOnTabs.UseVisualStyleBackColor = true;
            // 
            // chkCreateEmptyPanelOnStart
            // 
            chkCreateEmptyPanelOnStart._mice = MrngCheckBox.MouseState.OUT;
            chkCreateEmptyPanelOnStart.AutoSize = true;
            chkCreateEmptyPanelOnStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkCreateEmptyPanelOnStart.Location = new System.Drawing.Point(3, 187);
            chkCreateEmptyPanelOnStart.Name = "chkCreateEmptyPanelOnStart";
            chkCreateEmptyPanelOnStart.Size = new System.Drawing.Size(271, 17);
            chkCreateEmptyPanelOnStart.TabIndex = 7;
            chkCreateEmptyPanelOnStart.Text = "Create an empty panel when mRemoteNG starts";
            chkCreateEmptyPanelOnStart.UseVisualStyleBackColor = true;
            chkCreateEmptyPanelOnStart.CheckedChanged += chkCreateEmptyPanelOnStart_CheckedChanged;
            // 
            // txtBoxPanelName
            // 
            txtBoxPanelName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtBoxPanelName.Location = new System.Drawing.Point(35, 223);
            txtBoxPanelName.Name = "txtBoxPanelName";
            txtBoxPanelName.Size = new System.Drawing.Size(213, 22);
            txtBoxPanelName.TabIndex = 8;
            // 
            // lblPanelName
            // 
            lblPanelName.AutoSize = true;
            lblPanelName.Location = new System.Drawing.Point(32, 207);
            lblPanelName.Name = "lblPanelName";
            lblPanelName.Size = new System.Drawing.Size(69, 13);
            lblPanelName.TabIndex = 9;
            lblPanelName.Text = "Panel name:";
            // 
            // pnlOptions
            // 
            pnlOptions.Controls.Add(chkAlwaysShowPanelTabs);
            pnlOptions.Controls.Add(lblPanelName);
            pnlOptions.Controls.Add(chkShowProtocolOnTabs);
            pnlOptions.Controls.Add(txtBoxPanelName);
            pnlOptions.Controls.Add(chkDoubleClickClosesTab);
            pnlOptions.Controls.Add(chkCreateEmptyPanelOnStart);
            pnlOptions.Controls.Add(chkShowLogonInfoOnTabs);
            pnlOptions.Controls.Add(chkAlwaysShowPanelSelectionDlg);
            pnlOptions.Controls.Add(chkAlwaysShowConnectionTabs);
            pnlOptions.Controls.Add(chkOpenNewTabRightOfSelected);
            pnlOptions.Controls.Add(chkIdentifyQuickConnectTabs);
            pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlOptions.Location = new System.Drawing.Point(0, 30);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new System.Drawing.Size(610, 262);
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
            Size = new System.Drawing.Size(610, 490);
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
        private Controls.MrngTextBox txtBoxPanelName;
        private Controls.MrngLabel lblPanelName;
        private System.Windows.Forms.Panel pnlOptions;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
    }
}
