using NUnit.Framework;
using System.IO;
using System.Runtime.CompilerServices;

namespace mRemoteNGTests
{
    [TestFixture]
    public class BinaryFileTests
    {
        [Test]
        public void LargeAddressAwareFlagIsSet()
        {
            var exePath = GetTargetPath();
            Assert.That(IsLargeAware(exePath), Is.True);
        }

        public string GetTargetPath([CallerFilePath] string sourceFilePath = "")
        {
            const string debugOrRelease =
#if DEBUG
				"Debug";
#else
				"Release";
#endif

            const string normalOrPortable =
#if PORTABLE
            " Portable";
#else
            "";
#endif
            var path = Path.GetDirectoryName(sourceFilePath);
            var filePath = $"{path}\\..\\mRemoteV1\\bin\\{debugOrRelease}{normalOrPortable}\\mRemoteNG.exe";
            return filePath;
        }

        private bool IsLargeAware(string file)
        {
            using (var fs = File.OpenRead(file))
            {
                return IsLargeAware(fs);
            }
        }

        /// <summary>
        /// Checks if the stream is a MZ header and if it is large address aware
        /// </summary>
        /// <param name="stream">Stream to check, make sure its at the start of the MZ header</param>
        /// <returns></returns>
        private bool IsLargeAware(Stream stream)
        {
            const int imageFileLargeAddressAware = 0x20;

            var br = new BinaryReader(stream);

            if (br.ReadInt16() != 0x5A4D)       //No MZ Header
                return false;

            br.BaseStream.Position = 0x3C;
            var peHeaderLocation = br.ReadInt32();         //Get the PE header location.

            br.BaseStream.Position = peHeaderLocation;
            if (br.ReadInt32() != 0x4550)       //No PE header
                return false;

            br.BaseStream.Position += 0x12;
            return (br.ReadInt16() & imageFileLargeAddressAware) == imageFileLargeAddressAware;
        }
    }
}