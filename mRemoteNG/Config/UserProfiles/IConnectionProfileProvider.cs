using System;
using System.Collections.Generic;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IConnectionProfileProvider
    {
        IReadOnlyList<ConnectionProfile> GetProfiles(UserProfileSession user);
        ConnectionProfile AddProfile(UserProfileSession user, string name, string databasePath, bool isShared = false);
        void RenameProfile(UserProfileSession user, Guid profileId, string name);
        void DeleteProfile(UserProfileSession user, Guid profileId);
        void SetShared(UserProfileSession user, Guid profileId, bool isShared);
        void SetAccess(UserProfileSession administrator, Guid profileId, Guid userId, ProfileAccessLevel accessLevel);
        void SetLoadOnStartup(UserProfileSession user, Guid profileId, bool loadOnStartup);
    }
}
