using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using mRemoteNG.App;

namespace mRemoteNG.Connection.Protocol.RDP
{
    /// <summary>
    /// The keystrokes that log a Windows RDP session off: Win+R opens the Run box, then
    /// "shutdown /l /f" and Enter log the user off at once, exactly as a user would type it.
    /// </summary>
    /// <remarks>
    /// Why typing: asking the server to end the session through the remote session API
    /// (WTSLogoffSession) needs RPC access and rights on the server, which most networks don't
    /// allow from a workstation. Typing needs nothing extra, but it only works while the session
    /// accepts keystrokes: not at the lock screen, and not where Group Policy removes the Run box.
    ///
    /// Why forced: plain "logoff" lets open programs stop the log off to ask about unsaved work,
    /// which leaves the session hanging on a question nobody is looking at. "/f" closes them, like
    /// the old Resource Kit logoff tool. mRemoteNG asks before sending it, and warns that unsaved
    /// work is lost (see SessionLogOff).
    ///
    /// Keyboard layout: the control sends scan codes, i.e. physical keys, and the session turns them
    /// into characters with its keyboard layout, which RDP takes from the client by default. So each
    /// character is looked up in the client's layout: "/" is its own key on a US keyboard but
    /// Shift+7 on a German one.
    /// </remarks>
    [SupportedOSPlatform("windows")]
    internal static class RdpLogOffKeys
    {
        /// <summary>One key press or release, as sent to IMsRdpClientNonScriptable.SendKeys.</summary>
        internal readonly record struct Key(int KeyData, bool KeyUp);

        /// <summary>Logs the user off (/l) and closes open programs without asking (/f).</summary>
        internal const string LogOffCommand = "shutdown /l /f";

        // Keys that sit in the same place on every layout.
        private const int ScanEnter = 0x1C;
        private const int ScanLeftShift = 0x2A;
        private const int ScanLeftCtrl = 0x1D;
        private const int ScanLeftAlt = 0x38;

        // The left Windows key is an "extended" key (E0 5B). The RDP control takes the scan code with
        // the extended-key flag above it; confirmed by hand on a Windows Server session.
        private const int ExtendedKeyFlag = 0x100;
        private const int ScanLeftWindows = 0x5B | ExtendedKeyFlag;

        private const uint VirtualKeyR = 0x52;

        // VkKeyScanEx's high byte: which modifiers the character needs.
        private const int NeedsShift = 1;
        private const int NeedsCtrl = 2;
        private const int NeedsAlt = 4;

        /// <summary>
        /// How long to wait after Win+R for the Run box to open before typing into it. Slow or busy
        /// servers can take a moment; typed too early, the letters would land in whatever had focus.
        /// </summary>
        internal static readonly TimeSpan RunBoxOpenDelay = TimeSpan.FromMilliseconds(1000);

        /// <summary>Win+R on <paramref name="keyboardLayout"/>: Windows down, R down, R up, Windows up.</summary>
        internal static IReadOnlyList<Key> OpenRunBox(IntPtr keyboardLayout)
        {
            // Windows reads the shortcut by virtual key, so use the key that is "R" on this layout.
            int scanR = (int)NativeMethods.MapVirtualKeyEx(VirtualKeyR, NativeMethods.MAPVK_VK_TO_VSC, keyboardLayout);
            return
            [
                new(ScanLeftWindows, false),
                new(scanR, false),
                new(scanR, true),
                new(ScanLeftWindows, true),
            ];
        }

        /// <summary>
        /// <see cref="LogOffCommand"/> followed by Enter, typed on <paramref name="keyboardLayout"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The layout has no key for a character of the command.</exception>
        internal static IReadOnlyList<Key> TypeLogOffCommand(IntPtr keyboardLayout)
        {
            List<Key> keys = new();
            foreach (char character in LogOffCommand)
                keys.AddRange(Type(character, keyboardLayout));

            keys.Add(new Key(ScanEnter, false));
            keys.Add(new Key(ScanEnter, true));
            return keys;
        }

        /// <summary>
        /// The keys that type <paramref name="character"/>: modifiers down, the key down and up,
        /// modifiers up again (in reverse), so no modifier is left held in the session.
        /// </summary>
        private static IEnumerable<Key> Type(char character, IntPtr keyboardLayout)
        {
            short keyScan = NativeMethods.VkKeyScanEx(character, keyboardLayout);
            if (keyScan == -1)
                throw new InvalidOperationException($"The keyboard layout has no key for '{character}'.");

            uint virtualKey = (uint)(keyScan & 0xFF);
            int modifiers = (keyScan >> 8) & 0xFF;
            int scanCode = (int)NativeMethods.MapVirtualKeyEx(virtualKey, NativeMethods.MAPVK_VK_TO_VSC, keyboardLayout);

            List<int> held = new();
            if ((modifiers & NeedsShift) != 0) held.Add(ScanLeftShift);
            // Ctrl+Alt together is how AltGr characters are typed.
            if ((modifiers & NeedsCtrl) != 0) held.Add(ScanLeftCtrl);
            if ((modifiers & NeedsAlt) != 0) held.Add(ScanLeftAlt);

            foreach (int modifier in held)
                yield return new Key(modifier, false);

            yield return new Key(scanCode, false);
            yield return new Key(scanCode, true);

            for (int i = held.Count - 1; i >= 0; i--)
                yield return new Key(held[i], true);
        }
    }
}
