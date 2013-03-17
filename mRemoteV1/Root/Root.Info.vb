Imports System.Windows.Forms
Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Root
    <DefaultProperty("Name")> _
    Public Class Info
        Public Sub New(ByVal typ As RootType)
            _Type = typ
        End Sub

#Region "Properties"
        Private _Name As String = My.Language.strConnections
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            [ReadOnly](False), _
            Bindable(False), _
            DefaultValue(""), _
            DesignOnly(False), _
            LocalizedDisplayName("strPropertyNameName"), _
            LocalizedDescription("strPropertyDescriptionName"), _
            Attributes.Root()> _
        Public Overridable Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        Private _Password As Boolean
        <LocalizedCategory("strCategoryDisplay", 1), _
           Browsable(True), _
           [ReadOnly](False), _
           Bindable(False), _
           DefaultValue(""), _
           DesignOnly(False), _
           LocalizedDisplayName("strPasswordProtect"), _
           Attributes.Root(), _
           TypeConverter(GetType(mRemoteNG.Tools.Misc.YesNoTypeConverter))> _
        Public Property Password() As Boolean
            Get
                Return _Password
            End Get
            Set(ByVal value As Boolean)
                _Password = value
            End Set
        End Property

        Private _PasswordString As String
        <Category(""), _
           Browsable(False), _
           [ReadOnly](False), _
           Bindable(False), _
           DefaultValue(""), _
           DesignOnly(False)> _
        Public Property PasswordString() As String
            Get
                Return _PasswordString
            End Get
            Set(ByVal value As String)
                _PasswordString = value
            End Set
        End Property





        Private _Type As Root.Info.RootType = RootType.Connection
        <Category(""), _
          Browsable(False), _
          [ReadOnly](False), _
          Bindable(False), _
          DefaultValue(""), _
          DesignOnly(False)> _
       Public Property Type() As Root.Info.RootType
            Get
                Return _Type
            End Get
            Set(ByVal value As Root.Info.RootType)
                _Type = value
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
#End Region

        Public Enum RootType
            Connection
            Credential
            PuttySessions
        End Enum

        Public Class Attributes
            Public Class Root
                Inherits Attribute
            End Class
        End Class
    End Class
End Namespace

