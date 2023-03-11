using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.PowerShell
{
    [SupportedOSPlatform("windows")]
    public class ProtocolPowerShell : ProtocolBase
    {
        #region Private Fields

        private IntPtr _handle;
        private readonly ConnectionInfo _connectionInfo;
        private ConsoleControl.ConsoleControl _consoleControl;

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
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, "Attempting to start remote PowerShell session.", true);

                _consoleControl = new ConsoleControl.ConsoleControl
                {
                    Dock = DockStyle.Fill,
                    BackColor = ColorTranslator.FromHtml("#012456"),
                    ForeColor = Color.White,
                    IsInputEnabled = true,
                    Padding = new Padding(0, 20, 0, 0)
                };

                _consoleControl.StartProcess(@"C:\Windows\system32\WindowsPowerShell\v1.0\PowerShell.exe",
                    $@"-NoExit -Command ""$password = ConvertTo-SecureString '{_connectionInfo.Password}' -AsPlainText -Force; $cred = New-Object System.Management.Automation.PSCredential -ArgumentList @('{_connectionInfo.Domain}\{_connectionInfo.Username}', $password); Enter-PSSession -ComputerName {_connectionInfo.Hostname} -Credential $cred""");
                
                while (!_consoleControl.IsHandleCreated) break;
                _handle = _consoleControl.Handle;
                NativeMethods.SetParent(_handle, InterfaceControl.Handle);
                
                Resize(this, new EventArgs());
                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(Language.ConnectionFailed, ex);
                return false;
            }
        }

        public override void Focus()
        {
            try
            {
                NativeMethods.SetForegroundWindow(_handle);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.IntAppFocusFailed, ex);
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
                Runtime.MessageCollector.AddExceptionMessage(Language.IntAppResizeFailed, ex);
            }
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