using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace mRemoteNG.Network.Proxy
{
    internal sealed class Socks5ProxyClient : IProxyClient
    {
        private readonly string _proxyHost;
        private readonly int _proxyPort;
        private readonly string _username;
        private readonly string _password;

        public Socks5ProxyClient(string proxyHost, int proxyPort, string username, string password)
        {
            _proxyHost = proxyHost;
            _proxyPort = proxyPort;
            _username = username ?? string.Empty;
            _password = password ?? string.Empty;
        }

        public TcpClient Connect(string destinationHost, int destinationPort, int timeoutMs)
        {
            TcpClient tcpClient = ProxyClientUtilities.CreateConnectedTcpClient(_proxyHost, _proxyPort, timeoutMs);
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = timeoutMs;
                stream.WriteTimeout = timeoutMs;

                SendGreeting(stream);
                HandleAuthentication(stream);
                SendConnectRequest(stream, destinationHost, destinationPort);
                ValidateConnectResponse(stream);

                return tcpClient;
            }
            catch
            {
                tcpClient.Dispose();
                throw;
            }
        }

        private void SendGreeting(NetworkStream stream)
        {
            // Only offer username/password auth (0x02) when credentials are configured.
            // Offering both 0x00 and 0x02 would let a compromised proxy downgrade to no-auth.
            byte[] methods = string.IsNullOrEmpty(_username)
                ? new byte[] { 0x00 }
                : new byte[] { 0x02 };

            byte[] greeting = new byte[2 + methods.Length];
            greeting[0] = 0x05;
            greeting[1] = (byte)methods.Length;
            Buffer.BlockCopy(methods, 0, greeting, 2, methods.Length);

            stream.Write(greeting, 0, greeting.Length);
        }

        private void HandleAuthentication(NetworkStream stream)
        {
            byte[] methodSelection = new byte[2];
            ProxyClientUtilities.ReadExact(stream, methodSelection, 0, methodSelection.Length);

            if (methodSelection[0] != 0x05)
                throw new IOException("SOCKS5 proxy returned an invalid protocol version.");

            switch (methodSelection[1])
            {
                case 0x00:
                    if (!string.IsNullOrEmpty(_username))
                        throw new IOException(
                            "SOCKS5 proxy selected no-authentication but credentials were provided. " +
                            "This may indicate a downgrade attack.");
                    return;
                case 0x02:
                    AuthenticateWithUsernamePassword(stream);
                    return;
                case 0xFF:
                    throw new IOException("SOCKS5 proxy did not accept any authentication method.");
                default:
                    throw new IOException($"SOCKS5 proxy selected unsupported auth method 0x{methodSelection[1]:X2}.");
            }
        }

        private void AuthenticateWithUsernamePassword(NetworkStream stream)
        {
            byte[] usernameBytes = Encoding.ASCII.GetBytes(_username);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(_password);

            if (usernameBytes.Length > byte.MaxValue)
                throw new InvalidOperationException("SOCKS5 username exceeds 255 bytes.");

            if (passwordBytes.Length > byte.MaxValue)
                throw new InvalidOperationException("SOCKS5 password exceeds 255 bytes.");

            byte[] authRequest = new byte[3 + usernameBytes.Length + passwordBytes.Length];
            authRequest[0] = 0x01;
            authRequest[1] = (byte)usernameBytes.Length;
            Buffer.BlockCopy(usernameBytes, 0, authRequest, 2, usernameBytes.Length);
            authRequest[2 + usernameBytes.Length] = (byte)passwordBytes.Length;
            Buffer.BlockCopy(passwordBytes, 0, authRequest, 3 + usernameBytes.Length, passwordBytes.Length);

            stream.Write(authRequest, 0, authRequest.Length);

            byte[] authResponse = new byte[2];
            ProxyClientUtilities.ReadExact(stream, authResponse, 0, authResponse.Length);

            if (authResponse[1] != 0x00)
                throw new IOException("SOCKS5 proxy username/password authentication failed.");
        }

        private static void SendConnectRequest(NetworkStream stream, string destinationHost, int destinationPort)
        {
            byte addressType;
            byte[] addressBytes;

            if (IPAddress.TryParse(destinationHost, out IPAddress? parsedAddress))
            {
                switch (parsedAddress.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        addressType = 0x01;
                        addressBytes = parsedAddress.GetAddressBytes();
                        break;
                    case AddressFamily.InterNetworkV6:
                        addressType = 0x04;
                        addressBytes = parsedAddress.GetAddressBytes();
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported destination IP address family for SOCKS5.");
                }
            }
            else
            {
                byte[] hostBytes = Encoding.ASCII.GetBytes(destinationHost);
                if (hostBytes.Length > byte.MaxValue)
                    throw new InvalidOperationException("SOCKS5 destination host exceeds 255 bytes.");

                addressType = 0x03;
                addressBytes = new byte[1 + hostBytes.Length];
                addressBytes[0] = (byte)hostBytes.Length;
                Buffer.BlockCopy(hostBytes, 0, addressBytes, 1, hostBytes.Length);
            }

            byte[] request = new byte[4 + addressBytes.Length + 2];
            request[0] = 0x05;
            request[1] = 0x01; // CONNECT
            request[2] = 0x00; // reserved
            request[3] = addressType;
            Buffer.BlockCopy(addressBytes, 0, request, 4, addressBytes.Length);
            request[4 + addressBytes.Length] = (byte)((destinationPort >> 8) & 0xFF);
            request[5 + addressBytes.Length] = (byte)(destinationPort & 0xFF);

            stream.Write(request, 0, request.Length);
        }

        private static void ValidateConnectResponse(NetworkStream stream)
        {
            byte[] responseHeader = new byte[4];
            ProxyClientUtilities.ReadExact(stream, responseHeader, 0, responseHeader.Length);

            if (responseHeader[0] != 0x05)
                throw new IOException("SOCKS5 proxy returned an invalid protocol version for CONNECT response.");

            if (responseHeader[1] != 0x00)
                throw new IOException($"SOCKS5 proxy CONNECT failed with status 0x{responseHeader[1]:X2}.");

            int addressLength;
            switch (responseHeader[3])
            {
                case 0x01:
                    addressLength = 4;
                    break;
                case 0x04:
                    addressLength = 16;
                    break;
                case 0x03:
                    int domainLength = stream.ReadByte();
                    if (domainLength < 0)
                        throw new IOException("SOCKS5 proxy response ended before domain length could be read.");

                    addressLength = domainLength;
                    break;
                default:
                    throw new IOException($"SOCKS5 proxy returned unsupported address type 0x{responseHeader[3]:X2}.");
            }

            byte[] addressAndPort = new byte[addressLength + 2];
            ProxyClientUtilities.ReadExact(stream, addressAndPort, 0, addressAndPort.Length);
        }
    }
}
