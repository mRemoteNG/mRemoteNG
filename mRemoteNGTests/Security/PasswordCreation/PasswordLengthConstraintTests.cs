using System;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordLengthConstraintTests
    {
        private PasswordLengthConstraint _lengthConstraint;


        [Test]
        public void PasswordLessThanMinimumLengthFailsValidation()
        {
            var password = "123456789".ConvertToSecureString();
            _lengthConstraint = new PasswordLengthConstraint(10);
            Assert.That(_lengthConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordThatEqualsMinimumLengthPassesValidation()
        {
            var password = "12345".ConvertToSecureString();
            _lengthConstraint = new PasswordLengthConstraint(5);
            Assert.That(_lengthConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordGreaterThanMaxLengthFailsValidation()
        {
            var password = "123456".ConvertToSecureString();
            _lengthConstraint = new PasswordLengthConstraint(1, 5);
            Assert.That(_lengthConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordThatEqualsMaxLengthPassesValidation()
        {
            var password = "12345".ConvertToSecureString();
            _lengthConstraint = new PasswordLengthConstraint(1, 5);
            Assert.That(_lengthConstraint.Validate(password), Is.True);
        }

        [Test]
        public void MinimumLengthMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordLengthConstraint(-1));
        }

        [Test]
        public void MaximumLengthMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordLengthConstraint(1, -1));
        }

        [Test]
        public void MaximumLengthMustBeGreaterThanMinimumLength()
        {
            Assert.Throws<ArgumentException>(() => new PasswordLengthConstraint(4, 1));
        }
    }
}