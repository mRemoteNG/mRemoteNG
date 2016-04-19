Imports System.Text
Imports Crownwood.Magic.Controls
Imports mRemote3G.App
Imports mRemote3G.Messages
Imports mRemote3G.Tools
Imports SHDocVw

Namespace Connection

    Namespace Protocol
        Public Class HTTPBase
            Inherits Base

#Region "Private Properties"

            Private wBrowser As Control
            Public httpOrS As String
            Public defaultPort As Integer
            Private tabTitle As String

#End Region

#Region "Public Methods"

            Public Sub New(RenderingEngine As RenderingEngine)
                Try
                    Me.Control = New System.Windows.Forms.WebBrowser

                    NewExtended()
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strHttpConnectionFailed & vbNewLine &
                                                        ex.ToString(), True)
                End Try
            End Sub

            Public Overridable Sub NewExtended()
            End Sub

            Public Overrides Function SetProps() As Boolean
                MyBase.SetProps()

                Try
                    Dim objTabPage = TryCast(Me.InterfaceControl.Parent, TabPage)
                    Me.tabTitle = objTabPage.Title
                Catch ex As Exception
                    Me.tabTitle = ""
                End Try

                Try
                    Me.wBrowser = Me.Control

                    Dim objWebBrowser = TryCast(wBrowser, System.Windows.Forms.WebBrowser)
                    Dim objAxWebBrowser = DirectCast(objWebBrowser.ActiveXInstance, WebBrowser)

                    objWebBrowser.ScrollBarsEnabled = True

                    AddHandler objWebBrowser.Navigated, AddressOf wBrowser_Navigated
                    AddHandler objWebBrowser.DocumentTitleChanged, AddressOf wBrowser_DocumentTitleChanged
                    AddHandler objAxWebBrowser.NewWindow3, AddressOf wBrowser_NewWindow3

                    Return True
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strHttpSetPropsFailed & vbNewLine &
                                                        ex.ToString(), True)
                    Return False
                End Try
            End Function

            Public Overrides Function Connect() As Boolean
                Try
                    Dim strHost As String = Me.InterfaceControl.Info.Hostname
                    Dim strAuth = ""

                    If _
                        Not ((Force And Info.Force.NoCredentials) = Info.Force.NoCredentials) And
                        Not String.IsNullOrEmpty(InterfaceControl.Info.Username) And
                        Not String.IsNullOrEmpty(InterfaceControl.Info.Password) Then
                        strAuth = "Authorization: Basic " +
                                  Convert.ToBase64String(
                                      Encoding.ASCII.GetBytes(
                                          Me.InterfaceControl.Info.Username & ":" & Me.InterfaceControl.Info.Password)) &
                                  vbNewLine
                    End If

                    If Me.InterfaceControl.Info.Port <> defaultPort Then
                        If strHost.EndsWith("/") Then
                            strHost = strHost.Substring(0, strHost.Length - 1)
                        End If

                        If strHost.Contains(httpOrS & "://") = False Then
                            strHost = httpOrS & "://" & strHost
                        End If


                        TryCast(wBrowser, System.Windows.Forms.WebBrowser).Navigate(
                            strHost & ":" & Me.InterfaceControl.Info.Port, Nothing, Nothing, strAuth)
                    Else
                        If strHost.Contains(httpOrS & "://") = False Then
                            strHost = httpOrS & "://" & strHost
                        End If


                        TryCast(wBrowser, System.Windows.Forms.WebBrowser).Navigate(strHost, Nothing, Nothing, strAuth)
                    End If

                    MyBase.Connect()
                    Return True
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        Language.Language.strHttpConnectFailed & vbNewLine &
                                                        ex.ToString(), True)
                    Return False
                End Try
            End Function

#End Region

#Region "Private Methods"

#End Region

#Region "Events"

            Private Sub wBrowser_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs)
                Dim objWebBrowser = TryCast(wBrowser, System.Windows.Forms.WebBrowser)
                If objWebBrowser Is Nothing Then Return

                ' This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
                objWebBrowser.AllowWebBrowserDrop = False

                RemoveHandler objWebBrowser.Navigated, AddressOf wBrowser_Navigated
            End Sub

            Private Sub wBrowser_NewWindow3(ByRef ppDisp As Object, ByRef Cancel As Boolean, dwFlags As Long,
                                            bstrUrlContext As String, bstrUrl As String)
                If (dwFlags And NWMF.NWMF_OVERRIDEKEY) Then
                    Cancel = False
                Else
                    Cancel = True
                End If
            End Sub

            Private Sub wBrowser_LastTabRemoved(sender As Object)
                Me.Close()
            End Sub

            Private Sub wBrowser_DocumentTitleChanged()
                Try
                    Dim tabP As TabPage
                    tabP = TryCast(InterfaceControl.Parent, TabPage)

                    If tabP IsNot Nothing Then
                        Dim shortTitle = ""


                        If TryCast(wBrowser, System.Windows.Forms.WebBrowser).DocumentTitle.Length >= 30 Then
                            shortTitle =
                                TryCast(wBrowser, System.Windows.Forms.WebBrowser).DocumentTitle.Substring(0, 29) &
                                " ..."
                        Else
                            shortTitle = TryCast(wBrowser, System.Windows.Forms.WebBrowser).DocumentTitle
                        End If

                        If Me.tabTitle <> "" Then
                            tabP.Title = tabTitle & " - " & shortTitle
                        Else
                            tabP.Title = shortTitle
                        End If
                    End If
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.Language.strHttpDocumentTileChangeFailed & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

#End Region

#Region "Enums"

            Public Enum RenderingEngine
                None = 0
                <LocalizedAttributes.LocalizedDescription("strHttpInternetExplorer")>
                IE = 1
            End Enum

            Private Enum NWMF
                ' ReSharper disable InconsistentNaming
                NWMF_UNLOADING = &H1
                NWMF_USERINITED = &H2
                NWMF_FIRST = &H4
                NWMF_OVERRIDEKEY = &H8
                NWMF_SHOWHELP = &H10
                NWMF_HTMLDIALOG = &H20
                NWMF_FROMDIALOGCHILD = &H40
                NWMF_USERREQUESTED = &H80
                NWMF_USERALLOWED = &H100
                NWMF_FORCEWINDOW = &H10000
                NWMF_FORCETAB = &H20000
                NWMF_SUGGESTWINDOW = &H40000
                NWMF_SUGGESTTAB = &H80000
                NWMF_INACTIVETAB = &H100000
                ' ReSharper restore InconsistentNaming
            End Enum

#End Region
        End Class
    End Namespace

End Namespace