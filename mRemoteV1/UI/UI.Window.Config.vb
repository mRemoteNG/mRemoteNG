Imports WeifenLuo.WinFormsUI.Docking
Imports System.Net.NetworkInformation
Imports mRemote.App.Runtime

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
                Me.btnShowInheritance.Image = Global.mRemote.My.Resources.Resources.Inheritance
                Me.btnShowInheritance.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnShowInheritance.Name = "btnShowInheritance"
                Me.btnShowInheritance.Size = New System.Drawing.Size(23, 22)
                Me.btnShowInheritance.Text = "Inheritance"
                '
                'btnShowDefaultInheritance
                '
                Me.btnShowDefaultInheritance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnShowDefaultInheritance.Image = Global.mRemote.My.Resources.Resources.Inheritance_Default
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
                Me.btnShowProperties.Image = Global.mRemote.My.Resources.Resources.Properties
                Me.btnShowProperties.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.btnShowProperties.Name = "btnShowProperties"
                Me.btnShowProperties.Size = New System.Drawing.Size(23, 22)
                Me.btnShowProperties.Text = "Properties"
                '
                'btnShowDefaultProperties
                '
                Me.btnShowDefaultProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.btnShowDefaultProperties.Image = Global.mRemote.My.Resources.Resources.Properties_Default
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
                Me.btnHostStatus.Image = Global.mRemote.My.Resources.Resources.HostStatus_Check
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
                Me.HideOnClose = True
                Me.Icon = Global.mRemote.My.Resources.Resources.Config_Icon
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

            'Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
            '    Debug.Print("key: " & keyData.ToString)
            '    Debug.Print("msg: " & msg.Msg)
            '    Debug.Print("hwnd: " & msg.HWnd.ToString)
            '    Debug.Print("lparam: " & msg.LParam.ToString)
            '    Debug.Print("wparam: " & msg.WParam.ToString)
            '    Debug.Print("result: " & msg.Result.ToString)

            '    If keyData = Keys.Tab Then
            '        Dim curGridItemLabel As String = pGrid.SelectedGridItem.Label
            '        Dim gridItemIndex As Integer

            '        For gridItemIndex = 0 To pGrid.SelectedGridItem.Parent.GridItems.Count
            '            If pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex).Label = curGridItemLabel Then
            '                Exit For
            '            End If
            '        Next

            '        If pGrid.SelectedGridItem.Parent.GridItems.Count > gridItemIndex + 1 Then
            '            pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex + 1).Select()
            '        End If
            '    ElseIf keyData = (Keys.Tab Or Keys.Shift) Then
            '        Dim curGridItemLabel As String = pGrid.SelectedGridItem.Label
            '        Dim gridItemIndex As Integer

            '        For gridItemIndex = 0 To pGrid.SelectedGridItem.Parent.GridItems.Count
            '            If pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex).Label = curGridItemLabel Then
            '                Exit For
            '            End If
            '        Next

            '        If gridItemIndex - 1 >= 0 Then
            '            pGrid.SelectedGridItem.Parent.GridItems(gridItemIndex - 1).Select()
            '        End If
            '    End If

            '    Return MyBase.ProcessCmdKey(msg, keyData)
            'End Function

            Public Sub SetPropertyGridObject(ByVal Obj As Object)
                Try
                    Me.btnShowProperties.Enabled = False
                    Me.btnShowInheritance.Enabled = False
                    Me.btnShowDefaultProperties.Enabled = False
                    Me.btnShowDefaultInheritance.Enabled = False
                    Me.btnIcon.Enabled = False
                    Me.btnHostStatus.Enabled = False

                    Me.btnIcon.Image = Nothing

                    If TypeOf Obj Is mRemote.Connection.Info Then 'CONNECTION INFO
                        If TryCast(Obj, mRemote.Connection.Info).IsContainer = False Then 'NO CONTAINER
                            If Me.PropertiesVisible Then 'Properties selected
                                Me.pGrid.SelectedObject = Obj

                                Me.btnShowProperties.Enabled = True
                                If TryCast(Obj, mRemote.Connection.Info).Parent IsNot Nothing Then
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

                                If TryCast(Obj, mRemote.Connection.Info).IsDefault Then 'Is the default connection
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
                                Me.pGrid.SelectedObject = TryCast(Obj, mRemote.Connection.Info).Inherit

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
                        ElseIf TryCast(Obj, mRemote.Connection.Info).IsContainer Then 'CONTAINER
                            Me.pGrid.SelectedObject = Obj

                            Me.btnShowProperties.Enabled = True
                            If TryCast(TryCast(Obj, mRemote.Connection.Info).Parent, mRemote.Container.Info).Parent IsNot Nothing Then
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

                        Dim conIcon As Icon = mRemote.Connection.Icon.FromString(TryCast(Obj, mRemote.Connection.Info).Icon)
                        If conIcon IsNot Nothing Then
                            Me.btnIcon.Image = conIcon.ToBitmap
                        End If
                    ElseIf TypeOf Obj Is mRemote.Root.Info Then 'ROOT
                        Me.PropertiesVisible = True
                        Me.DefaultPropertiesVisible = False
                        Me.btnShowProperties.Enabled = True
                        Me.btnShowInheritance.Enabled = False
                        Me.btnShowDefaultProperties.Enabled = True
                        Me.btnShowDefaultInheritance.Enabled = True
                        Me.btnIcon.Enabled = False
                        Me.btnHostStatus.Enabled = False

                        Me.pGrid.SelectedObject = Obj
                    ElseIf TypeOf Obj Is mRemote.Connection.Info.Inheritance Then 'INHERITANCE
                        Me.pGrid.SelectedObject = Obj

                        If Me.InheritanceVisible Then
                            Me.InheritanceVisible = True
                            Me.btnShowProperties.Enabled = True
                            Me.btnShowInheritance.Enabled = True
                            Me.btnShowDefaultProperties.Enabled = False
                            Me.btnShowDefaultInheritance.Enabled = False
                            Me.btnIcon.Enabled = True
                            Me.btnHostStatus.Enabled = Not TryCast(TryCast(Obj, mRemote.Connection.Info.Inheritance).Parent, mRemote.Connection.Info).IsContainer

                            Me.InheritanceVisible = True

                            Dim conIcon As Icon = mRemote.Connection.Icon.FromString(TryCast(TryCast(Obj, mRemote.Connection.Info.Inheritance).Parent, mRemote.Connection.Info).Icon)
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
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SetPropertyGridObject (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
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

                    Dim tsDefault As ToolStrip = New ToolStrip

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
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Load (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
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
                    If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
                        Select Case e.ChangedItem.Label
                            Case My.Resources.strPropertyNameProtocol
                                TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).SetDefaultPort()
                            Case My.Resources.strPropertyNameName
                                App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Text = Me.pGrid.SelectedObject.Name
                            Case My.Resources.strPropertyNameIcon
                                Dim conIcon As Icon = mRemote.Connection.Icon.FromString(TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).Icon)
                                If conIcon IsNot Nothing Then
                                    Me.btnIcon.Image = conIcon.ToBitmap
                                End If
                            Case My.Resources.strPropertyNamePuttySession
                                mRemote.Connection.PuttySession.PuttySessions = mRemote.Connection.Protocol.PuttyBase.GetSessions()
                            Case My.Resources.strPropertyNameAddress
                                Me.SetHostStatus(Me.pGrid.SelectedObject)
                        End Select

                        If TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).IsDefault Then
                            App.Runtime.DefaultConnectionToSettings()
                        End If
                    End If

                    If TypeOf Me.pGrid.SelectedObject Is mRemote.Root.Info Then
                        Dim rInfo As mRemote.Root.Info = Me.pGrid.SelectedObject

                        Select Case e.ChangedItem.Label
                            Case Language.Base.PasswordProtect
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

                    If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info.Inheritance Then
                        If TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info.Inheritance).IsDefault Then
                            App.Runtime.DefaultInheritanceToSettings()
                        End If
                    End If

                    Me.ShowHideGridItems()
                    App.Runtime.SaveConnectionsBG()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "pGrid_PopertyValueChanged (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub ShowHideGridItems()
                Try
                    Dim strHide As String = ""

                    If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
                        Dim conI As mRemote.Connection.Info = pGrid.SelectedObject

                        Select Case conI.Protocol
                            Case mRemote.Connection.Protocol.Protocols.RDP
                                strHide &= "ExtApp;RenderingEngine;PuttySession;ICAEncryption;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;"
                                If conI.RDGatewayUsageMethod = mRemote.Connection.Protocol.RDP.RDGatewayUsageMethod.Never Then
                                    strHide &= "RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                                Else If conI.RDGatewayUseConnectionCredentials Then
                                    strHide &= "RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                                End If
                            Case mRemote.Connection.Protocol.Protocols.VNC
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;PuttySession;ICAEncryption;RDPAuthenticationLevel;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                                If conI.VNCAuthMode = mRemote.Connection.Protocol.VNC.AuthMode.AuthVNC Then
                                    strHide &= "Username;Domain;"
                                End If
                                If conI.VNCProxyType = mRemote.Connection.Protocol.VNC.ProxyType.ProxyNone Then
                                    strHide &= "VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyUsername;"
                                End If
                            Case mRemote.Connection.Protocol.Protocols.SSH1
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.SSH2
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.Telnet
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;Password;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;Username;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.Rlogin
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;Password;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;Username;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.RAW
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;Password;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;Username;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.HTTP
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;PuttySession;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.HTTPS
                                strHide &= "ExtApp;CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;ICAEncryption;RDPAuthenticationLevel;PuttySession;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.ICA
                                strHide &= "ExtApp;DisplayThemes;DisplayWallpaper;PuttySession;RDPAuthenticationLevel;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;UseConsoleSession;RenderingEngine;Port;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                            Case mRemote.Connection.Protocol.Protocols.IntApp
                                strHide &= "CacheBitmaps;Colors;DisplayThemes;DisplayWallpaper;Domain;PuttySession;ICAEncryption;RDPAuthenticationLevel;RedirectDiskDrives;RedirectKeys;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;Resolution;UseConsoleSession;RenderingEngine;VNCAuthMode;VNCColors;VNCCompression;VNCEncoding;VNCProxyIP;VNCProxyPassword;VNCProxyPort;VNCProxyType;VNCProxyUsername;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"
                        End Select

                        If conI.IsDefault = False Then
                            With conI.Inherit
                                If .CacheBitmaps Then
                                    strHide &= "CacheBitmaps;"
                                End If

                                If .Colors Then
                                    strHide &= "Colors;"
                                End If

                                If .Description Then
                                    strHide &= "Description;"
                                End If

                                If .DisplayThemes Then
                                    strHide &= "DisplayThemes;"
                                End If

                                If .DisplayWallpaper Then
                                    strHide &= "DisplayWallpaper;"
                                End If

                                If .Domain Then
                                    strHide &= "Domain;"
                                End If

                                If .Icon Then
                                    strHide &= "Icon;"
                                End If

                                If .Password Then
                                    strHide &= "Password;"
                                End If

                                If .Port Then
                                    strHide &= "Port;"
                                End If

                                If .Protocol Then
                                    strHide &= "Protocol;"
                                End If

                                If .PuttySession Then
                                    strHide &= "PuttySession;"
                                End If

                                If .RedirectDiskDrives Then
                                    strHide &= "RedirectDiskDrives;"
                                End If

                                If .RedirectKeys Then
                                    strHide &= "RedirectKeys;"
                                End If

                                If .RedirectPorts Then
                                    strHide &= "RedirectPorts;"
                                End If

                                If .RedirectPrinters Then
                                    strHide &= "RedirectPrinters;"
                                End If

                                If .RedirectSmartCards Then
                                    strHide &= "RedirectSmartCards;"
                                End If

                                If .RedirectSound Then
                                    strHide &= "RedirectSound;"
                                End If

                                If .Resolution Then
                                    strHide &= "Resolution;"
                                End If

                                If .UseConsoleSession Then
                                    strHide &= "UseConsoleSession;"
                                End If

                                If .RenderingEngine Then
                                    strHide &= "RenderingEngine;"
                                End If

                                If .ICAEncryption Then
                                    strHide &= "ICAEncryption;"
                                End If

                                If .RDPAuthenticationLevel Then
                                    strHide &= "RDPAuthenticationLevel;"
                                End If

                                If .Username Then
                                    strHide &= "Username;"
                                End If

                                If .Panel Then
                                    strHide &= "Panel;"
                                End If

                                If conI.IsContainer Then
                                    strHide &= "Hostname;"
                                End If

                                If .PreExtApp Then
                                    strHide &= "PreExtApp;"
                                End If

                                If .PostExtApp Then
                                    strHide &= "PostExtApp;"
                                End If

                                If .MacAddress Then
                                    strHide &= "MacAddress;"
                                End If

                                If .UserField Then
                                    strHide &= "UserField;"
                                End If

                                If .VNCAuthMode Then
                                    strHide &= "VNCAuthMode;"
                                End If

                                If .VNCColors Then
                                    strHide &= "VNCColors;"
                                End If

                                If .VNCCompression Then
                                    strHide &= "VNCCompression;"
                                End If

                                If .VNCEncoding Then
                                    strHide &= "VNCEncoding;"
                                End If

                                If .VNCProxyIP Then
                                    strHide &= "VNCProxyIP;"
                                End If

                                If .VNCProxyPassword Then
                                    strHide &= "VNCProxyPassword;"
                                End If

                                If .VNCProxyPort Then
                                    strHide &= "VNCProxyPort;"
                                End If

                                If .VNCProxyType Then
                                    strHide &= "VNCProxyType;"
                                End If

                                If .VNCProxyUsername Then
                                    strHide &= "VNCProxyUsername;"
                                End If

                                If .VNCViewOnly Then
                                    strHide &= "VNCViewOnly;"
                                End If

                                If .VNCSmartSizeMode Then
                                    strHide &= "VNCSmartSizeMode;"
                                End If

                                If .ExtApp Then
                                    strHide &= "ExtApp"
                                End If

                                If .RDGatewayHostname Then
                                    strHide &= "RDGatewayHostname"
                                End If
                            End With
                        Else
                            strHide = "Hostname;Name;"
                        End If
                    ElseIf TypeOf Me.pGrid.SelectedObject Is mRemote.Root.Info Then
                        strHide &= "TreeNode;"
                    Else
                        strHide = ";"
                    End If

                    Me.pGrid.HiddenProperties = strHide.Split(";")

                    Me.pGrid.Refresh()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "ShowHideGridItems (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub btnShowProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowProperties.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info.Inheritance Then
                    If TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info.Inheritance).IsDefault Then
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag, mRemote.Root.Info))
                    Else
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info.Inheritance).Parent)
                    End If
                ElseIf TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
                    If TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).IsDefault Then
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(App.Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag, mRemote.Root.Info))
                    End If
                End If
            End Sub

            Private Sub btnShowDefaultProperties_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDefaultProperties.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemote.Root.Info Or TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info.Inheritance Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = False
                    Me.DefaultPropertiesVisible = True
                    Me.DefaultInheritanceVisible = False
                    Me.SetPropertyGridObject(App.Runtime.DefaultConnectionFromSettings())
                End If
            End Sub

            Private Sub btnShowInheritance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowInheritance.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = True
                    Me.DefaultPropertiesVisible = False
                    Me.DefaultInheritanceVisible = False
                    Me.SetPropertyGridObject(TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).Inherit)
                End If
            End Sub

            Private Sub btnShowDefaultInheritance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDefaultInheritance.Click
                If TypeOf Me.pGrid.SelectedObject Is mRemote.Root.Info Or TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
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
                    If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
                        Me.cMenIcons.Items.Clear()

                        For Each iStr As String In mRemote.Connection.Icon.Icons
                            Dim tI As New ToolStripMenuItem
                            tI.Text = iStr
                            tI.Image = mRemote.Connection.Icon.FromString(iStr).ToBitmap
                            AddHandler tI.Click, AddressOf IconMenu_Click

                            Me.cMenIcons.Items.Add(tI)
                        Next

                        Dim mPos As New Point(PointToScreen(New Point(e.Location.X + Me.pGrid.Width - 100, e.Location.Y)))
                        Me.cMenIcons.Show(mPos)
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "btnIcon_Click (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub IconMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                Try
                    If TypeOf Me.pGrid.SelectedObject Is mRemote.Connection.Info Then
                        TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).Icon = TryCast(sender, ToolStripMenuItem).Text
                        Dim conIcon As Icon = mRemote.Connection.Icon.FromString(TryCast(Me.pGrid.SelectedObject, mRemote.Connection.Info).Icon)
                        If conIcon IsNot Nothing Then
                            Me.btnIcon.Image = conIcon.ToBitmap
                        End If

                        App.Runtime.SaveConnectionsBG()
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "IconMenu_Click (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
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
                    If TypeOf ConnectionInfo Is mRemote.Connection.Info Then
                        'continue
                    ElseIf TypeOf ConnectionInfo Is mRemote.Connection.Info.Inheritance Then
                        ConnectionInfo = TryCast(ConnectionInfo, mRemote.Connection.Info.Inheritance).Parent
                    Else
                        Me.btnHostStatus.Image = My.Resources.HostStatus_Check
                        Exit Sub
                    End If

                    If TryCast(ConnectionInfo, mRemote.Connection.Info).IsContainer Then
                        Me.btnHostStatus.Image = My.Resources.HostStatus_Check
                        Exit Sub
                    End If

                    Me.btnHostStatus.Image = My.Resources.HostStatus_Check
                    Me.btnHostStatus.Tag = "checking"
                    HostName = TryCast(ConnectionInfo, mRemote.Connection.Info).Hostname
                    pThread = New Threading.Thread(AddressOf CheckHostAlive)
                    pThread.IsBackground = True
                    pThread.Start()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SetHostStatus (UI.Window.Config) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

        End Class
    End Namespace
End Namespace