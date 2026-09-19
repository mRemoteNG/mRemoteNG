using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpProtocolPasswordTests
    {
        [TestCase(RdpVersion.Rdc6, true, false, true)]
        [TestCase(RdpVersion.Rdc7, false, true, true)]
        [TestCase(RdpVersion.Rdc8, false, false, true)]
        [TestCase(RdpVersion.Rdc8, true, false, false)]
        [TestCase(RdpVersion.Rdc8, false, true, false)]
        [TestCase(RdpVersion.Rdc11, true, true, false)]
        public void ShouldAssignClearTextPassword_ReturnsExpectedValue(
            RdpVersion protocolVersion,
            bool useRestrictedAdmin,
            bool useRemoteCredentialGuard,
            bool expected)
        {
            var connectionInfo = new ConnectionInfo
            {
                UseRestrictedAdmin = useRestrictedAdmin,
                UseRCG = useRemoteCredentialGuard
            };
            var protocol = new TestableRdpProtocol(protocolVersion);

            Assert.That(protocol.ShouldAssignPassword(connectionInfo), Is.EqualTo(expected));
        }

        private sealed class TestableRdpProtocol : RdpProtocol
        {
            private readonly RdpVersion _protocolVersion;

            public TestableRdpProtocol(RdpVersion protocolVersion)
            {
                _protocolVersion = protocolVersion;
            }

            protected override RdpVersion RdpProtocolVersion => _protocolVersion;

            public bool ShouldAssignPassword(ConnectionInfo connectionInfo)
            {
                return ShouldAssignClearTextPassword(connectionInfo);
            }
        }
    }
}
