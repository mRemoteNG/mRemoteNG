using System;
using System.Collections.Generic;

namespace mRemoteNG.Config.UserProfiles
{
    public interface IConnectionProfileProvider
    {
        IReadOnlyList<ConnectionProfile> GetProfiles(Guid userId);
        ConnectionProfile AddProfile(Guid userId, string name, string databasePath, bool isShared = false);
        void RenameProfile(Guid userId, Guid profileId, string name);
        void DeleteProfile(Guid userId, Guid profileId);
        void SetAccess(Guid administratorId, Guid profileId, Guid userId, ProfileAccessLevel accessLevel);
        void SetLoadOnStartup(Guid userId, Guid profileId, bool loadOnStartup);
    }
}
