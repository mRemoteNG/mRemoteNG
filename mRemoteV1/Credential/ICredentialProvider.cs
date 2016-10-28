
namespace mRemoteNG.Credential
{
    public interface ICredentialProvider
    {
        ICredentialRepository LoadCredentials();
    }
}