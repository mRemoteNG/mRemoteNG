using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace mRemoteNG.Plugins.PortScan;

/// <summary>
/// Parses the port scanner's single address field into a start/end address pair. Three notations
/// are accepted, in either IPv4 or IPv6 form:
/// <list type="bullet">
/// <item><description>a single address — <c>192.168.1.1</c> / <c>2001:db8::1</c></description></item>
/// <item><description>an explicit range — <c>192.168.1.1 - 192.168.1.254</c></description></item>
/// <item><description>a CIDR block — <c>192.168.1.0/24</c> / <c>2001:db8::/120</c></description></item>
/// </list>
/// </summary>
internal static class IpRangeParser
{
    /// <summary>Example text shown to the user when the field is empty or unparsable.</summary>
    public const string SyntaxHint =
        "Enter a single address, a range (192.168.1.1 - 192.168.1.254) or a CIDR block (192.168.1.0/24). IPv4 and IPv6 are supported.";

    /// <summary>
    /// Attempts to parse <paramref name="text"/>. On success <paramref name="start"/> and
    /// <paramref name="end"/> are the inclusive bounds of the range, ordered lowest first;
    /// on failure <paramref name="error"/> explains why.
    /// </summary>
    public static bool TryParse(string? text, out IPAddress? start, out IPAddress? end, out string error)
    {
        start = null;
        end = null;

        string input = text?.Trim() ?? string.Empty;
        if (input.Length == 0)
        {
            error = SyntaxHint;
            return false;
        }

        // '/' and '-' cannot occur inside an IPv4 or IPv6 literal, so the first occurrence of
        // either unambiguously identifies the notation.
        int slashIndex = input.IndexOf('/');
        if (slashIndex >= 0)
            return TryParseCidr(input[..slashIndex].Trim(), input[(slashIndex + 1)..].Trim(),
                                out start, out end, out error);

        int dashIndex = input.IndexOf('-');
        if (dashIndex >= 0)
            return TryParseRange(input[..dashIndex].Trim(), input[(dashIndex + 1)..].Trim(),
                                 out start, out end, out error);

        if (!TryParseAddress(input, out IPAddress? single, out error))
            return false;

        start = single;
        end = single;
        return true;
    }

    private static bool TryParseRange(string startText,
                                      string endText,
                                      out IPAddress? start,
                                      out IPAddress? end,
                                      out string error)
    {
        start = null;
        end = null;

        if (!TryParseAddress(startText, out IPAddress? first, out error) ||
            !TryParseAddress(endText, out IPAddress? last, out error))
            return false;

        if (first!.AddressFamily != last!.AddressFamily)
        {
            error = "A range cannot mix IPv4 and IPv6 addresses.";
            return false;
        }

        if (first.AddressFamily == AddressFamily.InterNetworkV6 && first.ScopeId != last.ScopeId)
        {
            error = "IPv6 range endpoints must use the same scope ID.";
            return false;
        }

        // Accept the endpoints in either order so "192.168.1.254 - 192.168.1.1" still works.
        if (Compare(first, last) > 0)
            (first, last) = (last, first);

        start = first;
        end = last;
        return true;
    }

    private static bool TryParseCidr(string addressText,
                                     string prefixText,
                                     out IPAddress? start,
                                     out IPAddress? end,
                                     out string error)
    {
        start = null;
        end = null;

        if (!TryParseAddress(addressText, out IPAddress? address, out error))
            return false;

        byte[] addressBytes = address!.GetAddressBytes();
        int bitCount = addressBytes.Length * 8;

        if (!int.TryParse(prefixText, NumberStyles.None, CultureInfo.InvariantCulture, out int prefixLength) ||
            prefixLength > bitCount)
        {
            error = string.Format(CultureInfo.CurrentCulture,
                                  "'{0}' is not a valid prefix length (0-{1}).", prefixText, bitCount);
            return false;
        }

        byte[] firstBytes = new byte[addressBytes.Length];
        byte[] lastBytes = new byte[addressBytes.Length];

        for (int i = 0; i < addressBytes.Length; i++)
        {
            int bitsInThisByte = Math.Clamp(prefixLength - (i * 8), 0, 8);
            byte mask = (byte)(bitsInThisByte == 0 ? 0x00 : 0xFF << (8 - bitsInThisByte));

            // Network address (host bits cleared) and broadcast/last address (host bits set).
            firstBytes[i] = (byte)(addressBytes[i] & mask);
            lastBytes[i] = (byte)(addressBytes[i] | (byte)~mask);
        }

        start = address.AddressFamily == AddressFamily.InterNetworkV6
            ? new IPAddress(firstBytes, address.ScopeId)
            : new IPAddress(firstBytes);
        end = address.AddressFamily == AddressFamily.InterNetworkV6
            ? new IPAddress(lastBytes, address.ScopeId)
            : new IPAddress(lastBytes);
        error = string.Empty;
        return true;
    }

    private static bool TryParseAddress(string text, out IPAddress? address, out string error)
    {
        if (IPAddress.TryParse(text, out address) &&
            address.AddressFamily is AddressFamily.InterNetwork or AddressFamily.InterNetworkV6)
        {
            error = string.Empty;
            return true;
        }

        address = null;
        error = string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid IP address.", text);
        return false;
    }

    /// <summary>
    /// Compares two same-family addresses as unsigned big-endian integers.
    /// </summary>
    private static int Compare(IPAddress address1, IPAddress address2)
    {
        byte[] bytes1 = address1.GetAddressBytes();
        byte[] bytes2 = address2.GetAddressBytes();

        for (int i = 0; i < bytes1.Length; i++)
        {
            if (bytes1[i] != bytes2[i])
                return bytes1[i] < bytes2[i] ? -1 : 1;
        }

        return 0;
    }
}
