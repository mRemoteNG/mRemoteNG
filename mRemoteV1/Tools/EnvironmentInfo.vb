Imports System.Runtime.InteropServices

Namespace Tools
    Public Class EnvironmentInfo
        Public Shared ReadOnly Property IsWow64 As Boolean
            Get
                Dim isWow64ProcessDelegate As Win32.IsWow64ProcessDelegate = GetIsWow64ProcessDelegate()
                If isWow64ProcessDelegate Is Nothing Then Return False

                Dim isWow64Process As Boolean
                Dim result As Boolean = isWow64ProcessDelegate.Invoke(Process.GetCurrentProcess().Handle, isWow64Process)
                If Not result Then Return False

                Return isWow64Process
            End Get
        End Property

        Private Shared Function GetIsWow64ProcessDelegate() As Win32.IsWow64ProcessDelegate
            Dim moduleHandle As IntPtr = Win32.LoadLibrary("kernel32")
            If moduleHandle = IntPtr.Zero Then Return Nothing

            Dim functionPointer As IntPtr = Win32.GetProcAddress(moduleHandle, "IsWow64Process")
            If functionPointer = IntPtr.Zero Then Return Nothing

            Return Marshal.GetDelegateForFunctionPointer(functionPointer, GetType(Win32.IsWow64ProcessDelegate))
        End Function

        Protected Class Win32
            ' ReSharper disable InconsistentNaming
            <DllImport("kernel32", CharSet := CharSet.Unicode, SetLastError := True)>
            Public Shared Function LoadLibrary(<[In], MarshalAs(UnmanagedType.LPTStr)> lpFileName As String) As IntPtr
            End Function

            <DllImport("kernel32", ExactSpelling := True, CharSet := CharSet.Unicode, SetLastError := True)>
            Public Shared Function GetProcAddress(<[In]> hModule As IntPtr,
                                                  <[In], MarshalAs(UnmanagedType.LPStr)> lpProcName As String) As IntPtr
            End Function

            Public Delegate Function IsWow64ProcessDelegate _
                (<[In]> hProcess As IntPtr, <[Out]> ByRef Wow64Process As Boolean) As Boolean
            ' ReSharper restore InconsistentNaming

            Private Sub New()
            End Sub
        End Class

        Private Sub New()
        End Sub
    End Class
End Namespace

