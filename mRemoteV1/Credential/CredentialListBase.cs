using System;
using System.Collections.Generic;
using System.Linq;


namespace mRemoteNG.Credential
{
    public class CredentialListBase : List<ICredentialRecord>, ICredentialList
    {
        public ICredentialProvider CredentialProvider { get; }

        public CredentialListBase(ICredentialProvider credentialProvider)
        {
            CredentialProvider = credentialProvider;
        }

        public ICredentialRecord GetCredential(Guid uniqueId)
        {
            return this.FirstOrDefault(cred => cred.Id == uniqueId);
        }

        public bool Contains(Guid uniqueId)
        {
            return Count != 0 && Exists(cred => cred.Id == uniqueId);
        }
    }
}