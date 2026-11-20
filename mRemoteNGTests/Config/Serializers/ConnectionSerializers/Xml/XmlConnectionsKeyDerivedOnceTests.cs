using System;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

/// <summary>
/// From format 3.0 on, a connections file protected with a password has one salt on its root
/// element, and the key of all its values is derived once from it, at the iteration count current
/// guidance asks for (CVE-2026-87868). Files in the older formats derive a key for each value.
/// </summary>
public class XmlConnectionsKeyDerivedOnceTests
{
    private const string ConnectionPassword = "connection password for the test";

    private ConnectionTreeModel _model;
    private RootNodeInfo _root;
    private bool _useDpapiProtection;

    [SetUp]
    public void Setup()
    {
        _useDpapiProtection = mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection;
        mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection = false;

        _model = new ConnectionTreeModel();
        _root = new RootNodeInfo(RootNodeType.Connection);
        _root.AddChild(new ConnectionInfo { Name = "con1", Hostname = "server01", Password = ConnectionPassword });
        _model.AddRootNode(_root);
    }

    [TearDown]
    public void TearDown()
    {
        mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection = _useDpapiProtection;
    }

    private string SaveLikeTheApp(bool useFullEncryption = false)
    {
        ICryptographyProvider cryptographyProvider = new ConnectionsFileCryptoProviderFactory().Build();
        return new XmlConnectionSerializerFactory().Build(cryptographyProvider, _model, useFullEncryption: useFullEncryption).Serialize(_root);
    }

    private static ConnectionInfo LoadConnection(string xml, string rootPassword = null)
    {
        var deserializer = new XmlConnectionsDeserializer(() =>
            rootPassword == null ? new Optional<SecureString>() : new Optional<SecureString>(rootPassword.ConvertToSecureString()));
        ConnectionTreeModel loaded = deserializer.Deserialize(xml);

        Assert.That(loaded, Is.Not.Null);
        return loaded.GetRecursiveChildList().Single(c => c.Name == "con1");
    }

    [Test]
    public void AppSavesInFormat30WithTheSaltOfTheFile()
    {
        var root = SecureXmlHelper.LoadXmlFromString(SaveLikeTheApp()).DocumentElement;

        Assert.Multiple(() =>
        {
            Assert.That(double.Parse(root.GetAttribute("ConfVersion"), CultureInfo.InvariantCulture), Is.EqualTo(3.0));
            Assert.That(Convert.FromBase64String(root.GetAttribute("KdfSalt")), Has.Length.EqualTo(DerivedKeyCryptographyProvider.SaltSizeInBytes));
            Assert.That(int.Parse(root.GetAttribute("KdfIterations"), CultureInfo.InvariantCulture), Is.EqualTo(DerivedKeyCryptographyProvider.DefaultIterations));
        });
    }

    // The iteration count in the settings is the one for a key derived for each value, which has
    // to stay low enough for a file with many passwords to load.
    [Test]
    public void AppDoesNotUseTheIterationCountOfTheSettingsForTheFile()
    {
        int iterations = mRemoteNG.Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations;
        mRemoteNG.Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations = 5000;
        try
        {
            var root = SecureXmlHelper.LoadXmlFromString(SaveLikeTheApp()).DocumentElement;

            Assert.That(int.Parse(root.GetAttribute("KdfIterations"), CultureInfo.InvariantCulture), Is.EqualTo(DerivedKeyCryptographyProvider.DefaultIterations));
        }
        finally
        {
            mRemoteNG.Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations = iterations;
        }
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void PasswordsAreLoadedBack()
    {
        ConnectionInfo loaded = LoadConnection(SaveLikeTheApp());

        Assert.Multiple(() =>
        {
            Assert.That(loaded.Hostname, Is.EqualTo("server01"));
            Assert.That(loaded.Password, Is.EqualTo(ConnectionPassword));
        });
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void PasswordsAreLoadedBackFromAFileWithItsOwnPassword()
    {
        _root.PasswordString = "the file password";

        ConnectionInfo loaded = LoadConnection(SaveLikeTheApp(), "the file password");

        Assert.That(loaded.Password, Is.EqualTo(ConnectionPassword));
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void FileWithItsOwnPasswordDoesNotLoadWithoutIt()
    {
        _root.PasswordString = "the file password";
        string xml = SaveLikeTheApp();

        var deserializer = new XmlConnectionsDeserializer(() => new Optional<SecureString>());

        Assert.That(deserializer.Deserialize(xml), Is.Null);
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void CompletelyEncryptedFileIsLoadedBack()
    {
        _root.PasswordString = "the file password";

        ConnectionInfo loaded = LoadConnection(SaveLikeTheApp(useFullEncryption: true), "the file password");

        Assert.Multiple(() =>
        {
            Assert.That(loaded.Hostname, Is.EqualTo("server01"));
            Assert.That(loaded.Password, Is.EqualTo(ConnectionPassword));
        });
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void FileInFormat29IsStillLoaded()
    {
        var cryptographyProvider = new AeadCryptographyProvider();
        var nodeSerializer = new XmlConnectionNodeSerializer29(cryptographyProvider, _root.PasswordString.ConvertToSecureString(), new SaveFilter());
        string xml = new XmlConnectionsSerializer(cryptographyProvider, nodeSerializer).Serialize(_root);
        Assert.That(xml, Does.Contain("ConfVersion=\"2.9\"").And.Not.Contain("KdfSalt"));

        ConnectionInfo loaded = LoadConnection(xml);

        Assert.That(loaded.Password, Is.EqualTo(ConnectionPassword));
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void SaltOfAnotherFileDoesNotOpenTheValues()
    {
        string xml = SaveLikeTheApp();
        string salt = SecureXmlHelper.LoadXmlFromString(xml).DocumentElement.GetAttribute("KdfSalt");
        string otherSalt = Convert.ToBase64String(DerivedKeyCryptographyProvider.NewSalt());

        var deserializer = new XmlConnectionsDeserializer(() => new Optional<SecureString>());

        Assert.That(deserializer.Deserialize(xml.Replace(salt, otherSalt)), Is.Null);
    }
}
