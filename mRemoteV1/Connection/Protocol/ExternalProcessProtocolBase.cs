using System;
using System.Diagnostics;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Connection.Protocol
{
    public class ExternalProcessProtocolBase : ProtocolBase
    {
        private IntPtr _winEventHook;
        private NativeMethods.WinEventDelegate _setForegroundDelegate;

        public override bool IsExternalProcess { get; } = true;

        protected Process ProtocolProcess { get; set; }

        protected IntPtr ProcessHandle { get; set; }

        public int ThreadId => (int)NativeMethods.GetWindowThreadProcessId(ProcessHandle, IntPtr.Zero);


        public override bool Connect()
        {
            _setForegroundDelegate = OnWinEventSetForeground;
            _winEventHook = NativeMethods.SetWinEventHook(
                NativeMethods.EVENT_SYSTEM_FOREGROUND,
                NativeMethods.EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero,
                _setForegroundDelegate,
                Convert.ToUInt32(ProtocolProcess.Id),
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

            var logMsg = setForegroundSuccessful
                ? "External protocol window set to foreground. "
                : "Failed to set external protocol window to foreground. ";

            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                logMsg +
                $"name:'{InterfaceControl.Info.Name}', " +
                $"protocol:'{InterfaceControl.Info.Protocol}', " +
                $"pid:{ProtocolProcess.Id}, " +
                $"hwnd:{ProcessHandle}");
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
            if (hwnd != ProtocolProcess.MainWindowHandle)
                return;

            //Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
            //    "Exernal protocol window set to foreground. " +
            //    $"protocol:{InterfaceControl.Info.Protocol} " +
            //    $"pid:{ProtocolProcess.Id}, " +
            //    $"hwnd:{ProtocolProcess.MainWindowHandle}");
        }
    }
}
