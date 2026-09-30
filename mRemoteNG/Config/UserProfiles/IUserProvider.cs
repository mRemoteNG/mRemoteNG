using System;
using System.Collections.Generic;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IUserProvider
    {
        IReadOnlyList<UserProfile> GetUsers();
        UserProfile GetUser(Guid userId);
        UserProfile GetUser(string userName);
        UserProfile AddUser(Guid administratorId, string userName);
        void RemoveUser(Guid administratorId, Guid userId);
    }
}
