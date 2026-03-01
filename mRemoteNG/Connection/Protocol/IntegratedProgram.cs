using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public class IntegratedProgram : ProtocolBase
    {
        #region Private Fields

        private ExternalTool? _externalTool;
        private IntPtr _handle;
        private Process? _process;

        #endregion

        #region Public Methods

        public override bool Initialize()
        {
            if (InterfaceControl.Info == null)
                return base.Initialize();

            _externalTool = Runtime.ExternalToolsService.GetExtAppByName(InterfaceControl.Info.ExtApp);
            _externalTool ??= CreateBuiltInShellPresetForIntegration(InterfaceControl.Info.ExtApp);

            if (_externalTool == null)
            {
                Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                                                     string.Format(CultureInfo.InvariantCulture, Language.CouldNotFindExternalTool,
                                                                   InterfaceControl.Info.ExtApp));
                return false;
            }

            _externalTool.ConnectionInfo = InterfaceControl.Info;

            return base.Initialize();
        }

        public override bool Connect()
        {
            try
            {
                if (_externalTool == null)
                    return false;

                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     $"Attempting to start: {_externalTool.DisplayName}", true);

                if (_externalTool.TryIntegrate == false)
                {
                    _externalTool.Start(InterfaceControl.Info);
                    /* Don't call close here... There's nothing for the override to do in this case since
                     * _process is not created in this scenario. When returning false, ProtocolBase.Close()
                     * will be called - which is just going to call IntegratedProgram.Close() again anyway...
                     * Close();
                     */
                    Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                         $"Assuming no other errors/exceptions occurred immediately before this message regarding {_externalTool.DisplayName}, the next \"closed by user\" message can be ignored",
                                                         true);
                    return false;
                }

                ExternalToolArgumentParser argParser = new(_externalTool.ConnectionInfo);
                string parsedFileName = argParser.ParseArguments(_externalTool.FileName);
                string parsedArguments = argParser.ParseArguments(_externalTool.Arguments);

                // Validate the executable path to prevent command injection
                PathValidator.ValidateExecutablePathOrThrow(parsedFileName, nameof(_externalTool.FileName));

                _process = new Process
                {
                    StartInfo =
                    {
                        // Use UseShellExecute = false for better security
                        // Only use true if we need runas for elevation (which IntegratedProgram doesn't use)
                        UseShellExecute = false,
                        FileName = parsedFileName,
                        Arguments = parsedArguments
                    },
                    EnableRaisingEvents = true
                };


                _process.Exited += ProcessExited;

                _process.Start();

                // WaitForInputIdle throws InvalidOperationException for console-based processes
                // (cmd.exe, powershell.exe, etc.) that have no message loop.
                try
                {
                    _process.WaitForInputIdle(Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000);
                }
                catch (InvalidOperationException)
                {
                    // Expected for console apps — continue to handle discovery
                }

                int timeoutMs = Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000;
                int processId = _process.Id;

                // Strategy 1: Poll Process.MainWindowHandle (works for direct GUI apps like PuTTY, Notepad++)
                _handle = PollMainWindowHandle(_process, timeoutMs);

                // Strategy 2: EnumWindows to find any visible top-level window owned by the process ID.
                // This catches windows that .NET's MainWindowHandle heuristic misses.
                if (_handle == IntPtr.Zero)
                {
                    _handle = FindWindowByProcessId(processId, timeoutMs);
                }

                // Strategy 3: Check child processes. Launcher apps (e.g. git-bash.exe) spawn a child
                // process and exit — the actual window belongs to the child.
                if (_handle == IntPtr.Zero)
                {
                    _handle = FindWindowInChildProcesses(processId, timeoutMs);
                }

                // Strategy 4: Known shell launchers can spawn deeper process trees
                // (for example wsl.exe -> wslhost.exe -> conhost.exe).
                if (_handle == IntPtr.Zero && IsCommonShellTool(_externalTool, parsedFileName))
                {
                    _handle = FindWindowInDescendantProcesses(processId, timeoutMs, maxDepth: 4);
                }

                if (_handle == IntPtr.Zero)
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.WarningMsg,
                        $"IntegratedProgram: Could not find a window handle for '{_externalTool.DisplayName}' (PID {processId}). " +
                        "The application may have opened in a separate window.");
                }
                else
                {
                    _ = NativeMethods.GetWindowThreadProcessId(_handle, out uint windowPid);
                    if (windowPid != (uint)_process.Id)
                    {
                        try
                        {
                            Process windowProcess = Process.GetProcessById((int)windowPid);

                            _process.Exited -= ProcessExited;
                            _process = windowProcess;
                            _process.EnableRaisingEvents = true;
                            _process.Exited += ProcessExited;

                            Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                $"IntegratedProgram: Tracking process changed from PID {processId} to PID {windowPid}", true);
                        }
                        catch (Exception ex)
                        {
                            Runtime.MessageCollector?.AddExceptionMessage("IntegratedProgram: Failed to attach to window owner process.", ex);
                        }
                    }
                }

                NativeMethods.SetParent(_handle, InterfaceControl.Handle);

                // Give keyboard focus to the embedded window after re-parenting.
                // Required for Java-based apps (e.g. TigerVNC) where the Java AWT focus model
                // does not automatically acquire Win32 keyboard focus after SetParent (#1442).
                if (_handle != IntPtr.Zero)
                    NativeMethods.SetFocus(_handle);

                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, Language.IntAppStuff, true);
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     string.Format(CultureInfo.InvariantCulture, Language.IntAppHandle, _handle), true);
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     string.Format(CultureInfo.InvariantCulture, Language.IntAppTitle, _process.MainWindowTitle),
                                                     true);
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     string.Format(CultureInfo.InvariantCulture, Language.PanelHandle,
                                                                   InterfaceControl.Parent?.Handle), true);

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
                // SetFocus is required for Java-based embedded apps (e.g. TigerVNC) to receive
                // keyboard input — SetForegroundWindow alone is insufficient for Java AWT windows (#1442).
                if (_handle != IntPtr.Zero)
                    NativeMethods.SetFocus(_handle);
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

        public override void Close()
        {
            /* only attempt this if we have a valid process object
             * Non-integrated tools will still call base.Close() and don't have a valid process object.
             * See Connect() above... This just muddies up the log.
             */
            if (_process != null)
            {
                try
                {
                    if (!_process.HasExited)
                    {
                        _process.Kill();
                    }
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.IntAppKillFailed, ex);
                }

                try
                {
                    if (!_process.HasExited)
                    {
                        _process.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector.AddExceptionMessage(Language.IntAppDisposeFailed, ex);
                }
            }

            base.Close();
        }

        #endregion

        #region Private Methods

        private void ProcessExited(object sender, EventArgs e)
        {
            Event_Closed(this);
        }

        /// <summary>
        /// Polls Process.MainWindowHandle for up to <paramref name="timeoutMs"/> milliseconds.
        /// This is the original strategy — works for direct GUI apps (PuTTY, Notepad++, etc.).
        /// </summary>
        private static IntPtr PollMainWindowHandle(Process process, int timeoutMs)
        {
            IntPtr handle = IntPtr.Zero;
            int startTicks = Environment.TickCount;
            while (handle == IntPtr.Zero &&
                   Environment.TickCount < startTicks + timeoutMs)
            {
                try
                {
                    if (process.HasExited) break;
                    process.Refresh();
                    if (!string.Equals(process.MainWindowTitle, "Default IME", StringComparison.Ordinal))
                    {
                        handle = process.MainWindowHandle;
                    }
                }
                catch (InvalidOperationException)
                {
                    break; // Process exited
                }

                if (handle == IntPtr.Zero)
                    Thread.Sleep(50);
            }
            return handle;
        }

        /// <summary>
        /// Uses EnumWindows + GetWindowThreadProcessId to find a visible top-level window
        /// belonging to the given process ID. Catches windows that .NET's MainWindowHandle misses
        /// (e.g. conhost windows, multi-window apps).
        /// </summary>
        private static IntPtr FindWindowByProcessId(int processId, int timeoutMs)
        {
            IntPtr found = IntPtr.Zero;
            int startTicks = Environment.TickCount;
            while (found == IntPtr.Zero &&
                   Environment.TickCount < startTicks + timeoutMs)
            {
                NativeMethods.EnumWindows((hWnd, lParam) =>
                {
                    _ = NativeMethods.GetWindowThreadProcessId(hWnd, out uint windowPid);
                    if (windowPid == (uint)processId && NativeMethods.IsWindowVisible(hWnd))
                    {
                        found = hWnd;
                        return false; // Stop enumeration
                    }
                    return true;
                }, IntPtr.Zero);

                if (found == IntPtr.Zero)
                    Thread.Sleep(50);
            }
            return found;
        }

        /// <summary>
        /// Searches for visible windows belonging to child processes of the given parent PID.
        /// This handles launcher-style apps (git-bash.exe → mintty.exe, wt.exe → child, etc.)
        /// where the launched process spawns a child and may exit.
        /// </summary>
        private static IntPtr FindWindowInChildProcesses(int parentProcessId, int timeoutMs)
        {
            IntPtr found = IntPtr.Zero;
            int startTicks = Environment.TickCount;
            while (found == IntPtr.Zero &&
                   Environment.TickCount < startTicks + timeoutMs)
            {
                List<int> childPids = GetChildProcessIds(parentProcessId);
                foreach (int childPid in childPids)
                {
                    NativeMethods.EnumWindows((hWnd, lParam) =>
                    {
                        _ = NativeMethods.GetWindowThreadProcessId(hWnd, out uint windowPid);
                        if (windowPid == (uint)childPid && NativeMethods.IsWindowVisible(hWnd))
                        {
                            found = hWnd;
                            return false;
                        }
                        return true;
                    }, IntPtr.Zero);

                    if (found != IntPtr.Zero) break;
                }

                if (found == IntPtr.Zero)
                    Thread.Sleep(100);
            }
            return found;
        }

        /// <summary>
        /// Searches for visible windows belonging to descendants of the given root PID.
        /// Useful for launchers that create a multi-level process chain.
        /// </summary>
        private static IntPtr FindWindowInDescendantProcesses(int rootProcessId, int timeoutMs, int maxDepth)
        {
            IntPtr found = IntPtr.Zero;
            int startTicks = Environment.TickCount;
            while (found == IntPtr.Zero &&
                   Environment.TickCount < startTicks + timeoutMs)
            {
                List<int> descendantPids = GetDescendantProcessIds(rootProcessId, maxDepth);
                foreach (int descendantPid in descendantPids)
                {
                    NativeMethods.EnumWindows((hWnd, lParam) =>
                    {
                        _ = NativeMethods.GetWindowThreadProcessId(hWnd, out uint windowPid);
                        if (windowPid == (uint)descendantPid && NativeMethods.IsWindowVisible(hWnd))
                        {
                            found = hWnd;
                            return false;
                        }
                        return true;
                    }, IntPtr.Zero);

                    if (found != IntPtr.Zero) break;
                }

                if (found == IntPtr.Zero)
                    Thread.Sleep(100);
            }
            return found;
        }

        /// <summary>
        /// Gets child process IDs for a given parent process ID via WMI.
        /// </summary>
        private static List<int> GetChildProcessIds(int parentPid)
        {
            List<int> children = [];
            try
            {
                using var searcher = new ManagementObjectSearcher(
                    $"SELECT ProcessId FROM Win32_Process WHERE ParentProcessId = {parentPid}");
                foreach (ManagementObject obj in searcher.Get())
                {
                    children.Add(Convert.ToInt32(obj["ProcessId"], CultureInfo.InvariantCulture));
                }
            }
            catch
            {
                // WMI query can fail if access is denied or service unavailable — not critical
            }
            return children;
        }

        private static List<int> GetDescendantProcessIds(int rootProcessId, int maxDepth)
        {
            if (maxDepth <= 0)
                return [];

            List<int> descendants = [];
            HashSet<int> visited = [rootProcessId];
            List<int> currentLevel = [rootProcessId];

            for (int depth = 0; depth < maxDepth && currentLevel.Count > 0; depth++)
            {
                List<int> nextLevel = [];
                foreach (int pid in currentLevel)
                {
                    List<int> children = GetChildProcessIds(pid);
                    foreach (int childPid in children)
                    {
                        if (visited.Add(childPid))
                        {
                            descendants.Add(childPid);
                            nextLevel.Add(childPid);
                        }
                    }
                }

                currentLevel = nextLevel;
            }

            return descendants;
        }

        private static ExternalTool? CreateBuiltInShellPresetForIntegration(string extAppName)
        {
            string normalized = NormalizeShellToolName(extAppName);
            switch (normalized)
            {
                case "cmd":
                case "command prompt":
                    return CreateBuiltInShellTool("cmd.exe", "%ComSpec%");
                case "pwsh":
                    return CreateBuiltInShellTool("pwsh.exe", "pwsh.exe");
                case "powershell":
                case "windows powershell":
                    return CreateBuiltInShellTool("powershell.exe", "powershell.exe");
                case "wsl":
                case "bash":
                    return CreateBuiltInShellTool("wsl.exe", @"%windir%\system32\wsl.exe");
                case "ubuntu":
                case "debian":
                case "kali-linux":
                case "kali":
                    // WSL distribution launchers installed via Windows Store
                    return CreateBuiltInShellTool(normalized + ".exe", normalized + ".exe");
                default:
                    // Handle versioned Ubuntu launchers (e.g. ubuntu2004, ubuntu2204, ubuntu2404)
                    if (normalized.Length > 6 && normalized.StartsWith("ubuntu", StringComparison.Ordinal) && char.IsDigit(normalized[6]))
                        return CreateBuiltInShellTool(normalized + ".exe", normalized + ".exe");
                    return null;
            }
        }

        private static ExternalTool CreateBuiltInShellTool(string displayName, string fileName)
        {
            return new ExternalTool(displayName, fileName)
            {
                TryIntegrate = true,
                ShowOnToolbar = false
            };
        }

        private static bool IsCommonShellTool(ExternalTool externalTool, string parsedFileName)
        {
            return IsShellToolIdentifier(NormalizeShellToolName(externalTool.DisplayName))
                || IsShellToolIdentifier(NormalizeShellToolName(externalTool.FileName))
                || IsShellToolIdentifier(NormalizeShellToolName(parsedFileName));
        }

        private static bool IsShellToolIdentifier(string identifier)
        {
            return string.Equals(identifier, "cmd", StringComparison.Ordinal)
                || string.Equals(identifier, "pwsh", StringComparison.Ordinal)
                || string.Equals(identifier, "powershell", StringComparison.Ordinal)
                || string.Equals(identifier, "windows powershell", StringComparison.Ordinal)
                || string.Equals(identifier, "wsl", StringComparison.Ordinal)
                || string.Equals(identifier, "bash", StringComparison.Ordinal)
                || string.Equals(identifier, "ubuntu", StringComparison.Ordinal)
                || string.Equals(identifier, "debian", StringComparison.Ordinal)
                || string.Equals(identifier, "kali-linux", StringComparison.Ordinal)
                || string.Equals(identifier, "kali", StringComparison.Ordinal)
                // Versioned Ubuntu launchers (e.g. ubuntu2004, ubuntu2204, ubuntu2404)
                || (identifier.Length > 6 && identifier.StartsWith("ubuntu", StringComparison.Ordinal) && char.IsDigit(identifier[6]));
        }

        private static string NormalizeShellToolName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string normalized = value.Trim().Trim('"');
            if (normalized.Contains('\\') || normalized.Contains('/'))
            {
                normalized = Path.GetFileName(normalized);
            }

            normalized = normalized.ToLowerInvariant();
            if (normalized.EndsWith(".exe", StringComparison.Ordinal))
            {
                normalized = normalized.Substring(0, normalized.Length - 4);
            }

            return normalized;
        }

        #endregion

        #region Enumerations

        public enum Defaults
        {
            Port = 0
        }

        #endregion
    }
}