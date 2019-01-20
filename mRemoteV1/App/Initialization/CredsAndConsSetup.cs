using System.IO;
using System.Linq;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Tools;

namespace mRemoteNG.App.Initialization
{
	public class CredsAndConsSetup
    {
        public void LoadCredsAndCons(ConnectionsService connectionsService, CredentialService credentialService)
        {
            var saveOnEditService = new SaveConnectionsOnEdit(connectionsService);
            connectionsService.ConnectionTreeModel.CollectionChanged += saveOnEditService.ConnectionTreeModelOnCollectionChanged;
            connectionsService.ConnectionTreeModel.PropertyChanged += saveOnEditService.ConnectionTreeModelOnPropertyChanged;

            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.ConnectionsService.GetStartupConnectionFileName()))
                Runtime.ConnectionsService.NewConnectionsFile(Runtime.ConnectionsService.GetStartupConnectionFileName());

            credentialService.LoadRepositoryList();
            LoadDefaultConnectionCredentials(credentialService);
            Runtime.LoadConnections();
        }

        private void LoadDefaultConnectionCredentials(CredentialService credentialService)
        {
            var defaultCredId = Settings.Default.ConDefaultCredentialRecord;
            var matchedCredentials = credentialService
                .GetCredentialRecords()
                .Where(record => record.Id.Equals(defaultCredId))
                .ToArray();

            DefaultConnectionInfo.Instance.CredentialRecordId = matchedCredentials.FirstOrDefault()?.Id.ToOptional();
        }
    }
}