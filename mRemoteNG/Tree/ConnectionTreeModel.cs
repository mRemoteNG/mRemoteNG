using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Properties;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public sealed class ConnectionTreeModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
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

        public void MoveRootNode(ContainerInfo rootNode, int targetIndex)
        {
            if (!RootNodes.Contains(rootNode))
                return;

            int clampedIndex = targetIndex;
            if (clampedIndex < 0)
            {
                clampedIndex = 0;
            }
            else if (clampedIndex > RootNodes.Count - 1)
            {
                clampedIndex = RootNodes.Count - 1;
            }
            int currentIndex = RootNodes.IndexOf(rootNode);

            if (currentIndex == clampedIndex)
                return;

            RootNodes.RemoveAt(currentIndex);
            RootNodes.Insert(clampedIndex, rootNode);
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void SetRootNodeOrder(IReadOnlyList<ContainerInfo> orderedRoots)
        {
            if (orderedRoots == null || orderedRoots.Count != RootNodes.Count)
                return;

            HashSet<ContainerInfo> orderedRootSet = new(orderedRoots);
            if (orderedRootSet.Count != RootNodes.Count || RootNodes.Exists(root => !orderedRootSet.Contains(root)))
                return;

            RootNodes.Clear();
            RootNodes.AddRange(orderedRoots);
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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

        public IEnumerable<ConnectionInfo> GetRecursiveChildList(ContainerInfo container)
        {
            return container.GetRecursiveChildList();
        }

        public IEnumerable<ConnectionInfo> GetRecursiveFavoriteChildList(ContainerInfo container)
        {
            return container.GetRecursiveFavoriteChildList();
        }

        public void RenameNode(ConnectionInfo connectionInfo, string newName)
        {
            if (newName == null || newName.Length <= 0)
                return;

            connectionInfo.Name = newName;
            if (Settings.Default.SetHostnameLikeDisplayName)
                connectionInfo.Hostname = newName;
        }

        public void DeleteNode(ConnectionInfo connectionInfo)
        {
            if (connectionInfo is RootNodeInfo)
                return;

            connectionInfo?.RemoveParent();
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaiseCollectionChangedEvent(object? sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }

        private void RaisePropertyChangedEvent(object? sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
        }
    }
}