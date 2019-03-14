using System;
using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Connections
{
    public class SaveConnectionsOnEdit
    {
        private readonly ConnectionsService _connectionsService;

        public SaveConnectionsOnEdit(ConnectionsService connectionsService)
        {
            if (connectionsService == null)
                throw new ArgumentNullException(nameof(connectionsService));

            _connectionsService = connectionsService;
            connectionsService.ConnectionsLoaded += ConnectionsServiceOnConnectionsLoaded;
        }

        private void ConnectionsServiceOnConnectionsLoaded(object sender, ConnectionsLoadedEventArgs connectionsLoadedEventArgs)
        {
            connectionsLoadedEventArgs.NewConnectionTreeModel.CollectionChanged += ConnectionTreeModelOnCollectionChanged;
            connectionsLoadedEventArgs.NewConnectionTreeModel.PropertyChanged += ConnectionTreeModelOnPropertyChanged;

            foreach (var oldTree in connectionsLoadedEventArgs.PreviousConnectionTreeModel)
            {
                oldTree.CollectionChanged -= ConnectionTreeModelOnCollectionChanged;
                oldTree.PropertyChanged -= ConnectionTreeModelOnPropertyChanged;
            }
        }

        private void ConnectionTreeModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveConnectionOnEdit(propertyChangedEventArgs.PropertyName);
        }

        private void ConnectionTreeModelOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            SaveConnectionOnEdit();
        }

        private void SaveConnectionOnEdit(string propertyName = "")
        {
            if (!mRemoteNG.Settings.Default.SaveConnectionsAfterEveryEdit)
                return;
            if (FrmMain.Default.IsClosing)
                return;

            _connectionsService.SaveConnectionsAsync(propertyName);
        }
    }
}
