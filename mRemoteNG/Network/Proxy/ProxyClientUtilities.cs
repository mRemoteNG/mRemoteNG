using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace mRemoteNG.Network.Proxy
{
    internal static class ProxyClientUtilities
    {
        public static TcpClient CreateConnectedTcpClient(string host, int port, int timeoutMs)
        {
            TcpClient tcpClient = new();
            try
            {
                if (!tcpClient.ConnectAsync(host, port).Wait(timeoutMs))
                    throw new TimeoutException($"Timed out connecting to proxy {host}:{port}.");

                tcpClient.NoDelay = true;
                tcpClient.SendTimeout = timeoutMs;
                tcpClient.ReceiveTimeout = timeoutMs;
                return tcpClient;
            }
            catch
            {
                tcpClient.Dispose();
                throw;
            }
        }

        public static void ReadExact(NetworkStream stream, byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = stream.Read(buffer, offset + totalRead, count - totalRead);
                if (read <= 0)
                    throw new IOException("Unexpected end of stream while reading proxy response.");

                totalRead += read;
            }
        }

        public static byte[] ReadUntil(NetworkStream stream, byte[] terminator, int maxBytes)
        {
            List<byte> bytes = new();

            while (bytes.Count < maxBytes)
            {
                int value = stream.ReadByte();
                if (value < 0)
                    throw new IOException("Unexpected end of stream while reading proxy response.");

                bytes.Add((byte)value);
                if (EndsWith(bytes, terminator))
                    return bytes.ToArray();
            }

            throw new IOException("Proxy response exceeded maximum expected size.");
        }

        private static bool EndsWith(IReadOnlyList<byte> bytes, IReadOnlyList<byte> suffix)
        {
            if (bytes.Count < suffix.Count) return false;

            int start = bytes.Count - suffix.Count;
            for (int i = 0; i < suffix.Count; i++)
            {
                if (bytes[start + i] != suffix[i])
                    return false;
            }

            return true;
        }
    }
}
