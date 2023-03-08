using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
	public class RemoteDesktopConnectionManager27DeserializerTests
    {
        private string _connectionFileContents;
        private RemoteDesktopConnectionManagerDeserializer _deserializer;
        private const string ExpectedName = "server1_displayname";
        private const string ExpectedHostname = "server1";
        private const string ExpectedDescription = "Comment text here";
        private const string ExpectedUserViaAPI = "123";
        private const string ExpectedUsername = "myusername1";
        private const string ExpectedDomain = "mydomain";
        private const string ExpectedPassword = "passwordHere!";
        private const bool ExpectedUseConsoleSession = true;
        private const int ExpectedPort = 9933;
        private const RDGatewayUsageMethod ExpectedGatewayUsageMethod = RDGatewayUsageMethod.Always;
        private const string ExpectedGatewayHostname = "gatewayserverhost.innerdomain.net";
        private const string ExpectedGatewayUsername = "gatewayusername";
        private const string ExpectedGatewayDomain = "innerdomain";
        private const string ExpectedGatewayPassword = "gatewayPassword123";
        private const RDPResolutions ExpectedRdpResolution = RDPResolutions.FitToWindow;
        private const RDPColors ExpectedRdpColorDepth = RDPColors.Colors24Bit;
        private const RDPSounds ExpectedAudioRedirection = RDPSounds.DoNotPlay;
        private const bool ExpectedKeyRedirection = true;
        private const bool ExpectedSmartcardRedirection = true;
        private const bool ExpectedDriveRedirection = true;
        private const bool ExpectedPortRedirection = true;
        private const bool ExpectedPrinterRedirection = true;
        private const AuthenticationLevel ExpectedAuthLevel = AuthenticationLevel.WarnOnFailedAuth;
        private const string ExpectedStartProgram = "alternate shell";

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _connectionFileContents = Resources.test_rdcman_v2_7_schema3;
            _deserializer = new RemoteDesktopConnectionManagerDeserializer();
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
			var numberOfRootNodes = connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasContents()
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
			var rootNodeContents = connectionTreeModel.RootNodes.First().Children;
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void AllSubRootFoldersImported()
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
			var rootNode = connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var rootNodeContents = importedRdcmanRootNode.Children.Count(node => node.Name == "Group1" || node.Name == "Group2");
            Assert.That(rootNodeContents, Is.EqualTo(2));
        }

        [TestCaseSource(nameof(ExpectedPropertyValues))]
        public void PropertiesWithValuesAreCorrectlyImported(Func<ConnectionInfo, object> propSelector, object expectedValue)
        {
	        var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);

			var connection = connectionTreeModel
				.GetRecursiveChildList()
				.OfType<ContainerInfo>()
				.First(node => node.Name == "Group1")
				.Children
				.First();

            Assert.That(propSelector(connection), Is.EqualTo(expectedValue));
        }

		[TestCaseSource(nameof(NullPropertyValues))]
        public void PropertiesWithoutValuesAreIgnored(Func<ConnectionInfo, object> propSelector)
        {
	        var connectionTreeModel = _deserializer.Deserialize(Resources.test_rdcman_v2_7_schema3_empty_values);

	        var importedConnection = connectionTreeModel
		        .GetRecursiveChildList()
		        .OfType<ContainerInfo>()
		        .First(node => node.Name == "Group1")
		        .Children
		        .First();

			Assert.That(propSelector(importedConnection), Is.EqualTo(propSelector(new ConnectionInfo())));
        }

        [TestCaseSource(nameof(NullPropertyValues))]
        public void NonExistantPropertiesAreIgnored(Func<ConnectionInfo, object> propSelector)
        {
	        var connectionTreeModel = _deserializer.Deserialize(Resources.test_rdcman_v2_7_schema3_null_values);

	        var importedConnection = connectionTreeModel
		        .GetRecursiveChildList()
		        .OfType<ContainerInfo>()
		        .First(node => node.Name == "Group1")
		        .Children
		        .First();

	        Assert.That(propSelector(importedConnection), Is.EqualTo(propSelector(new ConnectionInfo())));
        }

		[Test]
        public void ExceptionThrownOnBadSchemaVersion()
        {
            var badFileContents = Resources.test_rdcman_v2_2_badschemaversion;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnUnsupportedVersion()
        {
            var badFileContents = Resources.test_rdcman_badVersionNumber;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnNoVersion()
        {
            var badFileContents = Resources.test_rdcman_noversion;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        private static IEnumerable<TestCaseData> ExpectedPropertyValues()
        {
	        return new[]
	        {
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.Name), ExpectedName).SetName(nameof(ConnectionInfo.Name)),
                new TestCaseData((Func<ConnectionInfo,object>)(con => con.Hostname), ExpectedHostname).SetName(nameof(ConnectionInfo.Hostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Description), ExpectedDescription).SetName(nameof(ConnectionInfo.Description)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Username), ExpectedUsername).SetName(nameof(ConnectionInfo.Username)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Domain), ExpectedDomain).SetName(nameof(ConnectionInfo.Domain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Protocol), ProtocolType.RDP).SetName(nameof(ConnectionInfo.Protocol)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.UseConsoleSession), ExpectedUseConsoleSession).SetName(nameof(ConnectionInfo.UseConsoleSession)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Port), ExpectedPort).SetName(nameof(ConnectionInfo.Port)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayUsageMethod), ExpectedGatewayUsageMethod).SetName(nameof(ConnectionInfo.RDGatewayUsageMethod)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayHostname), ExpectedGatewayHostname).SetName(nameof(ConnectionInfo.RDGatewayHostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayUsername), ExpectedGatewayUsername).SetName(nameof(ConnectionInfo.RDGatewayUsername)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayDomain), ExpectedGatewayDomain).SetName(nameof(ConnectionInfo.RDGatewayDomain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Resolution), ExpectedRdpResolution).SetName(nameof(ConnectionInfo.Resolution)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Colors), ExpectedRdpColorDepth).SetName(nameof(ConnectionInfo.Colors)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSound), ExpectedAudioRedirection).SetName(nameof(ConnectionInfo.RedirectSound)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectKeys), ExpectedKeyRedirection).SetName(nameof(ConnectionInfo.RedirectKeys)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDPAuthenticationLevel), ExpectedAuthLevel).SetName(nameof(ConnectionInfo.RDPAuthenticationLevel)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSmartCards), ExpectedSmartcardRedirection).SetName(nameof(ConnectionInfo.RedirectSmartCards)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPrinters), ExpectedPrinterRedirection).SetName(nameof(ConnectionInfo.RedirectPrinters)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPorts), ExpectedPortRedirection).SetName(nameof(ConnectionInfo.RedirectPorts)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectDiskDrives), ExpectedDriveRedirection).SetName(nameof(ConnectionInfo.RedirectDiskDrives)),
		        new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDPStartProgram), ExpectedStartProgram).SetName(nameof(ConnectionInfo.RDPStartProgram)),
			};
        }

        private static IEnumerable<TestCaseData> NullPropertyValues()
        {
			return new[]
			{
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Name)).SetName(nameof(ConnectionInfo.Name)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.ExternalAddressProvider)).SetName(nameof(ConnectionInfo.ExternalAddressProvider)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.ExternalCredentialProvider)).SetName(nameof(ConnectionInfo.ExternalCredentialProvider)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.UserViaAPI)).SetName(nameof(ConnectionInfo.UserViaAPI)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Hostname)).SetName(nameof(ConnectionInfo.Hostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Description)).SetName(nameof(ConnectionInfo.Description)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Username)).SetName(nameof(ConnectionInfo.Username)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Domain)).SetName(nameof(ConnectionInfo.Domain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Protocol)).SetName(nameof(ConnectionInfo.Protocol)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.UseConsoleSession)).SetName(nameof(ConnectionInfo.UseConsoleSession)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Port)).SetName(nameof(ConnectionInfo.Port)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayUsageMethod)).SetName(nameof(ConnectionInfo.RDGatewayUsageMethod)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayHostname)).SetName(nameof(ConnectionInfo.RDGatewayHostname)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayUsername)).SetName(nameof(ConnectionInfo.RDGatewayUsername)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayDomain)).SetName(nameof(ConnectionInfo.RDGatewayDomain)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayExternalCredentialProvider)).SetName(nameof(ConnectionInfo.RDGatewayExternalCredentialProvider)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDGatewayUserViaAPI)).SetName(nameof(ConnectionInfo.RDGatewayUserViaAPI)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Resolution)).SetName(nameof(ConnectionInfo.Resolution)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.Colors)).SetName(nameof(ConnectionInfo.Colors)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSound)).SetName(nameof(ConnectionInfo.RedirectSound)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectKeys)).SetName(nameof(ConnectionInfo.RedirectKeys)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RDPAuthenticationLevel)).SetName(nameof(ConnectionInfo.RDPAuthenticationLevel)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectSmartCards)).SetName(nameof(ConnectionInfo.RedirectSmartCards)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPrinters)).SetName(nameof(ConnectionInfo.RedirectPrinters)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectPorts)).SetName(nameof(ConnectionInfo.RedirectPorts)),
				new TestCaseData((Func<ConnectionInfo,object>)(con => con.RedirectDiskDrives)).SetName(nameof(ConnectionInfo.RedirectDiskDrives)),
			};
		}
    }
}