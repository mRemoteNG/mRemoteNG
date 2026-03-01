using mRemoteNG.Connection.Protocol.VNC;

namespace mRemoteNG.Network.Proxy
{
    internal static class ProxyClientFactory
    {
        public static IProxyClient? Create(ProtocolVNC.ProxyType proxyType,
                                           string proxyHost,
                                           int proxyPort,
                                           string username,
                                           string password)
        {
            return proxyType switch
            {
                ProtocolVNC.ProxyType.ProxyHTTP => new HttpProxyClient(proxyHost, proxyPort, username, password),
                ProtocolVNC.ProxyType.ProxySocks4 => new Socks4ProxyClient(proxyHost, proxyPort, username),
                ProtocolVNC.ProxyType.ProxySocks5 => new Socks5ProxyClient(proxyHost, proxyPort, username, password),
                _ => null
            };
        }
    }
}
