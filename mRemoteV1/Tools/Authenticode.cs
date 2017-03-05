#if !PORTABLE
using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Reflection;
using System.ComponentModel;
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
#pragma warning disable 414
#pragma warning disable 169


namespace mRemoteNG.Tools
{
	public class Authenticode
	{
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
					_thumbprint = certificate2.Thumbprint;
					if (_thumbprint != ThumbprintToMatch)
					{
						Status = StatusValue.ThumbprintNotMatch;
						return Status;
					}
				}

			    var trustFileInfo = new NativeMethods.WINTRUST_FILE_INFO {pcwszFilePath = FilePath};
			    trustFileInfoPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustFileInfo));
				Marshal.StructureToPtr(trustFileInfo, trustFileInfoPointer, false);

			    var trustData = new NativeMethods.WINTRUST_DATA
			    {
			        dwUIChoice = (uint) Display,
			        fdwRevocationChecks = NativeMethods.WTD_REVOKE_WHOLECHAIN,
			        dwUnionChoice = NativeMethods.WTD_CHOICE_FILE,
			        pFile = trustFileInfoPointer,
			        dwStateAction = NativeMethods.WTD_STATEACTION_IGNORE,
			        dwProvFlags = NativeMethods.WTD_DISABLE_MD2_MD4,
			        dwUIContext = (uint) DisplayContext
			    };
			    trustDataPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustData));
				Marshal.StructureToPtr(trustData, trustDataPointer, false);

			    var windowHandle = DisplayParentForm?.Handle ?? IntPtr.Zero;
					
				_trustProviderErrorCode = NativeMethods.WinVerifyTrust(windowHandle, NativeMethods.WINTRUST_ACTION_GENERIC_VERIFY_V2, trustDataPointer);
			    // ReSharper disable once SwitchStatementMissingSomeCases
				switch (_trustProviderErrorCode)
				{
					case NativeMethods.TRUST_E_NOSIGNATURE:
						Status = StatusValue.NoSignature;
						break;
					case NativeMethods.TRUST_E_SUBJECT_NOT_TRUSTED:
						break;
							
				}
				if (_trustProviderErrorCode != 0)
				{
					Status = StatusValue.TrustProviderError;
					return Status;
				}
					
				Status = StatusValue.Verified;
				return Status;
			}
			catch (CryptographicException ex)
			{
				var hResultProperty = ex.GetType().GetProperty("HResult", BindingFlags.NonPublic | BindingFlags.Instance);
				var hResult = Convert.ToInt32(hResultProperty.GetValue(ex, null));
				if (hResult == NativeMethods.CRYPT_E_NO_MATCH)
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
				if (trustDataPointer != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(trustDataPointer);
				}
				if (trustFileInfoPointer != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(trustFileInfoPointer);
				}
			}
		}
        #endregion
			
        #region Public Properties

	    private DisplayValue Display { get; set; } = DisplayValue.None;

	    private DisplayContextValue DisplayContext {get; set;}
	    private Form DisplayParentForm {get; set;}
	    internal Exception Exception {get; private set;}
	    private string FilePath {get; set;}
        internal bool RequireThumbprintMatch { get; set;}

	    internal StatusValue Status { get; private set; }

	    public string GetStatusMessage()
	    {
	        // ReSharper disable once SwitchStatementMissingSomeCases
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
	                return $"The thumbprint does not match. {_thumbprint} {(char) 0x2260} {ThumbprintToMatch}.";
	            case StatusValue.TrustProviderError:
	                var ex = new Win32Exception(_trustProviderErrorCode);
	                return $"The trust provider returned an error. {ex.Message}";
	            case StatusValue.UnhandledException:
	                return $"An unhandled exception occurred. {Exception.Message}";
	            default:
	                return "The status is unknown.";
	        }
	    }
			
		private string _thumbprint;

        internal string ThumbprintToMatch { get; set;}
			
		private int _trustProviderErrorCode;

	    #endregion
		
        #region Public Enums

	    private enum DisplayValue : uint
		{
			Unknown = 0,
			All = NativeMethods.WTD_UI_ALL,
			None = NativeMethods.WTD_UI_NONE,
			NoBad = NativeMethods.WTD_UI_NOBAD,
			NoGood = NativeMethods.WTD_UI_NOGOOD
		}

	    private enum DisplayContextValue : uint
		{
			Execute = NativeMethods.WTD_UICONTEXT_EXECUTE,
			Install = NativeMethods.WTD_UICONTEXT_INSTALL
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
			
        #region Protected Classes

	    private static class NativeMethods
		{
			// ReSharper disable InconsistentNaming
			[DllImport("wintrust.dll", CharSet = CharSet.Auto, SetLastError = false)]
            public static extern int WinVerifyTrust([In()]IntPtr hWnd, [In(), MarshalAs(UnmanagedType.LPStruct)]Guid pgActionOID, [In()]IntPtr pWVTData);
				
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public class WINTRUST_DATA
			{
				public WINTRUST_DATA()
				{
					cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA));
				}

			    private uint cbStruct;
				public IntPtr pPolicyCallbackData;
				public IntPtr pSIPClientData;
				public uint dwUIChoice;
				public uint fdwRevocationChecks;
				public uint dwUnionChoice;
				public IntPtr pFile;
				public uint dwStateAction;
				public IntPtr hWVTStateData;
				public IntPtr pwszURLReference;
				public uint dwProvFlags;
				public uint dwUIContext;
			}
				
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public class WINTRUST_FILE_INFO
			{
				public WINTRUST_FILE_INFO()
				{
					cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
				}

			    private uint cbStruct;
				[MarshalAs(UnmanagedType.LPTStr)]public string pcwszFilePath;
				public IntPtr hFile;
				public IntPtr pgKnownSubject;
			}
				
			public const int CRYPT_E_NO_MATCH = unchecked ((int) 0x80092009);
				
			public const int TRUST_E_SUBJECT_NOT_TRUSTED = unchecked ((int) 0x800B0004);
			public const int TRUST_E_NOSIGNATURE = unchecked ((int) 0x800B0100);
				
			public static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");
				
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
	}
}
#endif