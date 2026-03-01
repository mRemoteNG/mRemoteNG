using mRemoteNG.App;
using Microsoft.Win32;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Tabs;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public class PuttyBase : ProtocolBase
    {
        private const int IDM_RECONF = 0x50; // PuTTY Settings Menu ID
        private const int TerminalTitlePollIntervalMs = 500;
        private const int WindowTextBufferLength = 512;
        private const int OpeningCommandPollIntervalMs = 250;
        private bool _isPuttyNg;
        private readonly DisplayProperties _display = new();
        private readonly Lock _terminalTitleSync = new();
        private Timer? _terminalTitleTimer;
        private string _fallbackTabText = string.Empty;
        private string _initialTerminalTitle = string.Empty;
        private string _lastTerminalTitle = string.Empty;
        private bool _terminalTitleTrackingEnabled;
        private bool _postOpenLayoutResizePending;
        private bool _postOpenLayoutResizeHooked;
        private System.Windows.Forms.Timer? _windowSearchTimer;
        private int _windowSearchStartTime;
        private System.Windows.Forms.Timer? _openingCommandTimer;
        private IntPtr _openingCommandPendingHandle;
        private string _openingCommandPendingCommand = string.Empty;
        private string _openingCommandInitialTitle = string.Empty;
        private int _openingCommandElapsedMs;
        private long _processStartTicks;
        private bool _pendingResumeReconnect;
        private long _resumeEventTickCount;

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
            StopTerminalTitleTracking();

            // Check whether this exit should trigger an auto-reconnect due to sleep/resume.
            // Only applies to SSH sessions that exited with a network error (non-zero code)
            // within 30 seconds of the last system resume event.
            if (_pendingResumeReconnect)
            {
                _pendingResumeReconnect = false;
                int resumeExitCode = PuttyProcess?.ExitCode ?? 0;
                long elapsedSinceResume = Environment.TickCount64 - _resumeEventTickCount;

                if (resumeExitCode != 0 && elapsedSinceResume < 30_000)
                {
                    // Network error caused by sleep — reconnect after a delay instead of closing.
                    ScheduleAutoReconnect();
                    return;
                }
            }

            // If PuTTY exited with an error within 30 seconds, it likely indicates
            // an authentication failure. Prompt the user to update the stored password.
            try
            {
                bool hasStoredPassword = !string.IsNullOrEmpty(InterfaceControl?.Info?.Password);
                int exitCode = PuttyProcess?.ExitCode ?? 0;
                long elapsedMs = Environment.TickCount64 - _processStartTicks;

                if (hasStoredPassword && exitCode != 0 && elapsedMs < 30_000)
                {
                    PromptToUpdatePassword();
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error checking PuTTY exit for password prompt", ex);
            }

            Event_Closed(this);
        }

        private void ScheduleAutoReconnect()
        {
            const int ReconnectDelayMs = 5000;

            System.Threading.Tasks.Task.Delay(ReconnectDelayMs).ContinueWith(task =>
            {
                try
                {
                    if (InterfaceControl == null || InterfaceControl.IsDisposed || !InterfaceControl.IsHandleCreated)
                        return;

                    InterfaceControl.BeginInvoke((MethodInvoker)ExecuteAutoReconnect);
                }
                catch (ObjectDisposedException)
                {
                    // Intentionally empty — control may be disposed
                }
                catch (InvalidOperationException)
                {
                    // Intentionally empty — control may be disposed
                }
            }, System.Threading.Tasks.TaskScheduler.Default);
        }

        private void ExecuteAutoReconnect()
        {
            try
            {
                if (InterfaceControl == null || InterfaceControl.IsDisposed)
                    return;

                // Dispose the old (already exited) process before starting a new one.
                try { PuttyProcess?.Dispose(); } catch { }
                PuttyProcess = null;

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    "Auto-reconnecting SSH session after sleep/resume...", true);

                if (!Connect())
                    Event_Closed(this);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(
                    "SSH auto-reconnect after resume failed", ex);
                Event_Closed(this);
            }
        }

        #endregion

        #region Public Methods

        public virtual bool isRunning()
        {
            return PuttyProcess?.HasExited == false;
        }

        public static void CreatePipe(object oData)
        {
            string data = (string)oData;
            string random = data[..8];
            string password = data[8..];
            NamedPipeServerStream server = new($"mRemoteNGSecretPipe{random}");
            server.WaitForConnection();
            StreamWriter writer = new(server);
            writer.Write(password);
            writer.Flush();
            server.Dispose();
        }

        protected virtual bool UseTerminalTitlePollingTimer => true;

        protected virtual int PowerModeChangedResizeDelay => 2000;

        protected virtual string ReadTerminalWindowTitle()
        {
            try
            {
                if (PuttyHandle == IntPtr.Zero)
                    return string.Empty;

                StringBuilder textBuffer = new(WindowTextBufferLength);
                NativeMethods.SendMessage(PuttyHandle, NativeMethods.WM_GETTEXT, (IntPtr)textBuffer.Capacity, textBuffer);
                return textBuffer.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        protected void StartTerminalTitleTracking()
        {
            StopTerminalTitleTracking();

            if (InterfaceControl.Parent is not ConnectionTab connectionTab)
                return;

            lock (_terminalTitleSync)
            {
                _fallbackTabText = connectionTab.TabText;
                _initialTerminalTitle = ReadTerminalWindowTitle();
                _lastTerminalTitle = _initialTerminalTitle;
                _terminalTitleTrackingEnabled = true;

                if (UseTerminalTitlePollingTimer)
                {
                    _terminalTitleTimer = new Timer(_ => UpdateTabTitleFromTerminalTitle(),
                                                    null,
                                                    TerminalTitlePollIntervalMs,
                                                    TerminalTitlePollIntervalMs);
                }
            }
        }

        protected void StopTerminalTitleTracking()
        {
            Timer? timerToDispose;
            string fallbackTabText;
            bool restoreFallback;

            lock (_terminalTitleSync)
            {
                restoreFallback = _terminalTitleTrackingEnabled;
                _terminalTitleTrackingEnabled = false;
                timerToDispose = _terminalTitleTimer;
                _terminalTitleTimer = null;
                fallbackTabText = _fallbackTabText;
            }

            timerToDispose?.Dispose();

            if (restoreFallback)
                ApplyTabText(fallbackTabText);
        }

        private void StopWindowSearch()
        {
            if (_windowSearchTimer == null) return;
            _windowSearchTimer.Stop();
            _windowSearchTimer.Dispose();
            _windowSearchTimer = null;
        }

        private void StopOpeningCommandTimer()
        {
            if (_openingCommandTimer == null) return;
            _openingCommandTimer.Stop();
            _openingCommandTimer.Dispose();
            _openingCommandTimer = null;
        }

        private void OpeningCommandTimer_Tick(object? sender, EventArgs e)
        {
            _openingCommandElapsedMs += OpeningCommandPollIntervalMs;
            string currentTitle = ReadTerminalWindowTitle();
            bool titleChanged = !string.IsNullOrEmpty(currentTitle)
                                && !string.Equals(currentTitle, _openingCommandInitialTitle, StringComparison.Ordinal);
            bool timedOut = _openingCommandElapsedMs >= Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000;

            if (!titleChanged && !timedOut)
                return;

            StopOpeningCommandTimer();

            if (_openingCommandPendingHandle != IntPtr.Zero && !string.IsNullOrEmpty(_openingCommandPendingCommand))
            {
                NativeMethods.SetForegroundWindow(_openingCommandPendingHandle);
                SendKeys.SendWait(_openingCommandPendingCommand);
                _openingCommandPendingCommand = string.Empty;
                _openingCommandPendingHandle = IntPtr.Zero;
            }
        }

        private void WindowSearchTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (PuttyProcess == null || PuttyProcess.HasExited)
                {
                    StopWindowSearch();
                    Event_Closed(this);
                    return;
                }

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
                        _ = NativeMethods.GetClassName(candidateHandle, className, className.Capacity);
                        string cls = className.ToString();

                        if (cls.Equals("PuTTY", StringComparison.OrdinalIgnoreCase))
                        {
                            PuttyHandle = candidateHandle;
                        }
                    }
                }

                if (PuttyHandle != IntPtr.Zero)
                {
                    StopWindowSearch();
                    CompleteConnectionSetup();
                }
                else if (Environment.TickCount - _windowSearchStartTime > Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000)
                {
                    StopWindowSearch();
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "PuTTY window discovery timed out.");
                    CompleteConnectionSetup();
                }
            }
            catch (Exception ex)
            {
                StopWindowSearch();
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Error during PuTTY window discovery: " + ex.Message);
            }
        }

        private void CompleteConnectionSetup()
        {
            if (!_isPuttyNg && PuttyHandle != IntPtr.Zero)
            {
                NativeMethods.SetParent(PuttyHandle, InterfaceControl.Handle);
            }

            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.PuttyStuff, true);
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(CultureInfo.InvariantCulture, Language.PuttyHandle, PuttyHandle), true);
            if (PuttyProcess != null)
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(CultureInfo.InvariantCulture, Language.PuttyTitle, PuttyProcess.MainWindowTitle), true);
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(CultureInfo.InvariantCulture, Language.PanelHandle, InterfaceControl.Parent?.Handle), true);

            if (!string.IsNullOrEmpty(InterfaceControl.Info?.OpeningCommand) && PuttyHandle != IntPtr.Zero)
            {
                var parser = new ExternalToolArgumentParser(InterfaceControl.Info);
                string parsedCommand = parser.ParseArguments(InterfaceControl.Info.OpeningCommand.TrimEnd(), escapeForShell: false);
                string finalCommand = EscapeSendKeys(parsedCommand) + "\n";
                string initialTitle = ReadTerminalWindowTitle();
                if (string.IsNullOrEmpty(initialTitle))
                {
                    // No title detectable yet — send immediately.
                    NativeMethods.SetForegroundWindow(PuttyHandle);
                    SendKeys.SendWait(finalCommand);
                }
                else
                {
                    // Defer sending until the terminal title changes, which signals that
                    // SSH authentication has completed and the shell is ready.  This prevents
                    // the command from being injected into keyboard-interactive auth prompts.
                    _openingCommandPendingCommand = finalCommand;
                    _openingCommandPendingHandle = PuttyHandle;
                    _openingCommandInitialTitle = initialTitle;
                    _openingCommandElapsedMs = 0;
                    _openingCommandTimer = new System.Windows.Forms.Timer { Interval = OpeningCommandPollIntervalMs };
                    _openingCommandTimer.Tick += OpeningCommandTimer_Tick;
                    _openingCommandTimer.Start();
                }
            }

            Resize(this, EventArgs.Empty);
            SchedulePostOpenLayoutResizePass();

            StartTerminalTitleTracking();
            base.Connect();
        }

        protected virtual void UpdateTabTitleFromTerminalTitle()
        {
            string terminalTitle = ReadTerminalWindowTitle();

            lock (_terminalTitleSync)
            {
                if (!_terminalTitleTrackingEnabled)
                    return;

                if (string.Equals(_lastTerminalTitle, terminalTitle, StringComparison.Ordinal))
                    return;

                _lastTerminalTitle = terminalTitle;
            }

            string tabText = ResolveTabText(terminalTitle);
            ApplyTabText(tabText);
        }

        private string ResolveTabText(string terminalTitle)
        {
            lock (_terminalTitleSync)
            {
                if (string.IsNullOrWhiteSpace(terminalTitle) ||
                    string.Equals(terminalTitle, _initialTerminalTitle, StringComparison.Ordinal))
                {
                    return _fallbackTabText;
                }
            }

            string tabText = terminalTitle.Replace("&", "&&", StringComparison.Ordinal);

            if (Properties.OptionsTabsPanelsPage.Default.ShowLogonInfoOnTabs)
            {
                ConnectionInfo? info = InterfaceControl?.Info;
                if (info != null)
                {
                    string domain = info.Domain;
                    string username = info.Username;
                    if (domain != "" || username != "")
                    {
                        string logonSuffix = " (";
                        if (domain != "")
                            logonSuffix += domain;
                        if (username != "")
                        {
                            if (domain != "")
                                logonSuffix += @"\";
                            logonSuffix += username;
                        }
                        logonSuffix += ")";
                        tabText += logonSuffix;
                    }
                }
            }

            return tabText;
        }

        private void ApplyTabText(string tabText)
        {
            if (InterfaceControl.Parent is not ConnectionTab connectionTab)
                return;

            if (connectionTab.IsDisposed || connectionTab.Disposing)
                return;

            void Update()
            {
                if (connectionTab.IsDisposed || connectionTab.Disposing)
                    return;

                connectionTab.TabText = tabText;

                if (!connectionTab.IsActivated)
                {
                    connectionTab.HasUnreadActivity = true;
                }
            }

            if (connectionTab.InvokeRequired)
            {
                try
                {
                    connectionTab.BeginInvoke((Action)Update);
                }
                catch (ObjectDisposedException)
                {
                    // The tab was disposed while marshaling the title update.
                }
                catch (InvalidOperationException)
                {
                    // The tab handle is no longer available.
                }
            }
            else
            {
                Update();
            }
        }

        private void ResetPostOpenLayoutResizeState()
        {
            _postOpenLayoutResizePending = false;
            UnhookPostOpenLayoutResize();
        }

        private void HookPostOpenLayoutResize()
        {
            if (_postOpenLayoutResizeHooked)
                return;

            if (InterfaceControl.IsDisposed)
                return;

            InterfaceControl.HandleCreated += InterfaceControl_HandleCreated;
            _postOpenLayoutResizeHooked = true;
        }

        private void UnhookPostOpenLayoutResize()
        {
            if (!_postOpenLayoutResizeHooked)
                return;

            try
            {
                if (!InterfaceControl.IsDisposed)
                    InterfaceControl.HandleCreated -= InterfaceControl_HandleCreated;
            }
            catch (ObjectDisposedException)
            {
                // Interface control already disposed.
            }
            catch (InvalidOperationException)
            {
                // Interface handle is no longer available.
            }
            finally
            {
                _postOpenLayoutResizeHooked = false;
            }
        }

        private void InterfaceControl_HandleCreated(object? sender, EventArgs e)
        {
            RequestPostOpenLayoutResizePass();
        }

        protected virtual void QueuePostOpenLayoutResizePass(MethodInvoker resizeAction)
        {
            InterfaceControl.BeginInvoke(resizeAction);
        }

        protected void SchedulePostOpenLayoutResizePass()
        {
            _postOpenLayoutResizePending = true;
            HookPostOpenLayoutResize();
            RequestPostOpenLayoutResizePass();
        }

        internal void RequestPostOpenLayoutResizePass()
        {
            if (!_postOpenLayoutResizePending)
                return;

            if (InterfaceControl.IsDisposed)
            {
                ResetPostOpenLayoutResizeState();
                return;
            }

            if (!InterfaceControl.IsHandleCreated)
                return;

            try
            {
                QueuePostOpenLayoutResizePass((MethodInvoker)(() =>
                {
                    if (!_postOpenLayoutResizePending || InterfaceControl.IsDisposed)
                        return;

                    _postOpenLayoutResizePending = false;
                    UnhookPostOpenLayoutResize();
                    Resize(this, EventArgs.Empty);
                }));
            }
            catch (ObjectDisposedException)
            {
                ResetPostOpenLayoutResizeState();
            }
            catch (InvalidOperationException)
            {
                // Handle may have been recreated between checks; keep pending and retry later.
            }
        }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            if (InterfaceControl != null)
            {
                InterfaceControl.Resize += Resize;
            }

            return true;
        }

        public override bool Connect()
        {
            string optionalTemporaryPrivateKeyPath = ""; // path to ppk file instead of password. only temporary (extracted from credential vault).

            try
            {
                StopTerminalTitleTracking();
                ResetPostOpenLayoutResizeState();
                _isPuttyNg = PuttyTypeDetector.GetPuttyType() == PuttyTypeDetector.PuttyType.PuttyNg;

                // Validate PuttyPath to prevent command injection
                PathValidator.ValidateExecutablePathOrThrow(PuttyPath ?? string.Empty, nameof(PuttyPath));

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
                        string domain = InterfaceControl.Info?.Domain ?? "";
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
                                    optionalTemporaryPrivateKeyPath = Path.GetTempFileName();
                                    File.WriteAllText(optionalTemporaryPrivateKeyPath, privatekey);
                                    FileInfo fileInfo = new(optionalTemporaryPrivateKeyPath)
                                    {
                                        Attributes = FileAttributes.Temporary
                                    };
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
                                    optionalTemporaryPrivateKeyPath = Path.GetTempFileName();
                                    File.WriteAllText(optionalTemporaryPrivateKeyPath, privatekey);
                                    FileInfo fileInfo = new(optionalTemporaryPrivateKeyPath)
                                    {
                                        Attributes = FileAttributes.Temporary
                                    };
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
                        else if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.PasswordSafe) {
                            try
                            {
                                ExternalConnectors.PasswordSafe.PasswordSafeCli.ReadPassword($"{UserViaAPI}", out username, out password, out _, out privatekey);
                            }
                            catch (ExternalConnectors.PasswordSafe.PasswordSafeCliException ex)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPPasswordSafeCommandLine + ": " + ex.Arguments);
                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPPasswordSafeReadFailed + Environment.NewLine + ex.Message);
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
                        else if (InterfaceControl.Info?.ExternalCredentialProvider == ExternalCredentialProvider.LAPS)
                        {
                            try
                            {
                                ExternalConnectors.LAPS.LAPSHelper.QueryLAPSPassword(InterfaceControl.Info?.Hostname ?? "", out username, out password, out _);
                            }
                            catch (ExternalConnectors.LAPS.LAPSException ex)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPLAPSQueryFailed + Environment.NewLine + ex.Message);
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
                                    switch (Properties.OptionsCredentialsPage.Default.ExternalCredentialProviderDefault)
                                    {
                                        case ExternalCredentialProvider.DelineaSecretServer:
                                            try
                                            {
                                                ExternalConnectors.DSS.SecretServerInterface.FetchSecretFromServer(
                                                    $"{Properties.OptionsCredentialsPage.Default.UserViaAPIDefault}", out username, out password, out _, out privatekey);
                                            }
                                            catch (Exception ex)
                                            {
                                                Event_ErrorOccured(this, "Secret Server Interface Error: " + ex.Message, 0);
                                            }

                                            break;
                                        case ExternalCredentialProvider.ClickstudiosPasswordState:
                                            try
                                            {
                                                ExternalConnectors.CPS.PasswordstateInterface.FetchSecretFromServer(
                                                    $"{Properties.OptionsCredentialsPage.Default.UserViaAPIDefault}", out username, out password, out _, out privatekey);
                                            }
                                            catch (Exception ex)
                                            {
                                                Event_ErrorOccured(this, "Passwordstate Interface Error: " + ex.Message, 0);
                                            }

                                            break;
                                        case ExternalCredentialProvider.OnePassword:
                                            try
                                            {
                                                ExternalConnectors.OP.OnePasswordCli.ReadPassword(
                                                    $"{Properties.OptionsCredentialsPage.Default.UserViaAPIDefault}", out username, out password, out _, out privatekey);
                                            }
                                            catch (ExternalConnectors.OP.OnePasswordCliException ex)
                                            {
                                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPOnePasswordCommandLine + ": " + ex.Arguments);
                                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPOnePasswordReadFailed + Environment.NewLine + ex.Message);
                                            }

                                            break;
                                        case ExternalCredentialProvider.PasswordSafe:
                                            try
                                            {
                                                ExternalConnectors.PasswordSafe.PasswordSafeCli.ReadPassword(
                                                    $"{Properties.OptionsCredentialsPage.Default.UserViaAPIDefault}", out username, out password, out _, out privatekey);
                                            }
                                            catch (ExternalConnectors.PasswordSafe.PasswordSafeCliException ex)
                                            {
                                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ECPPasswordSafeCommandLine + ": " + ex.Arguments);
                                                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ECPPasswordSafeReadFailed + Environment.NewLine + ex.Message);
                                            }

                                            break;
                                    }

                                    break;
                            }
                        }


                        if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(optionalTemporaryPrivateKeyPath))
                        {
                            if (string.Equals(Properties.OptionsCredentialsPage.Default.EmptyCredentials, "custom", StringComparison.Ordinal))
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
                                // Prepend domain if set and username isn't already domain-qualified
                                string loginName = !string.IsNullOrEmpty(domain) && !username.Contains('\\') && !username.Contains('@')
                                    ? domain + @"\" + username
                                    : username;
                                arguments.Add("-l", loginName);
                            }

                                                    if (!string.IsNullOrEmpty(password))
                                                    {
                                                        Version puttyVersion = PuttyTypeDetector.GetPuttyVersion(PuttyPath ?? string.Empty);
                                                        // -pwfile was introduced in PuTTY 0.81
                                                        if (puttyVersion >= new Version(0, 81))
                                                        {
                                                            string random = string.Join("", Guid.NewGuid().ToString("n").Take(8));
                                                            // write data to pipe
                                                            Thread thread = new(new ParameterizedThreadStart(CreatePipe));
                                                            thread.Start($"{random}{password}");
                                                            // start putty with piped password
                                                            arguments.Add("-pwfile", $"\\\\.\\PIPE\\mRemoteNGSecretPipe{random}");
                                                        }
                                                        else
                                                        {
                                                            arguments.Add("-pw", password);
                                                        }
                                                        // Disable interactive prompts so PuTTY exits on auth failure
                                                        // instead of hanging with a password retry prompt (#1213)
                                                        if (!IsSshTunnelSession())
                                                        {
                                                            arguments.Add("-batch");
                                                        }
                                                    }                        }

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
                                if (!string.Equals(pingMessage, "ping", StringComparison.Ordinal)) throw new FormatException("Invalid ping from VaultOpenbao SSH OTP plugin");
                                await writer.WriteLineAsync("pong");
                                string dataRequest = await reader.ReadLineAsync(cts) ?? throw new FormatException("Invalid data request from VaultOpenbao SSH OTP plugin");
                                var data = DeserializeData(dataRequest);
                                if (data.Username != username || data.Hostname != InterfaceControl.Info.Hostname || data.Port != InterfaceControl.Info.Port)
                                    throw new FormatException("Mismatched data request from VaultOpenbao SSH OTP plugin");
                                await writer.WriteLineAsync(password);
                            }).ConfigureAwait(false);
                        }

                        // use private key if specified; otherwise try auto-discovery of default keys
                        if (!string.IsNullOrEmpty(optionalTemporaryPrivateKeyPath))
                        {
                            arguments.Add("-i", optionalTemporaryPrivateKeyPath);
                        }
                        else if (!string.IsNullOrEmpty(InterfaceControl.Info?.PrivateKeyPath))
                        {
                            arguments.Add("-i", InterfaceControl.Info.PrivateKeyPath);
                        }
                        else if (string.IsNullOrEmpty(password))
                        {
                            // No explicit key or password configured: auto-discover a default SSH key from ~/.ssh/
                            string? discoveredKey = FindDefaultSshKey();
                            if (discoveredKey != null)
                            {
                                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                    $"No private key configured; auto-discovered SSH key: {discoveredKey}", true);
                                arguments.Add("-i", discoveredKey);
                            }
                        }

                    }

                    arguments.Add("-P", InterfaceControl.Info?.Port.ToString(CultureInfo.InvariantCulture) ?? "22");
                    arguments.Add(InterfaceControl.Info?.Hostname ?? "");
                }

                if (_isPuttyNg)
                {
                    arguments.Add("-hwndparent", InterfaceControl.Handle.ToString(CultureInfo.InvariantCulture));
                }

                PuttyProcess.StartInfo.Arguments = arguments.ToString();
                // add additional SSH options, f.e. tunnel or noshell parameters that may be specified for the connection
                // Only apply to SSH protocols — SSHOptions must not leak to Telnet/Rlogin/Raw/Serial (#1056)
                if (PuttyProtocol == Putty_Protocol.ssh && !string.IsNullOrEmpty(InterfaceControl.Info?.SSHOptions))
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
                _processStartTicks = Environment.TickCount64;

                _windowSearchStartTime = Environment.TickCount;
                _windowSearchTimer = new System.Windows.Forms.Timer();
                _windowSearchTimer.Interval = 50;
                _windowSearchTimer.Tick += WindowSearchTimer_Tick;
                _windowSearchTimer.Start();

                return true;
            }
            catch (Exception ex)
            {
                StopTerminalTitleTracking();
                ResetPostOpenLayoutResizeState();
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ConnectionFailed + Environment.NewLine + ex.Message);
                return false;
            }
            finally
            {
                // Securely wipe then delete the temporary private key file
                if (!string.IsNullOrEmpty(optionalTemporaryPrivateKeyPath))
                {
                    System.Threading.Thread.Sleep(500);
                    try
                    {
                        if (System.IO.File.Exists(optionalTemporaryPrivateKeyPath))
                        {
                            var fi = new System.IO.FileInfo(optionalTemporaryPrivateKeyPath);
                            long length = fi.Length;
                            using (var fs = new System.IO.FileStream(optionalTemporaryPrivateKeyPath, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.None))
                            {
                                byte[] zeros = new byte[Math.Min(length, 4096)];
                                long remaining = length;
                                while (remaining > 0)
                                {
                                    int toWrite = (int)Math.Min(remaining, zeros.Length);
                                    fs.Write(zeros, 0, toWrite);
                                    remaining -= toWrite;
                                }
                                fs.Flush();
                            }
                        }
                    }
                    catch { /* best-effort wipe */ }
                    try { System.IO.File.Delete(optionalTemporaryPrivateKeyPath); } catch { }
                }
            }
        }

        public override void Focus()
        {
            try
            {
                if (PuttyHandle == IntPtr.Zero)
                    return;

                IntPtr foregroundWindow = NativeMethods.GetForegroundWindow();
                IntPtr connectionWindowHandle = InterfaceControl.FindForm()?.Handle ?? IntPtr.Zero;
                IntPtr mainWindowHandle = FrmMain.IsCreated ? FrmMain.Default.Handle : IntPtr.Zero;

                // Avoid stealing focus from unrelated windows during taskbar/app switching.
                if (foregroundWindow != PuttyHandle &&
                    foregroundWindow != connectionWindowHandle &&
                    foregroundWindow != mainWindowHandle)
                {
                    return;
                }

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
                if (InterfaceControl.Size == Size.Empty || PuttyHandle == IntPtr.Zero)
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

        public override void OnPowerModeChanged(PowerModes powerMode)
        {
            if (powerMode != PowerModes.Resume)
                return;

            // After hibernate/sleep, GPU and display drivers may take a variable amount
            // of time to restore.  Fire multiple resize attempts with increasing delays
            // so the PuTTY window fills its container as soon as the UI is ready.
            int[] delays = PowerModeChangedResizeDelay == 0
                ? [0]  // test override — single immediate attempt
                : [PowerModeChangedResizeDelay, PowerModeChangedResizeDelay * 2, PowerModeChangedResizeDelay * 4];

            foreach (int delay in delays)
            {
                ScheduleResizeAfterDelay(delay);
            }

            // Mark SSH sessions for auto-reconnect: if the PuTTY process exits with a
            // network error shortly after resume, we reconnect automatically instead of
            // showing the "connection closed" state.
            if (PuttyProtocol == Putty_Protocol.ssh && isRunning())
            {
                _pendingResumeReconnect = true;
                _resumeEventTickCount = Environment.TickCount64;
            }
        }

        private void ScheduleResizeAfterDelay(int delayMs)
        {
            void DoResize()
            {
                try
                {
                    if (InterfaceControl != null && !InterfaceControl.IsDisposed && InterfaceControl.IsHandleCreated)
                    {
                        InterfaceControl.BeginInvoke((MethodInvoker)(() => Resize(this, EventArgs.Empty)));
                    }
                }
                catch (Exception)
                {
                    _ = 0; // Ignore if we can't invoke (e.g. app closing)
                }
            }

            if (delayMs <= 0)
            {
                DoResize();
                return;
            }

            System.Threading.Tasks.Task.Delay(delayMs).ContinueWith(_ => DoResize(),
                System.Threading.Tasks.TaskScheduler.Default);
        }

        private bool IsSshTunnelSession()
        {
            ConnectionInfo? info = InterfaceControl?.Info;
            if (info == null)
                return false;

            if (info.Protocol != ProtocolType.SSH1 && info.Protocol != ProtocolType.SSH2)
                return false;

            string sshOptions = info.SSHOptions ?? string.Empty;
            return sshOptions.Contains(" -L ", StringComparison.OrdinalIgnoreCase) ||
                   sshOptions.StartsWith("-L ", StringComparison.OrdinalIgnoreCase);
        }

        private bool TryClosePuttyGracefully()
        {
            if (PuttyProcess == null || PuttyProcess.HasExited)
                return true;

            bool closeRequested = false;
            if (PuttyHandle != IntPtr.Zero)
            {
                closeRequested = NativeMethods.PostMessage(
                    PuttyHandle,
                    NativeMethods.WM_CLOSE,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }

            if (!closeRequested)
                closeRequested = PuttyProcess.CloseMainWindow();

            if (!closeRequested)
                return false;

            return PuttyProcess.WaitForExit(1000);
        }

        public override void Close()
        {
            if (InterfaceControl != null)
            {
                InterfaceControl.Resize -= Resize;
            }

            StopTerminalTitleTracking();
            StopWindowSearch();
            StopOpeningCommandTimer();
            ResetPostOpenLayoutResizeState();

            try
            {
                if (PuttyProcess?.HasExited == false)
                {
                    bool processExited = TryClosePuttyGracefully();
                    if (!processExited && !IsSshTunnelSession())
                    {
                        PuttyProcess.Kill();
                    }
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

        public void CopyAllToClipboard()
        {
            try
            {
                if (PuttyHandle != IntPtr.Zero)
                {
                    NativeMethods.PostMessage(PuttyHandle, NativeMethods.WM_SYSCOMMAND, (IntPtr)0x0170, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Failed to copy session output to clipboard: " + ex.Message, true);
            }
        }

        #endregion

        #region Enums

#pragma warning disable CA1707 // Legacy PuTTY protocol enum names; renaming would require updating all subclasses
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
#pragma warning restore CA1707

        #endregion

        #region Private Helpers

        private static string? FindDefaultSshKey()
        {
            string sshDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh");
            if (!Directory.Exists(sshDir))
                return null;

            // Prefer PuTTY-format (.ppk) keys first, then OpenSSH format (supported by PuTTY 0.75+)
            string[] defaultKeyNames =
            [
                "id_ed25519.ppk", "id_rsa.ppk", "id_ecdsa.ppk",
                "id_ed25519", "id_rsa", "id_ecdsa", "id_dsa"
            ];
            foreach (string keyName in defaultKeyNames)
            {
                string candidate = Path.Combine(sshDir, keyName);
                if (File.Exists(candidate))
                    return candidate;
            }
            return null;
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
                uint.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(strings[2])), CultureInfo.InvariantCulture)
            );
        }
        #endregion
    }
}
