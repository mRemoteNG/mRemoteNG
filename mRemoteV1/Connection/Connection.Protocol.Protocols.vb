Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Connection
    Namespace Protocol
        Public Enum Protocols
            <LocalizedDescription("strRDP")> _
            RDP = 0
            <LocalizedDescription("strVnc")> _
            VNC = 1
            <LocalizedDescription("strSsh1")> _
            SSH1 = 2
            <LocalizedDescription("strSsh2")> _
            SSH2 = 3
            <LocalizedDescription("strTelnet")> _
            Telnet = 4
            <LocalizedDescription("strRlogin")> _
            Rlogin = 5
            <LocalizedDescription("strRAW")> _
            RAW = 6
            <LocalizedDescription("strHttp")> _
            HTTP = 7
            <LocalizedDescription("strHttps")> _
            HTTPS = 8
            <LocalizedDescription("strICA")> _
            ICA = 9
            <LocalizedDescription("strExtApp")> _
            IntApp = 20
            <Browsable(False)> _
            NONE = 999
        End Enum
    End Namespace
End Namespace
