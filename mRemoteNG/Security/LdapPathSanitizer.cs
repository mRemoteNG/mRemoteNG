using System;
using System.Text;

namespace mRemoteNG.Security
{
    /// <summary>
    /// Provides methods to sanitize LDAP distinguished names and filter values to prevent LDAP injection attacks.
    /// Based on OWASP recommendations for LDAP injection prevention.
    /// </summary>
    public static class LdapPathSanitizer
    {
        /// <summary>
        /// Sanitizes an LDAP distinguished name (DN) by escaping special characters.
        /// This should be used for DN values like those passed to DirectoryEntry constructor.
        /// </summary>
        /// <param name="distinguishedName">The DN to sanitize</param>
        /// <returns>A sanitized DN string safe from LDAP injection</returns>
        public static string SanitizeDistinguishedName(string distinguishedName)
        {
            if (string.IsNullOrEmpty(distinguishedName))
                return distinguishedName;

            // Escape special characters in DN according to RFC 4514
            // Special characters that need escaping: , \ # + < > ; " =
            // and leading/trailing spaces
            StringBuilder result = new StringBuilder();
            
            for (int i = 0; i < distinguishedName.Length; i++)
            {
                char c = distinguishedName[i];
                
                // Escape special characters
                switch (c)
                {
                    case '\\':
                        result.Append("\\\\");
                        break;
                    case ',':
                        result.Append("\\,");
                        break;
                    case '+':
                        result.Append("\\+");
                        break;
                    case '"':
                        result.Append("\\\"");
                        break;
                    case '<':
                        result.Append("\\<");
                        break;
                    case '>':
                        result.Append("\\>");
                        break;
                    case ';':
                        result.Append("\\;");
                        break;
                    case '=':
                        result.Append("\\=");
                        break;
                    case '#':
                        // # needs to be escaped only if it's the first character
                        if (i == 0)
                            result.Append("\\#");
                        else
                            result.Append(c);
                        break;
                    case ' ':
                        // Leading and trailing spaces need to be escaped
                        if (i == 0 || i == distinguishedName.Length - 1)
                            result.Append("\\ ");
                        else
                            result.Append(c);
                        break;
                    default:
                        // Check for control characters (0x00-0x1F and 0x7F)
                        if (c < 0x20 || c == 0x7F)
                        {
                            result.Append("\\");
                            result.Append(((int)c).ToString("x2"));
                        }
                        else
                        {
                            result.Append(c);
                        }
                        break;
                }
            }
            
            return result.ToString();
        }

        /// <summary>
        /// Sanitizes an LDAP filter value by escaping special characters.
        /// This should be used for filter values in LDAP search filters.
        /// </summary>
        /// <param name="filterValue">The filter value to sanitize</param>
        /// <returns>A sanitized filter value safe from LDAP injection</returns>
        public static string SanitizeFilterValue(string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
                return filterValue;

            // Escape special characters in filter according to RFC 4515
            // Special characters that need escaping: * ( ) \ NUL
            StringBuilder result = new StringBuilder();
            
            foreach (char c in filterValue)
            {
                switch (c)
                {
                    case '\\':
                        result.Append("\\5c");
                        break;
                    case '*':
                        result.Append("\\2a");
                        break;
                    case '(':
                        result.Append("\\28");
                        break;
                    case ')':
                        result.Append("\\29");
                        break;
                    case '\0':
                        result.Append("\\00");
                        break;
                    default:
                        // Check for other control characters
                        if (c < 0x20 || c == 0x7F)
                        {
                            result.Append("\\");
                            result.Append(((int)c).ToString("x2"));
                        }
                        else
                        {
                            result.Append(c);
                        }
                        break;
                }
            }
            
            return result.ToString();
        }

        /// <summary>
        /// Validates that a distinguished name appears to be in valid LDAP DN format.
        /// This is a basic check to ensure the DN structure is reasonable.
        /// </summary>
        /// <param name="distinguishedName">The DN to validate</param>
        /// <returns>True if the DN appears valid, false otherwise</returns>
        public static bool IsValidDistinguishedNameFormat(string distinguishedName)
        {
            if (string.IsNullOrWhiteSpace(distinguishedName))
                return false;

            // Basic validation: should start with LDAP:// or contain = for attribute=value pairs
            // This is a simple heuristic check, not a full RFC validation
            if (distinguishedName.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase) ||
                distinguishedName.StartsWith("LDAPS://", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Check if it looks like a DN (contains attribute=value pairs)
            return distinguishedName.Contains("=");
        }
    }
}
