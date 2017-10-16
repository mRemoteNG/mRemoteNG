using System;
using System.IO;
using mRemoteNG.Credential;

namespace mRemoteNG.App.Initialization
{
	public class CredsAndConsSetup
    {
        private readonly CredentialServiceFacade _credentialsService;

        public CredsAndConsSetup(CredentialServiceFacade credentialsService)
        {
            if (credentialsService == null)
                throw new ArgumentNullException(nameof(credentialsService));

            _credentialsService = credentialsService;
        }

        public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.ConnectionsService.GetStartupConnectionFileName()))
                Runtime.ConnectionsService.NewConnections(Runtime.ConnectionsService.GetStartupConnectionFileName());

            LoadCredentialRepositoryList();
            Runtime.LoadConnections();
        }

        private void LoadCredentialRepositoryList()
        {
            _credentialsService.LoadRepositoryList();
        }
    }
}