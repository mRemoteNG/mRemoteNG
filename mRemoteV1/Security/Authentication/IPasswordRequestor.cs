using System.Security;

namespace mRemoteNG.Security.Authentication
{
    public interface IPasswordRequestor
    {
        SecureString RequestPassword();
    }
}