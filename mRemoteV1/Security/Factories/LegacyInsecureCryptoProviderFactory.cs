using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Security.Factories
{
    public class LegacyInsecureCryptoProviderFactory : ICryptoProviderFactory
    {
        public ICryptographyProvider Build()
        {
            return new LegacyRijndaelCryptographyProvider();
        }
    }
}