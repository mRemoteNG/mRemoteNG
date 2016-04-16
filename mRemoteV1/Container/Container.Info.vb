Imports System.ComponentModel
Imports mRemote3G.Tools

Namespace Container
    <DefaultProperty("Name")>
    Public Class Info

#Region "Properties"

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            Browsable(True),
            [ReadOnly](False),
            Bindable(False),
            DefaultValue(""),
            DesignOnly(False),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName"),
            Attributes.Container>
        Public Property Name As String
            Get
                Return ConnectionInfo.Name
            End Get
            Set
                ConnectionInfo.Name = value
            End Set
        End Property

        Private _TreeNode As TreeNode

        <Category(""),
            Browsable(False),
            [ReadOnly](False),
            Bindable(False),
            DefaultValue(""),
            DesignOnly(False)>
        Public Property TreeNode As TreeNode
            Get
                Return Me._TreeNode
            End Get
            Set
                Me._TreeNode = value
            End Set
        End Property

        Private _Parent As Object

        <Category(""),
            Browsable(False)>
        Public Property Parent As Object
            Get
                Return Me._Parent
            End Get
            Set
                Me._Parent = value
            End Set
        End Property

        'Private _GlobalID As Integer = 0
        '<Category(""), _
        '    Browsable(False)> _
        'Public Property GlobalID() As Integer
        '    Get
        '        Return _GlobalID
        '    End Get
        '    Set(ByVal value As Integer)
        '        _GlobalID = value
        '    End Set
        'End Property

        Private _IsExpanded As Boolean

        <Category(""),
            Browsable(False),
            [ReadOnly](False),
            Bindable(False),
            DefaultValue(""),
            DesignOnly(False)>
        Public Property IsExpanded As Boolean
            Get
                Return Me._IsExpanded
            End Get
            Set
                Me._IsExpanded = value
            End Set
        End Property

        Private _ConnectionInfo As New Connection.Info

        Public Property ConnectionInfo As Connection.Info
            Get
                Return Me._ConnectionInfo
            End Get
            Set
                Me._ConnectionInfo = value
            End Set
        End Property

#End Region

#Region "Methods"

        Public Function Copy() As Info
            Return Me.MemberwiseClone
        End Function

        Public Sub New()
            Me.SetDefaults()
        End Sub

        Public Sub SetDefaults()
            If Me.IsExpanded = Nothing Then
                Me.IsExpanded = True
            End If
        End Sub

#End Region

        Public Class Attributes
            Public Class Container
                Inherits Attribute
            End Class

            Private Sub New()
            End Sub
        End Class
    End Class
End Namespace
