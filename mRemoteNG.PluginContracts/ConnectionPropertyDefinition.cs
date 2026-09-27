namespace mRemoteNG.PluginContracts;

public sealed class ConnectionPropertyDefinition
{
    public required string Key { get; init; }

    public required string Category { get; init; }

    public required string DisplayName { get; init; }

    public string Description { get; init; } = string.Empty;

    public PluginPropertyType PropertyType { get; init; } = PluginPropertyType.String;

    public string DefaultValue { get; init; } = string.Empty;

    public IReadOnlyCollection<string> SupportedProtocols { get; init; } = [];
}
