

using System.Collections.Generic;

namespace mRemoteNG.Credential
{
    public class CredentialRecordCatalog
    {
        public List<ICredentialRecord> CredentialRecords { get; } = new List<ICredentialRecord>();

        public void AddCredentialList(ICredentialList credentialList)
        {
            CredentialRecords.AddRange(credentialList);
        }

        public void RemoveCredentialList(ICredentialList credentialList)
        {
            foreach (var credentialToRemove in credentialList)
            {
                if (!CredentialRecords.Contains(credentialToRemove))
                    continue;
                CredentialRecords.Remove(credentialToRemove);
            }
        }
    }
}