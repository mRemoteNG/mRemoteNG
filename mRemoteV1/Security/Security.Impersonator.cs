using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text.RegularExpressions;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Security
{
    public class Impersonator
    {
        private WindowsImpersonationContext impersonatedUser;

        private IntPtr tokenHandle = new IntPtr(0);

        // GetErrorMessage formats and returns an error message corresponding to the input errorCode.
        private string GetErrorMessage(int errorCode)
        {
            var messageSize = 255;
            var lpMsgBuf = "";

            var dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

            var ptrlpSource = IntPtr.Zero;
            var prtArguments = IntPtr.Zero;

            var retVal = FormatMessage(dwFlags, ref ptrlpSource, errorCode, 0, ref lpMsgBuf, messageSize,
                ref prtArguments);
            lpMsgBuf = Regex.Replace(lpMsgBuf, @"\r\n|\n|\r", " ");
            return lpMsgBuf;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void StartImpersonation(string DomainName, string UserName, string Password)
        {
            try
            {
                if (impersonatedUser != null)
                    throw new Exception("Already impersonating a user.");

                tokenHandle = IntPtr.Zero;

                var returnValue =
                    Convert.ToInt32(LogonUser(UserName, DomainName, Password, LOGON32_LOGON_NEW_CREDENTIALS,
                        LOGON32_PROVIDER_DEFAULT, ref tokenHandle));

                if (0 == returnValue)
                {
                    var errCode = Marshal.GetLastWin32Error();
                    var errMsg = "LogonUser failed with error code: " + errCode + "(" + GetErrorMessage(errCode) + ")";
                    var exLogon = new ApplicationException(errMsg);
                    throw exLogon;
                }

                var newId = new WindowsIdentity(tokenHandle);
                impersonatedUser = newId.Impersonate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Starting Impersonation failed (Sessions feature will not work)" + Environment.NewLine + ex.Message,
                    true);
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void StopImpersonation()
        {
            if (impersonatedUser == null)
                return;

            try
            {
                impersonatedUser.Undo(); // Stop impersonating the user.
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Stopping Impersonation failed" + Environment.NewLine + ex.Message, true);
                throw;
            }
            finally
            {
                if (!Equals(tokenHandle, IntPtr.Zero))
                    CloseHandle(tokenHandle);

                impersonatedUser = null;
            }
        }

        #region Logon API

        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        private const int SecurityImpersonation = 2;

        [DllImport("advapi32.dll", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType,
            int dwLogonProvider, ref IntPtr phToken);

        private const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
        private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
        private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

        [DllImport("kernel32.dll")]
        private static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId,
            ref string lpBuffer, int nSize, ref IntPtr Arguments);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL,
            ref IntPtr DuplicateTokenHandle);

        #endregion
    }
}