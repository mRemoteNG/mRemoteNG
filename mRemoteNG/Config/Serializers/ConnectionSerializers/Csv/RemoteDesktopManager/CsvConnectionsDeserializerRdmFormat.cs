#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;

#endregion

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Csv.RemoteDesktopManager
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    ///     Import of connections from the Remote Desktop Manager (RDM) in a CSV file format
    /// </summary>
    public partial class CsvConnectionsDeserializerRdmFormat : IDeserializer<string, ConnectionTreeModel>
    {
        private readonly List<RemoteDesktopManagerConnectionInfo> _connectionTypes;
        private readonly HashSet<string> _groups;

        public CsvConnectionsDeserializerRdmFormat()
        {
            _connectionTypes =
        [
            new(ProtocolType.RDP, "RDP (Microsoft Remote Desktop)", 3389, "Remote Desktop"),
            new(ProtocolType.SSH2, "SSH Shell", 22, "SSH")
        ];

            _groups = [];

            Containers = [];
        }

        private List<ContainerInfo> Containers { get; }

        /// <summary>
        ///     Deserializes the CSV file into a <see cref="ConnectionTreeModel" />
        /// </summary>
        /// <param name="serializedData">Data from the CSV file</param>
        /// <returns></returns>
        public ConnectionTreeModel Deserialize(string serializedData)
        {
            string[] lines = serializedData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> csvHeaders = new();

            List<(ConnectionInfo, string)> connections = new(); // (ConnectionInfo, group)

            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                string[] line = lines[lineNumber].Split(',');
                if (lineNumber == 0)
                {
                    csvHeaders = line.ToList();
                }
                else
                {
                    (ConnectionInfo connectionInfo, string group) = ParseConnectionInfo(csvHeaders, line);
                    if (connectionInfo == default) continue;

                    connections.Add((connectionInfo, group));
                }
            }

            ConnectionTreeModel connectionTreeModel = new();
            ContainerInfo unsortedConnections = new() { Name = "Unsorted" };

            foreach (ContainerInfo containerInfo in Containers) connectionTreeModel.AddRootNode(containerInfo);

            List<ContainerInfo> allChildren = Containers.SelectMany(x => x.GetRecursiveChildList().Select(y => (ContainerInfo)y)).ToList();

            foreach ((ConnectionInfo connection, string path) in connections)
                if (string.IsNullOrEmpty(path))
                {
                    unsortedConnections.AddChild(connection);
                }
                else
                {
                    ContainerInfo container = allChildren.FirstOrDefault(x => x.ConstantID == path);
                    if (container == default) continue;

                    container.AddChild(connection);
                }

            if (unsortedConnections.HasChildren())
                connectionTreeModel.AddRootNode(unsortedConnections);

            return connectionTreeModel;
        }

        /// <summary>
        ///     Parses a line from the CSV file and returns <see cref="ConnectionInfo" />
        /// </summary>
        /// <param name="headers">CSV Headers</param>
        /// <param name="connectionCsv">CSV Columns</param>
        /// <returns></returns>
        private (ConnectionInfo connectionInfo, string) ParseConnectionInfo(IList<string> headers, IReadOnlyList<string> connectionCsv)
        {
            if (headers.Count != connectionCsv.Count) return default;

            string hostString = connectionCsv[headers.IndexOf("Host")].Trim();
            if (string.IsNullOrEmpty(hostString)) return default;

            UriHostNameType hostType = Uri.CheckHostName(hostString);
            if (hostType == UriHostNameType.Unknown) return default;

            string connectionTypeString = connectionCsv[headers.IndexOf("ConnectionType")];
            if (string.IsNullOrEmpty(connectionTypeString)) return default;

            RemoteDesktopManagerConnectionInfo connectionType = _connectionTypes.FirstOrDefault(x => x.Name == connectionTypeString);
            if (connectionType == default) return default;

            string portString = connectionCsv[headers.IndexOf("Port")] ?? connectionType.Port.ToString();
            if (!int.TryParse(portString, out int port)) port = connectionType.Port;

            string name = connectionCsv[headers.IndexOf("Name")];
            string description = connectionCsv[headers.IndexOf("Description")];
            string group = connectionCsv[headers.IndexOf("Group")];

            string username = connectionCsv[headers.IndexOf("CredentialUserName")];
            string domain = connectionCsv[headers.IndexOf("CredentialDomain")];
            string password = connectionCsv[headers.IndexOf("CredentialPassword")];

            ConnectionInfo connectionInfo = new(Guid.NewGuid().ToString())
            {
                Name = name,
                Hostname = hostString,
                Port = port,
                Username = username,
                Password = password?.ConvertToSecureString(),
                Domain = domain,
                Icon = connectionType.IconName ?? "mRemoteNG",
                Description = description,
                Protocol = connectionType.Protocol
            };

            if (!string.IsNullOrEmpty(group))
                if (group.Contains('\\'))
                {
                    List<string> groupParts = group.Split('\\').ToList();
                    string parentContainerName = groupParts[0];

                    ContainerInfo parentContainer = Containers.FirstOrDefault(x => x.Name == parentContainerName);
                    if (parentContainer == default)
                    {
                        parentContainer = new ContainerInfo(group) { Name = parentContainerName };
                        Containers.Add(parentContainer);
                    }

                    groupParts.RemoveAt(0);

                    AddChildrenRecursive(group, groupParts, parentContainer);
                }

            return string.IsNullOrEmpty(group) ? (connectionInfo, default) : (connectionInfo, group);
        }

        /// <summary>
        ///     Adds a child to a container recursively
        /// </summary>
        /// <param name="group">Full path of the RDM Grouping</param>
        /// <param name="groupParts">Segements of the group path</param>
        /// <param name="parentContainer">Parent container to add children to</param>
        private void AddChildrenRecursive(string group, IList<string> groupParts, ContainerInfo parentContainer)
        {
            if (_groups.Contains(group)) return;

            int groupCount = groupParts.Count;
            while (groupCount > 0)
            {
                string childName = groupParts[0];
                ContainerInfo newContainer = new(group) { Name = childName };

                List<string> childrenNames = parentContainer.GetRecursiveChildList().Select(x => x.Name).ToList();
                if (!childrenNames.Any())
                {
                    groupCount = AddChild(parentContainer, newContainer, groupCount);
                    _groups.Add(group);
                    continue;
                }

                if (groupParts.Count > 1)
                {
                    ContainerInfo childContainer = (ContainerInfo)parentContainer.Children.FirstOrDefault(x => x.Name == childName);
                    if (childContainer == default)
                    {
                        groupCount = AddChild(parentContainer, newContainer, groupCount);
                        continue;
                    }

                    AddChildrenRecursive(group, groupParts.Skip(1).ToList(), childContainer);
                }
                else
                {
                    parentContainer.AddChild(newContainer);
                    _groups.Add(group);
                }

                groupCount--;
            }
        }

        /// <summary>
        ///     Adds a child to a container and returns the remaining group count
        /// </summary>
        /// <param name="parentContainer">Parent container</param>
        /// <param name="newContainer">New child container</param>
        /// <param name="groupCount">Remaining group count</param>
        /// <returns></returns>
        private static int AddChild(ContainerInfo parentContainer, ContainerInfo newContainer, int groupCount)
        {
            parentContainer.AddChild(newContainer);
            groupCount--;
            return groupCount;
        }
    }

    /// <summary>
    ///     Record of supported connection types
    /// </summary>
    /// <param name="Protocol">Procotol</param>
    /// <param name="Name">Display Name</param>
    /// <param name="Port">Default Port</param>
    /// <param name="IconName">Icon Name</param>
    internal sealed record RemoteDesktopManagerConnectionInfo(ProtocolType Protocol, string Name, int Port, string IconName);
}