using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers;

public class RemoteDesktopConnectionDeserializerTests
{
    // .rdp file schema: https://technet.microsoft.com/en-us/library/ff393699(v=ws.10).aspx
    private RemoteDesktopConnectionDeserializer _deserializer;
    private ConnectionTreeModel _connectionTreeModel;
    private const string ExpectedHostname = "testhostname.domain.com";
    private const string ExpectedUserName = "myusernamehere";
    private const string ExpectedDomain = "myspecialdomain";
    private const string ExpectedGatewayHostname = "gatewayhostname.domain.com";
    private const string ExpectedLoadBalanceInfo = "tsv://MS Terminal Services Plugin.1.RDS-NAME";
    private const int ExpectedPort = 9933;
    private const RDPColors ExpectedColors = RDPColors.Colors24Bit;
    private const bool ExpectedBitmapCaching = true;
    // A windowed file (screen mode id:i:1) is imported as SmartSize since the resizing rework.
    private const RDPResolutions ExpectedResolutionMode = RDPResolutions.SmartSize;
    private const bool ExpectedWallpaperDisplay = true;
    private const bool ExpectedThemesDisplay = true;
    private const bool ExpectedFontSmoothing = true;
    private const bool ExpectedDesktopComposition = true;
    private const bool ExpectedSmartcardRedirection = true;
    private const RDPDiskDrives ExpectedDriveRedirection = RDPDiskDrives.Local;
    private const bool ExpectedPortRedirection = true;
    private const bool ExpectedPrinterRedirection = true;
    private const RDPSounds ExpectedSoundRedirection = RDPSounds.BringToThisComputer;
    private const string ExpectedStartProgram = "alternate shell";

    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        var connectionFileContents = Resources.test_remotedesktopconnection_rdp;
        _deserializer = new RemoteDesktopConnectionDeserializer();
        _connectionTreeModel = _deserializer.Deserialize(connectionFileContents);
    }

    [Test]
    public void ConnectionTreeModelHasARootNode()
    {
        var numberOfRootNodes = _connectionTreeModel.RootNodes.Count;
        Assert.That(numberOfRootNodes, Is.GreaterThan(0));
    }

    [Test]
    public void RootNodeHasConnectionInfo()
    {
        var rootNodeContents = _connectionTreeModel.RootNodes.First().Children.OfType<ConnectionInfo>();
        Assert.That(rootNodeContents, Is.Not.Empty);
    }

    [Test]
    public void HostnameImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.Hostname, Is.EqualTo(ExpectedHostname));
    }

    [Test]
    public void PortImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.Port, Is.EqualTo(ExpectedPort));
    }

    [Test]
    public void UsernameImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.Username, Is.EqualTo(ExpectedUserName));
    }

    [Test]
    public void DomainImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.Domain, Is.EqualTo(ExpectedDomain));
    }

    [Test]
    public void RdpColorsImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.Colors, Is.EqualTo(ExpectedColors));
    }

    [Test]
    public void BitmapCachingImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.CacheBitmaps, Is.EqualTo(ExpectedBitmapCaching));
    }

    [Test]
    public void ResolutionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.Resolution, Is.EqualTo(ExpectedResolutionMode));
    }

    [Test]
    public void DisplayWallpaperImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.DisplayWallpaper, Is.EqualTo(ExpectedWallpaperDisplay));
    }

    [Test]
    public void DisplayThemesImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.DisplayThemes, Is.EqualTo(ExpectedThemesDisplay));
    }

    [Test]
    public void FontSmoothingImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.EnableFontSmoothing, Is.EqualTo(ExpectedFontSmoothing));
    }

    [Test]
    public void DesktopCompositionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.EnableDesktopComposition, Is.EqualTo(ExpectedDesktopComposition));
    }

    [Test]
    public void SmartcardRedirectionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.RedirectSmartCards, Is.EqualTo(ExpectedSmartcardRedirection));
    }

    [Test]
    public void DriveRedirectionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.RedirectDiskDrives, Is.EqualTo(ExpectedDriveRedirection));
    }

    [Test]
    public void PortRedirectionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.RedirectPorts, Is.EqualTo(ExpectedPortRedirection));
    }

    [Test]
    public void PrinterRedirectionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.RedirectPrinters, Is.EqualTo(ExpectedPrinterRedirection));
    }

    [Test]
    public void SoundRedirectionImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.RedirectSound, Is.EqualTo(ExpectedSoundRedirection));
    }

    [Test]
    public void LoadBalanceInfoImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.LoadBalanceInfo, Is.EqualTo(ExpectedLoadBalanceInfo));
    }

    [Test]
    public void StartProgramImportedCorrectly()
    {
        var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        Assert.That(connectionInfo.RDPStartProgram, Is.EqualTo(ExpectedStartProgram));
    }

    //[Test]
    //public void GatewayHostnameImportedCorrectly()
    //{
    //    var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
    //    Assert.That(connectionInfo.RDGatewayHostname, Is.EqualTo(_expectedGatewayHostname));
    //}

    private static ConnectionInfo Import(params string[] lines)
    {
        var model = new RemoteDesktopConnectionDeserializer().Deserialize(string.Join(Environment.NewLine, lines));
        return model.RootNodes.First().Children.OfType<ConnectionInfo>().First();
    }

    [Test]
    public void AdministrativeSessionImportedAsConsoleSession()
    {
        var connectionInfo = Import("full address:s:host", "administrative session:i:1");
        Assert.That(connectionInfo.UseConsoleSession, Is.True);
    }

    [TestCase("0", AuthenticationLevel.NoAuth)]
    [TestCase("1", AuthenticationLevel.AuthRequired)]
    [TestCase("2", AuthenticationLevel.WarnOnFailedAuth)]
    public void AuthenticationLevelImported(string value, AuthenticationLevel expected)
    {
        var connectionInfo = Import("full address:s:host", $"authentication level:i:{value}");
        Assert.That(connectionInfo.RDPAuthenticationLevel, Is.EqualTo(expected));
    }

    [TestCase("0", false)]
    [TestCase("1", true)]
    public void CredSspSupportImported(string value, bool expected)
    {
        var connectionInfo = Import("full address:s:host", $"enablecredsspsupport:i:{value}");
        Assert.That(connectionInfo.UseCredSsp, Is.EqualTo(expected));
    }

    [Test]
    public void GatewayUsernameAndDomainImported()
    {
        var connectionInfo = Import("full address:s:host", "gatewayusername:s:gwuser", "gatewaydomain:s:GWDOMAIN");
        Assert.That(connectionInfo.RDGatewayUsername, Is.EqualTo("gwuser"));
        Assert.That(connectionInfo.RDGatewayDomain, Is.EqualTo("GWDOMAIN"));
    }

    [Test]
    public void GatewayBypassForLocalAddressesImportedAsNever()
    {
        var connectionInfo = Import("full address:s:host", "gatewayusagemethod:i:1", "gatewayusagemethod:i:4");
        Assert.That(connectionInfo.RDGatewayUsageMethod, Is.EqualTo(RDGatewayUsageMethod.Never));
    }

    [TestCase("gatewaycredentialssource:i:4", "promptcredentialonce:i:1")]
    [TestCase("promptcredentialonce:i:1", "gatewaycredentialssource:i:4")]
    public void PromptCredentialOnceMakesGatewayUseConnectionCredentials(string first, string second)
    {
        var connectionInfo = Import("full address:s:host", "gatewayhostname:s:gw", first, second);
        Assert.That(connectionInfo.RDGatewayUseConnectionCredentials, Is.EqualTo(RDGatewayUseConnectionCredentials.Yes));
    }

    [Test]
    public void GatewayCredentialsStayManualWithoutPromptCredentialOnce()
    {
        var connectionInfo = Import("full address:s:host", "gatewayhostname:s:gw", "gatewaycredentialssource:i:4", "promptcredentialonce:i:0");
        Assert.That(connectionInfo.RDGatewayUseConnectionCredentials, Is.EqualTo(RDGatewayUseConnectionCredentials.No));
    }

    [TestCase("fe80::1")]
    [TestCase("2001:db8::10")]
    public void BareIpv6AddressImportedAsHostname(string address)
    {
        var connectionInfo = Import($"full address:s:{address}");
        Assert.That(connectionInfo.Hostname, Is.EqualTo(address));
    }

    [Test]
    public void BracketedIpv6AddressKeepsItsPort()
    {
        var connectionInfo = Import("full address:s:[fe80::1]:3391");
        Assert.That(connectionInfo.Port, Is.EqualTo(3391));
    }
}