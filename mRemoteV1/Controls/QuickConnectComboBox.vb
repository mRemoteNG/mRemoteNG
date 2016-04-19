Imports mRemote3G.App
Imports mRemote3G.Connection
Imports mRemote3G.Connection.Protocol
Imports mRemote3G.Messages

Namespace Controls
    Public Class QuickConnectComboBox
        Inherits ToolStripComboBox

        Private WithEvents _comboBox As ComboBox
        Private _ignoreEnter As Boolean = False

        Public Sub New()
            _comboBox = ComboBox
            _comboBox.DrawMode = DrawMode.OwnerDrawFixed

            ' This makes it so that _ignoreEnter works correctly before any items are added to the combo box
            _comboBox.Items.Clear()
        End Sub

        Private Sub ComboBox_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) _
            Handles _comboBox.PreviewKeyDown
            If e.KeyCode = Keys.Enter And _comboBox.DroppedDown Then _ignoreEnter = True
        End Sub

        Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
            MyBase.OnKeyDown(e)
            If e.KeyCode = Keys.Enter Then
                ' Only connect if Enter was not pressed while the combo box was dropped down
                If Not _ignoreEnter Then OnConnectRequested(New ConnectRequestedEventArgs(_comboBox.Text))
                _ignoreEnter = False
                e.Handled = True
            ElseIf e.KeyCode = Keys.Delete And _comboBox.DroppedDown Then
                If Not _comboBox.SelectedIndex = - 1 Then
                    ' Items can't be removed from the ComboBox while it is dropped down without possibly causing
                    ' an exception so we must close it, delete the item, and then drop it down again. When we
                    ' close it programmatically, the SelectedItem may revert to Nothing, so we must save it first.
                    Dim item As Object = _comboBox.SelectedItem
                    _comboBox.DroppedDown = False
                    _comboBox.Items.Remove(item)
                    _comboBox.SelectedIndex = - 1
                    If Not _comboBox.Items.Count = 0 Then
                        _comboBox.DroppedDown = True
                    End If
                End If
                e.Handled = True
            End If
        End Sub

        Private Sub ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles _comboBox.SelectedIndexChanged
            If Not TypeOf _comboBox.SelectedItem Is HistoryItem Then Return
            Dim historyItem = CType(_comboBox.SelectedItem, HistoryItem)
            OnProtocolChanged(New ProtocolChangedEventArgs(historyItem.ConnectionInfo.Protocol))
        End Sub

        Private Shared Sub ComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles _comboBox.DrawItem
            Dim comboBox = TryCast(sender, ComboBox)
            If comboBox Is Nothing Then Return
            Dim drawItem As Object = comboBox.Items(e.Index)

            Dim drawString As String
            If TypeOf drawItem Is HistoryItem Then
                Dim historyItem = CType(drawItem, HistoryItem)
                drawString = historyItem.ToString(includeProtocol := True)
            Else
                drawString = drawItem.ToString()
            End If

            e.DrawBackground()
            e.Graphics.DrawString(drawString, e.Font, New SolidBrush(e.ForeColor),
                                  New RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
            e.DrawFocusRectangle()
        End Sub

        Private Structure HistoryItem
            Implements IEquatable(Of HistoryItem)

            Public Property ConnectionInfo As Info

            Public Overloads Function Equals(other As HistoryItem) As Boolean _
                Implements IEquatable(Of HistoryItem).Equals
                If Not ConnectionInfo.Hostname = other.ConnectionInfo.Hostname Then Return False
                If Not ConnectionInfo.Port = other.ConnectionInfo.Port Then Return False
                If Not ConnectionInfo.Protocol = other.ConnectionInfo.Protocol Then Return False
                Return True
            End Function

            Public Overrides Function ToString() As String
                Return ToString(False)
            End Function

            Public Overloads Function ToString(includeProtocol As Boolean) As String
                Dim port As String = String.Empty
                If Not ConnectionInfo.Port = ConnectionInfo.GetDefaultPort() Then
                    port = String.Format(":{0}", ConnectionInfo.Port)
                End If
                If includeProtocol Then
                    Return String.Format("{0}{1} ({2})", ConnectionInfo.Hostname, port, ConnectionInfo.Protocol)
                Else
                    Return String.Format("{0}{1}", ConnectionInfo.Hostname, port)
                End If
            End Function
        End Structure

        Private Function Exists(searchItem As HistoryItem) As Boolean
            For Each item As Object In _comboBox.Items
                If Not TypeOf item Is HistoryItem Then Continue For
                Dim historyItem = CType(item, HistoryItem)
                If historyItem.Equals(searchItem) Then Return True
            Next
            Return False
        End Function

        Public Sub Add(connectionInfo As Info)
            Try
                Dim historyItem As New HistoryItem
                historyItem.ConnectionInfo = connectionInfo
                If Not Exists(historyItem) Then _comboBox.Items.Insert(0, historyItem)
            Catch ex As Exception
                Runtime.MessageCollector.AddExceptionMessage(Language.Language.strQuickConnectAddFailed, ex,
                                                             MessageClass.ErrorMsg, True)
            End Try
        End Sub

#Region "Events"

        Public Class ConnectRequestedEventArgs
            Inherits EventArgs

            Public Sub New(connectionString As String)
                _connectionString = connectionString
            End Sub

            Private ReadOnly _connectionString As String

            Public ReadOnly Property ConnectionString As String
                Get
                    Return _connectionString
                End Get
            End Property
        End Class

        Public Event ConnectRequested(sender As Object, e As ConnectRequestedEventArgs)

        Protected Overridable Sub OnConnectRequested(e As ConnectRequestedEventArgs)
            RaiseEvent ConnectRequested(Me, New ConnectRequestedEventArgs(e.ConnectionString))
        End Sub

        Public Class ProtocolChangedEventArgs
            Inherits EventArgs

            Public Sub New(protocol As Protocols)
                _protocol = protocol
            End Sub

            Private ReadOnly _protocol As Protocols

            Public ReadOnly Property Protocol As Protocols
                Get
                    Return _protocol
                End Get
            End Property
        End Class

        Public Event ProtocolChanged(sender As Object, e As ProtocolChangedEventArgs)

        Protected Overridable Sub OnProtocolChanged(e As ProtocolChangedEventArgs)
            RaiseEvent ProtocolChanged(Me, New ProtocolChangedEventArgs(e.Protocol))
        End Sub

#End Region
    End Class
End Namespace

