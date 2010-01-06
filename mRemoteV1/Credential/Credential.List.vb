Namespace Credential
    Public Class List
        Inherits CollectionBase

#Region "Public Properties"
        Default Public ReadOnly Property Items(ByVal Index As Object) As Credential.Info
            Get
                If TypeOf Index Is Credential.Info Then
                    Return Index
                Else
                    Return CType(List.Item(Index), Credential.Info)
                End If
            End Get
        End Property

        Public Shadows ReadOnly Property Count() As Integer
            Get
                Return List.Count
            End Get
        End Property
#End Region

#Region "Public Methods"
        Public Function Add(ByVal cInfo As Credential.Info) As Credential.Info
            List.Add(cInfo)
            Return cInfo
        End Function

        Public Sub AddRange(ByVal cInfo() As Credential.Info)
            For Each cI As Credential.Info In cInfo
                List.Add(cI)
            Next
        End Sub

        Public Function Copy() As Credential.List
            Try
                Return Me.MemberwiseClone
            Catch ex As Exception
            End Try

            Return Nothing
        End Function

        Public Shadows Sub Clear()
            List.Clear()
        End Sub
#End Region
    End Class
End Namespace