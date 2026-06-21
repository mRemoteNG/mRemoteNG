using System;
using System.Collections.Generic;

namespace mRemoteNG.Connection.Protocol.SshDotNet
{
    public enum PortForwardKind
    {
        Local,
        Remote,
        Dynamic
    }

    /// <summary>A single parsed port-forwarding rule.</summary>
    public sealed class PortForwardRule
    {
        public PortForwardKind Kind { get; }
        public string BindHost { get; }
        public uint BindPort { get; }
        /// <summary>Target host (null for <see cref="PortForwardKind.Dynamic"/>).</summary>
        public string Host { get; }
        /// <summary>Target port (0 for <see cref="PortForwardKind.Dynamic"/>).</summary>
        public uint Port { get; }

        private PortForwardRule(PortForwardKind kind, string bindHost, uint bindPort, string host, uint port)
        {
            Kind = kind;
            BindHost = bindHost;
            BindPort = bindPort;
            Host = host;
            Port = port;
        }

        public static PortForwardRule Local(string bindHost, uint bindPort, string host, uint port)
            => new(PortForwardKind.Local, bindHost, bindPort, host, port);

        public static PortForwardRule Remote(string bindHost, uint bindPort, string host, uint port)
            => new(PortForwardKind.Remote, bindHost, bindPort, host, port);

        public static PortForwardRule Dynamic(string bindHost, uint bindPort)
            => new(PortForwardKind.Dynamic, bindHost, bindPort, null, 0);
    }

    /// <summary>
    /// Parses port forwarding rules from the semicolon-separated string format.
    /// Format: L:localPort:remoteHost:remotePort | R:remotePort:localHost:localPort | D:localPort
    /// </summary>
    public static class PortForwardRuleParser
    {
        /// <summary>
        /// Parses the rule string into structured rules. Invalid rules are skipped (and logged);
        /// this method is pure with respect to the SSH connection, so it is fully unit-testable.
        /// </summary>
        public static IReadOnlyList<PortForwardRule> ParseRules(string rulesString)
        {
            var result = new List<PortForwardRule>();
            if (string.IsNullOrWhiteSpace(rulesString))
                return result;

            var rules = rulesString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var rule in rules)
            {
                var parsed = ParseRule(rule);
                if (parsed != null)
                    result.Add(parsed);
            }

            return result;
        }

        private static PortForwardRule ParseRule(string rule)
        {
            var parts = rule.Split(':');
            if (parts.Length < 2)
            {
                SshDotNetDiagnostics.LogWarning($"Tunnel: Invalid port forward rule (too few parts): '{rule}'");
                return null;
            }

            switch (parts[0].ToUpperInvariant())
            {
                case "L" when parts.Length == 4
                    && uint.TryParse(parts[1], out uint localPort)
                    && uint.TryParse(parts[3], out uint remotePort):
                    return PortForwardRule.Local("127.0.0.1", localPort, parts[2], remotePort);

                case "R" when parts.Length == 4
                    && uint.TryParse(parts[1], out uint rBindPort)
                    && uint.TryParse(parts[3], out uint lPort):
                    return PortForwardRule.Remote("0.0.0.0", rBindPort, parts[2], lPort);

                case "D" when parts.Length == 2
                    && uint.TryParse(parts[1], out uint socksPort):
                    return PortForwardRule.Dynamic("127.0.0.1", socksPort);

                default:
                    SshDotNetDiagnostics.LogWarning($"Tunnel: Unrecognized port forward rule: '{rule}'");
                    return null;
            }
        }

        /// <summary>Parses <paramref name="rulesString"/> and applies each rule to the tunnel manager.</summary>
        public static void ApplyRules(SshTunnelManager tunnelManager, string rulesString)
        {
            foreach (var rule in ParseRules(rulesString))
            {
                switch (rule.Kind)
                {
                    case PortForwardKind.Local:
                        tunnelManager.AddLocalForward(rule.BindHost, rule.BindPort, rule.Host, rule.Port);
                        break;
                    case PortForwardKind.Remote:
                        tunnelManager.AddRemoteForward(rule.BindHost, rule.BindPort, rule.Host, rule.Port);
                        break;
                    case PortForwardKind.Dynamic:
                        tunnelManager.AddDynamicForward(rule.BindHost, rule.BindPort);
                        break;
                }
            }
        }
    }
}
