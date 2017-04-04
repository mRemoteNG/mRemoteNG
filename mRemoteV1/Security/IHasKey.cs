using System.Security;

namespace mRemoteNG.Security
{
    public interface IHasKey
    {
        SecureString Key { get; set; }
    }
}