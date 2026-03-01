using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.SSH
{
    [SupportedOSPlatform("windows")]
    public class ProtocolOpenSSH(ConnectionInfo connectionInfo) : ProtocolBase
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
                string? sshExe = FindSshExe();
                if (sshExe == null)
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                        "Windows OpenSSH client (ssh.exe) was not found. " +
                        "Please install the OpenSSH Client optional feature via Settings > Apps > Optional Features.", true);
                    return false;
                }

                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                    $"Attempting to start OpenSSH session using {sshExe}.", true);

                _consoleControl = new ConsoleControl.ConsoleControl
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    IsInputEnabled = true,
                    Padding = new Padding(0, 20, 0, 0)
                };

                string arguments = BuildSshArguments();
                _consoleControl.StartProcess(sshExe, arguments);

                // Wait for the console control to create its handle
                int maxWaitMs = 5000;
                long startTicks = Environment.TickCount64;
                while (!_consoleControl.IsHandleCreated &&
                       Environment.TickCount64 < startTicks + maxWaitMs)
                {
                    System.Threading.Thread.Sleep(50);
                }

                if (!_consoleControl.IsHandleCreated)
                {
                    throw new TimeoutException("Failed to initialize OpenSSH console within 5 seconds.");
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

        #region Private Methods

        private static string? FindDefaultSshKey()
        {
            string sshDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh");
            if (!Directory.Exists(sshDir))
                return null;

            // Prefer modern key types (most secure first)
            string[] defaultKeyNames = ["id_ed25519", "id_ecdsa", "id_rsa", "id_dsa"];
            foreach (string keyName in defaultKeyNames)
            {
                string candidate = Path.Combine(sshDir, keyName);
                if (File.Exists(candidate))
                    return candidate;
            }
            return null;
        }

        private static string? FindSshExe()
        {
            // Try the standard Windows OpenSSH location first
            string systemSsh = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "OpenSSH", "ssh.exe");

            if (File.Exists(systemSsh))
                return systemSsh;

            // Fallback: try to find ssh.exe on PATH
            string? pathVar = Environment.GetEnvironmentVariable("PATH");
            if (pathVar != null)
            {
                foreach (string dir in pathVar.Split(Path.PathSeparator))
                {
                    string candidate = Path.Combine(dir.Trim(), "ssh.exe");
                    if (File.Exists(candidate))
                        return candidate;
                }
            }

            return null;
        }

        private string BuildSshArguments()
        {
            string hostname = _connectionInfo.Hostname.Trim();
            string username = _connectionInfo.Username;
            int port = _connectionInfo.Port;

            string args = "";

            // Add port if not default
            if (port > 0 && port != 22)
            {
                args += $"-p {port} ";
            }

            // Add SSH options (extra flags like -o StrictHostKeyChecking=no)
            string sshOptions = _connectionInfo.SSHOptions?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(sshOptions))
            {
                args += $"{sshOptions} ";
            }

            // Add private key if specified; otherwise try auto-discovery of default keys
            string keyPath = _connectionInfo.PrivateKeyPath?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(keyPath))
            {
                // Convert PuTTY .ppk to OpenSSH format hint — user should use OpenSSH-format keys
                args += $"-i \"{keyPath}\" ";
            }
            else
            {
                // Auto-discover standard SSH key files from ~/.ssh/ when no explicit key is configured
                string? discoveredKey = FindDefaultSshKey();
                if (discoveredKey != null)
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                        $"No private key configured; auto-discovered SSH key: {discoveredKey}", true);
                    args += $"-i \"{discoveredKey}\" ";
                }
            }

            // Build user@host or just host
            if (!string.IsNullOrEmpty(username))
            {
                args += $"{username}@{hostname}";
            }
            else
            {
                args += hostname;
            }

            return args.Trim();
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
