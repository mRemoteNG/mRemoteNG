using System;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public interface ICredentialList : IList<ICredentialRecord>
    {
        ICredentialRecord GetCredential(Guid uniqueId);

        bool Contains(Guid uniqueId);
    }
}