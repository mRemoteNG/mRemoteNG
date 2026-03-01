using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;

namespace mRemoteNG.Config.Putty
{
    [SupportedOSPlatform("windows")]
    public static class PuttySessionNameDecoder
    {
        public static string Decode(string encodedSessionName, Encoding? fallbackEncoding = null)
        {
            if (string.IsNullOrEmpty(encodedSessionName))
                return encodedSessionName ?? string.Empty;

            string utf8Decoded = DecodeWithEncoding(encodedSessionName, Encoding.UTF8);
            if (!utf8Decoded.Contains('\uFFFD'))
                return utf8Decoded;

            Encoding fallback;
            try
            {
                fallback = fallbackEncoding ?? Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
            }
            catch
            {
                return utf8Decoded; // codepage not available, return best-effort UTF-8
            }

            return DecodeWithEncoding(encodedSessionName, fallback);
        }

        private static string DecodeWithEncoding(string encodedSessionName, Encoding encoding)
        {
            List<byte> bytes = new(encodedSessionName.Length);
            for (int i = 0; i < encodedSessionName.Length; i++)
            {
                char currentCharacter = encodedSessionName[i];
                if (currentCharacter == '%' &&
                    i + 2 < encodedSessionName.Length &&
                    IsHex(encodedSessionName[i + 1]) &&
                    IsHex(encodedSessionName[i + 2]))
                {
                    bytes.Add(ParseHexByte(encodedSessionName[i + 1], encodedSessionName[i + 2]));
                    i += 2;
                }
                else
                {
                    bytes.AddRange(encoding.GetBytes([currentCharacter]));
                }
            }

            return encoding.GetString(bytes.ToArray());
        }

        private static bool IsHex(char value)
        {
            return value is >= '0' and <= '9' ||
                   value is >= 'a' and <= 'f' ||
                   value is >= 'A' and <= 'F';
        }

        private static byte ParseHexByte(char high, char low)
        {
            return (byte)((HexValue(high) << 4) + HexValue(low));
        }

        private static int HexValue(char value)
        {
            if (value is >= '0' and <= '9')
                return value - '0';
            if (value is >= 'a' and <= 'f')
                return value - 'a' + 10;
            return value - 'A' + 10;
        }
    }
}
