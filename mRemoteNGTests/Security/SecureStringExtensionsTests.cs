using System;
using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Security
{
    public class SecureStringExtensionsTests
    {
        [Test]
        public void ConvertToSecureString()
        {
            var securedString = "MySecureString".ConvertToSecureString();
            Assert.That(securedString.Length, Is.GreaterThan(0));
        }

        [Test]
        public void ConvertToUnsecureString()
        {
            var originalText = "MySecureString";
            var securedString = originalText.ConvertToSecureString();
            var unsecuredString = securedString.ConvertToUnsecureString();
            Assert.That(unsecuredString, Is.EqualTo(originalText));
        }

        [Test]
        public void ConvertToSecureStringOnNullStringThrowsException()
        {
            string myString = null;
            Assert.Throws<ArgumentNullException>(() => myString.ConvertToSecureString());
        }

        [Test]
        public void ConvertToUnsecureStringOnNullStringThrowsException()
        {
            SecureString secureString = null;
            Assert.Throws<ArgumentNullException>(() => secureString.ConvertToUnsecureString());
        }
    }
}