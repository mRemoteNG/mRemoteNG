using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace mRemoteNG.Security
{
    /// <summary>
    /// RFC 6238 TOTP (Time-based One-Time Password) provider.
    /// Generates and validates 6-digit TOTP codes with a 30-second time step.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class TotpProvider
    {
        private const int TimeStepSeconds = 30;
        private const int CodeDigits = 6;
        private static readonly int[] PowersOfTen = [1, 10, 100, 1000, 10000, 100000, 1000000];
        private const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        /// Generates a new random 160-bit TOTP secret encoded as Base32.
        /// </summary>
        public static string GenerateSecret()
        {
            byte[] secretBytes = new byte[20]; // 160 bits
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(secretBytes);
            return Base32Encode(secretBytes);
        }

        /// <summary>
        /// Generates the current TOTP code for the given Base32-encoded secret.
        /// </summary>
        public static string GenerateCode(string base32Secret)
        {
            return GenerateCode(base32Secret, DateTimeOffset.UtcNow);
        }

        /// <summary>
        /// Generates a TOTP code for the given Base32-encoded secret at a specific time.
        /// </summary>
        public static string GenerateCode(string base32Secret, DateTimeOffset timestamp)
        {
            byte[] key = Base32Decode(base32Secret);
            long timeStep = timestamp.ToUnixTimeSeconds() / TimeStepSeconds;
            byte[] timeBytes = BitConverter.GetBytes(timeStep);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(timeBytes);

#pragma warning disable S4790 // RFC 6238 compatibility: TOTP commonly uses HMAC-SHA1.
            using HMACSHA1 hmac = new(key);
#pragma warning restore S4790
            byte[] hash = hmac.ComputeHash(timeBytes);

            int offset = hash[^1] & 0x0F;
            int binaryCode = ((hash[offset] & 0x7F) << 24)
                           | ((hash[offset + 1] & 0xFF) << 16)
                           | ((hash[offset + 2] & 0xFF) << 8)
                           | (hash[offset + 3] & 0xFF);

            int otp = binaryCode % PowersOfTen[CodeDigits];
            return otp.ToString(CultureInfo.InvariantCulture).PadLeft(CodeDigits, '0');
        }

        /// <summary>
        /// Validates a TOTP code against the given secret, allowing a window of +/- 1 time step.
        /// </summary>
        public static bool ValidateCode(string base32Secret, string code)
        {
            if (string.IsNullOrWhiteSpace(base32Secret) || string.IsNullOrWhiteSpace(code))
                return false;

            string trimmedCode = code.Trim();
            if (trimmedCode.Length != CodeDigits)
                return false;

            DateTimeOffset now = DateTimeOffset.UtcNow;

            // Check current time step and +/- 1 to allow for clock skew
            for (int i = -1; i <= 1; i++)
            {
                DateTimeOffset checkTime = now.AddSeconds(i * TimeStepSeconds);
                string expected = GenerateCode(base32Secret, checkTime);
                if (string.Equals(expected, trimmedCode, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Generates an otpauth:// URI suitable for QR code generation.
        /// </summary>
        public static string GenerateOtpAuthUri(string secret, string accountName, string issuer = "mRemoteNG")
        {
            string encodedIssuer = Uri.EscapeDataString(issuer);
            string encodedAccount = Uri.EscapeDataString(accountName);
            return $"otpauth://totp/{encodedIssuer}:{encodedAccount}?secret={secret}&issuer={encodedIssuer}&algorithm=SHA1&digits={CodeDigits}&period={TimeStepSeconds}";
        }

        /// <summary>
        /// Encodes a byte array to a Base32 string (RFC 4648).
        /// </summary>
        internal static string Base32Encode(byte[] data)
        {
            if (data.Length == 0)
                return string.Empty;

            StringBuilder result = new((data.Length * 8 + 4) / 5);
            int buffer = data[0];
            int bitsLeft = 8;
            int next = 1;

            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < 5)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= data[next++] & 0xFF;
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = 5 - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }

                int index = 0x1F & (buffer >> (bitsLeft - 5));
                bitsLeft -= 5;
                result.Append(Base32Chars[index]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Decodes a Base32 string (RFC 4648) to a byte array.
        /// </summary>
        internal static byte[] Base32Decode(string base32)
        {
            if (string.IsNullOrEmpty(base32))
                return [];

            string cleaned = base32.TrimEnd('=').ToUpperInvariant();
            byte[] output = new byte[cleaned.Length * 5 / 8];
            int bitIndex = 0;
            int inputIndex = 0;
            int outputBits = 0;
            int outputIndex = 0;

            while (inputIndex < cleaned.Length)
            {
                int val = Base32Chars.IndexOf(cleaned[inputIndex]);
                if (val < 0)
                    throw new FormatException($"Invalid Base32 character: {cleaned[inputIndex]}");

                bitIndex = (bitIndex << 5) | val;
                outputBits += 5;

                if (outputBits >= 8)
                {
                    output[outputIndex++] = (byte)(bitIndex >> (outputBits - 8));
                    outputBits -= 8;
                    bitIndex &= (1 << outputBits) - 1;
                }

                inputIndex++;
            }

            return output;
        }
    }
}
