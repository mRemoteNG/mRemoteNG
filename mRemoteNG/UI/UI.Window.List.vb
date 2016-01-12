Imports System.Windows.Forms

Namespace UI
    Namespace Window
        Public Class List
            Inherits CollectionBase

#Region "Public Properties"
            Default Public ReadOnly Property Items(ByVal Index As Object) As UI.Window.Base
                Get
                    Me.CleanUp()
                    If TypeOf Index Is UI.Window.Base Then
                        If TryCast(Index, UI.Window.Base) IsNot Nothing Then
                            Return Index
                        End If
                    Else
                        If List.Count - 1 >= Index Then
                            If List.Item(Index) IsNot Nothing Then
                                Return TryCast(List.Item(Index), UI.Window.Base)
                            End If
                        End If
                    End If

                    Return Nothing
                End Get
            End Property

            Public Shadows ReadOnly Property Count() As Integer
                Get
                    Me.CleanUp()
                    Return List.Count
                End Get
            End Property
#End Region

#Region "Public Methods"
            Public Sub Add(ByVal uiWindow As UI.Window.Base)
                Me.List.Add(uiWindow)
                'AddHandler uiWindow.FormClosing, AddressOf uiFormClosing
            End Sub

            Public Sub AddRange(ByVal uiWindow() As UI.Window.Base)
                For Each uW As Form In uiWindow
                    Me.List.Add(uW)
                Next
            End Sub

            Public Sub Remove(ByVal uiWindow As UI.Window.Base)
                Me.List.Remove(uiWindow)
            End Sub

            Public Function FromString(ByVal uiWindow As String) As UI.Window.Base
                Me.CleanUp()

                For i As Integer = 0 To Me.List.Count - 1
                    If Me.Items(i).Text = uiWindow.Replace("&", "&&") Then
                        Return Me.Items(i)
                    End If
                Next

                Return Nothing
            End Function
#End Region

#Region "Private Methods"
            Private Sub CleanUp()
                For i As Integer = 0 To Me.List.Count - 1
                    If i > Me.List.Count - 1 Then
                        CleanUp()
                        Exit Sub
                    End If
                    If TryCast(Me.List(i), UI.Window.Base).IsDisposed Then
                        Me.List.RemoveAt(i)
                        CleanUp()
                        Exit Sub
                    End If
                Next
            End Sub
#End Region

#Region "Event Handlers"
            Private Sub uiFormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
                Me.List.Remove(sender)
            End Sub
#End Region
        End Class
    End Namespace
End Namespace