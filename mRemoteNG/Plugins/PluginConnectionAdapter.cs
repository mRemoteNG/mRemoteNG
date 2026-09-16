using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins;

internal sealed class PluginConnectionAdapter(ConnectionInfo connectionInfo) : IPluginConnection
{
    private readonly ConnectionInfo _connectionInfo = connectionInfo ?? throw new System.ArgumentNullException(nameof(connectionInfo));

    public string Hostname
    {
        get => _connectionInfo.Hostname;
        set => _connectionInfo.Hostname = value;
    }

    public bool IsContainer => _connectionInfo.IsContainer;

    public string Name
    {
        get => _connectionInfo.Name;
        set => _connectionInfo.Name = value;
    }

    public string ProtocolId => PluginProtocolMapper.ToPluginProtocolId(_connectionInfo.Protocol);

    public int Port
    {
        get => _connectionInfo.Port;
        set => _connectionInfo.Port = value;
    }

    public string Username
    {
        get => _connectionInfo.Username;
        set => _connectionInfo.Username = value;
    }

    public string Password
    {
        get => _connectionInfo.Password;
        set => _connectionInfo.Password = value;
    }

    public string Domain
    {
        get => _connectionInfo.Domain;
        set => _connectionInfo.Domain = value;
    }

    public string GetPluginProperty(string key, string defaultValue = "")
    {
        return _connectionInfo.GetPluginProperty(key, defaultValue);
    }

    public IReadOnlyDictionary<string, string> GetPluginProperties()
    {
        return _connectionInfo.PluginProperties;
    }

    public void SetPluginProperty(string key, string? value)
    {
        _connectionInfo.SetPluginProperty(key, value);
    }

    public bool TryGetPluginProperty(string key, out string value)
    {
        return _connectionInfo.TryGetPluginProperty(key, out value);
    }
}
