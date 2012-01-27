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
            Private _xmlTextWriter As XmlTextWriter
            Private _password As String = App.Info.General.EncryptionKey

            Private _sqlConnection As SqlConnection
            Private _sqlQuery As SqlCommand

            Private _currentNodeIndex As Integer = 0
            Private _parentConstantId As String = 0
#End Region

#Region "Public Properties"
            Public Property SQLHost As String
            Public Property SQLDatabaseName As String
            Public Property SQLUsername As String
            Public Property SQLPassword As String

            Public Property ConnectionFileName As String
            Public Property RootTreeNode As TreeNode
            Public Property Export As Boolean
            Public Property SaveFormat As Format
            Public Property SaveSecurity As Security.Save
            Public Property ConnectionList As Connection.List
            Public Property ContainerList As Container.List
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
                        SaveToXml()
                        If My.Settings.EncryptCompleteConnectionsFile Then
                            EncryptCompleteFile()
                        End If
                        If Not _Export Then SetMainFormText(ConnectionFileName)
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

                    databaseVersion = New System.Version(Convert.ToDouble(sqlDataReader.Item("confVersion"), CultureInfo.InvariantCulture))

                    sqlDataReader.Close()

                    If databaseVersion.CompareTo(New System.Version(2, 2)) = 0 Then ' 2.2
                        MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, String.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString, "2.3"))
                        sqlCommand = New SqlCommand("ALTER TABLE tblCons ADD EnableFontSmoothing bit NOT NULL DEFAULT 0, EnableDesktopComposition bit NOT NULL DEFAULT 0, InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;", sqlConnection)
                        sqlCommand.ExecuteNonQuery()
                        databaseVersion = New System.Version(2, 3)
                    End If

                    If databaseVersion.CompareTo(New System.Version(2, 3)) = 0 Then ' 2.3
                        isVerified = True
                    End If

                    If isVerified = False Then
                        MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, String.Format(My.Language.strErrorBadDatabaseVersion, databaseVersion.ToString, My.Application.Info.ProductName))
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, String.Format(My.Language.strErrorVerifyDatabaseVersionFailed, ex.Message))
                Finally
                    If sqlDataReader IsNot Nothing Then
                        If Not sqlDataReader.IsClosed Then sqlDataReader.Close()
                    End If
                End Try
                Return isVerified
            End Function

            Private Sub SaveToSQL()
                If _SQLUsername <> "" Then
                    _sqlConnection = New SqlConnection("Data Source=" & _SQLHost & ";Initial Catalog=" & _SQLDatabaseName & ";User Id=" & _SQLUsername & ";Password=" & _SQLPassword)
                Else
                    _sqlConnection = New SqlConnection("Data Source=" & _SQLHost & ";Initial Catalog=" & _SQLDatabaseName & ";Integrated Security=True")
                End If

                _sqlConnection.Open()

                If Not VerifyDatabaseVersion(_sqlConnection) Then
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strErrorConnectionListSaveFailed)
                    Return
                End If

                Dim tN As TreeNode
                tN = RootTreeNode.Clone

                Dim strProtected As String
                If tN.Tag IsNot Nothing Then
                    If TryCast(tN.Tag, mRemoteNG.Root.Info).Password = True Then
                        _password = TryCast(tN.Tag, mRemoteNG.Root.Info).PasswordString
                        strProtected = Security.Crypt.Encrypt("ThisIsProtected", _password)
                    Else
                        strProtected = Security.Crypt.Encrypt("ThisIsNotProtected", _password)
                    End If
                Else
                    strProtected = Security.Crypt.Encrypt("ThisIsNotProtected", _password)
                End If

                _sqlQuery = New SqlCommand("DELETE FROM tblRoot", _sqlConnection)
                _sqlQuery.ExecuteNonQuery()

                _sqlQuery = New SqlCommand("INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES('" & PrepareValueForDB(tN.Text) & "', 0, '" & strProtected & "'," & App.Info.Connections.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) & ")", _sqlConnection)
                _sqlQuery.ExecuteNonQuery()

                _sqlQuery = New SqlCommand("DELETE FROM tblCons", _sqlConnection)
                _sqlQuery.ExecuteNonQuery()

                Dim tNC As TreeNodeCollection
                tNC = tN.Nodes

                SaveNodesSQL(tNC)

                _sqlQuery = New SqlCommand("DELETE FROM tblUpdate", _sqlConnection)
                _sqlQuery.ExecuteNonQuery()
                _sqlQuery = New SqlCommand("INSERT INTO tblUpdate (LastUpdate) VALUES('" & Tools.Misc.DBDate(Now) & "')", _sqlConnection)
                _sqlQuery.ExecuteNonQuery()

                _sqlConnection.Close()
            End Sub

            Private Sub SaveNodesSQL(ByVal tnc As TreeNodeCollection)
                For Each node As TreeNode In tnc
                    _currentNodeIndex += 1

                    Dim curConI As Connection.Info
                    _sqlQuery = New SqlCommand("INSERT INTO tblCons (Name, Type, Expanded, Description, Icon, Panel, Username, " & _
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
                                               "PositionID, _parentConstantId, ConstantID, LastChange)" & _
                                               "VALUES (", _sqlConnection)

                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Or Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then
                        '_xmlTextWriter.WriteStartElement("Node")
                        _sqlQuery.CommandText &= "'" & PrepareValueForDB(node.Text) & "'," 'Name
                        _sqlQuery.CommandText &= "'" & Tree.Node.GetNodeType(node).ToString & "'," 'Type
                    End If

                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then 'container
                        _sqlQuery.CommandText &= "'" & Me._ContainerList(node.Tag).IsExpanded & "'," 'Expanded
                        curConI = Me._ContainerList(node.Tag).ConnectionInfo
                        SaveConnectionFieldsSQL(curConI)

                        _sqlQuery.CommandText = Tools.Misc.PrepareForDB(_sqlQuery.CommandText)
                        _sqlQuery.ExecuteNonQuery()
                        '_parentConstantId = _currentNodeIndex
                        SaveNodesSQL(node.Nodes)
                        '_xmlTextWriter.WriteEndElement()
                    End If

                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                        _sqlQuery.CommandText &= "'" & False & "',"
                        curConI = Me._ConnectionList(node.Tag)
                        SaveConnectionFieldsSQL(curConI)
                        '_xmlTextWriter.WriteEndElement()
                        _sqlQuery.CommandText = Tools.Misc.PrepareForDB(_sqlQuery.CommandText)
                        _sqlQuery.ExecuteNonQuery()
                    End If

                    '_parentConstantId = 0
                Next
            End Sub

            Private Sub SaveConnectionFieldsSQL(ByVal curConI As Connection.Info)
                With curConI
                    _sqlQuery.CommandText &= "'" & PrepareValueForDB(.Description) & "',"
                    _sqlQuery.CommandText &= "'" & PrepareValueForDB(.Icon) & "',"
                    _sqlQuery.CommandText &= "'" & PrepareValueForDB(.Panel) & "',"

                    If Me._SaveSecurity.Username = True Then
                        _sqlQuery.CommandText &= "'" & PrepareValueForDB(.Username) & "',"
                    Else
                        _sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        _sqlQuery.CommandText &= "'" & PrepareValueForDB(.Domain) & "',"
                    Else
                        _sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Password = True Then
                        _sqlQuery.CommandText &= "'" & PrepareValueForDB(Security.Crypt.Encrypt(.Password, _password)) & "',"
                    Else
                        _sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    _sqlQuery.CommandText &= "'" & PrepareValueForDB(.Hostname) & "',"
                    _sqlQuery.CommandText &= "'" & .Protocol.ToString & "',"
                    _sqlQuery.CommandText &= "'" & PrepareValueForDB(.PuttySession) & "',"
                    _sqlQuery.CommandText &= "'" & .Port & "',"
                    _sqlQuery.CommandText &= "'" & .UseConsoleSession & "',"
                    _sqlQuery.CommandText &= "'" & .RenderingEngine.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .ICAEncryption.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .RDPAuthenticationLevel.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .Colors.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .Resolution.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .DisplayWallpaper & "',"
                    _sqlQuery.CommandText &= "'" & .DisplayThemes & "',"
                    _sqlQuery.CommandText &= "'" & .EnableFontSmoothing & "',"
                    _sqlQuery.CommandText &= "'" & .EnableDesktopComposition & "',"
                    _sqlQuery.CommandText &= "'" & .CacheBitmaps & "',"
                    _sqlQuery.CommandText &= "'" & .RedirectDiskDrives & "',"
                    _sqlQuery.CommandText &= "'" & .RedirectPorts & "',"
                    _sqlQuery.CommandText &= "'" & .RedirectPrinters & "',"
                    _sqlQuery.CommandText &= "'" & .RedirectSmartCards & "',"
                    _sqlQuery.CommandText &= "'" & .RedirectSound.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .RedirectKeys & "',"

                    If curConI.OpenConnections.Count > 0 Then
                        _sqlQuery.CommandText &= 1 & ","
                    Else
                        _sqlQuery.CommandText &= 0 & ","
                    End If

                    _sqlQuery.CommandText &= "'" & .PreExtApp & "',"
                    _sqlQuery.CommandText &= "'" & .PostExtApp & "',"
                    _sqlQuery.CommandText &= "'" & .MacAddress & "',"
                    _sqlQuery.CommandText &= "'" & .UserField & "',"
                    _sqlQuery.CommandText &= "'" & .ExtApp & "',"

                    _sqlQuery.CommandText &= "'" & .VNCCompression.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .VNCEncoding.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .VNCAuthMode.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .VNCProxyType.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .VNCProxyIP & "',"
                    _sqlQuery.CommandText &= "'" & .VNCProxyPort & "',"
                    _sqlQuery.CommandText &= "'" & .VNCProxyUsername & "',"
                    _sqlQuery.CommandText &= "'" & Security.Crypt.Encrypt(.VNCProxyPassword, _password) & "',"
                    _sqlQuery.CommandText &= "'" & .VNCColors.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .VNCSmartSizeMode.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .VNCViewOnly & "',"

                    _sqlQuery.CommandText &= "'" & .RDGatewayUsageMethod.ToString & "',"
                    _sqlQuery.CommandText &= "'" & .RDGatewayHostname & "',"
                    _sqlQuery.CommandText &= "'" & .RDGatewayUseConnectionCredentials.ToString & "',"

                    If Me._SaveSecurity.Username = True Then
                        _sqlQuery.CommandText &= "'" & .RDGatewayUsername & "',"
                    Else
                        _sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Password = True Then
                        _sqlQuery.CommandText &= "'" & .RDGatewayPassword & "',"
                    Else
                        _sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        _sqlQuery.CommandText &= "'" & .RDGatewayDomain & "',"
                    Else
                        _sqlQuery.CommandText &= "'" & "" & "',"
                    End If

                    With .Inherit
                        If Me._SaveSecurity.Inheritance = True Then
                            _sqlQuery.CommandText &= "'" & .CacheBitmaps & "',"
                            _sqlQuery.CommandText &= "'" & .Colors & "',"
                            _sqlQuery.CommandText &= "'" & .Description & "',"
                            _sqlQuery.CommandText &= "'" & .DisplayThemes & "',"
                            _sqlQuery.CommandText &= "'" & .DisplayWallpaper & "',"
                            _sqlQuery.CommandText &= "'" & .EnableFontSmoothing & "',"
                            _sqlQuery.CommandText &= "'" & .EnableDesktopComposition & "',"
                            _sqlQuery.CommandText &= "'" & .Domain & "',"
                            _sqlQuery.CommandText &= "'" & .Icon & "',"
                            _sqlQuery.CommandText &= "'" & .Panel & "',"
                            _sqlQuery.CommandText &= "'" & .Password & "',"
                            _sqlQuery.CommandText &= "'" & .Port & "',"
                            _sqlQuery.CommandText &= "'" & .Protocol & "',"
                            _sqlQuery.CommandText &= "'" & .PuttySession & "',"
                            _sqlQuery.CommandText &= "'" & .RedirectDiskDrives & "',"
                            _sqlQuery.CommandText &= "'" & .RedirectKeys & "',"
                            _sqlQuery.CommandText &= "'" & .RedirectPorts & "',"
                            _sqlQuery.CommandText &= "'" & .RedirectPrinters & "',"
                            _sqlQuery.CommandText &= "'" & .RedirectSmartCards & "',"
                            _sqlQuery.CommandText &= "'" & .RedirectSound & "',"
                            _sqlQuery.CommandText &= "'" & .Resolution & "',"
                            _sqlQuery.CommandText &= "'" & .UseConsoleSession & "',"
                            _sqlQuery.CommandText &= "'" & .RenderingEngine & "',"
                            _sqlQuery.CommandText &= "'" & .Username & "',"
                            _sqlQuery.CommandText &= "'" & .ICAEncryption & "',"
                            _sqlQuery.CommandText &= "'" & .RDPAuthenticationLevel & "',"
                            _sqlQuery.CommandText &= "'" & .PreExtApp & "',"
                            _sqlQuery.CommandText &= "'" & .PostExtApp & "',"
                            _sqlQuery.CommandText &= "'" & .MacAddress & "',"
                            _sqlQuery.CommandText &= "'" & .UserField & "',"
                            _sqlQuery.CommandText &= "'" & .ExtApp & "',"

                            _sqlQuery.CommandText &= "'" & .VNCCompression & "',"
                            _sqlQuery.CommandText &= "'" & .VNCEncoding & "',"
                            _sqlQuery.CommandText &= "'" & .VNCAuthMode & "',"
                            _sqlQuery.CommandText &= "'" & .VNCProxyType & "',"
                            _sqlQuery.CommandText &= "'" & .VNCProxyIP & "',"
                            _sqlQuery.CommandText &= "'" & .VNCProxyPort & "',"
                            _sqlQuery.CommandText &= "'" & .VNCProxyUsername & "',"
                            _sqlQuery.CommandText &= "'" & .VNCProxyPassword & "',"
                            _sqlQuery.CommandText &= "'" & .VNCColors & "',"
                            _sqlQuery.CommandText &= "'" & .VNCSmartSizeMode & "',"
                            _sqlQuery.CommandText &= "'" & .VNCViewOnly & "',"

                            _sqlQuery.CommandText &= "'" & .RDGatewayUsageMethod & "',"
                            _sqlQuery.CommandText &= "'" & .RDGatewayHostname & "',"
                            _sqlQuery.CommandText &= "'" & .RDGatewayUseConnectionCredentials & "',"
                            _sqlQuery.CommandText &= "'" & .RDGatewayUsername & "',"
                            _sqlQuery.CommandText &= "'" & .RDGatewayPassword & "',"
                            _sqlQuery.CommandText &= "'" & .RDGatewayDomain & "',"
                        Else
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"

                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"
                            _sqlQuery.CommandText &= "'" & False & "',"

                            _sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayUsageMethod
                            _sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayHostname
                            _sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayUseConnectionCredentials
                            _sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayUsername
                            _sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayPassword
                            _sqlQuery.CommandText &= "'" & False & "'," ' .RDGatewayDomain
                        End If
                    End With

                    .PositionID = _currentNodeIndex

                    If .IsContainer = False Then
                        If .Parent IsNot Nothing Then
                            _parentConstantId = TryCast(.Parent, Container.Info).ConnectionInfo.ConstantID
                        Else
                            _parentConstantId = 0
                        End If
                    Else
                        If TryCast(.Parent, Container.Info).Parent IsNot Nothing Then
                            _parentConstantId = TryCast(TryCast(.Parent, Container.Info).Parent, Container.Info).ConnectionInfo.ConstantID
                        Else
                            _parentConstantId = 0
                        End If
                    End If

                    _sqlQuery.CommandText &= _currentNodeIndex & "," & _parentConstantId & "," & .ConstantID & ",'" & Tools.Misc.DBDate(Now) & "')"
                End With
            End Sub
#End Region

#Region "XML"
            Private Sub EncryptCompleteFile()
                Dim streamReader As New StreamReader(ConnectionFileName)

                Dim fileContents As String
                fileContents = streamReader.ReadToEnd
                streamReader.Close()

                If Not String.IsNullOrEmpty(fileContents) Then
                    Dim streamWriter As New StreamWriter(ConnectionFileName)
                    streamWriter.Write(Security.Crypt.Encrypt(fileContents, _password))
                    streamWriter.Close()
                End If
            End Sub

            Private Sub SaveToXml()
                Try
                    If IsConnectionsFileLoaded = False Then Exit Sub

                    Dim treeNode As TreeNode
                    Dim isExport As Boolean = False

                    If Tree.Node.GetNodeType(RootTreeNode) = Tree.Node.Type.Root Then
                        treeNode = RootTreeNode.Clone
                    Else
                        treeNode = New TreeNode("mR|Export (" + Tools.Misc.DBDate(Now) + ")")
                        treeNode.Nodes.Add(RootTreeNode.Clone)
                        isExport = True
                    End If

                    Dim tempFileName As String = Path.GetTempFileName()
                    _xmlTextWriter = New XmlTextWriter(tempFileName, System.Text.Encoding.UTF8)

                    _xmlTextWriter.Formatting = Formatting.Indented
                    _xmlTextWriter.Indentation = 4

                    _xmlTextWriter.WriteStartDocument()

                    _xmlTextWriter.WriteStartElement("Connections") ' Do not localize
                    _xmlTextWriter.WriteAttributeString("Name", "", treeNode.Text)
                    _xmlTextWriter.WriteAttributeString("Export", "", isExport)

                    If isExport Then
                        _xmlTextWriter.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsNotProtected", _password))
                    Else
                        If TryCast(treeNode.Tag, Root.Info).Password = True Then
                            _password = TryCast(treeNode.Tag, Root.Info).PasswordString
                            _xmlTextWriter.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsProtected", _password))
                        Else
                            _xmlTextWriter.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsNotProtected", _password))
                        End If
                    End If

                    _xmlTextWriter.WriteAttributeString("ConfVersion", "", App.Info.Connections.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture))

                    Dim treeNodeCollection As TreeNodeCollection
                    treeNodeCollection = treeNode.Nodes

                    SaveNode(treeNodeCollection)

                    _xmlTextWriter.WriteEndElement()
                    _xmlTextWriter.Close()

                    Dim backupFileName As String = ConnectionFileName & ".backup"
                    File.Delete(backupFileName)
                    File.Move(ConnectionFileName, backupFileName)
                    File.Move(tempFileName, ConnectionFileName)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveToXml failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SaveNode(ByVal tNC As TreeNodeCollection)
                Try
                    For Each node As TreeNode In tNC
                        Dim curConI As Connection.Info

                        If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Or Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then
                            _xmlTextWriter.WriteStartElement("Node")
                            _xmlTextWriter.WriteAttributeString("Name", "", node.Text)
                            _xmlTextWriter.WriteAttributeString("Type", "", Tree.Node.GetNodeType(node).ToString)
                        End If

                        If Tree.Node.GetNodeType(node) = Tree.Node.Type.Container Then 'container
                            _xmlTextWriter.WriteAttributeString("Expanded", "", Me._ContainerList(node.Tag).TreeNode.IsExpanded)
                            curConI = Me._ContainerList(node.Tag).ConnectionInfo
                            SaveConnectionFields(curConI)
                            SaveNode(node.Nodes)
                            _xmlTextWriter.WriteEndElement()
                        End If

                        If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                            curConI = Me._ConnectionList(node.Tag)
                            SaveConnectionFields(curConI)
                            _xmlTextWriter.WriteEndElement()
                        End If
                    Next
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveNode failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub SaveConnectionFields(ByVal curConI As Connection.Info)
                Try
                    _xmlTextWriter.WriteAttributeString("Descr", "", curConI.Description)

                    _xmlTextWriter.WriteAttributeString("Icon", "", curConI.Icon)

                    _xmlTextWriter.WriteAttributeString("Panel", "", curConI.Panel)

                    If Me._SaveSecurity.Username = True Then
                        _xmlTextWriter.WriteAttributeString("Username", "", curConI.Username)
                    Else
                        _xmlTextWriter.WriteAttributeString("Username", "", "")
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        _xmlTextWriter.WriteAttributeString("Domain", "", curConI.Domain)
                    Else
                        _xmlTextWriter.WriteAttributeString("Domain", "", "")
                    End If

                    If Me._SaveSecurity.Password = True Then
                        _xmlTextWriter.WriteAttributeString("Password", "", Security.Crypt.Encrypt(curConI.Password, _password))
                    Else
                        _xmlTextWriter.WriteAttributeString("Password", "", "")
                    End If

                    _xmlTextWriter.WriteAttributeString("Hostname", "", curConI.Hostname)

                    _xmlTextWriter.WriteAttributeString("Protocol", "", curConI.Protocol.ToString)

                    _xmlTextWriter.WriteAttributeString("PuttySession", "", curConI.PuttySession)

                    _xmlTextWriter.WriteAttributeString("Port", "", curConI.Port)

                    _xmlTextWriter.WriteAttributeString("ConnectToConsole", "", curConI.UseConsoleSession)

                    _xmlTextWriter.WriteAttributeString("RenderingEngine", "", curConI.RenderingEngine.ToString)

                    _xmlTextWriter.WriteAttributeString("ICAEncryptionStrength", "", curConI.ICAEncryption.ToString)

                    _xmlTextWriter.WriteAttributeString("RDPAuthenticationLevel", "", curConI.RDPAuthenticationLevel.ToString)

                    _xmlTextWriter.WriteAttributeString("Colors", "", curConI.Colors.ToString)

                    _xmlTextWriter.WriteAttributeString("Resolution", "", curConI.Resolution.ToString)

                    _xmlTextWriter.WriteAttributeString("DisplayWallpaper", "", curConI.DisplayWallpaper)

                    _xmlTextWriter.WriteAttributeString("DisplayThemes", "", curConI.DisplayThemes)

                    _xmlTextWriter.WriteAttributeString("EnableFontSmoothing", "", curConI.EnableFontSmoothing)

                    _xmlTextWriter.WriteAttributeString("EnableDesktopComposition", "", curConI.EnableDesktopComposition)

                    _xmlTextWriter.WriteAttributeString("CacheBitmaps", "", curConI.CacheBitmaps)

                    _xmlTextWriter.WriteAttributeString("RedirectDiskDrives", "", curConI.RedirectDiskDrives)

                    _xmlTextWriter.WriteAttributeString("RedirectPorts", "", curConI.RedirectPorts)

                    _xmlTextWriter.WriteAttributeString("RedirectPrinters", "", curConI.RedirectPrinters)

                    _xmlTextWriter.WriteAttributeString("RedirectSmartCards", "", curConI.RedirectSmartCards)

                    _xmlTextWriter.WriteAttributeString("RedirectSound", "", curConI.RedirectSound.ToString)

                    _xmlTextWriter.WriteAttributeString("RedirectKeys", "", curConI.RedirectKeys)

                    If curConI.OpenConnections.Count > 0 Then
                        _xmlTextWriter.WriteAttributeString("Connected", "", True)
                    Else
                        _xmlTextWriter.WriteAttributeString("Connected", "", False)
                    End If

                    _xmlTextWriter.WriteAttributeString("PreExtApp", "", curConI.PreExtApp)
                    _xmlTextWriter.WriteAttributeString("PostExtApp", "", curConI.PostExtApp)
                    _xmlTextWriter.WriteAttributeString("MacAddress", "", curConI.MacAddress)
                    _xmlTextWriter.WriteAttributeString("UserField", "", curConI.UserField)
                    _xmlTextWriter.WriteAttributeString("ExtApp", "", curConI.ExtApp)

                    _xmlTextWriter.WriteAttributeString("VNCCompression", "", curConI.VNCCompression.ToString)
                    _xmlTextWriter.WriteAttributeString("VNCEncoding", "", curConI.VNCEncoding.ToString)
                    _xmlTextWriter.WriteAttributeString("VNCAuthMode", "", curConI.VNCAuthMode.ToString)
                    _xmlTextWriter.WriteAttributeString("VNCProxyType", "", curConI.VNCProxyType.ToString)
                    _xmlTextWriter.WriteAttributeString("VNCProxyIP", "", curConI.VNCProxyIP)
                    _xmlTextWriter.WriteAttributeString("VNCProxyPort", "", curConI.VNCProxyPort)
                    _xmlTextWriter.WriteAttributeString("VNCProxyUsername", "", curConI.VNCProxyUsername)
                    _xmlTextWriter.WriteAttributeString("VNCProxyPassword", "", Security.Crypt.Encrypt(curConI.VNCProxyPassword, _password))
                    _xmlTextWriter.WriteAttributeString("VNCColors", "", curConI.VNCColors.ToString)
                    _xmlTextWriter.WriteAttributeString("VNCSmartSizeMode", "", curConI.VNCSmartSizeMode.ToString)
                    _xmlTextWriter.WriteAttributeString("VNCViewOnly", "", curConI.VNCViewOnly)

                    _xmlTextWriter.WriteAttributeString("RDGatewayUsageMethod", "", curConI.RDGatewayUsageMethod.ToString)
                    _xmlTextWriter.WriteAttributeString("RDGatewayHostname", "", curConI.RDGatewayHostname)

                    _xmlTextWriter.WriteAttributeString("RDGatewayUseConnectionCredentials", "", curConI.RDGatewayUseConnectionCredentials.ToString)

                    If Me._SaveSecurity.Username = True Then
                        _xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", curConI.RDGatewayUsername)
                    Else
                        _xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", "")
                    End If

                    If Me._SaveSecurity.Password = True Then
                        _xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", curConI.RDGatewayPassword)
                    Else
                        _xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", "")
                    End If

                    If Me._SaveSecurity.Domain = True Then
                        _xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", curConI.RDGatewayDomain)
                    Else
                        _xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", "")
                    End If

                    If Me._SaveSecurity.Inheritance = True Then
                        _xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", curConI.Inherit.CacheBitmaps)
                        _xmlTextWriter.WriteAttributeString("InheritColors", "", curConI.Inherit.Colors)
                        _xmlTextWriter.WriteAttributeString("InheritDescription", "", curConI.Inherit.Description)
                        _xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", curConI.Inherit.DisplayThemes)
                        _xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", curConI.Inherit.DisplayWallpaper)
                        _xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", curConI.Inherit.EnableFontSmoothing)
                        _xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", curConI.Inherit.EnableDesktopComposition)
                        _xmlTextWriter.WriteAttributeString("InheritDomain", "", curConI.Inherit.Domain)
                        _xmlTextWriter.WriteAttributeString("InheritIcon", "", curConI.Inherit.Icon)
                        _xmlTextWriter.WriteAttributeString("InheritPanel", "", curConI.Inherit.Panel)
                        _xmlTextWriter.WriteAttributeString("InheritPassword", "", curConI.Inherit.Password)
                        _xmlTextWriter.WriteAttributeString("InheritPort", "", curConI.Inherit.Port)
                        _xmlTextWriter.WriteAttributeString("InheritProtocol", "", curConI.Inherit.Protocol)
                        _xmlTextWriter.WriteAttributeString("InheritPuttySession", "", curConI.Inherit.PuttySession)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", curConI.Inherit.RedirectDiskDrives)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", curConI.Inherit.RedirectKeys)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", curConI.Inherit.RedirectPorts)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", curConI.Inherit.RedirectPrinters)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", curConI.Inherit.RedirectSmartCards)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", curConI.Inherit.RedirectSound)
                        _xmlTextWriter.WriteAttributeString("InheritResolution", "", curConI.Inherit.Resolution)
                        _xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", curConI.Inherit.UseConsoleSession)
                        _xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", curConI.Inherit.RenderingEngine)
                        _xmlTextWriter.WriteAttributeString("InheritUsername", "", curConI.Inherit.Username)
                        _xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", curConI.Inherit.ICAEncryption)
                        _xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", curConI.Inherit.RDPAuthenticationLevel)
                        _xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", curConI.Inherit.PreExtApp)
                        _xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", curConI.Inherit.PostExtApp)
                        _xmlTextWriter.WriteAttributeString("InheritMacAddress", "", curConI.Inherit.MacAddress)
                        _xmlTextWriter.WriteAttributeString("InheritUserField", "", curConI.Inherit.UserField)
                        _xmlTextWriter.WriteAttributeString("InheritExtApp", "", curConI.Inherit.ExtApp)
                        _xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", curConI.Inherit.VNCCompression)
                        _xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", curConI.Inherit.VNCEncoding)
                        _xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", curConI.Inherit.VNCAuthMode)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", curConI.Inherit.VNCProxyType)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", curConI.Inherit.VNCProxyIP)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", curConI.Inherit.VNCProxyPort)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", curConI.Inherit.VNCProxyUsername)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", curConI.Inherit.VNCProxyPassword)
                        _xmlTextWriter.WriteAttributeString("InheritVNCColors", "", curConI.Inherit.VNCColors)
                        _xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", curConI.Inherit.VNCSmartSizeMode)
                        _xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", curConI.Inherit.VNCViewOnly)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayUsageMethod", "", curConI.Inherit.RDGatewayUsageMethod)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", curConI.Inherit.RDGatewayHostname)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", curConI.Inherit.RDGatewayUseConnectionCredentials)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", curConI.Inherit.RDGatewayUsername)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", curConI.Inherit.RDGatewayPassword)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", curConI.Inherit.RDGatewayDomain)
                    Else
                        _xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritColors", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritDescription", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritDomain", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritIcon", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritPanel", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritPassword", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritPort", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritProtocol", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritPuttySession", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritResolution", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritUsername", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritMacAddress", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritUserField", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritExtApp", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCColors", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", False)
                        _xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", False)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveConnectionFields failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "CSV"
            Private csvWr As StreamWriter

            Private Sub SaveTomRCSV()
                If App.Runtime.IsConnectionsFileLoaded = False Then
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
                If App.Runtime.IsConnectionsFileLoaded = False Then
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
                If App.Runtime.IsConnectionsFileLoaded = False Then
                    Exit Sub
                End If

                Dim tN As TreeNode
                tN = RootTreeNode.Clone

                Dim tNC As TreeNodeCollection
                tNC = tN.Nodes

                _xmlTextWriter = New XmlTextWriter(ConnectionFileName, System.Text.Encoding.UTF8)
                _xmlTextWriter.Formatting = Formatting.Indented
                _xmlTextWriter.Indentation = 4

                _xmlTextWriter.WriteStartDocument()

                _xmlTextWriter.WriteStartElement("vRDConfig")
                _xmlTextWriter.WriteAttributeString("Version", "", "2.0")

                _xmlTextWriter.WriteStartElement("Connections")
                SaveNodeVRE(tNC)
                _xmlTextWriter.WriteEndElement()

                _xmlTextWriter.WriteEndElement()
                _xmlTextWriter.WriteEndDocument()
                _xmlTextWriter.Close()
            End Sub

            Private Sub SaveNodeVRE(ByVal tNC As TreeNodeCollection)
                For Each node As TreeNode In tNC
                    If Tree.Node.GetNodeType(node) = Tree.Node.Type.Connection Then
                        Dim curConI As Connection.Info = node.Tag

                        If curConI.Protocol = Connection.Protocol.Protocols.RDP Then
                            _xmlTextWriter.WriteStartElement("Connection")
                            _xmlTextWriter.WriteAttributeString("Id", "", "")

                            WriteVREitem(curConI)

                            _xmlTextWriter.WriteEndElement()
                        End If
                    Else
                        SaveNodeVRE(node.Nodes)
                    End If
                Next
            End Sub

            Private Sub WriteVREitem(ByVal con As Connection.Info)
                'Name
                _xmlTextWriter.WriteStartElement("ConnectionName")
                _xmlTextWriter.WriteValue(con.Name)
                _xmlTextWriter.WriteEndElement()

                'Hostname
                _xmlTextWriter.WriteStartElement("ServerName")
                _xmlTextWriter.WriteValue(con.Hostname)
                _xmlTextWriter.WriteEndElement()

                'Mac Adress
                _xmlTextWriter.WriteStartElement("MACAddress")
                _xmlTextWriter.WriteValue(con.MacAddress)
                _xmlTextWriter.WriteEndElement()

                'Management Board URL
                _xmlTextWriter.WriteStartElement("MgmtBoardUrl")
                _xmlTextWriter.WriteValue("")
                _xmlTextWriter.WriteEndElement()

                'Description
                _xmlTextWriter.WriteStartElement("Description")
                _xmlTextWriter.WriteValue(con.Description)
                _xmlTextWriter.WriteEndElement()

                'Port
                _xmlTextWriter.WriteStartElement("Port")
                _xmlTextWriter.WriteValue(con.Port)
                _xmlTextWriter.WriteEndElement()

                'Console Session
                _xmlTextWriter.WriteStartElement("Console")
                _xmlTextWriter.WriteValue(con.UseConsoleSession)
                _xmlTextWriter.WriteEndElement()

                'Redirect Clipboard
                _xmlTextWriter.WriteStartElement("ClipBoard")
                _xmlTextWriter.WriteValue(True)
                _xmlTextWriter.WriteEndElement()

                'Redirect Printers
                _xmlTextWriter.WriteStartElement("Printer")
                _xmlTextWriter.WriteValue(con.RedirectPrinters)
                _xmlTextWriter.WriteEndElement()

                'Redirect Ports
                _xmlTextWriter.WriteStartElement("Serial")
                _xmlTextWriter.WriteValue(con.RedirectPorts)
                _xmlTextWriter.WriteEndElement()

                'Redirect Disks
                _xmlTextWriter.WriteStartElement("LocalDrives")
                _xmlTextWriter.WriteValue(con.RedirectDiskDrives)
                _xmlTextWriter.WriteEndElement()

                'Redirect Smartcards
                _xmlTextWriter.WriteStartElement("SmartCard")
                _xmlTextWriter.WriteValue(con.RedirectSmartCards)
                _xmlTextWriter.WriteEndElement()

                'Connection Place
                _xmlTextWriter.WriteStartElement("ConnectionPlace")
                _xmlTextWriter.WriteValue("2") '----------------------------------------------------------
                _xmlTextWriter.WriteEndElement()

                'Smart Size
                _xmlTextWriter.WriteStartElement("AutoSize")
                _xmlTextWriter.WriteValue(IIf(con.Resolution = Connection.Protocol.RDP.RDPResolutions.SmartSize, True, False))
                _xmlTextWriter.WriteEndElement()

                'SeparateResolutionX
                _xmlTextWriter.WriteStartElement("SeparateResolutionX")
                _xmlTextWriter.WriteValue("1024")
                _xmlTextWriter.WriteEndElement()

                'SeparateResolutionY
                _xmlTextWriter.WriteStartElement("SeparateResolutionY")
                _xmlTextWriter.WriteValue("768")
                _xmlTextWriter.WriteEndElement()

                'TabResolutionX
                _xmlTextWriter.WriteStartElement("TabResolutionX")
                If con.Resolution <> Connection.Protocol.RDP.RDPResolutions.FitToWindow And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.Fullscreen And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.SmartSize Then
                    _xmlTextWriter.WriteValue(con.Resolution.ToString.Remove(con.Resolution.ToString.IndexOf("x")))
                Else
                    _xmlTextWriter.WriteValue("1024")
                End If
                _xmlTextWriter.WriteEndElement()

                'TabResolutionY
                _xmlTextWriter.WriteStartElement("TabResolutionY")
                If con.Resolution <> Connection.Protocol.RDP.RDPResolutions.FitToWindow And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.Fullscreen And _
                con.Resolution <> Connection.Protocol.RDP.RDPResolutions.SmartSize Then
                    _xmlTextWriter.WriteValue(con.Resolution.ToString.Remove(0, con.Resolution.ToString.IndexOf("x")))
                Else
                    _xmlTextWriter.WriteValue("768")
                End If
                _xmlTextWriter.WriteEndElement()

                'RDPColorDepth
                _xmlTextWriter.WriteStartElement("RDPColorDepth")
                _xmlTextWriter.WriteValue(con.Colors.ToString.Replace("Colors", "").Replace("Bit", ""))
                _xmlTextWriter.WriteEndElement()

                'Bitmap Caching
                _xmlTextWriter.WriteStartElement("BitmapCaching")
                _xmlTextWriter.WriteValue(con.CacheBitmaps)
                _xmlTextWriter.WriteEndElement()

                'Themes
                _xmlTextWriter.WriteStartElement("Themes")
                _xmlTextWriter.WriteValue(con.DisplayThemes)
                _xmlTextWriter.WriteEndElement()

                'Wallpaper
                _xmlTextWriter.WriteStartElement("Wallpaper")
                _xmlTextWriter.WriteValue(con.DisplayWallpaper)
                _xmlTextWriter.WriteEndElement()
            End Sub
#End Region
        End Class
    End Namespace
End Namespace