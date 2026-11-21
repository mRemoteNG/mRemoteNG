using System;
using System.Security;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.Versioning;

public class SqlVersion33To34UpgraderTests
{
    private static readonly SecureString RootPassword = "the root password".ConvertToSecureString();

    private SqlVersion33To34Upgrader _versionUpgrader;
    private LegacyRijndaelCryptographyProvider _oldProvider;
    private DerivedKeyCryptographyProvider _newProvider;

    [SetUp]
    public void Setup()
    {
        var sqlConnector = Substitute.For<MSSqlDatabaseConnector>("", "", "", "");
        _versionUpgrader = new SqlVersion33To34Upgrader(sqlConnector, RootPassword);
        _oldProvider = new LegacyRijndaelCryptographyProvider();
        _newProvider = new DerivedKeyCryptographyProvider(DerivedKeyCryptographyProvider.NewSalt(), 1000);
    }

    [Test]
    public void CanUpgradeIfVersionIs33()
    {
        Assert.That(_versionUpgrader.CanUpgrade(new Version(3, 3)), Is.True);
    }

    [Test]
    public void CannotUpgradeIfVersionIs34()
    {
        Assert.That(_versionUpgrader.CanUpgrade(new Version(3, 4)), Is.False);
    }

    [Test]
    public void TheUpgradeNeedsThePassword()
    {
        var sqlConnector = Substitute.For<MSSqlDatabaseConnector>("", "", "", "");

        Assert.Throws<ArgumentNullException>(() => new SqlVersion33To34Upgrader(sqlConnector, null));
    }

    [Test]
    public void StoredPasswordIsReadableUnderTheNewKey()
    {
        string stored = _oldProvider.Encrypt("the connection password", RootPassword);

        string moved = SqlVersion33To34Upgrader.Reencrypt(stored, _oldProvider, _newProvider, RootPassword);

        Assert.That(_newProvider.Decrypt(moved, RootPassword), Is.EqualTo("the connection password"));
    }

    // The loader has always taken a value the key does not open as an unencrypted one.
    [Test]
    public void UnencryptedValueIsKeptAsItIs()
    {
        string moved = SqlVersion33To34Upgrader.Reencrypt("typed in by hand", _oldProvider, _newProvider, RootPassword);

        Assert.That(_newProvider.Decrypt(moved, RootPassword), Is.EqualTo("typed in by hand"));
    }

    [TestCase(null)]
    [TestCase("")]
    public void EmptyValueStaysEmpty(string stored)
    {
        Assert.That(SqlVersion33To34Upgrader.Reencrypt(stored, _oldProvider, _newProvider, RootPassword), Is.EqualTo(stored));
    }

    [TestCase("ThisIsProtected")]
    [TestCase("ThisIsNotProtected")]
    public void RootMarkerStillAuthenticatesThePassword(string marker)
    {
        string stored = _oldProvider.Encrypt(marker, RootPassword);

        string moved = SqlVersion33To34Upgrader.Reencrypt(stored, _oldProvider, _newProvider, RootPassword);

        Assert.That(_newProvider.Decrypt(moved, RootPassword), Is.EqualTo(marker));
    }
}
