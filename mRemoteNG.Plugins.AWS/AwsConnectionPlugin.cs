using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExternalConnectors.AWS;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.AWS;

public sealed class AwsConnectionPlugin : IConnectionPropertyProviderPlugin, IConnectionAddressResolverPlugin
{
    private const string EnabledKey = "mRemoteNG.AWS.Enabled";
    private const string InstanceIdKey = "mRemoteNG.AWS.EC2InstanceId";
    private const string RegionKey = "mRemoteNG.AWS.EC2Region";

    public string Id => "mRemoteNG.AWS";

    public string DisplayName => "AWS";

    public Version Version => new(1, 0, 0);

    public Version MinimumHostVersion => new(1, 0, 0);

    public IReadOnlyCollection<ConnectionPropertyDefinition> ConnectionProperties => _connectionProperties;

    private static readonly IReadOnlyCollection<ConnectionPropertyDefinition> _connectionProperties =
    [
        new ConnectionPropertyDefinition
        {
            Key = EnabledKey,
            Category = "AWS",
            DisplayName = "Enable AWS hostname lookup",
            Description = "Resolve the connection hostname from the configured AWS EC2 instance before connecting.",
            PropertyType = PluginPropertyType.Boolean,
        },
        new ConnectionPropertyDefinition
        {
            Key = InstanceIdKey,
            Category = "AWS",
            DisplayName = "EC2 instance ID",
            Description = "The EC2 instance ID used for hostname lookup.",
        },
        new ConnectionPropertyDefinition
        {
            Key = RegionKey,
            Category = "AWS",
            DisplayName = "EC2 region",
            Description = "The AWS region used for hostname lookup.",
        },
    ];

    public void Initialize(IPluginContext context)
    {
    }

    public bool CanResolve(IPluginConnection connection)
    {
        return !connection.IsContainer &&
               bool.TryParse(connection.GetPluginProperty(EnabledKey, bool.FalseString), out bool isEnabled) &&
               isEnabled &&
               !string.IsNullOrWhiteSpace(connection.GetPluginProperty(InstanceIdKey)) &&
               !string.IsNullOrWhiteSpace(connection.GetPluginProperty(RegionKey));
    }

    public async Task ResolveAsync(IPluginConnection connection, CancellationToken cancellationToken)
    {
        if (!CanResolve(connection))
        {
            return;
        }

        cancellationToken.ThrowIfCancellationRequested();

        string instanceId = connection.GetPluginProperty(InstanceIdKey);
        string region = connection.GetPluginProperty(RegionKey);
        string hostname = await EC2FetchDataService.GetEC2InstanceDataAsync($"AWSAPI:{instanceId}", region).ConfigureAwait(true);
        if (!string.IsNullOrWhiteSpace(hostname))
        {
            connection.Hostname = hostname;
        }
    }
}
