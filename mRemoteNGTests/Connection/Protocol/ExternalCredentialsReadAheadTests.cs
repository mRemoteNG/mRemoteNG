using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

        private static ConnectionInfo RdpConnection(
            ExternalCredentialProvider connectionProvider,
            RDGatewayUseConnectionCredentials gatewayMode,
            ExternalCredentialProvider gatewayProvider) => new()
        {
            ExternalCredentialProvider = connectionProvider,
            UserViaAPI = "",
            RDGatewayUsageMethod = RDGatewayUsageMethod.Always,
            RDGatewayUseConnectionCredentials = gatewayMode,
            RDGatewayExternalCredentialProvider = gatewayProvider,
            RDGatewayUserViaAPI = ""
        };

        // What SetCredentials and SetRdGateway will use instead of running op themselves
        private static object ConnectionReadAhead(RdpProtocol protocol) => ReadAhead(protocol, "_onePasswordRead");

        private static object GatewayReadAhead(RdpProtocol protocol) => ReadAhead(protocol, "_onePasswordGatewayRead");

        private static object ReadAhead(RdpProtocol protocol, string field) =>
            typeof(RdpProtocol).GetField(field, BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(protocol);

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

        // Without a read ahead, SetRdGateway would run op on the UI thread (#3628)
        [TestCase(ExternalCredentialProvider.None)]
        [TestCase(ExternalCredentialProvider.DelineaSecretServer)]
        [TestCase(ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(ExternalCredentialProvider.VaultOpenbao)]
        public async Task RdpReadsAGatewayOn1PasswordOnAConnectionThatDoesNotUseIt(ExternalCredentialProvider connectionProvider)
        {
            var protocol = new RdpProtocol();

            await protocol.ReadExternalCredentialsAsync(
                RdpConnection(connectionProvider, RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.OnePassword));

            Assert.That(GatewayReadAhead(protocol), Is.Not.Null);
            Assert.That(ConnectionReadAhead(protocol), Is.Null);
        }

        // In the "external credential provider" mode a gateway without a provider of its own uses the connection's
        [Test]
        public async Task RdpReadsAGatewayThatUsesTheConnections1Password()
        {
            var protocol = new RdpProtocol();

            await protocol.ReadExternalCredentialsAsync(RdpConnection(ExternalCredentialProvider.OnePassword,
                RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.None));

            Assert.That(GatewayReadAhead(protocol), Is.Not.Null);
            Assert.That(ConnectionReadAhead(protocol), Is.Not.Null);
        }

        // SetRdGateway does not read these gateways from 1Password, so op would run for nothing
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.None)]
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.VaultOpenbao)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(RDGatewayUseConnectionCredentials.Yes, ExternalCredentialProvider.None)]
        [TestCase(RDGatewayUseConnectionCredentials.SmartCard, ExternalCredentialProvider.None)]
        [TestCase(RDGatewayUseConnectionCredentials.AccessToken, ExternalCredentialProvider.OnePassword)]
        public async Task RdpDoesNotReadAGatewayThatDoesNotUse1Password(
            RDGatewayUseConnectionCredentials gatewayMode, ExternalCredentialProvider gatewayProvider)
        {
            var protocol = new RdpProtocol();

            await protocol.ReadExternalCredentialsAsync(
                RdpConnection(ExternalCredentialProvider.OnePassword, gatewayMode, gatewayProvider));

            Assert.That(GatewayReadAhead(protocol), Is.Null);
            Assert.That(ConnectionReadAhead(protocol), Is.Not.Null);
        }

        [Test]
        public async Task RdpDoesNotReadTheGatewayWhenItIsNotUsed()
        {
            var protocol = new RdpProtocol();
            ConnectionInfo info = RdpConnection(ExternalCredentialProvider.None,
                RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.OnePassword);
            info.RDGatewayUsageMethod = RDGatewayUsageMethod.Never;

            await protocol.ReadExternalCredentialsAsync(info);

            Assert.That(GatewayReadAhead(protocol), Is.Null);
        }

        // As SetCredentials and SetRdGateway, which do not set any credentials then
        [Test]
        public async Task RdpReadsNothingWhenConnectingWithoutCredentials()
        {
            var protocol = new RdpProtocol { Force = ConnectionInfo.Force.NoCredentials };

            await protocol.ReadExternalCredentialsAsync(RdpConnection(ExternalCredentialProvider.OnePassword,
                RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.OnePassword));

            Assert.That(GatewayReadAhead(protocol), Is.Null);
            Assert.That(ConnectionReadAhead(protocol), Is.Null);
        }
    }
}
