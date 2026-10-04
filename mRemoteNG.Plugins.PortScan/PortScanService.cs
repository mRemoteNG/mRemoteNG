using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Numerics;

namespace mRemoteNG.Plugins.PortScan;

internal sealed class PortScanService
{
    /// <summary>
    /// Caps the number of addresses a single scan may enumerate. Every address in the range is
    /// pinged, so this is a practical scan limit as much as a guard against an IPv6 range - which
    /// can span an astronomically large number of addresses - exhausting memory.
    /// </summary>
    private const long MaxScanRange = 65536;

    /// <summary>
    /// Commonly scanned service ports: FTP/SSH/Telnet/SMTP/DNS/HTTP(S), Windows RPC/NetBIOS/SMB,
    /// LDAP(S), IMAP/POP3 (incl. TLS), rlogin, the usual databases, RDP, VNC, WinRM and common app
    /// ports. Every port backing a protocol column in the results list is included, so the
    /// SSH/Telnet/HTTP/HTTPS/Rlogin/RDP/VNC columns are still populated in this mode.
    /// </summary>
    public static readonly int[] CommonPorts =
    [
        21, 22, 23, 25, 53, 80, 110, 111, 135, 139, 143, 389, 443, 445, 465, 513, 587, 636, 993, 995,
        1433, 1521, 2049, 3306, 3389, 5432, 5900, 5985, 5986, 6379, 8080, 8443, 9200, 27017
    ];

    public async Task<IReadOnlyList<PortScanHostResult>> ScanAsync(
        IPAddress startAddress,
        IPAddress endAddress,
        IReadOnlyList<int> ports,
        int timeoutInMilliseconds,
        Action<string>? onBeginHostScan,
        Action<PortScanHostResult, int, int>? onHostScanned,
        CancellationToken cancellationToken)
    {
        List<IPAddress> addresses = ExpandAddresses(startAddress, endAddress);
        List<PortScanHostResult> results = [];

        for (int index = 0; index < addresses.Count; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IPAddress address = addresses[index];
            onBeginHostScan?.Invoke(address.ToString());

            PortScanHostResult result = await ScanHostAsync(address, ports, timeoutInMilliseconds, cancellationToken);
            results.Add(result);
            onHostScanned?.Invoke(result, index + 1, addresses.Count);
        }

        return results;
    }

    /// <summary>
    /// Enumerates every address between <paramref name="first"/> and <paramref name="last"/>
    /// inclusive. Addresses are treated as UNSIGNED big-endian integers so ordering and counting are
    /// correct across the whole space (e.g. an IPv4 range straddling 128.0.0.0); BigInteger covers
    /// both the 32-bit IPv4 and 128-bit IPv6 spaces. Throws when the endpoints mix address families
    /// or the range exceeds <see cref="MaxScanRange"/>.
    /// </summary>
    private static List<IPAddress> ExpandAddresses(IPAddress first, IPAddress last)
    {
        if (first.AddressFamily != last.AddressFamily)
        {
            throw new ArgumentException("A range cannot mix IPv4 and IPv6 addresses.");
        }

        AddressFamily family = first.AddressFamily;

        BigInteger start = ToBigInteger(first);
        BigInteger end = ToBigInteger(last);
        BigInteger min = BigInteger.Min(start, end);
        BigInteger max = BigInteger.Max(start, end);

        BigInteger addressCount = max - min + 1;
        if (addressCount > MaxScanRange)
        {
            throw new ArgumentOutOfRangeException(nameof(last),
                string.Format(CultureInfo.CurrentCulture,
                              "The range covers {0} addresses, which exceeds the {1} address scan limit.",
                              addressCount, MaxScanRange));
        }

        List<IPAddress> addresses = new((int)addressCount);
        for (BigInteger current = min; current <= max; current++)
        {
            addresses.Add(FromBigInteger(current, family));
        }

        return addresses;
    }

    private static async Task<PortScanHostResult> ScanHostAsync(
        IPAddress address,
        IEnumerable<int> ports,
        int timeoutInMilliseconds,
        CancellationToken cancellationToken)
    {
        PortScanHostResult result = new()
        {
            HostIp = address.ToString(),
        };

        bool hostAvailable = await PingAsync(address, timeoutInMilliseconds);
        if (hostAvailable)
        {
            result.HostName = await ResolveHostNameAsync(address);
            if (string.IsNullOrWhiteSpace(result.HostName))
            {
                result.HostName = result.HostIp;
            }

            foreach (int port in ports)
            {
                bool isOpen = await IsPortOpenAsync(address, port, timeoutInMilliseconds, cancellationToken);
                if (isOpen)
                {
                    result.OpenPorts.Add(port);
                }
                else
                {
                    result.ClosedPorts.Add(port);
                }

                switch (port)
                {
                    case 22:
                        result.Ssh = isOpen;
                        break;
                    case 23:
                        result.Telnet = isOpen;
                        break;
                    case 80:
                        result.Http = isOpen;
                        break;
                    case 443:
                        result.Https = isOpen;
                        break;
                    case 513:
                        result.Rlogin = isOpen;
                        break;
                    case 3389:
                        result.Rdp = isOpen;
                        break;
                    case 5900:
                        result.Vnc = isOpen;
                        break;
                }
            }
        }
        else
        {
            result.HostName = result.HostIp;
            result.ClosedPorts.AddRange(ports);
        }

        return result;
    }

    private static async Task<bool> PingAsync(IPAddress address, int timeoutInMilliseconds)
    {
        try
        {
            using Ping ping = new();
            PingReply reply = await ping.SendPingAsync(address, timeoutInMilliseconds);
            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<string> ResolveHostNameAsync(IPAddress address)
    {
        try
        {
            IPHostEntry entry = await Dns.GetHostEntryAsync(address);
            return entry.HostName;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static async Task<bool> IsPortOpenAsync(IPAddress address, int port, int timeoutInMilliseconds, CancellationToken cancellationToken)
    {
        try
        {
            using TcpClient client = new();
            Task connectTask = client.ConnectAsync(address, port);
            Task timeoutTask = Task.Delay(timeoutInMilliseconds, cancellationToken);
            Task completedTask = await Task.WhenAny(connectTask, timeoutTask);
            if (completedTask != connectTask)
            {
                return false;
            }

            await connectTask;
            return client.Connected;
        }
        catch
        {
            return false;
        }
    }

    private static BigInteger ToBigInteger(IPAddress address)
    {
        // GetAddressBytes() is big-endian (network order). Interpret it as an unsigned value.
        return new BigInteger(address.GetAddressBytes(), isUnsigned: true, isBigEndian: true);
    }

    private static IPAddress FromBigInteger(BigInteger value, AddressFamily family)
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
}
