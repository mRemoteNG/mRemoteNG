using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Properties;
using mRemoteNG.Tools.WindowsRegistry;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Settings.Registry;

/// <summary>
/// Exercises <see cref="OptRegistrySqlServerPage"/> against a throwaway
/// <see cref="RegistryHive.CurrentUser"/> path through its internal constructor, like
/// <see cref="OptRegistrySecurityPageTests"/>, since writing to LocalMachine needs administrator rights.
/// </summary>
[SupportedOSPlatform("windows")]
public class OptRegistrySqlServerPageTests
{
    private const RegistryHive TestHive = RegistryHive.CurrentUser;
    private const string TestSubKey = @"Software\mRemoteNGTest\OptRegistrySqlServerPageTests";

    private bool _useSqlServer;
    private bool _trustServerCertificate;

    [SetUp]
    public void Setup()
    {
        DeleteTestSubKeyIfExists();
        _useSqlServer = OptionsDBsPage.Default.UseSQLServer;
        _trustServerCertificate = OptionsDBsPage.Default.SQLTrustServerCertificate;
    }

    [TearDown]
    public void Teardown()
    {
        OptionsDBsPage.Default.UseSQLServer = _useSqlServer;
        OptionsDBsPage.Default.SQLTrustServerCertificate = _trustServerCertificate;
        DeleteTestSubKeyIfExists();
    }

    private static void DeleteTestSubKeyIfExists()
    {
        try
        {
            new WinRegistry().DeleteTree(TestHive, TestSubKey);
        }
        catch
        {
            // Subkey didn't exist — nothing to clean up.
        }
    }

    [Test]
    public void TrustServerCertificateSetInRegistryOverridesTheUserSetting()
    {
        OptionsDBsPage.Default.SQLTrustServerCertificate = false;
        WinRegistryEntry<bool>.New(TestHive, TestSubKey, "UseSQLServer", true).Write();
        WinRegistryEntry<bool>.New(TestHive, TestSubKey, "SQLTrustServerCertificate", true).Write();

        var page = new OptRegistrySqlServerPage(TestHive, TestSubKey);

        Assert.Multiple(() =>
        {
            Assert.That(page.SQLTrustServerCertificate.IsSet, Is.True);
            Assert.That(OptionsDBsPage.Default.SQLTrustServerCertificate, Is.True);
        });
    }

    [Test]
    public void TrustServerCertificateNotSetInRegistryLeavesTheUserSetting()
    {
        OptionsDBsPage.Default.SQLTrustServerCertificate = true;
        WinRegistryEntry<bool>.New(TestHive, TestSubKey, "UseSQLServer", true).Write();

        var page = new OptRegistrySqlServerPage(TestHive, TestSubKey);

        Assert.Multiple(() =>
        {
            Assert.That(page.SQLTrustServerCertificate.IsSet, Is.False);
            Assert.That(OptionsDBsPage.Default.SQLTrustServerCertificate, Is.True);
        });
    }
}
