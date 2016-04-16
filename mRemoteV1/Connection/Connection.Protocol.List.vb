Namespace Connection

    Namespace Protocol
        Public Class ProtoList
            Inherits CollectionBase

#Region "Public Properties"

            Default Public ReadOnly Property Items(Index As Object) As Base
                Get
                    If TypeOf Index Is Base Then
                        Return Index
                    Else
                        Return CType(List.Item(Index), Base)
                    End If
                End Get
            End Property

            Public Shadows ReadOnly Property Count As Integer
                Get
                    Return List.Count
                End Get
            End Property

#End Region

#Region "Public Methods"

            Public Function Add(cProt As Base) As Base
                Me.List.Add(cProt)
                Return cProt
            End Function

            Public Sub AddRange(cProt() As Base)
                For Each cP As Base In cProt
                    List.Add(cP)
                Next
            End Sub

            Public Sub Remove(cProt As Base)
                Try
                    Me.List.Remove(cProt)
                Catch ex As Exception
                End Try
            End Sub

            Public Shadows Sub Clear()
                Me.List.Clear()
            End Sub

#End Region
        End Class
    End Namespace

End Namespace