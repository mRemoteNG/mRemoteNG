#region

using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree;

#endregion

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Csv.RemoteDesktopManager;

public partial class CsvConnectionsDeserializerRdmFormat : IDeserializer<string, ConnectionTreeModel>
{
    private readonly List<RemoteDesktopManagerConnectionInfo> _connectionTypes;
    private readonly HashSet<string> _groups;

    public CsvConnectionsDeserializerRdmFormat()
    {
        _connectionTypes = new List<RemoteDesktopManagerConnectionInfo>
        {
            new(ProtocolType.RDP, "RDP (Microsoft Remote Desktop)", 3389),
            new(ProtocolType.SSH2, "SSH Shell", 22)
        };

        _groups = new HashSet<string>();

        Containers = new List<ContainerInfo>();
    }

    private List<ContainerInfo> Containers { get; }

    public ConnectionTreeModel Deserialize(string serializedData)
    {
        var lines = serializedData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var csvHeaders = new List<string>();

        var connections = new List<(ConnectionInfo, string)>(); // (ConnectionInfo, group)

        for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            var line = lines[lineNumber].Split(',');
            if (lineNumber == 0)
            {
                csvHeaders = line.ToList();
            }
            else
            {
                var (connectionInfo, group) = ParseConnectionInfo(csvHeaders, line);
                if (connectionInfo == default) continue;

                connections.Add((connectionInfo, group));
            }
        }

        var connectionTreeModel = new ConnectionTreeModel();
        var unsortedConnections = new ContainerInfo { Name = "Unsorted" };

        foreach (var containerInfo in Containers) connectionTreeModel.AddRootNode(containerInfo);

        var allChildren = Containers.SelectMany(x => x.GetRecursiveChildList().Select(y => (ContainerInfo)y)).ToList();

        foreach (var (connection, path) in connections)
            if (string.IsNullOrEmpty(path))
            {
                unsortedConnections.AddChild(connection);
            }
            else
            {
                var container = allChildren.FirstOrDefault(x => x.ConstantID == path);
                if (container == default) continue;

                container.AddChild(connection);
            }

        connectionTreeModel.AddRootNode(unsortedConnections);

        return connectionTreeModel;
    }

    private (ConnectionInfo connectionInfo, string) ParseConnectionInfo(IList<string> headers, IReadOnlyList<string> connectionCsv)
    {
        if (headers.Count != connectionCsv.Count) return default;

        var hostString = connectionCsv[headers.IndexOf("Host")].Trim();
        if (string.IsNullOrEmpty(hostString)) return default;

        var hostType = Uri.CheckHostName(hostString);
        if (hostType == UriHostNameType.Unknown) return default;

        var connectionTypeString = connectionCsv[headers.IndexOf("ConnectionType")];
        if (string.IsNullOrEmpty(connectionTypeString)) return default;

        var connectionType = _connectionTypes.FirstOrDefault(x => x.Name == connectionTypeString);
        if (connectionType == default) return default;

        var portString = connectionCsv[headers.IndexOf("Port")] ?? connectionType.Port.ToString();
        if (!int.TryParse(portString, out var port)) port = connectionType.Port;

        var name = connectionCsv[headers.IndexOf("Name")];
        var description = connectionCsv[headers.IndexOf("Description")];
        var group = connectionCsv[headers.IndexOf("Group")];

        var username = connectionCsv[headers.IndexOf("CredentialUserName")];
        var domain = connectionCsv[headers.IndexOf("CredentialDomain")];
        var password = connectionCsv[headers.IndexOf("CredentialPassword")];

        var connectionInfo = new ConnectionInfo(Guid.NewGuid().ToString())
        {
            Name = name,
            Hostname = hostString,
            Port = port,
            Username = username,
            Password = password,
            Domain = domain,
            Description = description,
            Protocol = connectionType.Protocol
        };

        if (!string.IsNullOrEmpty(group))
            if (group.Contains('\\'))
            {
                var groupParts = group.Split('\\').ToList();
                var parentContainerName = groupParts[0];
                var parentContainer = Containers.FirstOrDefault(x => x.Name == parentContainerName);
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

    private void AddChildrenRecursive(string group, IList<string> groupParts, ContainerInfo parentContainer)
    {
        if (_groups.Contains(group)) return;

        var groupCount = groupParts.Count;
        while (groupCount > 0)
        {
            var childName = groupParts[0];
            var newContainer = new ContainerInfo(group) { Name = childName };

            var childrenNames = parentContainer.GetRecursiveChildList().Select(x => x.Name).ToList();
            if (!childrenNames.Any())
            {
                groupCount = AddChild(parentContainer, newContainer, groupCount);
                _groups.Add(group);
                continue;
            }

            if (groupParts.Count > 1)
            {
                var childContainer = (ContainerInfo)parentContainer.Children.FirstOrDefault(x => x.Name == childName);
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

    private static int AddChild(ContainerInfo parentContainer, ContainerInfo newContainer, int groupCount)
    {
        parentContainer.AddChild(newContainer);
        groupCount--;
        return groupCount;
    }
}

internal sealed record RemoteDesktopManagerConnectionInfo(ProtocolType Protocol, string Name, int Port);