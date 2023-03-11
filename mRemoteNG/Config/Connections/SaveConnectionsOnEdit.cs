using System;
using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.UI.Forms;
using mRemoteNG.Properties;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
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
            connectionsLoadedEventArgs.NewConnectionTreeModel.CollectionChanged +=
                ConnectionTreeModelOnCollectionChanged;
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
            //OBSOLETE: mRemoteNG.Settings.Default.SaveConnectionsAfterEveryEdit is obsolete and should be removed in a future release
            if (Properties.OptionsBackupPage.Default.SaveConnectionsAfterEveryEdit || (Properties.OptionsBackupPage.Default.SaveConnectionsFrequency == (int)ConnectionsBackupFrequencyEnum.OnEdit))
            {
                if (FrmMain.Default.IsClosing)
                    return;

                _connectionsService.SaveConnectionsAsync(propertyName);
            }
        }
    }
}