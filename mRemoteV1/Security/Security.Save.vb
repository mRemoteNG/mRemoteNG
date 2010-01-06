Namespace Security
    Public Class Save
        Public Sub New(Optional ByVal DisableEverything As Boolean = False)
            If DisableEverything = False Then
                _Username = True
                _Password = True
                _Domain = True
                _Inheritance = True
            End If
        End Sub

        Private _Username As Boolean
        Public Property Username() As Boolean
            Get
                Return _Username
            End Get
            Set(ByVal value As Boolean)
                _Username = value
            End Set
        End Property

        Private _Password As Boolean
        Public Property Password() As Boolean
            Get
                Return _Password
            End Get
            Set(ByVal value As Boolean)
                _Password = value
            End Set
        End Property

        Private _Domain As Boolean
        Public Property Domain() As Boolean
            Get
                Return _Domain
            End Get
            Set(ByVal value As Boolean)
                _Domain = value
            End Set
        End Property

        Private _Inheritance As Boolean
        Public Property Inheritance() As Boolean
            Get
                Return _Inheritance
            End Get
            Set(ByVal value As Boolean)
                _Inheritance = value
            End Set
        End Property
    End Class
End Namespace
