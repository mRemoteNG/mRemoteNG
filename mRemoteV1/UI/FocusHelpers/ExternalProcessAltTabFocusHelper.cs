using System;
using mRemoteNG.App;
using mRemoteNG.Messages;
using System.Windows.Forms;

namespace mRemoteNG.UI.FocusHelpers
{
    public class ExternalProcessAltTabFocusHelper : IDisposable
    {
        private readonly SystemKeyboardHook _keyboardHook;
        private bool _currentlyFixingAltTab;

        /// <summary>
        /// TRUE if any part of mrng has focus - the main window or child processes
        /// </summary>
        public bool MrngFocused { get; set; }
        public bool ChildProcessHeldLastFocus { get; set; }
        public Action FocusMainWindowAction { get; }
        public Action FocusConnectionAction { get; }

        public ExternalProcessAltTabFocusHelper(Action focusMainWindowAction, Action focusConnectionAction)
        {
            FocusMainWindowAction = focusMainWindowAction;
            FocusConnectionAction = focusConnectionAction;
            _keyboardHook = new SystemKeyboardHook(KeyboardHookCallback);
        }

        public int KeyboardHookCallback(int msg, NativeMethods.KBDLLHOOKSTRUCT kbd)
        {
            var key = (Keys)kbd.vkCode;
            if (key.HasFlag(Keys.Tab) && kbd.flags.HasFlag(NativeMethods.KBDLLHOOKSTRUCTFlags.LLKHF_ALTDOWN))
            {
                if (msg == NativeMethods.WM_SYSKEYDOWN || msg == NativeMethods.WM_KEYDOWN)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"ALT-TAB PRESSED (CHILDPROC_FOCUSED={ChildProcessHeldLastFocus}, MRNG_FOCUSED={MrngFocused}, CURR_FIXING={_currentlyFixingAltTab})");
                    if (ChildProcessHeldLastFocus && MrngFocused && !_currentlyFixingAltTab)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "FIXING ALT-TAB FOR EXTAPP");
                        _currentlyFixingAltTab = true;

                        // simulate an extra TAB key press. This skips focus of the mrng main window.
                        NativeMethods.keybd_event((byte)Keys.Tab, 0, (uint)NativeMethods.KEYEVENTF.KEYUP, 0);
                        NativeMethods.keybd_event((byte)Keys.Tab, 0, 0, 0);

                        // WndProc will never get an event when we switch from a child proc to a completely different program since the main mrng window never had focus to begin with.
                        // Assume mrng as a whole will lose focus, even though the user could choose to retain focus on us. When Alt-tab completes, the mrng main window will
                        // receive the focus event and we will handle the child process focusing as necessary.
                        MrngFocused = false;
                        _currentlyFixingAltTab = false;
                    }
                }
            }

            // alt + right-shift
            if (key.HasFlag(Keys.RShiftKey) && kbd.flags.HasFlag(NativeMethods.KBDLLHOOKSTRUCTFlags.LLKHF_ALTDOWN))
            {
                if (msg != NativeMethods.WM_SYSKEYUP && msg != NativeMethods.WM_KEYUP)
                    return 0;

                if (!MrngFocused)
                    return 0;

                if (ChildProcessHeldLastFocus)
                {
                    FocusMainWindowAction();
                    return 1;
                }

                FocusConnectionAction();
                return 1;
            }

            return 0;
        }

        public void Dispose()
        {
            _keyboardHook?.Dispose();
        }
    }
}
