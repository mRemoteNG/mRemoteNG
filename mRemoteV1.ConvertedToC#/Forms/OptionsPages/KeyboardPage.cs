using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Config;
using mRemoteNG.My;

namespace mRemoteNG.Forms.OptionsPages
{
	public partial class KeyboardPage
	{
		public override string PageName {
			get { return Language.strOptionsTabKeyboard; }
			set { }
		}

		public override void ApplyLanguage()
		{
			base.ApplyLanguage();

			lblKeyboardShortcuts.Text = Language.strOptionsKeyboardLabelKeyboardShortcuts;
			btnNewKeyboardShortcut.Text = Language.strOptionsKeyboardButtonNew;
			btnDeleteKeyboardShortcut.Text = Language.strOptionsKeyboardButtonDelete;
			btnResetKeyboardShortcuts.Text = Language.strOptionsKeyboardButtonReset;
			grpModifyKeyboardShortcut.Text = Language.strOptionsKeyboardGroupModifyShortcut;
			btnResetAllKeyboardShortcuts.Text = Language.strOptionsKeyboardButtonResetAll;
		}

		public override void LoadSettings()
		{
			_tabsListViewGroup = new ListViewGroup(Language.strOptionsKeyboardCommandsGroupTabs);
			_previousTabListViewItem = new ListViewItem(Language.strOptionsKeyboardCommandsPreviousTab, _tabsListViewGroup);
			_nextTabListViewItem = new ListViewItem(Language.strOptionsKeyboardCommandsNextTab, _tabsListViewGroup);

			_keyboardShortcutMap = KeyboardShortcuts.Map.Clone();

			lvKeyboardCommands.Groups.Add(_tabsListViewGroup);
			lvKeyboardCommands.Items.Add(_previousTabListViewItem);
			lvKeyboardCommands.Items.Add(_nextTabListViewItem);
			_previousTabListViewItem.Selected = true;
		}

		public override void SaveSettings()
		{
			base.SaveSettings();

			if (_keyboardShortcutMap != null) {
				KeyboardShortcuts.Map = _keyboardShortcutMap;
			}
		}

		#region "Private Fields"
		private KeyboardShortcutMap _keyboardShortcutMap;
		private ListViewGroup _tabsListViewGroup;
		private ListViewItem _previousTabListViewItem;
		private ListViewItem _nextTabListViewItem;
			#endregion
		private bool _ignoreKeyboardShortcutTextChanged = false;

		#region "Private Methods"
		#region "Event Handlers"
		private void lvKeyboardCommands_SelectedIndexChanged(System.Object sender, EventArgs e)
		{
			bool isItemSelected = (lvKeyboardCommands.SelectedItems.Count == 1);
			EnableKeyboardShortcutControls(isItemSelected);

			if (!isItemSelected)
				return;

			ListViewItem selectedItem = lvKeyboardCommands.SelectedItems[0];

			lblKeyboardCommand.Text = selectedItem.Text;
			lstKeyboardShortcuts.Items.Clear();

			lstKeyboardShortcuts.Items.AddRange(_keyboardShortcutMap.GetShortcutKeys(GetSelectedShortcutCommand()));

			if (lstKeyboardShortcuts.Items.Count > 0)
				lstKeyboardShortcuts.SelectedIndex = 0;
		}

		private void lstKeyboardShortcuts_SelectedIndexChanged(System.Object sender, EventArgs e)
		{
			bool isItemSelected = (lstKeyboardShortcuts.SelectedItems.Count == 1);

			btnDeleteKeyboardShortcut.Enabled = isItemSelected;
			grpModifyKeyboardShortcut.Enabled = isItemSelected;
			hotModifyKeyboardShortcut.Enabled = isItemSelected;

			if (!isItemSelected) {
				hotModifyKeyboardShortcut.Text = string.Empty;
				return;
			}

			object selectedItem = lstKeyboardShortcuts.SelectedItems[0];
			ShortcutKey shortcutKey = selectedItem as ShortcutKey;
			if (shortcutKey == null)
				return;

			Keys keysValue = shortcutKey;
			Keys keyCode = keysValue & Keys.KeyCode;
			Keys modifiers = keysValue & Keys.Modifiers;

			_ignoreKeyboardShortcutTextChanged = true;
			hotModifyKeyboardShortcut.KeyCode = keyCode;
			hotModifyKeyboardShortcut.HotkeyModifiers = modifiers;
			_ignoreKeyboardShortcutTextChanged = false;
		}

		private void btnNewKeyboardShortcut_Click(System.Object sender, EventArgs e)
		{
			foreach (object item in lstKeyboardShortcuts.Items) {
				ShortcutKey shortcutKey = item as ShortcutKey;
				if (shortcutKey == null)
					continue;
				if (shortcutKey == 0) {
					lstKeyboardShortcuts.SelectedItem = item;
					return;
				}
			}

			lstKeyboardShortcuts.SelectedIndex = lstKeyboardShortcuts.Items.Add(new ShortcutKey(Keys.None));
			hotModifyKeyboardShortcut.Focus();
		}

		private void btnDeleteKeyboardShortcut_Click(System.Object sender, EventArgs e)
		{
			int selectedIndex = lstKeyboardShortcuts.SelectedIndex;

			ShortcutCommand command = GetSelectedShortcutCommand();
			ShortcutKey key = lstKeyboardShortcuts.SelectedItem as ShortcutKey;
			if (!(command == ShortcutCommand.None) & key != null) {
				_keyboardShortcutMap.Remove(GetSelectedShortcutCommand(), key);
			}

			lstKeyboardShortcuts.Items.Remove(lstKeyboardShortcuts.SelectedItem);

			if (selectedIndex >= lstKeyboardShortcuts.Items.Count)
				selectedIndex = lstKeyboardShortcuts.Items.Count - 1;
			lstKeyboardShortcuts.SelectedIndex = selectedIndex;
		}

		private void btnResetAllKeyboardShortcuts_Click(System.Object sender, EventArgs e)
		{
			_keyboardShortcutMap = KeyboardShortcuts.DefaultMap.Clone();
			lvKeyboardCommands_SelectedIndexChanged(this, new EventArgs());
		}

		private void btnResetKeyboardShortcuts_Click(System.Object sender, EventArgs e)
		{
			ShortcutCommand command = GetSelectedShortcutCommand();
			if (command == ShortcutCommand.None)
				return;
			_keyboardShortcutMap.SetShortcutKeys(command, KeyboardShortcuts.DefaultMap.GetShortcutKeys(command));
			lvKeyboardCommands_SelectedIndexChanged(this, new EventArgs());
		}

		private void hotModifyKeyboardShortcut_TextChanged(System.Object sender, EventArgs e)
		{
			if (_ignoreKeyboardShortcutTextChanged | lstKeyboardShortcuts.SelectedIndex < 0 | lstKeyboardShortcuts.SelectedIndex >= lstKeyboardShortcuts.Items.Count)
				return;

			Keys keysValue = (hotModifyKeyboardShortcut.KeyCode & Keys.KeyCode) | (hotModifyKeyboardShortcut.HotkeyModifiers & Keys.Modifiers);

			bool hadFocus = hotModifyKeyboardShortcut.ContainsFocus;

			ShortcutCommand command = GetSelectedShortcutCommand();
			ShortcutKey newShortcutKey = new ShortcutKey(keysValue);

			if (!(command == ShortcutCommand.None)) {
				ShortcutKey oldShortcutKey = lstKeyboardShortcuts.SelectedItem as ShortcutKey;
				if (oldShortcutKey != null) {
					_keyboardShortcutMap.Remove(command, oldShortcutKey);
				}
				_keyboardShortcutMap.Add(command, newShortcutKey);
			}

			lstKeyboardShortcuts.Items[lstKeyboardShortcuts.SelectedIndex] = newShortcutKey;

			if (hadFocus) {
				hotModifyKeyboardShortcut.Focus();
				hotModifyKeyboardShortcut.Select(hotModifyKeyboardShortcut.TextLength, 0);
			}
		}
		#endregion

		private ShortcutCommand GetSelectedShortcutCommand()
		{
			if (!(lvKeyboardCommands.SelectedItems.Count == 1))
				return ShortcutCommand.None;

			ListViewItem selectedItem = lvKeyboardCommands.SelectedItems[0];
			if (object.ReferenceEquals(selectedItem, _previousTabListViewItem)) {
				return ShortcutCommand.PreviousTab;
			} else if (object.ReferenceEquals(selectedItem, _nextTabListViewItem)) {
				return ShortcutCommand.NextTab;
			}
		}

		private void EnableKeyboardShortcutControls(bool enable = true)
		{
			lblKeyboardCommand.Visible = enable;
			lblKeyboardShortcuts.Enabled = enable;
			lstKeyboardShortcuts.Enabled = enable;
			btnNewKeyboardShortcut.Enabled = enable;
			btnResetKeyboardShortcuts.Enabled = enable;

			if (!enable) {
				btnDeleteKeyboardShortcut.Enabled = false;
				grpModifyKeyboardShortcut.Enabled = false;
				hotModifyKeyboardShortcut.Enabled = false;

				lstKeyboardShortcuts.Items.Clear();
				hotModifyKeyboardShortcut.Text = string.Empty;
			}
		}
		public KeyboardPage()
		{
			InitializeComponent();
		}
		#endregion
	}
}
