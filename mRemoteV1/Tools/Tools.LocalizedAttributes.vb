Imports System.ComponentModel

Namespace Tools
    Public Class LocalizedAttributes
        <AttributeUsage(AttributeTargets.All, AllowMultiple := False, Inherited := True)>
        Public Class LocalizedCategoryAttribute
            Inherits CategoryAttribute

            Private Const MaxOrder As Integer = 10
            Private ReadOnly Order As Integer

            Public Sub New(value As String, Optional ByVal Order As Integer = 1)
                MyBase.New(value)
                If Order > MaxOrder Then
                    Me.Order = MaxOrder
                Else
                    Me.Order = Order
                End If
            End Sub

            Protected Overrides Function GetLocalizedString(value As String) As String
                Dim OrderPrefix = ""
                For x = 0 To MaxOrder - Me.Order
                    OrderPrefix &= vbTab
                Next

                Return OrderPrefix & Language.Language.ResourceManager.GetString(value)
            End Function
        End Class

        <AttributeUsage(AttributeTargets.All, AllowMultiple := False, Inherited := True)>
        Public Class LocalizedDisplayNameAttribute
            Inherits DisplayNameAttribute

            Private Localized As Boolean

            Public Sub New(value As String)
                MyBase.New(value)
                Me.Localized = False
            End Sub

            Public Overrides ReadOnly Property DisplayName As String
                Get
                    If Not Me.Localized Then
                        Me.Localized = True
                        Me.DisplayNameValue = Language.Language.ResourceManager.GetString(Me.DisplayNameValue)
                    End If

                    Return MyBase.DisplayName
                End Get
            End Property
        End Class

        <AttributeUsage(AttributeTargets.All, AllowMultiple := False, Inherited := True)>
        Public Class LocalizedDescriptionAttribute
            Inherits DescriptionAttribute

            Private Localized As Boolean

            Public Sub New(value As String)
                MyBase.New(value)
                Me.Localized = False
            End Sub

            Public Overrides ReadOnly Property Description As String
                Get
                    If Not Me.Localized Then
                        Me.Localized = True
                        Me.DescriptionValue = Language.Language.ResourceManager.GetString(Me.DescriptionValue)
                    End If

                    Return MyBase.Description
                End Get
            End Property
        End Class

        <AttributeUsage(AttributeTargets.All, AllowMultiple := False, Inherited := True)>
        Public Class LocalizedDefaultValueAttribute
            Inherits DefaultValueAttribute

            Public Sub New(name As String)
                MyBase.New(Language.Language.ResourceManager.GetString(name))
            End Sub

            ' This allows localized attributes in a derived class to override a matching
            ' non-localized attribute inherited from its base class
            Public Overrides ReadOnly Property TypeId As Object
                Get
                    Return GetType(DefaultValueAttribute)
                End Get
            End Property
        End Class

#Region "Special localization - with String.Format"

        <AttributeUsage(AttributeTargets.All, AllowMultiple := False, Inherited := True)>
        Public Class LocalizedDisplayNameInheritAttribute
            Inherits DisplayNameAttribute

            Private Localized As Boolean

            Public Sub New(value As String)
                MyBase.New(value)

                Me.Localized = False
            End Sub

            Public Overrides ReadOnly Property DisplayName As String
                Get
                    If Not Me.Localized Then
                        Me.Localized = True
                        Me.DisplayNameValue = String.Format(Language.Language.strFormatInherit,
                                                            Language.Language.ResourceManager.GetString(
                                                                Me.DisplayNameValue))
                    End If

                    Return MyBase.DisplayName
                End Get
            End Property
        End Class

        <AttributeUsage(AttributeTargets.All, AllowMultiple := False, Inherited := True)>
        Public Class LocalizedDescriptionInheritAttribute
            Inherits DescriptionAttribute

            Private Localized As Boolean

            Public Sub New(value As String)
                MyBase.New(value)

                Me.Localized = False
            End Sub

            Public Overrides ReadOnly Property Description As String
                Get
                    If Not Me.Localized Then
                        Me.Localized = True
                        Me.DescriptionValue = String.Format(Language.Language.strFormatInheritDescription,
                                                            Language.Language.ResourceManager.GetString(
                                                                Me.DescriptionValue))
                    End If

                    Return MyBase.Description
                End Get
            End Property
        End Class

#End Region

        Private Sub New()
        End Sub
    End Class
End Namespace
