Imports System.Windows.Forms
Imports System.Xml
Imports System.Globalization
Imports mRemoteNG.App.Runtime
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Namespace Config
    Namespace Connections
        Public Class Load
#Region "Private Properties"
            Private xDom As XmlDocument
            Private confVersion As Double
            Private pW As String = App.Info.General.EncryptionKey

            Private sqlCon As SqlConnection
            Private sqlQuery As SqlCommand
            Private sqlRd As SqlDataReader

            Private selNode As TreeNode
#End Region

#Region "Public Properties"
            Private _UseSQL As Boolean
            Public Property UseSQL() As Boolean
                Get
                    Return _UseSQL
                End Get
                Set(ByVal value As Boolean)
                    _UseSQL = value
                End Set
            End Property

            Private _SQLHost As String
            Public Property SQLHost() As String
                Get
                    Return _SQLHost
                End Get
                Set(ByVal value As String)
                    _SQLHost = value
                End Set
            End Property

            Private _SQLDatabaseName As String
            Public Property SQLDatabaseName() As String
                Get
                    Return _SQLDatabaseName
                End Get
                Set(ByVal value As String)
                    _SQLDatabaseName = value
                End Set
            End Property

            Private _SQLUsername As String
            Public Property SQLUsername() As String
                Get
                    Return _SQLUsername
                End Get
                Set(ByVal value As String)
                    _SQLUsername = value
                End Set
            End Property

            Private _SQLPassword As String
            Public Property SQLPassword() As String
                Get
                    Return _SQLPassword
                End Get
                Set(ByVal value As String)
                    _SQLPassword = value
                End Set
            End Property

            Private _SQLUpdate As Boolean
            Public Property SQLUpdate() As Boolean
                Get
                    Return _SQLUpdate
                End Get
                Set(ByVal value As Boolean)
                    _SQLUpdate = value
                End Set
            End Property

            Private _PreviousSelected As String
            Public Property PreviousSelected() As String
                Get
                    Return _PreviousSelected
                End Get
                Set(ByVal value As String)
                    _PreviousSelected = value
                End Set
            End Property

            Private _ConnectionFileName As String
            Public Property ConnectionFileName() As String
                Get
                    Return Me._ConnectionFileName
                End Get
                Set(ByVal value As String)
                    Me._ConnectionFileName = value
                End Set
            End Property

            Private _RootTreeNode As TreeNode
            Public Property RootTreeNode() As TreeNode
                Get
                    Return Me._RootTreeNode
                End Get
                Set(ByVal value As TreeNode)
                    Me._RootTreeNode = value
                End Set
            End Property

            Private _Import As Boolean
            Public Property Import() As Boolean
                Get
                    Return Me._Import
                End Get
                Set(ByVal value As Boolean)
                    Me._Import = value
                End Set
            End Property

            Private _ConnectionList As Connection.List
            Public Property ConnectionList() As Connection.List
                Get
                    Return Me._ConnectionList
                End Get
                Set(ByVal value As Connection.List)
                    Me._ConnectionList = value
                End Set
            End Property

            Private _ContainerList As Container.List
            Public Property ContainerList() As Container.List
                Get
                    Return Me._ContainerList
                End Get
                Set(ByVal value As Container.List)
                    Me._ContainerList = value
                End Set
            End Property

            Private _PreviousConnectionList As Connection.List
            Public Property PreviousConnectionList() As Connection.List
                Get
                    Return _PreviousConnectionList
                End Get
                Set(ByVal value As Connection.List)
                    _PreviousConnectionList = value
                End Set
            End Property

            Private _PreviousContainerList As Container.List
            Public Property PreviousContainerList() As Container.List
                Get
                    Return _PreviousContainerList
                End Get
                Set(ByVal value As Container.List)
                    _PreviousContainerList = value
                End Set
            End Property
#End Region

#Region "Public Methods"
            Public Sub Load()
                If _UseSQL = True Then
                    LoadFromSQL()
                    SetMainFormText("SQL Server")
                Else
                    Dim strCons As String = DecryptCompleteFile()
                    LoadFromXML(strCons)
                End If

                If Import = False Then
                    SetMainFormText(ConnectionFileName)
                End If
            End Sub
#End Region

#Region "SQL"
            Private Sub LoadFromSQL()
                Try
                    App.Runtime.IsConnectionsFileLoaded = False

                    If _SQLUsername <> "" Then
                        sqlCon = New SqlConnection("Data Source=" & _SQLHost & ";Initial Catalog=" & _SQLDatabaseName & ";User Id=" & _SQLUsername & ";Password=" & _SQLPassword)
                    Else
                        sqlCon = New SqlConnection("Data Source=" & _SQLHost & ";Initial Catalog=" & _SQLDatabaseName & ";Integrated Security=True")
                    End If

                    sqlCon.Open()

                    sqlQuery = New SqlCommand("SELECT * FROM tblRoot", sqlCon)
                    sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection)

                    sqlRd.Read()

                    If sqlRd.HasRows = False Then
                        App.Runtime.SaveConnections()

                        sqlQuery = New SqlCommand("SELECT * FROM tblRoot", sqlCon)
                        sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection)

                        sqlRd.Read()
                    End If

                    Me.confVersion = Convert.ToDouble(sqlRd.Item("confVersion"), CultureInfo.InvariantCulture)

                    Dim rootNode As TreeNode
                    rootNode = New TreeNode(sqlRd.Item("Name"))

                    Dim rInfo As New Root.Info(Root.Info.RootType.Connection)
                    rInfo.Name = rootNode.Text
                    rInfo.TreeNode = rootNode

                    rootNode.Tag = rInfo
                    rootNode.ImageIndex = Images.Enums.TreeImage.Root
                    rootNode.SelectedImageIndex = Images.Enums.TreeImage.Root

                    If Security.Crypt.Decrypt(sqlRd.Item("Protected"), pW) <> "ThisIsNotProtected" Then
                        If Authenticate(sqlRd.Item("Protected"), False, rInfo) = False Then
                            My.Settings.LoadConsFromCustomLocation = False
                            My.Settings.CustomConsPath = ""
                            rootNode.Remove()
                            Exit Sub
                        End If
                    End If

                    'Me._RootTreeNode.Text = rootNode.Text
                    'Me._RootTreeNode.Tag = rootNode.Tag
                    'Me._RootTreeNode.ImageIndex = Images.Enums.TreeImage.Root
                    'Me._RootTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Root

                    sqlRd.Close()

                    ' SECTION 3. Populate the TreeView with the DOM nodes.
                    AddNodesFromSQL(rootNode)
                    'AddNodeFromXML(xDom.DocumentElement, Me._RootTreeNode)

                    rootNode.Expand()

                    'expand containers
                    For Each contI As Container.Info In Me._ContainerList
                        If contI.IsExpanded = True Then
                            contI.TreeNode.Expand()
                        End If
                    Next

                    'open connections from last mremote session
                    If My.Settings.OpenConsFromLastSession = True And My.Settings.NoReconnect = False Then
                        For Each conI As Connection.Info In Me._ConnectionList
                            If conI.PleaseConnect = True Then
                                App.Runtime.OpenConnection(conI)
                            End If
                        Next
                    End If

                    'Tree.Node.TreeView.Nodes.Clear()
                    'Tree.Node.TreeView.Nodes.Add(rootNode)

                    AddNodeToTree(rootNode)
                    SetSelectedNode(selNode)

                    App.Runtime.IsConnectionsFileLoaded = True
                    'App.Runtime.Windows.treeForm.InitialRefresh()

                    sqlCon.Close()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLoadFromSqlFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Delegate Sub AddNodeToTreeCB(ByVal TreeNode As TreeNode)
            Private Sub AddNodeToTree(ByVal TreeNode As TreeNode)
                If Tree.Node.TreeView.InvokeRequired Then
                    Dim d As New AddNodeToTreeCB(AddressOf AddNodeToTree)
                    App.Runtime.Windows.treeForm.Invoke(d, New Object() {TreeNode})
                Else
                    App.Runtime.Windows.treeForm.tvConnections.Nodes.Clear()
                    App.Runtime.Windows.treeForm.tvConnections.Nodes.Add(TreeNode)
                    App.Runtime.Windows.treeForm.InitialRefresh()
                End If
            End Sub

            Private Delegate Sub SetSelectedNodeCB(ByVal TreeNode As TreeNode)
            Private Sub SetSelectedNode(ByVal TreeNode As TreeNode)
                If Tree.Node.TreeView.InvokeRequired Then
                    Dim d As New SetSelectedNodeCB(AddressOf SetSelectedNode)
                    App.Runtime.Windows.treeForm.Invoke(d, New Object() {TreeNode})
                Else
                    App.Runtime.Windows.treeForm.tvConnections.SelectedNode = TreeNode
                End If
            End Sub


            Private Sub AddNodesFromSQL(ByVal baseNode As TreeNode)
                Try
                    sqlCon.Open()
                    sqlQuery = New SqlCommand("SELECT * FROM tblCons ORDER BY PositionID ASC", sqlCon)
                    sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection)

                    If sqlRd.HasRows = False Then
                        Exit Sub
                    End If

                    Dim tNode As TreeNode

                    While sqlRd.Read
                        tNode = New TreeNode(sqlRd.Item("Name"))
                        'baseNode.Nodes.Add(tNode)

                        If Tree.Node.GetNodeTypeFromString(sqlRd.Item("Type")) = Tree.Node.Type.Connection Then
                            Dim conI As Connection.Info = GetConnectionInfoFromSQL()
                            conI.TreeNode = tNode
                            'conI.Parent = prevCont 'NEW

                            Me._ConnectionList.Add(conI)

                            tNode.Tag = conI

                            If SQLUpdate = True Then
                                Dim prevCon As Connection.Info = PreviousConnectionList.FindByConstantID(conI.ConstantID)

                                If prevCon IsNot Nothing Then
                                    For Each prot As Connection.Protocol.Base In prevCon.OpenConnections
                                        prot.InterfaceControl.Info = conI
                                        conI.OpenConnections.Add(prot)
                                    Next

                                    If conI.OpenConnections.Count > 0 Then
                                        tNode.ImageIndex = Images.Enums.TreeImage.ConnectionOpen
                                        tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionOpen
                                    Else
                                        tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                                        tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                                    End If
                                Else
                                    tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                                    tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                                End If

                                If conI.ConstantID = _PreviousSelected Then
                                    selNode = tNode
                                End If
                            Else
                                tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                                tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                            End If
                        ElseIf Tree.Node.GetNodeTypeFromString(sqlRd.Item("Type")) = Tree.Node.Type.Container Then
                            Dim contI As New Container.Info
                            'If tNode.Parent IsNot Nothing Then
                            '    If Tree.Node.GetNodeType(tNode.Parent) = Tree.Node.Type.Container Then
                            '        contI.Parent = tNode.Parent.Tag
                            '    End If
                            'End If
                            'prevCont = contI 'NEW
                            contI.TreeNode = tNode

                            contI.Name = sqlRd.Item("Name")

                            Dim conI As Connection.Info

                            conI = GetConnectionInfoFromSQL()

                            conI.Parent = contI
                            conI.IsContainer = True
                            contI.ConnectionInfo = conI

                            If SQLUpdate = True Then
                                Dim prevCont As Container.Info = PreviousContainerList.FindByConstantID(conI.ConstantID)
                                If prevCont IsNot Nothing Then
                                    contI.IsExpanded = prevCont.IsExpanded
                                End If

                                If conI.ConstantID = _PreviousSelected Then
                                    selNode = tNode
                                End If
                            Else
                                If sqlRd.Item("Expanded") = True Then
                                    contI.IsExpanded = True
                                Else
                                    contI.IsExpanded = False
                                End If
                            End If

                            Me._ContainerList.Add(contI)
                            Me._ConnectionList.Add(conI)

                            tNode.Tag = contI
                            tNode.ImageIndex = Images.Enums.TreeImage.Container
                            tNode.SelectedImageIndex = Images.Enums.TreeImage.Container
                        End If

                        If sqlRd.Item("ParentID") <> 0 Then
                            Dim pNode As TreeNode = Tree.Node.GetNodeFromConstantID(sqlRd.Item("ParentID"))

                            If pNode IsNot Nothing Then
                                pNode.Nodes.Add(tNode)

                                If Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Connection Then
                                    TryCast(tNode.Tag, Connection.Info).Parent = pNode.Tag
                                ElseIf Tree.Node.GetNodeType(tNode) = Tree.Node.Type.Container Then
                                    TryCast(tNode.Tag, Container.Info).Parent = pNode.Tag
                                End If
                            Else
                                baseNode.Nodes.Add(tNode)
                            End If
                        Else
                            baseNode.Nodes.Add(tNode)
                        End If

                        'AddNodesFromSQL(tNode)
                    End While
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strAddNodesFromSqlFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Function GetConnectionInfoFromSQL() As Connection.Info
                Try
                    Dim conI As New Connection.Info

                    With sqlRd
                        conI.PositionID = .Item("PositionID")
                        conI.ConstantID = .Item("ConstantID")
                        conI.Name = .Item("Name")
                        conI.Description = .Item("Description")
                        conI.Hostname = .Item("Hostname")
                        conI.Username = .Item("Username")
                        conI.Password = Security.Crypt.Decrypt(.Item("Password"), pW)
                        conI.Domain = .Item("DomainName")
                        conI.DisplayWallpaper = .Item("DisplayWallpaper")
                        conI.DisplayThemes = .Item("DisplayThemes")
                        conI.CacheBitmaps = .Item("CacheBitmaps")
                        conI.UseConsoleSession = .Item("ConnectToConsole")

                        conI.RedirectDiskDrives = .Item("RedirectDiskDrives")
                        conI.RedirectPrinters = .Item("RedirectPrinters")
                        conI.RedirectPorts = .Item("RedirectPorts")
                        conI.RedirectSmartCards = .Item("RedirectSmartCards")
                        conI.RedirectKeys = .Item("RedirectKeys")
                        conI.RedirectSound = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPSounds), .Item("RedirectSound"))

                        conI.Protocol = Tools.Misc.StringToEnum(GetType(Connection.Protocol.Protocols), .Item("Protocol"))
                        conI.Port = .Item("Port")
                        conI.PuttySession = .Item("PuttySession")

                        conI.Colors = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPColors), .Item("Colors"))
                        conI.Resolution = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPResolutions), .Item("Resolution"))

                        conI.Inherit = New Connection.Info.Inheritance(conI)
                        conI.Inherit.CacheBitmaps = .Item("InheritCacheBitmaps")
                        conI.Inherit.Colors = .Item("InheritColors")
                        conI.Inherit.Description = .Item("InheritDescription")
                        conI.Inherit.DisplayThemes = .Item("InheritDisplayThemes")
                        conI.Inherit.DisplayWallpaper = .Item("InheritDisplayWallpaper")
                        conI.Inherit.Domain = .Item("InheritDomain")
                        conI.Inherit.Icon = .Item("InheritIcon")
                        conI.Inherit.Panel = .Item("InheritPanel")
                        conI.Inherit.Password = .Item("InheritPassword")
                        conI.Inherit.Port = .Item("InheritPort")
                        conI.Inherit.Protocol = .Item("InheritProtocol")
                        conI.Inherit.PuttySession = .Item("InheritPuttySession")
                        conI.Inherit.RedirectDiskDrives = .Item("InheritRedirectDiskDrives")
                        conI.Inherit.RedirectKeys = .Item("InheritRedirectKeys")
                        conI.Inherit.RedirectPorts = .Item("InheritRedirectPorts")
                        conI.Inherit.RedirectPrinters = .Item("InheritRedirectPrinters")
                        conI.Inherit.RedirectSmartCards = .Item("InheritRedirectSmartCards")
                        conI.Inherit.RedirectSound = .Item("InheritRedirectSound")
                        conI.Inherit.Resolution = .Item("InheritResolution")
                        conI.Inherit.UseConsoleSession = .Item("InheritUseConsoleSession")
                        conI.Inherit.Username = .Item("InheritUsername")

                        conI.Icon = .Item("Icon")
                        conI.Panel = .Item("Panel")

                        If Me.confVersion > 1.5 Then '1.6
                            conI.ICAEncryption = Tools.Misc.StringToEnum(GetType(Connection.Protocol.ICA.EncryptionStrength), .Item("ICAEncryptionStrength"))
                            conI.Inherit.ICAEncryption = .Item("InheritICAEncryptionStrength")

                            conI.PreExtApp = .Item("PreExtApp")
                            conI.PostExtApp = .Item("PostExtApp")
                            conI.Inherit.PreExtApp = .Item("InheritPreExtApp")
                            conI.Inherit.PostExtApp = .Item("InheritPostExtApp")
                        End If

                        If Me.confVersion > 1.6 Then '1.7
                            conI.VNCCompression = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Compression), .Item("VNCCompression"))
                            conI.VNCEncoding = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Encoding), .Item("VNCEncoding"))
                            conI.VNCAuthMode = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.AuthMode), .Item("VNCAuthMode"))
                            conI.VNCProxyType = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.ProxyType), .Item("VNCProxyType"))
                            conI.VNCProxyIP = .Item("VNCProxyIP")
                            conI.VNCProxyPort = .Item("VNCProxyPort")
                            conI.VNCProxyUsername = .Item("VNCProxyUsername")
                            conI.VNCProxyPassword = Security.Crypt.Decrypt(.Item("VNCProxyPassword"), pW)
                            conI.VNCColors = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.Colors), .Item("VNCColors"))
                            conI.VNCSmartSizeMode = Tools.Misc.StringToEnum(GetType(Connection.Protocol.VNC.SmartSizeMode), .Item("VNCSmartSizeMode"))
                            conI.VNCViewOnly = .Item("VNCViewOnly")

                            conI.Inherit.VNCCompression = .Item("InheritVNCCompression")
                            conI.Inherit.VNCEncoding = .Item("InheritVNCEncoding")
                            conI.Inherit.VNCAuthMode = .Item("InheritVNCAuthMode")
                            conI.Inherit.VNCProxyType = .Item("InheritVNCProxyType")
                            conI.Inherit.VNCProxyIP = .Item("InheritVNCProxyIP")
                            conI.Inherit.VNCProxyPort = .Item("InheritVNCProxyPort")
                            conI.Inherit.VNCProxyUsername = .Item("InheritVNCProxyUsername")
                            conI.Inherit.VNCProxyPassword = .Item("InheritVNCProxyPassword")
                            conI.Inherit.VNCColors = .Item("InheritVNCColors")
                            conI.Inherit.VNCSmartSizeMode = .Item("InheritVNCSmartSizeMode")
                            conI.Inherit.VNCViewOnly = .Item("InheritVNCViewOnly")
                        End If

                        If Me.confVersion > 1.7 Then '1.8
                            conI.RDPAuthenticationLevel = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.AuthenticationLevel), .Item("RDPAuthenticationLevel"))

                            conI.Inherit.RDPAuthenticationLevel = .Item("InheritRDPAuthenticationLevel")
                        End If

                        If Me.confVersion > 1.8 Then '1.9
                            conI.RenderingEngine = Tools.Misc.StringToEnum(GetType(Connection.Protocol.HTTPBase.RenderingEngine), .Item("RenderingEngine"))
                            conI.MacAddress = .Item("MacAddress")

                            conI.Inherit.RenderingEngine = .Item("InheritRenderingEngine")
                            conI.Inherit.MacAddress = .Item("InheritMacAddress")
                        End If

                        If Me.confVersion > 1.9 Then '2.0
                            conI.UserField = .Item("UserField")

                            conI.Inherit.UserField = .Item("InheritUserField")
                        End If

                        If Me.confVersion > 2.0 Then '2.1
                            conI.ExtApp = .Item("ExtApp")

                            conI.Inherit.ExtApp = .Item("InheritExtApp")
                        End If

                        If Me.confVersion >= 2.2 Then
                            conI.RDGatewayUsageMethod = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), .Item("RDGatewayUsageMethod"))
                            conI.RDGatewayHostname = .Item("RDGatewayHostname")
                            conI.RDGatewayUseConnectionCredentials = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), .Item("RDGatewayUseConnectionCredentials"))
                            conI.RDGatewayUsername = .Item("RDGatewayUsername")
                            conI.RDGatewayPassword = Security.Crypt.Decrypt(.Item("RDGatewayPassword"), pW)
                            conI.RDGatewayDomain = .Item("RDGatewayDomain")
                            conI.Inherit.RDGatewayUsageMethod = .Item("InheritRDGatewayUsageMethod")
                            conI.Inherit.RDGatewayHostname = .Item("InheritRDGatewayHostname")
                            conI.Inherit.RDGatewayUsername = .Item("InheritRDGatewayUsername")
                            conI.Inherit.RDGatewayPassword = .Item("InheritRDGatewayPassword")
                            conI.Inherit.RDGatewayDomain = .Item("InheritRDGatewayDomain")
                        End If

                        If Me.confVersion >= 2.3 Then
                            conI.EnableFontSmoothing = .Item("EnableFontSmoothing")
                            conI.EnableDesktopComposition = .Item("EnableDesktopComposition")
                            conI.Inherit.EnableFontSmoothing = .Item("InheritEnableFontSmoothing")
                            conI.Inherit.EnableDesktopComposition = .Item("InheritEnableDesktopComposition")
                        End If

                        If SQLUpdate = True Then
                            conI.PleaseConnect = .Item("Connected")
                        End If
                    End With

                    Return conI
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strGetConnectionInfoFromSqlFailed & vbNewLine & ex.Message, True)
                End Try

                Return Nothing
            End Function
#End Region

#Region "XML"
            Private Function DecryptCompleteFile() As String
                Dim sRd As New StreamReader(Me._ConnectionFileName)

                Dim strCons As String
                strCons = sRd.ReadToEnd
                sRd.Close()

                If strCons <> "" Then
                    Dim strDecr As String = ""
                    Dim notDecr As Boolean = True

                    If strCons.Contains("<?xml version=""1.0"" encoding=""utf-8""?>") Then
                        strDecr = strCons
                        Return strDecr
                    End If

                    Try
                        strDecr = Security.Crypt.Decrypt(strCons, pW)

                        If strDecr <> strCons Then
                            notDecr = False
                        Else
                            notDecr = True
                        End If
                    Catch ex As Exception
                        notDecr = True
                    End Try

                    If notDecr Then
                        If Authenticate(strCons, True) = True Then
                            strDecr = Security.Crypt.Decrypt(strCons, pW)
                            notDecr = False
                        Else
                            notDecr = True
                        End If

                        If notDecr = False Then
                            Return strDecr
                        End If
                    Else
                        Return strDecr
                    End If
                End If

                Return ""
            End Function

            Private Sub LoadFromXML(Optional ByVal cons As String = "")
                Try
                    App.Runtime.IsConnectionsFileLoaded = False

                    ' SECTION 1. Create a DOM Document and load the XML data into it.
                    Me.xDom = New XmlDocument()
                    If cons <> "" Then
                        xDom.LoadXml(cons)
                    Else
                        xDom.Load(Me._ConnectionFileName)
                    End If

                    If xDom.DocumentElement.HasAttribute("ConfVersion") Then
                        Me.confVersion = Convert.ToDouble(xDom.DocumentElement.Attributes("ConfVersion").Value, CultureInfo.InvariantCulture)
                    Else
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strOldConffile)
                    End If

                    ' SECTION 2. Initialize the treeview control.
                    Dim rootNode As TreeNode

                    Try
                        rootNode = New TreeNode(xDom.DocumentElement.Attributes("Name").Value)
                    Catch ex As Exception
                        rootNode = New TreeNode(xDom.DocumentElement.Name)
                    End Try

                    Dim rInfo As New Root.Info(Root.Info.RootType.Connection)
                    rInfo.Name = rootNode.Text
                    rInfo.TreeNode = rootNode

                    rootNode.Tag = rInfo

                    If Me.confVersion > 1.3 Then '1.4
                        If Security.Crypt.Decrypt(xDom.DocumentElement.Attributes("Protected").Value, pW) <> "ThisIsNotProtected" Then
                            If Authenticate(xDom.DocumentElement.Attributes("Protected").Value, False, rInfo) = False Then
                                My.Settings.LoadConsFromCustomLocation = False
                                My.Settings.CustomConsPath = ""
                                rootNode.Remove()
                                Exit Sub
                            End If
                        End If
                    End If

                    Dim imp As Boolean = False

                    If Me.confVersion > 0.9 Then '1.0
                        If xDom.DocumentElement.Attributes("Export").Value = True Then
                            imp = True
                        End If
                    End If

                    If Me._Import = True And imp = False Then
                        MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strCannotImportNormalSessionFile)

                        Exit Sub
                    End If

                    If imp = False Then
                        Me._RootTreeNode.Text = rootNode.Text
                        Me._RootTreeNode.Tag = rootNode.Tag
                        Me._RootTreeNode.ImageIndex = Images.Enums.TreeImage.Root
                        Me._RootTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Root
                    End If

                    ' SECTION 3. Populate the TreeView with the DOM nodes.
                    AddNodeFromXML(xDom.DocumentElement, Me._RootTreeNode)

                    Me._RootTreeNode.Expand()

                    'expand containers
                    For Each contI As Container.Info In Me._ContainerList
                        If contI.IsExpanded = True Then
                            contI.TreeNode.Expand()
                        End If
                    Next

                    'open connections from last mremote session
                    If My.Settings.OpenConsFromLastSession = True And My.Settings.NoReconnect = False Then
                        For Each conI As Connection.Info In Me._ConnectionList
                            If conI.PleaseConnect = True Then
                                App.Runtime.OpenConnection(conI)
                            End If
                        Next
                    End If

                    Me._RootTreeNode.EnsureVisible()

                    App.Runtime.IsConnectionsFileLoaded = True
                    App.Runtime.Windows.treeForm.InitialRefresh()
                Catch ex As Exception
                    App.Runtime.IsConnectionsFileLoaded = False
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLoadFromXmlFailed & vbNewLine & ex.Message, True)
                    Throw
                End Try
            End Sub

            Private prevCont As Container.Info
            Private Sub AddNodeFromXML(ByRef inXmlNode As XmlNode, ByRef inTreeNode As TreeNode)
                Try
                    Dim i As Integer

                    Dim xNode As XmlNode
                    Dim xNodeList As XmlNodeList
                    Dim tNode As TreeNode

                    ' Loop through the XML nodes until the leaf is reached.
                    ' Add the nodes to the TreeView during the looping process.
                    If inXmlNode.HasChildNodes() Then
                        xNodeList = inXmlNode.ChildNodes
                        For i = 0 To xNodeList.Count - 1
                            xNode = xNodeList(i)
                            inTreeNode.Nodes.Add(New TreeNode(xNode.Attributes("Name").Value))
                            tNode = inTreeNode.Nodes(i)

                            If Tree.Node.GetNodeTypeFromString(xNode.Attributes("Type").Value) = Tree.Node.Type.Connection Then 'connection info
                                Dim conI As Connection.Info = GetConnectionInfoFromXml(xNode)
                                conI.TreeNode = tNode
                                conI.Parent = prevCont 'NEW

                                Me._ConnectionList.Add(conI)

                                tNode.Tag = conI
                                tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
                                tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed
                            ElseIf Tree.Node.GetNodeTypeFromString(xNode.Attributes("Type").Value) = Tree.Node.Type.Container Then  'container info
                                Dim contI As New Container.Info
                                If tNode.Parent IsNot Nothing Then
                                    If Tree.Node.GetNodeType(tNode.Parent) = Tree.Node.Type.Container Then
                                        contI.Parent = tNode.Parent.Tag
                                    End If
                                End If
                                prevCont = contI 'NEW
                                contI.TreeNode = tNode

                                contI.Name = xNode.Attributes("Name").Value

                                If Me.confVersion > 0.7 Then '0.8
                                    If xNode.Attributes("Expanded").Value = "True" Then
                                        contI.IsExpanded = True
                                    Else
                                        contI.IsExpanded = False
                                    End If
                                End If

                                Dim conI As Connection.Info
                                If Me.confVersion > 0.8 Then '0.9
                                    conI = GetConnectionInfoFromXml(xNode)
                                Else
                                    conI = New Connection.Info
                                End If

                                conI.Parent = contI
                                conI.IsContainer = True
                                contI.ConnectionInfo = conI

                                Me._ContainerList.Add(contI)

                                tNode.Tag = contI
                                tNode.ImageIndex = Images.Enums.TreeImage.Container
                                tNode.SelectedImageIndex = Images.Enums.TreeImage.Container
                            End If

                            AddNodeFromXML(xNode, tNode)
                        Next
                    Else
                        inTreeNode.Text = inXmlNode.Attributes("Name").Value.Trim
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strAddNodeFromXmlFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Function GetConnectionInfoFromXml(ByVal xxNode As XmlNode) As Connection.Info
                Dim conI As New Connection.Info

                Try
                    With xxNode
                        If Me.confVersion > 0.1 Then '0.2
                            conI.Name = .Attributes("Name").Value
                            conI.Description = .Attributes("Descr").Value
                            conI.Hostname = .Attributes("Hostname").Value
                            conI.Username = .Attributes("Username").Value
                            conI.Password = Security.Crypt.Decrypt(.Attributes("Password").Value, pW)
                            conI.Domain = .Attributes("Domain").Value
                            conI.DisplayWallpaper = .Attributes("DisplayWallpaper").Value
                            conI.DisplayThemes = .Attributes("DisplayThemes").Value
                            conI.CacheBitmaps = .Attributes("CacheBitmaps").Value

                            If Me.confVersion < 1.1 Then '1.0 - 0.1
                                If .Attributes("Fullscreen").Value = True Then
                                    conI.Resolution = Connection.Protocol.RDP.RDPResolutions.Fullscreen
                                Else
                                    conI.Resolution = Connection.Protocol.RDP.RDPResolutions.FitToWindow
                                End If
                            End If
                        End If

                        If Me.confVersion > 0.2 Then '0.3
                            If Me.confVersion < 0.7 Then
                                If CType(.Attributes("UseVNC").Value, Boolean) = True Then
                                    conI.Protocol = Connection.Protocol.Protocols.VNC
                                    conI.Port = .Attributes("VNCPort").Value
                                Else
                                    conI.Protocol = Connection.Protocol.Protocols.RDP
                                End If
                            End If
                        Else
                            conI.Port = Connection.Protocol.RDP.Defaults.Port
                            conI.Protocol = Connection.Protocol.Protocols.RDP
                        End If

                        If Me.confVersion > 0.3 Then '0.4
                            If Me.confVersion < 0.7 Then
                                If CType(.Attributes("UseVNC").Value, Boolean) = True Then
                                    conI.Port = .Attributes("VNCPort").Value
                                Else
                                    conI.Port = .Attributes("RDPPort").Value
                                End If
                            End If

                            conI.UseConsoleSession = .Attributes("ConnectToConsole").Value
                        Else
                            If Me.confVersion < 0.7 Then
                                If CType(.Attributes("UseVNC").Value, Boolean) = True Then
                                    conI.Port = Connection.Protocol.VNC.Defaults.Port
                                Else
                                    conI.Port = Connection.Protocol.RDP.Defaults.Port
                                End If
                            End If
                            conI.UseConsoleSession = False
                        End If

                        If Me.confVersion > 0.4 Then '0.5 and 0.6
                            conI.RedirectDiskDrives = .Attributes("RedirectDiskDrives").Value
                            conI.RedirectPrinters = .Attributes("RedirectPrinters").Value
                            conI.RedirectPorts = .Attributes("RedirectPorts").Value
                            conI.RedirectSmartCards = .Attributes("RedirectSmartCards").Value
                        Else
                            conI.RedirectDiskDrives = False
                            conI.RedirectPrinters = False
                            conI.RedirectPorts = False
                            conI.RedirectSmartCards = False
                        End If

                        If Me.confVersion > 0.6 Then '0.7
                            conI.Protocol = Tools.Misc.StringToEnum(GetType(Connection.Protocol.Protocols), .Attributes("Protocol").Value)
                            conI.Port = .Attributes("Port").Value
                        End If

                        If Me.confVersion > 0.9 Then '1.0
                            conI.RedirectKeys = .Attributes("RedirectKeys").Value
                        End If

                        If Me.confVersion > 1.1 Then '1.2
                            conI.PuttySession = .Attributes("PuttySession").Value
                        End If

                        If Me.confVersion > 1.2 Then '1.3
                            conI.Colors = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPColors), .Attributes("Colors").Value)
                            conI.Resolution = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPResolutions), .Attributes("Resolution").Value)
                            conI.RedirectSound = Tools.Misc.StringToEnum(GetType(Connection.Protocol.RDP.RDPSounds), .Attributes("RedirectSound").Value)
                        Else
                            Select Case .Attributes("Colors").Value
                                Case 0
                                    conI.Colors = Connection.Protocol.RDP.RDPColors.Colors256
                                Case 1
                                    conI.Colors = Connection.Protocol.RDP.RDPColors.Colors16Bit
                                Case 2
                                    conI.Colors = Connection.Protocol.RDP.RDPColors.Colors24Bit
                                Case 3
                                    conI.Colors = Connection.Protocol.RDP.RDPColors.Colors32Bit
                                Case 4
                                    conI.Colors = Connection.Protocol.RDP.RDPColors.Colors15Bit
                            End Select

                            conI.RedirectSound = .Attributes("RedirectSound").Value
                        End If

                        If Me.confVersion > 1.2 Then '1.3
                            conI.Inherit = New Connection.Info.Inheritance(conI)
                            conI.Inherit.CacheBitmaps = .Attributes("InheritCacheBitmaps").Value
                            conI.Inherit.Colors = .Attributes("InheritColors").Value
                            conI.Inherit.Description = .Attributes("InheritDescription").Value
                            conI.Inherit.DisplayThemes = .Attributes("InheritDisplayThemes").Value
                            conI.Inherit.DisplayWallpaper = .Attributes("InheritDisplayWallpaper").Value
                            conI.Inherit.Domain = .Attributes("InheritDomain").Value
                            conI.Inherit.Icon = .Attributes("InheritIcon").Value
                            conI.Inherit.Panel = .Attributes("InheritPanel").Value
                            conI.Inherit.Password = .Attributes("InheritPassword").Value
                            conI.Inherit.Port = .Attributes("InheritPort").Value
                            conI.Inherit.Protocol = .Attributes("InheritProtocol").Value
                            conI.Inherit.PuttySession = .Attributes("InheritPuttySession").Value
                            conI.Inherit.RedirectDiskDrives = .Attributes("InheritRedirectDiskDrives").Value
                            conI.Inherit.RedirectKeys = .Attributes("InheritRedirectKeys").Value
                            conI.Inherit.RedirectPorts = .Attributes("InheritRedirectPorts").Value
                            conI.Inherit.RedirectPrinters = .Attributes("InheritRedirectPrinters").Value
                            conI.Inherit.RedirectSmartCards = .Attributes("InheritRedirectSmartCards").Value
                            conI.Inherit.RedirectSound = .Attributes("InheritRedirectSound").Value
                            conI.Inherit.Resolution = .Attributes("InheritResolution").Value
                            conI.Inherit.UseConsoleSession = .Attributes("InheritUseConsoleSession").Value
                            conI.Inherit.Username = .Attributes("InheritUsername").Value

                            conI.Icon = .Attributes("Icon").Value
                            conI.Panel = .Attributes("Panel").Value
                        Else
                            conI.Inherit = New Connection.Info.Inheritance(conI, .Attributes("Inherit").Value)

                            conI.Icon = .Attributes("Icon").Value.Replace(".ico", "")
                            conI.Panel = My.Language.strGeneral
                        End If

                        If Me.confVersion > 1.4 Then '1.5
                            conI.PleaseConnect = .Attributes("Connected").Value
                        End If

                        If Me.confVersion > 1.5 Then '1.6
                            conI.ICAEncryption = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.ICA.EncryptionStrength), .Attributes("ICAEncryptionStrength").Value)
                            conI.Inherit.ICAEncryption = .Attributes("InheritICAEncryptionStrength").Value

                            conI.PreExtApp = .Attributes("PreExtApp").Value
                            conI.PostExtApp = .Attributes("PostExtApp").Value
                            conI.Inherit.PreExtApp = .Attributes("InheritPreExtApp").Value
                            conI.Inherit.PostExtApp = .Attributes("InheritPostExtApp").Value
                        End If

                        If Me.confVersion > 1.6 Then '1.7
                            conI.VNCCompression = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.VNC.Compression), .Attributes("VNCCompression").Value)
                            conI.VNCEncoding = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.VNC.Encoding), .Attributes("VNCEncoding").Value)
                            conI.VNCAuthMode = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.VNC.AuthMode), .Attributes("VNCAuthMode").Value)
                            conI.VNCProxyType = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.VNC.ProxyType), .Attributes("VNCProxyType").Value)
                            conI.VNCProxyIP = .Attributes("VNCProxyIP").Value
                            conI.VNCProxyPort = .Attributes("VNCProxyPort").Value
                            conI.VNCProxyUsername = .Attributes("VNCProxyUsername").Value
                            conI.VNCProxyPassword = Security.Crypt.Decrypt(.Attributes("VNCProxyPassword").Value, pW)
                            conI.VNCColors = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.VNC.Colors), .Attributes("VNCColors").Value)
                            conI.VNCSmartSizeMode = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.VNC.SmartSizeMode), .Attributes("VNCSmartSizeMode").Value)
                            conI.VNCViewOnly = .Attributes("VNCViewOnly").Value

                            conI.Inherit.VNCCompression = .Attributes("InheritVNCCompression").Value
                            conI.Inherit.VNCEncoding = .Attributes("InheritVNCEncoding").Value
                            conI.Inherit.VNCAuthMode = .Attributes("InheritVNCAuthMode").Value
                            conI.Inherit.VNCProxyType = .Attributes("InheritVNCProxyType").Value
                            conI.Inherit.VNCProxyIP = .Attributes("InheritVNCProxyIP").Value
                            conI.Inherit.VNCProxyPort = .Attributes("InheritVNCProxyPort").Value
                            conI.Inherit.VNCProxyUsername = .Attributes("InheritVNCProxyUsername").Value
                            conI.Inherit.VNCProxyPassword = .Attributes("InheritVNCProxyPassword").Value
                            conI.Inherit.VNCColors = .Attributes("InheritVNCColors").Value
                            conI.Inherit.VNCSmartSizeMode = .Attributes("InheritVNCSmartSizeMode").Value
                            conI.Inherit.VNCViewOnly = .Attributes("InheritVNCViewOnly").Value
                        End If

                        If Me.confVersion > 1.7 Then '1.8
                            conI.RDPAuthenticationLevel = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.RDP.AuthenticationLevel), .Attributes("RDPAuthenticationLevel").Value)

                            conI.Inherit.RDPAuthenticationLevel = .Attributes("InheritRDPAuthenticationLevel").Value
                        End If

                        If Me.confVersion > 1.8 Then '1.9
                            conI.RenderingEngine = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.HTTPBase.RenderingEngine), .Attributes("RenderingEngine").Value)
                            conI.MacAddress = .Attributes("MacAddress").Value

                            conI.Inherit.RenderingEngine = .Attributes("InheritRenderingEngine").Value
                            conI.Inherit.MacAddress = .Attributes("InheritMacAddress").Value
                        End If

                        If Me.confVersion > 1.9 Then '2.0
                            conI.UserField = .Attributes("UserField").Value
                            conI.Inherit.UserField = .Attributes("InheritUserField").Value
                        End If

                        If Me.confVersion > 2.0 Then '2.1
                            conI.ExtApp = .Attributes("ExtApp").Value
                            conI.Inherit.ExtApp = .Attributes("InheritExtApp").Value
                        End If

                        If Me.confVersion > 2.1 Then '2.2
                            ' Get settings
                            conI.RDGatewayUsageMethod = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), .Attributes("RDGatewayUsageMethod").Value)
                            conI.RDGatewayHostname = .Attributes("RDGatewayHostname").Value
                            conI.RDGatewayUseConnectionCredentials = Tools.Misc.StringToEnum(GetType(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), .Attributes("RDGatewayUseConnectionCredentials").Value)
                            conI.RDGatewayUsername = .Attributes("RDGatewayUsername").Value
                            conI.RDGatewayPassword = Security.Crypt.Decrypt(.Attributes("RDGatewayPassword").Value, pW)
                            conI.RDGatewayDomain = .Attributes("RDGatewayDomain").Value

                            ' Get inheritance settings
                            conI.Inherit.RDGatewayUsageMethod = .Attributes("InheritRDGatewayUsageMethod").Value
                            conI.Inherit.RDGatewayHostname = .Attributes("InheritRDGatewayHostname").Value
                            conI.Inherit.RDGatewayUseConnectionCredentials = .Attributes("InheritRDGatewayUseConnectionCredentials").Value
                            conI.Inherit.RDGatewayUsername = .Attributes("InheritRDGatewayUsername").Value
                            conI.Inherit.RDGatewayPassword = .Attributes("InheritRDGatewayPassword").Value
                            conI.Inherit.RDGatewayDomain = .Attributes("InheritRDGatewayDomain").Value
                        End If

                        If Me.confVersion > 2.2 Then '2.3
                            ' Get settings
                            conI.EnableFontSmoothing = .Attributes("EnableFontSmoothing").Value
                            conI.EnableDesktopComposition = .Attributes("EnableDesktopComposition").Value

                            ' Get inheritance settings
                            conI.Inherit.EnableFontSmoothing = .Attributes("InheritEnableFontSmoothing").Value
                            conI.Inherit.EnableDesktopComposition = .Attributes("InheritEnableDesktopComposition").Value
                        End If
                    End With
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strGetConnectionInfoFromXmlFailed, conI.Name, Me.ConnectionFileName, ex.Message), False)
                End Try
                Return conI
            End Function

            Private Function Authenticate(ByVal Value As String, ByVal CompareToOriginalValue As Boolean, Optional ByVal RootInfo As mRemoteNG.Root.Info = Nothing) As Boolean
                If CompareToOriginalValue Then
                    Do Until Security.Crypt.Decrypt(Value, pW) <> Value
                        pW = Tools.Misc.PasswordDialog(False)

                        If pW = "" Then
                            Return False
                        End If
                    Loop
                Else
                    Do Until Security.Crypt.Decrypt(Value, pW) = "ThisIsProtected"
                        pW = Tools.Misc.PasswordDialog(False)

                        If pW = "" Then
                            Return False
                        End If
                    Loop

                    RootInfo.Password = True
                    RootInfo.PasswordString = pW
                End If

                Return True
            End Function
#End Region
        End Class
    End Namespace
End Namespace