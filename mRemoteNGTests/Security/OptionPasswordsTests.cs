using System;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    public class OptionPasswordsTests
    {
        private const string Password = "MySecret!";

        [Test]
        public void AProtectedPasswordComesBack()
        {
            string stored = OptionPasswords.Protect(Password, portable: false);

            Assert.That(OptionPasswords.Unprotect(stored), Is.EqualTo(Password));
        }

        [Test]
        public void AProtectedPasswordCannotBeReadWithTheDefaultKey()
        {
            string stored = OptionPasswords.Protect(Password, portable: false);

            Assert.That(stored, Does.Not.Contain(Password));
            Assert.That(() => new LegacyRijndaelCryptographyProvider().Decrypt(stored, Runtime.EncryptionKey),
                        Throws.InstanceOf<EncryptionException>());
        }

        [Test]
        public void APasswordSavedByAnEarlierVersionIsStillRead()
        {
            string stored = new LegacyRijndaelCryptographyProvider().Encrypt(Password, Runtime.EncryptionKey);

            Assert.That(OptionPasswords.Unprotect(stored), Is.EqualTo(Password));
        }

        [Test]
        public void AnEmptyPasswordStaysEmpty()
        {
            Assert.That(OptionPasswords.Protect("", portable: false), Is.Empty);
            Assert.That(OptionPasswords.Unprotect(""), Is.Empty);
        }

        [Test]
        public void APortableBuildKeepsTheOldForm()
        {
            string stored = OptionPasswords.Protect(Password, portable: true);

            Assert.That(OptionPasswords.IsProtected(stored), Is.False);
            Assert.That(new LegacyRijndaelCryptographyProvider().Decrypt(stored, Runtime.EncryptionKey), Is.EqualTo(Password));
        }

        [Test]
        public void APasswordProtectedForSomeoneElseReadsAsNone()
        {
            string foreign = "dpapi:" + Convert.ToBase64String(new byte[] { 1, 0, 0, 0, 0xd0, 0x8c, 0x9d, 0xdf, 1, 0x15, 0xd1, 0x11 });

            Assert.That(OptionPasswords.Unprotect(foreign), Is.Empty);
            Assert.That(OptionPasswords.Unprotect("dpapi:not base64!"), Is.Empty);
        }

        [Test]
        public void AnOldFormPasswordIsMovedToDpapi()
        {
            string old = new LegacyRijndaelCryptographyProvider().Encrypt(Password, Runtime.EncryptionKey);

            string moved = OptionPasswords.Reprotect(old);

            Assert.That(OptionPasswords.IsProtected(moved), Is.True);
            Assert.That(OptionPasswords.Unprotect(moved), Is.EqualTo(Password));
        }

        [Test]
        public void ReprotectLeavesProtectedEmptyAndUnreadableValuesAsTheyAre()
        {
            string protectedValue = OptionPasswords.Protect(Password, portable: false);

            Assert.That(OptionPasswords.Reprotect(protectedValue), Is.EqualTo(protectedValue));
            Assert.That(OptionPasswords.Reprotect(""), Is.Empty);
            Assert.That(OptionPasswords.Reprotect("not a password!"), Is.EqualTo("not a password!"));
        }
    }
}
