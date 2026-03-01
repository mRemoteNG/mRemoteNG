using System.Collections.Generic;
using System.Collections.Specialized;
using mRemoteNG.Connection;

namespace mRemoteNG.Container
{
    /// <summary>
    /// Represents a node in the connection tree that can contain child connection nodes.
    /// Abstracting the container behavior away from the concrete <see cref="ContainerInfo"/>
    /// type allows <see cref="mRemoteNG.Tree.Root.RootNodeInfo"/> to be decoupled from the
    /// <see cref="ConnectionInfo"/> object graph in a future refactoring step.
    /// </summary>
    /// <remarks>
    /// See GitHub issue #242 for the full architectural proposal.
    /// </remarks>
    public interface ITreeNodeContainer
    {
        bool HasChildren();
        void AddChild(ConnectionInfo newChildItem);
        void AddChildAt(ConnectionInfo newChildItem, int index);
        void AddChildAbove(ConnectionInfo newChildItem, ConnectionInfo reference);
        void AddChildBelow(ConnectionInfo newChildItem, ConnectionInfo reference);
        void AddChildRange(IEnumerable<ConnectionInfo> newChildren);
        void RemoveChild(ConnectionInfo removalTarget);
        void RemoveChildRange(IEnumerable<ConnectionInfo> removalTargets);
        IEnumerable<ConnectionInfo> GetRecursiveChildList();
        event NotifyCollectionChangedEventHandler? CollectionChanged;
    }
}
