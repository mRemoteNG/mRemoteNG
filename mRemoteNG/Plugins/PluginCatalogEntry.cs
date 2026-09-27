namespace mRemoteNG.Plugins;

public sealed record PluginCatalogEntry(
    string PluginId,
    string DisplayName,
    string Version,
    bool IsEnabled);
