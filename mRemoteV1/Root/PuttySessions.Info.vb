Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Root
    Namespace PuttySessions
        Public Class Info
            Inherits Root.Info

            Public Sub New()
                MyBase.New(RootType.PuttySessions)
            End Sub

            Public Overrides Property Name() As String

            Private _panel As String = My.Language.strGeneral
            <LocalizedCategory("strCategoryDisplay", 1), _
            LocalizedDisplayName("strPropertyNamePanel"), _
            LocalizedDescription("strPropertyDescriptionPanel")> _
            Public Property Panel() As String
                Get
                    Return _panel
                End Get
                Set(ByVal value As String)
                    _panel = value
                End Set
            End Property
        End Class
    End Namespace
End Namespace

