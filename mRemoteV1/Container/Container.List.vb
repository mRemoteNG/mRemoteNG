Namespace Container
    Public Class List
        Inherits CollectionBase

#Region "Public Properties"

        Default Public ReadOnly Property Items(Index As Object) As Info
            Get
                If TypeOf Index Is Info Then
                    Return Index
                Else
                    Return CType(List.Item(Index), Info)
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

        Public Function Add(cInfo As Info) As Info
            Me.List.Add(cInfo)
            Return cInfo
        End Function

        Public Sub AddRange(cInfo() As Info)
            For Each cI As Info In cInfo
                List.Add(cI)
            Next
        End Sub

        Public Function FindByConstantID(id As String) As Info
            For Each contI As Info In List
                If contI.ConnectionInfo.ConstantID = id Then
                    Return contI
                End If
            Next

            Return Nothing
        End Function

        Public Function Copy() As List
            Try
                Return Me.MemberwiseClone
            Catch ex As Exception
            End Try

            Return Nothing
        End Function

        Public Shadows Sub Clear()
            Me.List.Clear()
        End Sub

#End Region
    End Class
End Namespace