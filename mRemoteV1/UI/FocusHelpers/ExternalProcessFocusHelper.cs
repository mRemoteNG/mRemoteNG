using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.UI.Tabs;
using Message = System.Windows.Forms.Message;

namespace mRemoteNG.UI.FocusHelpers
{
    public class ExternalProcessFocusHelper : IDisposable
    {
        private readonly IntPtr _mainWindowHandle;
        private int _extFocusCount;
        private readonly SystemKeyboardHook _keyboardHook;
        private bool _currentlyFixingAltTab;
        private bool _childProcessHeldLastFocus;
        private bool _inSizeMove;
        private bool _fixingMainWindowFocus;

        /// <summary>
        /// TRUE if any part of mrng has focus - the main window or child processes
        /// </summary>
        public bool MrngFocused => MainWindowFocused || ChildProcessFocused;

        /// <summary>
        /// TRUE when the main mRemoteNG window has focus.
        /// </summary>
        public bool MainWindowFocused { get; private set; }

        /// <summary>
        /// TRUE when a child process (not the mRemoteNG main window) has focus.
        /// </summary>
        public bool ChildProcessFocused => _extFocusCount > 0;

        /// <summary>
        /// TRUE when a child process was the last window within mRemoteNG
        /// to have focus.
        /// </summary>
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

        private void ProtocolOnDisconnected(object sender, string disconnectedMessage, int? reasonCode)
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

            ifc.Protocol.Focus();
            var conFormWindow = ifc.FindForm();
            ((ConnectionTab)conFormWindow)?.RefreshInterfaceController();
        }

        /// <summary>
        /// Toggle focus between the main window and the last active external app.
        /// </summary>
        public void ToggleFocus()
        {
            if (ChildProcessFocused)
            {
                ForceExtAppToGiveUpFocus();
                var focused = NativeMethods.SetForegroundWindow(_mainWindowHandle);
            }
            else if (MainWindowFocused)
            {
                ActivateConnection();
            }
        }

        /// <summary>
        /// A hook into system keyboard events. 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="kbd"></param>
        /// <remarks>
        /// This is the only way we can detect ALT-TAB events on Win10, since the EVENT_SYSTEM_SWITCHSTART and
        /// EVENT_SYSTEM_SWITCHEND events do not seem to fire. If there is another way to detect that a user
        /// is switching between apps using ALT-TAB, then we should try to use that. Hooking into keyboard
        /// events like this feels dirty, as it is often used by malicious programs.
        /// </remarks>
        /// <returns></returns>
        private int KeyboardHookCallback(int msg, NativeMethods.KBDLLHOOKSTRUCT kbd)
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

            return 0;
        }

        /// <summary>
        /// Ensure that focus events on the main window are allowed to complete
        /// without the external app stealing focus away.
        /// </summary>
        private void ForceExtAppToGiveUpFocus()
        {
            if (!ChildProcessHeldLastFocus)
                return;

            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Forcing extpp to give up focus.");
            ChildProcessHeldLastFocus = false;
        }

        /// <summary>
        /// When we alt-tab and an external app has focus, 
        /// </summary>
        private void FixExternalAppAltTab()
        {
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "FIXING ALT-TAB FOR EXTAPP");
            _currentlyFixingAltTab = true;

            // simulate an extra TAB key press. This skips focus of the mrng main window.
            // end the current alt-tab key press
            NativeMethods.keybd_event((byte) Keys.Tab, 0, (uint) NativeMethods.KEYEVENTF.KEYUP, 0);
            // start a new alt-tab key press
            NativeMethods.keybd_event((byte) Keys.Tab, 0, 0, 0);

            // WndProc will never get an event when we switch from a child proc to a completely different program since the main mrng window never had focus to begin with.
            // Assume mrng as a whole will lose focus, even though the user could choose to retain focus on us. When Alt-tab completes, the mrng main window will
            // receive the focus event and we will handle the child process focusing as necessary.
            MainWindowFocused = false;
            _currentlyFixingAltTab = false;
        }

        private void ExternalWindowFocused()
        {
            _extFocusCount++;
            ChildProcessHeldLastFocus = true;
        }

        private void ExternalWindowDefocused()
        {
            _extFocusCount--;
            ChildProcessHeldLastFocus = !MainWindowFocused && !ChildProcessFocused;
        }

        /// <summary>
        /// Give the <see cref="ExternalProcessFocusHelper"/> a chance to
        /// hook into a system window's WndProc message handler. Returns
        /// true if an action has been taken which should override the
        /// base windows own WndProc handling.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="baseWindowProcessMessage">
        /// Call this with the current message to allow the default window handler
        /// to process the message. Useful if you want to perform some work after
        /// the default processing is complete.
        /// </param>
        /// <returns>
        /// Returns true if an action has been taken which should override the
        /// base windows own WndProc handling.
        /// </returns>
        public bool HandleWndProc(ref Message m, Action<Message> baseWindowProcessMessage)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (m.Msg)
            {
                case NativeMethods.WM_PARENTNOTIFY:
                    var notifyType = m.WParam.ToInt32();
                    // ignore non-click notify events
                    if (notifyType == NativeMethods.WM_CREATE || notifyType == NativeMethods.WM_DESTROY)
                        break;

                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Main window clicked");
                    // when the main window is clicked, assume the user wants the main window focused
                    ForceExtAppToGiveUpFocus();
                    break;
                case NativeMethods.WM_ACTIVATEAPP:
                    if (_fixingMainWindowFocus)
                        break;

                    // main window is being deactivated
                    if (m.WParam.ToInt32() == 0)
                    {
                        MainWindowFocused = false;
                        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"mRemoteNG main window lost focus (_childProcessHeldLastFocus={ChildProcessHeldLastFocus})");
                        break;
                    }

                    MainWindowFocused = true;
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"mRemoteNG main window received focus (_childProcessHeldLastFocus={ChildProcessHeldLastFocus})");

                    break;
                case NativeMethods.WM_SETFOCUS:
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "WM_SETFOCUS");
                    _fixingMainWindowFocus = true;
                    baseWindowProcessMessage(m);
                    _fixingMainWindowFocus = false;
                    if (ChildProcessHeldLastFocus && MainWindowFocused)
                        ActivateConnection();
                    return true;
                case NativeMethods.WM_ACTIVATE:
                    if (m.WParam.ToInt32() == NativeMethods.WA_INACTIVE)
                        break;
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "WM_ACTIVATE");
                    break;

                case NativeMethods.WM_NCACTIVATE:
                    // Never allow the mRemoteNG window to display itself as inactive. By doing this,
                    // we ensure focus events can propagate to child connection windows
                    NativeMethods.DefWindowProc(_mainWindowHandle, Convert.ToUInt32(m.Msg), (IntPtr)1, m.LParam);
                    m.Result = (IntPtr)1;
                    return true;

                // handle re-focusing connection when the main window moves or resizes
                case NativeMethods.WM_ENTERSIZEMOVE:
                    _inSizeMove = true;
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Begin app window move/resize");
                    break;
                case NativeMethods.WM_EXITSIZEMOVE:
                    _inSizeMove = false;
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "End app window move/resize");
                    // This handles activations from clicks that started a size/move operation
                    ActivateConnection();
                    break;
                case NativeMethods.WM_WINDOWPOSCHANGED:
                    // Ignore this message if the window wasn't activated
                    if (!MainWindowFocused)
                        break;

                    var windowPos = (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.WINDOWPOS));
                    if ((windowPos.flags & NativeMethods.SWP_NOACTIVATE) == 0)
                    {
                        if (!MainWindowFocused && !_inSizeMove)
                        {
                            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "WM_WINDOWPOSCHANGED DONE");
                            ActivateConnection();
                        }
                    }
                    break;
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _keyboardHook?.Dispose();
        }
    }
}
