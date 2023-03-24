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
            SuspendLayout();
            // 
            // chkAlwaysShowPanelTabs
            // 
            chkAlwaysShowPanelTabs._mice = MrngCheckBox.MouseState.OUT;
            chkAlwaysShowPanelTabs.AutoSize = true;
            chkAlwaysShowPanelTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkAlwaysShowPanelTabs.Location = new System.Drawing.Point(9, 8);
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
            chkAlwaysShowConnectionTabs.Location = new System.Drawing.Point(9, 31);
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
            chkIdentifyQuickConnectTabs.Location = new System.Drawing.Point(9, 123);
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
            chkOpenNewTabRightOfSelected.Location = new System.Drawing.Point(9, 54);
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
            chkAlwaysShowPanelSelectionDlg.Location = new System.Drawing.Point(9, 169);
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
            chkShowLogonInfoOnTabs.Location = new System.Drawing.Point(9, 77);
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
            chkDoubleClickClosesTab.Location = new System.Drawing.Point(9, 146);
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
            chkShowProtocolOnTabs.Location = new System.Drawing.Point(9, 100);
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
            chkCreateEmptyPanelOnStart.Location = new System.Drawing.Point(9, 192);
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
            txtBoxPanelName.Location = new System.Drawing.Point(41, 228);
            txtBoxPanelName.Name = "txtBoxPanelName";
            txtBoxPanelName.Size = new System.Drawing.Size(213, 22);
            txtBoxPanelName.TabIndex = 8;
            // 
            // lblPanelName
            // 
            lblPanelName.AutoSize = true;
            lblPanelName.Location = new System.Drawing.Point(38, 212);
            lblPanelName.Name = "lblPanelName";
            lblPanelName.Size = new System.Drawing.Size(69, 13);
            lblPanelName.TabIndex = 9;
            lblPanelName.Text = "Panel name:";
            // 
            // TabsPanelsPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(lblPanelName);
            Controls.Add(txtBoxPanelName);
            Controls.Add(chkCreateEmptyPanelOnStart);
            Controls.Add(chkAlwaysShowPanelTabs);
            Controls.Add(chkAlwaysShowConnectionTabs);
            Controls.Add(chkIdentifyQuickConnectTabs);
            Controls.Add(chkOpenNewTabRightOfSelected);
            Controls.Add(chkAlwaysShowPanelSelectionDlg);
            Controls.Add(chkShowLogonInfoOnTabs);
            Controls.Add(chkDoubleClickClosesTab);
            Controls.Add(chkShowProtocolOnTabs);
            Name = "TabsPanelsPage";
            Size = new System.Drawing.Size(610, 490);
            ResumeLayout(false);
            PerformLayout();
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
    }
}
