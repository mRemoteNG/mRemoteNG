using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    [TestFixture]
    public class LdapPathSanitizerTests
    {
        [Test]
        public void SanitizeDistinguishedName_EscapesBackslash()
        {
            string input = "CN=Test\\User";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("CN\\=Test\\\\User"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesComma()
        {
            string input = "CN=User,Test";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("CN\\=User\\,Test"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesPlus()
        {
            string input = "CN=User+Admin";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("CN\\=User\\+Admin"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesQuotes()
        {
            string input = "CN=\"Test User\"";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("CN\\=\\\"Test User\\\""));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesLessThan()
        {
            string input = "CN=<User>";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("CN\\=\\<User\\>"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesSemicolon()
        {
            string input = "CN=User;Admin";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("CN\\=User\\;Admin"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesLeadingHash()
        {
            string input = "#Test";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("\\#Test"));
        }

        [Test]
        public void SanitizeDistinguishedName_DoesNotEscapeMiddleHash()
        {
            string input = "Test#User";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("Test#User"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesLeadingSpace()
        {
            string input = " Test";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("\\ Test"));
        }

        [Test]
        public void SanitizeDistinguishedName_EscapesTrailingSpace()
        {
            string input = "Test ";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("Test\\ "));
        }

        [Test]
        public void SanitizeDistinguishedName_DoesNotEscapeMiddleSpace()
        {
            string input = "Test User";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            Assert.That(result, Is.EqualTo("Test User"));
        }

        [Test]
        public void SanitizeDistinguishedName_ReturnsEmptyForNull()
        {
            string result = LdapPathSanitizer.SanitizeDistinguishedName(null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void SanitizeDistinguishedName_ReturnsEmptyForEmpty()
        {
            string result = LdapPathSanitizer.SanitizeDistinguishedName(string.Empty);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SanitizeFilterValue_EscapesAsterisk()
        {
            string input = "user*";
            string result = LdapPathSanitizer.SanitizeFilterValue(input);
            Assert.That(result, Is.EqualTo("user\\2a"));
        }

        [Test]
        public void SanitizeFilterValue_EscapesParentheses()
        {
            string input = "(admin)";
            string result = LdapPathSanitizer.SanitizeFilterValue(input);
            Assert.That(result, Is.EqualTo("\\28admin\\29"));
        }

        [Test]
        public void SanitizeFilterValue_EscapesBackslash()
        {
            string input = "user\\admin";
            string result = LdapPathSanitizer.SanitizeFilterValue(input);
            Assert.That(result, Is.EqualTo("user\\5cadmin"));
        }

        [Test]
        public void SanitizeFilterValue_EscapesNullCharacter()
        {
            string input = "user\0admin";
            string result = LdapPathSanitizer.SanitizeFilterValue(input);
            Assert.That(result, Is.EqualTo("user\\00admin"));
        }

        [Test]
        public void SanitizeFilterValue_ReturnsNullForNull()
        {
            string result = LdapPathSanitizer.SanitizeFilterValue(null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void SanitizeFilterValue_ReturnsEmptyForEmpty()
        {
            string result = LdapPathSanitizer.SanitizeFilterValue(string.Empty);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsTrueForLdapUri()
        {
            string input = "LDAP://dc=example,dc=com";
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat(input);
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsTrueForLdapsUri()
        {
            string input = "LDAPS://dc=example,dc=com";
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat(input);
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsTrueForDnWithEquals()
        {
            string input = "CN=User,OU=Users,DC=example,DC=com";
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat(input);
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsFalseForNull()
        {
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat(null);
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsFalseForEmpty()
        {
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat(string.Empty);
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsFalseForWhitespace()
        {
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat("   ");
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidDistinguishedNameFormat_ReturnsFalseForPlainString()
        {
            bool result = LdapPathSanitizer.IsValidDistinguishedNameFormat("plainstring");
            Assert.That(result, Is.False);
        }

        [Test]
        public void SanitizeDistinguishedName_PreventsLdapInjectionAttempt()
        {
            // Test common LDAP injection patterns
            string input = "*)(objectClass=*))(|(cn=*";
            string result = LdapPathSanitizer.SanitizeDistinguishedName(input);
            // Verify special characters are escaped
            Assert.That(result, Does.Contain("\\="));
        }

        [Test]
        public void SanitizeFilterValue_PreventsLdapInjectionAttempt()
        {
            // Test common LDAP injection patterns
            string input = "*)(uid=*))(|(uid=*";
            string result = LdapPathSanitizer.SanitizeFilterValue(input);
            // Verify special characters are escaped
            Assert.That(result, Does.Contain("\\2a"));
            Assert.That(result, Does.Contain("\\28"));
            Assert.That(result, Does.Contain("\\29"));
        }
    }
}
