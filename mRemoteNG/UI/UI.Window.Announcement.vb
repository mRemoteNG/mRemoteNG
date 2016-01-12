﻿Imports System.ComponentModel
Imports mRemoteNG.My
Imports WeifenLuo.WinFormsUI.Docking
Imports mRemoteNG.App.Runtime

Namespace UI
    Namespace Window
        Public Class Announcement
            Inherits Base
#Region "Public Methods"
            Public Sub New(ByVal panel As DockContent)
                WindowType = Type.Announcement
                DockPnl = panel
                InitializeComponent()
            End Sub
#End Region

#Region "Private Fields"
            Private _appUpdate As App.Update
#End Region

#Region "Private Methods"
            Private Sub Announcement_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
                AddHandler webBrowser.Navigated, AddressOf webBrowser_Navigated

                ApplyLanguage()
                CheckForAnnouncement()
            End Sub

            Private Sub ApplyLanguage()

            End Sub

            Private Sub webBrowser_Navigated(sender As Object, e As System.Windows.Forms.WebBrowserNavigatedEventArgs)
                ' This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
                webBrowser.AllowWebBrowserDrop = False

                RemoveHandler webBrowser.Navigated, AddressOf webBrowser_Navigated
            End Sub

            Private Sub CheckForAnnouncement()
                If _appUpdate Is Nothing Then
                    _appUpdate = New App.Update
                ElseIf _appUpdate.IsGetAnnouncementInfoRunning Then
                    Return
                End If

                AddHandler _appUpdate.GetAnnouncementInfoCompletedEvent, AddressOf GetAnnouncementInfoCompleted

                _appUpdate.GetAnnouncementInfoAsync()
            End Sub

            Private Sub GetAnnouncementInfoCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
                If InvokeRequired Then
                    Dim myDelegate As New AsyncCompletedEventHandler(AddressOf GetAnnouncementInfoCompleted)
                    Invoke(myDelegate, New Object() {sender, e})
                    Return
                End If

                Try
                    RemoveHandler _appUpdate.GetAnnouncementInfoCompletedEvent, AddressOf GetAnnouncementInfoCompleted

                    If e.Cancelled Then Return
                    If e.Error IsNot Nothing Then Throw e.Error

                    webBrowser.Navigate(_appUpdate.CurrentAnnouncementInfo.Address)
                Catch ex As Exception
                    MessageCollector.AddExceptionMessage(Language.strUpdateGetAnnouncementInfoFailed, ex)
                End Try
            End Sub
#End Region
        End Class
    End Namespace
End Namespace