Imports System.Windows.Forms
Imports System.Xml
Imports System.IO
Imports System.Globalization
Imports mRemoteNG.App.Runtime
Imports System.Data.SqlClient
Imports mRemoteNG.Tools.Misc
Imports mRemoteNG.My.Resources

Namespace Config
    Namespace Connections
        Public Class Save
#Region "Public Enums"
            Public Enum Format
                None
                mRXML
                mRCSV
                vRDvRE
                vRDCSV
                SQL
            End Enum
#End Region

#Region "Private Properties"
            Private xW As XmlTextWriter
            Private pW As String = App.Info.General.EncryptionKey

            Private sqlCon As SqlConnection
            Private sqlQuery As SqlCommand
            Private sqlWr As Integer

            Private gIndex As Integer = 0
            Private parentID As String = 0
#End Region

#Region "Public Properties"
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

            Private _Export As Boolean
            Public Property Export() As Boolean
                Get
                    Return Me._Export
                End Get
                Set(ByVal value As Boolean)
                    Me._Export = value
                End Set
            End Property

            Private _SaveFormat As Format
            Public Property SaveFormat() As Format
                Get
                    Return _SaveFormat
                End Get
                Set(ByVal value As Format)
                    _SaveFormat = value
                End Set
            End Property

            Private _SaveSecurity As Security.Save
            Public Property SaveSecurity() As Security.Save
                Get
                    Return Me._SaveSecurity
                End Get
                Set(ByVal value As Security.Save)
                    Me._SaveSecurity = value
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
#End Region

#Region "Public Methods"
            Public Sub Save()
                Select Case SaveFormat
                    Case Format.SQL
                        SaveToSQL()
                        SetMainFormText("SQL Server")
                    Case Format.mRCSV
                        SaveTomRCSV()
                    Case Format.vRDvRE
                        SaveToVRE()
                    Case Format.vRDCSV
                        SaveTovRDCSV()
                    Case Else
                        SaveToXML()
                        If My.Settings.EncryptCompleteConnectionsFile Then
                            EncryptCompleteFile()
                        End If
                        If Not _Export Then SetMainFormText(_ConnectionFileName)
                End Select
            End Sub
#End Region

#Region "SQL"
            Private Function VerifyDatabaseVersion(ByVal sqlConnection As SqlConnection) As Boolean
                Dim isVerified As Boolean = False
                Dim sqlDataReader As SqlDataReader = Nothing
                Dim databaseVersion As System.Version = Nothing
                Try
                    Dim sqlCommand As New SqlCommand("SELECT * FROM tblRoot", sqlConnection)
                    sqlDataReader = sqlCommand.ExecuteReader()
                    If (Not sqlDataReader.HasRows) Then Return True ' assume new empty database
                    sqlDataReader.Read()

                    Dim enCulture As CultureInfo = New CultureInfo("en-US")
                    databaseVersion = New System.Version(Convert.ToDouble(sqlDataReader.Item("confVersion"), enCulture))

                    sqlDataReader.Close()

                    If databaseVersion.CompareTo(New System.Version(2, 2)) = 0 Then ' 2.2
                        mC.AddMessage(Messages.MessageClass.InformationMsg, String.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString, "2.3"))
                        sqlCommand = New SqlCommand("ALTER TABLE tblCons ADD EnableFontSmoothing bit NOT NULL DEFAULT 0, EnableDesktopComposition bit NOT NULL DEFAULT 0, InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;", sqlConnection)
                        sqlCommand.ExecuteNonQuery()
                        databaseVersion = New System.Version(2, 3)
                    End If

                    If databaseVersion.CompareTo(New System.Version(2, 3)) = 0 Then ' 2.3
                        isVerified = True
                    End If

                    If isVerified = False Then
                        mC.AddMessage(Messages.MessageClass.WarningMsg, String.Format(strErrorBadDatabaseVersion, databaseVersion.ToString, My.Application.Info.ProductName))
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(strErrorVerifyDatabaseVersionFailed, ex.Message))
                Finally
                    If sqlDataReader IsNot Nothing Then
                        If Not sqlDataReader.IsClosed Then sqlDataReader.Close()
                    End If
                End Try
                Return isVerified
            End Function

            Private Sub SaveToSQL()
                If _SQLUsername <> "" Then
                    sqlCon = New SqlConnection("Data Source=" & _SQLHost & ";Initial Catalog=" & _SQLDatabaseName & ";User Id=" & _SQLUsername & ";Password=" & _SQLPassword)
                Else
                    sqlCon = New SqlConnection("Data Source=" & _SQLHost & ";Initial Catalog=" & _SQLDatabaseName & ";Integrated Security=True")
                End If

                sqlCon.Open()

                If Not VerifyDatabaseVersion(sqlCon) Then
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, strErrorConnectionListSaveFailed)
                    Return
                End If

                Dim tN As TreeNode
                tN = RootTreeNode.Clone

                Dim strProtected As String
                If tN.Tag IsNot Nothing Then
                    If TryCast(tN.Tag, mRemoteNG.Root.Info).Password = True Then
                        pW = TryCast(tN.Tag, mRemoteNG.Root.Info).PasswordString
                        strProtected = Security.Crypt.Encrypt("ThisIsProtected", pW)
                    Else
                        strProtected = Security.Crypt.Encrypt("ThisIsNotProtected", pW)
                    End If
                Else
                    strProtected = Security.Crypt.Encrypt("ThisIsNotProtected", pW)
                End If

                sqlQuery = New SqlCommand("DELETE FROM tblRoot", sqlCon)
                sqlWr = sqlQuery.ExecuteNonQuery

                Dim enCulture As CultureInfo = New CultureInfo("en-US")
                sqlQuery = New SqlCommand("INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES('" & PrepareValueForDB(tN.Text) & "', 0, '" & strProtected & "'," & App.Info.Connections.ConnectionFileVersion.ToString(enCulture) & ")", sqlCon)
                sqlWr = sqlQuery.ExecuteNonQuery

                sqlQuery = New SqlCommand("DELETE FROM tblCons", sqlCon)
                sqlWr = sqlQuery.ExecuteNonQuery

                Dim tNC As TreeNodeCollection
                tNC = tN.Nodes

                SaveNodesSQL(tNC)

                sqlQuery = New SqlCommand("DELETE FROM tblUpdate", sqlCon)
                sqlWr = sqlQuery.ExecuteNonQuery
                sqlQuery = New SqlCommand("INSERT INTO tblUpdate (LastUpdate) VALUES('" & Tools.Misc.DBDate(Now) & "')", sqlCon)
                sqlWr = sqlQuery.ExecuteNonQuery

                sqlCon.Close()
            End Sub

            Private Sub SaveNodesSQL(ByVal tnc As TreeNodeCollection)
                For Each node As TreeNode In tnc
                    gIndex += 1

                    Dim curConI As Connection.Info
                    sqlQuery = New SqlCommand("INSERT INTO tblCons (Name, Type, Expanded, Description, Icon, Panel, Username, " & _
                                               "DomainName, Password, Hostname, Protocol, PuttySession, " & _
                                               "Port, ConnectToConsole, RenderingEngine, ICAEncryptionStrength, RDPAuthenticationLevel, Colors, Resolution, DisplayWallpaper, " & _
                                               "DisplayThemes, EnableFontSmoothing, EnableDesktopComposition, CacheBitmaps, RedirectDiskDrives, RedirectPorts, " & _
                                               "RedirectPrinters, RedirectSmartCards, RedirectSound, RedirectKeys, " & _
                                               "Connected, PreExtApp, PostExtApp, MacAddress, UserField, ExtApp, VNCCompression, VNCEncoding, VNCAuthMode, " & _
                                               "VNCProxyType, VNCProxyIP, VNCProxyPort, VNCProxyUsername, VNCProxyPassword, " & _
                                               "VNCColors, VNCSmartSizeMode, VNCViewOnly, " & _
                                               "RDGatewayUsageMethod, RDGatewayHostname, RDGatewayUseConnectionCredentials, RDGatewayUsername, RDGatewayPassword, RDGatewayDomain, " & _
                                               "InheritCacheBitmaps, InheritColors, " & _
                                               "InheritDescription, InheritDisplayThemes, InheritDisplayWallpaper, InheritEnableFontSmoothing, InheritEnableDesktopComposition, InheritDomain, " & _
                                               "InheritIcon, InheritPanel, InheritPassword, InheritPort, " & _
                                               "InheritProtocol, InheritPuttySession, InheritRedirectDiskDrives, " & _
                                               "InheritRedirectKeys, InheritRedirectPorts, InheritRedirectPrinters, " & _
                                               "InheritRedirectSmartCards, InheritRedirectSound, InheritResolution, " & _
                                               "InheritUseConsoleSession, InheritRenderingEngine, InheritUsername, InheritICAEncryptionStrength, InheritRDPAuthenticationLevel, " & _
                                               "InheritPreExtApp, InheritPostExtApp, InheritMacAddress, InheritUserField, InheritExtApp, InheritVNCCompression, InheritVNCEncoding, " & _
                                               "InheritVNCAuthMode, InheritVNCProxyType, InheritVNCProxyIP, InheritVNCProxyPort, " & _
                                               "InheritVNCProxyUsername, InheritVNCProxyPassword, InheritVNCColors, " & _
                                               "InheritVNCSmartSizeMode, InheritVNCViewOnly, " & _
                                               "InheritRDGatewayUsageMethod, InheritRDGatewayHostname, InheritRDGatewayUseConnectionCredentials, InheritRDGatewayUsername, InheritRDGatewayPassword, InheritRDGatewayDomain, " & _
                                               "PositionID, ParentID, ConstantID, LastChange)" & _
                                               "VALUES (", sqlCon)

                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Or Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then
                        'xW.WriteStartElement("Node")
                        sqlQuery.CommandText &= "'" & PrepareValueForDB(node.Text) & "'," 'Name
                        sqlQuery.CommandText &= "'" & Tree.Node.GetNodeType(node).ToString & "'," 'Type
                    End If

                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then 'container
                        sqlQuery.CommandText &= "'" & Me._ContainerList(node.Tag).IsExpanded & "'," 'Expanded
                        curConI = Me._ContainerList(node.Tag).ConnectionInfo
                        SaveConnectionFieldsSQL(curConI)

                        sqlQuery.CommandText = Tools.Misc.PrepareForDB(sqlQuery.CommandText)
                        sqlQuery.ExecuteNonQuery()
                        'parentID = gIndex
                        SaveNodesSQL(node.Nodes)
                        'xW.WriteEndElement()
                    End If

                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                        sqlQuery.CommandText &= "'" & False & "',"
                        curConI = Me._ConnectionList(node.Tag)
                        SaveConnectionFieldsSQL(curConI)
                        'xW.WriteEndElement()
                        sqlQuery.CommandText = Tools.Misc.PrepareForDB(sqlQuery.CommandText)
                        sqlQuery.ExecuteNonQuery()
                    End If

                    'parentID = 0
                Next
            End Sub

            Private Sub SaveConnectionFieldsSQL(ByVal curConI As Connection.Info)
                With curConI
                    sqlQuery.CommandText &= "'" & PrepareValueForDB(.Description) & "',"
                    sqlQuery.CommandText &= "'" & PrepareValueForDB(.Icon) & "',"
                    sqlQuery.CommandText &= "'" & PrepareValueForDB(.Panel) & "',"

                    If Me._SaveSecurity.Username = True Then
                        sqlQuery.CommandText &= "'" & PrepareValueForDB(.Username) & "',"
                    Else
                        sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        sqlQuery.CommandText &= "'" & PrepareValueForDB(.Domain) & "',"
                    Else
                        sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Password = True Then
                        sqlQuery.CommandText &= "'" & PrepareValueForDB(Security.Crypt.Encrypt(.Password, pW)) & "',"
                    Else
                        sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    sqlQuery.CommandText &= "'" & PrepareValueForDB(.Hostname) & "',"
                    sqlQuery.CommandText &= "'" & .Protocol.ToString & "',"
                    sqlQuery.CommandText &= "'" & PrepareValueForDB(.PuttySession) & "',"
                    sqlQuery.CommandText &= "'" & .Port & "',"
                    sqlQuery.CommandText &= "'" & .UseConsoleSession & "',"
                    sqlQuery.CommandText &= "'" & .RenderingEngine.ToString & "',"
                    sqlQuery.CommandText &= "'" & .ICAEncryption.ToString & "',"
                    sqlQuery.CommandText &= "'" & .RDPAuthenticationLevel.ToString & "',"
                    sqlQuery.CommandText &= "'" & .Colors.ToString & "',"
                    sqlQuery.CommandText &= "'" & .Resolution.ToString & "',"
                    sqlQuery.CommandText &= "'" & .DisplayWallpaper & "',"
                    sqlQuery.CommandText &= "'" & .DisplayThemes & "',"
                    sqlQuery.CommandText &= "'" & .EnableFontSmoothing & "',"
                    sqlQuery.CommandText &= "'" & .EnableDesktopComposition & "',"
                    sqlQuery.CommandText &= "'" & .CacheBitmaps & "',"
                    sqlQuery.CommandText &= "'" & .RedirectDiskDrives & "',"
                    sqlQuery.CommandText &= "'" & .RedirectPorts & "',"
                    sqlQuery.CommandText &= "'" & .RedirectPrinters & "',"
                    sqlQuery.CommandText &= "'" & .RedirectSmartCards & "',"
                    sqlQuery.CommandText &= "'" & .RedirectSound.ToString & "',"
                    sqlQuery.CommandText &= "'" & .RedirectKeys & "',"

                    If curConI.OpenConnections.Count > 0 Then
                        sqlQuery.CommandText &= 1 & ","
                    Else
                        sqlQuery.CommandText &= 0 & ","
                    End If

                    sqlQuery.CommandText &= "'" & .PreExtApp & "',"
                    sqlQuery.CommandText &= "'" & .PostExtApp & "',"
                    sqlQuery.CommandText &= "'" & .MacAddress & "',"
                    sqlQuery.CommandText &= "'" & .UserField & "',"
                    sqlQuery.CommandText &= "'" & .ExtApp & "',"

                    sqlQuery.CommandText &= "'" & .VNCCompression.ToString & "',"
                    sqlQuery.CommandText &= "'" & .VNCEncoding.ToString & "',"
                    sqlQuery.CommandText &= "'" & .VNCAuthMode.ToString & "',"
                    sqlQuery.CommandText &= "'" & .VNCProxyType.ToString & "',"
                    sqlQuery.CommandText &= "'" & .VNCProxyIP & "',"
                    sqlQuery.CommandText &= "'" & .VNCProxyPort & "',"
                    sqlQuery.CommandText &= "'" & .VNCProxyUsername & "',"
                    sqlQuery.CommandText &= "'" & Security.Crypt.Encrypt(.VNCProxyPassword, pW) & "',"
                    sqlQuery.CommandText &= "'" & .VNCColors.ToString & "',"
                    sqlQuery.CommandText &= "'" & .VNCSmartSizeMode.ToString & "',"
                    sqlQuery.CommandText &= "'" & .VNCViewOnly & "',"

                    sqlQuery.CommandText &= "'" & .RDGatewayUsageMethod.ToString & "',"
                    sqlQuery.CommandText &= "'" & .RDGatewayHostname & "',"
                    sqlQuery.CommandText &= "'" & .RDGatewayUseConnectionCredentials.ToString & "',"

                    If Me._SaveSecurity.Username = True Then
                        sqlQuery.CommandText &= "'" & .RDGatewayUsername & "',"
                    Else
                        sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Password = True Then
                        sqlQuery.CommandText &= "'" & .RDGatewayPassword & "',"
                    Else
                        sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        sqlQuery.CommandText &= "'" & .RDGatewayDomain & "',"
                    Else
                        sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    With .Inherit
                        If Me._SaveSecurity.Inheritance = True Then
                            sqlQuery.CommandText &= "'" & .CacheBitmaps & "',"
                            sqlQuery.CommandText &= "'" & .Colors & "',"
                            sqlQuery.CommandText &= "'" & .Description & "',"
                            sqlQuery.CommandText &= "'" & .DisplayThemes & "',"
                            sqlQuery.CommandText &= "'" & .DisplayWallpaper & "',"
                            sqlQuery.CommandText &= "'" & .EnableFontSmoothing & "',"
                            sqlQuery.CommandText &= "'" & .EnableDesktopComposition & "',"
                            sqlQuery.CommandText &= "'" & .Domain & "',"
                            sqlQuery.CommandText &= "'" & .Icon & "',"
                            sqlQuery.CommandText &= "'" & .Panel & "',"
                            sqlQuery.CommandText &= "'" & .Password & "',"
                            sqlQuery.CommandText &= "'" & .Port & "',"
                            sqlQuery.CommandText &= "'" & .Protocol & "',"
                            sqlQuery.CommandText &= "'" & .PuttySession & "',"
                            sqlQuery.CommandText &= "'" & .RedirectDiskDrives & "',"
                            sqlQuery.CommandText &= "'" & .RedirectKeys & "',"
                            sqlQuery.CommandText &= "'" & .RedirectPorts & "',"
                            sqlQuery.CommandText &= "'" & .RedirectPrinters & "',"
                            sqlQuery.CommandText &= "'" & .RedirectSmartCards & "',"
                            sqlQuery.CommandText &= "'" & .RedirectSound & "',"
                            sqlQuery.CommandText &= "'" & .Resolution & "',"
                            sqlQuery.CommandText &= "'" & .UseConsoleSession & "',"
                            sqlQuery.CommandText &= "'" & .RenderingEngine & "',"
                            sqlQuery.CommandText &= "'" & .Username & "',"
                            sqlQuery.CommandText &= "'" & .ICAEncryption & "',"
                            sqlQuery.CommandText &= "'" & .RDPAuthenticationLevel & "',"
                            sqlQuery.CommandText &= "'" & .PreExtApp & "',"
                            sqlQuery.CommandText &= "'" & .PostExtApp & "',"
                            sqlQuery.CommandText &= "'" & .MacAddress & "',"
                            sqlQuery.CommandText &= "'" & .UserField & "',"
                            sqlQuery.CommandText &= "'" & .ExtApp & "',"

                            sqlQuery.CommandText &= "'" & .VNCCompression & "',"
                            sqlQuery.CommandText &= "'" & .VNCEncoding & "',"
                            sqlQuery.CommandText &= "'" & .VNCAuthMode & "',"
                            sqlQuery.CommandText &= "'" & .VNCProxyType & "',"
                            sqlQuery.CommandText &= "'" & .VNCProxyIP & "',"
                            sqlQuery.CommandText &= "'" & .VNCProxyPort & "',"
                            sqlQuery.CommandText &= "'" & .VNCProxyUsername & "',"
                            sqlQuery.CommandText &= "'" & .VNCProxyPassword & "',"
                            sqlQuery.CommandText &= "'" & .VNCColors & "',"
                            sqlQuery.CommandText &= "'" & .VNCSmartSizeMode & "',"
                            sqlQuery.CommandText &= "'" & .VNCViewOnly & "',"

                            sqlQuery.CommandText &= "'" & .RDGatewayUsageMethod & "',"
                            sqlQuery.CommandText &= "'" & .RDGatewayHostname & "',"
                            sqlQuery.CommandText &= "'" & .RDGatewayUseConnectionCredentials & "',"
                            sqlQuery.CommandText &= "'" & .RDGatewayUsername & "',"
                            sqlQuery.CommandText &= "'" & .RDGatewayPassword & "',"
                            sqlQuery.CommandText &= "'" & .RDGatewayDomain & "',"
                        Else
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"

                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"
                            sqlQuery.CommandText &= "'" & False & "',"

                            sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayUsageMethod
                            sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayHostname
                            sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayUseConnectionCredentials
                            sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayUsername
                            sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayPassword
                            sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayDomain
                        End If
                    End With

                    .PositionID = gIndex

                    If .IsContainer = False Then
                        If .Parent IsNot Nothing Then
                            parentID = TryCast(.Parent, Container.Info).ConnectionInfo.ConstantID
                        Else
                            parentID = 0
                        End If
                    Else
                        If TryCast(.Parent, Container.Info).Parent IsNot Nothing Then
                            parentID = TryCast(TryCast(.Parent, Container.Info).Parent, Container.Info).ConnectionInfo.ConstantID
                        Else
                            parentID = 0
                        End If
                    End If

                    sqlQuery.CommandText &= gIndex & "," & parentID & "," & .ConstantID & ",'" & Tools.Misc.DBDate(Now) & "')"
                End With
            End Sub
#End Region

#Region "XML"
            Private Sub EncryptCompleteFile()
                Dim sRd As New StreamReader(Me._ConnectionFileName)

                Dim strCons As String
                strCons = sRd.ReadToEnd
                sRd.Close()

                If strCons <> "" Then
                    Dim sWr As New StreamWriter(Me._ConnectionFileName)
                    sWr.Write(Security.Crypt.Encrypt(strCons, pW))
                    sWr.Close()
                End If
            End Sub

            Private Sub SaveToXML()
                Try
                    If App.Runtime.ConnectionsFileLoaded = False Then
                        Exit Sub
                    End If

                    Dim tN As TreeNode
                    Dim exp As Boolean = False

                    If Tree.Node.GetNodeType(RootTreeNode) = Tree.Node.Type.Root Then
                        tN = RootTreeNode.Clone
                    Else
                        tN = New TreeNode("mR|Export (" + Tools.Misc.DBDate(Now) + ")")
                        tN.Nodes.Add(RootTreeNode.Clone)
                        exp = True
                    End If

                    xW = New XmlTextWriter(Me._ConnectionFileName, System.Text.Encoding.UTF8)

                    xW.Formatting = Formatting.Indented
                    xW.Indentation = 4

                    xW.WriteStartDocument()

                    xW.WriteStartElement("Connections")
                    xW.WriteAttributeString("Name", "", tN.Text)
                    xW.WriteAttributeString("Export", "", exp)

                    If exp Then
                        xW.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsNotProtected", pW))
                    Else
                        If TryCast(tN.Tag, mRemoteNG.Root.Info).Password = True Then
                            pW = TryCast(tN.Tag, mRemoteNG.Root.Info).PasswordString
                            xW.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsProtected", pW))
                        Else
                            xW.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsNotProtected", pW))
                        End If
                    End If

                    Dim enCulture As System.Globalization.CultureInfo = New CultureInfo("en-US")
                    xW.WriteAttributeString("ConfVersion", "", App.Info.Connections.ConnectionFileVersion.ToString(enCulture))

                    Dim tNC As TreeNodeCollection
                    tNC = tN.Nodes

                    saveNode(tNC)

                    xW.WriteEndElement()
                    xW.Close()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SaveToXML failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub saveNode(ByVal tNC As TreeNodeCollection)
                Try
                    For Each node As TreeNode In tNC
                        Dim curConI As Connection.Info

                        If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Or Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then
                            xW.WriteStartElement("Node")
                            xW.WriteAttributeString("Name", "", node.Text)
                            xW.WriteAttributeString("Type", "", Tree.Node.GetNodeType(node).ToString)
                        End If

                        If Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then 'container
                            xW.WriteAttributeString("Expanded", "", Me._ContainerList(node.Tag).TreeNode.IsExpanded)
                            curConI = Me._ContainerList(node.Tag).ConnectionInfo
                            SaveConnectionFields(curConI)
                            saveNode(node.Nodes)
                            xW.WriteEndElement()
                        End If

                        If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                            curConI = Me._ConnectionList(node.Tag)
                            SaveConnectionFields(curConI)
                            xW.WriteEndElement()
                        End If
                    Next
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "saveNode failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SaveConnectionFields(ByVal curConI As Connection.Info)
                Try
                    xW.WriteAttributeString("Descr", "", curConI.Description)

                    xW.WriteAttributeString("Icon", "", curConI.Icon)

                    xW.WriteAttributeString("Panel", "", curConI.Panel)

                    If Me._SaveSecurity.Username = True Then
                        xW.WriteAttributeString("Username", "", curConI.Username)
                    Else
                        xW.WriteAttributeString("Username", "", "")
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        xW.WriteAttributeString("Domain", "", curConI.Domain)
                    Else
                        xW.WriteAttributeString("Domain", "", "")
                    End If

                    If Me._SaveSecurity.Password = True Then
                        xW.WriteAttributeString("Password", "", Security.Crypt.Encrypt(curConI.Password, pW))
                    Else
                        xW.WriteAttributeString("Password", "", "")
                    End If

                    xW.WriteAttributeString("Hostname", "", curConI.Hostname)

                    xW.WriteAttributeString("Protocol", "", curConI.Protocol.ToString)

                    xW.WriteAttributeString("PuttySession", "", curConI.PuttySession)

                    xW.WriteAttributeString("Port", "", curConI.Port)

                    xW.WriteAttributeString("ConnectToConsole", "", curConI.UseConsoleSession)

                    xW.WriteAttributeString("RenderingEngine", "", curConI.RenderingEngine.ToString)

                    xW.WriteAttributeString("ICAEncryptionStrength", "", curConI.ICAEncryption.ToString)

                    xW.WriteAttributeString("RDPAuthenticationLevel", "", curConI.RDPAuthenticationLevel.ToString)

                    xW.WriteAttributeString("Colors", "", curConI.Colors.ToString)

                    xW.WriteAttributeString("Resolution", "", curConI.Resolution.ToString)

                    xW.WriteAttributeString("DisplayWallpaper", "", curConI.DisplayWallpaper)

                    xW.WriteAttributeString("DisplayThemes", "", curConI.DisplayThemes)

                    xW.WriteAttributeString("EnableFontSmoothing", "", curConI.EnableFontSmoothing)

                    xW.WriteAttributeString("EnableDesktopComposition", "", curConI.EnableDesktopComposition)

                    xW.WriteAttributeString("CacheBitmaps", "", curConI.CacheBitmaps)

                    xW.WriteAttributeString("RedirectDiskDrives", "", curConI.RedirectDiskDrives)

                    xW.WriteAttributeString("RedirectPorts", "", curConI.RedirectPorts)

                    xW.WriteAttributeString("RedirectPrinters", "", curConI.RedirectPrinters)

                    xW.WriteAttributeString("RedirectSmartCards", "", curConI.RedirectSmartCards)

                    xW.WriteAttributeString("RedirectSound", "", curConI.RedirectSound.ToString)

                    xW.WriteAttributeString("RedirectKeys", "", curConI.RedirectKeys)

                    If curConI.OpenConnections.Count > 0 Then
                        xW.WriteAttributeString("Connected", "", True)
                    Else
                        xW.WriteAttributeString("Connected", "", False)
                    End If

                    xW.WriteAttributeString("PreExtApp", "", curConI.PreExtApp)
                    xW.WriteAttributeString("PostExtApp", "", curConI.PostExtApp)
                    xW.WriteAttributeString("MacAddress", "", curConI.MacAddress)
                    xW.WriteAttributeString("UserField", "", curConI.UserField)
                    xW.WriteAttributeString("ExtApp", "", curConI.ExtApp)

                    xW.WriteAttributeString("VNCCompression", "", curConI.VNCCompression.ToString)
                    xW.WriteAttributeString("VNCEncoding", "", curConI.VNCEncoding.ToString)
                    xW.WriteAttributeString("VNCAuthMode", "", curConI.VNCAuthMode.ToString)
                    xW.WriteAttributeString("VNCProxyType", "", curConI.VNCProxyType.ToString)
                    xW.WriteAttributeString("VNCProxyIP", "", curConI.VNCProxyIP)
                    xW.WriteAttributeString("VNCProxyPort", "", curConI.VNCProxyPort)
                    xW.WriteAttributeString("VNCProxyUsername", "", curConI.VNCProxyUsername)
                    xW.WriteAttributeString("VNCProxyPassword", "", Security.Crypt.Encrypt(curConI.VNCProxyPassword, pW))
                    xW.WriteAttributeString("VNCColors", "", curConI.VNCColors.ToString)
                    xW.WriteAttributeString("VNCSmartSizeMode", "", curConI.VNCSmartSizeMode.ToString)
                    xW.WriteAttributeString("VNCViewOnly", "", curConI.VNCViewOnly)

                    xW.WriteAttributeString("RDGatewayUsageMethod", "", curConI.RDGatewayUsageMethod.ToString)
                    xW.WriteAttributeString("RDGatewayHostname", "", curConI.RDGatewayHostname)

                    xW.WriteAttributeString("RDGatewayUseConnectionCredentials", "", curConI.RDGatewayUseConnectionCredentials.ToString)

                    If Me._SaveSecurity.Username = True Then
                        xW.WriteAttributeString("RDGatewayUsername", "", curConI.RDGatewayUsername)
                    Else
                        xW.WriteAttributeString("RDGatewayUsername", "", "")
                    End If

                    If Me._SaveSecurity.Password = True Then
                        xW.WriteAttributeString("RDGatewayPassword", "", Security.Crypt.Encrypt(curConI.RDGatewayPassword, pW))
                    Else
                        xW.WriteAttributeString("RDGatewayPassword", "", "")
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        xW.WriteAttributeString("RDGatewayDomain", "", curConI.RDGatewayDomain)
                    Else
                        xW.WriteAttributeString("RDGatewayDomain", "", "")
                    End If

                    If Me._SaveSecurity.Inheritance = True Then
                        xW.WriteAttributeString("InheritCacheBitmaps", "", curConI.Inherit.CacheBitmaps)
                        xW.WriteAttributeString("InheritColors", "", curConI.Inherit.Colors)
                        xW.WriteAttributeString("InheritDescription", "", curConI.Inherit.Description)
                        xW.WriteAttributeString("InheritDisplayThemes", "", curConI.Inherit.DisplayThemes)
                        xW.WriteAttributeString("InheritDisplayWallpaper", "", curConI.Inherit.DisplayWallpaper)
                        xW.WriteAttributeString("InheritEnableFontSmoothing", "", curConI.Inherit.EnableFontSmoothing)
                        xW.WriteAttributeString("InheritEnableDesktopComposition", "", curConI.Inherit.EnableDesktopComposition)
                        xW.WriteAttributeString("InheritDomain", "", curConI.Inherit.Domain)
                        xW.WriteAttributeString("InheritIcon", "", curConI.Inherit.Icon)
                        xW.WriteAttributeString("InheritPanel", "", curConI.Inherit.Panel)
                        xW.WriteAttributeString("InheritPassword", "", curConI.Inherit.Password)
                        xW.WriteAttributeString("InheritPort", "", curConI.Inherit.Port)
                        xW.WriteAttributeString("InheritProtocol", "", curConI.Inherit.Protocol)
                        xW.WriteAttributeString("InheritPuttySession", "", curConI.Inherit.PuttySession)
                        xW.WriteAttributeString("InheritRedirectDiskDrives", "", curConI.Inherit.RedirectDiskDrives)
                        xW.WriteAttributeString("InheritRedirectKeys", "", curConI.Inherit.RedirectKeys)
                        xW.WriteAttributeString("InheritRedirectPorts", "", curConI.Inherit.RedirectPorts)
                        xW.WriteAttributeString("InheritRedirectPrinters", "", curConI.Inherit.RedirectPrinters)
                        xW.WriteAttributeString("InheritRedirectSmartCards", "", curConI.Inherit.RedirectSmartCards)
                        xW.WriteAttributeString("InheritRedirectSound", "", curConI.Inherit.RedirectSound)
                        xW.WriteAttributeString("InheritResolution", "", curConI.Inherit.Resolution)
                        xW.WriteAttributeString("InheritUseConsoleSession", "", curConI.Inherit.UseConsoleSession)
                        xW.WriteAttributeString("InheritRenderingEngine", "", curConI.Inherit.RenderingEngine)
                        xW.WriteAttributeString("InheritUsername", "", curConI.Inherit.Username)
                        xW.WriteAttributeString("InheritICAEncryptionStrength", "", curConI.Inherit.ICAEncryption)
                        xW.WriteAttributeString("InheritRDPAuthenticationLevel", "", curConI.Inherit.RDPAuthenticationLevel)
                        xW.WriteAttributeString("InheritPreExtApp", "", curConI.Inherit.PreExtApp)
                        xW.WriteAttributeString("InheritPostExtApp", "", curConI.Inherit.PostExtApp)
                        xW.WriteAttributeString("InheritMacAddress", "", curConI.Inherit.MacAddress)
                        xW.WriteAttributeString("InheritUserField", "", curConI.Inherit.UserField)
                        xW.WriteAttributeString("InheritExtApp", "", curConI.Inherit.ExtApp)
                        xW.WriteAttributeString("InheritVNCCompression", "", curConI.Inherit.VNCCompression)
                        xW.WriteAttributeString("InheritVNCEncoding", "", curConI.Inherit.VNCEncoding)
                        xW.WriteAttributeString("InheritVNCAuthMode", "", curConI.Inherit.VNCAuthMode)
                        xW.WriteAttributeString("InheritVNCProxyType", "", curConI.Inherit.VNCProxyType)
                        xW.WriteAttributeString("InheritVNCProxyIP", "", curConI.Inherit.VNCProxyIP)
                        xW.WriteAttributeString("InheritVNCProxyPort", "", curConI.Inherit.VNCProxyPort)
                        xW.WriteAttributeString("InheritVNCProxyUsername", "", curConI.Inherit.VNCProxyUsername)
                        xW.WriteAttributeString("InheritVNCProxyPassword", "", curConI.Inherit.VNCProxyPassword)
                        xW.WriteAttributeString("InheritVNCColors", "", curConI.Inherit.VNCColors)
                        xW.WriteAttributeString("InheritVNCSmartSizeMode", "", curConI.Inherit.VNCSmartSizeMode)
                        xW.WriteAttributeString("InheritVNCViewOnly", "", curConI.Inherit.VNCViewOnly)
                        xW.WriteAttributeString("InheritRDGatewayUsageMethod", "", curConI.Inherit.RDGatewayUsageMethod)
                        xW.WriteAttributeString("InheritRDGatewayHostname", "", curConI.Inherit.RDGatewayHostname)
                        xW.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", curConI.Inherit.RDGatewayUseConnectionCredentials)
                        xW.WriteAttributeString("InheritRDGatewayUsername", "", curConI.Inherit.RDGatewayUsername)
                        xW.WriteAttributeString("InheritRDGatewayPassword", "", curConI.Inherit.RDGatewayPassword)
                        xW.WriteAttributeString("InheritRDGatewayDomain", "", curConI.Inherit.RDGatewayDomain)
                    Else
                        xW.WriteAttributeString("InheritCacheBitmaps", "", False)
                        xW.WriteAttributeString("InheritColors", "", False)
                        xW.WriteAttributeString("InheritDescription", "", False)
                        xW.WriteAttributeString("InheritDisplayThemes", "", False)
                        xW.WriteAttributeString("InheritDisplayWallpaper", "", False)
                        xW.WriteAttributeString("InheritEnableFontSmoothing", "", False)
                        xW.WriteAttributeString("InheritEnableDesktopComposition", "", False)
                        xW.WriteAttributeString("InheritDomain", "", False)
                        xW.WriteAttributeString("InheritIcon", "", False)
                        xW.WriteAttributeString("InheritPanel", "", False)
                        xW.WriteAttributeString("InheritPassword", "", False)
                        xW.WriteAttributeString("InheritPort", "", False)
                        xW.WriteAttributeString("InheritProtocol", "", False)
                        xW.WriteAttributeString("InheritPuttySession", "", False)
                        xW.WriteAttributeString("InheritRedirectDiskDrives", "", False)
                        xW.WriteAttributeString("InheritRedirectKeys", "", False)
                        xW.WriteAttributeString("InheritRedirectPorts", "", False)
                        xW.WriteAttributeString("InheritRedirectPrinters", "", False)
                        xW.WriteAttributeString("InheritRedirectSmartCards", "", False)
                        xW.WriteAttributeString("InheritRedirectSound", "", False)
                        xW.WriteAttributeString("InheritResolution", "", False)
                        xW.WriteAttributeString("InheritUseConsoleSession", "", False)
                        xW.WriteAttributeString("InheritRenderingEngine", "", False)
                        xW.WriteAttributeString("InheritUsername", "", False)
                        xW.WriteAttributeString("InheritICAEncryptionStrength", "", False)
                        xW.WriteAttributeString("InheritRDPAuthenticationLevel", "", False)
                        xW.WriteAttributeString("InheritPreExtApp", "", False)
                        xW.WriteAttributeString("InheritPostExtApp", "", False)
                        xW.WriteAttributeString("InheritMacAddress", "", False)
                        xW.WriteAttributeString("InheritUserField", "", False)
                        xW.WriteAttributeString("InheritExtApp", "", False)
                        xW.WriteAttributeString("InheritVNCCompression", "", False)
                        xW.WriteAttributeString("InheritVNCEncoding", "", False)
                        xW.WriteAttributeString("InheritVNCAuthMode", "", False)
                        xW.WriteAttributeString("InheritVNCProxyType", "", False)
                        xW.WriteAttributeString("InheritVNCProxyIP", "", False)
                        xW.WriteAttributeString("InheritVNCProxyPort", "", False)
                        xW.WriteAttributeString("InheritVNCProxyUsername", "", False)
                        xW.WriteAttributeString("InheritVNCProxyPassword", "", False)
                        xW.WriteAttributeString("InheritVNCColors", "", False)
                        xW.WriteAttributeString("InheritVNCSmartSizeMode", "", False)
                        xW.WriteAttributeString("InheritVNCViewOnly", "", False)
                        xW.WriteAttributeString("InheritRDGatewayHostname", "", False)
                        xW.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", False)
                        xW.WriteAttributeString("InheritRDGatewayUsername", "", False)
                        xW.WriteAttributeString("InheritRDGatewayPassword", "", False)
                        xW.WriteAttributeString("InheritRDGatewayDomain", "", False)
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "SaveConnectionFields failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "CSV"
            Private csvWr As StreamWriter

            Private Sub SaveTomRCSV()
                If App.Runtime.ConnectionsFileLoaded = False Then
                    Exit Sub
                End If

                Dim tN As TreeNode
                tN = RootTreeNode.Clone

                Dim tNC As TreeNodeCollection
                tNC = tN.Nodes

                csvWr = New StreamWriter(ConnectionFileName)


                Dim csvLn As String = String.Empty

                csvLn += "Name;Folder;Description;Icon;Panel;"

                If SaveSecurity.Username Then
                    csvLn += "Username;"
                End If

                If SaveSecurity.Password Then
                    csvLn += "Password;"
                End If

                If SaveSecurity.Domain Then
                    csvLn += "Domain;"
                End If

                csvLn += "Hostname;Protocol;PuttySession;Port;ConnectToConsole;RenderingEngine;ICAEncryptionStrength;RDPAuthenticationLevel;Colors;Resolution;DisplayWallpaper;DisplayThemes;EnableFontSmoothing;EnableDesktopComposition;CacheBitmaps;RedirectDiskDrives;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;RedirectKeys;PreExtApp;PostExtApp;MacAddress;UserField;ExtApp;VNCCompression;VNCEncoding;VNCAuthMode;VNCProxyType;VNCProxyIP;VNCProxyPort;VNCProxyUsername;VNCProxyPassword;VNCColors;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;"

                If SaveSecurity.Inheritance Then
                    csvLn += "InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritUseConsoleSession;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain"
                End If

                csvWr.WriteLine(csvLn)

                SaveNodemRCSV(tNC)

                csvWr.Close()
            End Sub

            Private Sub SaveNodemRCSV(ByVal tNC As TreeNodeCollection)
                For Each node As TreeNode In tNC
                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                        Dim curConI As Connection.Info = node.Tag

                        WritemRCSVLine(curConI)
                    ElseIf Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then
                        SaveNodemRCSV(node.Nodes)
                    End If
                Next
            End Sub

            Private Sub WritemRCSVLine(ByVal con As Connection.Info)
                Dim nodePath As String = con.TreeNode.FullPath

                Dim firstSlash As Integer = nodePath.IndexOf("\")
                nodePath = nodePath.Remove(0, firstSlash + 1)
                Dim lastSlash As Integer = nodePath.LastIndexOf("\")

                If lastSlash > 0 Then
                    nodePath = nodePath.Remove(lastSlash)
                Else
                    nodePath = ""
                End If

                Dim csvLn As String = String.Empty

                csvLn += con.Name & ";" & nodePath & ";" & con.Description & ";" & con.Icon & ";" & con.Panel & ";"

                If SaveSecurity.Username Then
                    csvLn += con.Username & ";"
                End If

                If SaveSecurity.Password Then
                    csvLn += con.Password & ";"
                End If

                If SaveSecurity.Domain Then
                    csvLn += con.Domain & ";"
                End If

                csvLn += con.Hostname & ";" & con.Protocol.ToString & ";" & con.PuttySession & ";" & con.Port & ";" & con.UseConsoleSession & ";" & con.RenderingEngine.ToString & ";" & con.ICAEncryption.ToString & ";" & con.RDPAuthenticationLevel.ToString & ";" & con.Colors.ToString & ";" & con.Resolution.ToString & ";" & con.DisplayWallpaper & ";" & con.DisplayThemes & ";" & con.EnableFontSmoothing & ";" & con.EnableDesktopComposition & ";" & con.CacheBitmaps & ";" & con.RedirectDiskDrives & ";" & con.RedirectPorts & ";" & con.RedirectPrinters & ";" & con.RedirectSmartCards & ";" & con.RedirectSound.ToString & ";" & con.RedirectKeys & ";" & con.PreExtApp & ";" & con.PostExtApp & ";" & con.MacAddress & ";" & con.UserField & ";" & con.ExtApp & ";" & con.VNCCompression.ToString & ";" & con.VNCEncoding.ToString & ";" & con.VNCAuthMode.ToString & ";" & con.VNCProxyType.ToString & ";" & con.VNCProxyIP & ";" & con.VNCProxyPort & ";" & con.VNCProxyUsername & ";" & con.VNCProxyPassword & ";" & con.VNCColors.ToString & ";" & con.VNCSmartSizeMode.ToString & ";" & con.VNCViewOnly & ";"

                If SaveSecurity.Inheritance Then
                    csvLn += con.Inherit.CacheBitmaps & ";" & con.Inherit.Colors & ";" & con.Inherit.Description & ";" & con.Inherit.DisplayThemes & ";" & con.Inherit.DisplayWallpaper & ";" & con.Inherit.EnableFontSmoothing & ";" & con.Inherit.EnableDesktopComposition & ";" & con.Inherit.Domain & ";" & con.Inherit.Icon & ";" & con.Inherit.Panel & ";" & con.Inherit.Password & ";" & con.Inherit.Port & ";" & con.Inherit.Protocol & ";" & con.Inherit.PuttySession & ";" & con.Inherit.RedirectDiskDrives & ";" & con.Inherit.RedirectKeys & ";" & con.Inherit.RedirectPorts & ";" & con.Inherit.RedirectPrinters & ";" & con.Inherit.RedirectSmartCards & ";" & con.Inherit.RedirectSound & ";" & con.Inherit.Resolution & ";" & con.Inherit.UseConsoleSession & ";" & con.Inherit.RenderingEngine & ";" & con.Inherit.Username & ";" & con.Inherit.ICAEncryption & ";" & con.Inherit.RDPAuthenticationLevel & ";" & con.Inherit.PreExtApp & ";" & con.Inherit.PostExtApp & ";" & con.Inherit.MacAddress & ";" & con.Inherit.UserField & ";" & con.Inherit.ExtApp & ";" & con.Inherit.VNCCompression & ";" & con.Inherit.VNCEncoding & ";" & con.Inherit.VNCAuthMode & ";" & con.Inherit.VNCProxyType & ";" & con.Inherit.VNCProxyIP & ";" & con.Inherit.VNCProxyPort & ";" & con.Inherit.VNCProxyUsername & ";" & con.Inherit.VNCProxyPassword & ";" & con.Inherit.VNCColors & ";" & con.Inherit.VNCSmartSizeMode & ";" & con.Inherit.VNCViewOnly
                End If

                csvWr.WriteLine(csvLn)
            End Sub
#End Region

#Region "vRD CSV"
            Private Sub SaveTovRDCSV()
                If App.Runtime.ConnectionsFileLoaded = False Then
                    Exit Sub
                End If

                Dim tN As TreeNode
                tN = RootTreeNode.Clone

                Dim tNC As TreeNodeCollection
                tNC = tN.Nodes

                csvWr = New StreamWriter(ConnectionFileName)

                SaveNodevRDCSV(tNC)

                csvWr.Close()
            End Sub

            Private Sub SaveNodevRDCSV(ByVal tNC As TreeNodeCollection)
                For Each node As TreeNode In tNC
                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                        Dim curConI As Connection.Info = node.Tag

                        If curConI.Protocol = Connection.Protocol.Protocols.RDP Then
                            WritevRDCSVLine(curConI)
                        End If
                    ElseIf Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then
                        SaveNodevRDCSV(node.Nodes)
                    End If
                Next
            End Sub

            Private Sub WritevRDCSVLine(ByVal con As Connection.Info)
                Dim nodePath As String = con.TreeNode.FullPath

                Dim firstSlash As Integer = nodePath.IndexOf("\")
                nodePath = nodePath.Remove(0, firstSlash + 1)
                Dim lastSlash As Integer = nodePath.LastIndexOf("\")

                If lastSlash > 0 Then
                    nodePath = nodePath.Remove(lastSlash)
                Else
                    nodePath = ""
                End If

                csvWr.WriteLine(con.Name & ";" & con.Hostname & ";" & con.MacAddress & ";;" & con.Port & ";" & con.UseConsoleSession & ";" & nodePath)
            End Sub
#End Region

#Region "vRD VRE"
            Private Sub SaveToVRE()
                If App.Runtime.ConnectionsFileLoaded = False Then
                    Exit Sub
                End If

                Dim tN As TreeNode
                tN = RootTreeNode.Clone

                Dim tNC As TreeNodeCollection
                tNC = tN.Nodes

                xW = New XmlTextWriter(_ConnectionFileName, System.Text.Encoding.UTF8)
                xW.Formatting = Formatting.Indented
                xW.Indentation = 4

                xW.WriteStartDocument()

                xW.WriteStartElement("vRDConfig")
                xW.WriteAttributeString("Version", "", "2.0")

                xW.WriteStartElement("Connections")
                SaveNodeVRE(tNC)
                xW.WriteEndElement()

                xW.WriteEndElement()
                xW.WriteEndDocument()
                xW.Close()
            End Sub

            Private Sub SaveNodeVRE(ByVal tNC As TreeNodeCollection)
                For Each node As TreeNode In tNC
                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                        Dim curConI As Connection.Info = node.Tag

                        If curConI.Protocol = Connection.Protocol.Protocols.RDP Then
                            xW.WriteStartElement("Connection")
                            xW.WriteAttributeString("Id", "", "")

                            WriteVREitem(curConI)

                            xW.WriteEndElement()
                        End If
                    Else
                        SaveNodeVRE(node.Nodes)
                    End If
                Next
            End Sub

            Private Sub WriteVREitem(ByVal con As Connection.Info)
                'Name
                xW.WriteStartElement("ConnectionName")
                xW.WriteValue(con.Name)
                xW.WriteEndElement()

                'Hostname
                xW.WriteStartElement("ServerName")
                xW.WriteValue(con.Hostname)
                xW.WriteEndElement()

                'Mac Adress
                xW.WriteStartElement("MACAddress")
                xW.WriteValue(con.MacAddress)
                xW.WriteEndElement()

                'Management Board URL
                xW.WriteStartElement("MgmtBoardUrl")
                xW.WriteValue("")
                xW.WriteEndElement()

                'Description
                xW.WriteStartElement("Description")
                xW.WriteValue(con.Description)
                xW.WriteEndElement()

                'Port
                xW.WriteStartElement("Port")
                xW.WriteValue(con.Port)
                xW.WriteEndElement()

                'Console Session
                xW.WriteStartElement("Console")
                xW.WriteValue(con.UseConsoleSession)
                xW.WriteEndElement()

                'Redirect Clipboard
                xW.WriteStartElement("ClipBoard")
                xW.WriteValue(True)
                xW.WriteEndElement()

                'Redirect Printers
                xW.WriteStartElement("Printer")
                xW.WriteValue(con.RedirectPrinters)
                xW.WriteEndElement()

                'Redirect Ports
                xW.WriteStartElement("Serial")
                xW.WriteValue(con.RedirectPorts)
                xW.WriteEndElement()

                'Redirect Disks
                xW.WriteStartElement("LocalDrives")
                xW.WriteValue(con.RedirectDiskDrives)
                xW.WriteEndElement()

                'Redirect Smartcards
                xW.WriteStartElement("SmartCard")
                xW.WriteValue(con.RedirectSmartCards)
                xW.WriteEndElement()

                'Connection Place
                xW.WriteStartElement("ConnectionPlace")
                xW.WriteValue("2") '----------------------------------------------------------
                xW.WriteEndElement()

                'Smart Size
                xW.WriteStartElement("AutoSize")
                xW.WriteValue(IIf(con.Resolution = Connection.Protocol.RDP.RDPResolutions.SmartSize, True, False))
                xW.WriteEndElement()

                'SeparateResolutionX
                xW.WriteStartElement("SeparateResolutionX")
                xW.WriteValue("1024")
                xW.WriteEndElement()

                'SeparateResolutionY
                xW.WriteStartElement("SeparateResolutionY")
                xW.WriteValue("768")
                xW.WriteEndElement()

                'TabResolutionX
                xW.WriteStartElement("TabResolutionX")
                If con.Resolution <> Connection.Protocol.RDP.RDPResolutions.FitToWindow And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.Fullscreen And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.SmartSize Then
                    xW.WriteValue(con.Resolution.ToString.Remove(con.Resolution.ToString.IndexOf("x")))
                Else
                    xW.WriteValue("1024")
                End If
                xW.WriteEndElement()

                'TabResolutionY
                xW.WriteStartElement("TabResolutionY")
                If con.Resolution <> Connection.Protocol.RDP.RDPResolutions.FitToWindow And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.Fullscreen And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.SmartSize Then
                    xW.WriteValue(con.Resolution.ToString.Remove(0, con.Resolution.ToString.IndexOf("x")))
                Else
                    xW.WriteValue("768")
                End If
                xW.WriteEndElement()

                'RDPColorDepth
                xW.WriteStartElement("RDPColorDepth")
                xW.WriteValue(con.Colors.ToString.Replace("Colors", "").Replace("Bit", ""))
                xW.WriteEndElement()

                'Bitmap Caching
                xW.WriteStartElement("BitmapCaching")
                xW.WriteValue(con.CacheBitmaps)
                xW.WriteEndElement()

                'Themes
                xW.WriteStartElement("Themes")
                xW.WriteValue(con.DisplayThemes)
                xW.WriteEndElement()

                'Wallpaper
                xW.WriteStartElement("Wallpaper")
                xW.WriteValue(con.DisplayWallpaper)
                xW.WriteEndElement()
            End Sub
#End Region
        End Class
    End Namespace
End Namespace