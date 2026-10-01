using System;
using System.Collections.Generic;
using System.Security;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IUserProvider
    {
        IReadOnlyList<UserProfile> GetUsers();
        UserProfile GetUser(Guid userId);
        UserProfile GetUser(string userName);
        UserProfileSession Authenticate(string userName, SecureString password);
        UserProfile AddUser(UserProfileSession administrator, string userName);
        void RemoveUser(UserProfileSession administrator, Guid userId);
    }
}
