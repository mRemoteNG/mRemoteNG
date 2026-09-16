using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace mRemoteNG.Plugins.PortScan;

internal sealed class PortScanService
{
    private static readonly int[] DefaultPorts =
    [
        22,
        23,
        80,
        443,
        513,
        3389,
        5900,
    ];

    public async Task<IReadOnlyList<PortScanHostResult>> ScanAsync(
        IPAddress startAddress,
        IPAddress endAddress,
        int firstPort,
        int lastPort,
        int timeoutInMilliseconds,
        bool scanDefaultPortsOnly,
        Action<string>? onBeginHostScan,
        Action<PortScanHostResult, int, int>? onHostScanned,
        CancellationToken cancellationToken)
    {
        List<IPAddress> addresses = ExpandAddresses(startAddress, endAddress).ToList();
        List<int> ports = BuildPortList(firstPort, lastPort, scanDefaultPortsOnly);
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

    private static List<int> BuildPortList(int firstPort, int lastPort, bool scanDefaultPortsOnly)
    {
        if (scanDefaultPortsOnly)
        {
            return [.. DefaultPorts];
        }

        int start = Math.Min(firstPort, lastPort);
        int end = Math.Max(firstPort, lastPort);
        if (start == 0)
        {
            start = end;
        }

        List<int> ports = [];
        for (int port = start; port <= end; port++)
        {
            ports.Add(port);
        }

        return ports;
    }

    private static IEnumerable<IPAddress> ExpandAddresses(IPAddress first, IPAddress last)
    {
        if (first.AddressFamily != AddressFamily.InterNetwork || last.AddressFamily != AddressFamily.InterNetwork)
        {
            throw new NotSupportedException("Only IPv4 ranges are supported.");
        }

        uint start = ToUInt32(first);
        uint end = ToUInt32(last);
        uint min = Math.Min(start, end);
        uint max = Math.Max(start, end);

        for (uint current = min; current <= max; current++)
        {
            yield return FromUInt32(current);
            if (current == uint.MaxValue)
            {
                yield break;
            }
        }
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

    private static uint ToUInt32(IPAddress address)
    {
        byte[] bytes = address.GetAddressBytes();
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        return BitConverter.ToUInt32(bytes, 0);
    }

    private static IPAddress FromUInt32(uint value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        return new IPAddress(bytes);
    }
}
