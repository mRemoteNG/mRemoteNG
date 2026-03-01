using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public static class SecureCRTIniDeserializer
    {
        /// <summary>
        /// Parses a single SecureCRT .ini session file into a ConnectionInfo.
        /// SecureCRT stores sessions as INI-like files with typed prefixes:
        ///   S:"Key"=StringValue
        ///   D:"Key"=HexDword
        ///   Z:"Key"=HexEncodedString
        /// </summary>
        public static ConnectionInfo? Deserialize(string content, string sessionName)
        {
            Dictionary<string, string> strings = new(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, uint> dwords = new(StringComparer.OrdinalIgnoreCase);

            using StringReader reader = new(content);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length == 0 || line.StartsWith(';'))
                    continue;

                if (line.StartsWith("S:\"", StringComparison.Ordinal))
                    ParseStringEntry(line, strings);
                else if (line.StartsWith("D:\"", StringComparison.Ordinal))
                    ParseDwordEntry(line, dwords);
            }

            string hostname = GetString(strings, "Hostname");
            if (string.IsNullOrWhiteSpace(hostname))
                return null;

            ConnectionInfo connectionInfo = new()
            {
                Name = sessionName,
                Hostname = hostname,
                Username = GetString(strings, "Username"),
                Protocol = GetProtocol(strings),
                Description = GetString(strings, "Description")
            };

            connectionInfo.Port = GetPort(strings, dwords, connectionInfo.Protocol);

            return connectionInfo;
        }

        private static void ParseStringEntry(string line, Dictionary<string, string> dict)
        {
            // Format: S:"Key"=Value
            int firstQuote = line.IndexOf('"');
            int secondQuote = line.IndexOf('"', firstQuote + 1);
            if (firstQuote < 0 || secondQuote < 0)
                return;

            string key = line[(firstQuote + 1)..secondQuote];
            int equalsPos = line.IndexOf('=', secondQuote);
            if (equalsPos < 0)
                return;

            string value = line[(equalsPos + 1)..];
            dict[key] = value;
        }

        private static void ParseDwordEntry(string line, Dictionary<string, uint> dict)
        {
            // Format: D:"Key"=HexValue
            int firstQuote = line.IndexOf('"');
            int secondQuote = line.IndexOf('"', firstQuote + 1);
            if (firstQuote < 0 || secondQuote < 0)
                return;

            string key = line[(firstQuote + 1)..secondQuote];
            int equalsPos = line.IndexOf('=', secondQuote);
            if (equalsPos < 0)
                return;

            string hexValue = line[(equalsPos + 1)..].Trim();
            if (uint.TryParse(hexValue, System.Globalization.NumberStyles.HexNumber, null, out uint value))
                dict[key] = value;
        }

        private static string GetString(Dictionary<string, string> strings, string key)
        {
            return strings.TryGetValue(key, out string? value) ? value : string.Empty;
        }

        private static ProtocolType GetProtocol(Dictionary<string, string> strings)
        {
            string protocol = GetString(strings, "Protocol Name").ToUpperInvariant();
            return protocol switch
            {
                "SSH2" => ProtocolType.SSH2,
                "SSH1" => ProtocolType.SSH1,
                "TELNET" => ProtocolType.Telnet,
                "RLOGIN" => ProtocolType.Rlogin,
                "RDP" => ProtocolType.RDP,
                "RAW" => ProtocolType.RAW,
                "SERIAL" or "TAPI" => ProtocolType.RAW,
                _ => ProtocolType.SSH2
            };
        }

        private static int GetPort(Dictionary<string, string> strings, Dictionary<string, uint> dwords, ProtocolType protocol)
        {
            string portKey = protocol switch
            {
                ProtocolType.SSH1 => "[SSH1] Port",
                ProtocolType.SSH2 => "[SSH2] Port",
                _ => "Port"
            };

            if (dwords.TryGetValue(portKey, out uint portValue) && portValue > 0)
                return (int)portValue;

            // Fallback defaults
            return protocol switch
            {
                ProtocolType.SSH1 or ProtocolType.SSH2 => 22,
                ProtocolType.Telnet => 23,
                ProtocolType.RDP => 3389,
                _ => 22
            };
        }
    }
}
