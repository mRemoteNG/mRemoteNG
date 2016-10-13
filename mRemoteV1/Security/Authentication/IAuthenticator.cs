using System.Security;

namespace mRemoteNG.Security
{
    public interface IAuthenticator
    {
        bool Authenticate(SecureString password);
    }
}