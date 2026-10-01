using mRemoteNG.Connection;
using mRemoteNG.UI.Controls;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ExternalToolsToolStripTests
    {
        [TestCaseSource(nameof(ExternalToolSelections))]
        public void CanPassConnectionToExternalTool_ReturnsExpectedResult(ConnectionInfo selectedNode, bool expected)
        {
            Assert.That(ExternalToolsToolStrip.CanPassConnectionToExternalTool(selectedNode), Is.EqualTo(expected));
        }

        private static readonly object[] ExternalToolSelections =
        {
            new object[] { null, false },
            new object[] { new ConnectionInfo(), true },
            new object[] { new PuttySessionInfo(), true },
            new object[] { new ContainerInfo(), false }
        };
    }
}
