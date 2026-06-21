using Microsoft.IdentityModel.Tokens;

using mRemoteNG.App.Update;
using mRemoteNG.Config.Settings;
using mRemoteNG.UI.Forms;
using mRemoteNG.Resources.Language;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



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
            // Must be called before any other WinForms / Application.* usage so that
            // per-monitor font scaling and hit-testing are initialised correctly from
            // the very first UI operation (dialogs shown in MainAsync, exception
            // handlers, EnableVisualStyles, …).  The app manifest already declares
            // PerMonitorV2 awareness; this call keeps the WinForms runtime in sync.
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            // Ensure the real entry point is definitely STA
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static Task MainAsync(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

#if !SELF_CONTAINED
            // Runtime checks only needed for framework-dependent deployments
            // Self-contained builds include the runtime, so no check is needed
            // Note: .NET runtime check is not needed here — the .NET host (apphost)
            // natively displays a missing-runtime dialog with a download link.

            var checkFail = false;

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

            Lazy<bool> singleInstanceOption = new(() => Properties.OptionsStartupExitPage.Default.SingleInstance);
            if (singleInstanceOption.Value)
                StartApplicationAsSingleInstance();
            else
                StartApplication();

            return Task.CompletedTask;
        }

        // Assembly resolve handler
        private static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
        {
            try
            {
                string assemblyName = new AssemblyName(args.Name).Name ?? string.Empty;
                if (assemblyName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                    return null;

                string assemblyFile = assemblyName + ".dll";
                string assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assemblies", assemblyFile);

                if (File.Exists(assemblyPath))
                    return Assembly.LoadFrom(assemblyPath);
            }
            catch
            {
                // Suppress resolution exceptions; return null to continue standard probing
            }
            return null;
        }

        private static void StartApplication()
        {
            CatchAllUnhandledExceptions();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ShowSplashOnStaThread();

            Application.Run(FrmMain.Default);
        }

        public static void CloseSingletonInstanceMutex()
        {
            _mutex?.Close();
        }

        private static void StartApplicationAsSingleInstance()
        {
            const string mutexID = "mRemoteNG_SingleInstanceMutex";
            _mutex = new Mutex(false, mutexID, out bool newInstanceCreated);
            if (!newInstanceCreated)
            {
                SwitchToCurrentInstance();
                return;
            }

            StartApplication();
            GC.KeepAlive(_mutex);
        }

        private static void SwitchToCurrentInstance()
        {
            IntPtr singletonInstanceWindowHandle = GetRunningSingletonInstanceWindowHandle();
            if (singletonInstanceWindowHandle == IntPtr.Zero) return;
            if (NativeMethods.IsIconic(singletonInstanceWindowHandle) != 0)
                _ = NativeMethods.ShowWindow(singletonInstanceWindowHandle, (int)NativeMethods.SW_RESTORE);
            NativeMethods.SetForegroundWindow(singletonInstanceWindowHandle);
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
            Exception exception = e.ExceptionObject as Exception
                                  ?? new Exception(e.ExceptionObject?.ToString() ?? "Unknown error");
            FrmUnhandledException window = new(exception, e.IsTerminating);
            window.ShowDialog(FrmMain.Default);
        }

        private static void ShowSplashOnStaThread()
        {
            _wpfSplashThread = new System.Threading.Thread(() =>
            {
                _wpfSplash = FrmSplashScreenNew.GetInstance();

                // Center the splash screen on the primary screen before showing it
                _wpfSplash.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                _wpfSplash.ShowInTaskbar = false;
                _wpfSplash.Show();
                System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(_wpfSplash);
                System.Windows.Threading.Dispatcher.Run(); // WPF message loop
            })
            { IsBackground = true };
            _wpfSplashThread.SetApartmentState(System.Threading.ApartmentState.STA);
            _wpfSplashThread.Start();
        }

        private static void CloseSplash()
        {
            if (_wpfSplash != null)
            {
                _wpfSplash.Dispatcher.Invoke(() => _wpfSplash.Close());
                _wpfSplash = null;
            }
            if (_wpfSplashThread != null)
            {
                _wpfSplashThread.Join();
                _wpfSplashThread = null;
            }
        }

        // Helper to show a dialog with "Download" and "Cancel" buttons.
        // Returns DialogResult.OK if Download clicked, otherwise DialogResult.Cancel.
        // When hasValidUrl is false, the Download button is disabled.
        private static DialogResult ShowDownloadCancelDialog(string message, string caption, bool hasValidUrl = true)
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
                if (!hasValidUrl)
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
                Enabled = hasValidUrl,
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