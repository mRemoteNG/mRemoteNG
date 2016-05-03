using mRemoteNG.App;
using SharedLibraryNG;
using System;
using System.Collections.Generic;

namespace mRemoteNG.Config.KeyboardShortcuts
{
    public class KeyboardShortcutMap : ICloneable
    {
        private List<ShortcutMapping> _mappings;


        public List<ShortcutMapping> Mappings
        {
            get
            {
                return _mappings;
            }
        }


        public KeyboardShortcutMap()
        {
            _mappings = new List<ShortcutMapping>();
        }

        public KeyboardShortcutMap(List<ShortcutMapping> mappings)
        {
            _mappings = mappings;
        }


        public void Add(ShortcutCommand command, ShortcutKey shortcutKey)
        {
            if (Mappings.Contains(new ShortcutMapping(command, shortcutKey)))
            {
                return;
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

        private static ShortcutKey[] ParseConfigString(string shortcutKeysString)
        {
            List<ShortcutKey> shortcutKeys = new List<ShortcutKey>();
            foreach (string shortcutKeyString in shortcutKeysString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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

        private static bool ShortcutKeysMatch(ShortcutKey wanted, ShortcutKey pressed)
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
    }
}