using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopManagerDeserializerTests
    {
        private readonly RemoteDesktopManagerDeserializer _sut = new RemoteDesktopManagerDeserializer();
        private readonly Dictionary<ProtocolType, string> _rdmExports = new Dictionary<ProtocolType, string>
        {
            {ProtocolType.RDP, Resources.rdp_rdm_export},
            {ProtocolType.SSH2, Resources.ssh_rdm_export},
            {ProtocolType.VNC, Resources.vnc_rdm_export},
            {ProtocolType.Telnet, Resources.telnet_rdm_export},
            {ProtocolType.HTTPS, Resources.website_rdm_export},
        };

        [TestCaseSource(nameof(RdpPropertiesAndValues))]
        public void CorrectlyImportsRdpProperties(string propertyName, object expectedValue)
        {
            var rootContainer = _sut.Deserialize(_rdmExports[ProtocolType.RDP]);
            var node = rootContainer.RootNodes[0].Children.FirstOrDefault(i => i.Protocol == ProtocolType.RDP);
            Assert.That(node, Is.Not.Null);

            var actualValue = node.GetType().GetProperty(propertyName)?.GetValue(node);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(SshPropertiesAndValues))]
        public void CorrectlyImportsSshProperties(string propertyName, object expectedValue)
        {
            var rootContainer = _sut.Deserialize(_rdmExports[ProtocolType.SSH2]);
            var node = rootContainer.RootNodes[0].Children.FirstOrDefault(i => i.Protocol == ProtocolType.SSH2);
            Assert.That(node, Is.Not.Null);

            var actualValue = node.GetType().GetProperty(propertyName)?.GetValue(node);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(VncPropertiesAndValues))]
        public void CorrectlyImportsVncProperties(string propertyName, object expectedValue)
        {
            var rootContainer = _sut.Deserialize(_rdmExports[ProtocolType.VNC]);
            var node = rootContainer.RootNodes[0].Children.FirstOrDefault(i => i.Protocol == ProtocolType.VNC);
            Assert.That(node, Is.Not.Null);

            var actualValue = node.GetType().GetProperty(propertyName)?.GetValue(node);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(TelnetPropertiesAndValues))]
        public void CorrectlyImportsTelnetProperties(string propertyName, object expectedValue)
        {
            var rootContainer = _sut.Deserialize(_rdmExports[ProtocolType.Telnet]);
            var node = rootContainer.RootNodes[0].Children.FirstOrDefault(i => i.Protocol == ProtocolType.Telnet);
            Assert.That(node, Is.Not.Null);

            var actualValue = node.GetType().GetProperty(propertyName)?.GetValue(node);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(WebsitePropertiesAndValues))]
        public void CorrectlyImportsWebsiteProperties(string propertyName, object expectedValue)
        {
            var rootContainer = _sut.Deserialize(_rdmExports[ProtocolType.HTTPS]);
            var node = rootContainer.RootNodes[0].Children.FirstOrDefault(i => i.Protocol == ProtocolType.HTTPS);
            Assert.That(node, Is.Not.Null);

            var actualValue = node.GetType().GetProperty(propertyName)?.GetValue(node);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        private static IEnumerable<TestCaseData> RdpPropertiesAndValues()
        {
            return new[]
            {
                new TestCaseData(nameof(ConnectionInfo.ConstantID), "1f36e6f0-90ca-4607-b6ec-86227738486d"),
                new TestCaseData(nameof(ConnectionInfo.Name), "rdp connection"),
                new TestCaseData(nameof(ConnectionInfo.Protocol), ProtocolType.RDP),
                new TestCaseData(nameof(ConnectionInfo.Hostname), "my.rdp.host.com"),
                new TestCaseData(nameof(ConnectionInfo.Description), "This is a general description"),
                new TestCaseData(nameof(ConnectionInfo.Username), "user1"),
                new TestCaseData(nameof(ConnectionInfo.Domain), "domain1"),
                //new TestCaseData(nameof(ConnectionInfo.Password), "password1"),
                new TestCaseData(nameof(ConnectionInfo.Resolution), RDPResolutions.FitToWindow),
                new TestCaseData(nameof(ConnectionInfo.AutomaticResize), true),
                new TestCaseData(nameof(ConnectionInfo.CacheBitmaps), false),
                new TestCaseData(nameof(ConnectionInfo.DisplayThemes), false),
                new TestCaseData(nameof(ConnectionInfo.DisplayWallpaper), false),
                new TestCaseData(nameof(ConnectionInfo.EnableDesktopComposition), true),
                new TestCaseData(nameof(ConnectionInfo.EnableFontSmoothing), true),
                new TestCaseData(nameof(ConnectionInfo.RedirectSound), RDPSounds.DoNotPlay),
                new TestCaseData(nameof(ConnectionInfo.RedirectAudioCapture), true),
                new TestCaseData(nameof(ConnectionInfo.RedirectClipboard), false),
                new TestCaseData(nameof(ConnectionInfo.RedirectDiskDrives), false),
                new TestCaseData(nameof(ConnectionInfo.RedirectPrinters), true),
                new TestCaseData(nameof(ConnectionInfo.RedirectPorts), false),
                new TestCaseData(nameof(ConnectionInfo.RedirectSmartCards), true),
                new TestCaseData(nameof(ConnectionInfo.RDPMinutesToIdleTimeout), 17),
                new TestCaseData(nameof(ConnectionInfo.MacAddress), "mac-add-goes-here"),
                new TestCaseData(nameof(ConnectionInfo.UseCredSsp), true),
                new TestCaseData(nameof(ConnectionInfo.Port), 1234),
                new TestCaseData(nameof(ConnectionInfo.RdpVersion), RdpVersion.Rdc7),
                new TestCaseData(nameof(ConnectionInfo.RedirectKeys), true),
                new TestCaseData(nameof(ConnectionInfo.UseConsoleSession), true),
                new TestCaseData(nameof(ConnectionInfo.RDPAuthenticationLevel), AuthenticationLevel.WarnOnFailedAuth),
                new TestCaseData(nameof(ConnectionInfo.RDGatewayUsageMethod), RDGatewayUsageMethod.Always),
                new TestCaseData(nameof(ConnectionInfo.RDGatewayUseConnectionCredentials), RDGatewayUseConnectionCredentials.Yes),
                new TestCaseData(nameof(ConnectionInfo.RDGatewayHostname), "rdhost1"),
                new TestCaseData(nameof(ConnectionInfo.RDGatewayUsername), "rduser1"),
                new TestCaseData(nameof(ConnectionInfo.RDGatewayDomain), "rddomain1"),
                //new TestCaseData(nameof(ConnectionInfo.RDGatewayPassword), "rdpassword1"),
                new TestCaseData(nameof(ConnectionInfo.UseEnhancedMode), true),
                new TestCaseData(nameof(ConnectionInfo.UseVmId), true),
                new TestCaseData(nameof(ConnectionInfo.VmId), "instance-id-here"),
            };
        }

        private static IEnumerable<TestCaseData> SshPropertiesAndValues()
        {
            return new[]
            {
                new TestCaseData(nameof(ConnectionInfo.ConstantID), "44ae261f-0094-48a2-93cb-bc5abcfcc394"),
                new TestCaseData(nameof(ConnectionInfo.Name), "ssh connection"),
                new TestCaseData(nameof(ConnectionInfo.Protocol), ProtocolType.SSH2),
                new TestCaseData(nameof(ConnectionInfo.Hostname), "mysshhost"),
                new TestCaseData(nameof(ConnectionInfo.Description), "This is a linux host description"),
                new TestCaseData(nameof(ConnectionInfo.Username), "linuxuser1"),
                //new TestCaseData(nameof(ConnectionInfo.Password), "linuxpassword1"),
                new TestCaseData(nameof(ConnectionInfo.MacAddress), "some-mac-here"),
                new TestCaseData(nameof(ConnectionInfo.Port), 4321),
            };
        }

        private static IEnumerable<TestCaseData> VncPropertiesAndValues()
        {
            return new[]
            {
                new TestCaseData(nameof(ConnectionInfo.ConstantID), "16ff7dd6-2ac3-4e27-96cf-435b5bfe5a00"),
                new TestCaseData(nameof(ConnectionInfo.Name), "vnc connection"),
                new TestCaseData(nameof(ConnectionInfo.Protocol), ProtocolType.VNC),
                new TestCaseData(nameof(ConnectionInfo.Hostname), "vnchost1"),
                new TestCaseData(nameof(ConnectionInfo.Port), 9987),
                new TestCaseData(nameof(ConnectionInfo.Description), "This is a VNC description"),
                new TestCaseData(nameof(ConnectionInfo.Username), "winuser1"),
                new TestCaseData(nameof(ConnectionInfo.Domain), "windomain1"),
                //new TestCaseData(nameof(ConnectionInfo.Password), "vncpassword1"),
                new TestCaseData(nameof(ConnectionInfo.MacAddress), "some-mac-here"),

                new TestCaseData(nameof(ConnectionInfo.VNCEncoding), ProtocolVNC.Encoding.EncTight),
                new TestCaseData(nameof(ConnectionInfo.VNCAuthMode), ProtocolVNC.AuthMode.AuthWin),
                new TestCaseData(nameof(ConnectionInfo.VNCCompression), ProtocolVNC.Compression.Comp4),
                new TestCaseData(nameof(ConnectionInfo.VNCViewOnly), true),
                new TestCaseData(nameof(ConnectionInfo.VNCProxyIP), "proxyhost"),
                new TestCaseData(nameof(ConnectionInfo.VNCProxyPort), 7777),
            };
        }

        private static IEnumerable<TestCaseData> TelnetPropertiesAndValues()
        {
            return new[]
            {
                new TestCaseData(nameof(ConnectionInfo.ConstantID), "8d77d3ac-b414-4b51-ac10-60304d63cc6f"),
                new TestCaseData(nameof(ConnectionInfo.Name), "telnet connection"),
                new TestCaseData(nameof(ConnectionInfo.Protocol), ProtocolType.Telnet),
                new TestCaseData(nameof(ConnectionInfo.Hostname), "telnethost1"),
                new TestCaseData(nameof(ConnectionInfo.Description), "Telnet description"),
                new TestCaseData(nameof(ConnectionInfo.Username), "user1"),
                //new TestCaseData(nameof(ConnectionInfo.Password), "password1"),
                new TestCaseData(nameof(ConnectionInfo.MacAddress), "some-mac-here"),
                new TestCaseData(nameof(ConnectionInfo.Port), 7648),
            };
        }

        private static IEnumerable<TestCaseData> WebsitePropertiesAndValues()
        {
            return new[]
            {
                new TestCaseData(nameof(ConnectionInfo.ConstantID), "65092747-6870-42c9-b8bc-35ec9fb5b3fb"),
                new TestCaseData(nameof(ConnectionInfo.Name), "website connection"),
                new TestCaseData(nameof(ConnectionInfo.Protocol), ProtocolType.HTTPS),
                new TestCaseData(nameof(ConnectionInfo.Hostname), "www.google.com"),
                new TestCaseData(nameof(ConnectionInfo.Description), "Website description"),
                new TestCaseData(nameof(ConnectionInfo.Username), "user1"),
                new TestCaseData(nameof(ConnectionInfo.Domain), "domain1"),
                //new TestCaseData(nameof(ConnectionInfo.Password), "password1"),
                new TestCaseData(nameof(ConnectionInfo.Port), 8080),
                new TestCaseData(nameof(ConnectionInfo.RenderingEngine), HTTPBase.RenderingEngine.Gecko),
            };
        }
    }
}
