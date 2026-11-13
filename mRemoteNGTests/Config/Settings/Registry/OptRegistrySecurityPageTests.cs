using System.Runtime.Versioning;
using System.Threading;
using Microsoft.Win32;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Tools.WindowsRegistry;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Settings.Registry;

/// <summary>
/// Exercises <see cref="OptRegistrySecurityPage"/> against a throwaway
/// <see cref="RegistryHive.CurrentUser"/> path via its internal test constructor, instead of the
/// production <see cref="RegistryHive.LocalMachine"/> location — writing to LocalMachine normally
/// requires administrator rights, which a unit test must not depend on. CurrentUser needs no elevated
/// privileges, matching the pattern already used by
/// <c>mRemoteNGTests.Tools.Registry.RegistryEntryTest.BoolEntryTest</c> for the lower-level
/// <see cref="WinRegistryEntry{T}"/> itself.
/// </summary>
[SupportedOSPlatform("windows")]
[Apartment(ApartmentState.STA)] // OptRegistrySecurityPage.SetupValidation() constructs a WinForms control.
public class OptRegistrySecurityPageTests
{
    private const RegistryHive TestHive = RegistryHive.CurrentUser;
    private const string TestRoot = @"Software\mRemoteNGTest";
    private const string TestSubKey = $@"{TestRoot}\OptRegistrySecurityPageTests";

    [SetUp]
    public void Setup() => DeleteTestSubKeyIfExists();

    [TearDown]
    public void Teardown() => DeleteTestSubKeyIfExists();

    /// <summary>
    /// Deletes <see cref="TestSubKey"/> under <see cref="TestHive"/>, so each test starts from (and
    /// leaves behind) a clean subkey — OptRegistrySecurityPage's constructor reads every value
    /// unconditionally, so a leftover value from one test would leak into the next.
    /// <see cref="WinRegistry.DeleteTree"/> throws when the subkey doesn't exist yet (first run, or
    /// a prior test that never wrote anything), which is the expected, harmless case here.
    /// </summary>
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
    public void UseDpapiProtection_NotSetInRegistry_IsSetReturnsFalse()
    {
        var page = new OptRegistrySecurityPage(TestHive, TestSubKey);

        Assert.That(page.UseDpapiProtection.IsSet, Is.False);
    }

    [Test]
    public void UseDpapiProtection_SetTrueInRegistry_ReadBackAsTrue()
    {
        WinRegistryEntry<bool>.New(TestHive, TestSubKey, "UseDpapiProtection", true).Write();

        var page = new OptRegistrySecurityPage(TestHive, TestSubKey);

        Assert.Multiple(() =>
        {
            Assert.That(page.UseDpapiProtection.IsSet, Is.True);
            Assert.That(page.UseDpapiProtection.Value, Is.True);
        });
    }

    [Test]
    public void UseDpapiProtection_SetFalseInRegistry_ReadBackAsFalse()
    {
        WinRegistryEntry<bool>.New(TestHive, TestSubKey, "UseDpapiProtection", false).Write();

        var page = new OptRegistrySecurityPage(TestHive, TestSubKey);

        Assert.Multiple(() =>
        {
            Assert.That(page.UseDpapiProtection.IsSet, Is.True);
            Assert.That(page.UseDpapiProtection.Value, Is.False);
        });
    }

    [Test]
    public void UseDpapiProtection_SetTrueInRegistry_OverridesUserSetting()
    {
        mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection = false;

        WinRegistryEntry<bool>.New(TestHive, TestSubKey, "UseDpapiProtection", true).Write();
        _ = new OptRegistrySecurityPage(TestHive, TestSubKey);

        // OptRegistrySecurityPage's constructor applies every IsSet registry value onto the shared
        // Properties.OptionsSecurityPage.Default settings object — this is the same mechanism the
        // Options UI checkbox reads from (SecurityPage.LoadSettings), and the same mechanism
        // CryptoProviderFactoryFromSettings.Build() consults to decide AES vs. DPAPI.
        Assert.That(mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection, Is.True);
    }

    [Test]
    public void UseDpapiProtection_NotSetInRegistry_DoesNotOverrideExistingUserSetting()
    {
        mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection = true;

        _ = new OptRegistrySecurityPage(TestHive, TestSubKey);

        // No registry value was written for this test's subkey — the existing user setting (whatever
        // the user picked in Options > Security) must be left untouched.
        Assert.That(mRemoteNG.Properties.OptionsSecurityPage.Default.UseDpapiProtection, Is.True);
    }
}
