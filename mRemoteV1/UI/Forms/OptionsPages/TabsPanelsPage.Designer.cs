

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
			this.chkUseOnlyErrorsAndInfosPanel = new System.Windows.Forms.CheckBox();
			this.chkUseOnlyErrorsAndInfosPanel.CheckedChanged += new System.EventHandler(this.chkUseOnlyErrorsAndInfosPanel_CheckedChanged);
			this.chkOpenNewTabRightOfSelected = new System.Windows.Forms.CheckBox();
			this.lblSwitchToErrorsAndInfos = new System.Windows.Forms.Label();
			this.chkAlwaysShowPanelSelectionDlg = new System.Windows.Forms.CheckBox();
			this.chkMCInformation = new System.Windows.Forms.CheckBox();
			this.chkShowLogonInfoOnTabs = new System.Windows.Forms.CheckBox();
			this.chkMCErrors = new System.Windows.Forms.CheckBox();
			this.chkDoubleClickClosesTab = new System.Windows.Forms.CheckBox();
			this.chkMCWarnings = new System.Windows.Forms.CheckBox();
			this.chkShowProtocolOnTabs = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			//chkAlwaysShowPanelTabs
			//
			this.chkAlwaysShowPanelTabs.AutoSize = true;
			this.chkAlwaysShowPanelTabs.Location = new System.Drawing.Point(3, 0);
			this.chkAlwaysShowPanelTabs.Name = "chkAlwaysShowPanelTabs";
			this.chkAlwaysShowPanelTabs.Size = new System.Drawing.Size(139, 17);
			this.chkAlwaysShowPanelTabs.TabIndex = 12;
			this.chkAlwaysShowPanelTabs.Text = "Always show panel tabs";
			this.chkAlwaysShowPanelTabs.UseVisualStyleBackColor = true;
			//
			//chkIdentifyQuickConnectTabs
			//
			this.chkIdentifyQuickConnectTabs.AutoSize = true;
			this.chkIdentifyQuickConnectTabs.Location = new System.Drawing.Point(3, 92);
			this.chkIdentifyQuickConnectTabs.Name = "chkIdentifyQuickConnectTabs";
			this.chkIdentifyQuickConnectTabs.Size = new System.Drawing.Size(293, 17);
			this.chkIdentifyQuickConnectTabs.TabIndex = 16;
			this.chkIdentifyQuickConnectTabs.Text = Language.strIdentifyQuickConnectTabs;
			this.chkIdentifyQuickConnectTabs.UseVisualStyleBackColor = true;
			//
			//chkUseOnlyErrorsAndInfosPanel
			//
			this.chkUseOnlyErrorsAndInfosPanel.AutoSize = true;
			this.chkUseOnlyErrorsAndInfosPanel.Location = new System.Drawing.Point(3, 185);
			this.chkUseOnlyErrorsAndInfosPanel.Name = "chkUseOnlyErrorsAndInfosPanel";
			this.chkUseOnlyErrorsAndInfosPanel.Size = new System.Drawing.Size(278, 17);
			this.chkUseOnlyErrorsAndInfosPanel.TabIndex = 19;
			this.chkUseOnlyErrorsAndInfosPanel.Text = "Use only Notifications panel (no messagebox popups)";
			this.chkUseOnlyErrorsAndInfosPanel.UseVisualStyleBackColor = true;
			//
			//chkOpenNewTabRightOfSelected
			//
			this.chkOpenNewTabRightOfSelected.AutoSize = true;
			this.chkOpenNewTabRightOfSelected.Location = new System.Drawing.Point(3, 23);
			this.chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected";
			this.chkOpenNewTabRightOfSelected.Size = new System.Drawing.Size(280, 17);
			this.chkOpenNewTabRightOfSelected.TabIndex = 13;
			this.chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab";
			this.chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = true;
			//
			//lblSwitchToErrorsAndInfos
			//
			this.lblSwitchToErrorsAndInfos.AutoSize = true;
			this.lblSwitchToErrorsAndInfos.Location = new System.Drawing.Point(3, 210);
			this.lblSwitchToErrorsAndInfos.Name = "lblSwitchToErrorsAndInfos";
			this.lblSwitchToErrorsAndInfos.Size = new System.Drawing.Size(159, 13);
			this.lblSwitchToErrorsAndInfos.TabIndex = 20;
			this.lblSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:";
			//
			//chkAlwaysShowPanelSelectionDlg
			//
			this.chkAlwaysShowPanelSelectionDlg.AutoSize = true;
			this.chkAlwaysShowPanelSelectionDlg.Location = new System.Drawing.Point(3, 138);
			this.chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg";
			this.chkAlwaysShowPanelSelectionDlg.Size = new System.Drawing.Size(317, 17);
			this.chkAlwaysShowPanelSelectionDlg.TabIndex = 18;
			this.chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins";
			this.chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = true;
			//
			//chkMCInformation
			//
			this.chkMCInformation.AutoSize = true;
			this.chkMCInformation.Enabled = false;
			this.chkMCInformation.Location = new System.Drawing.Point(19, 230);
			this.chkMCInformation.Name = "chkMCInformation";
			this.chkMCInformation.Size = new System.Drawing.Size(83, 17);
			this.chkMCInformation.TabIndex = 21;
			this.chkMCInformation.Text = "Informations";
			this.chkMCInformation.UseVisualStyleBackColor = true;
			//
			//chkShowLogonInfoOnTabs
			//
			this.chkShowLogonInfoOnTabs.AutoSize = true;
			this.chkShowLogonInfoOnTabs.Location = new System.Drawing.Point(3, 46);
			this.chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs";
			this.chkShowLogonInfoOnTabs.Size = new System.Drawing.Size(203, 17);
			this.chkShowLogonInfoOnTabs.TabIndex = 14;
			this.chkShowLogonInfoOnTabs.Text = "Show logon information on tab names";
			this.chkShowLogonInfoOnTabs.UseVisualStyleBackColor = true;
			//
			//chkMCErrors
			//
			this.chkMCErrors.AutoSize = true;
			this.chkMCErrors.Enabled = false;
			this.chkMCErrors.Location = new System.Drawing.Point(19, 276);
			this.chkMCErrors.Name = "chkMCErrors";
			this.chkMCErrors.Size = new System.Drawing.Size(53, 17);
			this.chkMCErrors.TabIndex = 23;
			this.chkMCErrors.Text = "Errors";
			this.chkMCErrors.UseVisualStyleBackColor = true;
			//
			//chkDoubleClickClosesTab
			//
			this.chkDoubleClickClosesTab.AutoSize = true;
			this.chkDoubleClickClosesTab.Location = new System.Drawing.Point(3, 115);
			this.chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab";
			this.chkDoubleClickClosesTab.Size = new System.Drawing.Size(159, 17);
			this.chkDoubleClickClosesTab.TabIndex = 17;
			this.chkDoubleClickClosesTab.Text = "Double click on tab closes it";
			this.chkDoubleClickClosesTab.UseVisualStyleBackColor = true;
			//
			//chkMCWarnings
			//
			this.chkMCWarnings.AutoSize = true;
			this.chkMCWarnings.Enabled = false;
			this.chkMCWarnings.Location = new System.Drawing.Point(19, 253);
			this.chkMCWarnings.Name = "chkMCWarnings";
			this.chkMCWarnings.Size = new System.Drawing.Size(71, 17);
			this.chkMCWarnings.TabIndex = 22;
			this.chkMCWarnings.Text = "Warnings";
			this.chkMCWarnings.UseVisualStyleBackColor = true;
			//
			//chkShowProtocolOnTabs
			//
			this.chkShowProtocolOnTabs.AutoSize = true;
			this.chkShowProtocolOnTabs.Location = new System.Drawing.Point(3, 69);
			this.chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs";
			this.chkShowProtocolOnTabs.Size = new System.Drawing.Size(166, 17);
			this.chkShowProtocolOnTabs.TabIndex = 15;
			this.chkShowProtocolOnTabs.Text = "Show protocols on tab names";
			this.chkShowProtocolOnTabs.UseVisualStyleBackColor = true;
			//
			//TabsPanelsPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkAlwaysShowPanelTabs);
			this.Controls.Add(this.chkIdentifyQuickConnectTabs);
			this.Controls.Add(this.chkUseOnlyErrorsAndInfosPanel);
			this.Controls.Add(this.chkOpenNewTabRightOfSelected);
			this.Controls.Add(this.lblSwitchToErrorsAndInfos);
			this.Controls.Add(this.chkAlwaysShowPanelSelectionDlg);
			this.Controls.Add(this.chkMCInformation);
			this.Controls.Add(this.chkShowLogonInfoOnTabs);
			this.Controls.Add(this.chkMCErrors);
			this.Controls.Add(this.chkDoubleClickClosesTab);
			this.Controls.Add(this.chkMCWarnings);
			this.Controls.Add(this.chkShowProtocolOnTabs);
			this.Name = "TabsPanelsPage";
			this.PageIcon = (System.Drawing.Icon) (resources.GetObject("$this.PageIcon"));
			this.Size = new System.Drawing.Size(610, 489);
			this.ResumeLayout(false);
			this.PerformLayout();
				
		}
		internal System.Windows.Forms.CheckBox chkAlwaysShowPanelTabs;
		internal System.Windows.Forms.CheckBox chkIdentifyQuickConnectTabs;
		internal System.Windows.Forms.CheckBox chkUseOnlyErrorsAndInfosPanel;
		internal System.Windows.Forms.CheckBox chkOpenNewTabRightOfSelected;
		internal System.Windows.Forms.Label lblSwitchToErrorsAndInfos;
		internal System.Windows.Forms.CheckBox chkAlwaysShowPanelSelectionDlg;
		internal System.Windows.Forms.CheckBox chkMCInformation;
		internal System.Windows.Forms.CheckBox chkShowLogonInfoOnTabs;
		internal System.Windows.Forms.CheckBox chkMCErrors;
		internal System.Windows.Forms.CheckBox chkDoubleClickClosesTab;
		internal System.Windows.Forms.CheckBox chkMCWarnings;
		internal System.Windows.Forms.CheckBox chkShowProtocolOnTabs;
			
	}
}
