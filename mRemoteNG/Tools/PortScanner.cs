using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Messages;


namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class PortScanner
    {
        private readonly List<IPAddress> _ipAddresses = new List<IPAddress>();
        private readonly List<int> _ports = new List<int>();
        private Thread _scanThread;
        private readonly List<ScanHost> _scannedHosts = new List<ScanHost>();
        private readonly int _timeoutInMilliseconds;

        #region Public Methods

        public PortScanner(IPAddress ipAddress1,
                           IPAddress ipAddress2,
                           int port1,
                           int port2,
                           int timeoutInMilliseconds = 5000,
                           bool checkDefaultPortsOnly = false)
        {
            var ipAddressStart = IpAddressMin(ipAddress1, ipAddress2);
            var ipAddressEnd = IpAddressMax(ipAddress1, ipAddress2);

            var portStart = Math.Min(port1, port2);
            var portEnd = Math.Max(port1, port2);

            // if only one port was specified, just scan the one port...
            if (portStart == 0)
                portStart = portEnd;

            if (timeoutInMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutInMilliseconds));

            _timeoutInMilliseconds = timeoutInMilliseconds;
            _ports.Clear();

            if (checkDefaultPortsOnly)
                _ports.AddRange(new[]
                {
                    ScanHost.SshPort, ScanHost.TelnetPort, ScanHost.HttpPort, ScanHost.HttpsPort, ScanHost.RloginPort,
                    ScanHost.RdpPort, ScanHost.VncPort
                });
            else
            {
                for (var port = portStart; port <= portEnd; port++)
                {
                    _ports.Add(port);
                }
            }

            _ipAddresses.Clear();
            _ipAddresses.AddRange(IpAddressArrayFromRange(ipAddressStart, ipAddressEnd));

            _scannedHosts.Clear();
        }

        public void StartScan()
        {
            _scanThread = new Thread(ScanAsync);

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _scanThread.SetApartmentState(ApartmentState.STA);

            _scanThread.IsBackground = true;
            _scanThread.Start();
        }

        public void StopScan()
        {
            foreach (var p in _pings)
            {
                p.SendAsyncCancel();
            }

            // Obsolete: https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/thread-abort-obsolete
            //_scanThread.Abort();
        }

        public static bool IsPortOpen(string hostname, string port)
        {
            try
            {
                var tcpClient = new TcpClient(hostname, Convert.ToInt32(port));
                tcpClient.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        private int _hostCount;
        private readonly List<Ping> _pings = new List<Ping>();

        private void ScanAsync()
        {
            try
            {
                _hostCount = 0;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Tools.PortScan: Starting scan of {_ipAddresses.Count} hosts...", true);
                foreach (var ipAddress in _ipAddresses)
                {
                    RaiseBeginHostScanEvent(ipAddress);

                    var pingSender = new Ping();
                    _pings.Add(pingSender);

                    try
                    {
                        pingSender.PingCompleted += PingSender_PingCompleted;
                        pingSender.SendAsync(ipAddress, _timeoutInMilliseconds, ipAddress);
                    }
                    catch (Exception ex)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"Tools.PortScan: Ping failed for {ipAddress} {Environment.NewLine} {ex.Message}", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, $"StartScanBG failed (Tools.PortScan) {Environment.NewLine} {ex.Message}", true);
            }
        }

        /* Some examples found here:
         * http://stackoverflow.com/questions/2114266/convert-ping-application-to-multithreaded-version-to-increase-speed-c-sharp
         */
        private void PingSender_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            // used for clean up later...
            var p = (Ping)sender;

            // UserState is the IP Address
            var ip = e.UserState.ToString();
            var scanHost = new ScanHost(ip);
            _hostCount++;

            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                $"Tools.PortScan: Scanning {_hostCount} of {_ipAddresses.Count} hosts: {scanHost.HostIp}",
                                                true);


            if (e.Cancelled)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Tools.PortScan: CANCELLED host: {scanHost.HostIp}", true);
                // cleanup
                p.PingCompleted -= PingSender_PingCompleted;
                p.Dispose();
                return;
            }

            if (e.Error != null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Ping failed to {e.UserState} {Environment.NewLine} {e.Error.Message}",
                                                    true);
                scanHost.ClosedPorts.AddRange(_ports);
                scanHost.SetAllProtocols(false);
            }
            else if (e.Reply.Status == IPStatus.Success)
            {
                /* ping was successful, try to resolve the hostname */
                try
                {
                    scanHost.HostName = Dns.GetHostEntry(scanHost.HostIp).HostName;
                }
                catch (Exception dnsex)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                        $"Tools.PortScan: Could not resolve {scanHost.HostIp} {Environment.NewLine} {dnsex.Message}",
                                                        true);
                }

                if (string.IsNullOrEmpty(scanHost.HostName))
                {
                    scanHost.HostName = scanHost.HostIp;
                }

                foreach (var port in _ports)
                {
                    bool isPortOpen;
                    try
                    {
                        var tcpClient = new TcpClient(ip, port);
                        isPortOpen = true;
                        scanHost.OpenPorts.Add(port);
                        tcpClient.Close();
                    }
                    catch (Exception)
                    {
                        isPortOpen = false;
                        scanHost.ClosedPorts.Add(port);
                    }

                    if (port == ScanHost.SshPort)
                    {
                        scanHost.Ssh = isPortOpen;
                    }
                    else if (port == ScanHost.TelnetPort)
                    {
                        scanHost.Telnet = isPortOpen;
                    }
                    else if (port == ScanHost.HttpPort)
                    {
                        scanHost.Http = isPortOpen;
                    }
                    else if (port == ScanHost.HttpsPort)
                    {
                        scanHost.Https = isPortOpen;
                    }
                    else if (port == ScanHost.RloginPort)
                    {
                        scanHost.Rlogin = isPortOpen;
                    }
                    else if (port == ScanHost.RdpPort)
                    {
                        scanHost.Rdp = isPortOpen;
                    }
                    else if (port == ScanHost.VncPort)
                    {
                        scanHost.Vnc = isPortOpen;
                    }
                }
            }
            else if (e.Reply.Status != IPStatus.Success)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    $"Ping did not complete to {e.UserState} : {e.Reply.Status}", true);
                scanHost.ClosedPorts.AddRange(_ports);
                scanHost.SetAllProtocols(false);
            }

            // cleanup
            p.PingCompleted -= PingSender_PingCompleted;
            p.Dispose();

            var h = string.IsNullOrEmpty(scanHost.HostName) ? "HostNameNotFound" : scanHost.HostName;
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                $"Tools.PortScan: Scan of {scanHost.HostIp} ({h}) complete.", true);

            _scannedHosts.Add(scanHost);
            RaiseHostScannedEvent(scanHost, _hostCount, _ipAddresses.Count);

            if (_scannedHosts.Count == _ipAddresses.Count)
                RaiseScanCompleteEvent(_scannedHosts);
        }

        private static IEnumerable<IPAddress> IpAddressArrayFromRange(IPAddress ipAddress1, IPAddress ipAddress2)
        {
            var startIpAddress = IpAddressMin(ipAddress1, ipAddress2);
            var endIpAddress = IpAddressMax(ipAddress1, ipAddress2);

            var startAddress = IpAddressToInt32(startIpAddress);
            var endAddress = IpAddressToInt32(endIpAddress);
            var addressCount = endAddress - startAddress;

            var addressArray = new IPAddress[addressCount + 1];
            var index = 0;
            for (var address = startAddress; address <= endAddress; address++)
            {
                addressArray[index] = IpAddressFromInt32(address);
                index++;
            }

            return addressArray;
        }

        private static IPAddress IpAddressMin(IPAddress ipAddress1, IPAddress ipAddress2)
        {
            return IpAddressCompare(ipAddress1, ipAddress2) < 0 ? ipAddress1 : ipAddress2;
        }

        private static IPAddress IpAddressMax(IPAddress ipAddress1, IPAddress ipAddress2)
        {
            return IpAddressCompare(ipAddress1, ipAddress2) > 0 ? ipAddress1 : ipAddress2;
        }

        private static int IpAddressCompare(IPAddress ipAddress1, IPAddress ipAddress2)
        {
            return IpAddressToInt32(ipAddress1) - IpAddressToInt32(ipAddress2);
        }

        private static int IpAddressToInt32(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                throw (new ArgumentException("ipAddress"));
            }

            var addressBytes = ipAddress.GetAddressBytes(); // in network order (big-endian)
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(addressBytes); // to host order (little-endian)
            }

            Debug.Assert(addressBytes.Length == 4);

            return BitConverter.ToInt32(addressBytes, 0);
        }

        private static IPAddress IpAddressFromInt32(int ipAddress)
        {
            var addressBytes = BitConverter.GetBytes(ipAddress); // in host order
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(addressBytes); // to network order (big-endian)
            }

            Debug.Assert(addressBytes.Length == 4);

            return new IPAddress(addressBytes);
        }

        #endregion

        #region Events

        public delegate void BeginHostScanEventHandler(string host);

        public event BeginHostScanEventHandler BeginHostScan;

        private void RaiseBeginHostScanEvent(IPAddress ipAddress)
        {
            BeginHostScan?.Invoke(ipAddress.ToString());
        }

        public delegate void HostScannedEventHandler(ScanHost scanHost, int scannedHostCount, int totalHostCount);

        public event HostScannedEventHandler HostScanned;

        private void RaiseHostScannedEvent(ScanHost scanHost, int scannedHostCount, int totalHostCount)
        {
            HostScanned?.Invoke(scanHost, scannedHostCount, totalHostCount);
        }

        public delegate void ScanCompleteEventHandler(List<ScanHost> hosts);

        public event ScanCompleteEventHandler ScanComplete;

        private void RaiseScanCompleteEvent(List<ScanHost> hosts)
        {
            ScanComplete?.Invoke(hosts);
        }

        #endregion
    }
}