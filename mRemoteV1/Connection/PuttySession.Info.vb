Imports System.ComponentModel
Imports mRemote3G.App
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages
Imports mRemote3G.Root.PuttySessions
Imports mRemote3G.Tools

Namespace Connection

    Namespace PuttySession
        Public Class PuttyInfo
            Inherits Info
            Implements IComponent

#Region "Commands"

            <Command,
                LocalizedAttributes.LocalizedDisplayName("strPuttySessionSettings")>
            Public Sub SessionSettings()
                Try
                    Dim puttyProcess As New PuttyProcessController
                    If Not puttyProcess.Start() Then Return
                    If puttyProcess.SelectListBoxItem(PuttySession) Then
                        puttyProcess.ClickButton("&Load")
                    End If
                    puttyProcess.SetControlText("Button", "&Cancel", "&Close")
                    puttyProcess.SetControlVisible("Button", "&Open", False)
                    puttyProcess.WaitForExit()
                Catch ex As Exception
                   App.Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.Language.strErrorCouldNotLaunchPutty & vbNewLine & ex.ToString(), False)
                End Try
            End Sub

#End Region

#Region "Properties"

            <Browsable(False)>
            Public Property RootPuttySessionsInfo As PuttySessionsInfo

            <[ReadOnly](True)>
            Public Overrides Property PuttySession As String

            <[ReadOnly](True)>
            Public Overrides Property Name As String

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property Description As String

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property Icon As String
                Get
                    Return "PuTTY"
                End Get
                Set
                End Set
            End Property

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property Panel As String
                Get
                    Return RootPuttySessionsInfo.Panel
                End Get
                Set
                End Set
            End Property

            <[ReadOnly](True)>
            Public Overrides Property Hostname As String

            <[ReadOnly](True)>
            Public Overrides Property Username As String

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property Password As String

            <[ReadOnly](True)>
            Public Overrides Property Protocol As Protocols

            <[ReadOnly](True)>
            Public Overrides Property Port As Integer

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property PreExtApp As String

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property PostExtApp As String

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property MacAddress As String

            <[ReadOnly](True),
                Browsable(False)>
            Public Overrides Property UserField As String

#End Region

#Region "IComponent"

            <Browsable(False)>
            Public Property Site As ISite Implements IComponent.Site
                Get
                    Return New PropertyGridCommandSite(Me)
                End Get
                Set
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

