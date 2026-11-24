using mRemoteNG.Connection;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    /// <summary>
    /// New connections get the default panel's name as their Panel, so only another name keeps a
    /// connection in its own panel; the rest open in the connection panel selected last.
    /// </summary>
    public class ConnectionInitiatorPanelTests
    {
        [TestCase("Prod")]
        [TestCase("R&D")]
        public void AnotherPanelNameIsTheConnectionsOwnPanel(string panel)
        {
            Assert.That(ConnectionInitiator.HasOwnPanel(new ConnectionInfo { Panel = panel }), Is.True);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("General")]
        [TestCase("general")]
        public void NoPanelOrTheDefaultPanelIsNotOwnPanel(string panel)
        {
            Assert.That(ConnectionInitiator.HasOwnPanel(new ConnectionInfo { Panel = panel }), Is.False);
        }
    }
}
