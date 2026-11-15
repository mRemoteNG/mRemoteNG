using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpGatewayCredentialProviderTests
    {
        private static ConnectionInfo Gateway(
            RDGatewayUseConnectionCredentials mode,
            ExternalCredentialProvider connectionProvider,
            ExternalCredentialProvider gatewayProvider) => new()
        {
            RDGatewayUseConnectionCredentials = mode,
            ExternalCredentialProvider = connectionProvider,
            RDGatewayExternalCredentialProvider = gatewayProvider
        };

        // The gateway's credentials are typed in the gateway fields, and the connection takes its own
        // from an external provider.
        [TestCase(ExternalCredentialProvider.OnePassword)]
        [TestCase(ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(ExternalCredentialProvider.DelineaSecretServer)]
        [TestCase(ExternalCredentialProvider.VaultOpenbao)]
        public void TypedGatewayCredentialsAreNotReplacedFromTheConnectionsProvider(ExternalCredentialProvider connectionProvider)
        {
            ConnectionInfo info = Gateway(RDGatewayUseConnectionCredentials.No, connectionProvider, ExternalCredentialProvider.None);

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(ExternalCredentialProvider.None));
        }

        // The gateway takes its credentials from an external provider of its own and the connection does not.
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.OnePassword)]
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.DelineaSecretServer)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.OnePassword)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.DelineaSecretServer)]
        public void GatewayProviderIsReadWhenTheConnectionHasNone(
            RDGatewayUseConnectionCredentials mode,
            ExternalCredentialProvider gatewayProvider)
        {
            ConnectionInfo info = Gateway(mode, ExternalCredentialProvider.None, gatewayProvider);

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(gatewayProvider));
        }

        [TestCase(RDGatewayUseConnectionCredentials.No)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider)]
        public void GatewayProviderWinsOverTheConnectionsProvider(RDGatewayUseConnectionCredentials mode)
        {
            ConnectionInfo info = Gateway(mode, ExternalCredentialProvider.OnePassword, ExternalCredentialProvider.ClickstudiosPasswordState);

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(ExternalCredentialProvider.ClickstudiosPasswordState));
        }

        // In the "external credential provider" mode the typed gateway fields are hidden, so a gateway left
        // without a provider of its own uses the connection's.
        [TestCase(ExternalCredentialProvider.OnePassword)]
        [TestCase(ExternalCredentialProvider.ClickstudiosPasswordState)]
        [TestCase(ExternalCredentialProvider.DelineaSecretServer)]
        [TestCase(ExternalCredentialProvider.VaultOpenbao)]
        [TestCase(ExternalCredentialProvider.None)]
        public void ExternalModeWithoutAGatewayProviderUsesTheConnectionsProvider(ExternalCredentialProvider connectionProvider)
        {
            ConnectionInfo info = Gateway(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, connectionProvider, ExternalCredentialProvider.None);

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(connectionProvider));
        }

        // OpenBao reads with the connection's secret engine, mount and role.
        [TestCase(RDGatewayUseConnectionCredentials.No, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.VaultOpenbao)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.VaultOpenbao)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.None, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.None)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.OnePassword, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.None)]
        [TestCase(RDGatewayUseConnectionCredentials.ExternalCredentialProvider, ExternalCredentialProvider.VaultOpenbao, ExternalCredentialProvider.OnePassword, ExternalCredentialProvider.OnePassword)]
        public void OpenBaoIsReadOnlyWhenTheConnectionUsesIt(
            RDGatewayUseConnectionCredentials mode,
            ExternalCredentialProvider connectionProvider,
            ExternalCredentialProvider gatewayProvider,
            ExternalCredentialProvider expected)
        {
            ConnectionInfo info = Gateway(mode, connectionProvider, gatewayProvider);

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(expected));
        }

        // These modes do not read the gateway credentials from a provider, whatever is left in its hidden field.
        [TestCase(RDGatewayUseConnectionCredentials.Yes)]
        [TestCase(RDGatewayUseConnectionCredentials.SmartCard)]
        [TestCase(RDGatewayUseConnectionCredentials.AccessToken)]
        public void ModesWithoutGatewayCredentialsReadNoProvider(RDGatewayUseConnectionCredentials mode)
        {
            ConnectionInfo info = Gateway(mode, ExternalCredentialProvider.OnePassword, ExternalCredentialProvider.ClickstudiosPasswordState);

            Assert.That(RdpProtocol.GatewayCredentialProvider(info), Is.EqualTo(ExternalCredentialProvider.None));
        }
    }
}
