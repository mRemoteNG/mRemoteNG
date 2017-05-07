using System;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Credential;

namespace mRemoteNG.App.Initialization
{
    public class CredsAndConsSetup
    {
        private readonly CredentialServiceFacade _credentialsService;
        private readonly XmlCredentialManagerUpgrader _credentialManagerUpgrader;

        public CredsAndConsSetup(CredentialServiceFacade credentialsService, XmlCredentialManagerUpgrader credentialManagerUpgrader)
        {
            if (credentialsService == null)
                throw new ArgumentNullException(nameof(credentialsService));
            if (credentialManagerUpgrader == null)
                throw new ArgumentNullException(nameof(credentialManagerUpgrader));

            _credentialsService = credentialsService;
            _credentialManagerUpgrader = credentialManagerUpgrader;
        }

        public void LoadCredsAndCons()
        {
            if (Settings.Default.FirstStart && !Settings.Default.LoadConsFromCustomLocation && !File.Exists(Runtime.GetStartupConnectionFileName()))
                Runtime.NewConnections(Runtime.GetStartupConnectionFileName());

            var upgradeMap = _credentialManagerUpgrader.UpgradeUserFilesForCredentialManager();

            LoadCredentialRepositoryList();
            LoadDefaultConnectionCredentials();
            Runtime.LoadConnections();

            if (upgradeMap != null)
                _credentialManagerUpgrader.ApplyCredentialMapping(upgradeMap, Runtime.ConnectionTreeModel.GetRecursiveChildList());
        }

        private void LoadCredentialRepositoryList()
        {
            _credentialsService.LoadRepositoryList();
        }

        private void LoadDefaultConnectionCredentials()
        {
            var defaultCredId = Settings.Default.ConDefaultCredentialRecord;
            var matchedCredentials = _credentialsService.GetCredentialRecords().Where(record => record.Id.Equals(defaultCredId)).ToArray();
            DefaultConnectionInfo.Instance.CredentialRecord = matchedCredentials.Any() ? matchedCredentials.First() : null;
        }
    }
}