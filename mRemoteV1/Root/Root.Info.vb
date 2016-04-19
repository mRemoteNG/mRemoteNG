Imports System.ComponentModel
Imports mRemote3G.Tools

Namespace Root
    <DefaultProperty("Name")>
    Public Class Info

#Region "Constructors"

        Public Sub New(rootType As RootType)
            Type = rootType
        End Sub

#End Region

#Region "Public Properties"

        Private _name As String = Language.Language.strConnections

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            Browsable(True),
            LocalizedAttributes.LocalizedDefaultValue("strConnections"),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")>
        Public Overridable Property Name As String
            Get
                Return _name
            End Get
            Set
                If _name = value Then Return
                _name = value
                If TreeNode IsNot Nothing Then
                    TreeNode.Name = value
                    TreeNode.Text = value
                End If
            End Set
        End Property

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            Browsable(True),
            LocalizedAttributes.LocalizedDisplayName("strPasswordProtect"),
            TypeConverter(GetType(Misc.YesNoTypeConverter))>
        Public Property Password As Boolean

        <Browsable(False)>
        Public Property PasswordString As String

        <Browsable(False)>
        Public Property Type As RootType

        <Browsable(False)>
        Public Property TreeNode As TreeNode

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

