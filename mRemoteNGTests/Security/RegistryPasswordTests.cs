using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;

namespace mRemoteNGTests.Security;

/// <summary>
/// A password an administrator sets in the registry used to be encrypted under the default key,
/// which is public. It is now protected with DPAPI for the computer, so a copy of the value taken
/// elsewhere does not open; values in the old form still work, with a warning.
/// </summary>
public class RegistryPasswordTests
{
    private const string Password = "registry password for the test";

    private MessageCollector _messageCollector;

    [SetUp]
    public void Setup()
    {
        OptionPasswords.ForgetReportedRegistryValues();
        _messageCollector = new MessageCollector();
    }

    private int Warnings => _messageCollector.Messages.Count(m => m.Class == MessageClass.WarningMsg);

    [Test]
    public void APasswordProtectedForThisComputerComesBack()
    {
        string stored = OptionPasswords.ProtectForThisComputer(Password);

        Assert.Multiple(() =>
        {
            Assert.That(stored, Does.StartWith("dpapi-machine:").And.Not.Contain(Password));
            Assert.That(OptionPasswords.Unprotect(stored), Is.EqualTo(Password));
        });
    }

    // The documentation of the registry settings gives a PowerShell script that makes the value
    // with these exact calls; the script and the application must stay in step.
    [Test]
    public void AValueMadeTheWayTheDocumentedScriptMakesItIsRead()
    {
        byte[] protectedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(Password),
                                                      Encoding.UTF8.GetBytes("mRemoteNG registry password"),
                                                      DataProtectionScope.LocalMachine);
        string stored = "dpapi-machine:" + Convert.ToBase64String(protectedBytes);

        Assert.That(OptionPasswords.Unprotect(stored), Is.EqualTo(Password));
    }

    [Test]
    public void AValueProtectedForAnotherUseReadsAsNone()
    {
        byte[] protectedBytes = ProtectedData.Protect(Encoding.UTF8.GetBytes(Password),
                                                      Encoding.UTF8.GetBytes("mRemoteNG option password"),
                                                      DataProtectionScope.LocalMachine);

        Assert.That(OptionPasswords.Unprotect("dpapi-machine:" + Convert.ToBase64String(protectedBytes)), Is.Empty);
    }

    [Test]
    public void AValueProtectedForThisComputerIsTakenWithoutAWarning()
    {
        string stored = OptionPasswords.ProtectForThisComputer(Password);

        Assert.That(OptionPasswords.FromRegistry(stored, "SQLPassword", _messageCollector), Is.EqualTo(stored));
        Assert.That(Warnings, Is.Zero);
    }

    [Test]
    public void AValueInTheOldFormIsStillTakenWithAWarningOnce()
    {
        string stored = new LegacyRijndaelCryptographyProvider().Encrypt(Password, Runtime.EncryptionKey);

        Assert.That(OptionPasswords.FromRegistry(stored, "SQLPassword", _messageCollector), Is.EqualTo(stored));
        Assert.That(OptionPasswords.FromRegistry(stored, "SQLPassword", _messageCollector), Is.EqualTo(stored));

        Assert.That(Warnings, Is.EqualTo(1));
        Assert.That(_messageCollector.Messages.Single().Text, Does.Contain("SQLPassword"));
    }

    [Test]
    public void EachRegistryValueIsReportedOnItsOwn()
    {
        string stored = new LegacyRijndaelCryptographyProvider().Encrypt(Password, Runtime.EncryptionKey);

        OptionPasswords.FromRegistry(stored, "SQLPassword", _messageCollector);
        OptionPasswords.FromRegistry(stored, "DefaultPassword", _messageCollector);

        Assert.That(Warnings, Is.EqualTo(2));
    }

    [Test]
    public void APlainTextValueIsNotTaken()
    {
        Assert.That(OptionPasswords.FromRegistry("typed in clear", "SQLPassword", _messageCollector), Is.Null);
        Assert.That(OptionPasswords.FromRegistry("password", "SQLPassword", _messageCollector), Is.Null);
        Assert.That(Warnings, Is.Zero);
    }

    [Test]
    public void AValueProtectedOnAnotherComputerIsNotTakenAndIsReported()
    {
        string foreign = "dpapi-machine:" + Convert.ToBase64String(new byte[] { 1, 0, 0, 0, 0xd0, 0x8c, 0x9d, 0xdf, 1, 0x15, 0xd1, 0x11 });

        Assert.That(OptionPasswords.FromRegistry(foreign, "ProxyAuthPass", _messageCollector), Is.Null);
        Assert.That(Warnings, Is.EqualTo(1));
    }

    [Test]
    public void AnEmptyValueIsNotTaken()
    {
        Assert.That(OptionPasswords.FromRegistry("", "SQLPassword", _messageCollector), Is.Null);
        Assert.That(OptionPasswords.ProtectForThisComputer(""), Is.Empty);
    }

    [Test]
    public void AValueProtectedForThisComputerIsNotMovedToTheUser()
    {
        string stored = OptionPasswords.ProtectForThisComputer(Password);

        Assert.That(OptionPasswords.Reprotect(stored), Is.EqualTo(stored));
    }
}
