Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates

Namespace Tools
    Public Class Authenticode

#Region "Public Methods"

        Public Sub New(filePath As String)
            Me.FilePath = filePath
        End Sub

        Public Function Verify() As StatusValue
            Dim trustFileInfoPointer As IntPtr
            Dim trustDataPointer As IntPtr
            Try
                Dim fileInfo As New FileInfo(FilePath)
                If Not fileInfo.Exists() Then
                    _status = StatusValue.FileNotExist
                    Return _status
                End If
                If fileInfo.Length = 0 Then
                    _status = StatusValue.FileEmpty
                    Return _status
                End If

                If RequireThumbprintMatch Then
                    If String.IsNullOrEmpty(ThumbprintToMatch) Then
                        _status = StatusValue.NoThumbprintToMatch
                        Return _status
                    End If

                    Dim certificate As X509Certificate = X509Certificate.CreateFromSignedFile(FilePath)
                    Dim certificate2 As New X509Certificate2(certificate)
                    _thumbprint = certificate2.Thumbprint
                    If Not _thumbprint = ThumbprintToMatch Then
                        _status = StatusValue.ThumbprintNotMatch
                        Return _status
                    End If
                End If

                Dim trustFileInfo As New Win32.WINTRUST_FILE_INFO
                trustFileInfo.pcwszFilePath = FilePath
                trustFileInfoPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustFileInfo))
                Marshal.StructureToPtr(trustFileInfo, trustFileInfoPointer, False)

                Dim trustData As New Win32.WINTRUST_DATA
                With trustData
                    .dwUIChoice = Display
                    .fdwRevocationChecks = Win32.WTD_REVOKE_WHOLECHAIN
                    .dwUnionChoice = Win32.WTD_CHOICE_FILE
                    .pFile = trustFileInfoPointer
                    .dwStateAction = Win32.WTD_STATEACTION_IGNORE
                    .dwProvFlags = Win32.WTD_DISABLE_MD2_MD4
                    .dwUIContext = DisplayContext
                End With
                trustDataPointer = Marshal.AllocCoTaskMem(Marshal.SizeOf(trustData))
                Marshal.StructureToPtr(trustData, trustDataPointer, False)

                Dim windowHandle As IntPtr
                If DisplayParentForm Is Nothing Then
                    windowHandle = IntPtr.Zero
                Else
                    windowHandle = DisplayParentForm.Handle
                End If

                _trustProviderErrorCode = Win32.WinVerifyTrust(windowHandle, Win32.WINTRUST_ACTION_GENERIC_VERIFY_V2,
                                                               trustDataPointer)
                Select Case _trustProviderErrorCode
                    Case Win32.TRUST_E_NOSIGNATURE
                        _status = StatusValue.NoSignature
                    Case Win32.TRUST_E_SUBJECT_NOT_TRUSTED

                End Select
                If Not _trustProviderErrorCode = 0 Then
                    _status = StatusValue.TrustProviderError
                    Return _status
                End If

                _status = StatusValue.Verified
                Return _status
            Catch ex As CryptographicException
                Dim hResultProperty As PropertyInfo = ex.GetType.GetProperty("HResult",
                                                                             BindingFlags.NonPublic Or
                                                                             BindingFlags.Instance)
                Dim hResult As Integer = hResultProperty.GetValue(ex, Nothing)
                If hResult = Win32.CRYPT_E_NO_MATCH Then
                    _status = StatusValue.NoSignature
                    Return _status
                Else
                    _status = StatusValue.UnhandledException
                    Exception = ex
                    Return _status
                End If
            Catch ex As Exception
                _status = StatusValue.UnhandledException
                Exception = ex
                Return _status
            Finally
                If Not trustDataPointer = IntPtr.Zero Then Marshal.FreeCoTaskMem(trustDataPointer)
                If Not trustFileInfoPointer = IntPtr.Zero Then Marshal.FreeCoTaskMem(trustFileInfoPointer)
            End Try
        End Function

#End Region

#Region "Public Properties"

        Public Property Display As DisplayValue = DisplayValue.None
        Public Property DisplayContext As DisplayContextValue
        Public Property DisplayParentForm As Form
        Public Property Exception As Exception
        Public Property FilePath As String
        Public Property RequireThumbprintMatch As Boolean

        Private _status As StatusValue

        Public ReadOnly Property Status As StatusValue
            Get
                Return _status
            End Get
        End Property

        Public ReadOnly Property StatusMessage As String
            Get
                Select Case Status
                    Case StatusValue.Verified
                        Return "The file was verified successfully."
                    Case StatusValue.FileNotExist
                        Return "The specified file does not exist."
                    Case StatusValue.FileEmpty
                        Return "The specified file is empty."
                    Case StatusValue.NoSignature
                        Return "The specified file is not digitally signed."
                    Case StatusValue.NoThumbprintToMatch
                        Return "A thumbprint match is required but no thumbprint to match against was specified."
                    Case StatusValue.ThumbprintNotMatch
                        Return _
                            String.Format("The thumbprint does not match. {0} {1} {2}.", _thumbprint, ChrW(&H2260),
                                          ThumbprintToMatch)
                    Case StatusValue.TrustProviderError
                        Dim ex As New InvalidOperationException(_trustProviderErrorCode)
                        Return String.Format("The trust provider returned an error. {0}", ex.ToString())
                    Case StatusValue.UnhandledException
                        Return String.Format("An unhandled exception occurred. {0}", Exception.Message)
                    Case Else
                        Return "The status is unknown."
                End Select
            End Get
        End Property

        Private _thumbprint As String

        Public ReadOnly Property Thumbprint As String
            Get
                Return _thumbprint
            End Get
        End Property

        Public Property ThumbprintToMatch As String

        Private _trustProviderErrorCode As Integer

        Public ReadOnly Property TrustProviderErrorCode As Integer
            Get
                Return _trustProviderErrorCode
            End Get
        End Property

#End Region

#Region "Public Enums"

        Public Enum DisplayValue
            Unknown = 0
            All = Win32.WTD_UI_ALL
            None = Win32.WTD_UI_NONE
            NoBad = Win32.WTD_UI_NOBAD
            NoGood = Win32.WTD_UI_NOGOOD
        End Enum

        Public Enum DisplayContextValue
            Execute = Win32.WTD_UICONTEXT_EXECUTE
            Install = Win32.WTD_UICONTEXT_INSTALL
        End Enum

        Public Enum StatusValue
            Unknown = 0
            Verified
            FileNotExist
            FileEmpty
            NoSignature
            NoThumbprintToMatch
            ThumbprintNotMatch
            TrustProviderError
            UnhandledException
        End Enum

#End Region

#Region "Protected Classes"

        Protected Class Win32
            ' ReSharper disable InconsistentNaming
            <DllImport("wintrust.dll", CharSet := CharSet.Auto, SetLastError := False)>
            Public Shared Function WinVerifyTrust(<[In]> hWnd As IntPtr,
                                                  <[In], MarshalAs(UnmanagedType.LPStruct)> pgActionOID As Guid,
                                                  <[In]> pWVTData As IntPtr) As Integer
            End Function

            <StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)>
            Public Class WINTRUST_DATA
                Public cbStruct As UInt32 = Marshal.SizeOf(GetType(WINTRUST_DATA))
                Public pPolicyCallbackData As IntPtr
                Public pSIPClientData As IntPtr
                Public dwUIChoice As UInt32
                Public fdwRevocationChecks As UInt32
                Public dwUnionChoice As UInt32
                Public pFile As IntPtr
                Public dwStateAction As UInt32
                Public hWVTStateData As IntPtr
                Public pwszURLReference As IntPtr
                Public dwProvFlags As UInt32
                Public dwUIContext As UInt32
            End Class

            <StructLayout(LayoutKind.Sequential, CharSet := CharSet.Unicode)>
            Public Class WINTRUST_FILE_INFO
                Public cbStruct As UInt32 = Marshal.SizeOf(GetType(WINTRUST_FILE_INFO))
                <MarshalAs(UnmanagedType.LPTStr)> Public pcwszFilePath As String
                Public hFile As IntPtr
                Public pgKnownSubject As IntPtr
            End Class

            Public Const CRYPT_E_NO_MATCH As Integer = &H80092009

            Public Const TRUST_E_SUBJECT_NOT_TRUSTED As Integer = &H800B0004
            Public Const TRUST_E_NOSIGNATURE As Integer = &H800B0100

            Public Shared ReadOnly _
                WINTRUST_ACTION_GENERIC_VERIFY_V2 As New Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}")

            Public Const WTD_CHOICE_FILE As UInt32 = 1
            Public Const WTD_DISABLE_MD2_MD4 As UInt32 = &H2000
            Public Const WTD_REVOKE_WHOLECHAIN As UInt32 = 1

            Public Const WTD_STATEACTION_IGNORE As UInt32 = &H0
            Public Const WTD_STATEACTION_VERIFY As UInt32 = &H1
            Public Const WTD_STATEACTION_CLOSE As UInt32 = &H2

            Public Const WTD_UI_ALL As UInt32 = 1
            Public Const WTD_UI_NONE As UInt32 = 2
            Public Const WTD_UI_NOBAD As UInt32 = 3
            Public Const WTD_UI_NOGOOD As UInt32 = 4

            Public Const WTD_UICONTEXT_EXECUTE As UInt32 = 0
            Public Const WTD_UICONTEXT_INSTALL As UInt32 = 1
            ' ReSharper restore InconsistentNaming

            Private Sub New()
            End Sub
        End Class

#End Region

        Private Sub New()
        End Sub
    End Class
End Namespace