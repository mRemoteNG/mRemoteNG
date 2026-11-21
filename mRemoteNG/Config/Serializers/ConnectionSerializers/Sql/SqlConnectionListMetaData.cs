using System;
using System.Runtime.Versioning;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    public class SqlConnectionListMetaData
    {
        public string Name { get; set; }
        public string Protected { get; set; }
        public bool Export { get; set; }
        public Version ConfVersion { get; set; }

        /// <summary>
        /// The salt the key of the stored passwords is derived with. Null in a database before
        /// version 3.4, where the key is the MD5 of the password.
        /// </summary>
        public byte[] KdfSalt { get; set; }

        public int KdfIterations { get; set; }

        /// <summary>The provider that reads the passwords stored in this database.</summary>
        [SupportedOSPlatform("windows")]
        public ICryptographyProvider CryptographyProvider() =>
            KdfSalt != null
                ? new DerivedKeyCryptographyProvider(KdfSalt, KdfIterations)
                : new LegacyRijndaelCryptographyProvider();
    }
}
