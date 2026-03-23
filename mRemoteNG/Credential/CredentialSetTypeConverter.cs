using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;

namespace mRemoteNG.Credential
{
    /// <summary>
    /// TypeConverter for the UserViaAPI property that provides a dropdown list of available credential sets
    /// when the ExternalCredentialProvider is set to InternalCredentialSet.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class CredentialSetTypeConverter : StringConverter
    {
        /// <summary>
        /// Gets a list of available credential sets as title-to-GUID mappings.
        /// </summary>
        private static Dictionary<string, string> GetCredentialSetMap()
        {
            Dictionary<string, string> credentialMap = new()
            {
                // Add empty entry for no selection
                { string.Empty, string.Empty }
            };

            if (Runtime.CredentialService == null)
                return credentialMap;

            try
            {
                IEnumerable<ICredentialRecord> credentials = Runtime.CredentialService.GetCredentialRecords();
                foreach (ICredentialRecord credential in credentials)
                {
                    string title = credential.Title ?? $"Unnamed ({credential.Id})";
                    // Handle duplicate titles by appending GUID
                    if (credentialMap.ContainsKey(title))
                    {
                        title = $"{title} ({credential.Id})";
                    }
                    credentialMap[title] = credential.Id.ToString();
                }
            }
            catch
            {
                // Silently handle errors - return just the empty entry
            }

            return credentialMap;
        }

        /// <summary>
        /// Gets the display names (titles) of available credential sets.
        /// </summary>
        public static string[] CredentialSetTitles => GetCredentialSetMap().Keys.ToArray();

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return new StandardValuesCollection(CredentialSetTitles);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context)
        {
            // Allow custom values for other external credential providers (like DelineaSecretServer)
            // The dropdown will still show available credential sets for InternalCredentialSet
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
        {
            return true;
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string stringValue)
            {
                // If it's already a GUID, return it as-is
                if (Guid.TryParse(stringValue, out _))
                    return stringValue;

                // Otherwise, look up the GUID from the title
                Dictionary<string, string> map = GetCredentialSetMap();
                if (map.TryGetValue(stringValue, out string? guid))
                    return guid;

                // Return the original value as-is for custom credential providers
                return stringValue;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is string stringValue)
            {
                // If it's a GUID, convert to display title
                if (Guid.TryParse(stringValue, out Guid guid))
                {
                    Dictionary<string, string> map = GetCredentialSetMap();
                    foreach (KeyValuePair<string, string> kvp in map)
                    {
                        if (kvp.Value == stringValue)
                            return kvp.Key;
                    }
                    // If GUID not found in current list, show the GUID
                    return stringValue;
                }

                // Otherwise, return as-is
                return stringValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
