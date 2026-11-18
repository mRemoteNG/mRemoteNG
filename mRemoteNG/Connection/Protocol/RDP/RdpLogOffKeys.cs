using System;
using System.Collections.Generic;

namespace mRemoteNG.Connection.Protocol.RDP
{
    /// <summary>
    /// The keystrokes that log a Windows RDP session off: Win+R opens the Run box, then
    /// "logoff" and Enter run logoff.exe, exactly as a user would type it.
    /// </summary>
    /// <remarks>
    /// Typing into the session was chosen over asking the server to end the session through the
    /// remote session API (WTSLogoffSession): that needs RPC access and rights on the server, which
    /// most networks don't allow from a workstation. Typing needs nothing extra, but it only works
    /// while the session accepts keystrokes: not at the lock screen, and not where Group Policy
    /// removes the Run box.
    ///
    /// The keys are sent as scan codes, i.e. physical key positions. The remote session turns them
    /// into letters with its own keyboard layout. "l o g f" sit in the same place on QWERTY, QWERTZ
    /// and AZERTY keyboards, but a layout such as Dvorak would type other letters.
    /// </remarks>
    internal static class RdpLogOffKeys
    {
        /// <summary>One key press or release, as sent to IMsRdpClientNonScriptable.SendKeys.</summary>
        internal readonly record struct Key(int KeyData, bool KeyUp);

        // Scan codes (set 1) of the keys used.
        private const int ScanR = 0x13;
        private const int ScanL = 0x26;
        private const int ScanO = 0x18;
        private const int ScanG = 0x22;
        private const int ScanF = 0x21;
        private const int ScanEnter = 0x1C;

        // The left Windows key is an "extended" key (E0 5B). The RDP control takes the scan code
        // with the extended-key flag above it, the same way it handles the arrow keys.
        private const int ExtendedKeyFlag = 0x100;
        private const int ScanLeftWindows = 0x5B | ExtendedKeyFlag;

        /// <summary>
        /// How long to wait after Win+R for the Run box to open before typing into it. Slow or busy
        /// servers can take a moment; typed too early, the letters would land in whatever had focus.
        /// </summary>
        internal static readonly TimeSpan RunBoxOpenDelay = TimeSpan.FromMilliseconds(1000);

        /// <summary>Win+R: Windows down, R down, R up, Windows up.</summary>
        internal static IReadOnlyList<Key> OpenRunBox { get; } =
        [
            new(ScanLeftWindows, false),
            new(ScanR, false),
            new(ScanR, true),
            new(ScanLeftWindows, true),
        ];

        /// <summary>"logoff" followed by Enter, each key pressed and released.</summary>
        internal static IReadOnlyList<Key> TypeLogOffAndEnter { get; } = Tap(ScanL, ScanO, ScanG, ScanO, ScanF, ScanF, ScanEnter);

        private static Key[] Tap(params int[] scanCodes)
        {
            List<Key> keys = new(scanCodes.Length * 2);
            foreach (int scanCode in scanCodes)
            {
                keys.Add(new Key(scanCode, false));
                keys.Add(new Key(scanCode, true));
            }

            return keys.ToArray();
        }
    }
}
