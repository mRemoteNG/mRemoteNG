using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class InterfaceControlFrameColorTests
    {
        private Form _testForm;
        private ProtocolBase _mockProtocol;

        [SetUp]
        public void Setup()
        {
            _testForm = new Form { Size = new Size(1024, 768) };
            _mockProtocol = Substitute.For<ProtocolBase>();
        }

        [TearDown]
        public void Teardown()
        {
            _testForm?.Dispose();
        }

        private InterfaceControl CreateControl(ConnectionFrameColor frameColor)
        {
            var info = new ConnectionInfo
            {
                Protocol = ProtocolType.RDP,
                Hostname = "test-host",
                ConnectionFrameColor = frameColor
            };

            return new InterfaceControl(_testForm, _mockProtocol, info)
            {
                Size = new Size(800, 600)
            };
        }

        [Test]
        public void NoFrameColor_ClientAreaFillsControl()
        {
            using InterfaceControl ic = CreateControl(ConnectionFrameColor.None);
            // Force handle creation so WM_NCCALCSIZE is processed.
            var _ = ic.Handle;

            Assert.That(ic.ClientSize, Is.EqualTo(new Size(800, 600)));
        }

        [Test]
        public void FrameColorSet_ReservesNonClientBorder()
        {
            using InterfaceControl ic = CreateControl(ConnectionFrameColor.Red);
            var _ = ic.Handle;

            // The non-client frame (4px on each side) is reserved, so the client area shrinks by 8px
            // in each dimension. This is what keeps child controls from covering the colored frame.
            Assert.That(ic.ClientSize, Is.EqualTo(new Size(800 - 8, 600 - 8)));
        }

        [Test]
        public void RefreshFrameColor_UpdatesNonClientArea()
        {
            using InterfaceControl ic = CreateControl(ConnectionFrameColor.None);
            var _ = ic.Handle;
            Assert.That(ic.ClientSize, Is.EqualTo(new Size(800, 600)));

            ic.Info.ConnectionFrameColor = ConnectionFrameColor.Green;
            ic.RefreshFrameColor();

            Assert.That(ic.ClientSize, Is.EqualTo(new Size(800 - 8, 600 - 8)));
        }
    }
}
