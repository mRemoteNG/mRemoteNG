Imports System.Windows.Forms
Imports mRemote.App.Runtime
Imports Skybound.Gecko
Imports System.ComponentModel

Namespace Connection
    Namespace Protocol
        Public Class HTTPBase
            Inherits Connection.Protocol.Base

#Region "Private Properties"
            Private wBrowser As Control
            Public httpOrS As String
            Public defaultPort As Integer
            Private tabTitle As String
#End Region

#Region "Public Methods"
            Public Sub New(ByVal RenderingEngine As RenderingEngine)
                Try
                    If RenderingEngine = RenderingEngine.Gecko Then
                        'Skybound.Gecko.Xpcom.Initialize(My.Settings.XULRunnerPath)
                        Me.Control = New MiniGeckoBrowser.MiniGeckoBrowser
                        TryCast(Me.Control, MiniGeckoBrowser.MiniGeckoBrowser).XULrunnerPath = My.Settings.XULRunnerPath
                    Else
                        Me.Control = New WebBrowser
                    End If

                    NewExtended()
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't create new Connection.Protocol.HTTPBase" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Public Overridable Sub NewExtended()
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    wBrowser = Me.Control

                    If InterfaceControl.Info.RenderingEngine = RenderingEngine.Gecko Then
                        'AddHandler TryCast(wBrowser, GeckoWebBrowser).CreateWindow, AddressOf gex_CreateWindow
                        AddHandler TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).TitleChanged, AddressOf wBrowser_DocumentTitleChanged
                        AddHandler TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).LastTabRemoved, AddressOf wBrowser_LastTabRemoved
                        'wBrowser.Width = wBrowser.Width
                    Else
                        TryCast(wBrowser, WebBrowser).AllowWebBrowserDrop = False
                        TryCast(wBrowser, WebBrowser).ScrollBarsEnabled = True

                        AddHandler TryCast(wBrowser, WebBrowser).DocumentTitleChanged, AddressOf wBrowser_DocumentTitleChanged
                        AddHandler TryCast(wBrowser, WebBrowser).NewWindow, AddressOf wBrowser_NewWindow
                    End If

                    Return True
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't SetProps (Connection.Protocol.HTTPBase)" & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                Try
                    Dim strHost As String = Me.InterfaceControl.Info.Hostname
                    Dim strAuth As String = ""

                    If Me.InterfaceControl.Info.Username <> "" And Me.InterfaceControl.Info.Password <> "" Then
                        strAuth = "Authorization: Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(Me.InterfaceControl.Info.Username & ":" & Me.InterfaceControl.Info.Password)) & vbNewLine
                    End If

                    If Me.InterfaceControl.Info.Port <> defaultPort Then
                        If strHost.EndsWith("/") Then
                            strHost = strHost.Substring(0, strHost.Length - 1)
                        End If

                        If strHost.Contains(httpOrS & "://") = False Then
                            strHost = httpOrS & "://" & strHost
                        End If

                        If InterfaceControl.Info.RenderingEngine = RenderingEngine.Gecko Then
                            TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).Navigate(strHost & ":" & Me.InterfaceControl.Info.Port)
                        Else
                            TryCast(wBrowser, WebBrowser).Navigate(strHost & ":" & Me.InterfaceControl.Info.Port, Nothing, Nothing, strAuth)
                        End If
                    Else
                        If strHost.Contains(httpOrS & "://") = False Then
                            strHost = httpOrS & "://" & strHost
                        End If

                        If InterfaceControl.Info.RenderingEngine = RenderingEngine.Gecko Then
                            TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).Navigate(strHost)
                        Else
                            TryCast(wBrowser, WebBrowser).Navigate(strHost, Nothing, Nothing, strAuth)
                        End If
                    End If

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't Connect (Connection.Protocol.HTTPBase)" & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function
#End Region

#Region "Private Methods"

#End Region

#Region "Events"
            Private Sub gex_CreateWindow(ByVal sender As Object, ByVal e As Skybound.Gecko.GeckoCreateWindowEventArgs)
                'Dim tP As TabPage = AddTab()
                'e.WebBrowser = tP.Controls(0)
                e.WebBrowser = Me.wBrowser
            End Sub

            Private Sub wBrowser_NewWindow(ByVal sender As Object, ByVal e As CancelEventArgs)
                e.Cancel = True
                TryCast(wBrowser, WebBrowser).Navigate(TryCast(wBrowser, WebBrowser).StatusText)
            End Sub

            Private Sub wBrowser_LastTabRemoved(ByVal sender As Object)
                Me.Close()
            End Sub

            Private Sub wBrowser_DocumentTitleChanged()
                Try
                    Dim tabP As Crownwood.Magic.Controls.TabPage
                    tabP = TryCast(InterfaceControl.Parent, Crownwood.Magic.Controls.TabPage)

                    If tabP IsNot Nothing Then
                        If tabTitle = "" Then
                            tabTitle = tabP.Title
                        End If

                        Dim shortTitle As String = ""

                        If Me.InterfaceControl.Info.RenderingEngine = RenderingEngine.Gecko Then
                            If TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).Title.Length >= 30 Then
                                shortTitle = TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).Title.Substring(0, 29) & " ..."
                            Else
                                shortTitle = TryCast(wBrowser, MiniGeckoBrowser.MiniGeckoBrowser).Title
                            End If
                        Else
                            If TryCast(wBrowser, WebBrowser).DocumentTitle.Length >= 30 Then
                                shortTitle = TryCast(wBrowser, WebBrowser).DocumentTitle.Substring(0, 29) & " ..."
                            Else
                                shortTitle = TryCast(wBrowser, WebBrowser).DocumentTitle
                            End If
                        End If

                        tabP.Title = tabTitle & " - " & shortTitle
                    End If
                Catch ex As Exception
                    mC.AddMessage(Messages.MessageClass.WarningMsg, "wBrowser_DocumentTitleChanged (Connection.Protocol.HTTPBase) failed" & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

            Public Enum RenderingEngine
                <Description("Internet Explorer")> _
                IE = 1
                <Description("Gecko (Firefox)")> _
                Gecko = 2
            End Enum
        End Class
    End Namespace
End Namespace