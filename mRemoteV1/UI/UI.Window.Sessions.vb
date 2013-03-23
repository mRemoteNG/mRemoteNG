Imports System.Threading
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Sessions
            Inherits Base
#Region "Private Fields"
            Private _getSessionsThread As Thread
            Private _retrieved As Boolean = False
#End Region

#Region "Public Methods"
            Public Sub New(ByVal panel As DockContent)
                WindowType = Type.Sessions
                DockPnl = panel
                InitializeComponent()
            End Sub

            Public Sub GetSessions(Optional ByVal auto As Boolean = False)
                ClearList()
                If auto Then
                    _retrieved = False
                    If Not Settings.AutomaticallyGetSessionInfo Then Return
                End If

                Try
                    Dim connectionInfo As mRemoteNG.Connection.Info = TryCast(mRemoteNG.Tree.Node.SelectedNode.Tag, mRemoteNG.Connection.Info)
                    If connectionInfo Is Nothing Then Return

                    If Not (connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Or _
                            connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.ICA) Then Return

                    Dim data As New BackgroundData
                    With data
                        .Hostname = connectionInfo.Hostname
                        .Username = connectionInfo.Username
                        .Password = connectionInfo.Password
                        .Domain = connectionInfo.Domain

                        If Settings.EmptyCredentials = "custom" Then
                            If .Username = "" Then
                                .Username = Settings.DefaultUsername
                            End If

                            If .Password = "" Then
                                .Password = Security.Crypt.Decrypt(Settings.DefaultPassword, App.Info.General.EncryptionKey)
                            End If

                            If .Domain = "" Then
                                .Domain = Settings.DefaultDomain
                            End If
                        End If
                    End With

                    If _getSessionsThread IsNot Nothing Then
                        If _getSessionsThread.IsAlive Then _getSessionsThread.Abort()
                    End If
                    _getSessionsThread = New Thread(AddressOf GetSessionsBackground)
                    _getSessionsThread.SetApartmentState(ApartmentState.STA)
                    _getSessionsThread.IsBackground = True
                    _getSessionsThread.Start(data)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.GetSessions() failed." & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub KillSession()
                If sessionList.SelectedItems.Count = 0 Then Return

                Dim connectionInfo As mRemoteNG.Connection.Info = TryCast(mRemoteNG.Tree.Node.SelectedNode.Tag, mRemoteNG.Connection.Info)
                If connectionInfo Is Nothing Then Return

                If Not connectionInfo.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Then Return

                For Each lvItem As ListViewItem In sessionList.SelectedItems
                    KillSession(connectionInfo.Hostname, connectionInfo.Username, connectionInfo.Password, connectionInfo.Domain, lvItem.Tag)
                Next
            End Sub

            Public Sub KillSession(ByVal hostname As String, ByVal username As String, ByVal password As String, ByVal domain As String, ByVal sessionId As String)
                Try
                    Dim data As New BackgroundData
                    With data
                        .Hostname = hostname
                        .Username = username
                        .Password = password
                        .Domain = domain
                        .SessionId = sessionId
                    End With

                    Dim thread As New Thread(AddressOf KillSessionBackground)
                    thread.SetApartmentState(ApartmentState.STA)
                    thread.IsBackground = True
                    thread.Start(data)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "UI.Window.Sessions.KillSession() failed." & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Private Methods"
#Region "Form Stuff"
            Private Sub Sessions_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                TabText = Language.strMenuSessions
                Text = Language.strMenuSessions
                sessionActivityColumn.Text = Language.strActivity
                sessionMenuLogoff.Text = Language.strLogOff
                sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve
                sessionTypeColumn.Text = Language.strType
                sessionUsernameColumn.Text = Language.strColumnUsername
            End Sub
#End Region

            Private Sub GetSessionsBackground(ByVal dataObject As Object)
                Dim data As BackgroundData = TryCast(dataObject, BackgroundData)
                If data Is Nothing Then Return

                Dim impersonator As New Security.Impersonator
                Dim terminalSessions As New mRemoteNG.Connection.Protocol.RDP.TerminalSessions
                Dim serverHandle As Long = 0
                Try
                    With data
                        impersonator.StartImpersonation(.Domain, .Username, .Password)

                        If Not terminalSessions.OpenConnection(.Hostname) Then Return
                        serverHandle = terminalSessions.ServerHandle

                        GetSessions(terminalSessions)
                    End With

                    _retrieved = True
                Catch ex As ThreadAbortException

                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSessionGetFailed & vbNewLine & ex.Message, True)
                Finally
                    impersonator.StopImpersonation()
                    If Not serverHandle = 0 Then
                        terminalSessions.CloseConnection(serverHandle)
                    End If
                End Try
            End Sub

            ' Get sessions from an already impersonated and connected TerminalSessions object
            Private Sub GetSessions(ByVal terminalSessions As mRemoteNG.Connection.Protocol.RDP.TerminalSessions)
                Dim rdpSessions As mRemoteNG.Connection.Protocol.RDP.Sessions = terminalSessions.GetSessions
                For Each session As mRemoteNG.Connection.Protocol.RDP.Session In rdpSessions
                    Dim item As New ListViewItem
                    item.Tag = session.SessionID
                    item.Text = session.SessionUser
                    item.SubItems.Add(session.SessionState)
                    item.SubItems.Add(Replace(session.SessionName, vbNewLine, ""))
                    AddToList(item)
                Next
            End Sub

            Private Sub KillSessionBackground(ByVal dataObject As Object)
                Dim data As BackgroundData = TryCast(dataObject, BackgroundData)
                If data Is Nothing Then Return

                Dim impersonator As New Security.Impersonator
                Dim terminalSessions As New mRemoteNG.Connection.Protocol.RDP.TerminalSessions
                Dim serverHandle As Long = 0
                Try
                    With data
                        If String.IsNullOrEmpty(.Username) Or String.IsNullOrEmpty(.Password) Then Return

                        impersonator.StartImpersonation(.Domain, .Username, .Password)

                        If terminalSessions.OpenConnection(.Hostname) Then
                            serverHandle = terminalSessions.ServerHandle
                            terminalSessions.KillSession(.SessionId)
                        End If

                        ClearList()
                        GetSessions(terminalSessions)

                        _retrieved = True
                    End With
                Catch ex As ThreadAbortException

                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strSessionKillFailed & vbNewLine & ex.Message, True)
                Finally
                    impersonator.StopImpersonation()
                    If Not serverHandle = 0 Then
                        terminalSessions.CloseConnection(serverHandle)
                    End If
                End Try
            End Sub

            Delegate Sub AddToListCallback(ByVal item As ListViewItem)
            Private Sub AddToList(ByVal item As ListViewItem)
                If sessionList.InvokeRequired Then
                    Dim callback As New AddToListCallback(AddressOf AddToList)
                    sessionList.Invoke(callback, New Object() {item})
                Else
                    sessionList.Items.Add(item)
                End If
            End Sub

            Delegate Sub ClearListCallback()
            Private Sub ClearList()
                If sessionList.InvokeRequired Then
                    Dim callback As New ClearListCallback(AddressOf ClearList)
                    sessionList.Invoke(callback)
                Else
                    sessionList.Items.Clear()
                End If
            End Sub

            Private Sub menuSession_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
                If _retrieved Then
                    sessionMenuRetrieve.Text = Language.strRefresh
                Else
                    sessionMenuRetrieve.Text = Language.strMenuSessionRetrieve
                End If

                If sessionList.SelectedItems.Count = 0 Then
                    sessionMenuLogoff.Enabled = False
                Else
                    sessionMenuLogoff.Enabled = True
                End If
            End Sub

            Private Sub sessionMenuRetrieve_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles sessionMenuRetrieve.Click
                GetSessions()
            End Sub

            Private Sub sessionMenuLogoff_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles sessionMenuLogoff.Click
                KillSession()
            End Sub
#End Region

#Region "Private Classes"
            Private Class BackgroundData
                Public Hostname As String
                Public Username As String
                Public Password As String
                Public Domain As String
                Public SessionId As Long
            End Class
#End Region
        End Class
    End Namespace
End Namespace