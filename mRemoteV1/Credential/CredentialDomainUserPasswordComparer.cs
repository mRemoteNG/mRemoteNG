using System.Collections.Generic;
using mRemoteNG.Security;

namespace mRemoteNG.Credential
{
    public class CredentialDomainUserPasswordComparer : IEqualityComparer<ICredentialRecord>
    {
        public bool Equals(ICredentialRecord x, ICredentialRecord y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(ICredentialRecord obj)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + obj.Username.GetHashCode();
                hash = hash * 23 + obj.Domain.GetHashCode();
                hash = hash * 23 + obj.Password.ConvertToUnsecureString().GetHashCode();

                return hash;
            }
        }
    }
}