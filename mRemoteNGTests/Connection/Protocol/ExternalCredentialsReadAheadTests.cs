using System.Threading;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.SSH;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    // OpenConnection reads the credentials before it attaches the protocol to its tab, so the read must not
    // need the protocol's InterfaceControl. An empty 1Password reference fails before op is started, and the
    // failure is kept for Initialize and Connect instead of being thrown here.
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ExternalCredentialsReadAheadTests
    {
        private static ConnectionInfo OnePasswordConnection() => new()
        {
            ExternalCredentialProvider = ExternalCredentialProvider.OnePassword,
            UserViaAPI = ""
        };

        [Test]
        public void PuttyReadsBeforeItIsAttachedToATab()
        {
            var protocol = new ProtocolSSH2();

            Assert.That(protocol.InterfaceControl, Is.Null);
            Assert.DoesNotThrowAsync(() => protocol.ReadExternalCredentialsAsync(OnePasswordConnection()));
        }

        [Test]
        public void RdpReadsBeforeItIsAttachedToATab()
        {
            var protocol = new RdpProtocol();

            Assert.That(protocol.InterfaceControl, Is.Null);
            Assert.DoesNotThrowAsync(() => protocol.ReadExternalCredentialsAsync(OnePasswordConnection()));
        }
    }
}
