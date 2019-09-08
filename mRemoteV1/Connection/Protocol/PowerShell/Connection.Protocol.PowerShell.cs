using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.PowerShell
{
    public class ProtocolPowerShell : ProtocolBase
    {
        #region Private Fields

        private IntPtr _handle;
        private Process _process;
        private readonly ConnectionInfo _connectionInfo;

        public ProtocolPowerShell(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        #endregion

        #region Public Methods

        public override bool Connect()
        {
            try
            {
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     $"Attempting to start remote PowerShell session.", true);
                
                _process = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = true,
                        FileName = @"C:\Windows\system32\WindowsPowerShell\v1.0\PowerShell.exe",
                        Arguments = $@"-NoExit -Command ""$password = ConvertTo-SecureString '{_connectionInfo.Password}' -AsPlainText -Force; $cred = New-Object System.Management.Automation.PSCredential -ArgumentList @('{_connectionInfo.Domain}\{_connectionInfo.Username}', $password); Enter-PSSession -ComputerName {_connectionInfo.Hostname} -Credential $cred"""
                    },
                    EnableRaisingEvents = true,
                };
                
                _process.Exited += ProcessExited;
                _process.Start();
                _process.WaitForInputIdle();

                NativeMethods.SetParent(_process.Handle, InterfaceControl.Handle);


                
                //Resize(this, new EventArgs());
                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(Language.ConnectionFailed, ex);
                return false;
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public override void Focus()
        {
            try
            {
                NativeMethods.SetForegroundWindow(_handle);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppFocusFailed, ex);
            }
        }

        public override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Size == Size.Empty) return;
                NativeMethods.MoveWindow(_handle, -SystemInformation.FrameBorderSize.Width,
                                         -(SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height),
                                         InterfaceControl.Width + SystemInformation.FrameBorderSize.Width * 2,
                                         InterfaceControl.Height + SystemInformation.CaptionHeight +
                                         SystemInformation.FrameBorderSize.Height * 2, true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppResizeFailed, ex);
            }
        }

        public override void Close()
        {
            /* only attempt this if we have a valid process object
             * Non-integrated tools will still call base.Close() and don't have a valid process object.
             * See Connect() above... This just muddies up the log.
             */
            if (_process != null)
            {
                try
                {
                    if (!_process.HasExited)
                    {
                        _process.Kill();
                    }
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppKillFailed, ex);
                }

                try
                {
                    if (!_process.HasExited)
                    {
                        _process.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.strIntAppDisposeFailed, ex);
                }
            }

            base.Close();
        }

        #endregion

        #region Private Methods

        private void ProcessExited(object sender, EventArgs e)
        {
            Event_Closed(this);
        }

        #endregion

        #region Enumerations

        public enum Defaults
        {
            Port = 5985
        }

        #endregion
    }
}