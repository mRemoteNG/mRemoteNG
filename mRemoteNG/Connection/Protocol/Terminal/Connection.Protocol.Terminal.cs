using System;
using System.Drawing;
using System.IO;
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
        private ConsoleControl.ConsoleControl _consoleControl;

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

                string hostname = _connectionInfo.Hostname.Trim().ToLower();
                bool useLocalHost = hostname == "" || hostname.Equals("localhost");

                string processExe;
                string arguments;

                if (!useLocalHost)
                {
                    // Remote session: launch the OpenSSH client (ssh.exe) DIRECTLY.
                    //
                    // The previous implementation ran "cmd.exe /K ssh <host>", concatenating the
                    // attacker-controllable Hostname/Username connection fields into a string that
                    // cmd.exe then re-parsed. That allowed command injection through shell
                    // metacharacters (& | < > ^) — see issue #3335. Invoking ssh.exe directly means
                    // no shell interprets the arguments, and BuildSshArguments rejects any value that
                    // could be mis-parsed as an additional ssh argument.
                    string sshExe = FindSshExe();
                    if (sshExe == null)
                    {
                        Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                            "Windows OpenSSH client (ssh.exe) was not found. " +
                            "Please install the OpenSSH Client optional feature via Settings > Apps > Optional Features.", true);
                        return false;
                    }

                    processExe = sshExe;
                    arguments = BuildSshArguments(_connectionInfo.Hostname, _connectionInfo.Username, _connectionInfo.Port);
                }
                else
                {
                    // Local session: open the system command processor. No user-controlled input is
                    // passed, so there is nothing to inject.
                    processExe = Environment.GetEnvironmentVariable("COMSPEC") ?? @"C:\Windows\System32\cmd.exe";
                    arguments = "/K";
                }

                _consoleControl.StartProcess(processExe, arguments);

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
                    throw new Exception("Failed to initialize terminal console within 5 seconds. This may indicate system resource constraints or permission issues.");
                }

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

        #region Private Methods

        /// <summary>
        /// Builds the argument string passed to ssh.exe. The hostname and username come straight from
        /// the (potentially malicious) connections file, so both are validated: any value containing
        /// whitespace/control characters or starting with '-' is rejected, because ssh.exe would parse
        /// such a value as one or more additional arguments (e.g. -oProxyCommand=...) rather than as the
        /// target. ssh.exe is launched directly with no intervening shell, so shell metacharacters
        /// (&amp; | &lt; &gt; ^) carry no special meaning and cannot execute commands.
        /// </summary>
        private static string BuildSshArguments(string rawHostname, string rawUsername, int port)
        {
            string hostname = (rawHostname ?? string.Empty).Trim();
            string username = (rawUsername ?? string.Empty).Trim();

            if (!IsSafeSshToken(hostname))
                throw new ArgumentException($"Refusing to start SSH session: the hostname '{hostname}' contains characters that are not allowed.");

            if (username.Length > 0 && !IsSafeSshToken(username))
                throw new ArgumentException($"Refusing to start SSH session: the username '{username}' contains characters that are not allowed.");

            string args = "";

            if (port > 0 && port != (int)Defaults.Port)
                args += $"-p {port} ";

            if (username.Length > 0)
                args += $"{username}@{hostname}";
            else
                args += hostname;

            return args.Trim();
        }

        /// <summary>
        /// Returns true only for values that are safe to place on the ssh.exe command line as a single
        /// token: non-empty, no whitespace or control characters, and not starting with '-' (which ssh
        /// would treat as an option switch).
        /// </summary>
        private static bool IsSafeSshToken(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (value[0] == '-')
                return false;

            foreach (char c in value)
            {
                if (char.IsWhiteSpace(c) || char.IsControl(c))
                    return false;
            }

            return true;
        }

        private static string FindSshExe()
        {
            // Try the standard Windows OpenSSH location first
            string systemSsh = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "OpenSSH", "ssh.exe");

            if (File.Exists(systemSsh))
                return systemSsh;

            // Fallback: try to find ssh.exe on PATH
            string pathVar = Environment.GetEnvironmentVariable("PATH");
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

        #endregion

        #region Enumerations

        public enum Defaults
        {
            Port = 22
        }

        #endregion
    }
}
