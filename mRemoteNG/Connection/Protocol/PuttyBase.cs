using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public class PuttyBase : ProtocolBase
    {
        private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
        private bool _isPuttyNg;
        private readonly DisplayProperties _display = new();

        #region Public Properties

        protected Putty_Protocol PuttyProtocol { private get; set; }

        protected Putty_SSHVersion PuttySSHVersion { private get; set; }

        public IntPtr PuttyHandle { get; set; }

        private Process? PuttyProcess { get; set; }

        public static string? PuttyPath { get; set; }

        public bool Focused => NativeMethods.GetForegroundWindow() == PuttyHandle;

        #endregion

        #region Private Events & Handlers

        private void ProcessExited(object sender, EventArgs e)
        {
            Event_Closed(this);
        }

        #endregion

        #region Public Methods

        public bool isRunning()
        {
            return PuttyProcess?.HasExited == false;
        }

        public void CreatePipe(object oData)
        {
            string data = (string)oData;
            string random = data[..8];
            string password = data[8..];
            using NamedPipeServerStream server = CreatePipeServer($"mRemoteNGSecretPipe{random}");
            server.WaitForConnection();
            using StreamWriter writer = new(server);
            writer.Write(password);
            writer.Flush();
        }

        public override bool Connect()
        {
            string optionalTemporaryPrivateKeyPath = ""; // path to ppk file instead of password. only temporary (extracted from credential vault).

            try
            {
                _isPuttyNg = PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.PuttyNg;

                // Validate PuttyPath to prevent command injection
                PathValidator.ValidateExecutablePathOrThrow(PuttyPath, nameof(PuttyPath));

                PuttyProcess = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = PuttyPath
                    }
                };

                CommandLineArguments arguments = new() { EscapeForShell = false };

                arguments.Add("-load", InterfaceControl.Info.PuttySession);

                if (!(InterfaceControl.Info is PuttySessionInfo))
                {
                    arguments.Add("-" + PuttyProtocol);

                    if (PuttyProtocol == Putty_Protocol.ssh)
                    {

                        string username = InterfaceControl.Info?.Username ?? "";
                        //string password = InterfaceControl.Info?.Password?.ConvertToUnsecureString() ?? "";
                        string password = InterfaceControl.Info?.Password ?? "";
                        string UserViaAPI = InterfaceControl.Info?.UserViaAPI ?? "";
                        string privatekey = "";

                        // access secret server api if necessary
                        if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.DelineaSecretServer)
                        {
                            try
                            {
                                ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer($"{UserViaAPI}", out username, out password, out _, out privatekey);

                                if (!string.IsNullOrEmpty(privatekey))
                                {
                                    optionalTemporaryPrivateKeyPath = WriteTemporaryPrivateKeyFile(privatekey);
                                }
                            }
                            catch (Exception ex)
                            {
                                Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                            }
                        }
                        else if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.ClickstudiosPasswordState)
                        {
                            try
                            {
                                ExternalConnectors.CPS.PasswordstateInterface.FetchSecretFromServer($"{UserViaAPI}", out username, out password, out _, out privatekey);

                                if (!string.IsNullOrEmpty(privatekey))
                                {
                                    optionalTemporaryPrivateKeyPath = WriteTemporaryPrivateKeyFile(privatekey);
                                }
                            }
                            catch (Exception ex)
                            {
                                Event_ErrorOccured(this, "Passwordstate Interface Error: " + ex.Message, 0);
                            }
                        }
                        else if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.OnePassword) {
                            try
                            {
                                ExternalConnectors.OP.OnePasswordCli.ReadPassword($"{UserViaAPI}", out username, out password, out _, out privatekey);
                            }
                            catch (ExternalConnectors.OP.OnePasswordCliException ex)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPOnePasswordCommandLine + ": " + ex.Arguments);
                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPOnePasswordReadFailed + Environment.NewLine + ex.Message);
                            }
                        }
                        else if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.VaultOpenbao) {
                            try {
                                if (InterfaceControl.Info?.VaultOpenbaoSecretEngine == VaultOpenbaoSecretEngine.SSHOTP)
                                    ExternalConnectors.VO.VaultOpenbao.ReadOtpSSH($"{InterfaceControl.Info?.VaultOpenbaoMount}", $"{InterfaceControl.Info?.VaultOpenbaoRole}", $"{InterfaceControl.Info?.Username}", $"{InterfaceControl.Info?.Hostname}", out password);
                                else
                                    ExternalConnectors.VO.VaultOpenbao.ReadPasswordSSH((int)InterfaceControl.Info?.VaultOpenbaoSecretEngine, InterfaceControl.Info?.VaultOpenbaoMount ?? "", InterfaceControl.Info?.VaultOpenbaoRole ?? "", InterfaceControl.Info?.Username ?? "root", out password);
                            } catch (ExternalConnectors.VO.VaultOpenbaoException ex) {
                                Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                            }
                        }

                        if (string.IsNullOrEmpty(username))
                        {
                            switch (Properties.OptionsCredentialsPage.Default.EmptyCredentials)
                            {
                                case "windows":
                                    username = Environment.UserName;
                                    break;
                                case "custom" when !string.IsNullOrEmpty(Properties.OptionsCredentialsPage.Default.DefaultUsername):
                                    username = Properties.OptionsCredentialsPage.Default.DefaultUsername;
                                    break;
                                case "custom":

                                    if (Properties.OptionsCredentialsPage.Default.ExternalCredentialProviderDefault == ExternalCredentialProvider.DelineaSecretServer)
                                    {
                                        try
                                        {
                                            ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer(
                                                $"{Properties.OptionsCredentialsPage.Default.UserViaAPIDefault}", out username, out password, out _, out privatekey);
                                        }
                                        catch (Exception ex)
                                        {
                                            Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                                        }
                                    }

                                    break;
                            }
                        }


                        if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(optionalTemporaryPrivateKeyPath))
                        {
                            if (Properties.OptionsCredentialsPage.Default.EmptyCredentials == "custom")
                            {
                                LegacyRijndaelCryptographyProvider cryptographyProvider = new();
                                password = cryptographyProvider.Decrypt(Properties.OptionsCredentialsPage.Default.DefaultPassword, Runtime.EncryptionKey);
                            }
                        }

                        arguments.Add("-" + (int)PuttySSHVersion);

                        if (!Force.HasFlag(ConnectionInfo.Force.NoCredentials))
                        {
                            if (!string.IsNullOrEmpty(username))
                            {
                                arguments.Add("-l", username);
                            }

                            if (!string.IsNullOrEmpty(password))
                            {
                                string random = string.Join("", Guid.NewGuid().ToString("n").Take(8));
                                // write data to pipe
                                Thread thread = new(new ParameterizedThreadStart(CreatePipe));
                                thread.Start($"{random}{password}");
                                // start putty with piped password
                                arguments.Add("-pwfile", $"\\\\.\\PIPE\\mRemoteNGSecretPipe{random}");
                                //arguments.Add("-pw", password);
                            }
                        }

                        if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.VaultOpenbao && InterfaceControl.Info?.VaultOpenbaoSecretEngine == VaultOpenbaoSecretEngine.SSHOTP) {
                            if (!_isPuttyNg) {
                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Cannot connect to VaultOpenbao ssh otp without using puttyng to inject authenticator plugin");
                                return false;
                            }
                            arguments.Add("-auth-plugin");
                            string random = string.Join("", Guid.NewGuid().ToString("n").Take(8));
                            string pipename = $"mRemoteNGSecretPipe{random}";
                            arguments.Add($"{App.Info.GeneralAppInfo.HomePath}\\vault-ssh-helper-plugin.exe {username} --pipeName={pipename}");
                            System.Threading.Tasks.Task.Run(async () => {
                                using NamedPipeServerStream server = CreatePipeServer(pipename);
                                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;
                                await server.WaitForConnectionAsync(cts);
                                using var reader = new StreamReader(server, Utf8NoBom, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
                                using var writer = new StreamWriter(server, Utf8NoBom, bufferSize: 1024, leaveOpen: true) { AutoFlush = true };
                                string? pingMessage = await reader.ReadLineAsync(cts);
                                if (pingMessage != "ping") throw new FormatException("Invalid ping from VaultOpenbao SSH OTP plugin");
                                await writer.WriteLineAsync("pong");
                                string dataRequest = await reader.ReadLineAsync(cts) ?? throw new FormatException("Invalid data request from VaultOpenbao SSH OTP plugin");
                                var data = DeserializeData(dataRequest);
                                if (data.Username != username || data.Hostname != InterfaceControl.Info.Hostname || data.Port != InterfaceControl.Info.Port)
                                    throw new FormatException("Mismatched data request from VaultOpenbao SSH OTP plugin");
                                await writer.WriteLineAsync(password);
                            }).ConfigureAwait(false);
                        }

                        // use private key if specified
                        if (!string.IsNullOrEmpty(optionalTemporaryPrivateKeyPath))
                        {
                            arguments.Add("-i", optionalTemporaryPrivateKeyPath);
                        }

                    }

                    arguments.Add("-P", InterfaceControl.Info?.Port.ToString());
                    arguments.Add(InterfaceControl.Info.Hostname);
                }

                if (_isPuttyNg)
                {
                    arguments.Add("-hwndparent", InterfaceControl.Handle.ToString());
                }

                PuttyProcess.StartInfo.Arguments = arguments.ToString();
                // add additional SSH options, f.e. tunnel or noshell parameters that may be specified for the the connnection
                if (!string.IsNullOrEmpty(InterfaceControl.Info.SSHOptions))
                {
                    PuttyProcess.StartInfo.Arguments += " " + InterfaceControl.Info.SSHOptions;
                }

                PuttyProcess.EnableRaisingEvents = true;
                PuttyProcess.Exited += ProcessExited;

                // Start the process minimized for non-PuTTYNG so the window
                // does not flash at its default position on screen before
                // being reparented into the mRemoteNG panel.
                if (!_isPuttyNg)
                {
                    PuttyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                }

                PuttyProcess.Start();
                ChildProcessTracker.AddProcess(PuttyProcess);
                PuttyProcess.WaitForInputIdle(Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000);

                int startTicks = Environment.TickCount;
                while (PuttyHandle.ToInt32() == 0 &
                       Environment.TickCount < startTicks + Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000)
                {
                    if (PuttyProcess.HasExited)
                        break;

                    if (_isPuttyNg)
                    {
                        PuttyHandle = NativeMethods.FindWindowEx(InterfaceControl.Handle, new IntPtr(0), null, null);
                    }
                    else
                    {
                        PuttyProcess.Refresh();
                        IntPtr candidateHandle = PuttyProcess.MainWindowHandle;

                        if (candidateHandle != IntPtr.Zero)
                        {
                            // Check the window class name to distinguish the actual PuTTY
                            // terminal window ("PuTTY") from popup dialogs like the host key
                            // verification alert (class "#32770"). Dialogs must remain as
                            // top-level windows so the user can interact with them.
                            StringBuilder className = new(256);
                            NativeMethods.GetClassName(candidateHandle, className, className.Capacity);
                            string cls = className.ToString();

                            if (cls.Equals("PuTTY", StringComparison.OrdinalIgnoreCase))
                            {
                                PuttyHandle = candidateHandle;
                                // Hide the window immediately so it doesn't flash
                                // at its default position before being reparented.
                                NativeMethods.ShowWindow(PuttyHandle, (int)NativeMethods.SW_HIDE);
                            }
                        }
                    }

                    if (PuttyHandle.ToInt32() == 0)
                    {
                        Thread.Sleep(100);
                    }
                }

                if (!_isPuttyNg)
                {
                    NativeMethods.SetParent(PuttyHandle, InterfaceControl.Handle);

                    // Strip the title bar and thick frame border so the
                    // embedded PuTTY window fills the panel cleanly.
                    int style = NativeMethods.GetWindowLong(PuttyHandle, NativeMethods.GWL_STYLE);
                    style &= ~(NativeMethods.WS_CAPTION | NativeMethods.WS_THICKFRAME);
                    int previousStyle = NativeMethods.SetWindowLong(PuttyHandle, NativeMethods.GWL_STYLE, style);
                    
                    // Check if SetWindowLong failed (returns 0 on error, but 0 could also be the previous value)
                    // If it returns 0 and the previous GetWindowLong succeeded, log a warning
                    if (previousStyle == 0)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, 
                            Language.PuttyStuff + ": SetWindowLong returned 0, window style change may have failed", true);
                    }

                    // Force Windows to recalculate the non-client area so the
                    // removed caption and border actually disappear.
                    NativeMethods.SetWindowPos(PuttyHandle, IntPtr.Zero,
                        0, 0, 0, 0,
                        NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE |
                        NativeMethods.SWP_NOZORDER | NativeMethods.SWP_FRAMECHANGED);
                }

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.PuttyStuff, true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.PuttyHandle, PuttyHandle), true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.PuttyTitle, PuttyProcess.MainWindowTitle), true);
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.PanelHandle, InterfaceControl.Parent.Handle), true);

                if (!string.IsNullOrEmpty(InterfaceControl.Info?.OpeningCommand))
                {
                    NativeMethods.SetForegroundWindow(PuttyHandle);
                    string finalCommand = InterfaceControl.Info.OpeningCommand.TrimEnd() + "\n";
                    SendKeys.SendWait(finalCommand);
                }

                if (!_isPuttyNg)
                {
                    NativeMethods.ShowWindow(PuttyHandle, (int)NativeMethods.SW_RESTORE);
                }

                Resize(this, new EventArgs());
                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ConnectionFailed + Environment.NewLine + ex.Message);
                return false;
            }
            finally
            {
                // make sure to remove the private key file we created
                if (!string.IsNullOrEmpty(optionalTemporaryPrivateKeyPath))
                {
                    System.Threading.Thread.Sleep(500);
                    System.IO.File.Delete(optionalTemporaryPrivateKeyPath);
                }
            }
        }

        /// <summary>
        /// Atomically writes private-key material to a uniquely named temporary
        /// file and returns its path. Uses <see cref="FileMode.CreateNew"/> so an
        /// existing file is never overwritten, retrying on the (extremely
        /// unlikely) name collision. The caller owns the returned file and is
        /// responsible for deleting it.
        /// </summary>
        private static string WriteTemporaryPrivateKeyFile(string privateKey)
        {
            for (int attempt = 0; attempt < 5; attempt++)
            {
                string candidatePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".ppk");
                try
                {
                    using (FileStream stream = new(candidatePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new(stream))
                    {
                        writer.Write(privateKey);
                    }

                    File.SetAttributes(candidatePath, FileAttributes.Temporary);
                    return candidatePath;
                }
                catch (IOException) when (File.Exists(candidatePath))
                {
                    // Name collided with a pre-existing file - try a different name.
                }
            }

            throw new IOException("Unable to create a unique temporary private-key file.");
        }

        public override void Focus()
        {
            try
            {
                NativeMethods.SetForegroundWindow(PuttyHandle);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.PuttyFocusFailed + Environment.NewLine + ex.Message, true);
            }
        }

        protected override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (InterfaceControl.Size == Size.Empty)
                    return;

                if (_isPuttyNg)
                {
                    // PuTTYNG 0.70.0.1 and later doesn't have any window borders
                    // Use ClientRectangle to account for padding (for connection frame color)
                    Rectangle clientRect = InterfaceControl.ClientRectangle;
                    NativeMethods.MoveWindow(PuttyHandle, clientRect.X, clientRect.Y, clientRect.Width, clientRect.Height, true);
                }
                else
                {
                    // Window chrome (caption + thick frame) has been stripped
                    // after reparenting, so just fill the client rectangle.
                    Rectangle clientRect = InterfaceControl.ClientRectangle;

                    NativeMethods.MoveWindow(PuttyHandle, clientRect.X-8, clientRect.Y-30, clientRect.Width+32, clientRect.Height+38, true);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.PuttyResizeFailed + Environment.NewLine + ex.Message, true);
            }
        }

        public override void Close()
        {
            try
            {
                if (PuttyProcess?.HasExited == false)
                {
                    PuttyProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.PuttyKillFailed + Environment.NewLine + ex.Message, true);
            }

            try
            {
                PuttyProcess?.Dispose();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.PuttyDisposeFailed + Environment.NewLine + ex.Message, true);
            }

            base.Close();
        }

        public void ShowSettingsDialog()
        {
            try
            {
                NativeMethods.PostMessage(PuttyHandle, NativeMethods.WM_SYSCOMMAND, (IntPtr)IDM_RECONF, (IntPtr)0);
                NativeMethods.SetForegroundWindow(PuttyHandle);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.PuttyShowSettingsDialogFailed + Environment.NewLine + ex.Message, true);
            }
        }

        #endregion

        #region Enums

        protected enum Putty_Protocol
        {
            ssh = 0,
            telnet = 1,
            rlogin = 2,
            raw = 3,
            serial = 4
        }

        protected enum Putty_SSHVersion
        {
            ssh1 = 1,
            ssh2 = 2
        }

        #endregion

        #region VaultOpenbaoUtils
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        private static NamedPipeServerStream CreatePipeServer(string pipeName) {
            var pipeSecurity = new PipeSecurity();
            using var identity = WindowsIdentity.GetCurrent();
            var sid = identity.Owner ?? identity.User ?? throw new InvalidOperationException("Unable to determine current user SID.");
            pipeSecurity.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
            pipeSecurity.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.FullControl, AccessControlType.Allow));

            return NamedPipeServerStreamAcl.Create(
                pipeName: pipeName,
                direction: PipeDirection.InOut,
                maxNumberOfServerInstances: 1,
                transmissionMode: PipeTransmissionMode.Byte,
                options: PipeOptions.Asynchronous,
                inBufferSize: 0,
                outBufferSize: 0,
                pipeSecurity);
        }
        private static (string Username, string Hostname, uint Port) DeserializeData(string data) {
            var strings = data.Split(':');
            if (strings.Length != 3) {
                throw new FormatException("Invalid data format");
            }
            return (
                Encoding.UTF8.GetString(Convert.FromBase64String(strings[0])),
                Encoding.UTF8.GetString(Convert.FromBase64String(strings[1])),
                uint.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(strings[2])))
            );
        }
        #endregion
    }
}