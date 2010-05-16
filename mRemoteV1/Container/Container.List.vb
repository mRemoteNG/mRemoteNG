Namespace Container
    Public Class List
        Inherits CollectionBase

#Region "Public Properties"
        Default Public ReadOnly Property Items(ByVal Index As Object) As Container.Info
            Get
                If TypeOf Index Is Container.Info Then
                    Return Index
                Else
                    Return CType(List.Item(Index), Container.Info)
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
        Public Function Add(ByVal cInfo As Container.Info) As Container.Info
            Me.List.Add(cInfo)
            Return cInfo
        End Function

        Public Sub AddRange(ByVal cInfo() As Container.Info)
            For Each cI As Container.Info In cInfo
                List.Add(cI)
            Next
        End Sub

        Public Function FindByConstantID(ByVal id As String) As Container.Info
            For Each contI As Container.Info In List
                If contI.ConnectionInfo.ConstantID = id Then
                    Return contI
                End If
            Next

            Return Nothing
        End Function

        Public Function Copy() As Container.List
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