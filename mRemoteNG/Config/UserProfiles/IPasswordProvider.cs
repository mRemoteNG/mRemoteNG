using System;
using System.Security;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IPasswordProvider
    {
        void SetPassword(UserProfileSession actingUser, Guid userId, SecureString currentPassword, SecureString newPassword);
        bool VerifyPassword(Guid userId, SecureString password);
    }
}
