using System;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public interface ICredentialList : IList<ICredential>
    {
        ICredential GetCredential(Guid uniqueId);

        bool Contains(Guid uniqueId);
    }
}