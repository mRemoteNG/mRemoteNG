namespace mRemoteNG.PluginContracts;

public interface IPluginConnection
{
    string Hostname { get; set; }

    bool IsContainer { get; }

    string Name { get; set; }

    string ProtocolId { get; }

    string GetPluginProperty(string key, string defaultValue = "");

    IReadOnlyDictionary<string, string> GetPluginProperties();

    void SetPluginProperty(string key, string? value);

    bool TryGetPluginProperty(string key, out string value);
}
