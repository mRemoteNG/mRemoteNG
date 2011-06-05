Imports WeifenLuo.WinFormsUI.Docking
Imports System.Net.NetworkInformation
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Config
            Inherits UI.Window.Base


#Region "Form Init"
            Friend WithEvents btnShowProperties As System.Windows.Forms.ToolStripButton
            Friend WithEvents btnShowDefaultProperties As System.Windows.Forms.ToolStripButton
            Friend WithEvents btnShowInheritance As System.Windows.Forms.ToolStripButton
            Friend WithEvents btnShowDefaultInheritance As System.Windows.Forms.ToolStripButton
            Friend WithEvents btnIcon As System.Windows.Forms.ToolStripButton
            Friend WithEvents btnHostStatus As System.Windows.Forms.ToolStripButton
            Friend WithEvents cMenIcons As System.Windows.Forms.ContextMenuStrip
            Private components As System.ComponentModel.IContainer
            Friend WithEvents pGrid As Azuria.Common.Controls.FilteredPropertyGrid
            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Me.pGrid = New Azuria.Common.Controls.FilteredPropertyGrid
                Me.btnShowInheritance = New System.Windows.Forms.ToolStripButton
                Me.btnShowDefaultInheritance = New System.Windows.Forms.ToolStripButton
                Me.btnShowProperties = New System.Windows.Forms.ToolStripButton
                Me.btnShowDefaultProperties = New System.Windows.Forms.ToolStripButton
                Me.btnIcon = New System.Windows.Forms.ToolStripButton
                Me.btnHostStatus = New System.Windows.Forms.ToolStripButton
                Me.cMenIcons = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.SuspendLayout()
                '
                'pGrid
                '
                Me.pGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pGrid.BrowsableProperties = Nothing
                Me.pGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.pGrid.HiddenAttributes = Nothing
                Me.pGrid.HiddenProperties = Nothing
                Me.pGrid.Location = New System.Drawing.Point(0, 0)
                Me.pGrid.Name = "pGrid"
                Me.pGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized
                Me.pGrid.Size = New System.Drawing.Size(226, 530)
                Me.pGrid.TabIndex = 0
                Me.pGrid.UseCompatibleTextRendering = True
                '
                'btnShowInheritance
                '
                Me.btnShowInheritance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnShowInheritance.Image = Global.mRemoteNG.My.Resources.Resources.Inheritance
                Me.btnShowInheritance.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnShowInheritance.Name = "btnShowInheritance"
                Me.btnShowInheritance.Size = New System.Drawing.Size(23, 22)
                Me.btnShowInheritance.Text = "Inheritance"
                '
                'btnShowDefaultInheritance
                '
                Me.btnShowDefaultInheritance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnShowDefaultInheritance.Image = Global.mRemoteNG.My.Resources.Resources.Inheritance_Default
                Me.btnShowDefaultInheritance.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnShowDefaultInheritance.Name = "btnShowDefaultInheritance"
                Me.btnShowDefaultInheritance.Size = New System.Drawing.Size(23, 22)
                Me.btnShowDefaultInheritance.Text = "Default Inheritance"
                '
                'btnShowProperties
                '
                Me.btnShowProperties.Checked = True
                Me.btnShowProperties.CheckState = System.Windows.Forms.CheckState.Checked
                Me.btnShowProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnShowProperties.Image = Global.mRemoteNG.My.Resources.Resources.Properties
                Me.btnShowProperties.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnShowProperties.Name = "btnShowProperties"
                Me.btnShowProperties.Size = New System.Drawing.Size(23, 22)
                Me.btnShowProperties.Text = "Properties"
                '
                'btnShowDefaultProperties
                '
                Me.btnShowDefaultProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnShowDefaultProperties.Image = Global.mRemoteNG.My.Resources.Resources.Properties_Default
                Me.btnShowDefaultProperties.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnShowDefaultProperties.Name = "btnShowDefaultProperties"
                Me.btnShowDefaultProperties.Size = New System.Drawing.Size(23, 22)
                Me.btnShowDefaultProperties.Text = "Default Properties"
                '
                'btnIcon
                '
                Me.btnIcon.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
                Me.btnIcon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnIcon.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnIcon.Name = "btnIcon"
                Me.btnIcon.Size = New System.Drawing.Size(23, 22)
                Me.btnIcon.Text = "Icon"
                '
                'btnHostStatus
                '
                Me.btnHostStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
                Me.btnHostStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnHostStatus.Image = Global.mRemoteNG.My.Resources.Resources.HostStatus_Check
                Me.btnHostStatus.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnHostStatus.Name = "btnHostStatus"
                Me.btnHostStatus.Size = New System.Drawing.Size(23, 22)
                Me.btnHostStatus.Tag = "checking"
                Me.btnHostStatus.Text = "Status"
                '
                'cMenIcons
                '
                Me.cMenIcons.Name = "cMenIcons"
                Me.cMenIcons.Size = New System.Drawing.Size(61, 4)
                '
                'Config
                '
                Me.ClientSize = New System.Drawing.Size(226, 530)
                Me.Controls.Add(Me.pGrid)
                Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = Global.mRemoteNG.My.Resources.Resources.Config_Icon
                Me.Name = "Config"
                Me.TabText = "Config"
                Me.Text = "Config"
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Private Properties"
            Private ConfigLoading As Boolean = False
#End Region

#Region "Public Properties"
            Public Property PropertiesVisible() As Boolean
                Get
                    If Me.btnShowProperties.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    Me.btnShowProperties.Checked = value

                    If value = True Then
                        Me.btnShowInheritance.Checked = False
                        Me.btnShowDefaultInheritance.Checked = False
                        Me.btnShowDefaultProperties.Checked = False
                    End If
                End Set
            End Property

            Public Property InheritanceVisible() As Boolean
                Get
                    If Me.btnShowInheritance.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    Me.btnShowInheritance.Checked = value

                    If value = True Then
                        Me.btnShowProperties.Checked = False
                        Me.btnShowDefaultInheritance.Checked = False
                        Me.btnShowDefaultProperties.Checked = False
                    End If
                End Set
            End Property

            Public Property DefaultPropertiesVisible() As Boolean
                Get
                    If Me.btnShowDefaultProperties.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    Me.btnShowDefaultProperties.Checked = value

                    If value = True Then
                        Me.btnShowProperties.Checked = False
                        Me.btnShowDefaultInheritance.Checked = False
                        Me.btnShowInheritance.Checked = False
                    End If
                End Set
            End Property

            Public Property DefaultInheritanceVisible() As Boolean
                Get
                    If Me.btnShowDefaultInheritance.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set(ByVal value As Boolean)
                    Me.btnShowDefaultInheritance.Checked = value

                    If value = True Then
                        Me.btnShowProperties.Checked = False
                        Me.btnShowDefaultProperties.Checked = False
                        Me.btnShowInheritance.Checked = False
                    End If
                End Set
            End Property
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Config
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

            ' Main form handle command key events
            Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
#If Config = "Debug" Then
                Debug.Print("key: " & keyData.ToString)
                Debug.Print("msg: " & msg.Msg)
                Debug.Print("hwnd: " & msg.HWnd.ToString)
                Debug.Print("lparam: " & msg.LParam.ToString)
                Debug.Print("wparam: " & msg.WParam.ToString)
                Debug.Print("result: " & msg.Result.ToString)
#End If
                If keyData = Keys.Tab Then
                    Dim curGridItemLabel As String = pGrid.SelectedGridItem.Label
                    Dim gridItemIndex As Integer

                    For gridItemIndex = 0 To pGrid.SelectedGridItem.Parent.GridItems.Count
                        If pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex).Label = curGridItemLabel Then
                            Exit For
                        End If
                    Next

                    If pGrid.SelectedGridItem.Parent.GridItems.Count > gridItemIndex + 1 Then
                        pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex + 1).Select()
                    Else
                        pGrid.SelectedGridItem.Parent.GridItems(0).Select()
                    End If
                End If
                If keyData = (Keys.Tab Or Keys.Shift) Then
                    Dim curGridItemLabel As String = pGrid.SelectedGridItem.Label
                    Dim gridItemIndex As Integer

                    For gridItemIndex = 0 To pGrid.SelectedGridItem.Parent.GridItems.Count
                        If pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex).Label = curGridItemLabel Then
                            Exit For
                        End If
                    Next

                    If gridItemIndex - 1 >= 0 Then
                        pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex - 1).Select()
                    Else
                        pGrid.SelectedGridItem.Parent.GridItems(pGrid.SelectedGridItem.Parent.GridItems.Count - 1).Select()
                    End If
                End If

                Return MyBase.ProcessCmdKey(msg, keyData)
            End Function

            Public Sub SetPropertyGridObject(ByVal Obj As Object)
                Try
                    Me.btnShowProperties.Enabled = False
                    Me.btnShowInheritance.Enabled = False
                    Me.btnShowDefaultProperties.Enabled = False
                    Me.btnShowDefaultInheritance.Enabled = False
                    Me.btnIcon.Enabled = False
                    Me.btnHostStatus.Enabled = False

                    Me.btnIcon.Image = Nothing

                    If TypeOf Obj Is mRemoteNG.Connection.Info Then 'CONNECTION INFO
                        If TryCast(Obj, mRemoteNG.Connection.Info).IsContainer = False Then 'NO CONTAINER
                            If Me.PropertiesVisible Then 'Properties selected
                                Me.pGrid.SelectedObject = Obj

                                Me.btnShowProperties.Enabled = True
                                If TryCast(Obj, mRemoteNG.Connection.Info).Parent IsNot Nothing Then
                                    Me.btnShowInheritance.Enabled = True
                                Else
                                    Me.btnShowInheritance.Enabled = False
                                End If
                                Me.btnShowDefaultProperties.Enabled = False
                                Me.btnShowDefaultInheritance.Enabled = False
                                Me.btnIcon.Enabled = True
                                Me.btnHostStatus.Enabled = True
                            ElseIf Me.DefaultPropertiesVisible Then 'Defaults selected
                                Me.pGrid.SelectedObject = Obj

                                If TryCast(Obj, mRemoteNG.Connection.Info).IsDefault Then 'Is the default connection
                                    Me.btnShowProperties.Enabled = True
                                    Me.btnShowInheritance.Enabled = False
                                    Me.btnShowDefaultProperties.Enabled = True
                                    Me.btnShowDefaultInheritance.Enabled = True
                                    Me.btnIcon.Enabled = True
                                    Me.btnHostStatus.Enabled = False
                                Else 'is not the default connection
                                    Me.btnShowProperties.Enabled = True
                                    Me.btnShowInheritance.Enabled = True
                                    Me.btnShowDefaultProperties.Enabled = False
                                    Me.btnShowDefaultInheritance.Enabled = False
                                    Me.btnIcon.Enabled = True
                                    Me.btnHostStatus.Enabled = True

                                    Me.PropertiesVisible = True
                                End If
                            ElseIf Me.InheritanceVisible Then 'Inheritance selected
                                Me.pGrid.SelectedObject = TryCast(Obj, mRemoteNG.Connection.Info).Inherit

                                Me.btnShowProperties.Enabled = True
                                Me.btnShowInheritance.Enabled = True
                                Me.btnShowDefaultProperties.Enabled = False
                                Me.btnShowDefaultInheritance.Enabled = False
                                Me.btnIcon.Enabled = True
                                Me.btnHostStatus.Enabled = True
                            ElseIf Me.DefaultInheritanceVisible Then 'Default Inhertiance selected
                                Me.btnShowProperties.Enabled = True
                                Me.btnShowInheritance.Enabled = True
                                Me.btnShowDefaultProperties.Enabled = False
                                Me.btnShowDefaultInheritance.Enabled = False
                                Me.btnIcon.Enabled = True
                                Me.btnHostStatus.Enabled = True

                                Me.PropertiesVisible = True
                            End If
                        ElseIf TryCast(Obj, mRemoteNG.Connection.Info).IsContainer Then 'CONTAINER
                            Me.pGrid.SelectedObject = Obj

                            Me.btnShowProperties.Enabled = True
                            If TryCast(TryCast(Obj, mRemoteNG.Connection.Info).Parent, mRemoteNG.Container.Info).Parent IsNot Nothing Then
                                Me.btnShowInheritance.Enabled = True
                            Else
                                Me.btnShowInheritance.Enabled = False
                            End If
                            Me.btnShowDefaultProperties.Enabled = False
                            Me.btnShowDefaultInheritance.Enabled = False
                            Me.btnIcon.Enabled = True
                            Me.btnHostStatus.Enabled = False

                            Me.PropertiesVisible = True
                        End If

                        Dim conIcon As Icon = mRemoteNG.Connection.Icon.FromString(TryCast(Obj, mRemoteNG.Connection.Info).Icon)
                        If conIcon IsNot Nothing Then
                            Me.btnIcon.Image = conIcon.ToBitmap
                        End If
                    ElseIf TypeOf Obj Is mRemoteNG.Root.Info Then 'ROOT
                        Me.PropertiesVisible = True
                        Me.DefaultPropertiesVisible = False
                        Me.btnShowProperties.Enabled = True
                        Me.btnShowInheritance.Enabled = False
                        Me.btnShowDefaultProperties.Enabled = True
                        Me.btnShowDefaultInheritance.Enabled = True
                        Me.btnIcon.Enabled = False
                        Me.btnHostStatus.Enabled = False

                        Me.pGrid.SelectedObject = Obj
                    ElseIf TypeOf Obj Is mRemoteNG.Connection.Info.Inheritance Then 'INHERITANCE
                        Me.pGrid.SelectedObject = Obj

                        If Me.InheritanceVisible Then
                            Me.InheritanceVisible = True
                            Me.btnShowProperties.Enabled = True
                            Me.btnShowInheritance.Enabled = True
                            Me.btnShowDefaultProperties.Enabled = False
                            Me.btnShowDefaultInheritance.Enabled = False
                            Me.btnIcon.Enabled = True
                            Me.btnHostStatus.Enabled = Not TryCast(TryCast(Obj, mRemoteNG.Connection.Info.Inheritance).Parent, mRemoteNG.Connection.Info).IsContainer

                            Me.InheritanceVisible = True

                            Dim conIcon As Icon = mRemoteNG.Connection.Icon.FromString(TryCast(TryCast(Obj, mRemoteNG.Connection.Info.Inheritance).Parent, mRemoteNG.Connection.Info).Icon)
                            If conIcon IsNot Nothing Then
                                Me.btnIcon.Image = conIcon.ToBitmap
                            End If
                        ElseIf Me.DefaultInheritanceVisible Then
                            Me.btnShowProperties.Enabled = True
                            Me.btnShowInheritance.Enabled = False
                            Me.btnShowDefaultProperties.Enabled = True
                            Me.btnShowDefaultInheritance.Enabled = True
                            Me.btnIcon.Enabled = False
                            Me.btnHostStatus.Enabled = False

                            Me.DefaultInheritanceVisible = True
                        End If

                    End If

                    Me.ShowHideGridItems()
                    Me.SetHostStatus(Obj)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigPropertyGridObjectFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub pGrid_SelectedObjectChanged()
                Me.ShowHideGridItems()
            End Sub
#End Region

#Region "Private Methods"
            Private tsCustom As ToolStrip = Nothing

            Private Sub Config_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                Try
                    'Show PropertyGrid Toolbar buttons
                    tsCustom = New ToolStrip
                    tsCustom.Items.Add(btnShowProperties)
                    tsCustom.Items.Add(btnShowInheritance)
                    tsCustom.Items.Add(btnShowDefaultProperties)
                    tsCustom.Items.Add(btnShowDefaultInheritance)
                    tsCustom.Items.Add(btnHostStatus)
                    tsCustom.Items.Add(btnIcon)
                    tsCustom.Show()

                    Dim tsDefault As New ToolStrip

                    For Each ctrl As System.Windows.Forms.Control In pGrid.Controls
                        Dim tStrip As ToolStrip = TryCast(ctrl, ToolStrip)

                        If tStrip IsNot Nothing Then
                            tsDefault = tStrip
                            Exit For
                        End If
                    Next

                    tsDefault.AllowMerge = True
                    tsDefault.Items(tsDefault.Items.Count - 1).Visible = False
                    ToolStripManager.Merge(tsCustom, tsDefault)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigUiLoadFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ApplyLanguage()
                btnShowInheritance.Text = My.Resources.strButtonInheritance
                btnShowDefaultInheritance.Text = My.Resources.strButtonDefaultInheritance
                btnShowProperties.Text = My.Resources.strButtonProperties
                btnShowDefaultProperties.Text = My.Resources.strButtonDefaultProperties
                btnIcon.Text = My.Resources.strButtonIcon
                btnHostStatus.Text = My.Resources.strStatus
                Text = My.Resources.strMenuConfig
                TabText = My.Resources.strMenuConfig
            End Sub

            Private Sub pGrid_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles pGrid.PropertyValueChanged
                Try
                    If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                        Select Case e.ChangedItem.Label
                            Case My.Resources.strPropertyNameProtocol
                                TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).SetDefaultPort()
                            Case My.Resources.strPropertyNameName
                                App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Text = Me.pGrid.SelectedObject.Name
                            Case My.Resources.strPropertyNameIcon
                                Dim conIcon As Icon = mRemoteNG.Connection.Icon.FromString(TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).Icon)
                                If conIcon IsNot Nothing Then
                                    Me.btnIcon.Image = conIcon.ToBitmap
                                End If
                            Case My.Resources.strPropertyNamePuttySession
                                mRemoteNG.Connection.PuttySession.PuttySessions = mRemoteNG.Connection.Protocol.PuttyBase.GetSessions()
                            Case My.Resources.strPropertyNameAddress
                                Me.SetHostStatus(Me.pGrid.SelectedObject)
                        End Select

                        If TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).IsDefault Then
                            App.Runtime.DefaultConnectionToSettings()
                        End If
                    End If

                    If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Root.Info Then
                        Dim rInfo As mRemoteNG.Root.Info = Me.pGrid.SelectedObject

                        Select Case e.ChangedItem.Label
                            Case My.Resources.strPasswordProtect
                                If rInfo.Password = True Then
                                    Dim pw As String = Tools.Misc.PasswordDialog

                                    If pw <> "" Then
                                        rInfo.PasswordString = pw
                                    Else
                                        rInfo.Password = False
                                    End If
                                End If
                            Case My.Resources.strPropertyNameName
                                App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Text = Me.pGrid.SelectedObject.Name
                        End Select
                    End If

                    If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info.Inheritance Then
                        If TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info.Inheritance).IsDefault Then
                            App.Runtime.DefaultInheritanceToSettings()
                        End If
                    End If

                    Me.ShowHideGridItems()
                    App.Runtime.SaveConnectionsBG()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigPropertyGridValueFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ShowHideGridItems()
                Try
                    Dim strHide As New List(Of String)

                    If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                        Dim conI As mRemoteNG.Connection.Info = pGrid.SelectedObject

                        Select Case conI.Protocol
                            Case mRemoteNG.Connection.Protocol.Protocols.RDP
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("PuttySession")
                                strHide.Add("RenderingEngine")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                                If conI.RDGatewayUsageMethod = mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod.Never Then
                                    strHide.Add("RDGatewayDomain")
                                    strHide.Add("RDGatewayHostname")
                                    strHide.Add("RDGatewayPassword")
                                    strHide.Add("RDGatewayUseConnectionCredentials")
                                    strHide.Add("RDGatewayUsername")
                                ElseIf conI.RDGatewayUseConnectionCredentials Then
                                    strHide.Add("RDGatewayDomain")
                                    strHide.Add("RDGatewayPassword")
                                    strHide.Add("RDGatewayUsername")
                                End If
                            Case mRemoteNG.Connection.Protocol.Protocols.VNC
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                If conI.VNCAuthMode = mRemoteNG.Connection.Protocol.VNC.AuthMode.AuthVNC Then
                                    strHide.Add("Username;Domain")
                                End If
                                If conI.VNCProxyType = mRemoteNG.Connection.Protocol.VNC.ProxyType.ProxyNone Then
                                    strHide.Add("VNCProxyIP")
                                    strHide.Add("VNCProxyPassword")
                                    strHide.Add("VNCProxyPort")
                                    strHide.Add("VNCProxyUsername")
                                End If
                            Case mRemoteNG.Connection.Protocol.Protocols.SSH1
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.SSH2
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.Telnet
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("Password")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("Username")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.Rlogin
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("Password")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("Username")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.RAW
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("Password")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("Username")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.HTTP
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.HTTPS
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("ICAEncryption")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound;Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.ICA
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("ExtApp")
                                strHide.Add("Port")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                            Case mRemoteNG.Connection.Protocol.Protocols.IntApp
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ICAEncryption")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("VNCAuthMode")
                                strHide.Add("VNCColors")
                                strHide.Add("VNCCompression")
                                strHide.Add("VNCEncoding")
                                strHide.Add("VNCProxyIP")
                                strHide.Add("VNCProxyPassword")
                                strHide.Add("VNCProxyPort")
                                strHide.Add("VNCProxyType")
                                strHide.Add("VNCProxyUsername")
                                strHide.Add("VNCSmartSizeMode")
                                strHide.Add("VNCViewOnly")
                        End Select

                        If conI.IsDefault = False Then
                            With conI.Inherit
                                If .CacheBitmaps Then
                                    strHide.Add("CacheBitmaps")
                                End If

                                If .Colors Then
                                    strHide.Add("Colors")
                                End If

                                If .Description Then
                                    strHide.Add("Description")
                                End If

                                If .DisplayThemes Then
                                    strHide.Add("DisplayThemes")
                                End If

                                If .DisplayWallpaper Then
                                    strHide.Add("DisplayWallpaper")
                                End If

                                If .EnableFontSmoothing Then
                                    strHide.Add("EnableFontSmoothing")
                                End If

                                If .EnableDesktopComposition Then
                                    strHide.Add("EnableDesktopComposition")
                                End If

                                If .Domain Then
                                    strHide.Add("Domain")
                                End If

                                If .Icon Then
                                    strHide.Add("Icon")
                                End If

                                If .Password Then
                                    strHide.Add("Password")
                                End If

                                If .Port Then
                                    strHide.Add("Port")
                                End If

                                If .Protocol Then
                                    strHide.Add("Protocol")
                                End If

                                If .PuttySession Then
                                    strHide.Add("PuttySession")
                                End If

                                If .RedirectDiskDrives Then
                                    strHide.Add("RedirectDiskDrives")
                                End If

                                If .RedirectKeys Then
                                    strHide.Add("RedirectKeys")
                                End If

                                If .RedirectPorts Then
                                    strHide.Add("RedirectPorts")
                                End If

                                If .RedirectPrinters Then
                                    strHide.Add("RedirectPrinters")
                                End If

                                If .RedirectSmartCards Then
                                    strHide.Add("RedirectSmartCards")
                                End If

                                If .RedirectSound Then
                                    strHide.Add("RedirectSound")
                                End If

                                If .Resolution Then
                                    strHide.Add("Resolution")
                                End If

                                If .UseConsoleSession Then
                                    strHide.Add("UseConsoleSession")
                                End If

                                If .RenderingEngine Then
                                    strHide.Add("RenderingEngine")
                                End If

                                If .ICAEncryption Then
                                    strHide.Add("ICAEncryption")
                                End If

                                If .RDPAuthenticationLevel Then
                                    strHide.Add("RDPAuthenticationLevel")
                                End If

                                If .Username Then
                                    strHide.Add("Username")
                                End If

                                If .Panel Then
                                    strHide.Add("Panel")
                                End If

                                If conI.IsContainer Then
                                    strHide.Add("Hostname")
                                End If

                                If .PreExtApp Then
                                    strHide.Add("PreExtApp")
                                End If

                                If .PostExtApp Then
                                    strHide.Add("PostExtApp")
                                End If

                                If .MacAddress Then
                                    strHide.Add("MacAddress")
                                End If

                                If .UserField Then
                                    strHide.Add("UserField")
                                End If

                                If .VNCAuthMode Then
                                    strHide.Add("VNCAuthMode")
                                End If

                                If .VNCColors Then
                                    strHide.Add("VNCColors")
                                End If

                                If .VNCCompression Then
                                    strHide.Add("VNCCompression")
                                End If

                                If .VNCEncoding Then
                                    strHide.Add("VNCEncoding")
                                End If

                                If .VNCProxyIP Then
                                    strHide.Add("VNCProxyIP")
                                End If

                                If .VNCProxyPassword Then
                                    strHide.Add("VNCProxyPassword")
                                End If

                                If .VNCProxyPort Then
                                    strHide.Add("VNCProxyPort")
                                End If

                                If .VNCProxyType Then
                                    strHide.Add("VNCProxyType")
                                End If

                                If .VNCProxyUsername Then
                                    strHide.Add("VNCProxyUsername")
                                End If

                                If .VNCViewOnly Then
                                    strHide.Add("VNCViewOnly")
                                End If

                                If .VNCSmartSizeMode Then
                                    strHide.Add("VNCSmartSizeMode")
                                End If

                                If .ExtApp Then
                                    strHide.Add("ExtApp")
                                End If

                                If .RDGatewayUsageMethod Then
                                    strHide.Add("RDGatewayUsageMethod")
                                End If

                                If .RDGatewayHostname Then
                                    strHide.Add("RDGatewayHostname")
                                End If

                                If .RDGatewayUsername Then
                                    strHide.Add("RDGatewayUsername")
                                End If

                                If .RDGatewayPassword Then
                                    strHide.Add("RDGatewayPassword")
                                End If

                                If .RDGatewayDomain Then
                                    strHide.Add("RDGatewayDomain")
                                End If

                                If .RDGatewayUseConnectionCredentials Then
                                    strHide.Add("RDGatewayUseConnectionCredentials")
                                End If

                                If .RDGatewayHostname Then
                                    strHide.Add("RDGatewayHostname")
                                End If
                            End With
                        Else
                            strHide.Add("Hostname")
                            strHide.Add("Name")
                        End If
                    ElseIf TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Root.Info Then
                        strHide.Add("TreeNode")
                    End If

                    Me.pGrid.HiddenProperties = strHide.ToArray

                    Me.pGrid.Refresh()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigPropertyGridHideItemsFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub btnShowProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowProperties.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info.Inheritance Then
                    If TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info.Inheritance).IsDefault Then
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag, mRemoteNG.Root.Info))
                    Else
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info.Inheritance).Parent)
                    End If
                ElseIf TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                    If TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).IsDefault Then
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag, mRemoteNG.Root.Info))
                    End If
                End If
            End Sub

            Private Sub btnShowDefaultProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDefaultProperties.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Root.Info Or TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info.Inheritance Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = False
                    Me.DefaultPropertiesVisible = True
                    Me.DefaultInheritanceVisible = False
                    Me.SetPropertyGridObject(App.Runtime.DefaultConnectionFromSettings())
                End If
            End Sub

            Private Sub btnShowInheritance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowInheritance.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = True
                    Me.DefaultPropertiesVisible = False
                    Me.DefaultInheritanceVisible = False
                    Me.SetPropertyGridObject(TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).Inherit)
                End If
            End Sub

            Private Sub btnShowDefaultInheritance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDefaultInheritance.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Root.Info Or TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = False
                    Me.DefaultPropertiesVisible = False
                    Me.DefaultInheritanceVisible = True
                    Me.SetPropertyGridObject(App.Runtime.DefaultInheritanceFromSettings())
                End If
            End Sub

            Private Sub btnHostStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHostStatus.Click
                SetHostStatus(Me.pGrid.SelectedObject)
            End Sub

            Private Sub btnIcon_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnIcon.MouseUp
                Try
                    If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                        Me.cMenIcons.Items.Clear()

                        For Each iStr As String In mRemoteNG.Connection.Icon.Icons
                            Dim tI As New ToolStripMenuItem
                            tI.Text = iStr
                            tI.Image = mRemoteNG.Connection.Icon.FromString(iStr).ToBitmap
                            AddHandler tI.Click, AddressOf IconMenu_Click

                            Me.cMenIcons.Items.Add(tI)
                        Next

                        Dim mPos As New Point(PointToScreen(New Point(e.Location.X + Me.pGrid.Width - 100, e.Location.Y)))
                        Me.cMenIcons.Show(mPos)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigPropertyGridButtonIconClickFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub IconMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                Try
                    If TypeOf Me.pGrid.SelectedObject Is mRemoteNG.Connection.Info Then
                        TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).Icon = TryCast(sender, ToolStripMenuItem).Text
                        Dim conIcon As Icon = mRemoteNG.Connection.Icon.FromString(TryCast(Me.pGrid.SelectedObject, mRemoteNG.Connection.Info).Icon)
                        If conIcon IsNot Nothing Then
                            Me.btnIcon.Image = conIcon.ToBitmap
                        End If

                        App.Runtime.SaveConnectionsBG()
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigPropertyGridMenuClickFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Host Status (Ping)"
            Private HostName As String
            Private pThread As Threading.Thread

            Private Sub CheckHostAlive()
                Dim pingSender As New Ping
                Dim pReply As PingReply

                Try
                    pReply = pingSender.Send(HostName)

                    If pReply.Status = IPStatus.Success Then
                        If Me.btnHostStatus.Tag = "checking" Then
                            ShowStatusImage(My.Resources.HostStatus_On)
                        End If
                    Else
                        If Me.btnHostStatus.Tag = "checking" Then
                            ShowStatusImage(My.Resources.HostStatus_Off)
                        End If
                    End If
                Catch ex As Exception
                    If Me.btnHostStatus.Tag = "checking" Then
                        ShowStatusImage(My.Resources.HostStatus_Off)
                    End If
                End Try
            End Sub

            Delegate Sub ShowStatusImageCB(ByVal [Image] As Image)
            Private Sub ShowStatusImage(ByVal [Image] As Image)
                If Me.pGrid.InvokeRequired Then
                    Dim d As New ShowStatusImageCB(AddressOf ShowStatusImage)
                    Me.pGrid.Invoke(d, New Object() {[Image]})
                Else
                    Me.btnHostStatus.Image = Image
                    Me.btnHostStatus.Tag = "checkfinished"
                End If
            End Sub

            Public Sub SetHostStatus(ByVal ConnectionInfo As Object)
                Try
                    Me.btnHostStatus.Image = My.Resources.HostStatus_Check

                    ' To check status, ConnectionInfo must be an mRemoteNG.Connection.Info that is not a container
                    If TypeOf ConnectionInfo Is mRemoteNG.Connection.Info Then
                        If TryCast(ConnectionInfo, mRemoteNG.Connection.Info).IsContainer Then Return
                    Else
                        Return
                    End If

                    Me.btnHostStatus.Tag = "checking"
                    HostName = TryCast(ConnectionInfo, mRemoteNG.Connection.Info).Hostname
                    pThread = New Threading.Thread(AddressOf CheckHostAlive)
                    pThread.SetApartmentState(Threading.ApartmentState.STA)
                    pThread.IsBackground = True
                    pThread.Start()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strConfigPropertyGridSetHostStatusFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

        End Class
    End Namespace
End Namespace