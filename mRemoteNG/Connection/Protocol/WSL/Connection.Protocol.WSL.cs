using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.WSL
{
    [SupportedOSPlatform("windows")]
    public class ProtocolWSL(ConnectionInfo connectionInfo) : ProtocolBase
    {
        #region Private Fields

        private IntPtr _handle;
        private readonly ConnectionInfo _connectionInfo = connectionInfo;
        private ConsoleControl.ConsoleControl? _consoleControl;

        #endregion

        #region Public Methods

        public override bool Connect()
        {
            try
            {
                // Check if WSL is installed
                if (!IsWslInstalled())
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, 
                        "WSL is not installed on this system. Please install WSL to use this protocol.", true);
                    return false;
                }

                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, 
                    "Attempting to start WSL session.", true);

                _consoleControl = new ConsoleControl.ConsoleControl
                {
                    Dock = DockStyle.Fill,
                    BackColor = ColorTranslator.FromHtml("#300A24"), // Ubuntu terminal color
                    ForeColor = Color.White,
                    IsInputEnabled = true,
                    Padding = new Padding(0, 20, 0, 0)
                };

                // Path to wsl.exe
                string wslExe = @"C:\Windows\System32\wsl.exe";
                
                // Build arguments based on connection info
                string arguments = BuildWslArguments();

                _consoleControl.StartProcess(wslExe, arguments);

                _handle = _consoleControl.Handle;
                NativeMethods.SetParent(_handle, InterfaceControl.Handle);

                Resize(this, EventArgs.Empty);
                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(Language.ConnectionFailed, ex);
                return false;
            }
        }

        private static bool IsWslInstalled()
        {
            try
            {
                // Check if wsl.exe exists
                string wslPath = @"C:\Windows\System32\wsl.exe";
                if (!File.Exists(wslPath))
                {
                    return false;
                }

                // Additional check: Try to execute wsl.exe --status to verify it's properly installed
                // For now, just check if the file exists
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string BuildWslArguments()
        {
            string arguments = "";

            // If a hostname is specified, treat it as a distribution name
            if (!string.IsNullOrEmpty(_connectionInfo.Hostname))
            {
                string hostname = _connectionInfo.Hostname.Trim();
                // Check if it's not localhost (WSL doesn't use localhost as a distribution name)
                if (!hostname.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    arguments = $"-d {hostname}";
                }
            }

            // If username is specified, we can try to use it
            if (!string.IsNullOrEmpty(_connectionInfo.Username))
            {
                arguments += $" -u {_connectionInfo.Username}";
            }

            return arguments.Trim();
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

        protected override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Size == Size.Empty) return;
                // Use ClientRectangle to account for padding (for connection frame color)
                Rectangle clientRect = InterfaceControl.ClientRectangle;
                NativeMethods.MoveWindow(_handle, 
                                         clientRect.X - SystemInformation.FrameBorderSize.Width,
                                         clientRect.Y - (SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height),
                                         clientRect.Width + SystemInformation.FrameBorderSize.Width * 2,
                                         clientRect.Height + SystemInformation.CaptionHeight +
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
            Port = 0 // WSL doesn't use a traditional port
        }

        #endregion
    }
}
