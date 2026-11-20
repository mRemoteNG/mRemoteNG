using System;
using System.Collections;
using System.Security;
using System.Text;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;

namespace mRemoteNGTests.Security;

/// <summary>
/// The key of a connection list is derived once, with PBKDF2-HMAC-SHA256 and a salt of its own,
/// instead of being the MD5 of the password or derived at a low iteration count for every value
/// (CVE-2026-87868).
/// </summary>
public class DerivedKeyCryptographyProviderTests
{
    // Low enough to keep the tests fast; nothing in the format depends on it.
    private const int FastIterations = 1000;

    private byte[] _salt;
    private SecureString _password;

    [SetUp]
    public void Setup()
    {
        _salt = DerivedKeyCryptographyProvider.NewSalt();
        _password = "correct horse battery".ConvertToSecureString();
    }

    private DerivedKeyCryptographyProvider Provider(byte[] salt = null) => new(salt ?? _salt, FastIterations);

    [Test]
    public void KeyIsDerivedWithPbkdf2HmacSha256()
    {
        // RFC 7914, section 11: PBKDF2-HMAC-SHA256 with P = "passwd", S = "salt", c = 1; the first 32 bytes.
        byte[] key = DerivedKeyCryptographyProvider.DeriveKey("passwd", Encoding.ASCII.GetBytes("salt"), 1);

        Assert.That(Convert.ToHexString(key), Is.EqualTo("55AC046E56E3089FEC1691C22544B605F94185216DDE0465E68B9D57C20DACBC"));
    }

    [Test]
    public void DefaultIterationCountFollowsOwasp()
    {
        Assert.That(DerivedKeyCryptographyProvider.DefaultIterations, Is.GreaterThanOrEqualTo(600_000));
    }

    [Test]
    public void NewSaltsAreRandomAndLongEnough()
    {
        byte[] first = DerivedKeyCryptographyProvider.NewSalt();
        byte[] second = DerivedKeyCryptographyProvider.NewSalt();

        Assert.Multiple(() =>
        {
            Assert.That(first, Has.Length.EqualTo(DerivedKeyCryptographyProvider.SaltSizeInBytes));
            Assert.That(DerivedKeyCryptographyProvider.SaltSizeInBytes, Is.GreaterThanOrEqualTo(16));
            Assert.That(first, Is.Not.EqualTo(second));
        });
    }

    [Test]
    public void AnotherInstanceWithTheSameSaltDecryptsTheValue()
    {
        string cipherText = Provider().Encrypt("the connection password", _password);

        Assert.That(Provider().Decrypt(cipherText, _password), Is.EqualTo("the connection password"));
    }

    [Test]
    public void EachValueGetsItsOwnNonce()
    {
        DerivedKeyCryptographyProvider provider = Provider();

        Assert.That(provider.Encrypt("same", _password), Is.Not.EqualTo(provider.Encrypt("same", _password)));
    }

    [Test]
    public void ValueIsTheNonceFollowedByTheCipherTextAndTheTag()
    {
        byte[] value = Convert.FromBase64String(Provider().Encrypt("12345678", _password));

        // AES-GCM: a 12 byte nonce, the cipher text as long as the plain text, a 16 byte tag
        Assert.That(value, Has.Length.EqualTo(12 + 8 + 16));
    }

    [Test]
    public void WrongPasswordIsRejected()
    {
        string cipherText = Provider().Encrypt("secret", _password);

        Assert.Throws<EncryptionException>(() => Provider().Decrypt(cipherText, "wrong".ConvertToSecureString()));
    }

    [Test]
    public void WrongPasswordDoesNotKeepTheRightOneFromWorking()
    {
        DerivedKeyCryptographyProvider provider = Provider();
        string cipherText = provider.Encrypt("secret", _password);

        Assert.Throws<EncryptionException>(() => provider.Decrypt(cipherText, "wrong".ConvertToSecureString()));
        Assert.That(provider.Decrypt(cipherText, _password), Is.EqualTo("secret"));
    }

    [Test]
    public void ValueMadeUnderAnotherSaltIsRejected()
    {
        string cipherText = Provider().Encrypt("secret", _password);

        Assert.Throws<EncryptionException>(() => Provider(DerivedKeyCryptographyProvider.NewSalt()).Decrypt(cipherText, _password));
    }

    [Test]
    public void ChangedValueIsRejected()
    {
        byte[] value = Convert.FromBase64String(Provider().Encrypt("secret", _password));
        value[^1] ^= 1;

        Assert.Throws<EncryptionException>(() => Provider().Decrypt(Convert.ToBase64String(value), _password));
    }

    [TestCase("not base64!")]
    [TestCase("AAAA")]
    public void MalformedValueIsRejected(string cipherText)
    {
        Assert.Throws<EncryptionException>(() => Provider().Decrypt(cipherText, _password));
    }

    [Test]
    public void EmptyValueStaysEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Provider().Encrypt("", _password), Is.Empty);
            Assert.That(Provider().Decrypt("", _password), Is.Empty);
        });
    }

    [Test]
    public void ShortSaltIsRefused()
    {
        Assert.Throws<ArgumentException>(() => new DerivedKeyCryptographyProvider(new byte[8], FastIterations));
    }

    [Test]
    public void SaltCannotBeChangedFromOutside()
    {
        DerivedKeyCryptographyProvider provider = Provider();
        string cipherText = provider.Encrypt("secret", _password);
        provider.Salt[0] ^= 1;

        Assert.That(provider.Decrypt(cipherText, _password), Is.EqualTo("secret"));
    }

    [TestCaseSource(nameof(AllEngineAndModeCombos))]
    public void EveryEngineAndModeRoundTrips(BlockCipherEngines engine, BlockCipherModes mode)
    {
        string longText = new('x', 10_000);
        DerivedKeyCryptographyProvider provider = new CryptoProviderFactory(engine, mode).BuildWithKeyDerivedOnce(_salt, FastIterations);

        Assert.Multiple(() =>
        {
            Assert.That(provider.CipherEngine, Is.EqualTo(engine));
            Assert.That(provider.CipherMode, Is.EqualTo(mode));
            Assert.That(provider.Decrypt(provider.Encrypt(longText, _password), _password), Is.EqualTo(longText));
        });
    }

    private static IEnumerable AllEngineAndModeCombos
    {
        get
        {
            foreach (object engine in Enum.GetValues(typeof(BlockCipherEngines)))
            foreach (object mode in Enum.GetValues(typeof(BlockCipherModes)))
                yield return new TestCaseData(engine, mode);
        }
    }
}
