Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Threading
Imports mRemote3G.App
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages

Namespace Tools

    Namespace PortScan
        Public Class ScanHost

#Region "Properties"

            Private Shared _SSHPort As Integer = SSH1.Defaults.Port

            Public Shared Property SSHPort As Integer
                Get
                    Return _SSHPort
                End Get
                Set
                    _SSHPort = value
                End Set
            End Property

            Private Shared _TelnetPort As Integer = Connection.Protocol.Telnet.Defaults.Port

            Public Shared Property TelnetPort As Integer
                Get
                    Return _TelnetPort
                End Get
                Set
                    _TelnetPort = value
                End Set
            End Property

            Private Shared _HTTPPort As Integer = Connection.Protocol.HTTP.Defaults.Port

            Public Shared Property HTTPPort As Integer
                Get
                    Return _HTTPPort
                End Get
                Set
                    _HTTPPort = value
                End Set
            End Property

            Private Shared _HTTPSPort As Integer = Connection.Protocol.HTTPS.Defaults.Port

            Public Shared Property HTTPSPort As Integer
                Get
                    Return _HTTPSPort
                End Get
                Set
                    _HTTPSPort = value
                End Set
            End Property

            Private Shared _RloginPort As Integer = Connection.Protocol.Rlogin.Defaults.Port

            Public Shared Property RloginPort As Integer
                Get
                    Return _RloginPort
                End Get
                Set
                    _RloginPort = value
                End Set
            End Property

            Private Shared _RDPPort As Integer = Connection.Protocol.RDP.Defaults.Port

            Public Shared Property RDPPort As Integer
                Get
                    Return _RDPPort
                End Get
                Set
                    _RDPPort = value
                End Set
            End Property

            Private Shared _VNCPort As Integer = Connection.Protocol.VNC.Defaults.Port

            Public Shared Property VNCPort As Integer
                Get
                    Return _VNCPort
                End Get
                Set
                    _VNCPort = value
                End Set
            End Property

            Private _hostName As String = ""

            Public Property HostName As String
                Get
                    Return _hostName
                End Get
                Set
                    _hostName = value
                End Set
            End Property

            Public ReadOnly Property HostNameWithoutDomain As String
                Get
                    If String.IsNullOrEmpty(HostName) OrElse HostName = HostIp Then Return HostIp
                    Return HostName.Split(".")(0)
                End Get
            End Property

            Private _hostIp As String

            Public Property HostIp As String
                Get
                    Return _hostIp
                End Get
                Set
                    _hostIp = value
                End Set
            End Property

            Private _openPorts As New ArrayList

            Public Property OpenPorts As ArrayList
                Get
                    Return _openPorts
                End Get
                Set
                    _openPorts = value
                End Set
            End Property

            Private _closedPorts As ArrayList

            Public Property ClosedPorts As ArrayList
                Get
                    Return _closedPorts
                End Get
                Set
                    _closedPorts = value
                End Set
            End Property

            Private _RDP As Boolean

            Public Property RDP As Boolean
                Get
                    Return _RDP
                End Get
                Set
                    _RDP = value
                End Set
            End Property

            Private _VNC As Boolean

            Public Property VNC As Boolean
                Get
                    Return _VNC
                End Get
                Set
                    _VNC = value
                End Set
            End Property

            Private _SSH As Boolean

            Public Property SSH As Boolean
                Get
                    Return _SSH
                End Get
                Set
                    _SSH = value
                End Set
            End Property

            Private _Telnet As Boolean

            Public Property Telnet As Boolean
                Get
                    Return _Telnet
                End Get
                Set
                    _Telnet = value
                End Set
            End Property

            Private _Rlogin As Boolean

            Public Property Rlogin As Boolean
                Get
                    Return _Rlogin
                End Get
                Set
                    _Rlogin = value
                End Set
            End Property

            Private _HTTP As Boolean

            Public Property HTTP As Boolean
                Get
                    Return _HTTP
                End Get
                Set
                    _HTTP = value
                End Set
            End Property

            Private _HTTPS As Boolean

            Public Property HTTPS As Boolean
                Get
                    Return _HTTPS
                End Get
                Set
                    _HTTPS = value
                End Set
            End Property

#End Region

#Region "Methods"

            Public Sub New(host As String)
                _hostIp = host
                _openPorts = New ArrayList
                _closedPorts = New ArrayList
            End Sub

            Public Overrides Function ToString() As String
                Try
                    Return _
                        "SSH: " & _SSH & " Telnet: " & _Telnet & " HTTP: " & _HTTP & " HTTPS: " & _HTTPS & " Rlogin: " &
                        _Rlogin & " RDP: " & _RDP & " VNC: " & _VNC
                Catch ex As Exception
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "ToString failed (Tools.PortScan)",
                                                        True)
                    Return ""
                End Try
            End Function

            Public Function ToListViewItem(import As Boolean) As ListViewItem
                Try
                    Dim listViewItem As New ListViewItem
                    listViewItem.Tag = Me
                    If _hostName <> "" Then
                        listViewItem.Text = _hostName
                    Else
                        listViewItem.Text = _hostIp
                    End If

                    If import Then
                        listViewItem.SubItems.Add(BoolToYesNo(_SSH))
                        listViewItem.SubItems.Add(BoolToYesNo(_Telnet))
                        listViewItem.SubItems.Add(BoolToYesNo(_HTTP))
                        listViewItem.SubItems.Add(BoolToYesNo(_HTTPS))
                        listViewItem.SubItems.Add(BoolToYesNo(_Rlogin))
                        listViewItem.SubItems.Add(BoolToYesNo(_RDP))
                        listViewItem.SubItems.Add(BoolToYesNo(_VNC))
                    Else
                        Dim strOpen = ""
                        Dim strClosed = ""

                        For Each p As Integer In _openPorts
                            strOpen &= p & ", "
                        Next

                        For Each p As Integer In _closedPorts
                            strClosed &= p & ", "
                        Next

                        listViewItem.SubItems.Add(strOpen.Substring(0, IIf(strOpen.Length > 0, strOpen.Length - 2, strOpen.Length)))
                        listViewItem.SubItems.Add(strClosed.Substring(0, IIf(strClosed.Length > 0, strClosed.Length - 2, strClosed.Length)))
                    End If

                    Return listViewItem
                Catch ex As Exception
                    Runtime.MessageCollector.AddExceptionMessage("Tools.PortScan.ToListViewItem() failed.", ex,
                                                                 MessageClass.WarningMsg, True)
                    Return Nothing
                End Try
            End Function

            Private Function BoolToYesNo(value As Boolean) As String
                If value Then
                    Return Language.Language.strYes
                Else
                    Return Language.Language.strNo
                End If
            End Function

            Public Sub SetAllProtocols(value As Boolean)
                _VNC = value
                _Telnet = value
                _SSH = value
                _Rlogin = value
                _RDP = value
                _HTTPS = value
                _HTTP = value
            End Sub

#End Region
        End Class

        Public Class Scanner

#Region "Private Members"

            Private ReadOnly _ipAddresses As New List(Of IPAddress)
            Private ReadOnly _ports As New List(Of Integer)

            Private _scanThread As Thread
            Private ReadOnly _scannedHosts As New List(Of ScanHost)

#End Region

#Region "Public Methods"

            Public Sub New(ipAddress1 As IPAddress, ipAddress2 As IPAddress)
                Dim ipAddressStart As IPAddress = IpAddressMin(ipAddress1, ipAddress2)
                Dim ipAddressEnd As IPAddress = IpAddressMax(ipAddress1, ipAddress2)

                _ports.Clear()
                _ports.AddRange(New Integer() {ScanHost.SSHPort, ScanHost.TelnetPort, ScanHost.HTTPPort,
                                               ScanHost.HTTPSPort, ScanHost.RloginPort, ScanHost.RDPPort,
                                               ScanHost.VNCPort})

                _ipAddresses.Clear()
                _ipAddresses.AddRange(IpAddressArrayFromRange(ipAddressStart, ipAddressEnd))

                _scannedHosts.Clear()
            End Sub

            Public Sub New(ipAddress1 As IPAddress, ipAddress2 As IPAddress, port1 As Integer, port2 As Integer)
                Me.New(ipAddress1, ipAddress2)

                Dim portStart As Integer = Math.Min(port1, port2)
                Dim portEnd As Integer = Math.Max(port1, port2)

                _ports.Clear()
                For port As Integer = portStart To portEnd
                    _ports.Add(port)
                Next
            End Sub

            Public Sub StartScan()
                _scanThread = New Thread(AddressOf ScanAsync)
                _scanThread.SetApartmentState(ApartmentState.STA)
                _scanThread.IsBackground = True
                _scanThread.Start()
            End Sub

            Public Sub StopScan()
                _scanThread.Abort()
            End Sub

            Public Shared Function IsPortOpen(hostname As String, port As String) As Boolean
                Try
                    ' ReSharper disable UnusedVariable
                    Dim tcpClient As New TcpClient(hostname, port)
                    ' ReSharper restore UnusedVariable
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function

#End Region

#Region "Private Methods"

            Private Sub ScanAsync()
                Try
                    Dim hostCount = 0

                    For Each ipAddress As IPAddress In _ipAddresses
                        RaiseEvent BeginHostScan(ipAddress.ToString())

                        Dim scanHost As New ScanHost(ipAddress.ToString())
                        hostCount += 1

                        If Not IsHostAlive(ipAddress) Then
                            scanHost.ClosedPorts.AddRange(_ports)
                            scanHost.SetAllProtocols(False)
                        Else
                            For Each port As Integer In _ports
                                Dim isPortOpen As Boolean

                                Try
                                    ' ReSharper disable UnusedVariable
                                    Dim tcpClient As New TcpClient(ipAddress.ToString, port)
                                    ' ReSharper restore UnusedVariable

                                    isPortOpen = True
                                    scanHost.OpenPorts.Add(port)
                                Catch ex As Exception
                                    isPortOpen = False
                                    scanHost.ClosedPorts.Add(port)
                                End Try

                                Select Case port
                                    Case ScanHost.SSHPort
                                        scanHost.SSH = isPortOpen
                                    Case ScanHost.TelnetPort
                                        scanHost.Telnet = isPortOpen
                                    Case ScanHost.HTTPPort
                                        scanHost.HTTP = isPortOpen
                                    Case ScanHost.HTTPSPort
                                        scanHost.HTTPS = isPortOpen
                                    Case ScanHost.RloginPort
                                        scanHost.Rlogin = isPortOpen
                                    Case ScanHost.RDPPort
                                        scanHost.RDP = isPortOpen
                                    Case ScanHost.VNCPort
                                        scanHost.VNC = isPortOpen
                                End Select
                            Next
                        End If

                        Try
                            scanHost.HostName = Dns.GetHostEntry(scanHost.HostIp).HostName
                        Catch ex As Exception
                        End Try
                        If String.IsNullOrEmpty(scanHost.HostName) Then scanHost.HostName = scanHost.HostIp

                        _scannedHosts.Add(scanHost)
                        RaiseEvent HostScanned(scanHost, hostCount, _ipAddresses.Count)
                    Next

                    RaiseEvent ScanComplete(_scannedHosts)
                Catch ex As Exception
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "StartScanBG failed (Tools.PortScan)" & vbNewLine & ex.ToString(), True)
                End Try
            End Sub

            Private Shared Function IsHostAlive(ipAddress As IPAddress) As Boolean
                Dim pingSender As New Ping
                Dim pingReply As PingReply

                Try
                    pingReply = pingSender.Send(ipAddress)

                    If pingReply.Status = IPStatus.Success Then
                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Function

            Private Shared Function IpAddressArrayFromRange(ipAddress1 As IPAddress, ipAddress2 As IPAddress) _
                As IPAddress()
                Dim startIpAddress As IPAddress = IpAddressMin(ipAddress1, ipAddress2)
                Dim endIpAddress As IPAddress = IpAddressMax(ipAddress1, ipAddress2)

                Dim startAddress As Int32 = IpAddressToInt32(startIpAddress)
                Dim endAddress As Int32 = IpAddressToInt32(endIpAddress)
                Dim addressCount As Integer = endAddress - startAddress

                Dim addressArray(addressCount) As IPAddress
                Dim index = 0
                For address As Int32 = startAddress To endAddress
                    addressArray(index) = IpAddressFromInt32(address)
                    index = index + 1
                Next

                Return addressArray
            End Function

            Private Shared Function IpAddressMin(ipAddress1 As IPAddress, ipAddress2 As IPAddress) As IPAddress
                If (IpAddressCompare(ipAddress1, ipAddress2) < 0) Then ' ipAddress1 < ipAddress2
                    Return ipAddress1
                Else
                    Return ipAddress2
                End If
            End Function

            Private Shared Function IpAddressMax(ipAddress1 As IPAddress, ipAddress2 As IPAddress) As IPAddress
                If (IpAddressCompare(ipAddress1, ipAddress2) > 0) Then ' ipAddress1 > ipAddress2
                    Return ipAddress1
                Else
                    Return ipAddress2
                End If
            End Function

            Private Shared Function IpAddressCompare(ipAddress1 As IPAddress, ipAddress2 As IPAddress) As Integer
                Return IpAddressToInt32(ipAddress1) - IpAddressToInt32(ipAddress2)
            End Function

            Private Shared Function IpAddressToInt32(ipAddress As IPAddress) As Int32
                If Not ipAddress.AddressFamily = AddressFamily.InterNetwork Then _
                    Throw New ArgumentException("ipAddress")

                Dim addressBytes As Byte() = ipAddress.GetAddressBytes() ' in network order (big-endian)
                If BitConverter.IsLittleEndian Then Array.Reverse(addressBytes) ' to host order (little-endian)
                Debug.Assert(addressBytes.Length = 4)

                Return BitConverter.ToInt32(addressBytes, 0)
            End Function

            Private Shared Function IpAddressFromInt32(ipAddress As Int32) As IPAddress
                Dim addressBytes As Byte() = BitConverter.GetBytes(ipAddress) ' in host order
                If BitConverter.IsLittleEndian Then Array.Reverse(addressBytes) ' to network order (big-endian)
                Debug.Assert(addressBytes.Length = 4)

                Return New IPAddress(addressBytes)
            End Function

#End Region

#Region "Events"

            Public Event BeginHostScan(host As String)
            Public Event HostScanned(scanHost As ScanHost, scannedHostCount As Integer, totalHostCount As Integer)
            Public Event ScanComplete(hosts As List(Of ScanHost))

#End Region
        End Class
    End Namespace

End Namespace