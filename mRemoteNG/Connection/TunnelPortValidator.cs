using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;

namespace mRemoteNG.Connection
{
    public class TunnelPortValidator : ITunnelPortValidator
    {
        public async Task<bool> ValidatePortAsync(int port)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < 5000)
            {
                try
                {
                    using (TcpClient client = new())
                    {
                        await client.ConnectAsync(System.Net.IPAddress.Loopback, port);
                    }
                    return true;
                }
                catch
                {
                    await Task.Delay(500);
                }
            }
            return false;
        }
    }
}
