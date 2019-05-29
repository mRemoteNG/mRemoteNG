

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public sealed partial class TabsPanelsPage : OptionsPage
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
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.chkAlwaysShowPanelTabs = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkAlwaysShowConnectionTabs = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkIdentifyQuickConnectTabs = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkOpenNewTabRightOfSelected = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkAlwaysShowPanelSelectionDlg = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkShowLogonInfoOnTabs = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkDoubleClickClosesTab = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkShowProtocolOnTabs = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkCreateEmptyPanelOnStart = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.txtBoxPanelName = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblPanelName = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.SuspendLayout();
            // 
            // chkAlwaysShowPanelTabs
            // 
            this.chkAlwaysShowPanelTabs._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkAlwaysShowPanelTabs.AutoSize = true;
            this.chkAlwaysShowPanelTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAlwaysShowPanelTabs.Location = new System.Drawing.Point(3, 3);
            this.chkAlwaysShowPanelTabs.Name = "chkAlwaysShowPanelTabs";
            this.chkAlwaysShowPanelTabs.Size = new System.Drawing.Size(149, 17);
            this.chkAlwaysShowPanelTabs.TabIndex = 0;
            this.chkAlwaysShowPanelTabs.Text = "Always show panel tabs";
            this.chkAlwaysShowPanelTabs.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysShowConnectionTabs
            // 
            this.chkAlwaysShowConnectionTabs._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkAlwaysShowConnectionTabs.AutoSize = true;
            this.chkAlwaysShowConnectionTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAlwaysShowConnectionTabs.Location = new System.Drawing.Point(3, 26);
            this.chkAlwaysShowConnectionTabs.Name = "chkAlwaysShowConnectionTabs";
            this.chkAlwaysShowConnectionTabs.Size = new System.Drawing.Size(178, 17);
            this.chkAlwaysShowConnectionTabs.TabIndex = 0;
            this.chkAlwaysShowConnectionTabs.Text = "Always show connection tabs";
            this.chkAlwaysShowConnectionTabs.UseVisualStyleBackColor = true;
            this.chkAlwaysShowConnectionTabs.Visible = false;
            // 
            // chkIdentifyQuickConnectTabs
            // 
            this.chkIdentifyQuickConnectTabs._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkIdentifyQuickConnectTabs.AutoSize = true;
            this.chkIdentifyQuickConnectTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIdentifyQuickConnectTabs.Location = new System.Drawing.Point(3, 118);
            this.chkIdentifyQuickConnectTabs.Name = "chkIdentifyQuickConnectTabs";
            this.chkIdentifyQuickConnectTabs.Size = new System.Drawing.Size(315, 17);
            this.chkIdentifyQuickConnectTabs.TabIndex = 4;
            this.chkIdentifyQuickConnectTabs.Text = global::mRemoteNG.Language.strIdentifyQuickConnectTabs;
            this.chkIdentifyQuickConnectTabs.UseVisualStyleBackColor = true;
            // 
            // chkOpenNewTabRightOfSelected
            // 
            this.chkOpenNewTabRightOfSelected._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkOpenNewTabRightOfSelected.AutoSize = true;
            this.chkOpenNewTabRightOfSelected.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOpenNewTabRightOfSelected.Location = new System.Drawing.Point(3, 49);
            this.chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected";
            this.chkOpenNewTabRightOfSelected.Size = new System.Drawing.Size(309, 17);
            this.chkOpenNewTabRightOfSelected.TabIndex = 1;
            this.chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab";
            this.chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysShowPanelSelectionDlg
            // 
            this.chkAlwaysShowPanelSelectionDlg._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkAlwaysShowPanelSelectionDlg.AutoSize = true;
            this.chkAlwaysShowPanelSelectionDlg.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAlwaysShowPanelSelectionDlg.Location = new System.Drawing.Point(3, 164);
            this.chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg";
            this.chkAlwaysShowPanelSelectionDlg.Size = new System.Drawing.Size(347, 17);
            this.chkAlwaysShowPanelSelectionDlg.TabIndex = 6;
            this.chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins";
            this.chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = true;
            // 
            // chkShowLogonInfoOnTabs
            // 
            this.chkShowLogonInfoOnTabs._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkShowLogonInfoOnTabs.AutoSize = true;
            this.chkShowLogonInfoOnTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowLogonInfoOnTabs.Location = new System.Drawing.Point(3, 72);
            this.chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs";
            this.chkShowLogonInfoOnTabs.Size = new System.Drawing.Size(226, 17);
            this.chkShowLogonInfoOnTabs.TabIndex = 2;
            this.chkShowLogonInfoOnTabs.Text = "Show logon information on tab names";
            this.chkShowLogonInfoOnTabs.UseVisualStyleBackColor = true;
            // 
            // chkDoubleClickClosesTab
            // 
            this.chkDoubleClickClosesTab._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkDoubleClickClosesTab.AutoSize = true;
            this.chkDoubleClickClosesTab.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDoubleClickClosesTab.Location = new System.Drawing.Point(3, 141);
            this.chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab";
            this.chkDoubleClickClosesTab.Size = new System.Drawing.Size(170, 17);
            this.chkDoubleClickClosesTab.TabIndex = 5;
            this.chkDoubleClickClosesTab.Text = "Double click on tab closes it";
            this.chkDoubleClickClosesTab.UseVisualStyleBackColor = true;
            // 
            // chkShowProtocolOnTabs
            // 
            this.chkShowProtocolOnTabs._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkShowProtocolOnTabs.AutoSize = true;
            this.chkShowProtocolOnTabs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowProtocolOnTabs.Location = new System.Drawing.Point(3, 95);
            this.chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs";
            this.chkShowProtocolOnTabs.Size = new System.Drawing.Size(180, 17);
            this.chkShowProtocolOnTabs.TabIndex = 3;
            this.chkShowProtocolOnTabs.Text = "Show protocols on tab names";
            this.chkShowProtocolOnTabs.UseVisualStyleBackColor = true;
            // 
            // chkCreateEmptyPanelOnStart
            // 
            this.chkCreateEmptyPanelOnStart._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkCreateEmptyPanelOnStart.AutoSize = true;
            this.chkCreateEmptyPanelOnStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCreateEmptyPanelOnStart.Location = new System.Drawing.Point(3, 187);
            this.chkCreateEmptyPanelOnStart.Name = "chkCreateEmptyPanelOnStart";
            this.chkCreateEmptyPanelOnStart.Size = new System.Drawing.Size(271, 17);
            this.chkCreateEmptyPanelOnStart.TabIndex = 7;
            this.chkCreateEmptyPanelOnStart.Text = "Create an empty panel when mRemoteNG starts";
            this.chkCreateEmptyPanelOnStart.UseVisualStyleBackColor = true;
            this.chkCreateEmptyPanelOnStart.CheckedChanged += new System.EventHandler(this.chkCreateEmptyPanelOnStart_CheckedChanged);
            // 
            // txtBoxPanelName
            // 
            this.txtBoxPanelName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPanelName.Location = new System.Drawing.Point(35, 223);
            this.txtBoxPanelName.Name = "txtBoxPanelName";
            this.txtBoxPanelName.Size = new System.Drawing.Size(213, 22);
            this.txtBoxPanelName.TabIndex = 8;
            // 
            // lblPanelName
            // 
            this.lblPanelName.AutoSize = true;
            this.lblPanelName.Location = new System.Drawing.Point(32, 207);
            this.lblPanelName.Name = "lblPanelName";
            this.lblPanelName.Size = new System.Drawing.Size(69, 13);
            this.lblPanelName.TabIndex = 9;
            this.lblPanelName.Text = "Panel name:";
            // 
            // TabsPanelsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblPanelName);
            this.Controls.Add(this.txtBoxPanelName);
            this.Controls.Add(this.chkCreateEmptyPanelOnStart);
            this.Controls.Add(this.chkAlwaysShowPanelTabs);
            this.Controls.Add(this.chkAlwaysShowConnectionTabs);
            this.Controls.Add(this.chkIdentifyQuickConnectTabs);
            this.Controls.Add(this.chkOpenNewTabRightOfSelected);
            this.Controls.Add(this.chkAlwaysShowPanelSelectionDlg);
            this.Controls.Add(this.chkShowLogonInfoOnTabs);
            this.Controls.Add(this.chkDoubleClickClosesTab);
            this.Controls.Add(this.chkShowProtocolOnTabs);
            this.Name = "TabsPanelsPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.Base.NGCheckBox chkAlwaysShowPanelTabs;
		internal Controls.Base.NGCheckBox chkAlwaysShowConnectionTabs;
		internal Controls.Base.NGCheckBox chkIdentifyQuickConnectTabs;
		internal Controls.Base.NGCheckBox chkOpenNewTabRightOfSelected;
		internal Controls.Base.NGCheckBox chkAlwaysShowPanelSelectionDlg;
		internal Controls.Base.NGCheckBox chkShowLogonInfoOnTabs;
		internal Controls.Base.NGCheckBox chkDoubleClickClosesTab;
		internal Controls.Base.NGCheckBox chkShowProtocolOnTabs;
        private Controls.Base.NGCheckBox chkCreateEmptyPanelOnStart;
        private Controls.Base.NGTextBox txtBoxPanelName;
        private Controls.Base.NGLabel lblPanelName;
    }
}
