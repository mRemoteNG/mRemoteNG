Imports mRemoteNG.App.Runtime

Namespace Config.Import
    Public Class PortScan
        Public Shared Sub Import(ByVal hosts As IEnumerable, ByVal protocol As Connection.Protocol.Protocols, ByVal parentTreeNode As TreeNode)
            For Each host As Tools.PortScan.ScanHost In hosts
                Dim finalProtocol As Connection.Protocol.Protocols
                Dim protocolValid As Boolean = False

                Dim treeNode As TreeNode = Tree.Node.AddNode(Tree.Node.Type.Connection, host.HostNameWithoutDomain)

                Dim connectionInfo As New Connection.Info()
                connectionInfo.Inherit = New Connection.Info.Inheritance(connectionInfo)

                connectionInfo.Name = host.HostNameWithoutDomain
                connectionInfo.Hostname = host.HostName

                Select Case protocol
                    Case Connection.Protocol.Protocols.SSH2
                        If host.SSH Then
                            finalProtocol = Connection.Protocol.Protocols.SSH2
                            protocolValid = True
                        End If
                    Case Connection.Protocol.Protocols.Telnet
                        If host.Telnet Then
                            finalProtocol = Connection.Protocol.Protocols.Telnet
                            protocolValid = True
                        End If
                    Case Connection.Protocol.Protocols.HTTP
                        If host.HTTP Then
                            finalProtocol = Connection.Protocol.Protocols.HTTP
                            protocolValid = True
                        End If
                    Case Connection.Protocol.Protocols.HTTPS
                        If host.HTTPS Then
                            finalProtocol = Connection.Protocol.Protocols.HTTPS
                            protocolValid = True
                        End If
                    Case Connection.Protocol.Protocols.Rlogin
                        If host.Rlogin Then
                            finalProtocol = Connection.Protocol.Protocols.Rlogin
                            protocolValid = True
                        End If
                    Case Connection.Protocol.Protocols.RDP
                        If host.RDP Then
                            finalProtocol = Connection.Protocol.Protocols.RDP
                            protocolValid = True
                        End If
                    Case Connection.Protocol.Protocols.VNC
                        If host.VNC Then
                            finalProtocol = Connection.Protocol.Protocols.VNC
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

                    ConnectionList.Add(connectionInfo)
                End If
            Next
        End Sub
    End Class
End Namespace