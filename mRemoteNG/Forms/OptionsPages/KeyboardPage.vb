Imports mRemoteNG.Config
Imports mRemoteNG.My

Namespace Forms.OptionsPages
    Public Class KeyboardPage
        Public Overrides Property PageName() As String
            Get
                Return Language.strOptionsTabKeyboard
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides Sub ApplyLanguage()
            MyBase.ApplyLanguage()

            lblKeyboardShortcuts.Text = Language.strOptionsKeyboardLabelKeyboardShortcuts
            btnNewKeyboardShortcut.Text = Language.strOptionsKeyboardButtonNew
            btnDeleteKeyboardShortcut.Text = Language.strOptionsKeyboardButtonDelete
            btnResetKeyboardShortcuts.Text = Language.strOptionsKeyboardButtonReset
            grpModifyKeyboardShortcut.Text = Language.strOptionsKeyboardGroupModifyShortcut
            btnResetAllKeyboardShortcuts.Text = Language.strOptionsKeyboardButtonResetAll
        End Sub

        Public Overrides Sub LoadSettings()
            _tabsListViewGroup = New ListViewGroup(Language.strOptionsKeyboardCommandsGroupTabs)
            _previousTabListViewItem = New ListViewItem(Language.strOptionsKeyboardCommandsPreviousTab, _tabsListViewGroup)
            _nextTabListViewItem = New ListViewItem(Language.strOptionsKeyboardCommandsNextTab, _tabsListViewGroup)

            _keyboardShortcutMap = KeyboardShortcuts.Map.Clone()

            lvKeyboardCommands.Groups.Add(_tabsListViewGroup)
            lvKeyboardCommands.Items.Add(_previousTabListViewItem)
            lvKeyboardCommands.Items.Add(_nextTabListViewItem)
            _previousTabListViewItem.Selected = True
        End Sub

        Public Overrides Sub SaveSettings()
            MyBase.SaveSettings()

            If _keyboardShortcutMap IsNot Nothing Then
                KeyboardShortcuts.Map = _keyboardShortcutMap
            End If
        End Sub

#Region "Private Fields"
        Private _keyboardShortcutMap As KeyboardShortcutMap
        Private _tabsListViewGroup As ListViewGroup
        Private _previousTabListViewItem As ListViewItem
        Private _nextTabListViewItem As ListViewItem
        Private _ignoreKeyboardShortcutTextChanged As Boolean = False
#End Region

#Region "Private Methods"
#Region "Event Handlers"
        Private Sub lvKeyboardCommands_SelectedIndexChanged(sender As System.Object, e As EventArgs) Handles lvKeyboardCommands.SelectedIndexChanged
            Dim isItemSelected As Boolean = (lvKeyboardCommands.SelectedItems.Count = 1)
            EnableKeyboardShortcutControls(isItemSelected)

            If Not isItemSelected Then Return

            Dim selectedItem As ListViewItem = lvKeyboardCommands.SelectedItems(0)

            lblKeyboardCommand.Text = selectedItem.Text
            lstKeyboardShortcuts.Items.Clear()

            lstKeyboardShortcuts.Items.AddRange(_keyboardShortcutMap.GetShortcutKeys(GetSelectedShortcutCommand()))

            If lstKeyboardShortcuts.Items.Count > 0 Then lstKeyboardShortcuts.SelectedIndex = 0
        End Sub

        Private Sub lstKeyboardShortcuts_SelectedIndexChanged(sender As System.Object, e As EventArgs) Handles lstKeyboardShortcuts.SelectedIndexChanged
            Dim isItemSelected As Boolean = (lstKeyboardShortcuts.SelectedItems.Count = 1)

            btnDeleteKeyboardShortcut.Enabled = isItemSelected
            grpModifyKeyboardShortcut.Enabled = isItemSelected
            hotModifyKeyboardShortcut.Enabled = isItemSelected

            If Not isItemSelected Then
                hotModifyKeyboardShortcut.Text = String.Empty
                Return
            End If

            Dim selectedItem As Object = lstKeyboardShortcuts.SelectedItems(0)
            Dim shortcutKey As ShortcutKey = TryCast(selectedItem, ShortcutKey)
            If shortcutKey Is Nothing Then Return

            Dim keysValue As Keys = shortcutKey
            Dim keyCode As Keys = keysValue And Keys.KeyCode
            Dim modifiers As Keys = keysValue And Keys.Modifiers

            _ignoreKeyboardShortcutTextChanged = True
            hotModifyKeyboardShortcut.KeyCode = keyCode
            hotModifyKeyboardShortcut.HotkeyModifiers = modifiers
            _ignoreKeyboardShortcutTextChanged = False
        End Sub

        Private Sub btnNewKeyboardShortcut_Click(sender As System.Object, e As EventArgs) Handles btnNewKeyboardShortcut.Click
            For Each item As Object In lstKeyboardShortcuts.Items
                Dim shortcutKey As ShortcutKey = TryCast(item, ShortcutKey)
                If shortcutKey Is Nothing Then Continue For
                If shortcutKey = 0 Then
                    lstKeyboardShortcuts.SelectedItem = item
                    Return
                End If
            Next

            lstKeyboardShortcuts.SelectedIndex = lstKeyboardShortcuts.Items.Add(New ShortcutKey(Keys.None))
            hotModifyKeyboardShortcut.Focus()
        End Sub

        Private Sub btnDeleteKeyboardShortcut_Click(sender As System.Object, e As EventArgs) Handles btnDeleteKeyboardShortcut.Click
            Dim selectedIndex As Integer = lstKeyboardShortcuts.SelectedIndex

            Dim command As ShortcutCommand = GetSelectedShortcutCommand()
            Dim key As ShortcutKey = TryCast(lstKeyboardShortcuts.SelectedItem, ShortcutKey)
            If Not command = ShortcutCommand.None And key IsNot Nothing Then
                _keyboardShortcutMap.Remove(GetSelectedShortcutCommand(), key)
            End If

            lstKeyboardShortcuts.Items.Remove(lstKeyboardShortcuts.SelectedItem)

            If selectedIndex >= lstKeyboardShortcuts.Items.Count Then selectedIndex = lstKeyboardShortcuts.Items.Count - 1
            lstKeyboardShortcuts.SelectedIndex = selectedIndex
        End Sub

        Private Sub btnResetAllKeyboardShortcuts_Click(sender As System.Object, e As EventArgs) Handles btnResetAllKeyboardShortcuts.Click
            _keyboardShortcutMap = KeyboardShortcuts.DefaultMap.Clone()
            lvKeyboardCommands_SelectedIndexChanged(Me, New EventArgs())
        End Sub

        Private Sub btnResetKeyboardShortcuts_Click(sender As System.Object, e As EventArgs) Handles btnResetKeyboardShortcuts.Click
            Dim command As ShortcutCommand = GetSelectedShortcutCommand()
            If command = ShortcutCommand.None Then Return
            _keyboardShortcutMap.SetShortcutKeys(command, KeyboardShortcuts.DefaultMap.GetShortcutKeys(command))
            lvKeyboardCommands_SelectedIndexChanged(Me, New EventArgs())
        End Sub

        Private Sub hotModifyKeyboardShortcut_TextChanged(sender As System.Object, e As EventArgs) Handles hotModifyKeyboardShortcut.TextChanged
            If _ignoreKeyboardShortcutTextChanged Or _
                lstKeyboardShortcuts.SelectedIndex < 0 Or _
                lstKeyboardShortcuts.SelectedIndex >= lstKeyboardShortcuts.Items.Count Then Return

            Dim keysValue As Keys = (hotModifyKeyboardShortcut.KeyCode And Keys.KeyCode) Or _
                                    (hotModifyKeyboardShortcut.HotkeyModifiers And Keys.Modifiers)

            Dim hadFocus As Boolean = hotModifyKeyboardShortcut.ContainsFocus

            Dim command As ShortcutCommand = GetSelectedShortcutCommand()
            Dim newShortcutKey As New ShortcutKey(keysValue)

            If Not command = ShortcutCommand.None Then
                Dim oldShortcutKey As ShortcutKey = TryCast(lstKeyboardShortcuts.SelectedItem, ShortcutKey)
                If oldShortcutKey IsNot Nothing Then
                    _keyboardShortcutMap.Remove(command, oldShortcutKey)
                End If
                _keyboardShortcutMap.Add(command, newShortcutKey)
            End If

            lstKeyboardShortcuts.Items(lstKeyboardShortcuts.SelectedIndex) = newShortcutKey

            If hadFocus Then
                hotModifyKeyboardShortcut.Focus()
                hotModifyKeyboardShortcut.Select(hotModifyKeyboardShortcut.TextLength, 0)
            End If
        End Sub
#End Region

        Private Function GetSelectedShortcutCommand() As ShortcutCommand
            If Not (lvKeyboardCommands.SelectedItems.Count = 1) Then Return ShortcutCommand.None

            Dim selectedItem As ListViewItem = lvKeyboardCommands.SelectedItems(0)
            If selectedItem Is _previousTabListViewItem Then
                Return ShortcutCommand.PreviousTab
            ElseIf selectedItem Is _nextTabListViewItem Then
                Return ShortcutCommand.NextTab
            End If
        End Function

        Private Sub EnableKeyboardShortcutControls(Optional ByVal enable As Boolean = True)
            lblKeyboardCommand.Visible = enable
            lblKeyboardShortcuts.Enabled = enable
            lstKeyboardShortcuts.Enabled = enable
            btnNewKeyboardShortcut.Enabled = enable
            btnResetKeyboardShortcuts.Enabled = enable

            If Not enable Then
                btnDeleteKeyboardShortcut.Enabled = False
                grpModifyKeyboardShortcut.Enabled = False
                hotModifyKeyboardShortcut.Enabled = False

                lstKeyboardShortcuts.Items.Clear()
                hotModifyKeyboardShortcut.Text = String.Empty
            End If
        End Sub
#End Region
    End Class
End Namespace