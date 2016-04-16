Namespace Forms.OptionsPages

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class TabsPanelsPage
        Inherits OptionsPage

        'UserControl overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TabsPanelsPage))
            Me.chkAlwaysShowPanelTabs = New System.Windows.Forms.CheckBox()
            Me.chkIdentifyQuickConnectTabs = New System.Windows.Forms.CheckBox()
            Me.chkUseOnlyErrorsAndInfosPanel = New System.Windows.Forms.CheckBox()
            Me.chkOpenNewTabRightOfSelected = New System.Windows.Forms.CheckBox()
            Me.lblSwitchToErrorsAndInfos = New System.Windows.Forms.Label()
            Me.chkAlwaysShowPanelSelectionDlg = New System.Windows.Forms.CheckBox()
            Me.chkMCInformation = New System.Windows.Forms.CheckBox()
            Me.chkShowLogonInfoOnTabs = New System.Windows.Forms.CheckBox()
            Me.chkMCErrors = New System.Windows.Forms.CheckBox()
            Me.chkDoubleClickClosesTab = New System.Windows.Forms.CheckBox()
            Me.chkMCWarnings = New System.Windows.Forms.CheckBox()
            Me.chkShowProtocolOnTabs = New System.Windows.Forms.CheckBox()
            Me.SuspendLayout()
            '
            'chkAlwaysShowPanelTabs
            '
            Me.chkAlwaysShowPanelTabs.AutoSize = True
            Me.chkAlwaysShowPanelTabs.Location = New System.Drawing.Point(3, 0)
            Me.chkAlwaysShowPanelTabs.Name = "chkAlwaysShowPanelTabs"
            Me.chkAlwaysShowPanelTabs.Size = New System.Drawing.Size(139, 17)
            Me.chkAlwaysShowPanelTabs.TabIndex = 12
            Me.chkAlwaysShowPanelTabs.Text = "Always show panel tabs"
            Me.chkAlwaysShowPanelTabs.UseVisualStyleBackColor = True
            '
            'chkIdentifyQuickConnectTabs
            '
            Me.chkIdentifyQuickConnectTabs.AutoSize = True
            Me.chkIdentifyQuickConnectTabs.Location = New System.Drawing.Point(3, 92)
            Me.chkIdentifyQuickConnectTabs.Name = "chkIdentifyQuickConnectTabs"
            Me.chkIdentifyQuickConnectTabs.Size = New System.Drawing.Size(293, 17)
            Me.chkIdentifyQuickConnectTabs.TabIndex = 16
            Me.chkIdentifyQuickConnectTabs.Text = Global.mRemote3G.Language.Language.strIdentifyQuickConnectTabs
            Me.chkIdentifyQuickConnectTabs.UseVisualStyleBackColor = True
            '
            'chkUseOnlyErrorsAndInfosPanel
            '
            Me.chkUseOnlyErrorsAndInfosPanel.AutoSize = True
            Me.chkUseOnlyErrorsAndInfosPanel.Location = New System.Drawing.Point(3, 185)
            Me.chkUseOnlyErrorsAndInfosPanel.Name = "chkUseOnlyErrorsAndInfosPanel"
            Me.chkUseOnlyErrorsAndInfosPanel.Size = New System.Drawing.Size(278, 17)
            Me.chkUseOnlyErrorsAndInfosPanel.TabIndex = 19
            Me.chkUseOnlyErrorsAndInfosPanel.Text = "Use only Notifications panel (no messagebox popups)"
            Me.chkUseOnlyErrorsAndInfosPanel.UseVisualStyleBackColor = True
            '
            'chkOpenNewTabRightOfSelected
            '
            Me.chkOpenNewTabRightOfSelected.AutoSize = True
            Me.chkOpenNewTabRightOfSelected.Location = New System.Drawing.Point(3, 23)
            Me.chkOpenNewTabRightOfSelected.Name = "chkOpenNewTabRightOfSelected"
            Me.chkOpenNewTabRightOfSelected.Size = New System.Drawing.Size(280, 17)
            Me.chkOpenNewTabRightOfSelected.TabIndex = 13
            Me.chkOpenNewTabRightOfSelected.Text = "Open new tab to the right of the currently selected tab"
            Me.chkOpenNewTabRightOfSelected.UseVisualStyleBackColor = True
            '
            'lblSwitchToErrorsAndInfos
            '
            Me.lblSwitchToErrorsAndInfos.AutoSize = True
            Me.lblSwitchToErrorsAndInfos.Location = New System.Drawing.Point(3, 210)
            Me.lblSwitchToErrorsAndInfos.Name = "lblSwitchToErrorsAndInfos"
            Me.lblSwitchToErrorsAndInfos.Size = New System.Drawing.Size(159, 13)
            Me.lblSwitchToErrorsAndInfos.TabIndex = 20
            Me.lblSwitchToErrorsAndInfos.Text = "Switch to Notifications panel on:"
            '
            'chkAlwaysShowPanelSelectionDlg
            '
            Me.chkAlwaysShowPanelSelectionDlg.AutoSize = True
            Me.chkAlwaysShowPanelSelectionDlg.Location = New System.Drawing.Point(3, 138)
            Me.chkAlwaysShowPanelSelectionDlg.Name = "chkAlwaysShowPanelSelectionDlg"
            Me.chkAlwaysShowPanelSelectionDlg.Size = New System.Drawing.Size(317, 17)
            Me.chkAlwaysShowPanelSelectionDlg.TabIndex = 18
            Me.chkAlwaysShowPanelSelectionDlg.Text = "Always show panel selection dialog when opening connectins"
            Me.chkAlwaysShowPanelSelectionDlg.UseVisualStyleBackColor = True
            '
            'chkMCInformation
            '
            Me.chkMCInformation.AutoSize = True
            Me.chkMCInformation.Enabled = False
            Me.chkMCInformation.Location = New System.Drawing.Point(19, 230)
            Me.chkMCInformation.Name = "chkMCInformation"
            Me.chkMCInformation.Size = New System.Drawing.Size(83, 17)
            Me.chkMCInformation.TabIndex = 21
            Me.chkMCInformation.Text = "Informations"
            Me.chkMCInformation.UseVisualStyleBackColor = True
            '
            'chkShowLogonInfoOnTabs
            '
            Me.chkShowLogonInfoOnTabs.AutoSize = True
            Me.chkShowLogonInfoOnTabs.Location = New System.Drawing.Point(3, 46)
            Me.chkShowLogonInfoOnTabs.Name = "chkShowLogonInfoOnTabs"
            Me.chkShowLogonInfoOnTabs.Size = New System.Drawing.Size(203, 17)
            Me.chkShowLogonInfoOnTabs.TabIndex = 14
            Me.chkShowLogonInfoOnTabs.Text = "Show logon information on tab names"
            Me.chkShowLogonInfoOnTabs.UseVisualStyleBackColor = True
            '
            'chkMCErrors
            '
            Me.chkMCErrors.AutoSize = True
            Me.chkMCErrors.Enabled = False
            Me.chkMCErrors.Location = New System.Drawing.Point(19, 276)
            Me.chkMCErrors.Name = "chkMCErrors"
            Me.chkMCErrors.Size = New System.Drawing.Size(53, 17)
            Me.chkMCErrors.TabIndex = 23
            Me.chkMCErrors.Text = "Errors"
            Me.chkMCErrors.UseVisualStyleBackColor = True
            '
            'chkDoubleClickClosesTab
            '
            Me.chkDoubleClickClosesTab.AutoSize = True
            Me.chkDoubleClickClosesTab.Location = New System.Drawing.Point(3, 115)
            Me.chkDoubleClickClosesTab.Name = "chkDoubleClickClosesTab"
            Me.chkDoubleClickClosesTab.Size = New System.Drawing.Size(159, 17)
            Me.chkDoubleClickClosesTab.TabIndex = 17
            Me.chkDoubleClickClosesTab.Text = "Double click on tab closes it"
            Me.chkDoubleClickClosesTab.UseVisualStyleBackColor = True
            '
            'chkMCWarnings
            '
            Me.chkMCWarnings.AutoSize = True
            Me.chkMCWarnings.Enabled = False
            Me.chkMCWarnings.Location = New System.Drawing.Point(19, 253)
            Me.chkMCWarnings.Name = "chkMCWarnings"
            Me.chkMCWarnings.Size = New System.Drawing.Size(71, 17)
            Me.chkMCWarnings.TabIndex = 22
            Me.chkMCWarnings.Text = "Warnings"
            Me.chkMCWarnings.UseVisualStyleBackColor = True
            '
            'chkShowProtocolOnTabs
            '
            Me.chkShowProtocolOnTabs.AutoSize = True
            Me.chkShowProtocolOnTabs.Location = New System.Drawing.Point(3, 69)
            Me.chkShowProtocolOnTabs.Name = "chkShowProtocolOnTabs"
            Me.chkShowProtocolOnTabs.Size = New System.Drawing.Size(166, 17)
            Me.chkShowProtocolOnTabs.TabIndex = 15
            Me.chkShowProtocolOnTabs.Text = "Show protocols on tab names"
            Me.chkShowProtocolOnTabs.UseVisualStyleBackColor = True
            '
            'TabsPanelsPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.chkAlwaysShowPanelTabs)
            Me.Controls.Add(Me.chkIdentifyQuickConnectTabs)
            Me.Controls.Add(Me.chkUseOnlyErrorsAndInfosPanel)
            Me.Controls.Add(Me.chkOpenNewTabRightOfSelected)
            Me.Controls.Add(Me.lblSwitchToErrorsAndInfos)
            Me.Controls.Add(Me.chkAlwaysShowPanelSelectionDlg)
            Me.Controls.Add(Me.chkMCInformation)
            Me.Controls.Add(Me.chkShowLogonInfoOnTabs)
            Me.Controls.Add(Me.chkMCErrors)
            Me.Controls.Add(Me.chkDoubleClickClosesTab)
            Me.Controls.Add(Me.chkMCWarnings)
            Me.Controls.Add(Me.chkShowProtocolOnTabs)
            Me.Name = "TabsPanelsPage"
            Me.PageIcon = CType(resources.GetObject("$this.PageIcon"), System.Drawing.Icon)
            Me.Size = New System.Drawing.Size(610, 489)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents chkAlwaysShowPanelTabs As System.Windows.Forms.CheckBox
        Friend WithEvents chkIdentifyQuickConnectTabs As System.Windows.Forms.CheckBox
        Friend WithEvents chkUseOnlyErrorsAndInfosPanel As System.Windows.Forms.CheckBox
        Friend WithEvents chkOpenNewTabRightOfSelected As System.Windows.Forms.CheckBox
        Friend WithEvents lblSwitchToErrorsAndInfos As System.Windows.Forms.Label
        Friend WithEvents chkAlwaysShowPanelSelectionDlg As System.Windows.Forms.CheckBox
        Friend WithEvents chkMCInformation As System.Windows.Forms.CheckBox
        Friend WithEvents chkShowLogonInfoOnTabs As System.Windows.Forms.CheckBox
        Friend WithEvents chkMCErrors As System.Windows.Forms.CheckBox
        Friend WithEvents chkDoubleClickClosesTab As System.Windows.Forms.CheckBox
        Friend WithEvents chkMCWarnings As System.Windows.Forms.CheckBox
        Friend WithEvents chkShowProtocolOnTabs As System.Windows.Forms.CheckBox

    End Class
End Namespace