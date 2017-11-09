using System.Security;

namespace mRemoteNG.Security
{
    public interface IKeyProvider
    {
        SecureString GetKey();
    }
}