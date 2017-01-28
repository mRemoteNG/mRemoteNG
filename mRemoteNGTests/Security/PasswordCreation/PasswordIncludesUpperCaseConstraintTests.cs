using System;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordIncludesUpperCaseConstraintTests
    {
        private PasswordIncludesUpperCaseConstraint _lowerCaseConstraint;

        [Test]
        public void PasswordThatExceedsMinimumLowerCasePassesValidation()
        {
            var password = "HELLO".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesUpperCaseConstraint();
            Assert.That(_lowerCaseConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordThatMeetsMinimumLowerCasePassesValidation()
        {
            var password = "HELLO".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesUpperCaseConstraint(5);
            Assert.That(_lowerCaseConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordWithFewerThanMinimumLowerCaseFailsValidation()
        {
            var password = "Hello".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesUpperCaseConstraint(2);
            Assert.That(_lowerCaseConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordWithoutUpperCaseFailsValidation()
        {
            var password = "hello".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesUpperCaseConstraint();
            Assert.That(_lowerCaseConstraint.Validate(password), Is.False);
        }

        [Test]
        public void CountToRequireMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordIncludesUpperCaseConstraint(-1));
        }
    }
}