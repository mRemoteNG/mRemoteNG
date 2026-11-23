using System;
using System.IO;
using mRemoteNG.UI;
using NUnit.Framework;

namespace mRemoteNGTests.Themes
{
    [TestFixture]
    public class FluentConnectionIconTests
    {
        [Test]
        public void DefaultReplacement_RequiresOriginalBytesAndOriginalName()
        {
            string folder = Path.Combine(Path.GetTempPath(), "mRemoteNG-icons", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "default.ico");
            try
            {
                using (Stream source = typeof(StatusImageList).Assembly.GetManifestResourceStream("mRemoteNG.Themes.FluentIcons.StockDefault.ico"))
                using (Stream destination = File.Create(path)) source.CopyTo(destination);
                Assert.That(StatusImageList.IsUnmodifiedDefaultIcon("mRemoteNG", path), Is.True);
                Assert.That(StatusImageList.IsUnmodifiedDefaultIcon("MyCustomIcon", path), Is.False);
                byte[] changed = File.ReadAllBytes(path); changed[changed.Length - 1] ^= 1; File.WriteAllBytes(path, changed);
                Assert.That(StatusImageList.IsUnmodifiedDefaultIcon("mRemoteNG", path), Is.False);
                Assert.That(StatusImageList.IsUnmodifiedDefaultIcon("mRemoteNG", Path.Combine(folder, "missing.ico")), Is.False);
            }
            finally { Directory.Delete(folder, true); }
        }
    }
}
