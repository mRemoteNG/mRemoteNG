using NUnit.Framework;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;

namespace mRemoteNGTests.Connection.Protocol.RDP
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class RdpProtocolDesktopScaleFactorTests
    {
        private class TestableRdpProtocol : RdpProtocol
        {
            public void SetConnectionInfo(ConnectionInfo info)
            {
                this.connectionInfo = info;
            }

            public uint GetDesktopScaleFactor()
            {
                return this.DesktopScaleFactor;
            }
        }

        [Test]
        public void DesktopScaleFactor_Returns100_WhenSetToScale100()
        {
            var protocol = new TestableRdpProtocol();
            var info = new ConnectionInfo { DesktopScaleFactor = RDPDesktopScaleFactor.Scale100 };
            protocol.SetConnectionInfo(info);

            Assert.That(protocol.GetDesktopScaleFactor(), Is.EqualTo(100));
        }

        [Test]
        public void DesktopScaleFactor_Returns125_WhenSetToScale125()
        {
            var protocol = new TestableRdpProtocol();
            var info = new ConnectionInfo { DesktopScaleFactor = RDPDesktopScaleFactor.Scale125 };
            protocol.SetConnectionInfo(info);

            Assert.That(protocol.GetDesktopScaleFactor(), Is.EqualTo(125));
        }

        [Test]
        public void DesktopScaleFactor_Returns150_WhenSetToScale150()
        {
            var protocol = new TestableRdpProtocol();
            var info = new ConnectionInfo { DesktopScaleFactor = RDPDesktopScaleFactor.Scale150 };
            protocol.SetConnectionInfo(info);

            Assert.That(protocol.GetDesktopScaleFactor(), Is.EqualTo(150));
        }

        [Test]
        public void DesktopScaleFactor_Returns200_WhenSetToScale200()
        {
            var protocol = new TestableRdpProtocol();
            var info = new ConnectionInfo { DesktopScaleFactor = RDPDesktopScaleFactor.Scale200 };
            protocol.SetConnectionInfo(info);

            Assert.That(protocol.GetDesktopScaleFactor(), Is.EqualTo(200));
        }

        [Test]
        public void DesktopScaleFactor_ReturnsCalculatedValue_WhenSetToAuto()
        {
            var protocol = new TestableRdpProtocol();
            var info = new ConnectionInfo { DesktopScaleFactor = RDPDesktopScaleFactor.Auto };
            protocol.SetConnectionInfo(info);

            // Default calculated value is 100 because _frmMain is null/disposed in this context
            // so ResolutionScalingFactor returns 1.0f
            Assert.That(protocol.GetDesktopScaleFactor(), Is.EqualTo(100));
        }

        [Test]
        public void DesktopScaleFactor_ReturnsCalculatedValue_WhenConnectionInfoIsNull()
        {
            var protocol = new TestableRdpProtocol();
            protocol.SetConnectionInfo(null);

            // Default calculated value is 100
            Assert.That(protocol.GetDesktopScaleFactor(), Is.EqualTo(100));
        }
    }
}
