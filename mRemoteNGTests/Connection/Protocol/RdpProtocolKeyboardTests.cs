using mRemoteNG.Connection.Protocol.RDP;
using MSTSCLib;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpProtocolKeyboardTests
    {
        [TestCase(2, false, 0)]
        [TestCase(2, true, 1)]
        [TestCase(1, false, 0)]
        [TestCase(0, true, 1)]
        [TestCase(0, false, 0)]
        [TestCase(1, true, 1)]
        public void ConfigureKeyboardRedirection_OverridesPreviousMode(int previousMode, bool redirectKeys, int expectedMode)
        {
            var securedSettings = Substitute.For<IMsRdpClientSecuredSettings>();
            securedSettings.KeyboardHookMode = previousMode;
            securedSettings.ClearReceivedCalls();

            RdpProtocol.ConfigureKeyboardRedirection(securedSettings, redirectKeys);

            securedSettings.Received(1).KeyboardHookMode = expectedMode;
            Assert.That(securedSettings.KeyboardHookMode, Is.EqualTo(expectedMode));
        }
    }
}
