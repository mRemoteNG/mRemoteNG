using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using mRemoteNG.App.Runtime;
using SharedLibraryNG;

namespace mRemoteNG.Config
{
	public class KeyboardShortcuts
	{
		#region "Public Properties"
		private static KeyboardShortcutMap _defaultMap = null;
		public static KeyboardShortcutMap DefaultMap {
			get {
				LoadDefaultMap();
				return _defaultMap;
			}
		}

		private static KeyboardShortcutMap _map;
		public static KeyboardShortcutMap Map {
			get {
				Load();
				return _map;
			}
			set {
				CancelKeyNotifications();
				_map = value;
				Save();
				RequestKeyNotifications(_handle);
			}
		}
		#endregion

		#region "Public Methods"
		public static void RequestKeyNotifications(IntPtr handle)
		{
			// ReSharper disable LocalizableElement
			if (handle == IntPtr.Zero)
				throw new ArgumentException("The handle cannot be null.", "handle");
			if (!(_handle == IntPtr.Zero) & !(_handle == handle))
				throw new ArgumentException("The handle must match the handle that was specified the first time this function was called.", "handle");
			// ReSharper restore LocalizableElement
			_handle = handle;
			foreach (ShortcutMapping shortcutMapping in Map.Mappings) {
				KeyboardHook.RequestKeyNotification(handle, shortcutMapping.Key.KeyCode, shortcutMapping.Key.ModifierKeys, false);
			}
		}

		public static ShortcutCommand CommandFromHookKeyMessage(Message m)
		{
			SharedLibraryNG.KeyboardHook.HookKeyMsgData msgData = Marshal.PtrToStructure(m.LParam, typeof(KeyboardHook.HookKeyMsgData));
			return Map.GetCommand(msgData.KeyCode, msgData.ModifierKeys);
		}
		#endregion

		#region "Private Fields"
		// ReSharper disable once UnusedMember.Local
		private static KeyboardHook _keyboardHook = new KeyboardHook();
		private static bool _mapLoaded = false;
			#endregion
		private static IntPtr _handle = IntPtr.Zero;

		#region "Private Methods"
		private static void LoadDefaultMap()
		{
			if (_defaultMap != null)
				return;
			_defaultMap = new KeyboardShortcutMap();
			_defaultMap.AddFromConfigString(ShortcutCommand.PreviousTab, mRemoteNG.My.Settings.Properties("KeysPreviousTab").DefaultValue);
			_defaultMap.AddFromConfigString(ShortcutCommand.NextTab, mRemoteNG.My.Settings.Properties("KeysNextTab").DefaultValue);
		}

		private static void Load()
		{
			if (_mapLoaded)
				return;
			_map = new KeyboardShortcutMap();
			_map.AddFromConfigString(ShortcutCommand.PreviousTab, mRemoteNG.My.Settings.KeysPreviousTab);
			_map.AddFromConfigString(ShortcutCommand.NextTab, mRemoteNG.My.Settings.KeysNextTab);
			_mapLoaded = true;
		}

		private static void Save()
		{
			if (_map == null)
				return;
			mRemoteNG.My.Settings.KeysPreviousTab = _map.GetConfigString(ShortcutCommand.PreviousTab);
			mRemoteNG.My.Settings.KeysNextTab = _map.GetConfigString(ShortcutCommand.NextTab);
		}

		private static void CancelKeyNotifications()
		{
			if (_handle == IntPtr.Zero)
				return;
			foreach (ShortcutMapping shortcutMapping in Map.Mappings) {
				KeyboardHook.CancelKeyNotification(_handle, shortcutMapping.Key.KeyCode, shortcutMapping.Key.ModifierKeys, false);
			}
		}
		#endregion
	}

	public class KeyboardShortcutMap : ICloneable
	{

		#region "Public Properties"
		private readonly List<ShortcutMapping> _mappings;
		public List<ShortcutMapping> Mappings {
			get { return _mappings; }
		}
		#endregion

		#region "Constructors"
		public KeyboardShortcutMap()
		{
			_mappings = new List<ShortcutMapping>();
		}

		public KeyboardShortcutMap(List<ShortcutMapping> mappings)
		{
			_mappings = mappings;
		}
		#endregion

		#region "Public Methods"
		public void Add(ShortcutCommand command, ShortcutKey shortcutKey)
		{
			if (Mappings.Contains(new ShortcutMapping(command, shortcutKey)))
				return;
			Mappings.Add(new ShortcutMapping(command, shortcutKey));
		}

		public void AddRange(ShortcutCommand command, IEnumerable<ShortcutKey> shortcutKeys)
		{
			foreach (ShortcutKey shortcutKey in shortcutKeys) {
				Add(command, shortcutKey);
			}
		}

		public void Remove(ShortcutCommand command, ShortcutKey shortcutKey)
		{
			Mappings.Remove(new ShortcutMapping(command, shortcutKey));
		}

		public void AddFromConfigString(ShortcutCommand command, string configString)
		{
			foreach (ShortcutKey shortcutKey in ParseConfigString(configString)) {
				Add(command, shortcutKey);
			}
		}

		public string GetConfigString(ShortcutCommand command)
		{
			List<string> parts = new List<string>();
			foreach (ShortcutKey shortcutKey in GetShortcutKeys(command)) {
				parts.Add(shortcutKey.ToConfigString());
			}
			return string.Join(", ", parts.ToArray());
		}

		public ShortcutKey[] GetShortcutKeys(ShortcutCommand command)
		{
			List<ShortcutKey> shortcutKeys = new List<ShortcutKey>();
			foreach (ShortcutMapping shortcutMapping in Mappings) {
				if (shortcutMapping.Command == command)
					shortcutKeys.Add(shortcutMapping.Key);
			}
			return shortcutKeys.ToArray();
		}

		public void SetShortcutKeys(ShortcutCommand command, IEnumerable<ShortcutKey> shortcutKeys)
		{
			ClearShortcutKeys(command);
			AddRange(command, shortcutKeys);
		}

		public ShortcutCommand GetCommand(Int32 keyCode, SharedLibraryNG.KeyboardHook.ModifierKeys modifierKeys)
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

		#region "Private Methods"
		private static ShortcutKey[] ParseConfigString(string shortcutKeysString)
		{
			List<ShortcutKey> shortcutKeys = new List<ShortcutKey>();
			foreach (string shortcutKeyString in shortcutKeysString.Split(new char[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
				try {
					shortcutKeys.Add(ShortcutKey.FromConfigString(shortcutKeyString.Trim()));
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(string.Format("KeyboardShortcuts.ParseShortcutKeysString({0}) failed at {1}.", shortcutKeysString, shortcutKeyString), ex, , true);
					continue;
				}
			}
			return shortcutKeys.ToArray();
		}

		private ShortcutCommand GetCommand(ShortcutKey shortcutKey)
		{
			foreach (ShortcutMapping shortcutMapping in Mappings) {
				if (ShortcutKeysMatch(shortcutMapping.Key, shortcutKey))
					return shortcutMapping.Command;
			}
			return ShortcutCommand.None;
		}

		private static bool ShortcutKeysMatch(ShortcutKey wanted, ShortcutKey pressed)
		{
			if (!(wanted.KeyCode == pressed.KeyCode))
				return false;
			return KeyboardHook.ModifierKeysMatch(wanted.ModifierKeys, pressed.ModifierKeys);
		}

		private void ClearShortcutKeys(ShortcutCommand command)
		{
			List<ShortcutMapping> mappingsToRemove = new List<ShortcutMapping>();
			foreach (ShortcutMapping mapping in Mappings) {
				if (mapping.Command == command)
					mappingsToRemove.Add(mapping);
			}

			foreach (ShortcutMapping mapping in mappingsToRemove) {
				Mappings.Remove(mapping);
			}
		}
		#endregion
	}

	[ImmutableObject(true)]
	public class ShortcutMapping : IEquatable<ShortcutMapping>
	{

		#region "Public Properties"
		private readonly ShortcutCommand _command;
		public ShortcutCommand Command {
			get { return _command; }
		}

		private readonly ShortcutKey _key;
		public ShortcutKey Key {
			get { return _key; }
		}
		#endregion

		#region "Constructors"
		public ShortcutMapping(ShortcutCommand command, ShortcutKey key)
		{
			_command = command;
			_key = key;
		}
		#endregion

		#region "Public Methods"
		public bool Equals(ShortcutMapping other)
		{
			if (!(Command == other.Command))
				return false;
			if (!(Key == other.Key))
				return false;
			return true;
		}
		#endregion
	}

	[ImmutableObject(true)]
	public class ShortcutKey : IEquatable<ShortcutKey>
	{

		#region "Public Properties"
		private readonly Int32 _keyCode;
		public Int32 KeyCode {
			get { return _keyCode; }
		}

		private readonly SharedLibraryNG.KeyboardHook.ModifierKeys _modifierKeys;
		public KeyboardHook.ModifierKeys ModifierKeys {
			get { return _modifierKeys; }
		}
		#endregion

		#region "Constructors"
		public ShortcutKey(Int32 keyCode, SharedLibraryNG.KeyboardHook.ModifierKeys modifierKeys)
		{
			_keyCode = keyCode;
			_modifierKeys = modifierKeys;
		}

		public ShortcutKey(Keys keysValue)
		{
			_keyCode = keysValue & Keys.KeyCode;
			if (!((keysValue & Keys.Shift) == 0))
				_modifierKeys = _modifierKeys | KeyboardHook.ModifierKeys.Shift;
			if (!((keysValue & Keys.Control) == 0))
				_modifierKeys = _modifierKeys | KeyboardHook.ModifierKeys.Control;
			if (!((keysValue & Keys.Alt) == 0))
				_modifierKeys = _modifierKeys | KeyboardHook.ModifierKeys.Alt;
		}
		#endregion

		#region "Public Methods"
		public string ToConfigString()
		{
			return string.Join("/", new string[] {
				KeyCode,
				Convert.ToInt32(ModifierKeys)
			});
		}

		public static ShortcutKey FromConfigString(string shortcutKeyString)
		{
			string[] parts = shortcutKeyString.Split(new char[] { "/" }, 2);
			if (!(parts.Length == 2))
				throw new ArgumentException(string.Format("ShortcutKey.FromString({0}) failed. parts.Length != 2", shortcutKeyString), shortcutKeyString);
			return new ShortcutKey(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
		}

		public override string ToString()
		{
			return HotkeyControl.KeysToString(this);
		}

		public bool Equals(ShortcutKey other)
		{
			if (!(KeyCode == other.KeyCode))
				return false;
			if (!(ModifierKeys == other.ModifierKeys))
				return false;
			return true;
		}
		#endregion

		#region "Operators"
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
			Keys keysValue = ShortcutKey.KeyCode & Keys.KeyCode;
			if (!((ShortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Shift) == 0))
				keysValue = keysValue | Keys.Shift;
			if (!((ShortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Control) == 0))
				keysValue = keysValue | Keys.Control;
			if (!((ShortcutKey.ModifierKeys & KeyboardHook.ModifierKeys.Alt) == 0))
				keysValue = keysValue | Keys.Alt;
			return keysValue;
		}

		public static implicit operator ShortcutKey(Keys keys)
		{
			return new ShortcutKey(Keys);
		}
		#endregion
	}

	public enum ShortcutCommand : int
	{
		None = 0,
		PreviousTab,
		NextTab
	}
}
