using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

//
// Hotkey selection control, written by serenity@exscape.org, 2006-08-03
// Please mail me if you find a bug.
//

namespace SharedLibraryNG
{
    /// <summary>
    /// A simple control that allows the user to select pretty much any valid hotkey combination
    /// </summary>
    public class HotkeyControl : TextBox
    {
	    private const string KeySeparator = " + ";

        // These variables store the current hotkey and modifier(s)
        private Keys _keyCode = Keys.None;
        private Keys _modifiers = Keys.None;

        // ArrayLists used to enforce the use of proper modifiers.
        // Shift+A isn't a valid hotkey, for instance, as it would screw up when the user is typing.
        private readonly ArrayList _needNonShiftModifier;
        private readonly ArrayList _needNonAltGrModifier;

        private readonly ContextMenu _emptyContextMenu = new ContextMenu();

        /// <summary>
        /// Used to make sure that there is no right-click menu available
        /// </summary>
        public override ContextMenu ContextMenu
        {
            get
            {
                return _emptyContextMenu;
            }
			// ReSharper disable once ValueParameterNotUsed
			set
            {
                base.ContextMenu = _emptyContextMenu;
            }
        }

        /// <summary>
        /// Forces the control to be non-multiline
        /// </summary>
        public override bool Multiline
        {
            get
            {
                return base.Multiline;
            }
			// ReSharper disable once ValueParameterNotUsed
            set
            {
                // Ignore what the user wants; force Multiline to false
                base.Multiline = false;
            }
        }

        /// <summary>
        /// Creates a new HotkeyControl
        /// </summary>
        public HotkeyControl()
        {
            // Handle events that occurs when keys are pressed
            KeyUp += HotkeyControl_KeyUp;

            // Fill the ArrayLists that contain all invalid hotkey combinations
            _needNonShiftModifier = new ArrayList();
            _needNonAltGrModifier = new ArrayList();
            PopulateModifierLists();
        }

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			ContextMenu = _emptyContextMenu; // Disable right-clicking
			Multiline = false;
			Text = "None";
		}

        /// <summary>
        /// Populates the ArrayLists specifying disallowed hotkeys
        /// such as Shift+A, Ctrl+Alt+4 (would produce a dollar sign) etc
        /// </summary>
        private void PopulateModifierLists()
        {
            // Shift + 0 - 9, A - Z
            for (var k = Keys.D0; k <= Keys.Z; k++)
                _needNonShiftModifier.Add((int)k);

            // Shift + Numpad keys
            for (var k = Keys.NumPad0; k <= Keys.NumPad9; k++)
                _needNonShiftModifier.Add((int)k);

            // Shift + Misc (,;<./ etc)
            for (var k = Keys.Oem1; k <= Keys.OemBackslash; k++)
                _needNonShiftModifier.Add((int)k);

            // Misc keys that we can't loop through
            _needNonShiftModifier.Add((int)Keys.Insert);
            _needNonShiftModifier.Add((int)Keys.Help);
            _needNonShiftModifier.Add((int)Keys.Multiply);
            _needNonShiftModifier.Add((int)Keys.Add);
            _needNonShiftModifier.Add((int)Keys.Subtract);
            _needNonShiftModifier.Add((int)Keys.Divide);
            _needNonShiftModifier.Add((int)Keys.Decimal);
            _needNonShiftModifier.Add((int)Keys.Return);
            _needNonShiftModifier.Add((int)Keys.Escape);
            _needNonShiftModifier.Add((int)Keys.NumLock);
            _needNonShiftModifier.Add((int)Keys.Scroll);
            _needNonShiftModifier.Add((int)Keys.Pause);

            // Ctrl+Alt + 0 - 9
            for (var k = Keys.D0; k <= Keys.D9; k++)
                _needNonAltGrModifier.Add((int)k);
        }

        /// <summary>
        /// Fires when all keys are released. If the current hotkey isn't valid, reset it.
        /// Otherwise, do nothing and keep the text and hotkey as it was.
        /// </summary>
        void HotkeyControl_KeyUp(object sender, KeyEventArgs e)
        {
			if (_keyCode == Keys.None && ModifierKeys == Keys.None) ResetHotkey();
        }

        /// <summary>
        /// Handles some misc keys, such as Ctrl+Delete and Shift+Insert
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
			var keyCode = keyData & Keys.KeyCode;
			var modifiers = keyData & Keys.Modifiers;

			if (keyData == Keys.Back || keyData == Keys.Delete)
            {
                ResetHotkey();
                return true;
            }

			_keyCode = keyCode;
			_modifiers = modifiers;
			Redraw();

			return true;
        }

        /// <summary>
        /// Clears the current hotkey and resets the TextBox
        /// </summary>
        public void ResetHotkey()
        {
            _keyCode = Keys.None;
            _modifiers = Keys.None;
			Text = "None";
        }

        /// <summary>
        /// Used to get/set the hotkey (e.g. Keys.A)
        /// </summary>
        public Keys KeyCode
        {
            get
            {
                return _keyCode;
            }
            set
            {
                _keyCode = value;
                Redraw(false);
            }
        }

        /// <summary>
        /// Used to get/set the modifier keys (e.g. Keys.Alt | Keys.Control)
        /// </summary>
        public Keys HotkeyModifiers
        {
            get
            {
                return _modifiers;
            }
            set
            {
                _modifiers = value;
                Redraw(false);
            }
        }

	    /// <summary>
        /// Redraws the TextBox when necessary.
        /// </summary>
        /// <param name="validate">Specifies whether this function was called by the Hotkey/HotkeyModifiers properties or by the user.</param>
        private void Redraw(bool validate = true)
        {
            // Only validate input if it comes from the user
            if (validate)
            {
                // No modifier or shift only, AND a hotkey that needs another modifier
	            if ((_modifiers == Keys.Shift || _modifiers == Keys.None) &&
	                _needNonShiftModifier.Contains((int) _keyCode))
	            {
		            if (_modifiers == Keys.None)
		            {
			            // Set Ctrl+Alt as the modifier unless Ctrl+Alt+<key> won't work...
			            if (_needNonAltGrModifier.Contains((int) _keyCode) == false)
				            _modifiers = Keys.Alt | Keys.Control;
			            else // ... in that case, use Shift+Alt instead.
				            _modifiers = Keys.Alt | Keys.Shift;
		            }
		            else
		            {
			            // User pressed Shift and an invalid key (e.g. a letter or a number),
			            // that needs another set of modifier keys
			            _keyCode = Keys.None;
			            Text = _modifiers + " + Invalid Key";
			            return;
		            }
	            }
	            // Check all Ctrl+Alt keys
	            if ((_modifiers == (Keys.Alt | Keys.Control)) &&
	                _needNonAltGrModifier.Contains((int) _keyCode))
	            {
		            // Ctrl+Alt+4 etc won't work; reset hotkey and tell the user
		            _keyCode = Keys.None;
		            Text = _modifiers + " + Invalid Key";
		            return;
	            }
            }

			// Don't allow modifiers keys for _keyCode
			if (_keyCode == Keys.ShiftKey ||
				_keyCode == Keys.LShiftKey ||
				_keyCode == Keys.RShiftKey ||
				_keyCode == Keys.ControlKey ||
				_keyCode == Keys.LControlKey ||
				_keyCode == Keys.RControlKey ||
				_keyCode == Keys.Menu ||
				_keyCode == Keys.LMenu ||
				_keyCode == Keys.RMenu ||
				_keyCode == Keys.LWin ||
				_keyCode == Keys.RWin)
				_keyCode = Keys.None;

			if (_modifiers == Keys.None)
            {
                if (_keyCode == Keys.None)
                {
                    ResetHotkey();
                    return;
                }

                // We get here if we've got a hotkey that is valid without a modifier,
                // like F1-F12, Media-keys etc.
                Text = _keyCode.ToString();
                return;
            }

			Text = string.Join(KeySeparator, new[] { KeysToString(_modifiers), KeysToString(_keyCode) });
        }

	    public static string KeysToString(Keys keys)
	    {
			if (keys == Keys.None) return "None";

		    var modifiers = (keys & Keys.Modifiers);
		    var keyCode = (keys & Keys.KeyCode);

		    var strings = new List<string>();

			if (modifiers != 0)
			{
				var modifierStrings = new List<string>(modifiers.ToString().Replace(", ", ",").Split(','));
				modifierStrings.Sort(new KeyModifierComparer());
				strings.AddRange(modifierStrings);
			}

			if (keyCode != 0)
			{
				var keyString = keyCode.ToString();
				var keyHashtable = new Dictionary<string, string>
					{
						{"Next", "PageDown"},
						{"Oemcomma", ","},
						{"OemMinus", "-"},
						{"OemOpenBrackets", "["},
						{"OemPeriod", "."},
						{"Oemplus", "="},
						{"OemQuestion", "/"},
						{"Oemtilde", "`"},
						{"D0", "0"},
						{"D1", "1"},
						{"D2", "2"},
						{"D3", "3"},
						{"D4", "4"},
						{"D5", "5"},
						{"D6", "6"},
						{"D7", "7"},
						{"D8", "8"},
						{"D9", "9"},
					};
				if (keyHashtable.ContainsKey(keyString)) keyString = keyHashtable[keyString];
				strings.Add(keyString);
			}

			return string.Join(KeySeparator, strings.ToArray());
	    }

	    private class KeyModifierComparer : IComparer<string>
	    {
		    private static readonly List<string> ModifierOrder = new List<string>
			    {
				    "control",
				    "alt",
				    "shift",
			    };

			public int Compare(string x, string y)
			{
				var xIndex = ModifierOrder.IndexOf(x.ToLowerInvariant());
				var yIndex = ModifierOrder.IndexOf(y.ToLowerInvariant());
				return xIndex - yIndex;
			}
		}
    }
}
