using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml;

/// <summary>
/// Round-trips a connections file encrypted with <see cref="DpapiCryptographyProvider"/> — the
/// alternative to mRemoteNG's password-derived AES scheme that ties the key to the current Windows
/// user account + machine instead of a (possibly absent) master password. Uses only test data created
/// in-process; never touches a real confCons.xml.
/// </summary>
public class XmlConnectionsDpapiRoundTripTests
{
    private ConnectionTreeModel _model;
    private RootNodeInfo _root;

    [SetUp]
    public void Setup()
    {
        _model = new ConnectionTreeModel();
        _root = new RootNodeInfo(RootNodeType.Connection);
        var connection = new ConnectionInfo
        {
            Name = "dpapi-test-connection",
            Hostname = "test-host.example",
            Username = "test-user",
            Password = "test-password-never-real"
        };
        _root.AddChild(connection);
        _model.AddRootNode(_root);
    }

    private string SaveWithDpapi()
    {
        var serializer = new XmlConnectionSerializerFactory().Build(new DpapiCryptographyProvider(), _model);
        return serializer.Serialize(_root);
    }

    [Test]
    public void SavedFileHasDpapiKeyProtectionMarker()
    {
        string xml = SaveWithDpapi();
        Assert.That(xml, Does.Contain("KeyProtection=\"Dpapi\""));
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void SavedFileCanBeLoadedBackWithoutAnyPassword()
    {
        string savedXml = SaveWithDpapi();

        // authenticationRequestor deliberately always returns "no password" — a DPAPI-protected file
        // must never need to ask for one. If it does, this test fails loudly instead of silently
        // falling back to a password prompt.
        var deserializer = new XmlConnectionsDeserializer(() => new Optional<System.Security.SecureString>());
        ConnectionTreeModel loaded = deserializer.Deserialize(savedXml);

        Assert.That(loaded, Is.Not.Null);
        ConnectionInfo loadedConnection = loaded.GetRecursiveChildList().Single(c => c.Name == "dpapi-test-connection");
        Assert.That(loadedConnection.Hostname, Is.EqualTo("test-host.example"));
        Assert.That(loadedConnection.Username, Is.EqualTo("test-user"));
        Assert.That(loadedConnection.Password, Is.EqualTo("test-password-never-real"));
    }

    [Test]
    public void DpapiProviderRoundTripsPlainStringDirectly()
    {
        // Lower-level check of the provider itself, independent of the XML plumbing above.
        var provider = new DpapiCryptographyProvider();
        string encrypted = provider.Encrypt("some-secret-value", null);

        Assert.That(encrypted, Is.Not.EqualTo("some-secret-value"));
        Assert.That(provider.Decrypt(encrypted, null), Is.EqualTo("some-secret-value"));
    }

    [Test]
    public void DpapiProviderThrowsClearExceptionOnForeignCiphertext()
    {
        // Simulates opening a DPAPI-protected file encrypted under a different Windows account/
        // machine: base64-valid, but not something this account's DPAPI master key can unprotect.
        var provider = new DpapiCryptographyProvider();
        string notActuallyDpapiProtected = System.Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });

        EncryptionException ex = Assert.Throws<EncryptionException>(
            () => provider.Decrypt(notActuallyDpapiProtected, null));
        Assert.That(ex.Message, Does.Contain("different Windows"));
    }

    [Test]
    public void EmptyStringRoundTripsAsEmptyStringWithoutCallingDpapi()
    {
        // Matches every other ICryptographyProvider implementation: an empty password/field must
        // not turn into a DPAPI call (and therefore not into stored ciphertext either).
        var provider = new DpapiCryptographyProvider();
        Assert.That(provider.Encrypt("", null), Is.EqualTo(""));
        Assert.That(provider.Decrypt("", null), Is.EqualTo(""));
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void FileWithForeignDpapiRootProtectedValueFailsTheWholeLoad()
    {
        // Regression test for the PR review finding: a DPAPI-protected connections file whose root
        // Protected value cannot be decrypted here (different Windows account/machine, or simply
        // corrupted) must fail the entire Deserialize() call, not silently continue loading nodes
        // with a tree full of undecryptable passwords that a later save would overwrite as garbage.
        string savedXml = SaveWithDpapi();
        string foreignProtectedValue = System.Convert.ToBase64String(new byte[] { 9, 9, 9, 9, 9, 9, 9, 9 });
        string corruptedXml = Regex.Replace(savedXml, "Protected=\"[^\"]*\"", $"Protected=\"{foreignProtectedValue}\"");
        Assert.That(corruptedXml, Does.Contain(foreignProtectedValue));

        var deserializer = new XmlConnectionsDeserializer(() => new Optional<System.Security.SecureString>());

        Assert.Throws<EncryptionException>(() => deserializer.Deserialize(corruptedXml));
    }

    [TestCase("", false)]
    [TestCase(null, true)]
    [Apartment(ApartmentState.STA)]
    public void FileWithMissingOrEmptyDpapiRootProtectedValueFailsWholeLoad(string protectedValue, bool removeAttribute)
    {
        string savedXml = SaveWithDpapi();
        string corruptedXml = removeAttribute
            ? Regex.Replace(savedXml, " Protected=\"[^\"]*\"", "")
            : Regex.Replace(savedXml, "Protected=\"[^\"]*\"", $"Protected=\"{protectedValue}\"");
        var deserializer = new XmlConnectionsDeserializer(() => new Optional<System.Security.SecureString>());

        Assert.Throws<EncryptionException>(() => deserializer.Deserialize(corruptedXml));
    }

    [Test]
    [Apartment(ApartmentState.STA)]
    public void FileWithUnexpectedDpapiRootMarkerFailsWholeLoad()
    {
        string savedXml = SaveWithDpapi();
        string invalidProtectedValue = new DpapiCryptographyProvider().Encrypt("unexpected-root-marker", null);
        string corruptedXml = Regex.Replace(savedXml, "Protected=\"[^\"]*\"", $"Protected=\"{invalidProtectedValue}\"");
        var deserializer = new XmlConnectionsDeserializer(() => new Optional<System.Security.SecureString>());

        Assert.Throws<EncryptionException>(() => deserializer.Deserialize(corruptedXml));
    }
}
