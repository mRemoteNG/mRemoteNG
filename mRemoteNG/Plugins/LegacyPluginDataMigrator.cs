using mRemoteNG.Connection;

namespace mRemoteNG.Plugins;

internal static class LegacyPluginDataMigrator
{
    public const string AwsEnabledKey = "mRemoteNG.AWS.Enabled";
    public const string AwsInstanceIdKey = "mRemoteNG.AWS.EC2InstanceId";
    public const string AwsRegionKey = "mRemoteNG.AWS.EC2Region";

    public static void Migrate(ConnectionInfo connectionInfo)
    {
        bool hasAwsLegacyData = connectionInfo.ExternalAddressProvider == ExternalAddressProvider.AmazonWebServices ||
                                !string.IsNullOrWhiteSpace(connectionInfo.EC2InstanceId);

        if (!hasAwsLegacyData)
        {
            return;
        }

        if (!connectionInfo.TryGetPluginProperty(AwsEnabledKey, out _))
        {
            connectionInfo.SetPluginProperty(AwsEnabledKey, bool.TrueString);
        }

        if (!string.IsNullOrWhiteSpace(connectionInfo.EC2InstanceId) &&
            !connectionInfo.TryGetPluginProperty(AwsInstanceIdKey, out _))
        {
            connectionInfo.SetPluginProperty(AwsInstanceIdKey, connectionInfo.EC2InstanceId);
        }

        if (!string.IsNullOrWhiteSpace(connectionInfo.EC2Region) &&
            !connectionInfo.TryGetPluginProperty(AwsRegionKey, out _))
        {
            connectionInfo.SetPluginProperty(AwsRegionKey, connectionInfo.EC2Region);
        }
    }
}
