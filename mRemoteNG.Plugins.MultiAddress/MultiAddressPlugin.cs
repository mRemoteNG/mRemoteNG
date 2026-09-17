using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.MultiAddress;

public sealed class MultiAddressPlugin : IConnectionPropertyProviderPlugin, IConnectionAddressResolverPlugin
{
    private const string EnabledKey = "mRp.MultiAddress.Enabled";
    private const string HostnameKey = "mRp.MultiAddress.Hostname";
    private const string IpAddressKey = "mRp.MultiAddress.IpAddress";
    private const string UseIpAddressAsPrimaryKey = "mRp.MultiAddress.UseIpAddressAsPrimary";
    private const string VerifyHostnameMatchesIpKey = "mRp.MultiAddress.VerifyHostnameMatchesIp";

    private IPluginContext? _context;
    private readonly Func<string, CancellationToken, Task<IPAddress[]>> _addressResolver;

    public MultiAddressPlugin()
        : this((hostname, cancellationToken) => Dns.GetHostAddressesAsync(hostname, cancellationToken))
    {
    }

    public MultiAddressPlugin(Func<string, CancellationToken, Task<IPAddress[]>> addressResolver)
    {
        _addressResolver = addressResolver ?? throw new ArgumentNullException(nameof(addressResolver));
    }

    public string Id => "mRp.MultiAddress";

    public string DisplayName => "Multi-address";

    public Version Version => new(1, 0, 0);

    public Version MinimumHostVersion => new(1, 0, 0);

    public IReadOnlyCollection<ConnectionPropertyDefinition> ConnectionProperties => _connectionProperties;

    private static readonly IReadOnlyCollection<ConnectionPropertyDefinition> _connectionProperties =
    [
        new ConnectionPropertyDefinition
        {
            Key = EnabledKey,
            Category = "Address",
            DisplayName = "Enable separate hostname/IP",
            Description = "Store hostname and IP address separately and let the plugin choose which address to connect to.",
            PropertyType = PluginPropertyType.Boolean,
        },
        new ConnectionPropertyDefinition
        {
            Key = HostnameKey,
            Category = "Address",
            DisplayName = "Hostname",
            Description = "Saved hostname used for DNS verification or as the connection target when IP is not primary.",
        },
        new ConnectionPropertyDefinition
        {
            Key = IpAddressKey,
            Category = "Address",
            DisplayName = "IP address",
            Description = "Saved IP address used when it is primary or when hostname verification fails.",
        },
        new ConnectionPropertyDefinition
        {
            Key = UseIpAddressAsPrimaryKey,
            Category = "Address",
            DisplayName = "Use IP address as primary",
            Description = "Connect with the saved IP address first when both hostname and IP address are configured.",
            PropertyType = PluginPropertyType.Boolean,
        },
        new ConnectionPropertyDefinition
        {
            Key = VerifyHostnameMatchesIpKey,
            Category = "Address",
            DisplayName = "Verify hostname matches IP",
            Description = "Resolve the saved hostname before connecting and warn when it does not resolve to the saved IP address.",
            PropertyType = PluginPropertyType.Boolean,
        },
    ];

    public void Initialize(IPluginContext context)
    {
        _context = context;
    }

    public bool CanResolve(IPluginConnection connection)
    {
        return !connection.IsContainer &&
               bool.TryParse(connection.GetPluginProperty(EnabledKey, bool.FalseString), out bool isEnabled) &&
               isEnabled &&
               (!string.IsNullOrWhiteSpace(GetTrimmedProperty(connection, HostnameKey)) ||
                !string.IsNullOrWhiteSpace(GetTrimmedProperty(connection, IpAddressKey)));
    }

    public async Task ResolveAsync(IPluginConnection connection, CancellationToken cancellationToken)
    {
        if (!CanResolve(connection))
        {
            return;
        }

        string hostname = GetTrimmedProperty(connection, HostnameKey);
        string ipAddress = GetTrimmedProperty(connection, IpAddressKey);
        bool useIpAddressAsPrimary = GetBooleanProperty(connection, UseIpAddressAsPrimaryKey);
        bool verifyHostnameMatchesIp = GetBooleanProperty(connection, VerifyHostnameMatchesIpKey);

        string resolvedTarget = SelectPreferredAddress(hostname, ipAddress, useIpAddressAsPrimary);
        if (!verifyHostnameMatchesIp || string.IsNullOrWhiteSpace(hostname) || string.IsNullOrWhiteSpace(ipAddress))
        {
            connection.Hostname = resolvedTarget;
            return;
        }

        if (!IPAddress.TryParse(ipAddress, out IPAddress? configuredIpAddress))
        {
            _context?.Messages.Warning($"Multi-address plugin: '{connection.Name}' has an invalid saved IP address.");
            connection.Hostname = resolvedTarget;
            return;
        }

        IPAddress[] resolvedAddresses;
        try
        {
            resolvedAddresses = await ResolveHostAddressesAsync(hostname, cancellationToken);
        }
        catch (ArgumentException)
        {
            _context?.Messages.Warning($"Multi-address plugin: '{connection.Name}' has an invalid saved hostname. Using the saved IP address instead.");
            connection.Hostname = string.IsNullOrWhiteSpace(ipAddress) ? resolvedTarget : ipAddress;
            return;
        }

        if (resolvedAddresses.Length == 0)
        {
            _context?.Messages.Warning($"Multi-address plugin: the saved hostname for '{connection.Name}' could not be resolved. Using the saved IP address instead.");
            connection.Hostname = string.IsNullOrWhiteSpace(ipAddress) ? resolvedTarget : ipAddress;
            return;
        }

        bool hostnameMatchesIpAddress = Array.Exists(resolvedAddresses, address => AddressesMatch(address, configuredIpAddress));
        if (!hostnameMatchesIpAddress)
        {
            _context?.Messages.Warning($"Multi-address plugin: the saved hostname for '{connection.Name}' did not match the saved IP address. Using the saved IP address.");
            connection.Hostname = ipAddress;
            return;
        }

        connection.Hostname = resolvedTarget;
    }

    private static string GetTrimmedProperty(IPluginConnection connection, string key)
    {
        return connection.GetPluginProperty(key)?.Trim() ?? string.Empty;
    }

    private static bool GetBooleanProperty(IPluginConnection connection, string key)
    {
        return bool.TryParse(connection.GetPluginProperty(key, bool.FalseString), out bool value) && value;
    }

    private static string SelectPreferredAddress(string hostname, string ipAddress, bool useIpAddressAsPrimary)
    {
        if (useIpAddressAsPrimary)
        {
            return !string.IsNullOrWhiteSpace(ipAddress) ? ipAddress : hostname;
        }

        return !string.IsNullOrWhiteSpace(hostname) ? hostname : ipAddress;
    }

    private async Task<IPAddress[]> ResolveHostAddressesAsync(string hostname, CancellationToken cancellationToken)
    {
        try
        {
            return await _addressResolver(hostname, cancellationToken);
        }
        catch (SocketException)
        {
            return [];
        }
    }

    private static bool AddressesMatch(IPAddress left, IPAddress right)
    {
        return NormalizeAddress(left).Equals(NormalizeAddress(right));
    }

    private static IPAddress NormalizeAddress(IPAddress address)
    {
        return address.IsIPv4MappedToIPv6
            ? address.MapToIPv4()
            : address;
    }
}
