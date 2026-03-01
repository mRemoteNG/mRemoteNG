using System.IO;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    [TestFixture]
    public class ExternalToolIconTests
    {
        [Test]
        public void IconPath_IsStoredCorrectly()
        {
            var tool = new ExternalTool();
            tool.IconPath = @"C:	est.ico";
            Assert.That(tool.IconPath, Is.EqualTo(@"C:	est.ico"));
        }
    }
}