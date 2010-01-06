Imports WeifenLuo.WinFormsUI.Docking
Imports mRemote.App.Runtime
Imports System.Threading

Namespace UI
    Namespace Window
        Public Class Announcment
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
                'Announcment
                '
                Me.ClientSize = New System.Drawing.Size(549, 474)
                Me.Controls.Add(Me.wBrowser)
                Me.Name = "Announcment"
                Me.TabText = "Announcment"
                Me.Text = "Announcment"
                Me.Icon = My.Resources.News_Icon
                Me.ResumeLayout(False)

            End Sub
#End Region

#Region "Public Methods"
            Public Sub New(ByVal Panel As DockContent)
                Me.WindowType = Type.Announcment
                Me.DockPnl = Panel
                Me.InitializeComponent()
            End Sub
#End Region

#Region "Private Methods"
            Private Sub Announcment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                ApplyLanguage()

                Me.CheckForAnnouncment()
            End Sub

            Private Sub ApplyLanguage()

            End Sub
#End Region

            Private aN As App.Announcment
            Private uT As Thread

            Public Event AnnouncmentCheckCompleted(ByVal AnnouncmentAvailable As Boolean)

            Private IsAnnouncmentCheckHandlerDeclared As Boolean = False

            Public Sub CheckForAnnouncment()
                Try
                    uT = New Thread(AddressOf CheckForAnnouncmentBG)
                    uT.IsBackground = True
                    'uT.SetApartmentState(ApartmentState.STA)

                    If Me.IsAnnouncmentCheckHandlerDeclared = False Then
                        AddHandler AnnouncmentCheckCompleted, AddressOf AnnouncmentCheckComplete
                        Me.IsAnnouncmentCheckHandlerDeclared = True
                    End If

                    uT.Start()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "CheckForAnnouncment (UI.Window.Announcment) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub CheckForAnnouncmentBG()
                Try
                    aN = New App.Announcment

                    If aN.IsAnnouncmentAvailable = True Then
                        RaiseEvent AnnouncmentCheckCompleted(True)
                    Else
                        RaiseEvent AnnouncmentCheckCompleted(False)
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "CheckForAnnouncmentBG (UI.Window.Announcment) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Sub AnnouncmentCheckComplete(ByVal AnnouncmentAvailable As Boolean)
                Try
                    wBrowser.Navigate(aN.curAI.URL)
                    My.Settings.LastAnnouncment = aN.curAI.Name
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "AnnouncmentCheckComplete (UI.Window.Announcment) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
        End Class
    End Namespace
End Namespace