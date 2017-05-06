namespace mRemoteNG.Security
{
    public interface ICryptoProviderFactory
    {
        ICryptographyProvider Build();
    }
}