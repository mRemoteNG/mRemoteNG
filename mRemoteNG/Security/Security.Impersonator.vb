Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions
Imports mRemoteNG.App.Runtime

<Assembly: SecurityPermissionAttribute(SecurityAction.RequestMinimum, UnmanagedCode:=True), _
Assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name:="FullTrust")> 

Namespace Security
    Public Class Impersonator
#Region "Logon API"
        Private Const LOGON32_PROVIDER_DEFAULT As Integer = 0
        Private Const LOGON32_LOGON_NEW_CREDENTIALS As Integer = 9

        Private Const SecurityImpersonation As Integer = 2

        Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String], _
        ByVal lpszDomain As [String], ByVal lpszPassword As [String], _
        ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
        ByRef phToken As IntPtr) As Integer

        Private Const FORMAT_MESSAGE_ALLOCATE_BUFFER As Integer = &H100
        Private Const FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
        Private Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000

        <DllImport("kernel32.dll")> _
        Private Shared Function FormatMessage(ByVal dwFlags As Integer, ByRef lpSource As IntPtr, _
        ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByRef lpBuffer As [String], _
        ByVal nSize As Integer, ByRef Arguments As IntPtr) As Integer
        End Function

        Private Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean

        Private Declare Auto Function DuplicateToken Lib "advapi32.dll" (ByVal ExistingTokenHandle As IntPtr, _
        ByVal SECURITY_IMPERSONATION_LEVEL As Integer, _
        ByRef DuplicateTokenHandle As IntPtr) As Integer
#End Region

        Private tokenHandle As New IntPtr(0)
        Private impersonatedUser As WindowsImpersonationContext = Nothing

        ' GetErrorMessage formats and returns an error message corresponding to the input errorCode.
        Private Function GetErrorMessage(ByVal errorCode As Integer) As String
            Dim messageSize As Integer = 255
            Dim lpMsgBuf As String = ""
            Dim dwFlags As Integer = FORMAT_MESSAGE_ALLOCATE_BUFFER Or FORMAT_MESSAGE_FROM_SYSTEM Or FORMAT_MESSAGE_IGNORE_INSERTS

            Dim ptrlpSource As IntPtr = IntPtr.Zero
            Dim prtArguments As IntPtr = IntPtr.Zero

            Dim retVal As Integer = FormatMessage(dwFlags, ptrlpSource, errorCode, 0, lpMsgBuf, messageSize, prtArguments)
            Return lpMsgBuf.Trim(New Char() {CChar(vbCr), CChar(vbLf)})
        End Function

        <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")> _
        Public Sub StartImpersonation(ByVal DomainName As String, ByVal UserName As String, ByVal Password As String)
            Try
                If Not (impersonatedUser Is Nothing) Then Throw New Exception("Already impersonating a user.")

                tokenHandle = IntPtr.Zero

                Dim returnValue As Integer = LogonUser(UserName, DomainName, Password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, tokenHandle)

                If 0 = returnValue Then
                    Dim errCode As Integer = Marshal.GetLastWin32Error()
                    Dim errMsg As String = "LogonUser failed with error code: " + errCode.ToString() + "(" + GetErrorMessage(errCode) + ")"
                    Dim exLogon As New ApplicationException(errMsg)
                    Throw exLogon
                End If

                Dim newId As New WindowsIdentity(tokenHandle)
                impersonatedUser = newId.Impersonate()
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Starting Impersonation failed (Sessions feature will not work)" & vbNewLine & ex.Message, True)
            End Try
        End Sub

        <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")> _
        Public Sub StopImpersonation()
            If impersonatedUser Is Nothing Then Exit Sub

            Try
                impersonatedUser.Undo() ' Stop impersonating the user.
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, "Stopping Impersonation failed" & vbNewLine & ex.Message, True)
                Throw
            Finally

                If Not System.IntPtr.op_Equality(tokenHandle, IntPtr.Zero) Then CloseHandle(tokenHandle)

                impersonatedUser = Nothing
            End Try
        End Sub
    End Class
End Namespace
