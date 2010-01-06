Imports System.ComponentModel
Imports mRemote.Tools.Misc.PropertyGridCategory
Imports mRemote.Tools.Misc.PropertyGridValue

Namespace Credential
    Public Class Info
#Region "1 Display"
        Private _Name As String
        <Category(Category1 & "Display"), _
           Browsable(True), _
           DisplayName(Language.Base.Props_Name), _
           Description("Enter a name")> _
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Private _Description As String
        <Category(Category1 & "Description"), _
           Browsable(True), _
           DisplayName(Language.Base.Props_Description), _
           Description("Enter a description")> _
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
#End Region
#Region "2 Credentials"
        Private _Username As String
        <Category(Category2 & "Credentials"), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Username), _
            Description("Enter a username")> _
        Public Property Username() As String
            Get
                Return _Username
            End Get
            Set(ByVal value As String)
                _Username = value
            End Set
        End Property

        Private _Password As String
        <Category(Category2 & "Credentials"), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Password), _
            Description("Enter a password"), _
            PasswordPropertyText(True)> _
        Public Property Password() As String
            Get
                Return _Password
            End Get
            Set(ByVal value As String)
                _Password = value
            End Set
        End Property

        Private _Domain As String
        <Category(Category2 & "Credentials"), _
            Browsable(True), _
            DisplayName(Language.Base.Props_Domain), _
            Description("Enter a domain")> _
        Public Property Domain() As String
            Get
                Return _Domain
            End Get
            Set(ByVal value As String)
                _Domain = value
            End Set
        End Property
#End Region
    End Class
End Namespace