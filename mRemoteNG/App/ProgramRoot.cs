using Microsoft.IdentityModel.Tokens;

using mRemoteNG.App.Update;
using mRemoteNG.Config.Settings;
using mRemoteNG.DotNet.Update;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;



namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public static class ProgramRoot
    {
        private static Mutex? _mutex;
        private static string customResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages");

        private static System.Threading.Thread? _wpfSplashThread;
        private static FrmSplashScreenNew? _wpfSplash;

        [STAThread]
        public static void Main(string[] args)
        {
            // Smoke test: --version prints version and exits immediately (no GUI)
            if (args.Length > 0 && string.Equals(args[0], "--version", StringComparison.Ordinal))
            {
                var version = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                    ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                    ?? "unknown";
                Console.WriteLine(version);
                Environment.Exit(0);
            }

            // Ensure the real entry point is definitely STA
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static Task MainAsync(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            CommandLineParser commandLineParser = new(args);
            commandLineParser.ApplySwitches();
            args = commandLineParser.GetNormalizedArguments();

#if !SELF_CONTAINED
            // Runtime checks only needed for framework-dependent deployments
            // Self-contained builds include the runtime, so no check is needed
            string? installedVersion = DotNetRuntimeCheck.GetLatestDotNetRuntimeVersion();
            //installedVersion = ""; // Force check for testing purposes

            var checkFail = false;

            // Checking .NET Runtime version
            var (latestRuntimeVersion, downloadUrl) = DotNetRuntimeCheck.GetLatestAvailableDotNetVersionAsync().GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(installedVersion))
            {
                try
                {
                    var result = ShowDownloadCancelDialog(
                        $".NET " + DotNetRuntimeCheck.RequiredDotnetVersion + ".0 " + Language.MsgRuntimeIsRequired + "\n\n" +
                        Language.MsgDownloadLatestRuntime + "\n" + downloadUrl + "\n\n" +
                        Language.MsgExit + "\n\n",
                        Language.MsgMissingRuntime + " .NET " + DotNetRuntimeCheck.RequiredDotnetVersion);

                    if (result == DialogResult.OK && InternetConnection.IsPosible())
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(fileName: downloadUrl) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unable to open download link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch { }
                checkFail = true;
            }

            // Checking Visual C++ Redistributable version
            if (VCppRuntimeCheck.GetInstalledVcRedistVersions() == null || VCppRuntimeCheck.GetInstalledVcRedistVersions().Count == 0)
            {
                var downloadUrl2 = "https://aka.ms/vs/17/release/vc_redist.x64.exe";
                try
                {
                    var result = ShowDownloadCancelDialog(
                        $"A Visual C++ (MSVC) " + Language.MsgRuntimeIsRequired + "\n\n" +
                        Language.MsgDownloadLatestRuntime + "\n" + downloadUrl2 + "\n\n" +
                        Language.MsgExit + "\n\n",
                        Language.MsgMissingRuntime + " Visual C++ Redistributable x64");

                    if (result == DialogResult.OK && InternetConnection.IsPosible())
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(fileName: downloadUrl2) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unable to open download link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch { }
                checkFail = true;
            }

            if (checkFail)
            {
                Environment.Exit(0);
            }
#endif

            // Wire portable settings provider before any settings access
            Config.Settings.Providers.PortableSettingsInitializer.EnsureInitialized();

            bool singleInstance = false;
            try
            {
                singleInstance = Properties.OptionsStartupExitPage.Default.SingleInstance;
            }
            catch (ConfigurationErrorsException ex)
            {
                HandleCorruptedUserConfig(ex);
            }

            if (singleInstance)
                StartApplicationAsSingleInstance(args);
            else
                StartApplication(args);

            return Task.CompletedTask;
        }

        // Assembly resolve handler — constrained to application directory only.
        // Only loads assemblies from known subdirectories (Languages/, Assemblies/)
        // under the application base path to avoid loading from arbitrary locations.
        private static readonly string _appBaseDir = AppDomain.CurrentDomain.BaseDirectory;

        private static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
        {
            try
            {
                AssemblyName asmName = new(args.Name);
                string name = asmName.Name ?? string.Empty;

                if (name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                {
                    // Satellite assemblies: probe Languages/{culture}/
                    string? culture = asmName.CultureName;
                    if (!string.IsNullOrEmpty(culture))
                    {
                        string satPath = Path.Combine(customResourcePath, culture, name + ".dll");
                        if (File.Exists(satPath) && IsUnderAppBase(satPath))
                            return Assembly.LoadFrom(satPath);
                    }
                    return null;
                }

                // Non-resource assemblies: probe Assemblies/ subfolder
                string assemblyFile = name + ".dll";
                string assemblyPath = Path.Combine(_appBaseDir, "Assemblies", assemblyFile);

                if (File.Exists(assemblyPath) && IsUnderAppBase(assemblyPath))
                    return Assembly.LoadFrom(assemblyPath);
            }
            catch
            {
                // Suppress resolution exceptions; return null to continue standard probing
            }
            return null;
        }

        /// <summary>
        /// Validates that a resolved assembly path is under the application base directory.
        /// Prevents loading assemblies from arbitrary/untrusted locations.
        /// </summary>
        private static bool IsUnderAppBase(string path)
        {
            string fullPath = Path.GetFullPath(path);
            return fullPath.StartsWith(_appBaseDir, StringComparison.OrdinalIgnoreCase);
        }

        private static void CheckLockalDB()
        {
            LocalDBManager settingsManager = new LocalDBManager(dbPath: "mRemoteNG.appSettings", useEncryption: false, schemaFilePath: "");
        }

        private static void StartApplication(string[]? args = null)
        {
            CatchAllUnhandledExceptions();

            // Fix #2062: ensure DockPanelSuite computes drag indicators correctly
            // across secondary monitors with different DPI/scaling.
            PatchController.EnablePerScreenDpi = true;

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Pass command-line args to Startup AFTER Application.Set* calls
            // to avoid premature Control/handle creation (fix fork#19)
            Startup.Instance.CommandLineArgs = args;

            ShowSplashOnStaThread();

            Application.Run(FrmMain.Default);
        }

        public static void CloseSingletonInstanceMutex()
        {
            _mutex?.Close();
        }

        private static void StartApplicationAsSingleInstance(string[] args)
        {
            const string mutexID = "mRemoteNG_SingleInstanceMutex";
            _mutex = new Mutex(false, mutexID, out bool newInstanceCreated);
            if (!newInstanceCreated)
            {
                SwitchToCurrentInstance(args);
                return;
            }

            StartApplication(args);
            GC.KeepAlive(_mutex);
        }

        private static void SwitchToCurrentInstance(string[] args)
        {
            IntPtr singletonInstanceWindowHandle = GetRunningSingletonInstanceWindowHandle();
            if (singletonInstanceWindowHandle == IntPtr.Zero) return;
            if (NativeMethods.IsIconic(singletonInstanceWindowHandle) != 0)
                _ = NativeMethods.ShowWindow(singletonInstanceWindowHandle, (int)NativeMethods.SW_RESTORE);
            NativeMethods.SetForegroundWindow(singletonInstanceWindowHandle);

            // Always send an activate signal so the running instance can bring itself
            // to front from its own context. SetForegroundWindow from another process
            // is blocked by Windows focus-stealing prevention; the running instance
            // calling SetForegroundWindow on itself is reliable.
            SendActivateToRunningInstance(singletonInstanceWindowHandle);

            if (args != null && args.Length > 0)
            {
                SendArgsToRunningInstance(singletonInstanceWindowHandle, args);
            }
        }

        private static void SendActivateToRunningInstance(IntPtr hWnd)
        {
            NativeMethods.COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)2; // dwData == 2: "bring to front" signal
            cds.cbData = 0;
            cds.lpData = IntPtr.Zero;
            NativeMethods.SendMessage(hWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ref cds);
        }

        private static void SendArgsToRunningInstance(IntPtr hWnd, string[] args)
        {
            string[] normalizedArgs = new CommandLineParser(args).GetNormalizedArguments();
            string message = string.Join("\n", normalizedArgs);

            NativeMethods.COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)1; // ID for args
            cds.cbData = (message.Length + 1) * 2;
            cds.lpData = Marshal.StringToHGlobalUni(message);
            
            try
            {
                NativeMethods.SendMessage(hWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ref cds);
            }
            finally
            {
                Marshal.FreeHGlobal(cds.lpData);
            }
        }

        private static IntPtr GetRunningSingletonInstanceWindowHandle()
        {
            IntPtr windowHandle = IntPtr.Zero;
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process enumeratedProcess in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                // Safely check for null MainModule and FileName
                string? enumeratedFileName = null;
                string? currentFileName = null;
                try
                {
                    enumeratedFileName = enumeratedProcess.MainModule?.FileName;
                    currentFileName = currentProcess.MainModule?.FileName;
                }
                catch
                {
                    // Access to MainModule can throw exceptions for some processes; ignore and continue
                    continue;
                }

                if (enumeratedProcess.Id != currentProcess.Id &&
                    !string.IsNullOrEmpty(enumeratedFileName) &&
                    !string.IsNullOrEmpty(currentFileName) &&
                    enumeratedFileName == currentFileName &&
                    enumeratedProcess.MainWindowHandle != IntPtr.Zero)
                    windowHandle = enumeratedProcess.MainWindowHandle;
            }

            return windowHandle;
        }

        /// <summary>
        /// Handles a corrupted user.config file by logging diagnostics, backing up the
        /// corrupted file, and deleting it so the application can continue with defaults.
        /// </summary>
        internal static void HandleCorruptedUserConfig(ConfigurationErrorsException ex)
        {
            string configPath = GetConfigFilePathFromException(ex);
            string logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Application.ProductName ?? "mRemoteNG",
                "user.config-error.log");

            try
            {
                string? logDir = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(logDir))
                    Directory.CreateDirectory(logDir);
            }
            catch { /* best effort */ }

            string logEntry =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] user.config load failed{Environment.NewLine}" +
                $"  Config file: {configPath}{Environment.NewLine}" +
                $"  Error: {ex.Message}{Environment.NewLine}" +
                $"  Inner: {ex.InnerException?.Message}{Environment.NewLine}" +
                $"  Stack: {ex.Demystify().StackTrace}{Environment.NewLine}{Environment.NewLine}";

            try { File.AppendAllText(logPath, logEntry); }
            catch { /* best effort */ }

            if (!string.IsNullOrEmpty(configPath) && File.Exists(configPath))
            {
                try
                {
                    string backup = configPath + ".corrupted." + DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                    File.Copy(configPath, backup, true);
                    File.Delete(configPath);
                }
                catch { /* best effort */ }
            }

            MessageBox.Show(
                $"Your settings file was corrupted and could not be loaded.{Environment.NewLine}{Environment.NewLine}" +
                $"File: {configPath}{Environment.NewLine}" +
                $"Error: {ex.InnerException?.Message ?? ex.Message}{Environment.NewLine}{Environment.NewLine}" +
                $"The corrupted file has been backed up and settings have been reset to defaults.{Environment.NewLine}" +
                $"Diagnostic details logged to: {logPath}",
                "mRemoteNG - Settings Reset",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private static string GetConfigFilePathFromException(ConfigurationErrorsException ex)
        {
            if (!string.IsNullOrEmpty(ex.Filename))
                return ex.Filename;
            if (ex.InnerException is ConfigurationErrorsException inner && !string.IsNullOrEmpty(inner.Filename))
                return inner.Filename;
            try { return Info.SettingsFileInfo.UserSettingsFilePath; }
            catch { return "(unknown path)"; }
        }

        private static void CatchAllUnhandledExceptions()
        {
            Application.ThreadException += ApplicationOnThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            CloseSplash();
            if (FrmMain.Default.IsDisposed) return;
            FrmUnhandledException window = new(e.Exception, false);
            window.ShowDialog(FrmMain.Default);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            FrmUnhandledException window = new(e.ExceptionObject as Exception ?? new InvalidOperationException(e.ExceptionObject?.ToString()), e.IsTerminating);
            window.ShowDialog(FrmMain.Default);
        }

        private static void ShowSplashOnStaThread()
        {
            _wpfSplashThread = new System.Threading.Thread(() =>
            {
                _wpfSplash = FrmSplashScreenNew.GetInstance();

                _wpfSplash.ShowInTaskbar = false;
                _wpfSplash.Show();
                System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(_wpfSplash);
                System.Windows.Threading.Dispatcher.Run(); // WPF message loop
            })
            { IsBackground = true };
            _wpfSplashThread.SetApartmentState(System.Threading.ApartmentState.STA);
            _wpfSplashThread.Start();
        }

        public static void CloseSplash()
        {
            if (_wpfSplash == null)
            {
                if (_wpfSplashThread != null)
                {
                    _wpfSplashThread.Join(TimeSpan.FromMilliseconds(100));
                    _wpfSplashThread = null;
                }
                return;
            }

            try
            {
                var splash = _wpfSplash;
                _wpfSplash = null; // Set to null first to avoid multiple calls

                if (splash.Dispatcher.HasShutdownStarted) return;

                if (splash.Dispatcher.CheckAccess())
                {
                    splash.Close();
                    splash.Dispatcher.InvokeShutdown();
                }
                else
                {
                    splash.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            splash.Close();
                            splash.Dispatcher.InvokeShutdown();
                        }
                        catch
                        {
                            // Ignore errors during async close
                        }
                    }));
                }
            }
            catch (TaskCanceledException) { /* Intentionally empty */ }
            catch (OperationCanceledException) { /* Intentionally empty */ }
            catch (Exception ex) { _ = ex; }
            finally
            {
                if (_wpfSplashThread != null)
                {
                    // Don't join if we're already on that thread
                    if (System.Threading.Thread.CurrentThread != _wpfSplashThread)
                    {
                        _wpfSplashThread.Join(TimeSpan.FromMilliseconds(500));
                    }
                    _wpfSplashThread = null;
                }
            }
        }

        // Helper to show a dialog with "Download" and "Cancel" buttons.
        // Returns DialogResult.OK if Download clicked, otherwise DialogResult.Cancel.
        private static DialogResult ShowDownloadCancelDialog(string message, string caption)
        {
            using Form dialog = new Form()
            {
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                ClientSize = new Size(560, 200),
                Icon = SystemIcons.Information
            };

            // Try to find a URL in the message (very simple heuristic: first "http" until whitespace/newline)
            int urlStart = message.IndexOf("http", StringComparison.OrdinalIgnoreCase);
            string? url = null;
            if (urlStart >= 0)
            {
                int urlEnd = message.IndexOfAny(new char[] { ' ', '\r', '\n', '\t' }, urlStart);
                if (urlEnd == -1) urlEnd = message.Length;
                url = message.Substring(urlStart, urlEnd - urlStart);
            }

            LinkLabel lbl = new LinkLabel()
            {
                AutoSize = false,
                Text = message,
                Location = new Point(12, 12),
                Size = new Size(dialog.ClientSize.Width - 24, dialog.ClientSize.Height - 60),
                TextAlign = ContentAlignment.TopLeft,
                LinkBehavior = LinkBehavior.SystemDefault
            };
            lbl.MaximumSize = new Size(dialog.ClientSize.Width - 24, 0);

            if (!string.IsNullOrEmpty(url) && urlStart >= 0)
            {
                // Ensure link indices are within bounds of the LinkLabel text
                int linkStartInLabel = urlStart;
                int linkLength = url.Length;
                if (linkStartInLabel + linkLength <= lbl.Text.Length)
                {
                    lbl.Links.Add(linkStartInLabel, linkLength, url);
                }
            }

            lbl.LinkClicked += (s, e) =>
            {
                string? linkUrl = e.Link?.LinkData as string;
                if (string.IsNullOrEmpty(linkUrl))
                    return;
                if (!InternetConnection.IsPosible())
                {
                    MessageBox.Show("No internet connection is available.", "Network", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Treat clicking the link the same as clicking the "Download" button:
                // set DialogResult to OK so the caller receives DialogResult.OK and can proceed to open the download URL.
                dialog.DialogResult = DialogResult.OK;
                // Do not call Process.Start here to avoid duplicate launches; caller already opens the URL when it sees DialogResult.OK.
            };

            Button btnDownload = new Button()
            {
                Text = "Download",
                DialogResult = DialogResult.OK,
                Size = new Size(100, 28),
            };
            Button btnCancel = new Button()
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Size = new Size(100, 28),
            };

            // Position buttons
            int padding = 12;
            btnCancel.Location = new Point(dialog.ClientSize.Width - padding - btnCancel.Width, dialog.ClientSize.Height - padding - btnCancel.Height);
            btnDownload.Location = new Point(btnCancel.Left - 8 - btnDownload.Width, btnCancel.Top);

            // Set dialog defaults
            dialog.Controls.Add(lbl);
            dialog.Controls.Add(btnDownload);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnDownload;
            dialog.CancelButton = btnCancel;

            // Adjust label height to wrap text properly
            lbl.Height = btnCancel.Top - lbl.Top - 8;

            return dialog.ShowDialog();
        }
    }
}