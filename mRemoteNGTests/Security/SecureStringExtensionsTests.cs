using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace mRemoteNGTests.Security
{
    [TestFixture]
    public class SecureStringExtensionsTests
    {
        private SecureString _secureString;
        private string _testPassword;

        [SetUp]
        public void Setup()
        {
            _secureString = new SecureString();
            _testPassword = "testPassword123";
        }

        [TearDown]
        public void Teardown()
        {
            _secureString = null;
        }

        [Test]
        public void ConvertToUnsecureStringDoesNotCorruptValue()
        {
            foreach (var c in _testPassword.ToCharArray())
                _secureString.AppendChar(c);
            Assert.That(_secureString.ConvertToUnsecureString(), Is.EqualTo(_testPassword));
        }

        [Test]
        public void ConvertToSecureStringDoesNotCorruptValue()
        {
            var securePassword = _testPassword.ConvertToSecureString();
            foreach (var c in _testPassword.ToCharArray())
                _secureString.AppendChar(c);
            Assert.That(securePassword.ConvertToUnsecureString(), Is.EqualTo(_secureString.ConvertToUnsecureString()));
        }

        [Test]
        public void ConvertToSecureStringThrowsExceptionWhenStringIsNull()
        {
            string testString = null;
            ActualValueDelegate<object> testDelegate = () => testString.ConvertToSecureString();
            Assert.That(testDelegate, Throws.ArgumentNullException);
        }

        [Test]
        public void ConvertToUnsecureStringThrowsExceptionWhenSecureStringIsNull()
        {
            SecureString testSecureString = null;
            ActualValueDelegate<object> testDelegate = () => testSecureString.ConvertToUnsecureString();
            Assert.That(testDelegate, Throws.ArgumentNullException);
        }
    }
}