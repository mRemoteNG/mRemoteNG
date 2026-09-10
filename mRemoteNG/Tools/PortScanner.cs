using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class PortScanner
    {
        private readonly List<IPAddress> _ipAddresses = [];
        private readonly List<int> _ports = [];
        private Thread _scanThread;
        private readonly List<ScanHost> _scannedHosts = [];
        private readonly int _timeoutInMilliseconds;

        #region Public Methods

        /// <summary>
        /// Scans an explicit list of ports on every address in the range, inclusive.
        /// </summary>
        public PortScanner(IPAddress ipAddress1,
                           IPAddress ipAddress2,
                           IEnumerable<int> ports,
                           int timeoutInMilliseconds = 5000)
        {
            ArgumentNullException.ThrowIfNull(ports);

            IPAddress ipAddressStart = IpAddressMin(ipAddress1, ipAddress2);
            IPAddress ipAddressEnd = IpAddressMax(ipAddress1, ipAddress2);

            if (timeoutInMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutInMilliseconds));

            // Materialise once: the sequence may be lazy, and validating it separately from
            // AddRange would otherwise enumerate it twice.
            List<int> requestedPorts = [.. ports];
            ValidatePorts(requestedPorts, nameof(ports));

            _timeoutInMilliseconds = timeoutInMilliseconds;
            _ports.Clear();
            _ports.AddRange(requestedPorts);

            _ipAddresses.Clear();
            _ipAddresses.AddRange(IpAddressArrayFromRange(ipAddressStart, ipAddressEnd));

            _scannedHosts.Clear();
        }

        public PortScanner(IPAddress ipAddress1,
                           IPAddress ipAddress2,
                           int port1,
                           int port2,
                           int timeoutInMilliseconds = 5000,
                           bool checkDefaultPortsOnly = false)
        {
            IPAddress ipAddressStart = IpAddressMin(ipAddress1, ipAddress2);
            IPAddress ipAddressEnd = IpAddressMax(ipAddress1, ipAddress2);

            int portStart = Math.Min(port1, port2);
            int portEnd = Math.Max(port1, port2);

            // if only one port was specified, just scan the one port...
            if (portStart == 0)
                portStart = portEnd;

            if (timeoutInMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutInMilliseconds));

            _timeoutInMilliseconds = timeoutInMilliseconds;
            _ports.Clear();

            if (checkDefaultPortsOnly)
                // port1/port2 are ignored in this mode, so they are deliberately not validated.
                _ports.AddRange(new[]
                {
                    ScanHost.SshPort, ScanHost.TelnetPort, ScanHost.HttpPort, ScanHost.HttpsPort, ScanHost.RloginPort,
                    ScanHost.RdpPort, ScanHost.VncPort
                });
            else
            {
                // Validated after the 0-means-unspecified rule above has been applied, so passing
                // (0, 3389) still scans the single port, and before the loop expands the range, so
                // an absurd endpoint cannot allocate its way to a million entries first.
                ValidatePort(portStart, nameof(port1));
                ValidatePort(portEnd, nameof(port2));

                for (int port = portStart; port <= portEnd; port++)
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
            foreach (Ping p in _pings)
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
                TcpClient tcpClient = new(hostname, Convert.ToInt32(port));
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
        private readonly List<Ping> _pings = [];

        private void ScanAsync()
        {
            try
            {
                _hostCount = 0;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Tools.PortScan: Starting scan of {_ipAddresses.Count} hosts...", true);
                foreach (IPAddress ipAddress in _ipAddresses)
                {
                    RaiseBeginHostScanEvent(ipAddress);

                    Ping pingSender = new();
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
            Ping p = (Ping)sender;

            // UserState is the IP Address
            string ip = e.UserState.ToString();
            ScanHost scanHost = new(ip);
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

                foreach (int port in _ports)
                {
                    bool isPortOpen;
                    try
                    {
                        TcpClient tcpClient = new(ip, port);
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

            string h = string.IsNullOrEmpty(scanHost.HostName) ? "HostNameNotFound" : scanHost.HostName;
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                $"Tools.PortScan: Scan of {scanHost.HostIp} ({h}) complete.", true);

            _scannedHosts.Add(scanHost);
            RaiseHostScannedEvent(scanHost, _hostCount, _ipAddresses.Count);

            if (_scannedHosts.Count == _ipAddresses.Count)
                RaiseScanCompleteEvent(_scannedHosts);
        }

        /// <summary>
        /// Rejects an unusable port list up front. Without this an empty list scans every host for
        /// nothing, and an out-of-range value only surfaces much later as a failure inside the
        /// per-port TcpClient connect, by which time the scan is already running.
        /// </summary>
        private static void ValidatePorts(List<int> ports, string paramName)
        {
            if (ports.Count == 0)
                throw new ArgumentException(Language.PortScanCustomPortsHint, paramName);

            foreach (int port in ports)
            {
                ValidatePort(port, paramName);
            }
        }

        /// <summary>Rejects a single port outside the usable 1..65535 range.</summary>
        private static void ValidatePort(int port, string paramName)
        {
            if (port is < PortListParser.MinPort or > PortListParser.MaxPort)
                throw new ArgumentOutOfRangeException(paramName,
                    string.Format(CultureInfo.CurrentCulture, Language.PortScanInvalidPort,
                                  port, PortListParser.MinPort, PortListParser.MaxPort));
        }

        /// <summary>
        /// Caps the number of addresses a single scan may enumerate. Every address in the range is
        /// pinged, so this is a practical scan limit as much as a guard against an IPv6 range - which
        /// can span an astronomically large number of addresses - exhausting memory.
        /// </summary>
        private const long MaxScanRange = 65536;

        private static IEnumerable<IPAddress> IpAddressArrayFromRange(IPAddress ipAddress1, IPAddress ipAddress2)
        {
            if (ipAddress1.AddressFamily != ipAddress2.AddressFamily)
                throw new ArgumentException(Language.PortScanMixedAddressFamilies);

            AddressFamily family = ipAddress1.AddressFamily;

            // Addresses are treated as UNSIGNED big-endian integers so ordering and counting are
            // correct across the whole space (e.g. an IPv4 range straddling 128.0.0.0). BigInteger
            // covers both the 32-bit IPv4 and 128-bit IPv6 spaces.
            BigInteger startAddress = IpAddressToBigInteger(IpAddressMin(ipAddress1, ipAddress2));
            BigInteger endAddress = IpAddressToBigInteger(IpAddressMax(ipAddress1, ipAddress2));

            BigInteger addressCount = endAddress - startAddress + 1;
            if (addressCount > MaxScanRange)
                throw new ArgumentOutOfRangeException(paramName: null,
                    string.Format(CultureInfo.CurrentCulture, Language.PortScanRangeTooLarge,
                                  addressCount, MaxScanRange));

            List<IPAddress> addresses = new((int)addressCount);
            for (BigInteger address = startAddress; address <= endAddress; address++)
            {
                addresses.Add(IpAddressFromBigInteger(address, family));
            }

            return addresses;
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
            return IpAddressToBigInteger(ipAddress1).CompareTo(IpAddressToBigInteger(ipAddress2));
        }

        private static BigInteger IpAddressToBigInteger(IPAddress ipAddress)
        {
            // GetAddressBytes() is big-endian (network order). Interpret it as an unsigned value.
            return new BigInteger(ipAddress.GetAddressBytes(), isUnsigned: true, isBigEndian: true);
        }

        private static IPAddress IpAddressFromBigInteger(BigInteger value, AddressFamily family)
        {
            int length = family == AddressFamily.InterNetworkV6 ? 16 : 4;
            byte[] addressBytes = new byte[length];

            // ToByteArray gives the minimal big-endian representation; right-align it into a
            // fixed-width, zero-padded buffer so IPAddress gets a valid 4- or 16-byte address.
            byte[] raw = value.ToByteArray(isUnsigned: true, isBigEndian: true);
            int copyLength = Math.Min(raw.Length, length);
            Array.Copy(raw, raw.Length - copyLength, addressBytes, length - copyLength, copyLength);

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