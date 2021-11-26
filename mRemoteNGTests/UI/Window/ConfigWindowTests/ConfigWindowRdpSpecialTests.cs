using System.Threading;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    [Apartment(ApartmentState.STA)]
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

        [TestCase(RDGatewayUsageMethod.Always)]
        [TestCase(RDGatewayUsageMethod.Detect)]
        public void RdGatewayPropertiesShown_WhenRdGatewayUsageMethodIsNotNever(RDGatewayUsageMethod gatewayUsageMethod)
        {
            ConnectionInfo.RDGatewayUsageMethod = gatewayUsageMethod;
            ConnectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.Yes;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayHostname),
                nameof(mRemoteNG.Connection.ConnectionInfo.RDGatewayUseConnectionCredentials)
            });

            RunVerification();
        }

        [TestCase(RDGatewayUseConnectionCredentials.No)]
        [TestCase(RDGatewayUseConnectionCredentials.SmartCard)]
        public void RdGatewayPropertiesShown_WhenRDGatewayUseConnectionCredentialsIsNotYes(RDGatewayUseConnectionCredentials useConnectionCredentials)
        {
            ConnectionInfo.RDGatewayUsageMethod = RDGatewayUsageMethod.Always;
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
            ConnectionInfo.RedirectSound = RDPSounds.BringToThisComputer;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.SoundQuality));

            RunVerification();
        }

        [TestCase(RDPResolutions.FitToWindow)]
        [TestCase(RDPResolutions.Fullscreen)]
        public void AutomaticResizePropertyShown_WhenResolutionIsDynamic(RDPResolutions resolution)
        {
            ConnectionInfo.Resolution = resolution;
            ExpectedPropertyList.Add(nameof(mRemoteNG.Connection.ConnectionInfo.AutomaticResize));

            RunVerification();
        }
    }
}
