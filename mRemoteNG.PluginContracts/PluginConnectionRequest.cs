namespace mRemoteNG.PluginContracts;

public sealed class PluginConnectionRequest
{
    public string Name { get; init; } = string.Empty;

    public string Hostname { get; init; } = string.Empty;

    public int? Port { get; init; }

    public string ProtocolId { get; init; } = string.Empty;
}
