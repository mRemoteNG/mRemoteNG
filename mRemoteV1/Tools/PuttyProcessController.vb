
Imports mRemote3G.App.Info

Namespace Tools
    Public Class PuttyProcessController
        Inherits ProcessController

        Public Overloads Function Start(Optional ByVal arguments As CommandLineArguments = Nothing) As Boolean
            Dim filename As String
            If My.Settings.UseCustomPuttyPath Then
                filename = My.Settings.CustomPuttyPath
            Else
                filename = General.PuttyPath
            End If
            Return Start(filename, arguments)
        End Function
    End Class
End Namespace

