using mRemoteNG.Config.Connections;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mRemoteNGTests.Config.Connections;

internal class XmlConnectionsLoaderTests
{
    [Test]
    public void ThrowsFileNotFound()
    {
        Assert.Throws<FileNotFoundException>(() => new XmlConnectionsLoader(FileTestHelpers.NewTempFilePath()).Load());
    }
}