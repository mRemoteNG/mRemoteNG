using System.Security;

namespace mRemoteNG.Security.Authentication
{
    public interface IAuthenticator
    {
        bool Authenticate(SecureString password);
    }
}