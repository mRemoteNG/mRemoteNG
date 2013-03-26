Imports mRemoteNG.My

Namespace Tools
    Public Class PuttyProcessController
        Inherits ProcessController
        Public Overloads Function Start(Optional ByVal arguments As CommandLineArguments = Nothing) As Boolean
            Dim filename As String
            If Settings.UseCustomPuttyPath Then
                filename = Settings.CustomPuttyPath
            Else
                filename = App.Info.General.PuttyPath
            End If
            Return Start(filename, arguments)
        End Function
    End Class
End Namespace

