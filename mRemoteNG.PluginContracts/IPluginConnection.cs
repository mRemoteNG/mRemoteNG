namespace mRemoteNG.PluginContracts;

public interface IPluginConnection
{
    string Hostname { get; set; }

    bool IsContainer { get; }

    string Name { get; set; }

    string ProtocolId { get; }

    int Port { get; set; }

    string Username { get; set; }

    string Password { get; set; }

    string Domain { get; set; }

    string GetPluginProperty(string key, string defaultValue = "");

    IReadOnlyDictionary<string, string> GetPluginProperties();

    void SetPluginProperty(string key, string? value);

    bool TryGetPluginProperty(string key, out string value);
}
