using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Credential;

namespace mRemoteNG.Config
{
    public class ConnectionToCredentialMap : Dictionary<Guid, ICredentialRecord>
    {
        private readonly IEqualityComparer<ICredentialRecord> _credentialComparer = new CredentialDomainUserPasswordComparer();

        public IEnumerable<ICredentialRecord> DistinctCredentialRecords => Values.Distinct(_credentialComparer);
    }
}
