using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SharedLibraryNG;


namespace mRemoteNG.Config.KeyboardShortcuts
{
	public class KeyboardShortcuts
	{
        private static KeyboardHook _keyboardHook = new KeyboardHook();
        private static bool _mapLoaded = false;
        private static IntPtr _handle = IntPtr.Zero;

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
}
