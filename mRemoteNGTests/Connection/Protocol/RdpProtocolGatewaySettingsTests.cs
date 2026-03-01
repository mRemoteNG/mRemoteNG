using System.Reflection;
using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpProtocolGatewaySettingsTests
    {
        private static bool InvokeShouldApplyExplicitGatewaySettings(RDGatewayUsageMethod usageMethod, string gatewayHostname)
        {
            MethodInfo? method = typeof(RdpProtocol).GetMethod(
                "ShouldApplyExplicitGatewaySettings",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.That(method, Is.Not.Null);
            return (bool)method!.Invoke(null, new object[] { usageMethod, gatewayHostname })!;
        }

        [TestCase(RDGatewayUsageMethod.Never, "", false)]
        [TestCase(RDGatewayUsageMethod.Always, "", true)]
        [TestCase(RDGatewayUsageMethod.Always, "gateway.example.com", true)]
        [TestCase(RDGatewayUsageMethod.Detect, "", false)]
        [TestCase(RDGatewayUsageMethod.Detect, "   ", false)]
        [TestCase(RDGatewayUsageMethod.Detect, "gateway.example.com", true)]
        [TestCase(RDGatewayUsageMethod.Detect, "10.0.0.7", true)]
        [TestCase(RDGatewayUsageMethod.Detect, "bad host", false)]
        public void ShouldApplyExplicitGatewaySettings_ReturnsExpectedValue(
            RDGatewayUsageMethod usageMethod,
            string gatewayHostname,
            bool expected)
        {
            bool result = InvokeShouldApplyExplicitGatewaySettings(usageMethod, gatewayHostname);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
