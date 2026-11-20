using System;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers;

/// <summary>
/// From version 3.4 on, the passwords in a database are encrypted under a key derived once with
/// the salt stored in tblRoot (CVE-2026-87868); before it, the key was the MD5 of the password.
/// </summary>
public class SqlConnectionListMetaDataTests
{
    [Test]
    public void DatabaseWithoutSaltIsReadWithTheOldKey()
    {
        var metaData = new SqlConnectionListMetaData { ConfVersion = new Version(3, 3) };

        Assert.That(metaData.CryptographyProvider(), Is.InstanceOf<LegacyRijndaelCryptographyProvider>());
    }

    [Test]
    public void DatabaseWithSaltIsReadWithTheKeyDerivedFromIt()
    {
        byte[] salt = DerivedKeyCryptographyProvider.NewSalt();
        var metaData = new SqlConnectionListMetaData { ConfVersion = new Version(3, 4), KdfSalt = salt, KdfIterations = 1000 };

        var provider = metaData.CryptographyProvider() as DerivedKeyCryptographyProvider;

        Assert.That(provider, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(provider.Salt, Is.EqualTo(salt));
            Assert.That(provider.KeyDerivationIterations, Is.EqualTo(1000));
        });
    }

    [Test]
    public void SupportedDatabaseVersionIs34()
    {
        Assert.That(SqlDatabaseVersionVerifier.SupportedVersion, Is.EqualTo(new Version(3, 4)));
    }

    // A newer version may store the passwords in a form this one cannot read; showing them as
    // read would be wrong, and saving over them would destroy them.
    [TestCase("3.5", true)]
    [TestCase("4.0", true)]
    [TestCase("3.4", false)]
    [TestCase("3.3", false)]
    [TestCase("2.2", false)]
    public void NewerDatabaseIsRefused(string version, bool refused)
    {
        Assert.That(SqlConnectionsLoader.IsNewerThanSupported(new Version(version)), Is.EqualTo(refused));
    }
}
