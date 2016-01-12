﻿Imports System.IO
Imports mRemoteNG.App.Runtime

Namespace Config.Import
    Public Class RemoteDesktopConnection
        Public Shared Sub Import(ByVal fileName As String, ByVal parentTreeNode As TreeNode)
            Dim lines As String() = File.ReadAllLines(fileName)

            Dim name As String = Path.GetFileNameWithoutExtension(fileName)
            Dim treeNode As TreeNode = New TreeNode(name)
            parentTreeNode.Nodes.Add(treeNode)

            Dim connectionInfo As New Connection.Info
            connectionInfo.Inherit = New Connection.Info.Inheritance(connectionInfo)
            connectionInfo.Name = name
            connectionInfo.TreeNode = treeNode

            If TypeOf treeNode.Parent.Tag Is Container.Info Then
                connectionInfo.Parent = treeNode.Parent.Tag
            End If

            treeNode.Name = name
            treeNode.Tag = connectionInfo
            treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed
            treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed

            For Each line As String In lines
                Dim parts() As String = line.Split(New Char() {":"}, 3)
                If parts.Length < 3 Then Continue For

                Dim key As String = parts(0)
                Dim value As String = parts(2)

                SetConnectionInfoParameter(connectionInfo, key, value)
            Next

            ConnectionList.Add(connectionInfo)
        End Sub

        Private Shared Sub SetConnectionInfoParameter(ByRef connectionInfo As Connection.Info, ByVal key As String, ByVal value As String)
            Select Case LCase(key)
                Case "full address"
                    Dim uri As New Uri("dummyscheme" + uri.SchemeDelimiter + value)
                    If Not String.IsNullOrEmpty(uri.Host) Then connectionInfo.Hostname = uri.Host
                    If Not uri.Port = -1 Then connectionInfo.Port = uri.Port
                Case "server port"
                    connectionInfo.Port = value
                Case "username"
                    connectionInfo.Username = value
                Case "domain"
                    connectionInfo.Domain = value
                Case "session bpp"
                    Select Case value
                        Case 8
                            connectionInfo.Colors = Connection.Protocol.RDP.RDPColors.Colors256
                        Case 15
                            connectionInfo.Colors = Connection.Protocol.RDP.RDPColors.Colors15Bit
                        Case 16
                            connectionInfo.Colors = Connection.Protocol.RDP.RDPColors.Colors16Bit
                        Case 24
                            connectionInfo.Colors = Connection.Protocol.RDP.RDPColors.Colors24Bit
                        Case 32
                            connectionInfo.Colors = Connection.Protocol.RDP.RDPColors.Colors32Bit
                    End Select
                Case "bitmapcachepersistenable"
                    If value = 1 Then
                        connectionInfo.CacheBitmaps = True
                    Else
                        connectionInfo.CacheBitmaps = False
                    End If
                Case "screen mode id"
                    If value = 2 Then
                        connectionInfo.Resolution = Connection.Protocol.RDP.RDPResolutions.Fullscreen
                    Else
                        connectionInfo.Resolution = Connection.Protocol.RDP.RDPResolutions.FitToWindow
                    End If
                Case "connect to console"
                    If value = 1 Then
                        connectionInfo.UseConsoleSession = True
                    End If
                Case "disable wallpaper"
                    If value = 1 Then
                        connectionInfo.DisplayWallpaper = True
                    Else
                        connectionInfo.DisplayWallpaper = False
                    End If
                Case "disable themes"
                    If value = 1 Then
                        connectionInfo.DisplayThemes = True
                    Else
                        connectionInfo.DisplayThemes = False
                    End If
                Case "allow font smoothing"
                    If value = 1 Then
                        connectionInfo.EnableFontSmoothing = True
                    Else
                        connectionInfo.EnableFontSmoothing = False
                    End If
                Case "allow desktop composition"
                    If value = 1 Then
                        connectionInfo.EnableDesktopComposition = True
                    Else
                        connectionInfo.EnableDesktopComposition = False
                    End If
                Case "redirectsmartcards"
                    If value = 1 Then
                        connectionInfo.RedirectSmartCards = True
                    Else
                        connectionInfo.RedirectSmartCards = False
                    End If
                Case "redirectdrives"
                    If value = 1 Then
                        connectionInfo.RedirectDiskDrives = True
                    Else
                        connectionInfo.RedirectDiskDrives = False
                    End If
                Case "redirectcomports"
                    If value = 1 Then
                        connectionInfo.RedirectPorts = True
                    Else
                        connectionInfo.RedirectPorts = False
                    End If
                Case "redirectprinters"
                    If value = 1 Then
                        connectionInfo.RedirectPrinters = True
                    Else
                        connectionInfo.RedirectPrinters = False
                    End If
                Case "audiomode"
                    Select Case value
                        Case 0
                            connectionInfo.RedirectSound = Connection.Protocol.RDP.RDPSounds.BringToThisComputer
                        Case 1
                            connectionInfo.RedirectSound = Connection.Protocol.RDP.RDPSounds.LeaveAtRemoteComputer
                        Case 2
                            connectionInfo.RedirectSound = Connection.Protocol.RDP.RDPSounds.DoNotPlay
                    End Select
            End Select
        End Sub
    End Class
End Namespace