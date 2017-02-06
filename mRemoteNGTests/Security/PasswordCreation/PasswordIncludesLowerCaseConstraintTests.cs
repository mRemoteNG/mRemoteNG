using System;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordIncludesLowerCaseConstraintTests
    {
        private PasswordIncludesLowerCaseConstraint _lowerCaseConstraint;

        [Test]
        public void PasswordThatExceedsMinimumLowerCasePassesValidation()
        {
            var password = "hello".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesLowerCaseConstraint();
            Assert.That(_lowerCaseConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordThatMeetsMinimumLowerCasePassesValidation()
        {
            var password = "hello".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesLowerCaseConstraint(5);
            Assert.That(_lowerCaseConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordWithFewerThanMinimumLowerCaseFailsValidation()
        {
            var password = "hELLO".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesLowerCaseConstraint(2);
            Assert.That(_lowerCaseConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordWithoutLowerCaseFailsValidation()
        {
            var password = "HELLO".ConvertToSecureString();
            _lowerCaseConstraint = new PasswordIncludesLowerCaseConstraint();
            Assert.That(_lowerCaseConstraint.Validate(password), Is.False);
        }

        [Test]
        public void CountToRequireMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordIncludesLowerCaseConstraint(-1));
        }
    }
}