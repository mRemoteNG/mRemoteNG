using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    [Apartment(ApartmentState.STA)]
    public class ConfigWindowGeneralTests
    {
        private ConfigWindow _configWindow;

        [SetUp]
        public void Setup()
        {
            _configWindow = new ConfigWindow();
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

		[Test]
        public void SwitchFromInheritanceToConnectionPropertiesWhenClickingRootNode()
        {
			// connection with a normal parent container
			var connection = new ConnectionInfo();
			connection.SetParent(new ContainerInfo());

			_configWindow.SelectedTreeNode = connection;
			_configWindow.ShowInheritanceProperties();

	        _configWindow.SelectedTreeNode = new RootNodeInfo(RootNodeType.Connection);
			Assert.That(_configWindow.PropertiesVisible, Is.True, 
				() => "The property mode should switch from inheritance to connection properties when clicking on the root node.");
		}

        [Test]
        public void SwitchFromInheritanceToConnectionPropertiesWhenClickingRootPuttyNode()
        {
	        // connection with a normal parent container
	        var connection = new ConnectionInfo();
	        connection.SetParent(new ContainerInfo());

	        _configWindow.SelectedTreeNode = connection;
	        _configWindow.ShowInheritanceProperties();

	        _configWindow.SelectedTreeNode = new RootPuttySessionsNodeInfo();
	        Assert.That(_configWindow.PropertiesVisible, Is.True,
		        () => "The property mode should switch from inheritance to connection properties when clicking on the root node.");
        }

        [TestCaseSource(nameof(EveryNodeType))]
        public void DefaultConnectionPropertiesCanBeShownRegardlessOfWhichNodeIsSelected(ConnectionInfo selectedObject)
        {
	        _configWindow.SelectedTreeNode = selectedObject;
			Assert.That(_configWindow.CanShowDefaultProperties, Is.True);
        }

        [TestCaseSource(nameof(EveryNodeType))]
        public void DefaultInheritancePropertiesCanBeShownRegardlessOfWhichNodeIsSelected(ConnectionInfo selectedObject)
        {
	        _configWindow.SelectedTreeNode = selectedObject;
	        Assert.That(_configWindow.CanShowDefaultInheritance, Is.True);
        }

        [TestCaseSource(nameof(EveryNodeType))]
		public void ConnectionPropertiesCanAlwaysBeShownUnlessNothingIsSelected(ConnectionInfo selectedObject)
        {
	        _configWindow.SelectedTreeNode = selectedObject;

	        var selectedObjectNotNull = selectedObject != null;
	        Assert.That(_configWindow.CanShowProperties, Is.EqualTo(selectedObjectNotNull));
		}

		[TestCaseSource(nameof(EveryNodeType))]
		public void InheritancePropertiesAreVisibleInCertainCases(ConnectionInfo selectedObject)
		{
			_configWindow.SelectedTreeNode = selectedObject;

			var shouldBeAvailable = selectedObject != null &&
									selectedObject is not RootNodeInfo &&
									selectedObject is not PuttySessionInfo &&
									selectedObject.Parent is not RootNodeInfo;

			Assert.That(_configWindow.CanShowInheritance, Is.EqualTo(shouldBeAvailable));
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

		private static IEnumerable<TestCaseData> EveryNodeType()
		{
			var protocolTypes = typeof(ProtocolType).GetEnumValues().OfType<ProtocolType>().ToList();
			var container = new ContainerInfo();
			var connections = protocolTypes
				.Select(protocolType =>
				{
					var c = new ConnectionInfo {Protocol = protocolType};
					c.SetParent(container);
					return new TestCaseData(c).SetName(protocolType + ", Connection");
				});

			var containers = protocolTypes
				.Select(protocolType =>
				{
					var c = new ContainerInfo { Protocol = protocolType };
					c.SetParent(container);
					return new TestCaseData(c).SetName(protocolType + ", Connection");
				});

			return connections
				.Concat(containers)
				.Concat(new[]
				{
					new TestCaseData(new RootNodeInfo(RootNodeType.Connection)).SetName("RootNode"),
					new TestCaseData(new RootPuttySessionsNodeInfo()).SetName("RootPuttyNode"),
					new TestCaseData(new PuttySessionInfo()).SetName("PuttyNode"),
					new TestCaseData(null).SetName("Null")
				});
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
            node.Resolution = RDPResolutions.Res800x600;
            node.RDGatewayUsageMethod = RDGatewayUsageMethod.Never;
            node.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.Yes;
            node.RedirectSound = RDPSounds.DoNotPlay;
            node.VNCAuthMode = ProtocolVNC.AuthMode.AuthVNC;
            node.VNCProxyType = ProtocolVNC.ProxyType.ProxyNone;
            node.UseVmId = false;
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
                nameof(ConnectionInfo.Favorite),
                nameof(ConnectionInfo.SSHTunnelConnectionName)
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
                        nameof(ConnectionInfo.UseVmId),
                        nameof(ConnectionInfo.UseConsoleSession),
                        nameof(ConnectionInfo.RDPAuthenticationLevel),
                        nameof(ConnectionInfo.RDPMinutesToIdleTimeout),
                        nameof(ConnectionInfo.LoadBalanceInfo),
                        nameof(ConnectionInfo.UseCredSsp),
                        nameof(ConnectionInfo.UseRestrictedAdmin),
                        nameof(ConnectionInfo.UseRCG),
                        nameof(ConnectionInfo.RDGatewayUsageMethod),
                        nameof(ConnectionInfo.Resolution),
                        nameof(ConnectionInfo.Colors),
                        nameof(ConnectionInfo.CacheBitmaps),
                        nameof(ConnectionInfo.DisplayWallpaper),
                        nameof(ConnectionInfo.DisplayThemes),
                        nameof(ConnectionInfo.EnableFontSmoothing),
                        nameof(ConnectionInfo.EnableDesktopComposition),
                        nameof(ConnectionInfo.DisableFullWindowDrag),
                        nameof(ConnectionInfo.DisableMenuAnimations),
                        nameof(ConnectionInfo.DisableCursorShadow),
                        nameof(ConnectionInfo.DisableCursorBlinking),
                        nameof(ConnectionInfo.RedirectKeys),
                        nameof(ConnectionInfo.RedirectDiskDrives),
                        nameof(ConnectionInfo.RedirectDiskDrivesCustom),
                        nameof(ConnectionInfo.RedirectPrinters),
                        nameof(ConnectionInfo.RedirectClipboard),
                        nameof(ConnectionInfo.RedirectPorts),
                        nameof(ConnectionInfo.RedirectSmartCards),
                        nameof(ConnectionInfo.RedirectSound),
                        nameof(ConnectionInfo.RedirectAudioCapture),
			            nameof(ConnectionInfo.RdpVersion),
                        nameof(ConnectionInfo.RDPStartProgram),
                        nameof(ConnectionInfo.RDPStartProgramWorkDir),
                        nameof(ConnectionInfo.RDGatewayExternalCredentialProvider),
                        nameof(ConnectionInfo.RDGatewayUserViaAPI),
                        nameof(ConnectionInfo.ExternalCredentialProvider),
                        nameof(ConnectionInfo.ExternalAddressProvider),
                        nameof(ConnectionInfo.UserViaAPI),
                    });
                    break;
                case ProtocolType.VNC:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.VNCSmartSizeMode),
                        nameof(ConnectionInfo.VNCViewOnly)
                    });
                    break;
                case ProtocolType.SSH1:
                    expectedProperties.AddRange(new[]
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.SSHOptions),
                        nameof(ConnectionInfo.PuttySession),
                        nameof(ConnectionInfo.OpeningCommand),
                        nameof(ConnectionInfo.ExternalCredentialProvider),
                    });
                    break;
                case ProtocolType.SSH2:
                    expectedProperties.AddRange(new []
                    {
                        nameof(ConnectionInfo.Username),
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.SSHOptions),
                        nameof(ConnectionInfo.PuttySession),
                        nameof(ConnectionInfo.OpeningCommand),
                        nameof(ConnectionInfo.ExternalAddressProvider),
                        nameof(ConnectionInfo.ExternalCredentialProvider),
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
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Port),
                        nameof(ConnectionInfo.RenderingEngine),
                    });
                    break;
                case ProtocolType.PowerShell:
                    expectedProperties.AddRange(new[]
                    {
                        nameof(ConnectionInfo.Password),
                        nameof(ConnectionInfo.Domain),
                        nameof(ConnectionInfo.Port),
                    });
                    break;
                case ProtocolType.IntApp:
                    expectedProperties.AddRange(new[]
                    {
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
