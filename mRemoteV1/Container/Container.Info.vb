Imports System.Windows.Forms
Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Container
    <DefaultProperty("Name")> _
    Public Class Info
#Region "Properties"
        Private _Name As String = "New Container"
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            [ReadOnly](False), _
            Bindable(False), _
            DefaultValue(""), _
            DesignOnly(False), _
            LocalizedDisplayName("strPropertyNameName"), _
            LocalizedDescription("strPropertyDescriptionName"), _
            Attributes.Container()> _
        Public Property Name() As String
            Get
                Return Me._ConnectionInfo.Name
            End Get
            Set(ByVal value As String)
                Me._ConnectionInfo.Name = value
            End Set
        End Property

        Private _TreeNode As TreeNode
        <Category(""), _
           Browsable(False), _
           [ReadOnly](False), _
           Bindable(False), _
           DefaultValue(""), _
           DesignOnly(False)> _
        Public Property TreeNode() As TreeNode
            Get
                Return Me._TreeNode
            End Get
            Set(ByVal value As TreeNode)
                Me._TreeNode = value
            End Set
        End Property

        Private _Parent As Object
        <Category(""), _
            Browsable(False)> _
        Public Property Parent() As Object
            Get
                Return Me._Parent
            End Get
            Set(ByVal value As Object)
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
        <Category(""), _
           Browsable(False), _
           [ReadOnly](False), _
           Bindable(False), _
           DefaultValue(""), _
           DesignOnly(False)> _
        Public Property IsExpanded() As Boolean
            Get
                Return Me._IsExpanded
            End Get
            Set(ByVal value As Boolean)
                Me._IsExpanded = value
            End Set
        End Property

        Private _ConnectionInfo As New Connection.Info
        Public Property ConnectionInfo() As Connection.Info
            Get
                Return Me._ConnectionInfo
            End Get
            Set(ByVal value As Connection.Info)
                Me._ConnectionInfo = value
            End Set
        End Property
#End Region

#Region "Methods"
        Public Function Copy() As Container.Info
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
        End Class
    End Class
End Namespace
