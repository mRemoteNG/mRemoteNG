using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Tools
{
    /// <summary>
    /// Builds and broadcasts Wake-on-LAN "magic packets" so that sleeping or
    /// powered-off workstations (that have WoL enabled in their firmware/NIC)
    /// can be woken before connecting to them.
    /// <para>
    /// A magic packet consists of 6 bytes of 0xFF followed by the target's
    /// 6-byte MAC address repeated 16 times (102 bytes total). It is sent as a
    /// UDP broadcast, typically to port 9 (the discard port).
    /// </para>
    /// </summary>
    public static class WakeOnLan
    {
        /// <summary>
        /// Default UDP port used for Wake-on-LAN magic packets.
        /// </summary>
        public const int DefaultPort = 9;

        /// <summary>
        /// Parses a MAC address string into its 6 bytes. Accepts the common
        /// separators ':' and '-' as well as unseparated 12-character strings.
        /// </summary>
        /// <param name="macAddress">The MAC address, e.g. "001122AABBCC", "00:11:22:AA:BB:CC" or "00-11-22-AA-BB-CC".</param>
        /// <returns>The 6-byte representation of the MAC address.</returns>
        /// <exception cref="ArgumentException">Thrown when the MAC address is null, empty or not a valid 6-byte MAC.</exception>
        public static byte[] ParseMacAddress(string macAddress)
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                throw new ArgumentException("MAC address is empty.", nameof(macAddress));

            string normalized = macAddress
                .Replace(":", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace(" ", "")
                .Trim();

            if (normalized.Length != 12)
                throw new ArgumentException($"Invalid MAC address: '{macAddress}'.", nameof(macAddress));

            byte[] bytes = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                if (!byte.TryParse(normalized.Substring(i * 2, 2), NumberStyles.HexNumber,
                                   CultureInfo.InvariantCulture, out bytes[i]))
                    throw new ArgumentException($"Invalid MAC address: '{macAddress}'.", nameof(macAddress));
            }

            return bytes;
        }

        /// <summary>
        /// Builds a 102-byte Wake-on-LAN magic packet for the given 6-byte MAC address.
        /// </summary>
        /// <param name="macBytes">The 6-byte MAC address.</param>
        /// <returns>The magic packet payload.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="macBytes"/> is not 6 bytes long.</exception>
        public static byte[] BuildMagicPacket(byte[] macBytes)
        {
            if (macBytes == null || macBytes.Length != 6)
                throw new ArgumentException("MAC address must be 6 bytes.", nameof(macBytes));

            byte[] packet = new byte[102];
            for (int i = 0; i < 6; i++)
                packet[i] = 0xFF;

            for (int i = 1; i <= 16; i++)
                Array.Copy(macBytes, 0, packet, i * 6, 6);

            return packet;
        }

        /// <summary>
        /// Sends a Wake-on-LAN magic packet for the given MAC address as a UDP broadcast.
        /// </summary>
        /// <param name="macAddress">The target MAC address.</param>
        /// <param name="port">The UDP port to send to (defaults to <see cref="DefaultPort"/>).</param>
        public static void Send(string macAddress, int port = DefaultPort)
        {
            byte[] packet = BuildMagicPacket(ParseMacAddress(macAddress));

            using UdpClient client = new UdpClient { EnableBroadcast = true };
            client.Send(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, port));
        }

        /// <summary>
        /// Attempts to send a Wake-on-LAN magic packet, logging any failure to the
        /// message collector instead of throwing.
        /// </summary>
        /// <param name="macAddress">The target MAC address.</param>
        /// <param name="port">The UDP port to send to (defaults to <see cref="DefaultPort"/>).</param>
        /// <returns><see langword="true"/> if the packet was sent successfully; otherwise <see langword="false"/>.</returns>
        public static bool TrySend(string macAddress, int port = DefaultPort)
        {
            try
            {
                Send(macAddress, port);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    string.Format(Language.WakeOnLanSent, macAddress));
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    $"Failed to send Wake-on-LAN magic packet to {macAddress}.", ex);
                return false;
            }
        }
    }
}
