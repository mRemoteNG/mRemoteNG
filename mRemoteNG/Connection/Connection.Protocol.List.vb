Namespace Connection
    Namespace Protocol
        Public Class List
            Inherits CollectionBase

#Region "Public Properties"
            Default Public ReadOnly Property Items(ByVal Index As Object) As Connection.Protocol.Base
                Get
                    If TypeOf Index Is Connection.Protocol.Base Then
                        Return Index
                    Else
                        Return CType(List.Item(Index), Connection.Protocol.Base)
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
            Public Function Add(ByVal cProt As Connection.Protocol.Base) As Connection.Protocol.Base
                Me.List.Add(cProt)
                Return cProt
            End Function

            Public Sub AddRange(ByVal cProt() As Connection.Protocol.Base)
                For Each cP As Connection.Protocol.Base In cProt
                    List.Add(cP)
                Next
            End Sub

            Public Sub Remove(ByVal cProt As Connection.Protocol.Base)
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