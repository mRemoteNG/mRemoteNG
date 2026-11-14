using System;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordIncludesUpperCaseConstraintTests
    {
        private PasswordIncludesUpperCaseConstraint _upperCaseConstraint;

        [Test]
        public void PasswordThatExceedsMinimumUpperCasePassesValidation()
        {
            var password = "HELLO".ConvertToSecureString();
            _upperCaseConstraint = new PasswordIncludesUpperCaseConstraint();
            Assert.That(_upperCaseConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordThatMeetsMinimumUpperCasePassesValidation()
        {
            var password = "HELLO".ConvertToSecureString();
            _upperCaseConstraint = new PasswordIncludesUpperCaseConstraint(5);
            Assert.That(_upperCaseConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordWithFewerThanMinimumUpperCaseFailsValidation()
        {
            var password = "Hello".ConvertToSecureString();
            _upperCaseConstraint = new PasswordIncludesUpperCaseConstraint(2);
            Assert.That(_upperCaseConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordWithoutUpperCaseFailsValidation()
        {
            var password = "hello".ConvertToSecureString();
            _upperCaseConstraint = new PasswordIncludesUpperCaseConstraint();
            Assert.That(_upperCaseConstraint.Validate(password), Is.False);
        }

        [Test]
        public void CountToRequireMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordIncludesUpperCaseConstraint(-1));
        }
    }
}