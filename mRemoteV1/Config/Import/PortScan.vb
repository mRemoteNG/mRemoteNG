Imports mRemote3G.App
Imports mRemote3G.Connection
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Tools.PortScan
Imports mRemote3G.Tree

Namespace Config.Import
    Public Class PortScan
        Public Shared Sub Import(hosts As IEnumerable, protocol As Protocols, parentTreeNode As TreeNode)
            For Each host As ScanHost In hosts
                Dim finalProtocol As Protocols
                Dim protocolValid = False

                Dim treeNode As TreeNode = Node.AddNode(Node.Type.Connection, host.HostNameWithoutDomain)

                Dim connectionInfo As New Info()
                connectionInfo.Inherit = New Info.Inheritance(connectionInfo)

                connectionInfo.Name = host.HostNameWithoutDomain
                connectionInfo.Hostname = host.HostName

                Select Case protocol
                    Case Protocols.SSH2
                        If host.SSH Then
                            finalProtocol = Protocols.SSH2
                            protocolValid = True
                        End If
                    Case Protocols.Telnet
                        If host.Telnet Then
                            finalProtocol = Protocols.Telnet
                            protocolValid = True
                        End If
                    Case Protocols.HTTP
                        If host.HTTP Then
                            finalProtocol = Protocols.HTTP
                            protocolValid = True
                        End If
                    Case Protocols.HTTPS
                        If host.HTTPS Then
                            finalProtocol = Protocols.HTTPS
                            protocolValid = True
                        End If
                    Case Protocols.Rlogin
                        If host.Rlogin Then
                            finalProtocol = Protocols.Rlogin
                            protocolValid = True
                        End If
                    Case Protocols.RDP
                        If host.RDP Then
                            finalProtocol = Protocols.RDP
                            protocolValid = True
                        End If
                    Case Protocols.VNC
                        If host.VNC Then
                            finalProtocol = Protocols.VNC
                            protocolValid = True
                        End If
                End Select

                If protocolValid Then
                    connectionInfo.Protocol = finalProtocol
                    connectionInfo.SetDefaultPort()

                    treeNode.Tag = connectionInfo
                    parentTreeNode.Nodes.Add(treeNode)

                    If TypeOf parentTreeNode.Tag Is Container.Info Then
                        connectionInfo.Parent = parentTreeNode.Tag
                    End If

                    Runtime.ConnectionList.Add(connectionInfo)
                End If
            Next
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace