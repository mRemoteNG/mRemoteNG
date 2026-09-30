using System;

namespace mRemoteNG.Config.UserProfiles
{
    public sealed class ConnectionProfile
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string DatabasePath { get; init; }
        public bool IsShared { get; init; }
        public ProfileAccessLevel AccessLevel { get; init; }
        public bool LoadOnStartup { get; init; }
    }
}
