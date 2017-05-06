using System.Collections.Generic;
using System.ComponentModel;

namespace mRemoteNG.Tools.CustomCollections
{
    public interface IFullyNotifiableList<T> : IList<T>, INotifyCollectionUpdated<T>
        where T : INotifyPropertyChanged
    {
    }
}