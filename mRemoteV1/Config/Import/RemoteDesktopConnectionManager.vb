Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Xml
Imports mRemote3G.App
Imports mRemote3G.Connection
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Images
Imports mRemote3G.Tools
Imports mRemote3G.Tree

Namespace Config.Import
    Public Class RemoteDesktopConnectionManager
        Public Shared Sub Import(fileName As String, parentTreeNode As TreeNode)
            Dim xmlDocument As New XmlDocument()
            xmlDocument.Load(fileName)

            Dim rdcManNode As XmlNode = xmlDocument.SelectSingleNode("/RDCMan")
            Dim schemaVersion As Integer = rdcManNode.Attributes("schemaVersion").Value
            If Not schemaVersion = 1 Then
                Throw New FileFormatException(String.Format("Unsupported schema version ({0}).", schemaVersion))
            End If

            Dim versionNode As XmlNode = rdcManNode.SelectSingleNode("./version")
            Dim version As New Version(versionNode.InnerText)
            If Not version = New Version(2, 2) Then
                Throw New FileFormatException(String.Format("Unsupported file version ({0}).", version))
            End If

            Dim fileNode As XmlNode = rdcManNode.SelectSingleNode("./file")
            ImportFileOrGroup(fileNode, parentTreeNode)
        End Sub

        Private Shared Sub ImportFileOrGroup(xmlNode As XmlNode, parentTreeNode As TreeNode)
            Dim propertiesNode As XmlNode = xmlNode.SelectSingleNode("./properties")
            Dim name As String = propertiesNode.SelectSingleNode("./name").InnerText

            Dim treeNode As New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim containerInfo As New Container.Info
            containerInfo.TreeNode = treeNode
            containerInfo.Name = name

            Dim connectionInfo As Info = ConnectionInfoFromXml(propertiesNode)
            connectionInfo.Parent = containerInfo
            connectionInfo.IsContainer = True
            containerInfo.ConnectionInfo = connectionInfo

            ' We can only inherit from a container node, not the root node or connection nodes
            If Node.GetNodeType(parentTreeNode) = Node.Type.Container Then
                containerInfo.Parent = parentTreeNode.Tag
            Else
                connectionInfo.Inherit.TurnOffInheritanceCompletely()
            End If

            treeNode.Name = name
            treeNode.Tag = containerInfo
            treeNode.ImageIndex = Enums.TreeImage.Container
            treeNode.SelectedImageIndex = Enums.TreeImage.Container

            For Each childNode As XmlNode In xmlNode.SelectNodes("./group|./server")
                Select Case childNode.Name
                    Case "group"
                        ImportFileOrGroup(childNode, treeNode)
                    Case "server"
                        ImportServer(childNode, treeNode)
                End Select
            Next

            containerInfo.IsExpanded = propertiesNode.SelectSingleNode("./expanded").InnerText
            If containerInfo.IsExpanded Then treeNode.Expand()

            Runtime.ContainerList.Add(containerInfo)
        End Sub

        Private Shared Sub ImportServer(serverNode As XmlNode, parentTreeNode As TreeNode)
            Dim name As String = serverNode.SelectSingleNode("./displayName").InnerText
            Dim treeNode As New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim connectionInfo As Info = ConnectionInfoFromXml(serverNode)
            connectionInfo.TreeNode = treeNode
            connectionInfo.Parent = parentTreeNode.Tag

            treeNode.Name = name
            treeNode.Tag = connectionInfo
            treeNode.ImageIndex = Enums.TreeImage.ConnectionClosed
            treeNode.SelectedImageIndex = Enums.TreeImage.ConnectionClosed

            Runtime.ConnectionList.Add(connectionInfo)
        End Sub

        Private Shared Function ConnectionInfoFromXml(xmlNode As XmlNode) As Info
            Dim connectionInfo As New Info
            connectionInfo.Inherit = New Info.Inheritance(connectionInfo)

            Dim name As String = xmlNode.SelectSingleNode("./name").InnerText

            Dim displayName As String
            Dim displayNameNode As XmlNode = xmlNode.SelectSingleNode("./displayName")
            If displayNameNode Is Nothing Then
                displayName = name
            Else
                displayName = displayNameNode.InnerText
            End If

            connectionInfo.Name = displayName
            connectionInfo.Description = xmlNode.SelectSingleNode("./comment").InnerText
            connectionInfo.Hostname = name

            Dim logonCredentialsNode As XmlNode = xmlNode.SelectSingleNode("./logonCredentials")
            If logonCredentialsNode.Attributes("inherit").Value = "None" Then
                connectionInfo.Username = logonCredentialsNode.SelectSingleNode("userName").InnerText

                Dim passwordNode As XmlNode = logonCredentialsNode.SelectSingleNode("./password")
                If passwordNode.Attributes("storeAsClearText").Value = "True" Then
                    connectionInfo.Password = passwordNode.InnerText
                Else
                    connectionInfo.Password = DecryptPassword(passwordNode.InnerText)
                End If

                connectionInfo.Domain = logonCredentialsNode.SelectSingleNode("./domain").InnerText
            Else
                connectionInfo.Inherit.Username = True
                connectionInfo.Inherit.Password = True
                connectionInfo.Inherit.Domain = True
            End If

            Dim connectionSettingsNode As XmlNode = xmlNode.SelectSingleNode("./connectionSettings")
            If connectionSettingsNode.Attributes("inherit").Value = "None" Then
                connectionInfo.UseConsoleSession =
                    connectionSettingsNode.SelectSingleNode("./connectToConsole").InnerText
                ' ./startProgram
                ' ./workingDir
                connectionInfo.Port = connectionSettingsNode.SelectSingleNode("./port").InnerText
            Else
                connectionInfo.Inherit.UseConsoleSession = True
                connectionInfo.Inherit.Port = True
            End If

            Dim gatewaySettingsNode As XmlNode = xmlNode.SelectSingleNode("./gatewaySettings")
            If gatewaySettingsNode.Attributes("inherit").Value = "None" Then
                If gatewaySettingsNode.SelectSingleNode("./enabled").InnerText = "True" Then
                    connectionInfo.RDGatewayUsageMethod = RDP.RDGatewayUsageMethod.Always
                Else
                    connectionInfo.RDGatewayUsageMethod = RDP.RDGatewayUsageMethod.Never
                End If

                connectionInfo.RDGatewayHostname = gatewaySettingsNode.SelectSingleNode("./hostName").InnerText
                connectionInfo.RDGatewayUsername = gatewaySettingsNode.SelectSingleNode("./userName").InnerText

                Dim passwordNode As XmlNode = logonCredentialsNode.SelectSingleNode("./password")
                If passwordNode.Attributes("storeAsClearText").Value = "True" Then
                    connectionInfo.RDGatewayPassword = passwordNode.InnerText
                Else
                    connectionInfo.Password = DecryptPassword(passwordNode.InnerText)
                End If

                connectionInfo.RDGatewayDomain = gatewaySettingsNode.SelectSingleNode("./domain").InnerText
                ' ./logonMethod
                ' ./localBypass
                ' ./credSharing
            Else
                connectionInfo.Inherit.RDGatewayUsageMethod = True
                connectionInfo.Inherit.RDGatewayHostname = True
                connectionInfo.Inherit.RDGatewayUsername = True
                connectionInfo.Inherit.RDGatewayPassword = True
                connectionInfo.Inherit.RDGatewayDomain = True
            End If

            Dim remoteDesktopNode As XmlNode = xmlNode.SelectSingleNode("./remoteDesktop")
            If remoteDesktopNode.Attributes("inherit").Value = "None" Then
                Dim resolutionString As String = remoteDesktopNode.SelectSingleNode("./size").InnerText.Replace(" ", "")
                Try
                    connectionInfo.Resolution = "Res" & Misc.StringToEnum(GetType(RDP.RDPResolutions), resolutionString)
                Catch ex As ArgumentException
                    connectionInfo.Resolution = RDP.RDPResolutions.FitToWindow
                End Try

                If remoteDesktopNode.SelectSingleNode("./sameSizeAsClientArea").InnerText = "True" Then
                    connectionInfo.Resolution = RDP.RDPResolutions.FitToWindow
                End If

                If remoteDesktopNode.SelectSingleNode("./fullScreen").InnerText = "True" Then
                    connectionInfo.Resolution = RDP.RDPResolutions.Fullscreen
                End If

                connectionInfo.Colors = remoteDesktopNode.SelectSingleNode("./colorDepth").InnerText
            Else
                connectionInfo.Inherit.Resolution = True
                connectionInfo.Inherit.Colors = True
            End If

            Dim localResourcesNode As XmlNode = xmlNode.SelectSingleNode("./localResources")
            If localResourcesNode.Attributes("inherit").Value = "None" Then
                Select Case localResourcesNode.SelectSingleNode("./audioRedirection").InnerText
                    Case 0 ' Bring to this computer
                        connectionInfo.RedirectSound = RDP.RDPSounds.BringToThisComputer
                    Case 1 ' Leave at remote computer
                        connectionInfo.RedirectSound = RDP.RDPSounds.LeaveAtRemoteComputer
                    Case 2 ' Do not play
                        connectionInfo.RedirectSound = RDP.RDPSounds.DoNotPlay
                End Select

                ' ./audioRedirectionQuality
                ' ./audioCaptureRedirection

                Select Case localResourcesNode.SelectSingleNode("./keyboardHook").InnerText
                    Case 0 ' On the local computer
                        connectionInfo.RedirectKeys = False
                    Case 1 ' On the remote computer
                        connectionInfo.RedirectKeys = True
                    Case 2 ' In full screen mode only
                        connectionInfo.RedirectKeys = False
                End Select

                ' ./redirectClipboard
                connectionInfo.RedirectDiskDrives = localResourcesNode.SelectSingleNode("./redirectDrives").InnerText
                connectionInfo.RedirectPorts = localResourcesNode.SelectSingleNode("./redirectPorts").InnerText
                connectionInfo.RedirectPrinters = localResourcesNode.SelectSingleNode("./redirectPrinters").InnerText
                connectionInfo.RedirectSmartCards =
                    localResourcesNode.SelectSingleNode("./redirectSmartCards").InnerText
            Else
                connectionInfo.Inherit.RedirectSound = True
                connectionInfo.Inherit.RedirectKeys = True
                connectionInfo.Inherit.RedirectDiskDrives = True
                connectionInfo.Inherit.RedirectPorts = True
                connectionInfo.Inherit.RedirectPrinters = True
                connectionInfo.Inherit.RedirectSmartCards = True
            End If

            Dim securitySettingsNode As XmlNode = xmlNode.SelectSingleNode("./securitySettings")
            If securitySettingsNode.Attributes("inherit").Value = "None" Then
                Select Case securitySettingsNode.SelectSingleNode("./authentication").InnerText
                    Case 0 ' No authentication
                        connectionInfo.RDPAuthenticationLevel = RDP.AuthenticationLevel.NoAuth
                    Case 1 ' Do not connect if authentication fails
                        connectionInfo.RDPAuthenticationLevel = RDP.AuthenticationLevel.AuthRequired
                    Case 2 ' Warn if authentication fails
                        connectionInfo.RDPAuthenticationLevel = RDP.AuthenticationLevel.WarnOnFailedAuth
                End Select
            Else
                connectionInfo.Inherit.RDPAuthenticationLevel = True
            End If

            ' ./displaySettings/thumbnailScale
            ' ./displaySettings/liveThumbnailUpdates
            ' ./displaySettings/showDisconnectedThumbnails

            Return connectionInfo
        End Function

        Private Shared Function DecryptPassword(ciphertext As String) As String
            If String.IsNullOrEmpty(ciphertext) Then Return Nothing

            Dim gcHandle As GCHandle
            Dim plaintextData As Win32.DATA_BLOB
            Try
                Dim ciphertextArray As Byte() = Convert.FromBase64String(ciphertext)
                gcHandle = GCHandle.Alloc(ciphertextArray, GCHandleType.Pinned)

                Dim ciphertextData As Win32.DATA_BLOB
                ciphertextData.cbData = ciphertextArray.Length
                ciphertextData.pbData = gcHandle.AddrOfPinnedObject()

                If Not Win32.CryptUnprotectData(ciphertextData, Nothing, Nothing, Nothing, Nothing, 0, plaintextData) _
                    Then Return Nothing

                Dim plaintextLength As Integer = plaintextData.cbData/2 ' Char = 2 bytes
                Dim plaintextArray(plaintextLength - 1) As Char
                Marshal.Copy(plaintextData.pbData, plaintextArray, 0, plaintextLength)

                Return New String(plaintextArray)
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage("RemoteDesktopConnectionManager.DecryptPassword() failed.",
                                                             ex, , True)
                Return Nothing
            Finally
                If gcHandle.IsAllocated Then gcHandle.Free()
                If Not plaintextData.pbData = IntPtr.Zero Then Win32.LocalFree(plaintextData.pbData)
            End Try
        End Function

        ' ReSharper disable once ClassNeverInstantiated.Local
        Private Class Win32
            ' ReSharper disable InconsistentNaming
            ' ReSharper disable IdentifierTypo
            ' ReSharper disable StringLiteralTypo
            <DllImport("crypt32.dll", CharSet:=CharSet.Unicode)>
            Public Shared Function CryptUnprotectData(ByRef dataIn As DATA_BLOB, ByVal description As String, ByRef optionalEntropy As DATA_BLOB, ByVal reserved As IntPtr, ByRef promptStruct As IntPtr, ByVal flags As Integer, ByRef dataOut As DATA_BLOB) As Boolean
            End Function

            <DllImport("kernel32.dll", CharSet := CharSet.Unicode)>
            Public Shared Sub LocalFree(ptr As IntPtr)
            End Sub

            Public Structure DATA_BLOB
                Public cbData As Integer
                Public pbData As IntPtr
            End Structure
            ' ReSharper restore StringLiteralTypo
            ' ReSharper restore IdentifierTypo
            ' ReSharper restore InconsistentNaming
        End Class

        Private Sub New()
        End Sub
    End Class
End Namespace