using System.Net.Sockets;

namespace mRemoteNG.Network.Proxy
{
    internal interface IProxyClient
    {
        TcpClient Connect(string destinationHost, int destinationPort, int timeoutMs);
    }
}
