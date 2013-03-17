Imports System.ComponentModel

Namespace Connection
    Namespace PuttySession
        Public Class Info
            Inherits Connection.Info

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
        End Class
    End Namespace
End Namespace

