using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace mRemoteNG.Network.Proxy
{
    internal sealed class Socks4ProxyClient : IProxyClient
    {
        private readonly string _proxyHost;
        private readonly int _proxyPort;
        private readonly string _userId;

        public Socks4ProxyClient(string proxyHost, int proxyPort, string userId)
        {
            _proxyHost = proxyHost;
            _proxyPort = proxyPort;
            _userId = userId ?? string.Empty;
        }

        public TcpClient Connect(string destinationHost, int destinationPort, int timeoutMs)
        {
            TcpClient tcpClient = ProxyClientUtilities.CreateConnectedTcpClient(_proxyHost, _proxyPort, timeoutMs);
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = timeoutMs;
                stream.WriteTimeout = timeoutMs;

                bool destinationIsIpv4 =
                    IPAddress.TryParse(destinationHost, out IPAddress? parsedAddress) &&
                    parsedAddress.AddressFamily == AddressFamily.InterNetwork;

                using MemoryStream request = new();
                request.WriteByte(0x04); // SOCKS version
                request.WriteByte(0x01); // CONNECT
                request.WriteByte((byte)((destinationPort >> 8) & 0xFF));
                request.WriteByte((byte)(destinationPort & 0xFF));

                if (destinationIsIpv4)
                {
                    byte[] ipBytes = parsedAddress!.GetAddressBytes();
                    request.Write(ipBytes, 0, ipBytes.Length);
                }
                else
                {
                    // SOCKS4a: 0.0.0.1 + hostname after userid
                    request.Write(new byte[] { 0x00, 0x00, 0x00, 0x01 }, 0, 4);
                }

                byte[] userIdBytes = Encoding.ASCII.GetBytes(_userId);
                request.Write(userIdBytes, 0, userIdBytes.Length);
                request.WriteByte(0x00);

                if (!destinationIsIpv4)
                {
                    byte[] hostBytes = Encoding.ASCII.GetBytes(destinationHost);
                    request.Write(hostBytes, 0, hostBytes.Length);
                    request.WriteByte(0x00);
                }

                byte[] requestBytes = request.ToArray();
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] response = new byte[8];
                ProxyClientUtilities.ReadExact(stream, response, 0, response.Length);

                if (response[1] != 0x5A)
                    throw new IOException($"SOCKS4 proxy CONNECT failed with status 0x{response[1]:X2}.");

                return tcpClient;
            }
            catch
            {
                tcpClient.Dispose();
                throw;
            }
        }
    }
}
