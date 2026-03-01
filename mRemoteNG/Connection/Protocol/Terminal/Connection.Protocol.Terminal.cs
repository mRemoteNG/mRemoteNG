using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.Terminal
{
    [SupportedOSPlatform("windows")]
    public class ProtocolTerminal(ConnectionInfo connectionInfo) : ProtocolBase
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
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, "Attempting to start Terminal session.", true);

                _consoleControl = new ConsoleControl.ConsoleControl
                {
                    Dock = DockStyle.Fill,
                    BackColor = ColorTranslator.FromHtml("#012456"),
                    ForeColor = Color.White,
                    IsInputEnabled = true,
                    Padding = new Padding(0, 20, 0, 0)
                };

                // Path to command prompt - dynamically determined from system
                // Using COMSPEC environment variable which points to the system's command processor
                string terminalExe = Environment.GetEnvironmentVariable("COMSPEC") ?? @"C:\Windows\System32\cmd.exe";

                // Setup arguments based on whether hostname is provided
                string arguments = "";
                string hostname = _connectionInfo.Hostname.Trim().ToLowerInvariant();
                bool useLocalHost = hostname == "" || hostname.Equals("localhost", StringComparison.Ordinal);
                
                if (!useLocalHost)
                {
                    // If hostname is provided, try to connect via SSH
                    // Note: Domain field is not used for SSH as it's Windows-specific
                    // SSH authentication will use standard SSH mechanisms (password prompt, keys, etc.)
                    string username = _connectionInfo.Username;
                    int port = _connectionInfo.Port;
                    
                    // Build SSH command
                    string sshCommand = "ssh";
                    
                    // Add port if it's not the default SSH port (22)
                    if (port > 0 && port != 22)
                    {
                        sshCommand += $" -p {port}";
                    }
                    
                    if (!string.IsNullOrEmpty(username))
                    {
                        sshCommand += $" {username}@{_connectionInfo.Hostname}";
                    }
                    else
                    {
                        sshCommand += $" {_connectionInfo.Hostname}";
                    }
                    
                    arguments = $"/K {sshCommand}";
                }
                else
                {
                    // For local sessions, just start cmd with /K to keep it open
                    arguments = "/K";
                }

                _consoleControl.StartProcess(terminalExe, arguments);

                // Wait for the console control to create its handle
                int maxWaitMs = 5000; // 5 seconds timeout
                long startTicks = Environment.TickCount64;
                while (!_consoleControl.IsHandleCreated && 
                       Environment.TickCount64 < startTicks + maxWaitMs)
                {
                    System.Threading.Thread.Sleep(50);
                }

                if (!_consoleControl.IsHandleCreated)
                {
                    throw new TimeoutException("Failed to initialize terminal console within 5 seconds. This may indicate system resource constraints or permission issues.");
                }

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
            Port = 22
        }

        #endregion
    }
}
