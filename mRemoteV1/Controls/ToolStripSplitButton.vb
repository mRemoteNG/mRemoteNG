Namespace Controls
    Public Class ToolStripSplitButton
        Inherits System.Windows.Forms.ToolStripSplitButton

        Public Overloads Property DropDown As ToolStripDropDown
            Get
                Return MyBase.DropDown
            End Get
            Set
                If MyBase.DropDown IsNot value Then
                    MyBase.DropDown = value
                    AddHandler MyBase.DropDown.Closing, AddressOf DropDown_Closing
                End If
            End Set
        End Property

        Private Sub DropDown_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs)
            If Not e.CloseReason = ToolStripDropDownCloseReason.AppClicked Then Return

            Dim dropDownButtonBoundsClient As Rectangle = DropDownButtonBounds ' Relative to the ToolStripSplitButton
            dropDownButtonBoundsClient.Offset(Bounds.Location) ' Relative to the parent of the ToolStripSplitButton
            Dim dropDownButtonBoundsScreen As Rectangle =
                    GetCurrentParent().RectangleToScreen(dropDownButtonBoundsClient) ' Relative to the screen

            If dropDownButtonBoundsScreen.Contains(Control.MousePosition) Then e.Cancel = True
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            _dropDownVisibleOnMouseDown = DropDown.Visible
            MyBase.OnMouseDown(e)
        End Sub

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            If _dropDownVisibleOnMouseDown Then
                DropDown.Close()
            Else
                MyBase.OnMouseUp(e)
            End If
        End Sub

        Private _dropDownVisibleOnMouseDown As Boolean = False
    End Class
End Namespace