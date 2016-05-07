using mRemoteNG.App;
using mRemoteNG.Messages;
using SharedLibraryNG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.Config.KeyboardShortcuts
{
    [ImmutableObject(true)]
    public class ShortcutKey : IEquatable<ShortcutKey>
    {
        #region Public Properties
        private int _keyCode;
        public int KeyCode
        {
            get
            {
                return _keyCode;
            }
        }

        private KeyboardHook.ModifierKeys _modifierKeys;
        public KeyboardHook.ModifierKeys ModifierKeys
        {
            get
            {
                return _modifierKeys;
            }
        }
        #endregion

        #region Constructors
        public ShortcutKey(int keyCode, KeyboardHook.ModifierKeys modifierKeys)
        {
            _keyCode = keyCode;
            _modifierKeys = modifierKeys;
        }

        public ShortcutKey(Keys keysValue)
        {
            _keyCode = (int)(keysValue & Keys.KeyCode);
            if (!((keysValue & Keys.Shift) == 0))
            {
                _modifierKeys = _modifierKeys | KeyboardHook.ModifierKeys.Shift;
            }
            if (!((keysValue & Keys.Control) == 0))
            {
                _modifierKeys = _modifierKeys | KeyboardHook.ModifierKeys.Control;
            }
            if (!((keysValue & Keys.Alt) == 0))
            {
                _modifierKeys = _modifierKeys | KeyboardHook.ModifierKeys.Alt;
            }
        }
        #endregion

        #region Public Methods
        public string ToConfigString()
        {
            return string.Join("/", new string[] { KeyCode.ToString(), (Convert.ToInt32(ModifierKeys)).ToString() });
        }

        public static ShortcutKey FromConfigString(string shortcutKeyString)
        {
            string[] parts = shortcutKeyString.Split(new char[] { '/' }, 2);
            if (!(parts.Length == 2))
            {
                throw (new ArgumentException(string.Format("ShortcutKey.FromString({0}) failed. parts.Length != 2", shortcutKeyString), shortcutKeyString));
            }
            return new ShortcutKey(Convert.ToInt32(parts[0]), (SharedLibraryNG.KeyboardHook.ModifierKeys)Convert.ToInt32(parts[1]));
        }

        public override string ToString()
        {
            return HotkeyControl.KeysToString((System.Windows.Forms.Keys)this.KeyCode);
        }

        public bool Equals(ShortcutKey other)
        {
            try
            {
                if (KeyCode != other.KeyCode)
                    return false;
                if (ModifierKeys != other.ModifierKeys)
                    return false;
            }
            catch (NullReferenceException e)
            {
                Runtime.MessageCollector.AddExceptionMessage("Encountered Exception", e);
                return false;
            }
            return true;
        }
        #endregion

        #region Operators
        public static bool operator ==(ShortcutKey shortcutKey1, ShortcutKey shortcutKey2)
        {
            return shortcutKey1.Equals(shortcutKey2);
        }

        public static bool operator !=(ShortcutKey shortcutKey1, ShortcutKey shortcutKey2)
        {
            return !shortcutKey1.Equals(shortcutKey2);
        }

        // This is a narrowing conversion because (Keys Or Keys.Modifiers) cannot hold all possible values of KeyboardHook.ModifierKeys
        public static explicit operator Keys(ShortcutKey shortcutKey)
        {
            Keys keysValue = System.Windows.Forms.Keys.A;
            if (!((shortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Shift) == 0))
            {
                keysValue = (Keys)(keysValue | Keys.Shift);
            }
            if (!((shortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Control) == 0))
            {
                keysValue = (Keys)(keysValue | Keys.Control);
            }
            if (!((shortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Alt) == 0))
            {
                keysValue = (Keys)(keysValue | Keys.Alt);
            }
            return keysValue;
        }

        public static implicit operator ShortcutKey(Keys keys)
        {
            return new ShortcutKey(keys);
        }
        #endregion
    }
}