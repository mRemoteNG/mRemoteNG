Imports System.ComponentModel
Imports mRemote3G.Tools

Namespace Credential
    Public Class Info

#Region "1 Display"

        Private _Name As String

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            Browsable(True),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameName"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionName")>
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set
                _Name = value
            End Set
        End Property

        Private _Description As String

        <LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 1),
            Browsable(True),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDescription"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDescription")>
        Public Property Description As String
            Get
                Return _Description
            End Get
            Set
                _Description = value
            End Set
        End Property

#End Region

#Region "2 Credentials"

        Private _Username As String

        <LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
            Browsable(True),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameUsername"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionUsername")>
        Public Property Username As String
            Get
                Return _Username
            End Get
            Set
                _Username = value
            End Set
        End Property

        Private _Password As String

        <LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
            Browsable(True),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNamePassword"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionPassword"),
            PasswordPropertyText(True)>
        Public Property Password As String
            Get
                Return _Password
            End Get
            Set
                _Password = value
            End Set
        End Property

        Private _Domain As String

        <LocalizedAttributes.LocalizedCategory("strCategoryCredentials", 2),
            Browsable(True),
            LocalizedAttributes.LocalizedDisplayName("strPropertyNameDomain"),
            LocalizedAttributes.LocalizedDescription("strPropertyDescriptionDomain")>
        Public Property Domain As String
            Get
                Return _Domain
            End Get
            Set
                _Domain = value
            End Set
        End Property

#End Region
    End Class
End Namespace