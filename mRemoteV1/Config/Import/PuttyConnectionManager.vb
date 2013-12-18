Imports System.Xml
Imports System.IO
Imports mRemoteNG.App.Runtime
Imports mRemoteNG.Connection.Protocol

Namespace Config.Import
    Public Class PuttyConnectionManager
        Public Shared Sub Import(ByVal fileName As String, ByVal parentTreeNode As TreeNode)
            Dim xmlDocument As New XmlDocument()
            xmlDocument.Load(fileName)

            Dim configurationNode As XmlNode = xmlDocument.SelectSingleNode("/configuration")
            'Dim version As New Version(configurationNode.Attributes("version").Value)
            'If Not version = New Version(0, 7, 1, 136) Then
            '    Throw New FileFormatException(String.Format("Unsupported file version ({0}).", version))
            'End If

            For Each rootNode As XmlNode In configurationNode.SelectNodes("./root")
                ImportRootOrContainer(rootNode, parentTreeNode)
            Next
        End Sub

        Private Shared Sub ImportRootOrContainer(ByVal xmlNode As XmlNode, ByVal parentTreeNode As TreeNode)
            Dim xmlNodeType As String = xmlNode.Attributes("type").Value
            Select Case xmlNode.Name
                Case "root"
                    If Not String.Compare(xmlNodeType, "database", ignoreCase:=True) = 0 Then
                        Throw New FileFormatException(String.Format("Unrecognized root node type ({0}).", xmlNodeType))
                    End If
                Case "container"
                    If Not String.Compare(xmlNodeType, "folder", ignoreCase:=True) = 0 Then
                        Throw New FileFormatException(String.Format("Unrecognized root node type ({0}).", xmlNodeType))
                    End If
                Case Else
                    ' ReSharper disable once LocalizableElement
                    Throw New ArgumentException("Argument must be either a root or a container node.", "xmlNode")
            End Select

            If parentTreeNode Is Nothing Then
                Throw New InvalidOperationException("parentInfo.TreeNode must not be null.")
            End If

            Dim name As String = xmlNode.Attributes("name").Value

            Dim treeNode As New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim containerInfo As New Container.Info
            containerInfo.TreeNode = treeNode
            containerInfo.Name = name

            Dim connectionInfo As Connection.Info = CreateConnectionInfo(name)
            connectionInfo.Parent = containerInfo
            connectionInfo.IsContainer = True
            containerInfo.ConnectionInfo = connectionInfo

            ' We can only inherit from a container node, not the root node or connection nodes
            If Tree.Node.GetNodeType(parentTreeNode) = Tree.Node.Type.Container Then
                containerInfo.Parent = parentTreeNode.Tag
            Else
                connectionInfo.Inherit.TurnOffInheritanceCompletely()
            End If

            treeNode.Name = name
            treeNode.Tag = containerInfo
            treeNode.ImageIndex = Images.Enums.TreeImage.Container
            treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container

            For Each childNode As XmlNode In xmlNode.SelectNodes("./*")
                Select Case childNode.Name
                    Case "container"
                        ImportRootOrContainer(childNode, treeNode)
                    Case "connection"
                        ImportConnection(childNode, treeNode)
                    Case Else
                        Throw New FileFormatException(String.Format("Unrecognized child node ({0}).", childNode.Name))
                End Select
            Next

            containerInfo.IsExpanded = xmlNode.Attributes("expanded").InnerText
            If containerInfo.IsExpanded Then treeNode.Expand()

            ContainerList.Add(containerInfo)
        End Sub

        Private Shared Sub ImportConnection(ByVal connectionNode As XmlNode, ByVal parentTreeNode As TreeNode)
            Dim connectionNodeType As String = connectionNode.Attributes("type").Value
            If Not String.Compare(connectionNodeType, "PuTTY", ignoreCase:=True) = 0 Then
                Throw New FileFormatException(String.Format("Unrecognized connection node type ({0}).", connectionNodeType))
            End If

            Dim name As String = connectionNode.Attributes("name").Value
            Dim treeNode As New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim connectionInfo As Connection.Info = ConnectionInfoFromXml(connectionNode)
            connectionInfo.TreeNode = treeNode
            connectionInfo.Parent = parentTreeNode.Tag

            treeNode.Name = name
            treeNode.Tag = connectionInfo
            treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
            treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed

            ConnectionList.Add(connectionInfo)
        End Sub

        Private Shared Function CreateConnectionInfo(ByVal name As String) As Connection.Info
            Dim connectionInfo As New Connection.Info
            connectionInfo.Inherit = New Connection.Info.Inheritance(connectionInfo)
            connectionInfo.Name = name
            Return connectionInfo
        End Function

        Private Shared Function ConnectionInfoFromXml(ByVal xmlNode As XmlNode) As Connection.Info
            Dim connectionInfoNode As XmlNode = xmlNode.SelectSingleNode("./connection_info")

            Dim name As String = connectionInfoNode.SelectSingleNode("./name").InnerText
            Dim connectionInfo As Connection.Info = CreateConnectionInfo(name)

            Dim protocol As String = connectionInfoNode.SelectSingleNode("./protocol").InnerText
            Select Case protocol.ToLowerInvariant()
                Case "telnet"
                    connectionInfo.Protocol = Protocols.Telnet
                Case "ssh"
                    connectionInfo.Protocol = Protocols.SSH2
                Case Else
                    Throw New FileFormatException(String.Format("Unrecognized protocol ({0}).", protocol))
            End Select

            connectionInfo.Hostname = connectionInfoNode.SelectSingleNode("./host").InnerText
            connectionInfo.Port = connectionInfoNode.SelectSingleNode("./port").InnerText
            connectionInfo.PuttySession = connectionInfoNode.SelectSingleNode("./session").InnerText
            ' ./commandline
            connectionInfo.Description = connectionInfoNode.SelectSingleNode("./description").InnerText

            Dim loginNode As XmlNode = xmlNode.SelectSingleNode("./login")
            connectionInfo.Username = loginNode.SelectSingleNode("login").InnerText
            connectionInfo.Password = loginNode.SelectSingleNode("password").InnerText
            ' ./prompt

            ' ./timeout/connectiontimeout
            ' ./timeout/logintimeout
            ' ./timeout/passwordtimeout
            ' ./timeout/commandtimeout

            ' ./command/command1
            ' ./command/command2
            ' ./command/command3
            ' ./command/command4
            ' ./command/command5

            ' ./options/loginmacro
            ' ./options/postcommands
            ' ./options/endlinechar

            Return connectionInfo
        End Function
    End Class
End Namespace