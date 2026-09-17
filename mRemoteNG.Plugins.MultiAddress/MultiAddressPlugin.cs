using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.MultiAddress;

/// <summary>
/// Adds separate hostname and IP address fields with primary-address selection and optional verification before connecting.
/// </summary>
public sealed class MultiAddressPlugin : IConnectionPropertyProviderPlugin, IConnectionAddressResolverPlugin
{
    private const string EnabledKey = "mRp.MultiAddress.Enabled";
    private const string HostnameKey = "mRp.MultiAddress.Hostname";
    private const string IpAddressKey = "mRp.MultiAddress.IpAddress";
    private const string UseIpAddressAsPrimaryKey = "mRp.MultiAddress.UseIpAddressAsPrimary";
    private const string VerifyHostnameMatchesIpKey = "mRp.MultiAddress.VerifyHostnameMatchesIp";

    private IPluginContext? _context;
    private readonly Func<string, CancellationToken, Task<IPAddress[]>> _addressResolver;

    /// <summary>
    /// Represents a hostname validation failure for the saved hostname field.
    /// </summary>
    public sealed class InvalidHostnameException(Exception innerException) : Exception("The saved hostname is invalid.", innerException);

    /// <summary>
    /// Creates the plugin using the default DNS resolver.
    /// </summary>
    public MultiAddressPlugin()
        : this(ResolveAddressesWithDnsAsync)
    {
    }

    /// <summary>
    /// Creates the plugin with a custom hostname resolver.
    /// </summary>
    /// <param name="addressResolver">Resolves a hostname to one or more IP addresses.</param>
    public MultiAddressPlugin(Func<string, CancellationToken, Task<IPAddress[]>> addressResolver)
    {
        _addressResolver = addressResolver ?? throw new ArgumentNullException(nameof(addressResolver));
    }

    /// <inheritdoc />
    public string Id => "mRp.MultiAddress";

    /// <inheritdoc />
    public string DisplayName => "Multi-address";

    /// <inheritdoc />
    public Version Version => new(1, 0, 0);

    /// <inheritdoc />
    public Version MinimumHostVersion => new(1, 0, 0);

    /// <inheritdoc />
    public IReadOnlyCollection<ConnectionPropertyDefinition> ConnectionProperties =>
    [
        new ConnectionPropertyDefinition
        {
            Key = EnabledKey,
            Category = GetString("Connection", "Connection"),
            DisplayName = GetString("MultiAddressEnableSeparateHostnameIp", "Enable separate hostname/IP"),
            Description = GetString("MultiAddressDescriptionEnableSeparateHostnameIp", "Store hostname and IP address separately and let the plugin choose which address to connect to."),
            PropertyType = PluginPropertyType.Boolean,
        },
        new ConnectionPropertyDefinition
        {
            Key = HostnameKey,
            Category = GetString("Connection", "Connection"),
            DisplayName = GetString("Hostname", "Hostname"),
            Description = GetString("MultiAddressDescriptionHostname", "Saved hostname used for DNS verification or as the connection target when IP is not primary."),
        },
        new ConnectionPropertyDefinition
        {
            Key = IpAddressKey,
            Category = GetString("HostnameIp", "Hostname/IP"),
            DisplayName = GetString("IP", "IP"),
            Description = GetString("MultiAddressDescriptionIpAddress", "Saved IP address used when it is primary or when hostname verification fails."),
        },
        new ConnectionPropertyDefinition
        {
            Key = UseIpAddressAsPrimaryKey,
            Category = GetString("HostnameIp", "Hostname/IP"),
            DisplayName = GetString("MultiAddressUseIpAddressAsPrimary", "Use IP address as primary"),
            Description = GetString("MultiAddressDescriptionUseIpAddressAsPrimary", "Connect with the saved IP address first when both hostname and IP address are configured."),
            PropertyType = PluginPropertyType.Boolean,
        },
        new ConnectionPropertyDefinition
        {
            Key = VerifyHostnameMatchesIpKey,
            Category = GetString("HostnameIp", "Hostname/IP"),
            DisplayName = GetString("MultiAddressVerifyHostnameMatchesIp", "Verify hostname matches IP"),
            Description = GetString("MultiAddressDescriptionVerifyHostnameMatchesIp", "Resolve the saved hostname before connecting and warn when it does not resolve to the saved IP address."),
            PropertyType = PluginPropertyType.Boolean,
        },
    ];

    /// <inheritdoc />
    public void Initialize(IPluginContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public bool CanResolve(IPluginConnection connection)
    {
        return !connection.IsContainer &&
               bool.TryParse(connection.GetPluginProperty(EnabledKey, bool.FalseString), out bool isEnabled) &&
               isEnabled &&
               (!string.IsNullOrWhiteSpace(GetTrimmedProperty(connection, HostnameKey)) ||
                !string.IsNullOrWhiteSpace(GetTrimmedProperty(connection, IpAddressKey)));
    }

    /// <inheritdoc />
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
            _context?.Messages.Warning(string.Format(GetString("MultiAddressInvalidSavedIpAddress", "Multi-address plugin: '{0}' has an invalid saved IP address."), connection.Name));
            connection.Hostname = resolvedTarget;
            return;
        }

        IPAddress[] resolvedAddresses;
        try
        {
            resolvedAddresses = await ResolveHostAddressesAsync(hostname, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (InvalidHostnameException)
        {
            _context?.Messages.Warning(string.Format(GetString("MultiAddressInvalidSavedHostname", "Multi-address plugin: '{0}' has an invalid saved hostname. Using the saved IP address instead."), connection.Name));
            connection.Hostname = string.IsNullOrWhiteSpace(ipAddress) ? resolvedTarget : ipAddress;
            return;
        }

        if (resolvedAddresses.Length == 0)
        {
            _context?.Messages.Warning(string.Format(GetString("MultiAddressHostnameNotResolved", "Multi-address plugin: the saved hostname for '{0}' could not be resolved. Using the saved IP address instead."), connection.Name));
            connection.Hostname = string.IsNullOrWhiteSpace(ipAddress) ? resolvedTarget : ipAddress;
            return;
        }

        bool hostnameMatchesIpAddress = Array.Exists(resolvedAddresses, address => AddressesMatch(address, configuredIpAddress));
        if (!hostnameMatchesIpAddress)
        {
            _context?.Messages.Warning(string.Format(GetString("MultiAddressHostnameDidNotMatchIp", "Multi-address plugin: the saved hostname for '{0}' did not match the saved IP address. Using the saved IP address."), connection.Name));
            connection.Hostname = ipAddress;
            return;
        }

        connection.Hostname = resolvedTarget;
    }

    private static string GetTrimmedProperty(IPluginConnection connection, string key)
    {
        return connection.GetPluginProperty(key)?.Trim() ?? string.Empty;
    }

    private string GetString(string resourceName, string fallback)
    {
        return _context?.Resources.GetString(resourceName, fallback) ?? fallback;
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
            return await _addressResolver(hostname, cancellationToken).ConfigureAwait(false);
        }
        catch (SocketException)
        {
            return [];
        }
    }

    private static async Task<IPAddress[]> ResolveAddressesWithDnsAsync(string hostname, CancellationToken cancellationToken)
    {
        try
        {
            return await Dns.GetHostAddressesAsync(hostname, cancellationToken);
        }
        catch (ArgumentException ex)
        {
            throw new InvalidHostnameException(ex);
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
