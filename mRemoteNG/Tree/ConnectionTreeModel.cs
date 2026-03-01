using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Tree
{
    /// <summary>
    /// The in-memory model backing the connection tree UI.
    /// Holds one or more <see cref="RootNodeInfo"/> root nodes, each containing
    /// a hierarchy of <see cref="ContainerInfo"/> folders and <see cref="ConnectionInfo"/> connections.
    /// Raises <see cref="INotifyCollectionChanged.CollectionChanged"/> and
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> events to keep the
    /// tree view synchronized with the data model.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class ConnectionTreeModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Tracks connection ConstantIDs present when this model was loaded from a data
        /// source (e.g. SQL database). Used during save to distinguish between connections
        /// the user explicitly deleted (was loaded, now absent) vs. connections added by
        /// other users after our last load (never loaded, should not be deleted). (#1424)
        /// </summary>
        private readonly HashSet<string> _loadedConnectionIds = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The set of connection IDs that were present at load time.
        /// </summary>
        public IReadOnlyCollection<string> LoadedConnectionIds => _loadedConnectionIds;

        /// <summary>
        /// Records a connection ID as having been loaded from the data source.
        /// </summary>
        public void TrackLoadedConnectionId(string constantId)
        {
            if (!string.IsNullOrEmpty(constantId))
                _loadedConnectionIds.Add(constantId);
        }

        public List<ContainerInfo> RootNodes { get; } = [];

        public void AddRootNode(ContainerInfo rootNode)
        {
            if (RootNodes.Contains(rootNode)) return;
            RootNodes.Add(rootNode);
            rootNode.CollectionChanged += RaiseCollectionChangedEvent!;
            rootNode.PropertyChanged += RaisePropertyChangedEvent!;
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, rootNode));
        }

        public void RemoveRootNode(ContainerInfo rootNode)
        {
            if (!RootNodes.Contains(rootNode)) return;
            rootNode.CollectionChanged -= RaiseCollectionChangedEvent!;
            rootNode.PropertyChanged -= RaisePropertyChangedEvent!;
            RootNodes.Remove(rootNode);
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, rootNode));
        }

        public IReadOnlyList<ConnectionInfo> GetRecursiveChildList()
        {
            List<ConnectionInfo> list = new();
            foreach (ContainerInfo rootNode in RootNodes)
            {
                list.AddRange(GetRecursiveChildList(rootNode));
            }

            return list;
        }

        public static IEnumerable<ConnectionInfo> GetRecursiveChildList(ContainerInfo container)
        {
            return container.GetRecursiveChildList();
        }

        public static IEnumerable<ConnectionInfo> GetRecursiveFavoriteChildList(ContainerInfo container)
        {
            return container.GetRecursiveFavoriteChildList();
        }

        public static void RenameNode(ConnectionInfo connectionInfo, string newName)
        {
            if (newName == null || newName.Length <= 0)
                return;

            connectionInfo.Name = newName;
            if (Settings.Default.SetHostnameLikeDisplayName)
                connectionInfo.Hostname = newName;
        }

        public static void DeleteNode(ConnectionInfo connectionInfo)
        {
            if (connectionInfo is RootNodeInfo)
                return;

            connectionInfo?.RemoveParent();
        }

        public ConnectionInfo? FindConnectionById(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                return null;

            foreach (ContainerInfo rootNode in RootNodes)
            {
                if (string.Equals(rootNode.ConstantID, connectionId, StringComparison.OrdinalIgnoreCase))
                    return rootNode;

                ConnectionInfo? childNode = rootNode.GetRecursiveChildList()
                    .FirstOrDefault(node => string.Equals(node.ConstantID, connectionId, StringComparison.OrdinalIgnoreCase));
                if (childNode != null)
                    return childNode;
            }

            return null;
        }

        public ConnectionInfo? ResolveLinkedConnection(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
                return null;

            if (string.IsNullOrWhiteSpace(connectionInfo.LinkedConnectionId))
                return connectionInfo;

            HashSet<string> visitedConnectionIds = new(StringComparer.OrdinalIgnoreCase)
            {
                connectionInfo.ConstantID
            };

            string currentLinkedId = connectionInfo.LinkedConnectionId;
            while (!string.IsNullOrWhiteSpace(currentLinkedId))
            {
                if (!visitedConnectionIds.Add(currentLinkedId))
                    return null;

                ConnectionInfo? candidate = FindConnectionById(currentLinkedId);
                if (candidate == null)
                    return null;

                if (string.IsNullOrWhiteSpace(candidate.LinkedConnectionId))
                    return candidate;

                currentLinkedId = candidate.LinkedConnectionId;
            }

            return null;
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaiseCollectionChangedEvent(object? sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        private void RaisePropertyChangedEvent(object? sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}