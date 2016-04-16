Imports System.ComponentModel
Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Threading
Imports Azuria.Common.Controls
Imports mRemote3G.App
Imports mRemote3G.Connection
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Connection.PuttySession
Imports mRemote3G.Messages
Imports mRemote3G.My
Imports mRemote3G.My.Resources
Imports mRemote3G.Themes
Imports mRemote3G.Tools
Imports WeifenLuo.WinFormsUI.Docking

Namespace UI

    Namespace Window
        Public Class Config
            Inherits Base

#Region "Form Init"

            Friend WithEvents btnShowProperties As ToolStripButton
            Friend WithEvents btnShowDefaultProperties As ToolStripButton
            Friend WithEvents btnShowInheritance As ToolStripButton
            Friend WithEvents btnShowDefaultInheritance As ToolStripButton
            Friend WithEvents btnIcon As ToolStripButton
            Friend WithEvents btnHostStatus As ToolStripButton
            Friend WithEvents cMenIcons As ContextMenuStrip
            Private components As IContainer
            Friend WithEvents propertyGridContextMenu As ContextMenuStrip
            Friend WithEvents propertyGridContextMenuShowHelpText As ToolStripMenuItem
            Friend WithEvents propertyGridContextMenuReset As ToolStripMenuItem
            Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
            Friend WithEvents pGrid As FilteredPropertyGrid

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container()
                Me.pGrid = New FilteredPropertyGrid()
                Me.propertyGridContextMenu = New ContextMenuStrip(Me.components)
                Me.propertyGridContextMenuReset = New ToolStripMenuItem()
                Me.ToolStripSeparator1 = New ToolStripSeparator()
                Me.propertyGridContextMenuShowHelpText = New ToolStripMenuItem()
                Me.btnShowInheritance = New ToolStripButton()
                Me.btnShowDefaultInheritance = New ToolStripButton()
                Me.btnShowProperties = New ToolStripButton()
                Me.btnShowDefaultProperties = New ToolStripButton()
                Me.btnIcon = New ToolStripButton()
                Me.btnHostStatus = New ToolStripButton()
                Me.cMenIcons = New ContextMenuStrip(Me.components)
                Me.propertyGridContextMenu.SuspendLayout()
                Me.SuspendLayout()
                '
                'pGrid
                '
                Me.pGrid.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                          Or AnchorStyles.Left) _
                                         Or AnchorStyles.Right),
                                        AnchorStyles)
                Me.pGrid.BrowsableProperties = Nothing
                Me.pGrid.ContextMenuStrip = Me.propertyGridContextMenu
                Me.pGrid.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point,
                                         CType(0, Byte))
                Me.pGrid.HiddenAttributes = Nothing
                Me.pGrid.HiddenProperties = Nothing
                Me.pGrid.Location = New Point(0, 0)
                Me.pGrid.Name = "pGrid"
                Me.pGrid.PropertySort = PropertySort.Categorized
                Me.pGrid.Size = New Size(226, 530)
                Me.pGrid.TabIndex = 0
                Me.pGrid.UseCompatibleTextRendering = True
                '
                'propertyGridContextMenu
                '
                Me.propertyGridContextMenu.Items.AddRange(New ToolStripItem() _
                                                             {Me.propertyGridContextMenuReset, Me.ToolStripSeparator1,
                                                              Me.propertyGridContextMenuShowHelpText})
                Me.propertyGridContextMenu.Name = "propertyGridContextMenu"
                Me.propertyGridContextMenu.Size = New Size(157, 76)
                '
                'propertyGridContextMenuReset
                '
                Me.propertyGridContextMenuReset.Name = "propertyGridContextMenuReset"
                Me.propertyGridContextMenuReset.Size = New Size(156, 22)
                Me.propertyGridContextMenuReset.Text = "&Reset"
                '
                'ToolStripSeparator1
                '
                Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
                Me.ToolStripSeparator1.Size = New Size(153, 6)
                '
                'propertyGridContextMenuShowHelpText
                '
                Me.propertyGridContextMenuShowHelpText.Name = "propertyGridContextMenuShowHelpText"
                Me.propertyGridContextMenuShowHelpText.Size = New Size(156, 22)
                Me.propertyGridContextMenuShowHelpText.Text = "&Show Help Text"
                '
                'btnShowInheritance
                '
                Me.btnShowInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image
                Me.btnShowInheritance.Image = Inheritance
                Me.btnShowInheritance.ImageTransparentColor = Color.Magenta
                Me.btnShowInheritance.Name = "btnShowInheritance"
                Me.btnShowInheritance.Size = New Size(23, 22)
                Me.btnShowInheritance.Text = "Inheritance"
                '
                'btnShowDefaultInheritance
                '
                Me.btnShowDefaultInheritance.DisplayStyle = ToolStripItemDisplayStyle.Image
                Me.btnShowDefaultInheritance.Image = Inheritance_Default
                Me.btnShowDefaultInheritance.ImageTransparentColor = Color.Magenta
                Me.btnShowDefaultInheritance.Name = "btnShowDefaultInheritance"
                Me.btnShowDefaultInheritance.Size = New Size(23, 22)
                Me.btnShowDefaultInheritance.Text = "Default Inheritance"
                '
                'btnShowProperties
                '
                Me.btnShowProperties.Checked = True
                Me.btnShowProperties.CheckState = CheckState.Checked
                Me.btnShowProperties.DisplayStyle = ToolStripItemDisplayStyle.Image
                Me.btnShowProperties.Image = Properties
                Me.btnShowProperties.ImageTransparentColor = Color.Magenta
                Me.btnShowProperties.Name = "btnShowProperties"
                Me.btnShowProperties.Size = New Size(23, 22)
                Me.btnShowProperties.Text = "Properties"
                '
                'btnShowDefaultProperties
                '
                Me.btnShowDefaultProperties.DisplayStyle = ToolStripItemDisplayStyle.Image
                Me.btnShowDefaultProperties.Image = Properties_Default
                Me.btnShowDefaultProperties.ImageTransparentColor = Color.Magenta
                Me.btnShowDefaultProperties.Name = "btnShowDefaultProperties"
                Me.btnShowDefaultProperties.Size = New Size(23, 22)
                Me.btnShowDefaultProperties.Text = "Default Properties"
                '
                'btnIcon
                '
                Me.btnIcon.Alignment = ToolStripItemAlignment.Right
                Me.btnIcon.DisplayStyle = ToolStripItemDisplayStyle.Image
                Me.btnIcon.ImageTransparentColor = Color.Magenta
                Me.btnIcon.Name = "btnIcon"
                Me.btnIcon.Size = New Size(23, 22)
                Me.btnIcon.Text = "Icon"
                '
                'btnHostStatus
                '
                Me.btnHostStatus.Alignment = ToolStripItemAlignment.Right
                Me.btnHostStatus.DisplayStyle = ToolStripItemDisplayStyle.Image
                Me.btnHostStatus.Image = HostStatus_Check
                Me.btnHostStatus.ImageTransparentColor = Color.Magenta
                Me.btnHostStatus.Name = "btnHostStatus"
                Me.btnHostStatus.Size = New Size(23, 22)
                Me.btnHostStatus.Tag = "checking"
                Me.btnHostStatus.Text = "Status"
                '
                'cMenIcons
                '
                Me.cMenIcons.Name = "cMenIcons"
                Me.cMenIcons.Size = New Size(61, 4)
                '
                'Config
                '
                Me.ClientSize = New Size(226, 530)
                Me.Controls.Add(Me.pGrid)
                Me.Font = New Font("Microsoft Sans Serif", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType(0, Byte))
                Me.HideOnClose = True
                Me.Icon = Config_Icon
                Me.Name = "Config"
                Me.TabText = "Config"
                Me.Text = "Config"
                Me.propertyGridContextMenu.ResumeLayout(False)
                Me.ResumeLayout(False)
            End Sub

#End Region

#Region "Private Properties"

            Private ConfigLoading As Boolean = False

#End Region

#Region "Public Properties"

            Public Property PropertiesVisible As Boolean
                Get
                    If Me.btnShowProperties.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set
                    Me.btnShowProperties.Checked = value

                    If value = True Then
                        Me.btnShowInheritance.Checked = False
                        Me.btnShowDefaultInheritance.Checked = False
                        Me.btnShowDefaultProperties.Checked = False
                    End If
                End Set
            End Property

            Public Property InheritanceVisible As Boolean
                Get
                    If Me.btnShowInheritance.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set
                    Me.btnShowInheritance.Checked = value

                    If value = True Then
                        Me.btnShowProperties.Checked = False
                        Me.btnShowDefaultInheritance.Checked = False
                        Me.btnShowDefaultProperties.Checked = False
                    End If
                End Set
            End Property

            Public Property DefaultPropertiesVisible As Boolean
                Get
                    If Me.btnShowDefaultProperties.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set
                    Me.btnShowDefaultProperties.Checked = value

                    If value = True Then
                        Me.btnShowProperties.Checked = False
                        Me.btnShowDefaultInheritance.Checked = False
                        Me.btnShowInheritance.Checked = False
                    End If
                End Set
            End Property

            Public Property DefaultInheritanceVisible As Boolean
                Get
                    If Me.btnShowDefaultInheritance.Checked Then
                        Return True
                    Else
                        Return False
                    End If
                End Get
                Set
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

            Public Sub New(Panel As DockContent)
                Me.WindowType = Type.Config
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

            ' Main form handle command key events
            ' Adapted from http://kiwigis.blogspot.com/2009/05/adding-tab-key-support-to-propertygrid.html
            Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, keyData As Keys) _
                As Boolean
                If (keyData And Keys.KeyCode) = Keys.Tab Then
                    Dim selectedItem As GridItem = pGrid.SelectedGridItem
                    Dim gridRoot As GridItem = selectedItem
                    While gridRoot.GridItemType <> GridItemType.Root
                        gridRoot = gridRoot.Parent
                    End While

                    Dim gridItems As New List(Of GridItem)
                    FindChildGridItems(gridRoot, gridItems)

                    If Not ContainsGridItemProperty(gridItems) Then Return True

                    Dim newItem As GridItem = selectedItem

                    If keyData = (Keys.Tab Or Keys.Shift) Then
                        newItem = FindPreviousGridItemProperty(gridItems, selectedItem)
                    ElseIf keyData = Keys.Tab Then
                        newItem = FindNextGridItemProperty(gridItems, selectedItem)
                    End If

                    pGrid.SelectedGridItem = newItem

                    Return True ' Handled
                Else
                    Return MyBase.ProcessCmdKey(msg, keyData)
                End If
            End Function

            Private Sub FindChildGridItems(item As GridItem, ByRef gridItems As List(Of GridItem))
                gridItems.Add(item)

                If Not item.Expandable Or item.Expanded Then
                    For Each child As GridItem In item.GridItems
                        FindChildGridItems(child, gridItems)
                    Next
                End If
            End Sub

            Private Function ContainsGridItemProperty(gridItems As List(Of GridItem)) As Boolean
                For Each item As GridItem In gridItems
                    If item.GridItemType = GridItemType.Property Then Return True
                Next
                Return False
            End Function

            Private Function FindPreviousGridItemProperty(gridItems As List(Of GridItem), startItem As GridItem) _
                As GridItem
                If gridItems.Count = 0 Then Return Nothing
                If startItem Is Nothing Then Return Nothing

                Dim startIndex As Integer = gridItems.IndexOf(startItem)

                If startItem.GridItemType = GridItemType.Property Then
                    startIndex = startIndex - 1
                    If startIndex < 0 Then startIndex = gridItems.Count - 1
                End If

                Dim previousIndex = 0
                Dim previousIndexValid = False
                For index As Integer = startIndex To 0 Step - 1
                    If gridItems(index).GridItemType = GridItemType.Property Then
                        previousIndex = index
                        previousIndexValid = True
                        Exit For
                    End If
                Next

                If previousIndexValid Then Return gridItems(previousIndex)

                For index As Integer = gridItems.Count - 1 To startIndex + 1 Step - 1
                    If gridItems(index).GridItemType = GridItemType.Property Then
                        previousIndex = index
                        previousIndexValid = True
                        Exit For
                    End If
                Next

                If Not previousIndexValid Then Return Nothing

                Return gridItems(previousIndex)
            End Function

            Private Function FindNextGridItemProperty(gridItems As List(Of GridItem), startItem As GridItem) As GridItem
                If gridItems.Count = 0 Then Return Nothing
                If startItem Is Nothing Then Return Nothing

                Dim startIndex As Integer = gridItems.IndexOf(startItem)

                If startItem.GridItemType = GridItemType.Property Then
                    startIndex = startIndex + 1
                    If startIndex >= gridItems.Count Then startIndex = 0
                End If

                Dim nextIndex = 0
                Dim nextIndexValid = False
                For index As Integer = startIndex To gridItems.Count - 1
                    If gridItems(index).GridItemType = GridItemType.Property Then
                        nextIndex = index
                        nextIndexValid = True
                        Exit For
                    End If
                Next

                If nextIndexValid Then Return gridItems(nextIndex)

                For index = 0 To startIndex - 1
                    If gridItems(index).GridItemType = GridItemType.Property Then
                        nextIndex = index
                        nextIndexValid = True
                        Exit For
                    End If
                Next

                If Not nextIndexValid Then Return Nothing

                Return gridItems(nextIndex)
            End Function

            Public Sub SetPropertyGridObject(Obj As Object)
                Try
                    Me.btnShowProperties.Enabled = False
                    Me.btnShowInheritance.Enabled = False
                    Me.btnShowDefaultProperties.Enabled = False
                    Me.btnShowDefaultInheritance.Enabled = False
                    Me.btnIcon.Enabled = False
                    Me.btnHostStatus.Enabled = False

                    Me.btnIcon.Image = Nothing

                    If TypeOf Obj Is Info Then 'CONNECTION INFO
                        If TryCast(Obj, Info).IsContainer = False Then 'NO CONTAINER
                            If Me.PropertiesVisible Then 'Properties selected
                                Me.pGrid.SelectedObject = Obj

                                Me.btnShowProperties.Enabled = True
                                If TryCast(Obj, Info).Parent IsNot Nothing Then
                                    Me.btnShowInheritance.Enabled = True
                                Else
                                    Me.btnShowInheritance.Enabled = False
                                End If
                                Me.btnShowDefaultProperties.Enabled = False
                                Me.btnShowDefaultInheritance.Enabled = False
                                btnIcon.Enabled = True
                                Me.btnHostStatus.Enabled = True
                            ElseIf Me.DefaultPropertiesVisible Then 'Defaults selected
                                Me.pGrid.SelectedObject = Obj

                                If TryCast(Obj, Info).IsDefault Then 'Is the default connection
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
                                Me.pGrid.SelectedObject = TryCast(Obj, Info).Inherit

                                Me.btnShowProperties.Enabled = True
                                Me.btnShowInheritance.Enabled = True
                                Me.btnShowDefaultProperties.Enabled = False
                                Me.btnShowDefaultInheritance.Enabled = False
                                Me.btnIcon.Enabled = True
                                Me.btnHostStatus.Enabled = True
                            ElseIf Me.DefaultInheritanceVisible Then 'Default Inhertiance selected
                                pGrid.SelectedObject = Obj

                                Me.btnShowProperties.Enabled = True
                                Me.btnShowInheritance.Enabled = True
                                Me.btnShowDefaultProperties.Enabled = False
                                Me.btnShowDefaultInheritance.Enabled = False
                                Me.btnIcon.Enabled = True
                                Me.btnHostStatus.Enabled = True

                                Me.PropertiesVisible = True
                            End If
                        ElseIf TryCast(Obj, Info).IsContainer Then 'CONTAINER
                            Me.pGrid.SelectedObject = Obj

                            Me.btnShowProperties.Enabled = True
                            If TryCast(TryCast(Obj, Info).Parent, Container.Info).Parent IsNot Nothing Then
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

                        Dim conIcon As Drawing.Icon =
                                Global.mRemote3G.Connection.Icon.FromString(TryCast(Obj, Info).Icon)
                        If conIcon IsNot Nothing Then
                            Me.btnIcon.Image = conIcon.ToBitmap
                        End If
                    ElseIf TypeOf Obj Is Root.Info Then 'ROOT
                        Dim rootInfo = CType(Obj, Root.Info)
                        Select Case rootInfo.Type
                            Case Root.Info.RootType.Connection
                                PropertiesVisible = True
                                DefaultPropertiesVisible = False
                                btnShowProperties.Enabled = True
                                btnShowInheritance.Enabled = False
                                btnShowDefaultProperties.Enabled = True
                                btnShowDefaultInheritance.Enabled = True
                                btnIcon.Enabled = False
                                btnHostStatus.Enabled = False
                            Case Root.Info.RootType.Credential
                                Throw New NotImplementedException
                            Case Root.Info.RootType.PuttySessions
                                PropertiesVisible = True
                                DefaultPropertiesVisible = False
                                btnShowProperties.Enabled = True
                                btnShowInheritance.Enabled = False
                                btnShowDefaultProperties.Enabled = False
                                btnShowDefaultInheritance.Enabled = False
                                btnIcon.Enabled = False
                                btnHostStatus.Enabled = False
                        End Select
                        pGrid.SelectedObject = Obj
                    ElseIf TypeOf Obj Is Info.Inheritance Then 'INHERITANCE
                        Me.pGrid.SelectedObject = Obj

                        If Me.InheritanceVisible Then
                            Me.InheritanceVisible = True
                            Me.btnShowProperties.Enabled = True
                            Me.btnShowInheritance.Enabled = True
                            Me.btnShowDefaultProperties.Enabled = False
                            Me.btnShowDefaultInheritance.Enabled = False
                            Me.btnIcon.Enabled = True
                            Me.btnHostStatus.Enabled =
                                Not TryCast(TryCast(Obj, Info.Inheritance).Parent, Info).IsContainer

                            Me.InheritanceVisible = True

                            Dim conIcon As Drawing.Icon =
                                    Global.mRemote3G.Connection.Icon.FromString(
                                        TryCast(TryCast(Obj, Info.Inheritance).Parent, Info).Icon)
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
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.Language.strConfigPropertyGridObjectFailed & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Public Sub pGrid_SelectedObjectChanged()
                Me.ShowHideGridItems()
            End Sub

#End Region

#Region "Private Methods"

            Private Sub ApplyLanguage()
                btnShowInheritance.Text = Language.Language.strButtonInheritance
                btnShowDefaultInheritance.Text = Language.Language.strButtonDefaultInheritance
                btnShowProperties.Text = Language.Language.strButtonProperties
                btnShowDefaultProperties.Text = Language.Language.strButtonDefaultProperties
                btnIcon.Text = Language.Language.strButtonIcon
                btnHostStatus.Text = Language.Language.strStatus
                Text = Language.Language.strMenuConfig
                TabText = Language.Language.strMenuConfig
                propertyGridContextMenuShowHelpText.Text = Language.Language.strMenuShowHelpText
            End Sub

            Private Sub ApplyTheme()
                With ThemeManager.ActiveTheme
                    pGrid.BackColor = .ToolbarBackgroundColor
                    pGrid.ForeColor = .ToolbarTextColor
                    pGrid.ViewBackColor = .ConfigPanelBackgroundColor
                    pGrid.ViewForeColor = .ConfigPanelTextColor
                    pGrid.LineColor = .ConfigPanelGridLineColor
                    pGrid.HelpBackColor = .ConfigPanelHelpBackgroundColor
                    pGrid.HelpForeColor = .ConfigPanelHelpTextColor
                    pGrid.CategoryForeColor = .ConfigPanelCategoryTextColor
                End With
            End Sub

            Private _originalPropertyGridToolStripItemCountValid As Boolean
            Private _originalPropertyGridToolStripItemCount As Integer

            Private Sub AddToolStripItems()
                Try
                    Dim customToolStrip = New ToolStrip
                    customToolStrip.Items.Add(btnShowProperties)
                    customToolStrip.Items.Add(btnShowInheritance)
                    customToolStrip.Items.Add(btnShowDefaultProperties)
                    customToolStrip.Items.Add(btnShowDefaultInheritance)
                    customToolStrip.Items.Add(btnHostStatus)
                    customToolStrip.Items.Add(btnIcon)
                    customToolStrip.Show()

                    Dim propertyGridToolStrip As New ToolStrip

                    Dim toolStrip As ToolStrip = Nothing
                    For Each control As Control In pGrid.Controls
                        toolStrip = TryCast(control, ToolStrip)

                        If toolStrip IsNot Nothing Then
                            propertyGridToolStrip = toolStrip
                            Exit For
                        End If
                    Next

                    If toolStrip Is Nothing Then
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                            Language.Language.
                                                               strCouldNotFindToolStripInFilteredPropertyGrid, True)
                        Return
                    End If

                    If Not _originalPropertyGridToolStripItemCountValid Then
                        _originalPropertyGridToolStripItemCount = propertyGridToolStrip.Items.Count
                        _originalPropertyGridToolStripItemCountValid = True
                    End If
                    Debug.Assert(_originalPropertyGridToolStripItemCount = 5)

                    ' Hide the "Property Pages" button
                    propertyGridToolStrip.Items(_originalPropertyGridToolStripItemCount - 1).Visible = False

                    Dim expectedToolStripItemCount As Integer = _originalPropertyGridToolStripItemCount +
                                                                customToolStrip.Items.Count
                    If propertyGridToolStrip.Items.Count <> expectedToolStripItemCount Then
                        propertyGridToolStrip.AllowMerge = True
                        ToolStripManager.Merge(customToolStrip, propertyGridToolStrip)
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strConfigUiLoadFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Private Sub Config_Load(sender As Object, e As EventArgs) Handles Me.Load
                ApplyLanguage()

                AddHandler ThemeManager.ThemeChanged, AddressOf ApplyTheme
                ApplyTheme()

                AddToolStripItems()

                pGrid.HelpVisible = Settings.ShowConfigHelpText
            End Sub

            Private Sub Config_SystemColorsChanged(sender As Object, e As EventArgs) Handles MyBase.SystemColorsChanged
                AddToolStripItems()
            End Sub

            Private Sub pGrid_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) _
                Handles pGrid.PropertyValueChanged
                Try
                    If TypeOf Me.pGrid.SelectedObject Is Info Then
                        Select Case e.ChangedItem.Label
                            Case Language.Language.strPropertyNameProtocol
                                TryCast(Me.pGrid.SelectedObject, Info).SetDefaultPort()
                            Case Language.Language.strPropertyNameName
                                Runtime.Windows.treeForm.tvConnections.SelectedNode.Text = Me.pGrid.SelectedObject.Name
                                If Settings.SetHostnameLikeDisplayName And TypeOf Me.pGrid.SelectedObject Is Info Then
                                    Dim connectionInfo = DirectCast(Me.pGrid.SelectedObject, Info)
                                    If Not String.IsNullOrEmpty(connectionInfo.Name) Then
                                        connectionInfo.Hostname = connectionInfo.Name
                                    End If
                                End If
                            Case Language.Language.strPropertyNameIcon
                                Dim conIcon As Drawing.Icon =
                                        Global.mRemote3G.Connection.Icon.FromString(
                                            TryCast(Me.pGrid.SelectedObject, Info).Icon)
                                If conIcon IsNot Nothing Then
                                    Me.btnIcon.Image = conIcon.ToBitmap
                                End If
                            Case Language.Language.strPropertyNameAddress
                                Me.SetHostStatus(Me.pGrid.SelectedObject)
                        End Select

                        If TryCast(Me.pGrid.SelectedObject, Info).IsDefault Then
                            Runtime.DefaultConnectionToSettings()
                        End If
                    End If

                    Dim rootInfo = TryCast(pGrid.SelectedObject, Root.Info)
                    If (rootInfo IsNot Nothing) Then
                        Select Case e.ChangedItem.PropertyDescriptor.Name
                            Case "Password"
                                If rootInfo.Password = True Then
                                    Dim passwordName As String
                                    If Settings.UseSQLServer Then
                                        passwordName = Language.Language.strSQLServer.TrimEnd(":")
                                    Else
                                        passwordName = Path.GetFileName(Runtime.GetStartupConnectionFileName())
                                    End If

                                    Dim password As String = Misc.PasswordDialog(passwordName)

                                    If String.IsNullOrEmpty(password) Then
                                        rootInfo.Password = False
                                    Else
                                        rootInfo.PasswordString = password
                                    End If
                                End If
                            Case "Name"
                                'Windows.treeForm.tvConnections.SelectedNode.Text = pGrid.SelectedObject.Name
                        End Select
                    End If

                    If TypeOf Me.pGrid.SelectedObject Is Info.Inheritance Then
                        If TryCast(Me.pGrid.SelectedObject, Info.Inheritance).IsDefault Then
                            Runtime.DefaultInheritanceToSettings()
                        End If
                    End If

                    Me.ShowHideGridItems()
                    Runtime.SaveConnectionsBG()
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strConfigPropertyGridValueFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Private Sub pGrid_PropertySortChanged(sender As Object, e As EventArgs) Handles pGrid.PropertySortChanged
                If pGrid.PropertySort = PropertySort.CategorizedAlphabetical Then
                    pGrid.PropertySort = PropertySort.Categorized
                End If
            End Sub

            Private Sub ShowHideGridItems()
                Try
                    Dim strHide As New List(Of String)

                    If TypeOf Me.pGrid.SelectedObject Is Info Then
                        Dim conI As Info = pGrid.SelectedObject

                        Select Case conI.Protocol
                            Case Protocols.RDP
                                strHide.Add("ExtApp")
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
                                If conI.RDGatewayUsageMethod = RDP.RDGatewayUsageMethod.Never Then
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
                                If Not (conI.Resolution = RDP.RDPResolutions.FitToWindow Or
                                        conI.Resolution = RDP.RDPResolutions.Fullscreen) Then
                                    strHide.Add("AutomaticResize")
                                End If
                            Case Protocols.VNC
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("ExtApp")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
                                If conI.VNCAuthMode = VNC.AuthMode.AuthVNC Then
                                    strHide.Add("Username")
                                    strHide.Add("Domain")
                                End If
                                If conI.VNCProxyType = VNC.ProxyType.ProxyNone Then
                                    strHide.Add("VNCProxyIP")
                                    strHide.Add("VNCProxyPassword")
                                    strHide.Add("VNCProxyPort")
                                    strHide.Add("VNCProxyUsername")
                                End If
                            Case Protocols.SSH1
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.SSH2
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.Telnet
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("Password")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.Rlogin
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("Password")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.RAW
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("Password")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.HTTP
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.HTTPS
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("ExtApp")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound;Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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
                            Case Protocols.IntApp
                                strHide.Add("CacheBitmaps")
                                strHide.Add("Colors")
                                strHide.Add("DisplayThemes")
                                strHide.Add("DisplayWallpaper")
                                strHide.Add("EnableFontSmoothing")
                                strHide.Add("EnableDesktopComposition")
                                strHide.Add("Domain")
                                strHide.Add("PuttySession")
                                strHide.Add("RDGatewayDomain")
                                strHide.Add("RDGatewayHostname")
                                strHide.Add("RDGatewayPassword")
                                strHide.Add("RDGatewayUsageMethod")
                                strHide.Add("RDGatewayUseConnectionCredentials")
                                strHide.Add("RDGatewayUsername")
                                strHide.Add("RDPAuthenticationLevel")
                                strHide.Add("LoadBalanceInfo")
                                strHide.Add("RedirectDiskDrives")
                                strHide.Add("RedirectKeys")
                                strHide.Add("RedirectPorts")
                                strHide.Add("RedirectPrinters")
                                strHide.Add("RedirectSmartCards")
                                strHide.Add("RedirectSound")
                                strHide.Add("RenderingEngine")
                                strHide.Add("Resolution")
                                strHide.Add("AutomaticResize")
                                strHide.Add("UseConsoleSession")
                                strHide.Add("UseCredSsp")
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

                                If .AutomaticResize Then strHide.Add("AutomaticResize")

                                If .UseConsoleSession Then
                                    strHide.Add("UseConsoleSession")
                                End If

                                If .UseCredSsp Then
                                    strHide.Add("UseCredSsp")
                                End If

                                If .RenderingEngine Then
                                    strHide.Add("RenderingEngine")
                                End If

                                If .RDPAuthenticationLevel Then
                                    strHide.Add("RDPAuthenticationLevel")
                                End If

                                If .LoadBalanceInfo Then strHide.Add("LoadBalanceInfo")

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
                    ElseIf TypeOf pGrid.SelectedObject Is Root.Info Then
                        Dim rootInfo = CType(pGrid.SelectedObject, Root.Info)
                        If rootInfo.Type = Root.Info.RootType.PuttySessions Then
                            strHide.Add("Password")
                        End If
                    End If

                    Me.pGrid.HiddenProperties = strHide.ToArray

                    Me.pGrid.Refresh()
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.Language.strConfigPropertyGridHideItemsFailed & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub btnShowProperties_Click(sender As Object, e As EventArgs) Handles btnShowProperties.Click
                If TypeOf Me.pGrid.SelectedObject Is Info.Inheritance Then
                    If TryCast(Me.pGrid.SelectedObject, Info.Inheritance).IsDefault Then
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag,
                                                         Root.Info))
                    Else
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(Me.pGrid.SelectedObject, Info.Inheritance).Parent)
                    End If
                ElseIf TypeOf Me.pGrid.SelectedObject Is Info Then
                    If TryCast(Me.pGrid.SelectedObject, Info).IsDefault Then
                        Me.PropertiesVisible = True
                        Me.InheritanceVisible = False
                        Me.DefaultPropertiesVisible = False
                        Me.DefaultInheritanceVisible = False
                        Me.SetPropertyGridObject(TryCast(Runtime.Windows.treeForm.tvConnections.SelectedNode.Tag,
                                                         Root.Info))
                    End If
                End If
            End Sub

            Private Sub btnShowDefaultProperties_Click(sender As Object, e As EventArgs) _
                Handles btnShowDefaultProperties.Click
                If TypeOf Me.pGrid.SelectedObject Is Root.Info Or TypeOf Me.pGrid.SelectedObject Is Info.Inheritance _
                    Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = False
                    Me.DefaultPropertiesVisible = True
                    Me.DefaultInheritanceVisible = False
                    Me.SetPropertyGridObject(Runtime.DefaultConnectionFromSettings())
                End If
            End Sub

            Private Sub btnShowInheritance_Click(sender As Object, e As EventArgs) Handles btnShowInheritance.Click
                If TypeOf Me.pGrid.SelectedObject Is Info Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = True
                    Me.DefaultPropertiesVisible = False
                    Me.DefaultInheritanceVisible = False
                    Me.SetPropertyGridObject(TryCast(Me.pGrid.SelectedObject, Info).Inherit)
                End If
            End Sub

            Private Sub btnShowDefaultInheritance_Click(sender As Object, e As EventArgs) _
                Handles btnShowDefaultInheritance.Click
                If TypeOf Me.pGrid.SelectedObject Is Root.Info Or TypeOf Me.pGrid.SelectedObject Is Info Then
                    Me.PropertiesVisible = False
                    Me.InheritanceVisible = False
                    Me.DefaultPropertiesVisible = False
                    Me.DefaultInheritanceVisible = True
                    Me.SetPropertyGridObject(Runtime.DefaultInheritanceFromSettings())
                End If
            End Sub

            Private Sub btnHostStatus_Click(sender As Object, e As EventArgs) Handles btnHostStatus.Click
                SetHostStatus(Me.pGrid.SelectedObject)
            End Sub

            Private Sub btnIcon_Click(sender As Object, e As MouseEventArgs) Handles btnIcon.MouseUp
                Try
                    If TypeOf pGrid.SelectedObject Is Info And
                       Not TypeOf pGrid.SelectedObject Is PuttyInfo Then
                        Me.cMenIcons.Items.Clear()

                        For Each iStr As String In Global.mRemote3G.Connection.Icon.Icons
                            Dim tI As New ToolStripMenuItem
                            tI.Text = iStr
                            tI.Image = Global.mRemote3G.Connection.Icon.FromString(iStr).ToBitmap
                            AddHandler tI.Click, AddressOf IconMenu_Click

                            Me.cMenIcons.Items.Add(tI)
                        Next

                        Dim _
                            mPos As _
                                New Point(PointToScreen(New Point(e.Location.X + Me.pGrid.Width - 100, e.Location.Y)))
                        Me.cMenIcons.Show(mPos)
                    End If
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.Language.strConfigPropertyGridButtonIconClickFailed & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Sub IconMenu_Click(sender As Object, e As EventArgs)
                Try
                    Dim connectionInfo = TryCast(pGrid.SelectedObject, Info)
                    If connectionInfo Is Nothing Then Return

                    Dim selectedMenuItem = TryCast(sender, ToolStripMenuItem)
                    If selectedMenuItem Is Nothing Then Return

                    Dim iconName As String = selectedMenuItem.Text
                    If String.IsNullOrEmpty(iconName) Then Return

                    Dim connectionIcon As Drawing.Icon = Global.mRemote3G.Connection.Icon.FromString(iconName)
                    If connectionIcon Is Nothing Then Return

                    btnIcon.Image = connectionIcon.ToBitmap()

                    connectionInfo.Icon = iconName
                    pGrid.Refresh()

                    Runtime.SaveConnectionsBG()
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.Language.strConfigPropertyGridMenuClickFailed & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

#End Region

#Region "Host Status (Ping)"

            Private HostName As String
            Private pThread As Thread

            Private Sub CheckHostAlive()
                Dim pingSender As New Ping
                Dim pReply As PingReply

                Try
                    pReply = pingSender.Send(HostName)

                    If pReply.Status = IPStatus.Success Then
                        If Me.btnHostStatus.Tag = "checking" Then
                            ShowStatusImage(HostStatus_On)
                        End If
                    Else
                        If Me.btnHostStatus.Tag = "checking" Then
                            ShowStatusImage(HostStatus_Off)
                        End If
                    End If
                Catch ex As Exception
                    If Me.btnHostStatus.Tag = "checking" Then
                        ShowStatusImage(HostStatus_Off)
                    End If
                End Try
            End Sub

            Delegate Sub ShowStatusImageCB([Image] As Image)

            Private Sub ShowStatusImage([Image] As Image)
                If Me.pGrid.InvokeRequired Then
                    Dim d As New ShowStatusImageCB(AddressOf ShowStatusImage)
                    Me.pGrid.Invoke(d, New Object() {[Image]})
                Else
                    Me.btnHostStatus.Image = Image
                    Me.btnHostStatus.Tag = "checkfinished"
                End If
            End Sub

            Public Sub SetHostStatus(ConnectionInfo As Object)
                Try
                    Me.btnHostStatus.Image = HostStatus_Check

                    ' To check status, ConnectionInfo must be an mRemote3G.Connection.Info that is not a container
                    If TypeOf ConnectionInfo Is Info Then
                        If TryCast(ConnectionInfo, Info).IsContainer Then Return
                    Else
                        Return
                    End If

                    Me.btnHostStatus.Tag = "checking"
                    HostName = TryCast(ConnectionInfo, Info).Hostname
                    pThread = New Thread(AddressOf CheckHostAlive)
                    pThread.SetApartmentState(ApartmentState.STA)
                    pThread.IsBackground = True
                    pThread.Start()
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.Language.strConfigPropertyGridSetHostStatusFailed & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

#End Region

            Private Sub propertyGridContextMenu_Opening(sender As Object, e As CancelEventArgs) _
                Handles propertyGridContextMenu.Opening
                Try
                    propertyGridContextMenuShowHelpText.Checked = Settings.ShowConfigHelpText
                    Dim gridItem As GridItem = pGrid.SelectedGridItem
                    propertyGridContextMenuReset.Enabled = (pGrid.SelectedObject IsNot Nothing AndAlso
                                                            gridItem IsNot Nothing AndAlso
                                                            gridItem.PropertyDescriptor IsNot Nothing AndAlso
                                                            gridItem.PropertyDescriptor.CanResetValue(
                                                                pGrid.SelectedObject))
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(
                        "UI.Window.Config.propertyGridContextMenu_Opening() failed.", ex, MessageClass.ErrorMsg, True)
                End Try
            End Sub

            Private Sub propertyGridContextMenuReset_Click(sender As Object, e As EventArgs) _
                Handles propertyGridContextMenuReset.Click
                Try
                    Dim gridItem As GridItem = pGrid.SelectedGridItem
                    If pGrid.SelectedObject IsNot Nothing AndAlso
                       gridItem IsNot Nothing AndAlso
                       gridItem.PropertyDescriptor IsNot Nothing AndAlso
                       gridItem.PropertyDescriptor.CanResetValue(pGrid.SelectedObject) Then
                        pGrid.ResetSelectedProperty()
                    End If
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage(
                        "UI.Window.Config.propertyGridContextMenuReset_Click() failed.", ex, MessageClass.ErrorMsg, True)
                End Try
            End Sub

            Private Sub propertyGridContextMenuShowHelpText_Click(sender As Object, e As EventArgs) _
                Handles propertyGridContextMenuShowHelpText.Click
                propertyGridContextMenuShowHelpText.Checked = Not propertyGridContextMenuShowHelpText.Checked
            End Sub

            Private Sub propertyGridContextMenuShowHelpText_CheckedChanged(sender As Object, e As EventArgs) _
                Handles propertyGridContextMenuShowHelpText.CheckedChanged
                Settings.ShowConfigHelpText = propertyGridContextMenuShowHelpText.Checked
                pGrid.HelpVisible = propertyGridContextMenuShowHelpText.Checked
            End Sub
        End Class
    End Namespace

End Namespace