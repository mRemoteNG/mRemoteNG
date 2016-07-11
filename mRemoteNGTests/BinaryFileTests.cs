using NUnit.Framework;
using System.IO;

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

        static string GetTargetPath([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            string debugOrRelease = "";
            string normalOrPortable = "";
#if DEBUG
            debugOrRelease = "Debug";
#else
            debugOrRelease = "Release";
#endif
#if PORTABLE
            normalOrPortable = " Portable";
#else
            normalOrPortable = "";
#endif
            var path = Path.GetDirectoryName(sourceFilePath);
            string FilePath = $"{path}\\..\\mRemoteV1\\bin\\{debugOrRelease}{normalOrPortable}\\mRemoteNG.exe";
            return FilePath;
        }

        static bool IsLargeAware(string file)
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
        /// <exception cref=""></exception>
        /// <returns></returns>
        static bool IsLargeAware(Stream stream)
        {
            const int IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x20;

            var br = new BinaryReader(stream);

            if (br.ReadInt16() != 0x5A4D)       //No MZ Header
                return false;

            br.BaseStream.Position = 0x3C;
            var peloc = br.ReadInt32();         //Get the PE header location.

            br.BaseStream.Position = peloc;
            if (br.ReadInt32() != 0x4550)       //No PE header
                return false;

            br.BaseStream.Position += 0x12;
            return (br.ReadInt16() & IMAGE_FILE_LARGE_ADDRESS_AWARE) == IMAGE_FILE_LARGE_ADDRESS_AWARE;
        }
    }
}