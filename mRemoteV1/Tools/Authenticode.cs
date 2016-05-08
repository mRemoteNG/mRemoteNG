using System;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Reflection;
using System.ComponentModel;


namespace mRemoteNG.Tools
{
	public class Authenticode
	{
        #region Public Methods
		public Authenticode(string filePath)
		{
			this.FilePath = filePath;
		}
			
		public StatusValue Verify()
		{
			IntPtr trustFileInfoPointer = default(IntPtr);
			IntPtr trustDataPointer = default(IntPtr);
			try
			{
				FileInfo fileInfo = new FileInfo(FilePath);
				if (!fileInfo.Exists)
				{
					_status = StatusValue.FileNotExist;
					return _status;
				}
				if (fileInfo.Length == 0)
				{
					_status = StatusValue.FileEmpty;
					return _status;
				}
					
				if (RequireThumbprintMatch)
				{
					if (string.IsNullOrEmpty(ThumbprintToMatch))
					{
						_status = StatusValue.NoThumbprintToMatch;
						return _status;
					}
						
					X509Certificate certificate = X509Certificate.CreateFromSignedFile(FilePath);
					X509Certificate2 certificate2 = new X509Certificate2(certificate);
					_thumbprint = certificate2.Thumbprint;
					if (!(_thumbprint == ThumbprintToMatch))
					{
						_status = StatusValue.ThumbprintNotMatch;
						return _status;
					}
				}
					
				Win32.WINTRUST_FILE_INFO trustFileInfo = new Win32.WINTRUST_FILE_INFO();
				trustFileInfo.pcwszFilePath = FilePath;
				trustFileInfoPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustFileInfo));
				Marshal.StructureToPtr(trustFileInfo, trustFileInfoPointer, false);
					
				Win32.WINTRUST_DATA trustData = new Win32.WINTRUST_DATA();
				trustData.dwUIChoice = (uint)Display;
				trustData.fdwRevocationChecks = Win32.WTD_REVOKE_WHOLECHAIN;
				trustData.dwUnionChoice = Win32.WTD_CHOICE_FILE;
				trustData.pFile = trustFileInfoPointer;
				trustData.dwStateAction = Win32.WTD_STATEACTION_IGNORE;
				trustData.dwProvFlags = Win32.WTD_DISABLE_MD2_MD4;
				trustData.dwUIContext = (uint)DisplayContext;
				trustDataPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustData));
				Marshal.StructureToPtr(trustData, trustDataPointer, false);
					
				IntPtr windowHandle = default(IntPtr);
				if (DisplayParentForm == null)
				{
					windowHandle = IntPtr.Zero;
				}
				else
				{
					windowHandle = DisplayParentForm.Handle;
				}
					
				_trustProviderErrorCode = Win32.WinVerifyTrust(windowHandle, Win32.WINTRUST_ACTION_GENERIC_VERIFY_V2, trustDataPointer);
				switch (_trustProviderErrorCode)
				{
					case Win32.TRUST_E_NOSIGNATURE:
						_status = StatusValue.NoSignature;
						break;
					case Win32.TRUST_E_SUBJECT_NOT_TRUSTED:
						break;
							
				}
				if (!(_trustProviderErrorCode == 0))
				{
					_status = StatusValue.TrustProviderError;
					return _status;
				}
					
				_status = StatusValue.Verified;
				return _status;
			}
			catch (CryptographicException ex)
			{
				PropertyInfo hResultProperty = ex.GetType().GetProperty("HResult", BindingFlags.NonPublic | BindingFlags.Instance);
				int hResult = Convert.ToInt32(hResultProperty.GetValue(ex, null));
				if (hResult == Win32.CRYPT_E_NO_MATCH)
				{
					_status = StatusValue.NoSignature;
					return _status;
				}
				else
				{
					_status = StatusValue.UnhandledException;
					Exception = ex;
					return _status;
				}
			}
			catch (Exception ex)
			{
				_status = StatusValue.UnhandledException;
				Exception = ex;
				return _status;
			}
			finally
			{
				if (!(trustDataPointer == IntPtr.Zero))
				{
					Marshal.FreeCoTaskMem(trustDataPointer);
				}
				if (!(trustFileInfoPointer == IntPtr.Zero))
				{
					Marshal.FreeCoTaskMem(trustFileInfoPointer);
				}
			}
		}
        #endregion
			
        #region Public Properties
		private DisplayValue _Display = DisplayValue.None;
        public DisplayValue Display
		{
			get { return _Display; }
			set { _Display = value; }
		}
		public DisplayContextValue DisplayContext {get; set;}
		public Form DisplayParentForm {get; set;}
		public Exception Exception {get; set;}
		public string FilePath {get; set;}
		public bool RequireThumbprintMatch {get; set;}
			
		private StatusValue _status;
        public StatusValue Status
		{
			get { return _status; }
		}
			
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
						return string.Format("The thumbprint does not match. {0} {1} {2}.", _thumbprint, Strings.ChrW(0x2260), ThumbprintToMatch);
					case StatusValue.TrustProviderError:
						Win32Exception ex = new Win32Exception(_trustProviderErrorCode);
						return string.Format("The trust provider returned an error. {0}", ex.Message);
					case StatusValue.UnhandledException:
						return string.Format("An unhandled exception occurred. {0}", Exception.Message);
					default:
						return "The status is unknown.";
				}
			}
		}
			
		private string _thumbprint;
        public string Thumbprint
		{
			get { return _thumbprint; }
		}
			
		public string ThumbprintToMatch {get; set;}
			
		private int _trustProviderErrorCode;
        public int TrustProviderErrorCode
		{
			get { return _trustProviderErrorCode; }
		}
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
			
        #region Protected Classes
		protected class Win32
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
				public UInt32 cbStruct;
				public IntPtr pPolicyCallbackData;
				public IntPtr pSIPClientData;
				public UInt32 dwUIChoice;
				public UInt32 fdwRevocationChecks;
				public UInt32 dwUnionChoice;
				public IntPtr pFile;
				public UInt32 dwStateAction;
				public IntPtr hWVTStateData;
				public IntPtr pwszURLReference;
				public UInt32 dwProvFlags;
				public UInt32 dwUIContext;
			}
				
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public class WINTRUST_FILE_INFO
			{
				public WINTRUST_FILE_INFO()
				{
					cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
				}
				public UInt32 cbStruct;
				[MarshalAs(UnmanagedType.LPTStr)]public string pcwszFilePath;
				public IntPtr hFile;
				public IntPtr pgKnownSubject;
			}
				
			public const int CRYPT_E_NO_MATCH = unchecked ((int) 0x80092009);
				
			public const int TRUST_E_SUBJECT_NOT_TRUSTED = unchecked ((int) 0x800B0004);
			public const int TRUST_E_NOSIGNATURE = unchecked ((int) 0x800B0100);
				
			public static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");
				
			public const UInt32 WTD_CHOICE_FILE = 1;
			public const UInt32 WTD_DISABLE_MD2_MD4 = 0x2000;
			public const UInt32 WTD_REVOKE_WHOLECHAIN = 1;
				
			public const UInt32 WTD_STATEACTION_IGNORE = 0x0;
			public const UInt32 WTD_STATEACTION_VERIFY = 0x1;
			public const UInt32 WTD_STATEACTION_CLOSE = 0x2;
				
			public const UInt32 WTD_UI_ALL = 1;
			public const UInt32 WTD_UI_NONE = 2;
			public const UInt32 WTD_UI_NOBAD = 3;
			public const UInt32 WTD_UI_NOGOOD = 4;
				
			public const UInt32 WTD_UICONTEXT_EXECUTE = 0;
			public const UInt32 WTD_UICONTEXT_INSTALL = 1;
			// ReSharper restore InconsistentNaming
		}
        #endregion
	}
}