using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.Container;

namespace mRemoteNG.Tree
{
    public interface IConnectionTreeModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
        List<ContainerInfo> RootNodes { get; }
        void AddRootNode(ContainerInfo rootNode);
        void RemoveRootNode(ContainerInfo rootNode);
        IReadOnlyList<ConnectionInfo> GetRecursiveChildList();
        IEnumerable<ConnectionInfo> GetRecursiveChildList(ContainerInfo container);
        void RenameNode(ConnectionInfo connectionInfo, string newName);
        void DeleteNode(ConnectionInfo connectionInfo);
        event NotifyCollectionChangedEventHandler CollectionChanged;
        event PropertyChangedEventHandler PropertyChanged;
    }
}