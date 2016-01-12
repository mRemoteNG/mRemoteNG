Imports System.Windows.Forms
Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Root
    <DefaultProperty("Name")> _
    Public Class Info
#Region "Constructors"
        Public Sub New(ByVal rootType As RootType)
            Type = rootType
        End Sub
#End Region

#Region "Public Properties"
        Private _name As String = My.Language.strConnections
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDefaultValue("strConnections"), _
            LocalizedDisplayName("strPropertyNameName"), _
            LocalizedDescription("strPropertyDescriptionName")> _
        Public Overridable Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                If _name = value Then Return
                _name = value
                If TreeNode IsNot Nothing Then
                    TreeNode.Name = value
                    TreeNode.Text = value
                End If
            End Set
        End Property

        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDisplayName("strPasswordProtect"), _
            TypeConverter(GetType(Tools.Misc.YesNoTypeConverter))> _
        Public Property Password() As Boolean

        <Browsable(False)> _
        Public Property PasswordString() As String

        <Browsable(False)> _
        Public Property Type() As RootType

        <Browsable(False)> _
        Public Property TreeNode() As TreeNode
#End Region

#Region "Public Enumerations"
        Public Enum RootType
            Connection
            Credential
            PuttySessions
        End Enum
#End Region
    End Class
End Namespace

