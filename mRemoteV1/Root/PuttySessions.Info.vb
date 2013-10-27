Imports mRemoteNG.My
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Root
    Namespace PuttySessions
        Public Class Info
            Inherits Root.Info

            Public Sub New()
                MyBase.New(RootType.PuttySessions)
            End Sub

            Private _name As String
            Public Overrides Property Name() As String
                Get
                    Return _name
                End Get
                Set(ByVal value As String)
                    If _name = value Then Return
                    _name = value
                    If TreeNode IsNot Nothing Then
                        TreeNode.Text = value
                    End If
                    Settings.PuttySavedSessionsName = value
                End Set
            End Property

            Private _panel As String = My.Language.strGeneral
            <LocalizedCategory("strCategoryDisplay", 1), _
            LocalizedDisplayName("strPropertyNamePanel"), _
            LocalizedDescription("strPropertyDescriptionPanel")> _
            Public Property Panel() As String
                Get
                    Return _panel
                End Get
                Set(ByVal value As String)
                    If _panel = value Then Return
                    _panel = value
                    Settings.PuttySavedSessionsPanel = value
                End Set
            End Property
        End Class
    End Namespace
End Namespace

