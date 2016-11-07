using System;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public interface ICredentialList : IList<ICredentialRecord>
    {
        ICredentialProvider CredentialProvider { get; }

        ICredentialRecord GetCredential(Guid uniqueId);

        bool Contains(Guid uniqueId);
    }
}