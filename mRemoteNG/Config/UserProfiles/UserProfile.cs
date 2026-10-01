using System;

namespace mRemoteNG.Config.UserProfiles
{
    public sealed class UserProfile
    {
        public static readonly Guid BuiltInAdministratorId = new("00000000-0000-0000-0000-000000000001");

        public Guid Id { get; init; }
        public string UserName { get; init; }
        public bool IsAdministrator { get; init; }
        public bool HasPassword { get; init; }
    }
}
