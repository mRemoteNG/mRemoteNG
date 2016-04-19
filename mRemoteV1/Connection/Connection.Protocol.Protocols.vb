
Imports mRemote3G.Tools

Namespace Connection

    Namespace Protocol
        Public Class Converter
            Public Shared Function ProtocolToString(protocol As Protocols) As String
                Return protocol.ToString()
            End Function

            Public Shared Function StringToProtocol(protocol As String) As Protocols
                Try
                    Return [Enum].Parse(GetType(Protocols), protocol, True)
                Catch ex As Exception
                    Return Protocols.RDP
                End Try
            End Function

            Private Sub New()
            End Sub
        End Class

        Public Enum Protocols
            <LocalizedAttributes.LocalizedDescription("strRDP")>
            RDP = 0
            <LocalizedAttributes.LocalizedDescription("strVnc")>
            VNC = 1
            <LocalizedAttributes.LocalizedDescription("strSsh1")>
            SSH1 = 2
            <LocalizedAttributes.LocalizedDescription("strSsh2")>
            SSH2 = 3
            <LocalizedAttributes.LocalizedDescription("strTelnet")>
            Telnet = 4
            <LocalizedAttributes.LocalizedDescription("strRlogin")>
            Rlogin = 5
            <LocalizedAttributes.LocalizedDescription("strRAW")>
            RAW = 6
            <LocalizedAttributes.LocalizedDescription("strHttp")>
            HTTP = 7
            <LocalizedAttributes.LocalizedDescription("strHttps")>
            HTTPS = 8
            <LocalizedAttributes.LocalizedDescription("strExtApp")>
            IntApp = 20
        End Enum
    End Namespace

End Namespace
