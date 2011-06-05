Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime
Imports System.Threading

Namespace UI
    Namespace Window
        Public Class Announcement
            Inherits UI.Window.Base

#Region "Form Init"
            Friend WithEvents wBrowser As System.Windows.Forms.WebBrowser

            Private Sub InitializeComponent()
                Me.wBrowser = New System.Windows.Forms.WebBrowser
                Me.SuspendLayout()
                '
                'wBrowser
                '
                Me.wBrowser.AllowWebBrowserDrop = False
                Me.wBrowser.Dock = System.Windows.Forms.DockStyle.Fill
                Me.wBrowser.Location = New System.Drawing.Point(0, 0)
                Me.wBrowser.MinimumSize = New System.Drawing.Size(20, 20)
                Me.wBrowser.Name = "wBrowser"
                Me.wBrowser.Size = New System.Drawing.Size(549, 474)
                Me.wBrowser.TabIndex = 0
                '
                'Announcement
                '
                Me.ClientSize = New System.Drawing.Size(549, 474)
                Me.Controls.Add(Me.wBrowser)
                Me.Name = "Announcement"
                Me.TabText = "Announcement"
                Me.Text = "Announcement"
                Me.Icon = My.Resources.News_Icon
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Announcement
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub
#End Region

#Region "Private Methods"
            Private Sub Announcement_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                Me.CheckForAnnouncement()
            End Sub

            Private Sub ApplyLanguage()

            End Sub
#End Region

            Private aN As App.Announcement
            Private uT As Thread

            Public Event AnnouncementCheckCompleted(ByVal AnnouncementAvailable As Boolean)

            Private IsAnnouncementCheckHandlerDeclared As Boolean = False

            Public Sub CheckForAnnouncement()
                Try
                    uT = New Thread(AddressOf CheckForAnnouncementBG)
                    uT.SetApartmentState(ApartmentState.STA)
                    uT.IsBackground = True

                    If Me.IsAnnouncementCheckHandlerDeclared = False Then
                        AddHandler AnnouncementCheckCompleted, AddressOf AnnouncementCheckComplete
                        Me.IsAnnouncementCheckHandlerDeclared = True
                    End If

                    uT.Start()
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CheckForAnnouncement (UI.Window.Announcement) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub CheckForAnnouncementBG()
                Try
                    aN = New App.Announcement

                    If aN.IsAnnouncementAvailable = True Then
                        RaiseEvent AnnouncementCheckCompleted(True)
                    Else
                        RaiseEvent AnnouncementCheckCompleted(False)
                    End If
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "CheckForAnnouncementBG (UI.Window.Announcement) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub AnnouncementCheckComplete(ByVal AnnouncementAvailable As Boolean)
                Try
                    wBrowser.Navigate(aN.curAI.URL)
                    My.Settings.LastAnnouncement = aN.curAI.Name
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "AnnouncementCheckComplete (UI.Window.Announcement) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
        End Class
    End Namespace
End Namespace