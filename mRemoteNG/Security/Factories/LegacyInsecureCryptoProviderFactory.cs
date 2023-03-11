using mRemoteNG.Security.SymmetricEncryption;
using System.Runtime.Versioning;

namespace mRemoteNG.Security.Factories
{
    [SupportedOSPlatform("windows")]
    public class LegacyInsecureCryptoProviderFactory : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            return new LegacyRijndaelCryptographyProvider();
        }
    }
}