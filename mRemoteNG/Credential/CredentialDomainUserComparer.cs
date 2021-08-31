using System.Collections;
using System.Collections.Generic;


namespace mRemoteNG.Credential
{
    public class CredentialDomainUserComparer : IComparer<ICredentialRecord>, IEqualityComparer<ICredentialRecord>
    {
        public int Compare(ICredentialRecord x, ICredentialRecord y)
        {
            var comparer = new CaseInsensitiveComparer();
            return comparer.Compare($"{x.Domain}\\{x.Username}", $"{y.Domain}\\{y.Username}");
        }

        public bool Equals(ICredentialRecord x, ICredentialRecord y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(ICredentialRecord obj)
        {
            return obj.Domain.GetHashCode() * 17 + obj.Username.GetHashCode();
        }
    }
}