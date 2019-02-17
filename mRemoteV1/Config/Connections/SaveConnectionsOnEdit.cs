using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Connections
{
    public class SaveConnectionsOnEdit
    {
        private IConnectionsService _connectionsService;

        public void Subscribe(IConnectionsService connectionsService)
        {
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
            connectionsService.ConnectionTreeModel.CollectionChanged += ConnectionTreeModelOnCollectionChanged;
            connectionsService.ConnectionTreeModel.PropertyChanged += ConnectionTreeModelOnPropertyChanged;
        }

        public void Unsubscribe()
        {
            _connectionsService.ConnectionTreeModel.CollectionChanged -= ConnectionTreeModelOnCollectionChanged;
            _connectionsService.ConnectionTreeModel.PropertyChanged -= ConnectionTreeModelOnPropertyChanged;
            _connectionsService = null;
        }

        private void ConnectionTreeModelOnPropertyChanged(object sender,
                                                          PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveConnectionOnEdit(propertyChangedEventArgs.PropertyName);
        }

        private void ConnectionTreeModelOnCollectionChanged(object sender,
                                                            NotifyCollectionChangedEventArgs
                                                                notifyCollectionChangedEventArgs)
        {
            SaveConnectionOnEdit();
        }

        private void SaveConnectionOnEdit(string propertyName = "")
        {
            if (!mRemoteNG.Settings.Default.SaveConnectionsAfterEveryEdit)
                return;

            _connectionsService?.SaveConnectionsAsync(propertyName);
        }
    }
}
