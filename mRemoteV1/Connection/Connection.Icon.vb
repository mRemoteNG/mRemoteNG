Imports System.ComponentModel
Imports mRemoteNG.App.Runtime

Namespace Connection
    Public Class Icon
        Inherits StringConverter

        Public Shared Icons As String() = New String() {}

        Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
            Return New StandardValuesCollection(Icons)
        End Function

        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Shared Function FromString(ByVal IconName As String) As Drawing.Icon
            Try
                Dim IconPath As String = My.Application.Info.DirectoryPath & "\Icons\" & IconName & ".ico"

                If IO.File.Exists(IconPath) Then
                    Dim nI As New Drawing.Icon(IconPath)

                    Return nI
                End If
            Catch ex As Exception
                MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn't get Icon from String" & vbNewLine & ex.Message)
            End Try

            Return Nothing
        End Function
    End Class
End Namespace