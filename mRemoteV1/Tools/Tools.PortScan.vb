Imports System.Threading
Imports mRemoteNG.App.Runtime
Imports System.Net.NetworkInformation

Namespace Tools
    Namespace PortScan
        Public Enum PortScanMode
            Normal = 1
            Import = 2
        End Enum

        Public Class ScanHost
#Region "Properties"
            Private Shared _SSHPort As Integer = Connection.Protocol.SSH1.Defaults.Port
            Public Shared Property SSHPort() As Integer
                Get
                    Return _SSHPort
                End Get
                Set(ByVal value As Integer)
                    _SSHPort = value
                End Set
            End Property

            Private Shared _TelnetPort As Integer = Connection.Protocol.Telnet.Defaults.Port
            Public Shared Property TelnetPort() As Integer
                Get
                    Return _TelnetPort
                End Get
                Set(ByVal value As Integer)
                    _TelnetPort = value
                End Set
            End Property

            Private Shared _HTTPPort As Integer = Connection.Protocol.HTTP.Defaults.Port
            Public Shared Property HTTPPort() As Integer
                Get
                    Return _HTTPPort
                End Get
                Set(ByVal value As Integer)
                    _HTTPPort = value
                End Set
            End Property

            Private Shared _HTTPSPort As Integer = Connection.Protocol.HTTPS.Defaults.Port
            Public Shared Property HTTPSPort() As Integer
                Get
                    Return _HTTPSPort
                End Get
                Set(ByVal value As Integer)
                    _HTTPSPort = value
                End Set
            End Property

            Private Shared _RloginPort As Integer = Connection.Protocol.Rlogin.Defaults.Port
            Public Shared Property RloginPort() As Integer
                Get
                    Return _RloginPort
                End Get
                Set(ByVal value As Integer)
                    _RloginPort = value
                End Set
            End Property

            Private Shared _RDPPort As Integer = Connection.Protocol.RDP.Defaults.Port
            Public Shared Property RDPPort() As Integer
                Get
                    Return _RDPPort
                End Get
                Set(ByVal value As Integer)
                    _RDPPort = value
                End Set
            End Property

            Private Shared _VNCPort As Integer = Connection.Protocol.VNC.Defaults.Port
            Public Shared Property VNCPort() As Integer
                Get
                    Return _VNCPort
                End Get
                Set(ByVal value As Integer)
                    _VNCPort = value
                End Set
            End Property



            Private _HostName As String
            Public Property HostName() As String
                Get
                    Return _HostName
                End Get
                Set(ByVal value As String)
                    _HostName = value
                End Set
            End Property

            Public ReadOnly Property HostNameWithoutDomain() As String
                Get
                    If _HostName <> _HostIP Then
                        If _HostName IsNot Nothing Then
                            If _HostName.Contains(".") Then
                                Return _HostName.Substring(0, _HostName.IndexOf("."))
                            Else
                                Return _HostName
                            End If
                        Else
                            Return _HostIP
                        End If
                    Else
                        Return _HostIP
                    End If
                End Get
            End Property

            Private _HostIP As String
            Public Property HostIP() As String
                Get
                    Return _HostIP
                End Get
                Set(ByVal value As String)
                    _HostIP = value
                End Set
            End Property

            Private _OpenPorts As New ArrayList
            Public Property OpenPorts() As ArrayList
                Get
                    Return _OpenPorts
                End Get
                Set(ByVal value As ArrayList)
                    _OpenPorts = value
                End Set
            End Property

            Private _ClosedPorts As ArrayList
            Public Property ClosedPorts() As ArrayList
                Get
                    Return _ClosedPorts
                End Get
                Set(ByVal value As ArrayList)
                    _ClosedPorts = value
                End Set
            End Property

            Private _RDP As Boolean
            Public Property RDP() As Boolean
                Get
                    Return _RDP
                End Get
                Set(ByVal value As Boolean)
                    _RDP = value
                End Set
            End Property

            Private _VNC As Boolean
            Public Property VNC() As Boolean
                Get
                    Return _VNC
                End Get
                Set(ByVal value As Boolean)
                    _VNC = value
                End Set
            End Property

            Private _SSH As Boolean
            Public Property SSH() As Boolean
                Get
                    Return _SSH
                End Get
                Set(ByVal value As Boolean)
                    _SSH = value
                End Set
            End Property

            Private _Telnet As Boolean
            Public Property Telnet() As Boolean
                Get
                    Return _Telnet
                End Get
                Set(ByVal value As Boolean)
                    _Telnet = value
                End Set
            End Property

            Private _Rlogin As Boolean
            Public Property Rlogin() As Boolean
                Get
                    Return _Rlogin
                End Get
                Set(ByVal value As Boolean)
                    _Rlogin = value
                End Set
            End Property

            Private _HTTP As Boolean
            Public Property HTTP() As Boolean
                Get
                    Return _HTTP
                End Get
                Set(ByVal value As Boolean)
                    _HTTP = value
                End Set
            End Property

            Private _HTTPS As Boolean
            Public Property HTTPS() As Boolean
                Get
                    Return _HTTPS
                End Get
                Set(ByVal value As Boolean)
                    _HTTPS = value
                End Set
            End Property
#End Region

#Region "Methods"
            Public Sub New(ByVal host As String)
                _HostIP = host
                _OpenPorts = New ArrayList
                _ClosedPorts = New ArrayList
            End Sub

            Public Overrides Function ToString() As String
                Try
                    Return "SSH: " & _SSH & " Telnet: " & _Telnet & " HTTP: " & _HTTP & " HTTPS: " & _HTTPS & " Rlogin: " & _Rlogin & " RDP: " & _RDP & " VNC: " & _VNC
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "ToString failed (Tools.PortScan)", True)
                    Return ""
                End Try
            End Function

            Public Function ToListViewItem(ByVal Mode As PortScanMode) As ListViewItem
                Try
                    Dim lvI As New ListViewItem
                    lvI.Tag = Me
                    If _HostName <> "" Then
                        lvI.Text = _HostName
                    Else
                        lvI.Text = _HostIP
                    End If

                    If Mode = PortScanMode.Import Then
                        lvI.SubItems.Add(BoolToYesNo(_SSH))
                        lvI.SubItems.Add(BoolToYesNo(_Telnet))
                        lvI.SubItems.Add(BoolToYesNo(_HTTP))
                        lvI.SubItems.Add(BoolToYesNo(_HTTPS))
                        lvI.SubItems.Add(BoolToYesNo(_Rlogin))
                        lvI.SubItems.Add(BoolToYesNo(_RDP))
                        lvI.SubItems.Add(BoolToYesNo(_VNC))
                    Else
                        Dim strOpen As String = ""
                        Dim strClosed As String = ""

                        For Each p As Integer In _OpenPorts
                            strOpen &= p & ", "
                        Next

                        For Each p As Integer In _ClosedPorts
                            strClosed &= p & ", "
                        Next

                        lvI.SubItems.Add(strOpen.Substring(0, IIf(strOpen.Length > 0, strOpen.Length - 2, strOpen.Length)))
                        lvI.SubItems.Add(strClosed.Substring(0, IIf(strClosed.Length > 0, strClosed.Length - 2, strClosed.Length)))
                    End If

                    Return lvI
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "ToString failed (Tools.PortScan)", True)
                    Return Nothing
                End Try
            End Function

            Private Function BoolToYesNo(ByVal Bool As Boolean) As String
                If Bool = True Then
                    Return My.Resources.strYes
                Else
                    Return My.Resources.strNo
                End If
            End Function

            Public Sub SetAllProtocols(ByVal Open As Boolean)
                _VNC = False
                _Telnet = False
                _SSH = False
                _Rlogin = False
                _RDP = False
                _HTTPS = False
                _HTTP = False
            End Sub
#End Region
        End Class

        Public Class Scanner
#Region "Properties"
            Private _StartIP As String
            Public Property StartIP() As String
                Get
                    Return _StartIP
                End Get
                Set(ByVal value As String)
                    _StartIP = value
                End Set
            End Property

            Private _EndIP As String
            Public Property EndIP() As String
                Get
                    Return _EndIP
                End Get
                Set(ByVal value As String)
                    _EndIP = value
                End Set
            End Property

            Private _StartPort As Integer
            Public Property StartPort() As Integer
                Get
                    Return _StartPort
                End Get
                Set(ByVal value As Integer)
                    _StartPort = value
                End Set
            End Property

            Private _EndPort As Integer
            Public Property EndPort() As Integer
                Get
                    Return _EndPort
                End Get
                Set(ByVal value As Integer)
                    _EndPort = value
                End Set
            End Property



            Private _ScannedHosts As ArrayList
            Public Property ScannedHosts() As ArrayList
                Get
                    Return _ScannedHosts
                End Get
                Set(ByVal value As ArrayList)
                    _ScannedHosts = value
                End Set
            End Property
#End Region

#Region "Methods"
            Public Sub New(ByVal startIP As String, ByVal endIP As String)
                Mode = PortScanMode.Import

                _StartIP = startIP
                _EndIP = endIP

                Ports = New ArrayList()
                Ports.AddRange(New Integer() {ScanHost.SSHPort, ScanHost.TelnetPort, ScanHost.HTTPPort, _
                                              ScanHost.HTTPSPort, ScanHost.RloginPort, ScanHost.RDPPort, _
                                              ScanHost.VNCPort})

                Hosts = GetIPRange(_StartIP, _EndIP)

                _ScannedHosts = New ArrayList()
            End Sub

            Public Sub New(ByVal startIP As String, ByVal endIP As String, ByVal startPort As String, ByVal endPort As String)
                Mode = PortScanMode.Normal

                _StartIP = startIP
                _EndIP = endIP

                _StartPort = startPort
                _EndPort = endPort

                Ports = New ArrayList()
                For p As Integer = startPort To endPort
                    Ports.Add(p)
                Next

                Hosts = GetIPRange(_StartIP, _EndIP)

                _ScannedHosts = New ArrayList()
            End Sub

            Public Event BeginHostScan(ByVal Host As String)
            Public Event HostScanned(ByVal SHost As ScanHost, ByVal HostsAlreadyScanned As Integer, ByVal HostsToBeScanned As Integer)
            Public Event ScanComplete(ByVal Hosts As ArrayList)


            Private Hosts As ArrayList
            Private Ports As ArrayList

            Private Mode As PortScanMode

            Private sThread As Thread


            Public Sub StartScan()
                sThread = New Thread(AddressOf StartScanBG)
                sThread.SetApartmentState(Threading.ApartmentState.STA)
                sThread.IsBackground = True
                sThread.Start()
            End Sub

            Public Sub StopScan()
                sThread.Abort()
            End Sub

            Public Shared Function IsPortOpen(ByVal Hostname As String, ByVal Port As String) As Boolean
                Try
                    Dim tcpClient As New System.Net.Sockets.TcpClient(Hostname, Port)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Sub StartScanBG()
                Try
                    Dim hCount As Integer = 0

                    For Each Host As String In Hosts
                        RaiseEvent BeginHostScan(Host)

                        Dim sHost As New ScanHost(Host)
                        hCount += 1

                        Dim HostAlive As Boolean = False

                        HostAlive = IsHostAlive(Host)

                        If HostAlive = False Then
                            sHost.ClosedPorts.AddRange(Ports)
                            sHost.SetAllProtocols(False)
                        Else
                            For Each Port As Integer In Ports
                                Dim err As Boolean = False

                                Try
                                    Dim tcpClient As New System.Net.Sockets.TcpClient(Host, Port)

                                    err = False
                                    sHost.OpenPorts.Add(Port)
                                Catch ex As Exception
                                    err = True
                                    sHost.ClosedPorts.Add(Port)
                                End Try

                                Select Case Port
                                    Case ScanHost.SSHPort
                                        sHost.SSH = Not err
                                    Case ScanHost.TelnetPort
                                        sHost.Telnet = Not err
                                    Case ScanHost.HTTPPort
                                        sHost.HTTP = Not err
                                    Case ScanHost.HTTPSPort
                                        sHost.HTTPS = Not err
                                    Case ScanHost.RloginPort
                                        sHost.Rlogin = Not err
                                    Case ScanHost.RDPPort
                                        sHost.RDP = Not err
                                    Case ScanHost.VNCPort
                                        sHost.VNC = Not err
                                End Select
                            Next
                        End If

                        If HostAlive = True Then
                            Try
                                sHost.HostName = Net.Dns.GetHostEntry(sHost.HostIP).HostName
                            Catch ex As Exception
                            End Try
                        End If

                        _ScannedHosts.Add(sHost)
                        RaiseEvent HostScanned(sHost, hCount, Hosts.Count)
                    Next

                    RaiseEvent ScanComplete(_ScannedHosts)
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "StartScanBG failed (Tools.PortScan)" & vbNewLine & ex.Message, True)
                End Try
            End Sub

            Private Function IsHostAlive(ByVal Host As String) As Boolean
                Dim pingSender As New Ping
                Dim pReply As PingReply

                Try
                    pReply = pingSender.Send(Host)

                    If pReply.Status = IPStatus.Success Then
                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Function


            Private Function GetIPRange(ByVal fromIP As String, ByVal toIP As String) As ArrayList
                Try
                    Dim arrIPs As New ArrayList

                    Dim ipFrom As String() = fromIP.Split(".")
                    Dim ipTo As String() = toIP.Split(".")

                    While Not matchIP(ipFrom, ipTo)
                        arrIPs.Add(String.Format("{0}.{1}.{2}.{3}", ipFrom(0), ipFrom(1), ipFrom(2), ipFrom(3)))
                        ipFrom(3) += 1
                        If ipFrom(3) > 255 Then
                            ipFrom(3) = 0
                            ipFrom(2) += 1
                            If ipFrom(2) > 255 Then
                                ipFrom(2) = 0
                                ipFrom(1) += 1
                                If ipFrom(1) > 255 Then
                                    ipFrom(1) = 0
                                    ipFrom(0) += 1
                                    If ipFrom(0) > 255 Then
                                        ipFrom(0) = 0
                                    End If
                                End If
                            End If
                        End If
                    End While

                    Return arrIPs
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "GetIPRange failed (Tools.PortScan)" & vbNewLine & ex.Message, True)
                    Return Nothing
                End Try
            End Function

            Private Function matchIP(ByVal fromIP As String(), ByVal toIP As String()) As Boolean
                Try
                    For c As Integer = 0 To fromIP.Length - 1
                        If c = fromIP.Length - 1 Then
                            If Not fromIP(c) = toIP(c) + 1 Then
                                Return False
                            End If
                        Else
                            If Not fromIP(c) = toIP(c) Then
                                Return False
                            End If
                        End If
                    Next
                    Return True
                Catch ex As Exception
                    MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "matchIP failed (Tools.PortScan)" & vbNewLine & ex.Message, True)
                    Return False
                End Try
            End Function
#End Region
        End Class
    End Namespace
End Namespace