using System;
using mRemoteNG.Security;
using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordIncludesSpecialCharactersConstraintTests
    {
        private PasswordIncludesSpecialCharactersConstraint _specialCharactersConstraint;

        [Test]
        public void PasswordWithMinimumSpecialCharsPassesValidation()
        {
            var password = "hello$".ConvertToSecureString();
            _specialCharactersConstraint = new PasswordIncludesSpecialCharactersConstraint();
            Assert.That(_specialCharactersConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordExceedingMinimumSpecialCharsPassesValidation()
        {
            var password = "hello!#%$".ConvertToSecureString();
            _specialCharactersConstraint = new PasswordIncludesSpecialCharactersConstraint(3);
            Assert.That(_specialCharactersConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordUnderMinimumSpecialCharsFailsValidation()
        {
            var password = "hello!".ConvertToSecureString();
            _specialCharactersConstraint = new PasswordIncludesSpecialCharactersConstraint(2);
            Assert.That(_specialCharactersConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordWithoutSpecialCharsFailsValidation()
        {
            var password = "hello".ConvertToSecureString();
            _specialCharactersConstraint = new PasswordIncludesSpecialCharactersConstraint();
            Assert.That(_specialCharactersConstraint.Validate(password), Is.False);
        }

        [Test]
        public void PasswordMatchingCustomCharsPassesValidation()
        {
            var password = "hello(".ConvertToSecureString();
            _specialCharactersConstraint = new PasswordIncludesSpecialCharactersConstraint(new[] {'('});
            Assert.That(_specialCharactersConstraint.Validate(password), Is.True);
        }

        [Test]
        public void PasswordWithoutCustomCharsFailsValidation()
        {
            var password = "hello!".ConvertToSecureString();
            _specialCharactersConstraint = new PasswordIncludesSpecialCharactersConstraint(new[] { '(' });
            Assert.That(_specialCharactersConstraint.Validate(password), Is.False);
        }

        [Test]
        public void CantProvideNullListOfCharacters()
        {
            Assert.Throws<ArgumentNullException>(() => new PasswordIncludesSpecialCharactersConstraint(null));
        }

        [Test]
        public void MinimumCountMustBeAPositiveValue()
        {
            Assert.Throws<ArgumentException>(() => new PasswordIncludesSpecialCharactersConstraint(-1));
        }
    }
}