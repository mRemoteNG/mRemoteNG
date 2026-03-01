using System;

namespace mRemoteNG.Config.Serializers.Versioning
{
    public interface ISqlDatabaseVersionVerifier
    {
        bool VerifyDatabaseVersion(Version dbVersion);
    }
}
