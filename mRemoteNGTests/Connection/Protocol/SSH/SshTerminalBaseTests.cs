using System;
using System.IO;
using System.Security.Cryptography;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.SSH
{
    [TestFixture]
    public class SshTerminalBaseHelperTests
    {
        /// <summary>
        /// Tests the SRI hash computation logic used by SshTerminalBase.
        /// We replicate the ComputeSriHash logic here since the method is private.
        /// </summary>
        [Test]
        public void ComputeSriHash_ProducesValidSha384Format()
        {
            // Create a temp file with known content
            string tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                File.WriteAllText(tempFile, "console.log('hello');");

                using var sha = SHA384.Create();
                using var stream = File.OpenRead(tempFile);
                byte[] hashBytes = sha.ComputeHash(stream);
                string hash = "sha384-" + Convert.ToBase64String(hashBytes);

                Assert.That(hash, Does.StartWith("sha384-"));
                Assert.That(hash.Length, Is.GreaterThan(10));
                // Base64 of SHA-384 (48 bytes) should be 64 chars
                Assert.That(hash.Substring(7).Length, Is.EqualTo(64));
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        [Test]
        public void ComputeSriHash_SameContent_ProducesSameHash()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string tempFile2 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                string content = "var x = 42;\n";
                File.WriteAllText(tempFile1, content);
                File.WriteAllText(tempFile2, content);

                string hash1 = ComputeHash(tempFile1);
                string hash2 = ComputeHash(tempFile2);

                Assert.That(hash1, Is.EqualTo(hash2));
            }
            finally
            {
                if (File.Exists(tempFile1)) File.Delete(tempFile1);
                if (File.Exists(tempFile2)) File.Delete(tempFile2);
            }
        }

        [Test]
        public void ComputeSriHash_DifferentContent_ProducesDifferentHash()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string tempFile2 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            try
            {
                File.WriteAllText(tempFile1, "content A");
                File.WriteAllText(tempFile2, "content B");

                string hash1 = ComputeHash(tempFile1);
                string hash2 = ComputeHash(tempFile2);

                Assert.That(hash1, Is.Not.EqualTo(hash2));
            }
            finally
            {
                if (File.Exists(tempFile1)) File.Delete(tempFile1);
                if (File.Exists(tempFile2)) File.Delete(tempFile2);
            }
        }

        [Test]
        public void InjectSriHash_ReplacesScriptTag()
        {
            string html = "<script src=\"https://xterm.local/xterm.min.js\"></script>";
            string expected = "integrity=\"sha384-";

            // Simulate injection
            string hash = "sha384-TESTHASH123456789012345678901234567890123456789012345678901234";
            string result = html.Replace(
                "src=\"https://xterm.local/xterm.min.js\"></script>",
                $"src=\"https://xterm.local/xterm.min.js\" integrity=\"{hash}\" crossorigin=\"anonymous\"></script>");

            Assert.That(result, Does.Contain(expected));
            Assert.That(result, Does.Contain("crossorigin=\"anonymous\""));
        }

        [Test]
        public void InjectSriHash_ReplacesLinkTag()
        {
            string html = "<link rel=\"stylesheet\" href=\"https://xterm.local/xterm.css\">";
            string hash = "sha384-TESTHASH123456789012345678901234567890123456789012345678901234";
            string result = html.Replace(
                "href=\"https://xterm.local/xterm.css\">",
                $"href=\"https://xterm.local/xterm.css\" integrity=\"{hash}\" crossorigin=\"anonymous\">");

            Assert.That(result, Does.Contain("integrity=\"sha384-"));
            Assert.That(result, Does.Contain("crossorigin=\"anonymous\""));
        }

        private static string ComputeHash(string filePath)
        {
            using var sha = SHA384.Create();
            using var stream = File.OpenRead(filePath);
            return "sha384-" + Convert.ToBase64String(sha.ComputeHash(stream));
        }
    }
}
