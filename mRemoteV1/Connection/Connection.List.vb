Namespace Connection
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
            For Each conI As Info In List
                If conI.ConstantID = id Then
                    Return conI
                End If
            Next

            Return Nothing
        End Function

        'Public Function Find(ByVal cInfo As Connection.Info)
        '    For Each cI As Connection.Info In List

        '    Next
        'End Function

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