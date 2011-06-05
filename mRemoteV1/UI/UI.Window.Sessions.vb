Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Sessions
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents clmSesType As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmSesUsername As System.Windows.Forms.ColumnHeader
            Friend WithEvents clmSesActivity As System.Windows.Forms.ColumnHeader
            Friend WithEvents cMenSession As System.Windows.Forms.ContextMenuStrip
            Private components As System.ComponentModel.IContainer
            Friend WithEvents cMenSessionRefresh As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents cMenSessionLogOff As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents lvSessions As System.Windows.Forms.ListView

            Private Sub InitializeComponent()
                Me.components = New System.ComponentModel.Container
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Sessions))
                Me.lvSessions = New System.Windows.Forms.ListView
                Me.clmSesUsername = New System.Windows.Forms.ColumnHeader
                Me.clmSesActivity = New System.Windows.Forms.ColumnHeader
                Me.clmSesType = New System.Windows.Forms.ColumnHeader
                Me.cMenSession = New System.Windows.Forms.ContextMenuStrip(Me.components)
                Me.cMenSessionRefresh = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenSessionLogOff = New System.Windows.Forms.ToolStripMenuItem
                Me.cMenSession.SuspendLayout()
                Me.SuspendLayout()
                '
                'lvSessions
                '
                Me.lvSessions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                            Or System.Windows.Forms.AnchorStyles.Left) _
                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.lvSessions.BorderStyle = System.Windows.Forms.BorderStyle.None
                Me.lvSessions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmSesUsername, Me.clmSesActivity, Me.clmSesType})
                Me.lvSessions.ContextMenuStrip = Me.cMenSession
                Me.lvSessions.FullRowSelect = True
                Me.lvSessions.GridLines = True
                Me.lvSessions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
                Me.lvSessions.Location = New System.Drawing.Point(0, -1)
                Me.lvSessions.MultiSelect = False
                Me.lvSessions.Name = "lvSessions"
                Me.lvSessions.ShowGroups = False
                Me.lvSessions.Size = New System.Drawing.Size(242, 174)
                Me.lvSessions.TabIndex = 0
                Me.lvSessions.UseCompatibleStateImageBehavior = False
                Me.lvSessions.View = System.Windows.Forms.View.Details
                '
                'clmSesUsername
                '
                Me.clmSesUsername.Text = My.Resources.strColumnUsername
                Me.clmSesUsername.Width = 80
                '
                'clmSesActivity
                '
                Me.clmSesActivity.Text = My.Resources.strActivity
                '
                'clmSesType
                '
                Me.clmSesType.Text = My.Resources.strType
                Me.clmSesType.Width = 80
                '
                'cMenSession
                '
                Me.cMenSession.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cMenSessionRefresh, Me.cMenSessionLogOff})
                Me.cMenSession.Name = "cMenSession"
                Me.cMenSession.Size = New System.Drawing.Size(124, 48)
                '
                'cMenSessionRefresh
                '
                Me.cMenSessionRefresh.Image = Global.mRemoteNG.My.Resources.Resources.Refresh
                Me.cMenSessionRefresh.Name = "cMenSessionRefresh"
                Me.cMenSessionRefresh.Size = New System.Drawing.Size(123, 22)
                Me.cMenSessionRefresh.Text = My.Resources.strRefresh
                '
                'cMenSessionLogOff
                '
                Me.cMenSessionLogOff.Image = Global.mRemoteNG.My.Resources.Resources.Session_LogOff
                Me.cMenSessionLogOff.Name = "cMenSessionLogOff"
                Me.cMenSessionLogOff.Size = New System.Drawing.Size(123, 22)
                Me.cMenSessionLogOff.Text = My.Resources.strLogOff
                '
                'Sessions
                '
                Me.ClientSize = New System.Drawing.Size(242, 173)
                Me.Controls.Add(Me.lvSessions)
                Me.HideOnClose = True
                Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
                Me.Name = "Sessions"
                Me.TabText = My.Resources.strMenuSessions
                Me.Text = My.Resources.strMenuSessions
                Me.cMenSession.ResumeLayout(False)
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Private Properties"
            Private tServerName As String
            Private tUserName As String
            Private tPassword As String
            Private tDomain As String
            Private tSessionID As Long
            Private tKillingSession As Boolean
            Private threadSessions As Threading.Thread
            Private tServerHandle As Long
#End Region

#Region "Public Properties"
            Private _CurrentHost As String
            Public Property CurrentHost() As String
                Get
                    Return Me._CurrentHost
                End Get
                Set(ByVal value As String)
                    Me._CurrentHost = value
                End Set
            End Property
#End Region

#Region "Form Stuff"
            Private Sub Sessions_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()
            End Sub

            Private Sub ApplyLanguage()
                clmSesUsername.Text = My.Resources.strColumnUsername
                clmSesActivity.Text = My.Resources.strActivity
                clmSesType.Text = My.Resources.strType
                cMenSessionRefresh.Text = My.Resources.strRefresh
                cMenSessionLogOff.Text = My.Resources.strLogOff
                TabText = My.Resources.strMenuSessions
                Text = My.Resources.strMenuSessions
            End Sub
#End Region

#Region "Private Methods"
            Private Sub GetSessionsBG()
                Try
                    Dim tS As New mRemoteNG.Connection.Protocol.RDP.TerminalSessions
                    Dim sU As New Security.Impersonator
                    Dim tsSessions As New mRemoteNG.Connection.Protocol.RDP.Sessions

                    sU.StartImpersonation(tDomain, tUserName, tPassword)

                    Try
                        'Trace.WriteLine("Opening connection to server: " & tServerName)
                        If tS.OpenConnection(tServerName) = True Then
                            tServerHandle = tS.ServerHandle
                            'Trace.WriteLine("Trying to get sessions")
                            tsSessions = tS.GetSessions
                        End If
                    Catch ex As Exception
                    End Try

                    Dim i As Integer = 0

                    'Trace.WriteLine("Sessions Count: " & tsSessions.Count)

                    If tServerName = Me._CurrentHost Then
                        For i = 0 To tsSessions.ItemsCount - 1
                            Dim lItem As New ListViewItem
                            lItem.Tag = tsSessions(i).SessionID
                            lItem.Text = tsSessions(i).SessionUser
                            lItem.SubItems.Add(tsSessions(i).SessionState)
                            lItem.SubItems.Add(Replace(tsSessions(i).SessionName, vbNewLine, ""))

                            'Trace.WriteLine("Session " & i & ": " & tsSessions(i).SessionUser)

                            AddToList(lItem)
                        Next
                    End If

                    sU.StopImpersonation()
                    sU = Nothing
                    tS.CloseConnection(tServerHandle)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSessionGetFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub KillSessionBG()
                Try
                    If tUserName = "" Or tPassword = "" Then
                        'Trace.WriteLine("No Logon Info")
                        Exit Sub
                    End If

                    Dim ts As New mRemoteNG.Connection.Protocol.RDP.TerminalSessions
                    Dim sU As New Security.Impersonator

                    sU.StartImpersonation(tDomain, tUserName, tPassword)

                    Try
                        If ts.OpenConnection(tServerName) = True Then
                            If ts.KillSession(tSessionID) = True Then
                                sU.StopImpersonation()
                                'Trace.WriteLine("Successfully killed session: " & tSessionID)
                            Else
                                sU.StopImpersonation()
                                'Trace.WriteLine("Killing session " & tSessionID & " failed")
                            End If
                        End If
                    Catch ex As Exception
                        sU.StopImpersonation()
                    End Try

                    sU.StopImpersonation()

                    ClearList()

                    GetSessionsBG()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Resources.strSessionKillFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub


            Delegate Sub AddToListCB(ByVal [ListItem] As ListViewItem)
            Private Sub AddToList(ByVal [ListItem] As ListViewItem)
                If Me.lvSessions.InvokeRequired Then
                    Dim d As New AddToListCB(AddressOf AddToList)
                    Me.lvSessions.Invoke(d, New Object() {[ListItem]})
                Else
                    Me.lvSessions.Items.Add(ListItem)
                End If
            End Sub

            Delegate Sub ClearListCB()
            Private Sub ClearList()
                If Me.lvSessions.InvokeRequired Then
                    Dim d As New ClearListCB(AddressOf ClearList)
                    Me.lvSessions.Invoke(d)
                Else
                    Me.lvSessions.Items.Clear()
                End If
            End Sub

            Private Sub cMenSessionRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenSessionRefresh.Click
                Me.GetSessions()
            End Sub

            Private Sub cMenSessionLogOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cMenSessionLogOff.Click
                Me.KillSession()
            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Sessions
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub

            Public Sub GetSessionsAuto()
                Try
                    Dim nowHost As String = ""

                    If TypeOf mRemoteNG.Tree.Node.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                        nowHost = TryCast(mRemoteNG.Tree.Node.SelectedNode.Tag, mRemoteNG.Connection.Info).Hostname
                    Else
                        Me.ClearList()
                        Exit Sub
                    End If

                    If My.Settings.AutomaticallyGetSessionInfo And Me._CurrentHost = nowHost Then
                        Dim conI As mRemoteNG.Connection.Info = mRemoteNG.Tree.Node.SelectedNode.Tag

                        If conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Or conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.ICA Then
                            'continue
                        Else
                            Me.ClearList()
                            Exit Sub
                        End If

                        Dim sUser As String = conI.Username
                        Dim sPass As String = conI.Password
                        Dim sDom As String = conI.Domain

                        If My.Settings.EmptyCredentials = "custom" Then
                            If sUser = "" Then
                                sUser = My.Settings.DefaultUsername
                            End If

                            If sPass = "" Then
                                sPass = Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
                            End If

                            If sDom = "" Then
                                sDom = My.Settings.DefaultDomain
                            End If
                        End If

                        Me.GetSessions(conI.Hostname, sUser, sPass, sDom)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "GetSessionsAuto (UI.Window.Sessions) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub GetSessions()
                If mRemoteNG.Tree.Node.SelectedNode Is Nothing Then
                    Exit Sub
                End If

                If TypeOf mRemoteNG.Tree.Node.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                    'continue
                Else
                    Me.ClearList()
                    Exit Sub
                End If

                Dim conI As mRemoteNG.Connection.Info = mRemoteNG.Tree.Node.SelectedNode.Tag

                If conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Or conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.ICA Then
                    'continue
                Else
                    Me.ClearList()
                    Exit Sub
                End If

                Me._CurrentHost = conI.Hostname

                Dim sUser As String = conI.Username
                Dim sPass As String = conI.Password
                Dim sDom As String = conI.Domain

                If My.Settings.EmptyCredentials = "custom" Then
                    If sUser = "" Then
                        sUser = My.Settings.DefaultUsername
                    End If

                    If sPass = "" Then
                        sPass = Security.Crypt.Decrypt(My.Settings.DefaultPassword, App.Info.General.EncryptionKey)
                    End If

                    If sDom = "" Then
                        sDom = My.Settings.DefaultDomain
                    End If
                End If

                Me.GetSessions(conI.Hostname, sUser, sPass, sDom)
            End Sub

            Public Sub GetSessions(ByVal ServerName As String, ByVal Username As String, ByVal Password As String, ByVal Domain As String)
                Try
                    Me.lvSessions.Items.Clear()

                    tServerName = ServerName
                    tUserName = Username
                    tPassword = Password
                    tDomain = Domain

                    threadSessions = New Threading.Thread(AddressOf GetSessionsBG)
                    threadSessions.SetApartmentState(Threading.ApartmentState.STA)
                    threadSessions.IsBackground = True
                    threadSessions.Start()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "GetSessions (UI.Window.Sessions) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Sub KillSession()
                If TypeOf mRemoteNG.Tree.Node.SelectedNode.Tag Is mRemoteNG.Connection.Info Then
                    'continue
                Else
                    Exit Sub
                End If

                Dim conI As mRemoteNG.Connection.Info = mRemoteNG.Tree.Node.SelectedNode.Tag

                If conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP Then
                    'continue
                Else
                    Exit Sub
                End If

                If Me.lvSessions.SelectedItems.Count > 0 Then
                    'continue
                Else
                    Exit Sub
                End If

                For Each lvItem As ListViewItem In Me.lvSessions.SelectedItems
                    Me.KillSession(conI.Hostname, conI.Username, conI.Password, conI.Domain, lvItem.Tag)
                Next
            End Sub

            Public Sub KillSession(ByVal ServerName As String, ByVal Username As String, ByVal Password As String, ByVal Domain As String, ByVal SessionID As String)
                Try
                    tServerName = ServerName
                    tUserName = Username
                    tPassword = Password
                    tDomain = Domain
                    tSessionID = SessionID

                    threadSessions = New Threading.Thread(AddressOf KillSessionBG)
                    threadSessions.SetApartmentState(Threading.ApartmentState.STA)
                    threadSessions.IsBackground = True
                    threadSessions.Start()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "KillSession (UI.Window.Sessions) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

        End Class
    End Namespace
End Namespace