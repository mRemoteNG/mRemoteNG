Imports mRemoteNG.Connection.PuttySession

Namespace Config.Putty
    Public Class DefaultSettingsProvider
        Inherits Provider

        Public Overrides Function GetSessionNames(Optional ByVal raw As Boolean = False) As String()
            Dim sessionNames(0) As String
            If raw Then
                sessionNames(0) = "Default%20Settings"
            Else
                sessionNames(0) = "Default Settings"
            End If
            Return sessionNames
        End Function

        Public Overrides Function GetSession(ByVal sessionName As String) As Info
            Return Nothing
        End Function
    End Class
End Namespace
