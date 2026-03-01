using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Config.Import
{
    public class GuacamoleImporter
    {
        private readonly IDatabaseConnector _connector;

        public GuacamoleImporter(IDatabaseConnector connector)
        {
            _connector = connector;
        }

        public void Import(ContainerInfo destinationContainer)
        {
            try
            {
                if (!_connector.IsConnected)
                {
                    _connector.Connect();
                }

                var groups = FetchGroups();
                var connections = FetchConnections();
                var parameters = FetchParameters();

                // Build the tree
                // Map group ID to ContainerInfo
                var groupMap = new Dictionary<int, ContainerInfo>();
                
                // First pass: create all containers
                foreach (var group in groups)
                {
                    var container = new ContainerInfo
                    {
                        Name = group.Name,
                        // Guacamole groups can be balancing or organizational. We treat both as folders for now.
                    };
                    groupMap[group.Id] = container;
                }

                // Second pass: build hierarchy
                // Root group in Guacamole usually has parent_id NULL.
                // However, we are importing into a destination container, so top-level Guacamole groups go there.
                
                foreach (var group in groups)
                {
                    var container = groupMap[group.Id];
                    if (group.ParentId.HasValue && groupMap.TryGetValue(group.ParentId.Value, out var parentContainer))
                    {
                        parentContainer.AddChild(container);
                    }
                    else
                    {
                        // Top level group (or parent not found/root), add to destination
                        // Note: Guacamole root group is often "ROOT" with ID 1 and Parent NULL.
                        // We might want to skip the root group itself and import its children, or import it as a folder.
                        // Let's import it as a folder if it has a name, or treat its children as top-level if it's the absolute root.
                        // Typically, the user might want to import everything under a "Guacamole Import" folder, which destinationContainer likely is or receives.
                        
                        // If it's the root group (ParentId null), we can add it to destination.
                        destinationContainer.AddChild(container);
                    }
                }

                // Third pass: add connections
                foreach (var conn in connections)
                {
                    var connectionInfo = CreateConnectionInfo(conn, parameters.Where(p => p.ConnectionId == conn.Id));
                    
                    if (conn.ParentId.HasValue && groupMap.TryGetValue(conn.ParentId.Value, out var connParentContainer))
                    {
                        connParentContainer.AddChild(connectionInfo);
                    }
                    else
                    {
                        destinationContainer.AddChild(connectionInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Guacamole Import failed.", ex);
                throw;
            }
            finally
            {
                // We don't dispose the connector here, the caller manages it.
            }
        }

        private List<GuacamoleGroup> FetchGroups()
        {
            var groups = new List<GuacamoleGroup>();
            using (var cmd = _connector.DbCommand("SELECT connection_group_id, parent_id, connection_group_name, type FROM guacamole_connection_group"))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new GuacamoleGroup
                        {
                            Id = Convert.ToInt32(reader["connection_group_id"], CultureInfo.InvariantCulture),
                            ParentId = GetNullableInt(reader["parent_id"]),
                            Name = reader["connection_group_name"]?.ToString() ?? string.Empty,
                            Type = reader["type"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return groups;
        }

        private List<GuacamoleConnection> FetchConnections()
        {
            var connections = new List<GuacamoleConnection>();
            using (var cmd = _connector.DbCommand("SELECT connection_id, parent_id, connection_name, protocol FROM guacamole_connection"))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        connections.Add(new GuacamoleConnection
                        {
                            Id = Convert.ToInt32(reader["connection_id"], CultureInfo.InvariantCulture),
                            ParentId = GetNullableInt(reader["parent_id"]),
                            Name = reader["connection_name"]?.ToString() ?? string.Empty,
                            Protocol = reader["protocol"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return connections;
        }

        private List<GuacamoleParameter> FetchParameters()
        {
            var parameters = new List<GuacamoleParameter>();
            using (var cmd = _connector.DbCommand("SELECT connection_id, parameter_name, parameter_value FROM guacamole_connection_parameter"))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        parameters.Add(new GuacamoleParameter
                        {
                            ConnectionId = Convert.ToInt32(reader["connection_id"], CultureInfo.InvariantCulture),
                            Name = reader["parameter_name"]?.ToString() ?? string.Empty,
                            Value = reader["parameter_value"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return parameters;
        }

        private static int? GetNullableInt(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        private static ConnectionInfo CreateConnectionInfo(GuacamoleConnection conn, IEnumerable<GuacamoleParameter> parameters)
        {
            var info = new ConnectionInfo
            {
                Name = conn.Name
            };

            var paramsDict = parameters.ToDictionary(p => p.Name, p => p.Value, StringComparer.Ordinal);

            // Map Protocol
            switch (conn.Protocol.ToLowerInvariant())
            {
                case "rdp":
                    info.Protocol = ProtocolType.RDP;
                    break;
                case "vnc":
                    info.Protocol = ProtocolType.VNC;
                    break;
                case "ssh":
                    info.Protocol = ProtocolType.SSH2;
                    break;
                case "telnet":
                    info.Protocol = ProtocolType.Telnet;
                    break;
                default:
                    // Fallback or ignore? Let's default to RDP or just keep it as is (which might default to RDP)
                    // If we don't support it, maybe SSH2 is a safe fallback or RDP.
                    info.Protocol = ProtocolType.RDP; 
                    break;
            }

            // Map Parameters
            if (paramsDict.TryGetValue("hostname", out var hostname)) info.Hostname = hostname;
            if (paramsDict.TryGetValue("port", out var port) && int.TryParse(port, out int portNum)) info.Port = portNum;
            if (paramsDict.TryGetValue("username", out var username)) info.Username = username;
            if (paramsDict.TryGetValue("password", out var password)) info.Password = password;
            if (paramsDict.TryGetValue("domain", out var domain)) info.Domain = domain;

            // RDP specific
            if (info.Protocol == ProtocolType.RDP)
            {
                if (paramsDict.TryGetValue("security", out var security))
                {
                   // Map security settings if needed
                }
                // ... map other RDP settings like width, height, color-depth, console, etc.
                if (paramsDict.TryGetValue("console", out var console) && string.Equals(console, "true", StringComparison.Ordinal)) info.UseConsoleSession = true;
            }

            return info;
        }

        private class GuacamoleGroup
        {
            public int Id { get; set; }
            public int? ParentId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
        }

        private class GuacamoleConnection
        {
            public int Id { get; set; }
            public int? ParentId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Protocol { get; set; } = string.Empty;
        }

        private class GuacamoleParameter
        {
            public int ConnectionId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }
    }
}
