using mRemoteNG.Security.PasswordCreation;
using NUnit.Framework;

namespace mRemoteNGTests.Security.PasswordCreation
{
    public class PasswordStrengthMeterTests
    {
        [Test]
        public void EmptyPasswordHasNoStrength()
        {
            Assert.That(PasswordStrengthMeter.Evaluate("").Strength, Is.EqualTo(PasswordStrength.None));
        }

        [TestCase("password")]
        [TestCase("P@ssw0rd1")]
        [TestCase("qwertyuiop")]
        [TestCase("1234")]
        public void CommonPasswordsAreWeak(string password)
        {
            Assert.That(PasswordStrengthMeter.Evaluate(password).Strength, Is.EqualTo(PasswordStrength.Weak));
        }

        [Test]
        public void OverlongPasswordIsNotEstimated()
        {
            string password = new string('a', PasswordStrengthMeter.MaximumPasswordLength + 1);

            Assert.That(PasswordStrengthMeter.Evaluate(password).Strength, Is.EqualTo(PasswordStrength.NotEstimated));
        }

        [Test]
        public void LongUncommonPassphraseIsStrong()
        {
            Assert.That(PasswordStrengthMeter.Evaluate("Tangerine-Ladder-Quartz-Mosquito-71").Strength,
                Is.EqualTo(PasswordStrength.Strong));
        }

        [Test]
        public void WeakPasswordComesWithAdvice()
        {
            Assert.That(PasswordStrengthMeter.Evaluate("password").Advice, Is.Not.Empty);
        }

        [Test]
        public void PasswordMadeFromTheUserNameRatesLower()
        {
            const string password = "bobsecord1987";
            PasswordStrength withoutUserInputs = PasswordStrengthMeter.Evaluate(password).Strength;
            PasswordStrength withUserInputs = PasswordStrengthMeter.Evaluate(password, new[] { "bobsecord" }).Strength;

            Assert.That(withUserInputs, Is.LessThan(withoutUserInputs));
        }
    }
}
