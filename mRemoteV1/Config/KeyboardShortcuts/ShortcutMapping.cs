using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace mRemoteNG.Config.KeyboardShortcuts
{
    [ImmutableObject(true)]
    public class ShortcutMapping : IEquatable<ShortcutMapping>
    {
        private ShortcutCommand _command;
        private ShortcutKey _key;

        public ShortcutCommand Command
        {
            get { return _command; }
        }

        public ShortcutKey Key
        {
            get { return _key; }
        }


        public ShortcutMapping(ShortcutCommand command, ShortcutKey key)
        {
            _command = command;
            _key = key;
        }


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
    }
}