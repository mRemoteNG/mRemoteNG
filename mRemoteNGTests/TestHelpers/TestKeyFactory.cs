using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace mRemoteNGTests.TestHelpers
{
    /// <summary>
    /// Generates throwaway SSH private keys at test time (via BouncyCastle) so key-based auth can be
    /// tested without committing any key material. Keys are written to temp files the caller deletes.
    /// </summary>
    internal static class TestKeyFactory
    {
        /// <summary>Generates a 2048-bit RSA private key in PEM (PKCS#1), optionally encrypted with a passphrase.</summary>
        public static string GenerateRsaPrivateKeyPem(string passphrase = null)
        {
            var generator = new RsaKeyPairGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            using var stringWriter = new StringWriter();
            var pemWriter = new PemWriter(stringWriter);
            if (string.IsNullOrEmpty(passphrase))
                pemWriter.WriteObject(keyPair.Private);
            else
                pemWriter.WriteObject(keyPair.Private, "AES-256-CBC", passphrase.ToCharArray(), new SecureRandom());
            pemWriter.Writer.Flush();
            return stringWriter.ToString();
        }

        /// <summary>Writes a PEM key to a unique temp file and returns its path.</summary>
        public static string WriteToTempFile(string pem)
        {
            string path = Path.Combine(Path.GetTempPath(), "mrng_test_key_" + Guid.NewGuid().ToString("N") + ".pem");
            File.WriteAllText(path, pem);
            return path;
        }
    }
}
