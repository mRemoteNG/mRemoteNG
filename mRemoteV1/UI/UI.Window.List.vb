

Namespace UI

    Namespace Window
        Public Class List
            Inherits CollectionBase

#Region "Public Properties"

            Default Public ReadOnly Property Items(Index As Object) As Base
                Get
                    Me.CleanUp()
                    If TypeOf Index Is Base Then
                        If TryCast(Index, Base) IsNot Nothing Then
                            Return Index
                        End If
                    Else
                        If List.Count - 1 >= Index Then
                            If List.Item(Index) IsNot Nothing Then
                                Return TryCast(List.Item(Index), Base)
                            End If
                        End If
                    End If

                    Return Nothing
                End Get
            End Property

            Public Shadows ReadOnly Property Count As Integer
                Get
                    Me.CleanUp()
                    Return List.Count
                End Get
            End Property

#End Region

#Region "Public Methods"

            Public Sub Add(uiWindow As Base)
                Me.List.Add(uiWindow)
                'AddHandler uiWindow.FormClosing, AddressOf uiFormClosing
            End Sub

            Public Sub AddRange(uiWindow() As Base)
                For Each uW As Form In uiWindow
                    Me.List.Add(uW)
                Next
            End Sub

            Public Sub Remove(uiWindow As Base)
                Me.List.Remove(uiWindow)
            End Sub

            Public Function FromString(uiWindow As String) As Base
                Me.CleanUp()

                For i = 0 To Me.List.Count - 1
                    If Me.Items(i).Text = uiWindow.Replace("&", "&&") Then
                        Return Me.Items(i)
                    End If
                Next

                Return Nothing
            End Function

#End Region

#Region "Private Methods"

            Private Sub CleanUp()
                For i = 0 To Me.List.Count - 1
                    If i > Me.List.Count - 1 Then
                        CleanUp()
                        Exit Sub
                    End If
                    If TryCast(Me.List(i), Base).IsDisposed Then
                        Me.List.RemoveAt(i)
                        CleanUp()
                        Exit Sub
                    End If
                Next
            End Sub

#End Region

#Region "Event Handlers"

            Private Sub uiFormClosing(sender As Object, e As FormClosingEventArgs)
                Me.List.Remove(sender)
            End Sub

#End Region
        End Class
    End Namespace

End Namespace