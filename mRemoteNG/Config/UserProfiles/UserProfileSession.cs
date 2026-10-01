using System;

namespace mRemoteNG.Config.UserProfiles
{
    public sealed class UserProfileSession
    {
        internal UserProfileSession(Guid storeId, UserProfile user)
        {
            StoreId = storeId;
            User = user;
        }

        internal Guid StoreId { get; }
        public UserProfile User { get; }
    }
}
