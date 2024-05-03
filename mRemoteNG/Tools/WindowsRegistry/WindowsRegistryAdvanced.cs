using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Security.SymmetricEncryption;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Extends the functionality of interacting with the Windows Registry, building upon the base WindowsRegistry class.
    /// </summary>
    public class WindowsRegistryAdvanced : WindowsRegistry , IRegistryAdvanced, IRegistryAdvancedRead
    {
        #region dword methods
        /// <summary>
        /// Retrieves a DWORD (32-bit integer) value from the Windows Registry based on the specified registry information.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The path to the registry key.</param>
        /// <param name="propertyName">The name of the registry property.</param>
        /// <param name="defaultValue">Optional default value to be used if the registry key is not present (default is null).</param>
        /// <returns>A WindowsRegistryKeyInteger instance representing the retrieved DWORD value.</returns>
        public WindowsRegistryKeyInteger GetInteger(RegistryHive hive, string path, string propertyName, int? defaultValue = null)
        {
            // Retrieve the Windows Registry key
            WindowsRegistryKey key = GetWindowsRegistryKey(hive, path, propertyName);

            // Create a WindowsRegistryKeyInteger instance and initialize it from the retrieved key
            WindowsRegistryKeyInteger IntKey = new();
            IntKey.ConvertFromWindowsRegistryKey(key);

            return IntKey;
        }
        #endregion

        #region string methods
        /// <summary>
        /// Retrieves a string value from the Windows Registry based on the specified registry information.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The path to the registry key.</param>
        /// <param name="propertyName">The name of the registry property.</param>
        /// <param name="defaultValue">Optional default value to be used if the registry key is not present (default is null).</param>
        /// <returns>A WindowsRegistryKeyString instance representing the retrieved string value.</returns>
        public WindowsRegistryKeyString GetString(RegistryHive hive, string path, string propertyName, string defaultValue = null)
        {
            // Retrieve the Windows Registry key
            WindowsRegistryKey key = GetWindowsRegistryKey(hive, path, propertyName);

            // Create a WindowsRegistryKeyString instance and initialize it from the retrieved key
            WindowsRegistryKeyString StrKey = new();
            StrKey.ConvertFromWindowsRegistryKey(key, defaultValue);

            return StrKey;
        }

        /// <summary>
        /// Retrieves a string value from the Windows Registry based on the specified registry information and validates it against a set of allowed values.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The path to the registry key.</param>
        /// <param name="propertyName">The name of the registry property.</param>
        /// <param name="allowedValues">An array of valid values against which the retrieved string is validated.</param>
        /// <param name="caseSensitive">Optional parameter indicating whether the validation is case-sensitive (default is false).</param>
        /// <returns>A WindowsRegistryKeyString instance representing the retrieved and validated string value.</returns>
        public WindowsRegistryKeyString GetStringValidated(RegistryHive hive, string path, string propertyName, string[] allowedValues, bool caseSensitive = false, string defaultValue = null)
        {
            // Retrieve the Windows Registry key
            WindowsRegistryKey key = GetWindowsRegistryKey(hive, path, propertyName);

            // Create a WindowsRegistryKeyString instance and initialize it from the retrieved key
            WindowsRegistryKeyString StrKey = new()
            {
                AllowedValues = allowedValues,
                IsCaseSensitiveValidation = caseSensitive
            };
            StrKey.ConvertFromWindowsRegistryKey(key, defaultValue);

            return StrKey;
        }

        /*public WindowsRegistryKeySecureString GetSecureString(RegistryHive hive, string path, string propertyName)
        {
            // Retrieve the Windows Registry key, the key should be encrypted
            var key = GetWindowsRegistryKey(hive, path, propertyName);

            // Create a WindowsRegistryKeyBoolean instance and initialize it from the retrieved key
            WindowsRegistryKeySecureString secureKey = new (); // class not exsists
            secureKey.ConvertFromWindowsRegistryKey(key); // no default possible!
            return secureKey
        }*/

        #endregion

        #region bool methods
        /// <summary>
        /// Retrieves a boolean value from the Windows Registry based on the specified registry information.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The path to the registry key.</param>
        /// <param name="propertyName">The name of the registry property.</param>
        /// <param name="defaultValue">An optional default value to use if the registry key is not present or if the value is not a valid boolean.</param>
        /// <returns>A WindowsRegistryKeyBoolean instance representing the retrieved boolean value.</returns>
        public WindowsRegistryKeyBoolean GetBoolean(RegistryHive hive, string path, string propertyName, bool? defaultValue = null)
        {
            // Retrieve the Windows Registry key
            WindowsRegistryKey key = GetWindowsRegistryKey(hive, path, propertyName);

            // Create a WindowsRegistryKeyBoolean instance and initialize it from the retrieved key
            WindowsRegistryKeyBoolean boolKey = new ();
            boolKey.ConvertFromWindowsRegistryKey(key, defaultValue);

            return boolKey;
        }

        #endregion
    }
}