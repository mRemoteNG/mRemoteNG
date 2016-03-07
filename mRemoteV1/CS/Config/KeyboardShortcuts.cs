using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using mRemoteNG.App;
using SharedLibraryNG;


namespace mRemoteNG.Config
{
	public class KeyboardShortcuts
	{
        #region Public Properties
		private static KeyboardShortcutMap _defaultMap = null;
        public static KeyboardShortcutMap DefaultMap
		{
			get
			{
				LoadDefaultMap();
				return _defaultMap;
			}
		}
			
		private static KeyboardShortcutMap _map;
        public static KeyboardShortcutMap Map
		{
			get
			{
				Load();
				return _map;
			}
			set
			{
				CancelKeyNotifications();
				_map = value;
				Save();
				RequestKeyNotifications(_handle);
			}
		}
        #endregion
			
        #region Public Methods
		public static void RequestKeyNotifications(IntPtr handle)
		{
			// ReSharper disable LocalizableElement
			if (handle == IntPtr.Zero)
			{
				throw (new ArgumentException("The handle cannot be null.", "handle"));
			}
			if (!(_handle == IntPtr.Zero) && !(_handle == handle))
			{
				throw (new ArgumentException("The handle must match the handle that was specified the first time this function was called.", "handle"));
			}
			// ReSharper restore LocalizableElement
			_handle = handle;
			foreach (ShortcutMapping shortcutMapping in Map.Mappings)
			{
				KeyboardHook.RequestKeyNotification(handle, shortcutMapping.Key.KeyCode, shortcutMapping.Key.ModifierKeys, false);
			}
		}
			
		public static ShortcutCommand CommandFromHookKeyMessage(Message m)
		{
            KeyboardHook.HookKeyMsgData msgData = (SharedLibraryNG.KeyboardHook.HookKeyMsgData)Marshal.PtrToStructure(m.LParam, typeof(KeyboardHook.HookKeyMsgData));
			return Map.GetCommand(msgData.KeyCode, msgData.ModifierKeys);
		}
        #endregion
			
        #region Private Fields
		// ReSharper disable once UnusedMember.Local
		private static KeyboardHook _keyboardHook = new KeyboardHook();
		private static bool _mapLoaded = false;
		private static IntPtr _handle = IntPtr.Zero;
        #endregion
			
        #region Private Methods
		private static void LoadDefaultMap()
		{
			if (_defaultMap != null)
			{
				return ;
			}
			_defaultMap = new KeyboardShortcutMap();
			_defaultMap.AddFromConfigString(ShortcutCommand.PreviousTab, System.Convert.ToString(My.Settings.Default.Properties["KeysPreviousTab"].DefaultValue));
			_defaultMap.AddFromConfigString(ShortcutCommand.NextTab, System.Convert.ToString(My.Settings.Default.Properties["KeysNextTab"].DefaultValue));
		}
			
		private static void Load()
		{
			if (_mapLoaded)
			{
				return ;
			}
			_map = new KeyboardShortcutMap();
			_map.AddFromConfigString(ShortcutCommand.PreviousTab, System.Convert.ToString(My.Settings.Default.KeysPreviousTab));
			_map.AddFromConfigString(ShortcutCommand.NextTab, System.Convert.ToString(My.Settings.Default.KeysNextTab));
			_mapLoaded = true;
		}
			
		private static void Save()
		{
			if (_map == null)
			{
				return ;
			}
			My.Settings.Default.KeysPreviousTab = _map.GetConfigString(ShortcutCommand.PreviousTab);
			My.Settings.Default.KeysNextTab = _map.GetConfigString(ShortcutCommand.NextTab);
		}
			
		private static void CancelKeyNotifications()
		{
			if (_handle == IntPtr.Zero)
			{
				return ;
			}
			foreach (ShortcutMapping shortcutMapping in Map.Mappings)
			{
				KeyboardHook.CancelKeyNotification(_handle, shortcutMapping.Key.KeyCode, shortcutMapping.Key.ModifierKeys, false);
			}
		}
        #endregion
	}
		
	public class KeyboardShortcutMap : ICloneable
	{
			
        #region Public Properties
		private List<ShortcutMapping> _mappings;
        public List<ShortcutMapping> Mappings
		{
			get
			{
				return _mappings;
			}
		}
        #endregion
			
        #region Constructors
		public KeyboardShortcutMap()
		{
			_mappings = new List<ShortcutMapping>();
		}
			
		public KeyboardShortcutMap(List<ShortcutMapping> mappings)
		{
			_mappings = mappings;
		}
        #endregion
			
        #region Public Methods
		public void Add(ShortcutCommand command, ShortcutKey shortcutKey)
		{
			if (Mappings.Contains(new ShortcutMapping(command, shortcutKey)))
			{
				return ;
			}
			Mappings.Add(new ShortcutMapping(command, shortcutKey));
		}
			
		public void AddRange(ShortcutCommand command, IEnumerable<ShortcutKey> shortcutKeys)
		{
			foreach (ShortcutKey shortcutKey in shortcutKeys)
			{
				Add(command, shortcutKey);
			}
		}
			
		public void Remove(ShortcutCommand command, ShortcutKey shortcutKey)
		{
			Mappings.Remove(new ShortcutMapping(command, shortcutKey));
		}
			
		public void AddFromConfigString(ShortcutCommand command, string configString)
		{
			foreach (ShortcutKey shortcutKey in ParseConfigString(configString))
			{
				Add(command, shortcutKey);
			}
		}
			
		public string GetConfigString(ShortcutCommand command)
		{
			List<string> parts = new List<string>();
			foreach (ShortcutKey shortcutKey in GetShortcutKeys(command))
			{
				parts.Add(shortcutKey.ToConfigString());
			}
			return string.Join(", ", parts.ToArray());
		}
			
		public ShortcutKey[] GetShortcutKeys(ShortcutCommand command)
		{
			List<ShortcutKey> shortcutKeys = new List<ShortcutKey>();
			foreach (ShortcutMapping shortcutMapping in Mappings)
			{
				if (shortcutMapping.Command == command)
				{
					shortcutKeys.Add(shortcutMapping.Key);
				}
			}
			return shortcutKeys.ToArray();
		}
			
		public void SetShortcutKeys(ShortcutCommand command, IEnumerable<ShortcutKey> shortcutKeys)
		{
			ClearShortcutKeys(command);
			AddRange(command, shortcutKeys);
		}
			
		public ShortcutCommand GetCommand(int keyCode, KeyboardHook.ModifierKeys modifierKeys)
		{
			return GetCommand(new ShortcutKey(keyCode, modifierKeys));
		}
			
		public object Clone()
		{
			List<ShortcutMapping> newMappings = new List<ShortcutMapping>();
			newMappings.AddRange(Mappings);
			return new KeyboardShortcutMap(newMappings);
		}
        #endregion
			
        #region Private Methods
		private static ShortcutKey[] ParseConfigString(string shortcutKeysString)
		{
			List<ShortcutKey> shortcutKeys = new List<ShortcutKey>();
			foreach (string shortcutKeyString in shortcutKeysString.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
			{
				try
				{
					shortcutKeys.Add(ShortcutKey.FromConfigString(shortcutKeyString.Trim()));
				}
				catch (Exception ex)
				{
					Runtime.MessageCollector.AddExceptionMessage(message: string.Format("KeyboardShortcuts.ParseShortcutKeysString({0}) failed at {1}.", shortcutKeysString, shortcutKeyString), ex: ex, logOnly: true);
					continue;
				}
			}
			return shortcutKeys.ToArray();
		}
			
		private ShortcutCommand GetCommand(ShortcutKey shortcutKey)
		{
			foreach (ShortcutMapping shortcutMapping in Mappings)
			{
				if (ShortcutKeysMatch(shortcutMapping.Key, shortcutKey))
				{
					return shortcutMapping.Command;
				}
			}
			return ShortcutCommand.None;
		}
			
		private static bool 
			ShortcutKeysMatch(ShortcutKey wanted, ShortcutKey pressed)
		{
			if (!(wanted.KeyCode == pressed.KeyCode))
			{
				return false;
			}
			return KeyboardHook.ModifierKeysMatch(wanted.ModifierKeys, pressed.ModifierKeys);
		}
			
		private void ClearShortcutKeys(ShortcutCommand command)
		{
			List<ShortcutMapping> mappingsToRemove = new List<ShortcutMapping>();
			foreach (ShortcutMapping mapping in Mappings)
			{
				if (mapping.Command == command)
				{
					mappingsToRemove.Add(mapping);
				}
			}
				
			foreach (ShortcutMapping mapping in mappingsToRemove)
			{
				Mappings.Remove(mapping);
			}
		}
        #endregion
	}
		
	[ImmutableObject(true)]
    public class ShortcutMapping : IEquatable<ShortcutMapping>
	{
        #region Public Properties
		private ShortcutCommand _command;
        public ShortcutCommand Command
		{
			get
			{
				return _command;
			}
		}
			
		private ShortcutKey _key;
        public ShortcutKey Key
		{
			get
			{
				return _key;
			}
		}
        #endregion
			
        #region Constructors
		public ShortcutMapping(ShortcutCommand command, ShortcutKey key)
		{
			_command = command;
			_key = key;
		}
        #endregion
			
        #region Public Methods
		public bool Equals(ShortcutMapping other)
		{
			if (!(Command == other.Command))
			{
				return false;
			}
			if (!(Key == other.Key))
			{
				return false;
			}
			return true;
		}
        #endregion
	}
		
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
			_keyCode = (int) (keysValue & Keys.KeyCode);
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
			return string.Join("/", new string[] {KeyCode.ToString(), (Convert.ToInt32(ModifierKeys)).ToString()});
		}
			
		public static ShortcutKey FromConfigString(string shortcutKeyString)
		{
			string[] parts = shortcutKeyString.Split(new char[] {'/'}, 2);
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
			if (!(KeyCode == other.KeyCode))
			{
				return false;
			}
			if (!(ModifierKeys == other.ModifierKeys))
			{
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
		public static explicit operator Keys (ShortcutKey shortcutKey)
		{
			Keys keysValue = System.Windows.Forms.Keys.A;
			if (!((shortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Shift) == 0))
			{
				keysValue = (Keys) (keysValue | Keys.Shift);
			}
			if (!((shortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Control) == 0))
			{
				keysValue = (Keys) (keysValue | Keys.Control);
			}
			if (!((shortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Alt) == 0))
			{
				keysValue = (Keys) (keysValue | Keys.Alt);
			}
			return keysValue;
		}
			
		public static implicit operator ShortcutKey (Keys keys)
		{
			return new ShortcutKey(keys);
		}
        #endregion
	}
		
	public enum ShortcutCommand
	{
		None = 0,
		PreviousTab,
		NextTab
	}
}
