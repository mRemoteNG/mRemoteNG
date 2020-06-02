using System;

namespace mRemoteNG.Tools.CustomCollections
{
    public interface INotifyCollectionUpdated<T>
    {
        event EventHandler<CollectionUpdatedEventArgs<T>> CollectionUpdated;
    }
}