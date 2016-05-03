using System;
using System.Windows.Forms;
using mRemoteNG.Config.KeyboardShortcuts;
using mRemoteNG.My;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class KeyboardPage
    {
        private bool _ignoreKeyboardShortcutTextChanged;
        private KeyboardShortcutMap _keyboardShortcutMap;
        private ListViewItem _nextTabListViewItem;
        private ListViewItem _previousTabListViewItem;
        private ListViewGroup _tabsListViewGroup;


        public KeyboardPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
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
            _previousTabListViewItem = new ListViewItem(Language.strOptionsKeyboardCommandsPreviousTab,
                _tabsListViewGroup);
            _nextTabListViewItem = new ListViewItem(Language.strOptionsKeyboardCommandsNextTab, _tabsListViewGroup);

            _keyboardShortcutMap = (KeyboardShortcutMap) KeyboardShortcuts.Map.Clone();

            lvKeyboardCommands.Groups.Add(_tabsListViewGroup);
            lvKeyboardCommands.Items.Add(_previousTabListViewItem);
            lvKeyboardCommands.Items.Add(_nextTabListViewItem);
            _previousTabListViewItem.Selected = true;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            if (_keyboardShortcutMap != null)
            {
                KeyboardShortcuts.Map = _keyboardShortcutMap;
            }
        }

        #region Private Methods

        #region Event Handlers

        public void lvKeyboardCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isItemSelected = lvKeyboardCommands.SelectedItems.Count == 1;
            EnableKeyboardShortcutControls(isItemSelected);

            if (!isItemSelected)
            {
                return;
            }

            var selectedItem = lvKeyboardCommands.SelectedItems[0];

            lblKeyboardCommand.Text = selectedItem.Text;
            lstKeyboardShortcuts.Items.Clear();

            lstKeyboardShortcuts.Items.AddRange(_keyboardShortcutMap.GetShortcutKeys(GetSelectedShortcutCommand()));

            if (lstKeyboardShortcuts.Items.Count > 0)
            {
                lstKeyboardShortcuts.SelectedIndex = 0;
            }
        }

        public void lstKeyboardShortcuts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var isItemSelected = lstKeyboardShortcuts.SelectedItems.Count == 1;

            btnDeleteKeyboardShortcut.Enabled = isItemSelected;
            grpModifyKeyboardShortcut.Enabled = isItemSelected;
            hotModifyKeyboardShortcut.Enabled = isItemSelected;

            if (!isItemSelected)
            {
                hotModifyKeyboardShortcut.Text = string.Empty;
                return;
            }

            var selectedItem = lstKeyboardShortcuts.SelectedItems[0];
            var shortcutKey = selectedItem as ShortcutKey;
            if (shortcutKey == null)
            {
                return;
            }

            //Keys keysValue = System.Windows.Forms.Keys.A;
            var keyCode = Keys.A;
            var modifiers = Keys.A;

            _ignoreKeyboardShortcutTextChanged = true;
            hotModifyKeyboardShortcut.KeyCode = keyCode;
            hotModifyKeyboardShortcut.HotkeyModifiers = modifiers;
            _ignoreKeyboardShortcutTextChanged = false;
        }

        public void btnNewKeyboardShortcut_Click(object sender, EventArgs e)
        {
            foreach (var item in lstKeyboardShortcuts.Items)
            {
                var shortcutKey = item as ShortcutKey;
                if (shortcutKey?.KeyCode == 0)
                {
                    lstKeyboardShortcuts.SelectedItem = item;
                    return;
                }
            }

            lstKeyboardShortcuts.SelectedIndex = lstKeyboardShortcuts.Items.Add(new ShortcutKey(Keys.None));
            hotModifyKeyboardShortcut.Focus();
        }

        public void btnDeleteKeyboardShortcut_Click(object sender, EventArgs e)
        {
            var selectedIndex = lstKeyboardShortcuts.SelectedIndex;

            var command = GetSelectedShortcutCommand();
            var key = lstKeyboardShortcuts.SelectedItem as ShortcutKey;
            if (command != ShortcutCommand.None & key != null)
            {
                _keyboardShortcutMap.Remove(GetSelectedShortcutCommand(), key);
            }

            lstKeyboardShortcuts.Items.Remove(lstKeyboardShortcuts.SelectedItem);

            if (selectedIndex >= lstKeyboardShortcuts.Items.Count)
            {
                selectedIndex = lstKeyboardShortcuts.Items.Count - 1;
            }
            lstKeyboardShortcuts.SelectedIndex = selectedIndex;
        }

        public void btnResetAllKeyboardShortcuts_Click(object sender, EventArgs e)
        {
            _keyboardShortcutMap = (KeyboardShortcutMap) KeyboardShortcuts.DefaultMap.Clone();
            lvKeyboardCommands_SelectedIndexChanged(this, new EventArgs());
        }

        public void btnResetKeyboardShortcuts_Click(object sender, EventArgs e)
        {
            var command = GetSelectedShortcutCommand();
            if (command == ShortcutCommand.None)
            {
                return;
            }
            _keyboardShortcutMap.SetShortcutKeys(command, KeyboardShortcuts.DefaultMap.GetShortcutKeys(command));
            lvKeyboardCommands_SelectedIndexChanged(this, new EventArgs());
        }

        public void hotModifyKeyboardShortcut_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreKeyboardShortcutTextChanged ||
                lstKeyboardShortcuts.SelectedIndex < 0 |
                lstKeyboardShortcuts.SelectedIndex >= lstKeyboardShortcuts.Items.Count)
            {
                return;
            }

            var keysValue = Keys.A;

            var hadFocus = hotModifyKeyboardShortcut.ContainsFocus;

            var command = GetSelectedShortcutCommand();
            var newShortcutKey = new ShortcutKey(keysValue);

            if (!(command == ShortcutCommand.None))
            {
                var oldShortcutKey = lstKeyboardShortcuts.SelectedItem as ShortcutKey;
                if (oldShortcutKey != null)
                {
                    _keyboardShortcutMap.Remove(command, oldShortcutKey);
                }
                _keyboardShortcutMap.Add(command, newShortcutKey);
            }

            lstKeyboardShortcuts.Items[lstKeyboardShortcuts.SelectedIndex] = newShortcutKey;

            if (hadFocus)
            {
                hotModifyKeyboardShortcut.Focus();
                hotModifyKeyboardShortcut.Select(hotModifyKeyboardShortcut.TextLength, 0);
            }
        }

        #endregion

        private ShortcutCommand GetSelectedShortcutCommand()
        {
            if (!(lvKeyboardCommands.SelectedItems.Count == 1))
            {
                return ShortcutCommand.None;
            }

            var selectedItem = lvKeyboardCommands.SelectedItems[0];
            if (selectedItem == _previousTabListViewItem)
            {
                return ShortcutCommand.PreviousTab;
            }
            if (selectedItem == _nextTabListViewItem)
            {
                return ShortcutCommand.NextTab;
            }
            return ShortcutCommand.None;
        }

        private void EnableKeyboardShortcutControls(bool enable = true)
        {
            lblKeyboardCommand.Visible = enable;
            lblKeyboardShortcuts.Enabled = enable;
            lstKeyboardShortcuts.Enabled = enable;
            btnNewKeyboardShortcut.Enabled = enable;
            btnResetKeyboardShortcuts.Enabled = enable;
            if (!enable)
            {
                btnDeleteKeyboardShortcut.Enabled = false;
                grpModifyKeyboardShortcut.Enabled = false;
                hotModifyKeyboardShortcut.Enabled = false;
                lstKeyboardShortcuts.Items.Clear();
                hotModifyKeyboardShortcut.Text = string.Empty;
            }
        }

        #endregion
    }
}