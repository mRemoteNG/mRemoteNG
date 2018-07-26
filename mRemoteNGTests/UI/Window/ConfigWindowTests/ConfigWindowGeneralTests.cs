using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Window;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    public class ConfigWindowGeneralTests
    {
        private ConfigWindow _configWindow;

        [SetUp]
        public void Setup()
        {
            _configWindow = new ConfigWindow
            {
                PropertiesVisible = true
            };
        }

        [TestCaseSource(nameof(ConnectionInfoGeneralTestCases))]
        public void PropertyGridShowCorrectPropertiesForConnectionInfo(ConnectionInfo connectionInfo, IEnumerable<string> expectedVisibleProperties)
        {
            _configWindow.SelectedTreeNode = connectionInfo;
            Assert.That(_configWindow.VisibleObjectProperties, Is.EquivalentTo(expectedVisibleProperties));
        }

        [Test]
        public void PropertyGridShowCorrectPropertiesForRootConnectionInfo()
        {
            var expectedVisibleProperties = new[]
            {
                nameof(RootNodeInfo.Name),
                nameof(RootNodeInfo.Password),
            };

            _configWindow.SelectedTreeNode = new RootNodeInfo(RootNodeType.Connection);
            Assert.That(_configWindow.VisibleObjectProperties, Is.EquivalentTo(expectedVisibleProperties));
        }

        [Test]
        public void PropertyGridShowCorrectPropertiesForRootPuttyInfo()
        {
            var expectedVisibleProperties = new[]
            {
                nameof(RootNodeInfo.Name),
            };

            _configWindow.SelectedTreeNode = new RootPuttySessionsNodeInfo();
            Assert.That(_configWindow.VisibleObjectProperties, Is.EquivalentTo(expectedVisibleProperties));
        }

        private static IEnumerable<TestCaseData> ConnectionInfoGeneralTestCases()
        {
            var protocolTypes = typeof(ProtocolType).GetEnumValues().OfType<ProtocolType>();
            var testCases = new List<TestCaseData>();

            foreach (var protocol in protocolTypes)
            {
                var expectedPropertyListConnection = BuildExpectedConnectionInfoPropertyList(protocol, false);
                var connectionInfo = ConstructConnectionInfo(protocol, false);
                var testCaseConnection = new TestCaseData(connectionInfo, expectedPropertyListConnection)
                    .SetName(protocol + ", ConnectionInfo");
                testCases.Add(testCaseConnection);

                var expectedPropertyListContainer = BuildExpectedConnectionInfoPropertyList(protocol, true);
                var containerInfo = ConstructConnectionInfo(protocol, true);
                var testCaseContainer = new TestCaseData(containerInfo, expectedPropertyListContainer)
                    .SetName(protocol + ", ContainerInfo");
                testCases.Add(testCaseContainer);
            }

            return testCases;
        }

        internal static ConnectionInfo ConstructConnectionInfo(ProtocolType protocol, bool isContainer)
        {
            // build connection info. set certain connection properties so
            // that toggled properties are hidden in the property grid. We
            // will test those separately in the special protocol tests.
            var node = isContainer 
                ? new ContainerInfo()
                : new ConnectionInfo();

            node.Protocol = protocol;
            node.Resolution = RdpProtocol.RDPResolutions.Res800x600;
            node.RDGatewayUsageMethod = RdpProtocol.RDGatewayUsageMethod.Never;
            node.RDGatewayUseConnectionCredentials = RdpProtocol.RDGatewayUseConnectionCredentials.Yes;
            node.RedirectSound = RdpProtocol.RDPSounds.DoNotPlay;
            node.VNCAuthMode = ProtocolVNC.AuthMode.AuthVNC;
            node.VNCProxyType = ProtocolVNC.ProxyType.ProxyNone;
            node.Inheritance.TurnOffInheritanceCompletely();

            return node;
        }

        internal static List<string> BuildExpectedConnectionInfoPropertyList(ProtocolType protocol, bool isContainer)
        {
            var expectedProperties = new List<string>
            {
                nameof(ConnectionInfo.Name),
                nameof(ConnectionInfo.Description),
                nameof(ConnectionInfo.Icon),
                nameof(ConnectionInfo.Panel),
                nameof(ConnectionInfo.Protocol),
                nameof(ConnectionInfo.PreExtApp),
                nameof(ConnectionInfo.PostExtApp),
                nameof(ConnectionInfo.MacAddress),
                nameof(ConnectionInfo.UserField),
            };

            if (!isContainer)
            {
                expectedProperties.AddRange(new []
                {
                    nameof(ConnectionInfo.Hostname),
                });
            }

            switch (protocol)
            {
                case ProtocolType.RDP:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Domain),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.UseConsoleSession),
                        nameof(ConnectionInfo.RDPAuthenticationLevel),
                        nameof(ConnectionInfo.RDPMinutesToIdleTimeout),
                        nameof(ConnectionInfo.LoadBalanceInfo),
                        nameof(ConnectionInfo.UseCredSsp),
                        nameof(ConnectionInfo.RDGatewayUsageMethod),
                        nameof(ConnectionInfo.Resolution),
                        nameof(ConnectionInfo.Colors),
                        nameof(ConnectionInfo.CacheBitmaps),
                        nameof(ConnectionInfo.DisplayWallpaper),
                        nameof(ConnectionInfo.DisplayThemes),
                        nameof(ConnectionInfo.EnableFontSmoothing),
                        nameof(ConnectionInfo.EnableDesktopComposition),
                        nameof(ConnectionInfo.RedirectKeys),
                        nameof(ConnectionInfo.RedirectDiskDrives),
                        nameof(ConnectionInfo.RedirectPrinters),
                        nameof(ConnectionInfo.RedirectPorts),
                        nameof(ConnectionInfo.RedirectSmartCards),
                        nameof(ConnectionInfo.RedirectSound),
                    });
                    break;
                case ProtocolType.VNC:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.VNCSmartSizeMode),
                        nameof(ConnectionInfo.VNCViewOnly),
                    });
                    break;
                case ProtocolType.SSH1:
                case ProtocolType.SSH2:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.PuttySession)
                    });
                    break;
                case ProtocolType.Telnet:
                case ProtocolType.Rlogin:
                case ProtocolType.RAW:
                    expectedProperties.AddRange(new[]
                    {
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.PuttySession),
                    });
                    break;
                case ProtocolType.HTTP:
                case ProtocolType.HTTPS:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.RenderingEngine),
                    });
                    break;
                case ProtocolType.ICA:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Domain),
                        nameof(ConnectionInfo.ICAEncryptionStrength),
                        nameof(ConnectionInfo.Resolution),
                        nameof(ConnectionInfo.Colors),
                        nameof(ConnectionInfo.CacheBitmaps),
                    });
                    break;
                case ProtocolType.IntApp:
                    expectedProperties.AddRange(new[]
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Domain),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.ExtApp),
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(protocol), protocol, null);
            }

            return expectedProperties;
        }
    }
}
