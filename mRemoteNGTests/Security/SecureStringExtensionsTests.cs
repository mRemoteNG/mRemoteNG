using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;

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
    }
}