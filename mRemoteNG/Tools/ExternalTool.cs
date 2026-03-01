using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class ExternalTool : INotifyPropertyChanged
    {
        private string _displayName = string.Empty; // Initialize to avoid CS8618
        private string _fileName = string.Empty; // Initialize to avoid CS8618
        private bool _waitForExit;
        private string _arguments = string.Empty; // Initialize to avoid CS8618
        private string _workingDir = string.Empty; // Initialize to avoid CS8618
        private string _category = string.Empty;
        private bool _tryIntegrate;
        private bool _showOnToolbar = true;
        private bool _hidden;
        private bool _runElevated;
        private bool _runOnStartup;
        private bool _stopOnShutdown;
        private Keys _hotkey = Keys.None;
        private string _authenticationType = string.Empty;
        private string _authenticationUsername = string.Empty;
        private string _authenticationPassword = string.Empty;
        private string _privateKeyFile = string.Empty;
        private string _passphrase = string.Empty;

        #region Public Properties

        public string DisplayName
        {
            get => _displayName;
            set => SetField(ref _displayName, value, nameof(DisplayName));
        }

        public string FileName
        {
            get => _fileName;
            set => SetField(ref _fileName, value, nameof(FileName));
        }

        public bool WaitForExit
        {
            get => _waitForExit;
            set
            {
                // WaitForExit cannot be turned on when TryIntegrate is true
                if (TryIntegrate)
                    return;
                SetField(ref _waitForExit, value, nameof(WaitForExit));
            }
        }

        public string Arguments
        {
            get => _arguments;
            set => SetField(ref _arguments, value, nameof(Arguments));
        }

        public string WorkingDir
        {
            get => _workingDir;
            set => SetField(ref _workingDir, value, nameof(WorkingDir));
        }

        public bool TryIntegrate
        {
            get => _tryIntegrate;
            set
            {
                // WaitForExit cannot be turned on when TryIntegrate is true
                if (value)
                    WaitForExit = false;
                SetField(ref _tryIntegrate, value, nameof(TryIntegrate));
            }
        }

        public bool ShowOnToolbar
        {
            get => _showOnToolbar;
            set => SetField(ref _showOnToolbar, value, nameof(ShowOnToolbar));
        }

        public string Category
        {
            get => _category;
            set => SetField(ref _category, value, nameof(Category));
        }

        public bool Hidden
        {
            get => _hidden;
            set => SetField(ref _hidden, value, nameof(Hidden));
        }

        public bool RunElevated
        {
            get => _runElevated;
            set => SetField(ref _runElevated, value, nameof(RunElevated));
        }

        public bool RunOnStartup
        {
            get => _runOnStartup;
            set => SetField(ref _runOnStartup, value, nameof(RunOnStartup));
        }

        public bool StopOnShutdown
        {
            get => _stopOnShutdown;
            set => SetField(ref _stopOnShutdown, value, nameof(StopOnShutdown));
        }

        public Keys Hotkey
        {
            get => _hotkey;
            set => SetField(ref _hotkey, value, nameof(Hotkey));
        }

        public string AuthenticationType
        {
            get => _authenticationType;
            set => SetField(ref _authenticationType, value, nameof(AuthenticationType));
        }

        public string AuthenticationUsername
        {
            get => _authenticationUsername;
            set => SetField(ref _authenticationUsername, value, nameof(AuthenticationUsername));
        }

        public string AuthenticationPassword
        {
            get => _authenticationPassword;
            set => SetField(ref _authenticationPassword, value, nameof(AuthenticationPassword));
        }

        public string PrivateKeyFile
        {
            get => _privateKeyFile;
            set => SetField(ref _privateKeyFile, value, nameof(PrivateKeyFile));
        }

        public string Passphrase
        {
            get => _passphrase;
            set => SetField(ref _passphrase, value, nameof(Passphrase));
        }

        public string IconPath { get; set; } = string.Empty;

        /// <summary>
        /// Tracks the process started by <see cref="StartForAutoRun"/> so it can be
        /// terminated on shutdown when <see cref="StopOnShutdown"/> is enabled.
        /// </summary>
        [Browsable(false)]
        public Process? TrackedProcess { get; private set; }

        public ConnectionInfo ConnectionInfo { get; set; } = new ConnectionInfo(); // Initialize to avoid CS8618

        public Icon Icon
        {
            get
            {
                if (!string.IsNullOrEmpty(IconPath) && File.Exists(IconPath))
                {
                    return MiscTools.GetIconFromFile(IconPath) ?? Properties.Resources.mRemoteNG_Icon;
                }

                return File.Exists(FileName) ? MiscTools.GetIconFromFile(FileName) ?? Properties.Resources.mRemoteNG_Icon : Properties.Resources.mRemoteNG_Icon;
            }
        }

        public Image Image => Icon?.ToBitmap() ?? Properties.Resources.mRemoteNG_Icon.ToBitmap();

        #endregion

        public ExternalTool(string displayName = "",
                            string fileName = "",
                            string arguments = "",
                            string workingDir = "",
                            bool runElevated = false)
        {
            DisplayName = displayName;
            FileName = fileName;
            Arguments = arguments;
            WorkingDir = workingDir;
            RunElevated = runElevated;
        }

        public void Start(ConnectionInfo startConnectionInfo = null!)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ExternalApp.Start() failed: FileName cannot be blank.");
                    return;
                }

                ConnectionInfo = startConnectionInfo ?? new ConnectionInfo(); // Ensure ConnectionInfo is not null
                if (startConnectionInfo is ContainerInfo container)
                {
                    container.Children.ForEach(Start);
                    return;
                }

                if (TryIntegrate)
                    StartIntegrated();
                else
                    StartExternalProcess();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ExternalApp.Start() failed.", ex);
            }
        }

        private void StartExternalProcess()
        {
            Process process = new();
            SetProcessProperties(process, ConnectionInfo);
            process.Start();

            // Attempt to bring the external tool window to the foreground
            // Fixes #1297: External Tool opens behind mRemoteNG window
            Task.Run(async () =>
            {
                try
                {
                    // Poll for the main window handle for up to 5 seconds
                    for (int i = 0; i < 50; i++)
                    {
                        process.Refresh();
                        if (process.MainWindowHandle != IntPtr.Zero)
                        {
                            NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                        await Task.Delay(100);
                    }
                }
                catch
                {
                    // Ignore exceptions (e.g., process exited or handle invalid)
                }
            });

            if (WaitForExit)
            {
                process.WaitForExit();
            }
        }

        /// <summary>
        /// Launches this tool for auto-run on startup and tracks the process for shutdown.
        /// </summary>
        public void StartForAutoRun()
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                        $"ExternalApp.StartForAutoRun() skipped: FileName is blank for '{DisplayName}'.");
                    return;
                }

                Process process = new();
                SetProcessProperties(process, new ConnectionInfo());
                process.Start();
                TrackedProcess = process;

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Auto-started external tool '{DisplayName}' (PID {process.Id}).", true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    $"ExternalApp.StartForAutoRun() failed for '{DisplayName}'.", ex);
            }
        }

        /// <summary>
        /// Stops the tracked process if it is still running.
        /// </summary>
        public void StopTrackedProcess()
        {
            try
            {
                if (TrackedProcess == null || TrackedProcess.HasExited)
                    return;

                TrackedProcess.Kill();
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Auto-stopped external tool '{DisplayName}' (PID {TrackedProcess.Id}).", true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(
                    $"ExternalApp.StopTrackedProcess() failed for '{DisplayName}'.", ex);
            }
            finally
            {
                TrackedProcess = null;
            }
        }

        private void SetProcessProperties(Process process, ConnectionInfo startConnectionInfo)
        {
            ExternalToolArgumentParser argParser = new(startConnectionInfo, this);
            string parsedFileName = argParser.ParseArguments(FileName);
            parsedFileName = NormalizeSystem32PathForWow64(parsedFileName);

            // Validate the executable path to prevent command injection
            PathValidator.ValidateExecutablePathOrThrow(parsedFileName, nameof(FileName));

            // Check if we should run as an alternate user (AuthenticationType = "runas").
            // Tokens like %USERNAME%, %PASSWORD%, %DOMAIN% are supported in auth fields.
            bool useAlternateCredentials = string.Equals(AuthenticationType, "runas", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(AuthenticationUsername);

            if (useAlternateCredentials)
            {
                // Alternate user credentials require UseShellExecute = false
                process.StartInfo.UseShellExecute = false;

                string parsedAuthUser = argParser.ParseArguments(AuthenticationUsername, escapeForShell: false);

                // Parse "DOMAIN\username" format into separate domain and username parts
                string userName = parsedAuthUser;
                string domain = string.Empty;
                int backslashIndex = parsedAuthUser.IndexOf('\\');
                if (backslashIndex >= 0)
                {
                    domain = parsedAuthUser.Substring(0, backslashIndex);
                    userName = parsedAuthUser.Substring(backslashIndex + 1);
                }

                process.StartInfo.UserName = userName;
                if (!string.IsNullOrEmpty(domain))
                    process.StartInfo.Domain = domain;

                if (!string.IsNullOrEmpty(AuthenticationPassword))
                {
                    string parsedAuthPassword = argParser.ParseArguments(AuthenticationPassword, escapeForShell: false);
                    var securePassword = new System.Security.SecureString();
                    foreach (char c in parsedAuthPassword)
                        securePassword.AppendChar(c);
                    securePassword.MakeReadOnly();
                    process.StartInfo.Password = securePassword;
                }
            }
            else
            {
                // When RunElevated is true, we must use UseShellExecute = true for the "runas" verb
                // When false, we use UseShellExecute = false for better security with ArgumentList
                process.StartInfo.UseShellExecute = RunElevated;
            }

            process.StartInfo.FileName = parsedFileName;

            bool isBatch = IsBatchFile(parsedFileName);

            if (RunElevated && !useAlternateCredentials)
            {
                process.StartInfo.Verb = "runas";
                // Elevated launch uses ShellExecute; build a safely quoted argument string.
                string rawArgs = argParser.ParseArguments(Arguments, escapeForShell: false);
                var parts = SplitCommandLineArguments(rawArgs);
                process.StartInfo.Arguments = string.Join(" ", parts
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .Select(QuoteArgumentForCmd));
            }
            else
            {
                if (isBatch)
                {
                    // Batch files are routed through cmd.exe by Windows even with UseShellExecute=false.
                    // ArgumentList uses C-runtime quoting which does NOT protect commas from cmd.exe.
                    // Build a manually quoted Arguments string instead.
                    string rawArgs = argParser.ParseArguments(Arguments, escapeForShell: false);
                    var parts = SplitCommandLineArguments(rawArgs);
                    process.StartInfo.Arguments = string.Join(" ", parts
                        .Where(a => !string.IsNullOrWhiteSpace(a))
                        .Select(QuoteArgumentForCmd));
                }
                else
                {
                    // Non-batch: use ArgumentList for proper C-runtime quoting
                    string parsedArguments = argParser.ParseArguments(Arguments, escapeForShell: false);
                    var argumentParts = SplitCommandLineArguments(parsedArguments);
                    foreach (var arg in argumentParts)
                    {
                        if (!string.IsNullOrWhiteSpace(arg))
                        {
                            process.StartInfo.ArgumentList.Add(arg);
                        }
                    }
                }
            }

            if (WorkingDir != "")
            {
                string parsedWorkingDir = argParser.ParseArguments(WorkingDir);
                PathValidator.ValidatePathOrThrow(parsedWorkingDir, nameof(WorkingDir));
                process.StartInfo.WorkingDirectory = parsedWorkingDir;
            }
        }

        /// <summary>
        /// Returns true if the file has a .cmd or .bat extension (batch file).
        /// </summary>
        private static bool IsBatchFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return ext.Equals(".cmd", StringComparison.OrdinalIgnoreCase)
                || ext.Equals(".bat", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Maps %windir%\System32\... to %windir%\Sysnative\... when running under WoW64,
        /// but falls back to the original path if the Sysnative target does not exist.
        /// </summary>
        private static string NormalizeSystem32PathForWow64(string fileName)
        {
            return NormalizeSystem32PathForWow64(
                fileName,
                Environment.Is64BitOperatingSystem,
                Environment.Is64BitProcess,
                File.Exists);
        }

        private static string NormalizeSystem32PathForWow64(
            string fileName,
            bool is64BitOperatingSystem,
            bool is64BitProcess,
            Func<string, bool> fileExists)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return fileName;

            if (!is64BitOperatingSystem || is64BitProcess)
                return fileName;

            string windowsDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            if (string.IsNullOrWhiteSpace(windowsDir))
                windowsDir = Environment.GetEnvironmentVariable("SystemRoot") ?? @"C:\Windows";

            string system32Prefix = Path.Combine(windowsDir, "System32") + Path.DirectorySeparatorChar;
            if (!fileName.StartsWith(system32Prefix, StringComparison.OrdinalIgnoreCase))
                return fileName;

            string relativePath = fileName.Substring(system32Prefix.Length);
            if (string.IsNullOrWhiteSpace(relativePath))
                return fileName;

            string sysnativePath = Path.Combine(windowsDir, "Sysnative", relativePath);
            return fileExists(sysnativePath) ? sysnativePath : fileName;
        }

        /// <summary>
        /// Wraps an argument in double quotes for cmd.exe. Internal quotes are escaped as "".
        /// </summary>
        private static string QuoteArgumentForCmd(string arg)
        {
            return "\"" + arg.Replace("\"", "\"\"") + "\"";
        }
        
        /// <summary>
        /// Splits command line arguments respecting quotes
        /// </summary>
        private static List<string> SplitCommandLineArguments(string arguments)
        {
            List<string> result = new();
            if (string.IsNullOrWhiteSpace(arguments))
                return result;
            
            bool inQuotes = false;
            int startIndex = 0;
            
            for (int i = 0; i < arguments.Length; i++)
            {
                char c = arguments[i];
                
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
                {
                    if (i > startIndex)
                    {
                        string arg = arguments.Substring(startIndex, i - startIndex).Trim();
                        // Remove surrounding quotes if present
                        if (arg.StartsWith('"') && arg.EndsWith('"') && arg.Length > 1)
                            arg = arg.Substring(1, arg.Length - 2);
                        if (!string.IsNullOrWhiteSpace(arg))
                            result.Add(arg);
                    }
                    startIndex = i + 1;
                }
            }

            // Add the last argument
            if (startIndex < arguments.Length)
            {
                string arg = arguments.Substring(startIndex).Trim();
                // Remove surrounding quotes if present
                if (arg.StartsWith('"') && arg.EndsWith('"') && arg.Length > 1)
                    arg = arg.Substring(1, arg.Length - 2);
                if (!string.IsNullOrWhiteSpace(arg))
                    result.Add(arg);
            }
            
            return result;
        }

        private void StartIntegrated()
        {
            try
            {
                ConnectionInfo newConnectionInfo = BuildConnectionInfoForIntegratedApp();
                Runtime.ConnectionInitiator.OpenConnection(newConnectionInfo);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ExternalApp.StartIntegrated() failed.", ex);
            }
        }

        private ConnectionInfo BuildConnectionInfoForIntegratedApp()
        {
            ConnectionInfo newConnectionInfo = GetAppropriateInstanceOfConnectionInfo();
            SetConnectionInfoFields(newConnectionInfo);
            return newConnectionInfo;
        }

        private ConnectionInfo GetAppropriateInstanceOfConnectionInfo()
        {
            ConnectionInfo newConnectionInfo = ConnectionInfo == null ? new ConnectionInfo() : ConnectionInfo.Clone();
            return newConnectionInfo;
        }

        private void SetConnectionInfoFields(ConnectionInfo newConnectionInfo)
        {
            newConnectionInfo.Protocol = ProtocolType.IntApp;
            newConnectionInfo.ExtApp = DisplayName;
            newConnectionInfo.Name = DisplayName;
            newConnectionInfo.Panel = Language._Tools;
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { }; // Updated to match nullability

        protected virtual void RaisePropertyChangedEvent(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChangedEvent(this, propertyName);
            return true;
        }
    }
}
