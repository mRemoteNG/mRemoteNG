using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    public class ConfigWindowRdpSpecialTests : ConfigWindowSpecialTestsBase
    {
        protected override ProtocolType Protocol => ProtocolType.RDP;

        [Test]
        public void PropertyShownWhenActive_RdpMinutesToIdleTimeout()
        {
            ConnectionInfo.RDPMinutesToIdleTimeout = 1;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.RDPAlertIdleTimeout));

            RunVerification();
        }

        [TestCase(RdpProtocol.RDGatewayUsageMethod.Always)]
        [TestCase(RdpProtocol.RDGatewayUsageMethod.Detect)]
        public void RdGatewayPropertiesShown_WhenRdGatewayUsageMethodIsNotNever(RdpProtocol.RDGatewayUsageMethod gatewayUsageMethod)
        {
            ConnectionInfo.RDGatewayUsageMethod = gatewayUsageMethod;
            ConnectionInfo.RDGatewayUseConnectionCredentials = RdpProtocol.RDGatewayUseConnectionCredentials.Yes;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayHostname),
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayUseConnectionCredentials)
            });

            RunVerification();
        }

        [TestCase(RdpProtocol.RDGatewayUseConnectionCredentials.No)]
        [TestCase(RdpProtocol.RDGatewayUseConnectionCredentials.SmartCard)]
        public void RdGatewayPropertiesShown_WhenRDGatewayUseConnectionCredentialsIsNotYes(RdpProtocol.RDGatewayUseConnectionCredentials useConnectionCredentials)
        {
            ConnectionInfo.RDGatewayUsageMethod = RdpProtocol.RDGatewayUsageMethod.Always;
            ConnectionInfo.RDGatewayUseConnectionCredentials = useConnectionCredentials;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayHostname),
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayUsername),
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayPassword),
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayDomain),
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayUseConnectionCredentials)
            });

            RunVerification();
        }

        [Test]
        public void SoundQualityPropertyShown_WhenRdpSoundsSetToBringToThisComputer()
        {
            ConnectionInfo.RedirectSound = RdpProtocol.RDPSounds.BringToThisComputer;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.SoundQuality));

            RunVerification();
        }

        [TestCase(RdpProtocol.RDPResolutions.FitToWindow)]
        [TestCase(RdpProtocol.RDPResolutions.Fullscreen)]
        public void AutomaticResizePropertyShown_WhenResolutionIsDynamic(RdpProtocol.RDPResolutions resolution)
        {
            ConnectionInfo.Resolution = resolution;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.AutomaticResize));

            RunVerification();
        }
    }
}
