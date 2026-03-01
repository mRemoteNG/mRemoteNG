using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace mRemoteNG.Network.Proxy
{
    internal sealed class HttpProxyClient : IProxyClient
    {
        private readonly string _proxyHost;
        private readonly int _proxyPort;
        private readonly string _username;
        private readonly string _password;

        public HttpProxyClient(string proxyHost, int proxyPort, string username, string password)
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

                string destination = $"{destinationHost}:{destinationPort}";
                StringBuilder requestBuilder = new();
                requestBuilder.Append("CONNECT ").Append(destination).Append(" HTTP/1.1\r\n");
                requestBuilder.Append("Host: ").Append(destination).Append("\r\n");
                requestBuilder.Append("Proxy-Connection: Keep-Alive\r\n");

                if (!string.IsNullOrEmpty(_username))
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{_password}"));
                    requestBuilder.Append("Proxy-Authorization: Basic ").Append(credentials).Append("\r\n");
                }

                requestBuilder.Append("\r\n");

                byte[] requestBytes = Encoding.ASCII.GetBytes(requestBuilder.ToString());
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] responseBytes = ProxyClientUtilities.ReadUntil(stream, Encoding.ASCII.GetBytes("\r\n\r\n"), 32768);
                string responseText = Encoding.ASCII.GetString(responseBytes);
                string statusLine = responseText.Split(new[] { "\r\n" }, StringSplitOptions.None)[0];

                string[] statusParts = statusLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (statusParts.Length < 2 || !string.Equals(statusParts[1], "200", StringComparison.Ordinal))
                    throw new IOException($"HTTP proxy CONNECT failed: {statusLine}");

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
