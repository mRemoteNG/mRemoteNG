using System;
using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins;

internal static class PluginProtocolMapper
{
    private static readonly IReadOnlyDictionary<string, ProtocolType> PluginToCore =
        new Dictionary<string, ProtocolType>(StringComparer.OrdinalIgnoreCase)
        {
            [PluginProtocolIds.Ard] = ProtocolType.ARD,
            [PluginProtocolIds.Http] = ProtocolType.HTTP,
            [PluginProtocolIds.Https] = ProtocolType.HTTPS,
            [PluginProtocolIds.Rdp] = ProtocolType.RDP,
            [PluginProtocolIds.Rlogin] = ProtocolType.Rlogin,
            [PluginProtocolIds.Ssh2] = ProtocolType.SSH2,
            [PluginProtocolIds.Telnet] = ProtocolType.Telnet,
            [PluginProtocolIds.Vnc] = ProtocolType.VNC,
        };

    public static bool SupportsProtocol(ConnectionPropertyDefinition definition, ProtocolType protocolType)
    {
        if (definition.SupportedProtocols.Count == 0)
        {
            return true;
        }

        string protocolId = ToPluginProtocolId(protocolType);
        return definition.SupportedProtocols.Contains(protocolId, StringComparer.OrdinalIgnoreCase);
    }

    public static string ToPluginProtocolId(ProtocolType protocolType)
    {
        return protocolType switch
        {
            ProtocolType.ARD => PluginProtocolIds.Ard,
            ProtocolType.HTTP => PluginProtocolIds.Http,
            ProtocolType.HTTPS => PluginProtocolIds.Https,
            ProtocolType.RDP => PluginProtocolIds.Rdp,
            ProtocolType.Rlogin => PluginProtocolIds.Rlogin,
            ProtocolType.SSH2 => PluginProtocolIds.Ssh2,
            ProtocolType.Telnet => PluginProtocolIds.Telnet,
            ProtocolType.VNC => PluginProtocolIds.Vnc,
            _ => protocolType.ToString().ToLowerInvariant(),
        };
    }

    public static bool TryToCoreProtocolType(string protocolId, out ProtocolType protocolType)
    {
        return PluginToCore.TryGetValue(protocolId, out protocolType);
    }
}
