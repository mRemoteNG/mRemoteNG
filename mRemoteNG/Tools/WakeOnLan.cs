using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class WakeOnLan
    {
        private const int MacAddressByteCount = 6;
        private const int MagicPacketRepeats = 16;
        private const int MagicPacketPrefixByteCount = 6;
        private const int WakeOnLanPort = 9;

        public static bool TrySendMagicPacket(string? macAddress)
        {
            if (!TryParseMacAddress(macAddress, out byte[] macAddressBytes))
                return false;

            byte[] magicPacket = BuildMagicPacket(macAddressBytes);

            try
            {
                using UdpClient udpClient = new();
                udpClient.EnableBroadcast = true;
                udpClient.Send(magicPacket, magicPacket.Length, new IPEndPoint(IPAddress.Broadcast, WakeOnLanPort));
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        public static bool IsValidMacAddress(string? macAddress)
        {
            return TryParseMacAddress(macAddress, out _);
        }

        private static bool TryParseMacAddress(string? macAddress, out byte[] macAddressBytes)
        {
            macAddressBytes = [];

            if (string.IsNullOrWhiteSpace(macAddress))
                return false;

            string normalizedMacAddress = macAddress
                .Replace(":", string.Empty, StringComparison.Ordinal)
                .Replace("-", string.Empty, StringComparison.Ordinal)
                .Replace(".", string.Empty, StringComparison.Ordinal)
                .Replace(" ", string.Empty, StringComparison.Ordinal);

            if (normalizedMacAddress.Length != MacAddressByteCount * 2 || !normalizedMacAddress.All(Uri.IsHexDigit))
                return false;

            byte[] parsedMacAddress = new byte[MacAddressByteCount];
            for (int i = 0; i < parsedMacAddress.Length; i++)
            {
                string macAddressByteText = normalizedMacAddress.Substring(i * 2, 2);
                if (!byte.TryParse(macAddressByteText, NumberStyles.HexNumber, CultureInfo.InvariantCulture,
                                   out parsedMacAddress[i]))
                    return false;
            }

            macAddressBytes = parsedMacAddress;
            return true;
        }

        private static byte[] BuildMagicPacket(byte[] macAddressBytes)
        {
            byte[] magicPacket = new byte[MagicPacketPrefixByteCount + (MacAddressByteCount * MagicPacketRepeats)];

            for (int i = 0; i < MagicPacketPrefixByteCount; i++)
            {
                magicPacket[i] = 0xFF;
            }

            for (int i = MagicPacketPrefixByteCount; i < magicPacket.Length; i += MacAddressByteCount)
            {
                Buffer.BlockCopy(macAddressBytes, 0, magicPacket, i, MacAddressByteCount);
            }

            return magicPacket;
        }
    }
}
