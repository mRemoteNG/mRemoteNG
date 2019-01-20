using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System.Linq;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopConnectionDeserializerTests
    {
        // .rdp file schema: https://technet.microsoft.com/en-us/library/ff393699(v=ws.10).aspx
        private RemoteDesktopConnectionDeserializer _deserializer;
        private SerializationResult _serializationResult;
        private ConnectionInfo _deserializedConnectionInfo;
        private const string ExpectedHostname = "testhostname.domain.com";
        private const string ExpectedUserName = "myusernamehere";
        private const string ExpectedDomain = "myspecialdomain";
        private const string ExpectedGatewayHostname = "gatewayhostname.domain.com";
        private const string ExpectedLoadBalanceInfo = "tsv://MS Terminal Services Plugin.1.RDS-NAME";
        private const int ExpectedPort = 9933;
        private const RdpProtocol.RDPColors ExpectedColors = RdpProtocol.RDPColors.Colors24Bit;
        private const bool ExpectedBitmapCaching = false;
        private const RdpProtocol.RDPResolutions ExpectedResolutionMode = RdpProtocol.RDPResolutions.FitToWindow;
        private const bool ExpectedWallpaperDisplay = true;
        private const bool ExpectedThemesDisplay = true;
        private const bool ExpectedFontSmoothing = true;
        private const bool ExpectedDesktopComposition = true;
        private const bool ExpectedSmartcardRedirection = true;
        private const bool ExpectedDriveRedirection = true;
        private const bool ExpectedPortRedirection = true;
        private const bool ExpectedPrinterRedirection = true;
        private const RdpProtocol.RDPSounds ExpectedSoundRedirection = RdpProtocol.RDPSounds.BringToThisComputer;


        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var connectionFileContents = Resources.test_remotedesktopconnection_rdp;
            _deserializer = new RemoteDesktopConnectionDeserializer();
            _serializationResult = _deserializer.Deserialize(connectionFileContents);
            _deserializedConnectionInfo = _serializationResult.ConnectionRecords.First();
        }

        [Test]
        public void HostnameImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.Hostname, Is.EqualTo(ExpectedHostname));
        }

        [Test]
        public void PortImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.Port, Is.EqualTo(ExpectedPort));
        }

        [Test]
        public void CredentialRecordIdSetCorrectly()
        {
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(_deserializedConnectionInfo.CredentialRecordId.FirstOrDefault(), Is.EqualTo(cred.Id));
        }

        [Test]
        public void UsernameImportedCorrectly()
        {
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(cred.Username, Is.EqualTo(ExpectedUserName));
        }

        [Test]
        public void DomainImportedCorrectly()
        {
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(cred.Domain, Is.EqualTo(ExpectedDomain));
        }

        [Test]
        public void RdpColorsImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.Colors, Is.EqualTo(ExpectedColors));
        }

        [Test]
        public void BitmapCachingImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.CacheBitmaps, Is.EqualTo(ExpectedBitmapCaching));
        }

        [Test]
        public void ResolutionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.Resolution, Is.EqualTo(ExpectedResolutionMode));
        }

        [Test]
        public void DisplayWallpaperImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.DisplayWallpaper, Is.EqualTo(ExpectedWallpaperDisplay));
        }

        [Test]
        public void DisplayThemesImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.DisplayThemes, Is.EqualTo(ExpectedThemesDisplay));
        }

        [Test]
        public void FontSmoothingImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.EnableFontSmoothing, Is.EqualTo(ExpectedFontSmoothing));
        }

        [Test]
        public void DesktopCompositionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.EnableDesktopComposition, Is.EqualTo(ExpectedDesktopComposition));
        }

        [Test]
        public void SmartcardRedirectionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.RedirectSmartCards, Is.EqualTo(ExpectedSmartcardRedirection));
        }

        [Test]
        public void DriveRedirectionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.RedirectDiskDrives, Is.EqualTo(ExpectedDriveRedirection));
        }

        [Test]
        public void PortRedirectionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.RedirectPorts, Is.EqualTo(ExpectedPortRedirection));
        }

        [Test]
        public void PrinterRedirectionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.RedirectPrinters, Is.EqualTo(ExpectedPrinterRedirection));
        }

        [Test]
        public void SoundRedirectionImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.RedirectSound, Is.EqualTo(ExpectedSoundRedirection));
        }

        [Test]
        public void LoadBalanceInfoImportedCorrectly()
        {
            Assert.That(_deserializedConnectionInfo.LoadBalanceInfo, Is.EqualTo(ExpectedLoadBalanceInfo));
        }

        //[Test]
        //public void GatewayHostnameImportedCorrectly()
        //{
        //    var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        //    Assert.That(connectionInfo.RDGatewayHostname, Is.EqualTo(_expectedGatewayHostname));
        //}
    }
}