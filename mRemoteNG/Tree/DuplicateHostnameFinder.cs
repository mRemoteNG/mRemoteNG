using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Tree
{
    /// <summary>
    /// Finds other connections that point at the same host, so users can be
    /// warned before they unknowingly create a copy of an existing connection.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class DuplicateHostnameFinder
    {
        public static IEnumerable<ConnectionInfo> FindDuplicates(IEnumerable<ConnectionInfo> connections, ConnectionInfo connection)
        {
            string hostname = connection?.Hostname?.Trim() ?? "";
            if (hostname.Length == 0)
                return [];

            return connections.Where(other =>
                other != connection &&
                other is not ContainerInfo &&
                other is not PuttySessionInfo &&
                string.Equals(other.Hostname?.Trim(), hostname, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Builds a readable location such as "Datacenter / SQL / sql01".
        /// The top-level root node is left out because every connection shares it.
        /// </summary>
        public static string GetTreePath(ConnectionInfo connection)
        {
            List<string> names = [connection.Name];
            for (ContainerInfo parent = connection.Parent; parent != null && parent is not RootNodeInfo; parent = parent.Parent)
                names.Insert(0, parent.Name);

            return string.Join(" / ", names);
        }
    }
}
