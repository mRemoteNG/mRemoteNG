Imports System.ComponentModel
Imports System.IO
Imports mRemote3G.App
Imports mRemote3G.Messages

Namespace Connection
    Public Class Icon
        Inherits StringConverter

        Public Shared Icons As String() = New String() {}

        Public Overloads Overrides Function GetStandardValues(context As ITypeDescriptorContext) _
            As StandardValuesCollection
            Return New StandardValuesCollection(Icons)
        End Function

        Public Overloads Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Shared Function FromString(IconName As String) As Drawing.Icon
            Try
                Dim IconPath As String = My.Application.Info.DirectoryPath & "\Icons\" & IconName & ".ico"

                If File.Exists(IconPath) Then
                    Dim nI As New Drawing.Icon(IconPath)

                    Return nI
                End If
            Catch ex As Exception
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                    "Couldn't get Icon from String" & vbNewLine & ex.ToString())
            End Try

            Return Nothing
        End Function
    End Class
End Namespace