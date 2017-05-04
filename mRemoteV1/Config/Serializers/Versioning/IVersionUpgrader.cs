using System;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public interface IVersionUpgrader
    {
        bool CanUpgrade(Version currentVersion);
        void Upgrade();
    }
}