using System;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordIncludesNumbersConstraintTests
    {
        private PasswordIncludesNumbersConstraint _includesNumbersConstraint;

        [Test]
        public void PasswordWithANumberPassesConstraint()
        {
            var password = "hello1".ConvertToSecureString();
            _includesNumbersConstraint = new PasswordIncludesNumbersConstraint();
            Assert.That(_includesNumbersConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordWithFewerThanTheMinimumCountOfNumbersFailsConstraint()
        {
            var password = "hello1".ConvertToSecureString();
            _includesNumbersConstraint = new PasswordIncludesNumbersConstraint(2);
            Assert.That(_includesNumbersConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordWithoutNumbersFailsConstraint()
        {
            var password = "hello".ConvertToSecureString();
            _includesNumbersConstraint = new PasswordIncludesNumbersConstraint();
            Assert.That(_includesNumbersConstraint.Validate(password), Is.False);
        }

        [Test]
        public void CountOfNumbersToRequireMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordIncludesNumbersConstraint(-1));
        }
    }
}