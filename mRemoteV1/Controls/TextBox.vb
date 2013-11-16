Imports System.ComponentModel

' Adapted from http://stackoverflow.com/a/3678888/2101395

Namespace Controls
    Public Class TextBox
        Inherits Windows.Forms.TextBox

#Region "Public Properties"
        <Category("Behavior"),
        DefaultValue(False)> _
        Public Property SelectAllOnFocus As Boolean = False
#End Region

#Region "Protected Methods"
        Protected Overrides Sub OnEnter(ByVal e As EventArgs)
            MyBase.OnEnter(e)

            If MouseButtons = MouseButtons.None Then
                SelectAll()
                _focusHandled = True
            End If
        End Sub

        Protected Overrides Sub OnLeave(ByVal e As EventArgs)
            MyBase.OnLeave(e)

            _focusHandled = False
        End Sub

        Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
            MyBase.OnMouseUp(e)

            If Not _focusHandled Then
                If SelectionLength = 0 Then SelectAll()
                _focusHandled = True
            End If
        End Sub
#End Region

#Region "Private Fields"
        Private _focusHandled As Boolean = False
#End Region
    End Class
End Namespace