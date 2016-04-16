Imports mRemote3G.Connection.Protocol

Namespace Tools
    Public Class PuttyTypeDetector
        Public Shared Function GetPuttyType() As PuttyType
            Return GetPuttyType(PuttyBase.PuttyPath)
        End Function

        Public Shared Function GetPuttyType(filename As String) As PuttyType
            If IsPuttyNg(filename) Then Return PuttyType.PuttyNg
            If IsKitty(filename) Then Return PuttyType.Kitty
            If IsXming(filename) Then Return PuttyType.Xming

            ' Check this last
            If IsPutty(filename) Then Return PuttyType.Putty

            Return PuttyType.Unknown
        End Function

        Private Shared Function IsPutty(filename As String) As Boolean
            Dim result As Boolean
            Try
                result = FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY")
            Catch
                result = False
            End Try
            Return result
        End Function

        Private Shared Function IsPuttyNg(filename As String) As Boolean
            Dim result As Boolean
            Try
                result = FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTYNG")
            Catch
                result = False
            End Try
            Return result
        End Function

        Private Shared Function IsKitty(filename As String) As Boolean
            Dim result As Boolean
            Try
                result = FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY") _
                         And FileVersionInfo.GetVersionInfo(filename).Comments.Contains("KiTTY")
            Catch
                result = False
            End Try
            Return result
        End Function

        Private Shared Function IsXming(filename As String) As Boolean
            Dim result As Boolean
            Try
                result = FileVersionInfo.GetVersionInfo(filename).InternalName.Contains("PuTTY") _
                         And FileVersionInfo.GetVersionInfo(filename).ProductVersion.Contains("Xming")
            Catch
                result = False
            End Try
            Return result
        End Function

        Public Enum PuttyType
            Unknown = 0
            Putty
            PuttyNg
            Kitty
            Xming
        End Enum

        Private Sub New()
        End Sub
    End Class
End Namespace