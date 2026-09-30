using System;
using System.Security;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IPasswordProvider
    {
        void SetPassword(Guid userId, SecureString password);
        bool VerifyPassword(Guid userId, SecureString password);
    }
}
