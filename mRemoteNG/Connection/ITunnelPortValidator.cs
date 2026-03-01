using System.Threading.Tasks;

namespace mRemoteNG.Connection
{
    public interface ITunnelPortValidator
    {
        Task<bool> ValidatePortAsync(int port);
    }
}
