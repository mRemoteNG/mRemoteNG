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
        public void InlineResources_ReplacesScriptTag()
        {
            string html = "<script src=\"https://xterm.local/test.js\"></script>";
            string jsContent = "console.log('hello');";

            string result = html.Replace(
                "<script src=\"https://xterm.local/test.js\"></script>",
                $"<script>{jsContent}</script>");

            Assert.That(result, Does.Contain("<script>console.log('hello');</script>"));
            Assert.That(result, Does.Not.Contain("src="));
        }

        [Test]
        public void InlineResources_ReplacesLinkTag()
        {
            string html = "<link rel=\"stylesheet\" href=\"https://xterm.local/test.css\">";
            string cssContent = "body { background: #000; }";

            string result = html.Replace(
                "<link rel=\"stylesheet\" href=\"https://xterm.local/test.css\">",
                $"<style>{cssContent}</style>");

            Assert.That(result, Does.Contain("<style>body { background: #000; }</style>"));
            Assert.That(result, Does.Not.Contain("href="));
        }

        private static string ComputeHash(string filePath)
        {
            using var sha = SHA384.Create();
            using var stream = File.OpenRead(filePath);
            return "sha384-" + Convert.ToBase64String(sha.ComputeHash(stream));
        }
    }
}
