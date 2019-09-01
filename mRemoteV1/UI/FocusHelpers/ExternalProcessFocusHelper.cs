using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.UI.Tabs;

namespace mRemoteNG.UI.FocusHelpers
{
    public class ExternalProcessFocusHelper : IDisposable
    {
        private readonly IntPtr _mainWindowHandle;
        private int _extFocusCount;
        private readonly SystemKeyboardHook _keyboardHook;
        private bool _currentlyFixingAltTab;
        private bool _childProcessHeldLastFocus;
        private bool _mainWindowFocused;
        private bool _connectionReleasingFocus;

        /// <summary>
        /// TRUE if any part of mrng has focus - the main window or child processes
        /// </summary>
        public bool MrngFocused => MainWindowFocused || ChildProcessFocused;

        public bool FixingMainWindowFocus { get; private set; }

        public bool MainWindowFocused
        {
            get => _mainWindowFocused;
            set
            {
                if (_mainWindowFocused == value)
                    return;

                _mainWindowFocused = value;
                // main window is receiving focus
                if (ChildProcessHeldLastFocus && _mainWindowFocused && !_connectionReleasingFocus)
                    ActivateConnection();
            }
        }

        public bool ChildProcessFocused => _extFocusCount > 0;

        public bool ChildProcessHeldLastFocus
        {
            get => _childProcessHeldLastFocus;
            private set
            {
                _childProcessHeldLastFocus = value;
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"_childProcessHeldLastFocus={_childProcessHeldLastFocus}");
            }
        }

        public ExternalProcessFocusHelper(
            IConnectionInitiator connectionInitiator,
            IntPtr mainWindowHandle)
        {
            connectionInitiator.ConnectionStarting += ConnectionInitiatorOnConnectionStarting;
            _mainWindowHandle = mainWindowHandle;
            _keyboardHook = new SystemKeyboardHook(KeyboardHookCallback);
        }

        private void ConnectionInitiatorOnConnectionStarting(object sender, ConnectionStartingEvent e)
        {
            if (!(e.Protocol is ExternalProcessProtocolBase extProc))
                return;

            extProc.FocusChanged += ExtProcOnFocusChanged;
            extProc.Disconnected += ProtocolOnDisconnected;
        }

        private void ExtProcOnFocusChanged(object sender, EventArgs e)
        {
            if (!(sender is IFocusable extProc))
                return;

            if (extProc.HasFocus)
                ExternalWindowFocused();
            else
                ExternalWindowDefocused();
        }

        private void ProtocolOnDisconnected(object sender, string disconnectedmessage, int? reasoncode)
        {
            if (!(sender is ExternalProcessProtocolBase prot))
                return;

            prot.FocusChanged -= ExtProcOnFocusChanged;
            prot.Disconnected -= ProtocolOnDisconnected;
        }

        public void ActivateConnection()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Performing special connection focus logic");
            
            //var cw = pnlDock.ActiveDocument as ConnectionWindow;
            //var dp = cw?.ActiveControl as DockPane;

            //if (!(dp?.ActiveContent is ConnectionTab tab))
            //{
            //    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Active content is not a tab. We won't focus a specific connection.");
            //    return;
            //}

            //var ifc = InterfaceControl.FindInterfaceControl(tab);
            var tab = TabHelper.Instance.CurrentTab;
            if (tab == null)
                return;

            var ifc = tab.InterfaceControl;
            if (ifc == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                    $"InterfaceControl for tab '{tab.Name}' was not found. We won't focus that connection.");
                return;
            }

            //FixingMainWindowFocus = true;
            //_focusMainWindowAction();
            //FixingMainWindowFocus = false;

            ifc.Protocol.Focus();
            //var conFormWindow = ifc.FindForm();
            //((ConnectionTab) conFormWindow)?.RefreshInterfaceController();
        }

        public int KeyboardHookCallback(int msg, NativeMethods.KBDLLHOOKSTRUCT kbd)
        {
            var key = (Keys)kbd.vkCode;
            // Alt + Tab
            if (key.HasFlag(Keys.Tab) && kbd.flags.HasFlag(NativeMethods.KBDLLHOOKSTRUCTFlags.LLKHF_ALTDOWN))
            {
                if (msg == NativeMethods.WM_SYSKEYDOWN || msg == NativeMethods.WM_KEYDOWN)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"ALT-TAB PRESSED (CHILDPROC_FOCUSED={ChildProcessFocused}, CHILDPROC_LAST_FOCUSED={ChildProcessHeldLastFocus}, MRNG_FOCUSED={MrngFocused}, CURR_FIXING={_currentlyFixingAltTab})");
                    if (ChildProcessHeldLastFocus && MrngFocused && !_currentlyFixingAltTab)
                    {
                        FixExternalAppAltTab();
                    }
                }
            }

            // Alt + `
            if (key.HasFlag(Keys.Oem3) && kbd.flags.HasFlag(NativeMethods.KBDLLHOOKSTRUCTFlags.LLKHF_ALTDOWN))
            {
                if (msg != NativeMethods.WM_SYSKEYUP && msg != NativeMethods.WM_KEYUP)
                    return 0;

                if (ChildProcessFocused)
                {
                    _connectionReleasingFocus = true;
                    var focused = NativeMethods.SetForegroundWindow(_mainWindowHandle);
                    _connectionReleasingFocus = false;
                    return 1;
                }

                if (!MainWindowFocused)
                    return 0;
                ActivateConnection();
                return 1;
            }

            return 0;
        }

        private void FixExternalAppAltTab()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "FIXING ALT-TAB FOR EXTAPP");
            _currentlyFixingAltTab = true;

            // simulate an extra TAB key press. This skips focus of the mrng main window.
            NativeMethods.keybd_event((byte) Keys.Tab, 0, (uint) NativeMethods.KEYEVENTF.KEYUP, 0);
            NativeMethods.keybd_event((byte) Keys.Tab, 0, 0, 0);

            // WndProc will never get an event when we switch from a child proc to a completely different program since the main mrng window never had focus to begin with.
            // Assume mrng as a whole will lose focus, even though the user could choose to retain focus on us. When Alt-tab completes, the mrng main window will
            // receive the focus event and we will handle the child process focusing as necessary.
            MainWindowFocused = false;
            _currentlyFixingAltTab = false;
        }

        public void Dispose()
        {
            _keyboardHook?.Dispose();
        }

        private void ExternalWindowFocused()
        {
            _extFocusCount++;
        }

        private void ExternalWindowDefocused()
        {
            _extFocusCount--;
            ChildProcessHeldLastFocus = !MainWindowFocused && !ChildProcessFocused;
        }
    }
}
