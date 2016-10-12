using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace mRemoteNG.Tools
{
    public class Authenticode
    {
        #region Protected Classes

        protected class Win32
        {
            // ReSharper disable InconsistentNaming
            [DllImport("wintrust.dll", CharSet = CharSet.Auto, SetLastError = false)]
            public static extern int WinVerifyTrust([In] IntPtr hWnd,
                [In] [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionOID, [In] IntPtr pWVTData);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public class WINTRUST_DATA
            {
                public uint cbStruct;
                public uint dwProvFlags;
                public uint dwStateAction;
                public uint dwUIChoice;
                public uint dwUIContext;
                public uint dwUnionChoice;
                public uint fdwRevocationChecks;
                public IntPtr hWVTStateData;
                public IntPtr pFile;
                public IntPtr pPolicyCallbackData;
                public IntPtr pSIPClientData;
                public IntPtr pwszURLReference;

                public WINTRUST_DATA()
                {
                    cbStruct = (uint) Marshal.SizeOf(typeof(WINTRUST_DATA));
                }
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public class WINTRUST_FILE_INFO
            {
                public uint cbStruct;
                public IntPtr hFile;
                [MarshalAs(UnmanagedType.LPTStr)] public string pcwszFilePath;
                public IntPtr pgKnownSubject;

                public WINTRUST_FILE_INFO()
                {
                    cbStruct = (uint) Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
                }
            }

            public const int CRYPT_E_NO_MATCH = unchecked ((int) 0x80092009);

            public const int TRUST_E_SUBJECT_NOT_TRUSTED = unchecked ((int) 0x800B0004);
            public const int TRUST_E_NOSIGNATURE = unchecked ((int) 0x800B0100);

            public static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 =
                new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

            public const uint WTD_CHOICE_FILE = 1;
            public const uint WTD_DISABLE_MD2_MD4 = 0x2000;
            public const uint WTD_REVOKE_WHOLECHAIN = 1;

            public const uint WTD_STATEACTION_IGNORE = 0x0;
            public const uint WTD_STATEACTION_VERIFY = 0x1;
            public const uint WTD_STATEACTION_CLOSE = 0x2;

            public const uint WTD_UI_ALL = 1;
            public const uint WTD_UI_NONE = 2;
            public const uint WTD_UI_NOBAD = 3;
            public const uint WTD_UI_NOGOOD = 4;

            public const uint WTD_UICONTEXT_EXECUTE = 0;
            public const uint WTD_UICONTEXT_INSTALL = 1;
            // ReSharper restore InconsistentNaming
        }

        #endregion

        #region Public Methods

        public Authenticode(string filePath)
        {
            FilePath = filePath;
        }

        public StatusValue Verify()
        {
            var trustFileInfoPointer = default(IntPtr);
            var trustDataPointer = default(IntPtr);
            try
            {
                var fileInfo = new FileInfo(FilePath);
                if (!fileInfo.Exists)
                {
                    Status = StatusValue.FileNotExist;
                    return Status;
                }
                if (fileInfo.Length == 0)
                {
                    Status = StatusValue.FileEmpty;
                    return Status;
                }

                if (RequireThumbprintMatch)
                {
                    if (string.IsNullOrEmpty(ThumbprintToMatch))
                    {
                        Status = StatusValue.NoThumbprintToMatch;
                        return Status;
                    }

                    var certificate = X509Certificate.CreateFromSignedFile(FilePath);
                    var certificate2 = new X509Certificate2(certificate);
                    Thumbprint = certificate2.Thumbprint;
                    if (!(Thumbprint == ThumbprintToMatch))
                    {
                        Status = StatusValue.ThumbprintNotMatch;
                        return Status;
                    }
                }

                var trustFileInfo = new Win32.WINTRUST_FILE_INFO();
                trustFileInfo.pcwszFilePath = FilePath;
                trustFileInfoPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustFileInfo));
                Marshal.StructureToPtr(trustFileInfo, trustFileInfoPointer, false);

                var trustData = new Win32.WINTRUST_DATA();
                trustData.dwUIChoice = (uint) Display;
                trustData.fdwRevocationChecks = Win32.WTD_REVOKE_WHOLECHAIN;
                trustData.dwUnionChoice = Win32.WTD_CHOICE_FILE;
                trustData.pFile = trustFileInfoPointer;
                trustData.dwStateAction = Win32.WTD_STATEACTION_IGNORE;
                trustData.dwProvFlags = Win32.WTD_DISABLE_MD2_MD4;
                trustData.dwUIContext = (uint) DisplayContext;
                trustDataPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustData));
                Marshal.StructureToPtr(trustData, trustDataPointer, false);

                var windowHandle = default(IntPtr);
                if (DisplayParentForm == null)
                    windowHandle = IntPtr.Zero;
                else
                    windowHandle = DisplayParentForm.Handle;

                TrustProviderErrorCode = Win32.WinVerifyTrust(windowHandle, Win32.WINTRUST_ACTION_GENERIC_VERIFY_V2,
                    trustDataPointer);
                switch (TrustProviderErrorCode)
                {
                    case Win32.TRUST_E_NOSIGNATURE:
                        Status = StatusValue.NoSignature;
                        break;
                    case Win32.TRUST_E_SUBJECT_NOT_TRUSTED:
                        break;
                }
                if (!(TrustProviderErrorCode == 0))
                {
                    Status = StatusValue.TrustProviderError;
                    return Status;
                }

                Status = StatusValue.Verified;
                return Status;
            }
            catch (CryptographicException ex)
            {
                var hResultProperty = ex.GetType()
                    .GetProperty("HResult", BindingFlags.NonPublic | BindingFlags.Instance);
                var hResult = Convert.ToInt32(hResultProperty.GetValue(ex, null));
                if (hResult == Win32.CRYPT_E_NO_MATCH)
                {
                    Status = StatusValue.NoSignature;
                    return Status;
                }
                else
                {
                    Status = StatusValue.UnhandledException;
                    Exception = ex;
                    return Status;
                }
            }
            catch (Exception ex)
            {
                Status = StatusValue.UnhandledException;
                Exception = ex;
                return Status;
            }
            finally
            {
                if (!(trustDataPointer == IntPtr.Zero))
                    Marshal.FreeCoTaskMem(trustDataPointer);
                if (!(trustFileInfoPointer == IntPtr.Zero))
                    Marshal.FreeCoTaskMem(trustFileInfoPointer);
            }
        }

        #endregion

        #region Public Properties

        public DisplayValue Display { get; set; } = DisplayValue.None;

        public DisplayContextValue DisplayContext { get; set; }
        public Form DisplayParentForm { get; set; }
        public Exception Exception { get; set; }
        public string FilePath { get; set; }
        public bool RequireThumbprintMatch { get; set; }

        public StatusValue Status { get; private set; }

        public string StatusMessage
        {
            get
            {
                switch (Status)
                {
                    case StatusValue.Verified:
                        return "The file was verified successfully.";
                    case StatusValue.FileNotExist:
                        return "The specified file does not exist.";
                    case StatusValue.FileEmpty:
                        return "The specified file is empty.";
                    case StatusValue.NoSignature:
                        return "The specified file is not digitally signed.";
                    case StatusValue.NoThumbprintToMatch:
                        return "A thumbprint match is required but no thumbprint to match against was specified.";
                    case StatusValue.ThumbprintNotMatch:
                        /* (char)0x2260 == the "not equal to" symbol (which I cannot print in here without changing the encoding of the file)
                         * Fancy...
                         * 
                         * "<>" is  fiarly cryptic for non-programers
                         * So is "!="
                         * "=/=" gets the job done, no?
                         * What about plain old English (or localized value): X is not equal to Y?
                         * :P
                         */
                        return string.Format("The thumbprint does not match. {0} {1} {2}.", Thumbprint, (char) 0x2260,
                            ThumbprintToMatch);
                    case StatusValue.TrustProviderError:
                        var ex = new Win32Exception(TrustProviderErrorCode);
                        return string.Format("The trust provider returned an error. {0}", ex.Message);
                    case StatusValue.UnhandledException:
                        return string.Format("An unhandled exception occurred. {0}", Exception.Message);
                    default:
                        return "The status is unknown.";
                }
            }
        }

        public string Thumbprint { get; private set; }

        public string ThumbprintToMatch { get; set; }

        public int TrustProviderErrorCode { get; private set; }

        #endregion

        #region Public Enums

        public enum DisplayValue : uint
        {
            Unknown = 0,
            All = Win32.WTD_UI_ALL,
            None = Win32.WTD_UI_NONE,
            NoBad = Win32.WTD_UI_NOBAD,
            NoGood = Win32.WTD_UI_NOGOOD
        }

        public enum DisplayContextValue : uint
        {
            Execute = Win32.WTD_UICONTEXT_EXECUTE,
            Install = Win32.WTD_UICONTEXT_INSTALL
        }

        public enum StatusValue
        {
            Unknown = 0,
            Verified,
            FileNotExist,
            FileEmpty,
            NoSignature,
            NoThumbprintToMatch,
            ThumbprintNotMatch,
            TrustProviderError,
            UnhandledException
        }

        #endregion
    }
}