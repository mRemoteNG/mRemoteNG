Imports System.ComponentModel
Imports mRemoteNG.Messages
Imports mRemoteNG.Tools.LocalizedAttributes
Imports mRemoteNG.My
Imports mRemoteNG.App.Runtime
Imports mRemoteNG.Tools

Namespace Connection
    Namespace PuttySession
        Public Class Info
            Inherits Connection.Info
            Implements IComponent

#Region "Commands"
            <Command(),
                DisplayName("Edit in PuTTY")> _
            Public Sub LaunchPutty()
                Try
                    Dim process As New Process
                    With process.StartInfo
                        .UseShellExecute = False
                        If Settings.UseCustomPuttyPath Then
                            .FileName = Settings.CustomPuttyPath
                        Else
                            .FileName = App.Info.General.PuttyPath
                        End If
                    End With
                    process.Start()
                    process.WaitForExit()
                Catch ex As Exception
                    MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strPuttyStartFailed & vbNewLine & ex.Message, True)
                End Try
            End Sub
#End Region

#Region "Properties"
            <Browsable(False)> _
            Public Property RootPuttySessionsInfo() As Root.PuttySessions.Info

            <[ReadOnly](True)> _
            Public Overrides Property PuttySession() As String

            <[ReadOnly](True)> _
            Public Overrides Property Name() As String

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property Description() As String

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property Icon() As String
                Get
                    Return "PuTTY"
                End Get
                Set(value As String)

                End Set
            End Property

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property Panel() As String
                Get
                    Return RootPuttySessionsInfo.Panel
                End Get
                Set(value As String)

                End Set
            End Property

            <[ReadOnly](True)> _
            Public Overrides Property Hostname() As String

            <[ReadOnly](True)> _
            Public Overrides Property Username() As String

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property Password() As String

            <[ReadOnly](True)> _
            Public Overrides Property Protocol() As Protocol.Protocols

            <[ReadOnly](True)> _
            Public Overrides Property Port() As Integer

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property PreExtApp() As String

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property PostExtApp() As String

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property MacAddress() As String

            <[ReadOnly](True), _
            Browsable(False)> _
            Public Overrides Property UserField() As String
#End Region

#Region "IComponent"
            Public Property Site() As ISite Implements IComponent.Site
                Get
                    Return New PropertyGridCommandSite(Me)
                End Get
                Set(value As ISite)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Sub Dispose() Implements IDisposable.Dispose
                RaiseEvent Disposed(Me, EventArgs.Empty)
            End Sub

            Public Event Disposed As EventHandler Implements IComponent.Disposed
#End Region
        End Class
    End Namespace
End Namespace

