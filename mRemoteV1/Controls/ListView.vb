Imports System.ComponentModel

Namespace Controls
    Public Class ListView
        Inherits System.Windows.Forms.ListView

#Region "Public Properties"

        <Category("Appearance"),
            DefaultValue(GetType(Color), "HighlightText")>
        Public Property HighlightForeColor As Color = SystemColors.HighlightText

        <Category("Appearance"),
            DefaultValue(GetType(Color), "Highlight")>
        Public Property HighlightBackColor As Color = SystemColors.Highlight

        <Category("Appearance"),
            DefaultValue(GetType(Color), "HotTrack")>
        Public Property HighlightBorderColor As Color = SystemColors.HotTrack

        <Category("Appearance"),
            DefaultValue(GetType(Color), "ControlText")>
        Public Property InactiveHighlightForeColor As Color = SystemColors.ControlText

        <Category("Appearance"),
            DefaultValue(GetType(Color), "Control")>
        Public Property InactiveHighlightBackColor As Color = SystemColors.Control

        <Category("Appearance"),
            DefaultValue(GetType(Color), "ControlDark")>
        Public Property InactiveHighlightBorderColor As Color = SystemColors.ControlDark

        <Category("Appearance"),
            DefaultValue(True)>
        Public Overloads Property ShowFocusCues As Boolean = True

        <Category("Appearance")>
        Public Property LabelAlignment As New Alignment(VerticalAlignment.Top, HorizontalAlignment.Left)

#End Region

#Region "Constructors"

        Public Sub New()
            OwnerDraw = True
        End Sub

#End Region

#Region "Protected Methods"

        Protected Overrides Sub OnDrawItem(e As DrawListViewItemEventArgs)
            If Not View = View.Tile Then MyBase.OnDrawItem(e)
            If e.ItemIndex < 0 Then MyBase.OnDrawItem(e)

            Dim foreColorBrush As Brush = Nothing
            Dim backColorBrush As Brush = Nothing
            Dim borderPen As Pen = Nothing
            Try
                If Focused Then
                    borderPen = New Pen(HighlightBorderColor)
                Else
                    borderPen = New Pen(InactiveHighlightBorderColor)
                End If

                If e.Item.Selected Then
                    If Focused Then
                        foreColorBrush = New SolidBrush(HighlightForeColor)
                        backColorBrush = New SolidBrush(HighlightBackColor)
                    Else
                        foreColorBrush = New SolidBrush(InactiveHighlightForeColor)
                        backColorBrush = New SolidBrush(InactiveHighlightBackColor)
                    End If
                Else
                    foreColorBrush = New SolidBrush(e.Item.ForeColor)
                    backColorBrush = New SolidBrush(BackColor)
                End If

                e.Graphics.FillRectangle(backColorBrush, e.Bounds)

                If Focused And ShowFocusCues Then
                    e.DrawFocusRectangle()
                ElseIf e.Item.Selected Then
                    e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1)
                End If

                Dim imageBounds As New Rectangle(e.Bounds.X + 2, e.Bounds.Y + 6, 16, 16)
                Dim textBounds As Rectangle = e.Bounds

                If e.Item.ImageList IsNot Nothing Then
                    Dim image As Image = Nothing
                    If _
                        Not String.IsNullOrEmpty(e.Item.ImageKey) And
                        e.Item.ImageList.Images.ContainsKey(e.Item.ImageKey) Then
                        image = e.Item.ImageList.Images.Item(e.Item.ImageKey)
                    ElseIf Not e.Item.ImageIndex < 0 And e.Item.ImageList.Images.Count > e.Item.ImageIndex Then
                        image = e.Item.ImageList.Images(e.Item.ImageIndex)
                    End If
                    If image IsNot Nothing Then
                        e.Graphics.DrawImageUnscaledAndClipped(image, imageBounds)
                        textBounds.X = textBounds.Left + 20
                        textBounds.Width = textBounds.Width - 20
                    End If
                End If

                e.Graphics.DrawString(e.Item.Text, e.Item.Font, foreColorBrush, textBounds, GetStringFormat())
            Finally
                If foreColorBrush IsNot Nothing Then foreColorBrush.Dispose()
                If backColorBrush IsNot Nothing Then backColorBrush.Dispose()
                If borderPen IsNot Nothing Then borderPen.Dispose()
            End Try
        End Sub

#End Region

#Region "Private Methods"

        Private Function GetStringFormat() As StringFormat
            Dim format As StringFormat = StringFormat.GenericDefault

            Select Case LabelAlignment.Vertical
                Case VerticalAlignment.Top
                    format.LineAlignment = StringAlignment.Near
                Case VerticalAlignment.Middle
                    format.LineAlignment = StringAlignment.Center
                Case VerticalAlignment.Bottom
                    format.LineAlignment = StringAlignment.Far
            End Select

            Select Case LabelAlignment.Horizontal
                Case HorizontalAlignment.Left
                    format.Alignment = StringAlignment.Near
                Case HorizontalAlignment.Center
                    format.Alignment = StringAlignment.Center
                Case HorizontalAlignment.Right
                    format.Alignment = StringAlignment.Far
            End Select

            If RightToLeft Then
                format.FormatFlags = format.FormatFlags Or StringFormatFlags.DirectionRightToLeft
            End If

            If LabelWrap Then
                format.FormatFlags = format.FormatFlags And Not StringFormatFlags.NoWrap
            Else
                format.FormatFlags = format.FormatFlags Or StringFormatFlags.NoWrap
            End If

            Return format
        End Function

#End Region
    End Class

    <TypeConverter(GetType(ExpandableObjectConverter))>
    Public Class Alignment
        Public Sub New()
        End Sub

        Public Sub New(verticalAlignment As VerticalAlignment, horizontalAlignment As HorizontalAlignment)
            Vertical = verticalAlignment
            Horizontal = horizontalAlignment
        End Sub

        <NotifyParentProperty(True),
            DefaultValue(VerticalAlignment.Top)>
        Public Property Vertical As VerticalAlignment = VerticalAlignment.Top

        <NotifyParentProperty(True),
            DefaultValue(HorizontalAlignment.Left)>
        Public Property Horizontal As HorizontalAlignment = HorizontalAlignment.Left

        Public Overrides Function ToString() As String
            Return String.Format("{0}, {1}", Vertical, Horizontal)
        End Function
    End Class

    Public Enum VerticalAlignment
        Top
        Middle
        Bottom
    End Enum

    Public Enum HorizontalAlignment
        Left
        Center
        Right
    End Enum
End Namespace
