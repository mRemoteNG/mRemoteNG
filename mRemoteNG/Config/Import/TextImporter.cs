using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using mRemoteNG.Connection;
using mRemoteNG.Container;

namespace mRemoteNG.Config.Import
{
    public class TextImporter : IConnectionImporter<string>
    {
        public void Import(string source, ContainerInfo destinationContainer)
        {
            if (string.IsNullOrWhiteSpace(source)) return;

            var lines = source.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

                var connectionInfo = ParseLine(trimmedLine);
                if (connectionInfo != null)
                {
                    destinationContainer.AddChild(connectionInfo);
                }
            }
        }

        private static ConnectionInfo? ParseLine(string line)
        {
            // Simple heuristic: split by whitespace, comma, or semicolon
            // Assumes format: Host [User] [Password] [Port]
            
            var parts = line.Split(new[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0) return null;

            var connectionInfo = new ConnectionInfo();
            connectionInfo.Hostname = parts[0];
            connectionInfo.Name = parts[0]; // Default name to hostname
            connectionInfo.Inheritance.Hostname = false;

            if (parts.Length > 1)
            {
                connectionInfo.Username = parts[1];
                connectionInfo.Inheritance.Username = false;
            }

            if (parts.Length > 2)
            {
                connectionInfo.Password = parts[2];
                connectionInfo.Inheritance.Password = false;
            }
            
            if (parts.Length > 3)
            {
                if (int.TryParse(parts[3], out int port))
                {
                    connectionInfo.Port = port;
                    connectionInfo.Inheritance.Port = false;
                }
            }

            // Defaults
            connectionInfo.Protocol = Connection.Protocol.ProtocolType.RDP; // Default to RDP

            return connectionInfo;
        }
    }
}
