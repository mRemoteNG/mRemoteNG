using System;

namespace mRemoteNG.Connection.Protocol.SshDotNet
{
    /// <summary>
    /// Parses port forwarding rules from the semicolon-separated string format.
    /// Format: L:localPort:remoteHost:remotePort | R:remotePort:localHost:localPort | D:localPort
    /// </summary>
    public static class PortForwardRuleParser
    {
        public static void ApplyRules(SshTunnelManager tunnelManager, string rulesString)
        {
            if (string.IsNullOrWhiteSpace(rulesString)) return;

            var rules = rulesString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var rule in rules)
            {
                var parts = rule.Split(':');
                if (parts.Length < 2)
                {
                    SshDotNetDiagnostics.LogWarning($"Tunnel: Invalid port forward rule (too few parts): '{rule}'");
                    continue;
                }

                switch (parts[0].ToUpperInvariant())
                {
                    case "L" when parts.Length == 4
                        && uint.TryParse(parts[1], out uint localPort)
                        && uint.TryParse(parts[3], out uint remotePort):
                        tunnelManager.AddLocalForward("127.0.0.1", localPort, parts[2], remotePort);
                        break;

                    case "R" when parts.Length == 4
                        && uint.TryParse(parts[1], out uint rBindPort)
                        && uint.TryParse(parts[3], out uint lPort):
                        tunnelManager.AddRemoteForward("0.0.0.0", rBindPort, parts[2], lPort);
                        break;

                    case "D" when parts.Length == 2
                        && uint.TryParse(parts[1], out uint socksPort):
                        tunnelManager.AddDynamicForward("127.0.0.1", socksPort);
                        break;

                    default:
                        SshDotNetDiagnostics.LogWarning($"Tunnel: Unrecognized port forward rule: '{rule}'");
                        break;
                }
            }
        }
    }
}
