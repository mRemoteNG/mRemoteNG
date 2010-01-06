Imports System.ComponentModel

Namespace Connection
    Namespace Protocol
        Public Enum Protocols
            <Description("RDP")> _
            RDP = 0
            <Description("VNC")> _
            VNC = 1
            <Description("SSH1")> _
            SSH1 = 2
            <Description("SSH2")> _
            SSH2 = 3
            <Description("Telnet")> _
            Telnet = 4
            <Description("Rlogin")> _
            Rlogin = 5
            <Description("RAW")> _
            RAW = 6
            <Description("HTTP")> _
            HTTP = 7
            <Description("HTTPS")> _
            HTTPS = 8
            <Description("ICA")> _
            ICA = 9
            <Description("Ext. App")> _
            IntApp = 20
            <Browsable(False)> _
            NONE = 999
        End Enum
    End Namespace
End Namespace
