using System.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Security
{
    public interface IKeyProvider
    {
        Optional<SecureString> GetKey();
    }
}