using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace mRemoteNG.Security
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Method to marshall a SecureString out of protected memory into a standard String object that is required by most other functions.
        /// Code initially taken from Fabio Pintos
        /// Source: https://blogs.msdn.microsoft.com/fpintos/2009/06/12/how-to-properly-convert-securestring-to-string/
        /// </summary>
        /// <param name="securePassword"></param>
        /// <returns></returns>
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString ConvertToSecureString(this string unsecuredPassword)
        {
            if (unsecuredPassword == null)
                throw new ArgumentNullException(nameof(unsecuredPassword));

            var secureString = new SecureString();
            foreach (var character in unsecuredPassword.ToCharArray())
                secureString.AppendChar(character);
            // ReSharper disable once RedundantAssignment
            unsecuredPassword = null;
            return secureString;
        }

        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}