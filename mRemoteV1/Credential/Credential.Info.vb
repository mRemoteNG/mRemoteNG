Imports System.ComponentModel
Imports mRemoteNG.Tools.LocalizedAttributes

Namespace Credential
    Public Class Info
#Region "1 Display"
        Private _Name As String
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameName"), _
            LocalizedDescription("strPropertyDescriptionName")> _
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Private _Description As String
        <LocalizedCategory("strCategoryDisplay", 1), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameDescription"), _
            LocalizedDescription("strPropertyDescriptionDescription")> _
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
        <LocalizedCategory("strCategoryCredentials", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameUsername"), _
            LocalizedDescription("strPropertyDescriptionUsername")> _
        Public Property Username() As String
            Get
                Return _Username
            End Get
            Set(ByVal value As String)
                _Username = value
            End Set
        End Property

        Private _Password As String
        <LocalizedCategory("strCategoryCredentials", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNamePassword"), _
            LocalizedDescription("strPropertyDescriptionPassword"), _
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
        <LocalizedCategory("strCategoryCredentials", 2), _
            Browsable(True), _
            LocalizedDisplayName("strPropertyNameDomain"), _
            LocalizedDescription("strPropertyDescriptionDomain")> _
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