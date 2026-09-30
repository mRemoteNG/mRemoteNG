using System;
using System.Security;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IPasswordProvider
    {
        void SetPassword(Guid actingUserId, Guid userId, SecureString currentPassword, SecureString newPassword);
        bool VerifyPassword(Guid userId, SecureString password);
    }
}
