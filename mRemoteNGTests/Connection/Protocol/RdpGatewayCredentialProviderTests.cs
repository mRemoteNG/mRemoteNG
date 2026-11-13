using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpGatewayCredentialProviderTests
    {
        // The connection takes its own credentials from an external provider, and the gateway's
        // credentials are typed in the gateway fields.
        [TestCase(ExternalCredentialProvider.OnePassword)]
        [TestCase(ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(ExternalCredentialProvider.DelineaSecretServer)]
        public void GatewayWithoutAProviderDoesNotReadTheConnectionsProvider(ExternalCredentialProvider connectionProvider)
        {
            var info = new ConnectionInfo
            {
                ExternalCredentialProvider = connectionProvider,
                RDGatewayExternalCredentialProvider = ExternalCredentialProvider.None
            };

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(ExternalCredentialProvider.None));
        }

        // The gateway takes its credentials from an external provider and the connection does not.
        [TestCase(ExternalCredentialProvider.OnePassword)]
        [TestCase(ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(ExternalCredentialProvider.DelineaSecretServer)]
        public void GatewayProviderIsReadWhenTheConnectionHasNone(ExternalCredentialProvider gatewayProvider)
        {
            var info = new ConnectionInfo
            {
                ExternalCredentialProvider = ExternalCredentialProvider.None,
                RDGatewayExternalCredentialProvider = gatewayProvider
            };

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(gatewayProvider));
        }

        [Test]
        public void GatewayProviderWinsOverTheConnectionsProvider()
        {
            var info = new ConnectionInfo
            {
                ExternalCredentialProvider = ExternalCredentialProvider.OnePassword,
                RDGatewayExternalCredentialProvider = ExternalCredentialProvider.ClickstudiosPasswordState
            };

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(ExternalCredentialProvider.ClickstudiosPasswordState));
        }

        // OpenBao reads with the connection's secret engine, mount and role.
        [TestCase(ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.None, ExternalCredentialProvider.VaultOpenbao)]
        [TestCase(ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.VaultOpenbao)]
        [TestCase(ExternalCredentialProvider.None, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.None)]
        [TestCase(ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.OnePassword, ExternalCredentialProvider.OnePassword)]
        public void OpenBaoIsReadOnlyWhenTheConnectionUsesIt(
            ExternalCredentialProvider connectionProvider,
            ExternalCredentialProvider gatewayProvider,
            ExternalCredentialProvider expected)
        {
            var info = new ConnectionInfo
            {
                ExternalCredentialProvider = connectionProvider,
                RDGatewayExternalCredentialProvider = gatewayProvider
            };

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(expected));
        }
    }
}
