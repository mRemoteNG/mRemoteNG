using System;
using System.Diagnostics;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Connection.Protocol
{
    public abstract class ExternalProcessProtocolBase : ProtocolBase, IFocusable
    {
        private IntPtr _winEventHook;
        private NativeMethods.WinEventDelegate _setForegroundDelegate;
        private bool _hasFocus;

        public override bool IsExternalProcess { get; } = true;

        protected Process ProtocolProcess { get; set; }

        protected IntPtr ProcessHandle { get; set; }

        public bool HasFocus
        {
            get => _hasFocus;
            private set
            {
                if (_hasFocus == value)
                    return;

                _hasFocus = value;
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                    string.Format("External protocol window set to {0}. name:'{1}', protocol:'{2}', pid:{3}, hwnd:{4}",
                        _hasFocus ? "foreground" : "background",
                        InterfaceControl.Info.Name,
                        InterfaceControl.Info.Protocol,
                        ProtocolProcess.Id,
                        ProcessHandle));
                OnFocusChanged();
            }
        }

        public int ThreadId => (int)NativeMethods.GetWindowThreadProcessId(ProcessHandle, IntPtr.Zero);


        public override bool Connect()
        {
            _setForegroundDelegate = OnWinEventSetForeground;
            _winEventHook = NativeMethods.SetWinEventHook(
                NativeMethods.EVENT_SYSTEM_FOREGROUND,
                NativeMethods.EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero,
                _setForegroundDelegate,
                /*Convert.ToUInt32(ProtocolProcess.Id)*/0,
                0,
                NativeMethods.WINEVENT_OUTOFCONTEXT);

            return base.Connect();
        }

        public override void Close()
        {
            if (NativeMethods.UnhookWinEvent(_winEventHook))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Successfully unhooked WinEvent listener from '{InterfaceControl.Info.Name}'");
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Failed to unhook WinEvent listener from '{InterfaceControl.Info.Name}'");
            }

            base.Close();
        }

        public override void Focus()
        {
            FocusChildProcessWindow();
        }

        private void FocusChildProcessWindow()
        {
            if (NativeMethods.GetForegroundWindow() == ProcessHandle)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Process already focused - do nothing");
                return;
            }

            var setForegroundSuccessful = NativeMethods.SetForegroundWindow(ProcessHandle);

            if (!setForegroundSuccessful)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                    "Failed to set external protocol window to foreground. " +
                    $"name:'{InterfaceControl.Info.Name}', " +
                    $"protocol:'{InterfaceControl.Info.Protocol}', " +
                    $"pid:{ProtocolProcess.Id}, " +
                    $"hwnd:{ProcessHandle}");
            }
        }

        /// <summary>
        /// This callback will be called when the external process window managed by
        /// this protocol is brought to the foreground.
        /// </summary>
        /// <param name="hWinEventHook"></param>
        /// <param name="eventType"></param>
        /// <param name="hwnd"></param>
        /// <param name="idObject"></param>
        /// <param name="idChild"></param>
        /// <param name="dwEventThread"></param>
        /// <param name="dwmsEventTime"></param>
        void OnWinEventSetForeground(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            HasFocus = hwnd == ProcessHandle;
        }

        public event EventHandler FocusChanged;
        protected virtual void OnFocusChanged()
        {
            FocusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
