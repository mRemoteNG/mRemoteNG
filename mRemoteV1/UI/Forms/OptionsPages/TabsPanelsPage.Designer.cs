

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class TabsPanelsPage : OptionsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabsPanelsPage));
            this.chkAlwaysShowPanelTabs = new System.Windows.Forms.CheckBox();
            this.chkIdentifyQuickConnectTabs = new System.Windows.Forms.CheckBox();
            this.chkOpenNewTabRightOfSelected = new System.Windows.Forms.CheckBox();
            this.chkAlwaysShowPanelSelectionDlg = new System.Windows.Forms.CheckBox();
            this.chkShowLogonInfoOnTabs = new System.Windows.Forms.CheckBox();
            this.chkDoubleClickClosesTab = new System.Windows.Forms.CheckBox();
            this.chkShowProtocolOnTabs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkAlwaysShowPanelTabs
            // 
            this.chkAlwaysShowPanelTabs.AutoSize = true;
            this.chkAlwaysShowPanelTabs.Location = new System.Drawing.Point(3, 0);
            this.chkAlwaysShowPanelTabs.Name = "chkAlwaysShowPanelTabs";
            this.chkAlwaysShowPanelTabs.Size = new System.Drawing.Size(139, 17);
            this.chkAlwaysShowPanelTabs.TabIndex = 12;
            this.chkAlwaysShowPanelTabs.Text = "Always show panel tabs";
            this.chkAlwaysShowPanelTabs.UseVisualStyleBackColor = true;
            // 
            // chkIdentifyQuickConnectTabs
            // 
            this.chkIdentifyQuickConnectTabs.AutoSize = true;
            this.chkIdentifyQuickConnectTabs.Location = new System.Drawing.Point(3, 92);
            this.chkIdentifyQuickConnectTabs.Name = "chkIdentifyQuickConnectTabs";
            this.chkIdentifyQuickConnectTabs.Size = new System.Drawing.Size(293, 17);
            this.chkIdentifyQuickConnectTabs.TabIndex = 16;
            this.chkIdentifyQuickConnectTabs.Text = global::mRemoteNG.Language.strIdentifyQuickConnectTabs;
            this.chkIdentifyQuickConnectTabs.UseVisualStyleBackColor = true;
            // 
            // chkOpenNewTabRightOfSelected
            // 
            this.chkOpenNewTabRightOfSelected.AutoSize = true;
            this.chkOpenNewTabRightOfSelected.Location = new System.Drawing.Point(3, 23);
            this.chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected";
            this.chkOpenNewTabRightOfSelected.Size = new System.Drawing.Size(280, 17);
            this.chkOpenNewTabRightOfSelected.TabIndex = 13;
            this.chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab";
            this.chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = true;
            // 
            // chkAlwaysShowPanelSelectionDlg
            // 
            this.chkAlwaysShowPanelSelectionDlg.AutoSize = true;
            this.chkAlwaysShowPanelSelectionDlg.Location = new System.Drawing.Point(3, 138);
            this.chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg";
            this.chkAlwaysShowPanelSelectionDlg.Size = new System.Drawing.Size(317, 17);
            this.chkAlwaysShowPanelSelectionDlg.TabIndex = 18;
            this.chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins";
            this.chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = true;
            // 
            // chkShowLogonInfoOnTabs
            // 
            this.chkShowLogonInfoOnTabs.AutoSize = true;
            this.chkShowLogonInfoOnTabs.Location = new System.Drawing.Point(3, 46);
            this.chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs";
            this.chkShowLogonInfoOnTabs.Size = new System.Drawing.Size(203, 17);
            this.chkShowLogonInfoOnTabs.TabIndex = 14;
            this.chkShowLogonInfoOnTabs.Text = "Show logon information on tab names";
            this.chkShowLogonInfoOnTabs.UseVisualStyleBackColor = true;
            // 
            // chkDoubleClickClosesTab
            // 
            this.chkDoubleClickClosesTab.AutoSize = true;
            this.chkDoubleClickClosesTab.Location = new System.Drawing.Point(3, 115);
            this.chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab";
            this.chkDoubleClickClosesTab.Size = new System.Drawing.Size(159, 17);
            this.chkDoubleClickClosesTab.TabIndex = 17;
            this.chkDoubleClickClosesTab.Text = "Double click on tab closes it";
            this.chkDoubleClickClosesTab.UseVisualStyleBackColor = true;
            // 
            // chkShowProtocolOnTabs
            // 
            this.chkShowProtocolOnTabs.AutoSize = true;
            this.chkShowProtocolOnTabs.Location = new System.Drawing.Point(3, 69);
            this.chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs";
            this.chkShowProtocolOnTabs.Size = new System.Drawing.Size(166, 17);
            this.chkShowProtocolOnTabs.TabIndex = 15;
            this.chkShowProtocolOnTabs.Text = "Show protocols on tab names";
            this.chkShowProtocolOnTabs.UseVisualStyleBackColor = true;
            // 
            // TabsPanelsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkAlwaysShowPanelTabs);
            this.Controls.Add(this.chkIdentifyQuickConnectTabs);
            this.Controls.Add(this.chkOpenNewTabRightOfSelected);
            this.Controls.Add(this.chkAlwaysShowPanelSelectionDlg);
            this.Controls.Add(this.chkShowLogonInfoOnTabs);
            this.Controls.Add(this.chkDoubleClickClosesTab);
            this.Controls.Add(this.chkShowProtocolOnTabs);
            this.Name = "TabsPanelsPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal System.Windows.Forms.CheckBox chkAlwaysShowPanelTabs;
		internal System.Windows.Forms.CheckBox chkIdentifyQuickConnectTabs;
		internal System.Windows.Forms.CheckBox chkOpenNewTabRightOfSelected;
		internal System.Windows.Forms.CheckBox chkAlwaysShowPanelSelectionDlg;
		internal System.Windows.Forms.CheckBox chkShowLogonInfoOnTabs;
		internal System.Windows.Forms.CheckBox chkDoubleClickClosesTab;
		internal System.Windows.Forms.CheckBox chkShowProtocolOnTabs;
			
	}
}
