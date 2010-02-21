Imports System.ComponentModel

Namespace Tools
    Public Class LocalizedAttributes
        <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)> _
        Public Class LocalizedCategoryAttribute
            Inherits CategoryAttribute

            Private Const MaxOrder As Integer = 10
            Private Order As Integer

            Public Sub New(ByVal value As String, Optional ByVal Order As Integer = 1)
                MyBase.New(value)
                If Order > LocalizedCategoryAttribute.MaxOrder Then
                    Me.Order = LocalizedCategoryAttribute.MaxOrder
                Else
                    Me.Order = Order
                End If
            End Sub

            Protected Overrides Function GetLocalizedString(ByVal value As String) As String
                Dim OrderPrefix As String = ""
                For x As Integer = 0 To LocalizedCategoryAttribute.MaxOrder - Me.Order
                    OrderPrefix &= vbTab
                Next

                Return OrderPrefix & My.Resources.ResourceManager.GetString(value)
            End Function
        End Class

        <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)> _
        Public Class LocalizedDisplayNameAttribute
            Inherits DisplayNameAttribute

            Private Localized As Boolean

            Public Sub New(ByVal value As String)
                MyBase.New(value)
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

            Public Sub New(ByVal value As String)
                MyBase.New(value)
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
