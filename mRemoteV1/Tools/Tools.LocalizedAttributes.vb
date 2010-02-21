Imports System.ComponentModel

Namespace Tools
    Public Class LocalizedAttributes
        <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)> _
        Public Class LocalizedDisplayNameAttribute
            Inherits DisplayNameAttribute

            Private Localized As Boolean

            Public Sub New(ByVal Text As String)
                MyBase.New(Text)
                Me.Localized = False
            End Sub

            Public Overrides ReadOnly Property DisplayName() As String
                Get
                    If Not Me.Localized Then
                        Me.Localized = True
                        Me.DisplayNameValue = My.Resources.ResourceManager.GetString(Me.DisplayNameValue)
                    End If

                    Return MyBase.DisplayName
                End Get
            End Property
        End Class

        <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)> _
        Public Class LocalizedDescriptionAttribute
            Inherits DescriptionAttribute

            Private Localized As Boolean

            Public Sub New(ByVal Text As String)
                MyBase.New(Text)
                Me.Localized = False
            End Sub

            Public Overrides ReadOnly Property Description() As String
                Get
                    If Not Me.Localized Then
                        Me.Localized = True
                        Me.DescriptionValue = My.Resources.ResourceManager.GetString(Me.DescriptionValue)
                    End If

                    Return MyBase.Description
                End Get
            End Property
        End Class
    End Class
End Namespace
