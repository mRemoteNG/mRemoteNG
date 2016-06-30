using System;
using System.Runtime.InteropServices;
using System.Security;

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
    }
}