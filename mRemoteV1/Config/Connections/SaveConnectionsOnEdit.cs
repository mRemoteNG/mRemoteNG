using System.Collections.Specialized;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Connections
{
    public class SaveConnectionsOnEdit
    {
        private readonly ConnectionsService _connectionsService;

        public SaveConnectionsOnEdit(ConnectionsService connectionsService)
        {
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
        }

        public void ConnectionTreeModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SaveConnectionOnEdit(propertyChangedEventArgs.PropertyName);
        }

        public void ConnectionTreeModelOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
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
