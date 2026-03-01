using System;
using System.Diagnostics;
using System.IO;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace mRemoteNGSpecs.Drivers
{
    /// <summary>
    /// Manages the mRemoteNG application process lifecycle for UI automation tests.
    /// </summary>
    public sealed class AppDriver : IDisposable
    {
        private Application? _application;
        private UIA3Automation? _automation;
        private bool _disposed;

        /// <summary>
        /// The FlaUI automation instance used for UI interactions.
        /// </summary>
        public UIA3Automation Automation => _automation ?? throw new InvalidOperationException("AppDriver not started.");

        /// <summary>
        /// The FlaUI application wrapper.
        /// </summary>
        public Application Application => _application ?? throw new InvalidOperationException("AppDriver not started.");

        /// <summary>
        /// Starts the mRemoteNG application and waits for the main window.
        /// </summary>
        /// <param name="timeout">Maximum time to wait for the main window.</param>
        /// <returns>The main window element.</returns>
        public Window Start(TimeSpan? timeout = null)
        {
            var exePath = FindExecutable();
            _automation = new UIA3Automation();
            _application = Application.Launch(exePath);

            var mainWindow = _application.GetMainWindow(_automation, timeout ?? TimeSpan.FromSeconds(30));
            return mainWindow;
        }

        /// <summary>
        /// Locates the mRemoteNG executable relative to the test assembly output.
        /// Build outputs both projects to bin\{platform}\{config}\ under their project dirs.
        /// </summary>
        private static string FindExecutable()
        {
            // Test assembly is at: mRemoteNGSpecs\bin\x64\Release\mRemoteNGSpecs.dll
            // App executable is at: mRemoteNG\bin\x64\Release\mRemoteNG.exe
            var testDir = AppContext.BaseDirectory;

            // Walk up from mRemoteNGSpecs\bin\x64\Release to the repo root
            var repoRoot = Path.GetFullPath(Path.Combine(testDir, "..", "..", "..", ".."));
            var exePath = Path.Combine(repoRoot, "mRemoteNG", "bin", "x64", "Release", "mRemoteNG.exe");

            if (!File.Exists(exePath))
                throw new FileNotFoundException(
                    $"mRemoteNG.exe not found at '{exePath}'. Build the main project first with build.ps1.",
                    exePath);

            return exePath;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                _application?.Close();
            }
            catch
            {
                // Best-effort close; kill if still running.
            }

            try
            {
                if (_application?.HasExited == false)
                    _application.Kill();
            }
            catch
            {
                // Process may already be gone.
            }

            _automation?.Dispose();
            _application?.Dispose();
        }
    }
}
