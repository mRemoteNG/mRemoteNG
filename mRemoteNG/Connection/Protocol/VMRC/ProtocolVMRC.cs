using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.VMRC
{
    [SupportedOSPlatform("windows")]
    public class ProtocolVMRC : ProtocolBase
    {
        private readonly ConnectionInfo _connectionInfo;
        private IntPtr _handle;
        private Process? _process;

        private static readonly string[] DefaultVmwareViewExecutables =
        [
            @"C:\Program Files (x86)\VMware\VMware Horizon View Client\vmware-view.exe",
            @"C:\Program Files\VMware\VMware Horizon View Client\vmware-view.exe"
        ];

        public ProtocolVMRC(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        public override bool Connect()
        {
            try
            {
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                    "Attempting to start VMware Horizon View Client.", true);

                string serverUrl = _connectionInfo.Hostname?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(serverUrl))
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                        "Server URL is required for VMRC protocol.");
                    return false;
                }

                string? vmwareViewPath = FindVmwareViewExecutable();
                if (string.IsNullOrEmpty(vmwareViewPath))
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                        "VMware Horizon View Client executable (vmware-view.exe) was not found.");
                    return false;
                }

                PathValidator.ValidateExecutablePathOrThrow(vmwareViewPath, nameof(vmwareViewPath));

                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = vmwareViewPath,
                        UseShellExecute = false
                    },
                    EnableRaisingEvents = true
                };

                foreach (string argument in BuildArguments(serverUrl,
                             _connectionInfo.VmId,
                             _connectionInfo.Username,
                             _connectionInfo.Domain,
                             _connectionInfo.Password))
                {
                    _process.StartInfo.ArgumentList.Add(argument);
                }

                _process.Exited += ProcessExited;
                _process.Start();

                try
                {
                    _process.WaitForInputIdle(Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000);
                }
                catch (InvalidOperationException)
                {
                    // Some GUI apps may not expose an input loop immediately.
                }

                int timeoutMs = Properties.OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000;
                int processId = _process.Id;

                _handle = PollMainWindowHandle(_process, timeoutMs);
                if (_handle == IntPtr.Zero)
                {
                    _handle = FindWindowByProcessId(processId, timeoutMs);
                }

                if (_handle == IntPtr.Zero)
                {
                    _handle = FindWindowInChildProcesses(processId, timeoutMs);
                }

                if (_handle == IntPtr.Zero)
                {
                    _handle = FindWindowInDescendantProcesses(processId, timeoutMs, 4);
                }

                if (_handle == IntPtr.Zero)
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.WarningMsg,
                        "VMRC: Could not find the Horizon View client window. The connection will close.");
                    return false;
                }

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
                    }
                    catch (Exception ex)
                    {
                        Runtime.MessageCollector?.AddExceptionMessage("VMRC: Failed to attach process to window owner.", ex);
                    }
                }

                NativeMethods.SetParent(_handle, InterfaceControl.Handle);
                NativeMethods.SetFocus(_handle);

                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg, Language.IntAppStuff, true);
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     string.Format(CultureInfo.InvariantCulture, Language.IntAppHandle, _handle), true);
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                                                     string.Format(CultureInfo.InvariantCulture, Language.IntAppTitle, _process.MainWindowTitle), true);

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
                if (_handle != IntPtr.Zero)
                {
                    NativeMethods.SetForegroundWindow(_handle);
                    NativeMethods.SetFocus(_handle);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage(Language.IntAppFocusFailed, ex);
            }
        }

        protected override void Resize(object sender, EventArgs e)
        {
            try
            {
                if (_handle == IntPtr.Zero || InterfaceControl.Size == Size.Empty)
                    return;

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
                Runtime.MessageCollector?.AddExceptionMessage(Language.IntAppResizeFailed, ex);
            }
        }

        public override void Close()
        {
            if (_process != null)
            {
                try
                {
                    if (!_process.HasExited)
                        _process.Kill();
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionMessage(Language.IntAppKillFailed, ex);
                }

                try
                {
                    _process.Dispose();
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionMessage(Language.IntAppDisposeFailed, ex);
                }
            }

            base.Close();
        }

        private static List<string> BuildArguments(
            string serverUrl,
            string? desktopName,
            string? userName,
            string? domainName,
            string? password)
        {
            List<string> arguments = [];
            arguments.Add("-serverURL");
            arguments.Add(serverUrl);

            AddArgument(arguments, "-desktopName", desktopName);
            AddArgument(arguments, "-userName", userName);
            AddArgument(arguments, "-domainName", domainName);
            AddArgument(arguments, "-password", password);

            return arguments;
        }

        private static void AddArgument(List<string> arguments, string name, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            arguments.Add(name);
            arguments.Add(value.Trim());
        }

        private static string? FindVmwareViewExecutable()
        {
            foreach (string path in DefaultVmwareViewExecutables)
            {
                if (File.Exists(path))
                    return path;
            }

            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (pathVariable != null)
            {
                foreach (string path in pathVariable.Split(Path.PathSeparator))
                {
                    string trimmedPath = path.Trim();
                    if (trimmedPath.Length == 0)
                        continue;

                    string filePath = Path.Combine(trimmedPath, "vmware-view.exe");
                    if (File.Exists(filePath))
                        return filePath;
                }
            }

            return null;
        }

        private static IntPtr PollMainWindowHandle(Process process, int timeoutMs)
        {
            IntPtr handle = IntPtr.Zero;
            int startTicks = Environment.TickCount;
            while (handle == IntPtr.Zero &&
                   Environment.TickCount < startTicks + timeoutMs)
            {
                try
                {
                    if (process.HasExited)
                        break;

                    process.Refresh();
                    if (!string.Equals(process.MainWindowTitle, "Default IME", StringComparison.Ordinal))
                    {
                        handle = process.MainWindowHandle;
                    }
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                if (handle == IntPtr.Zero)
                    Thread.Sleep(50);
            }
            return handle;
        }

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
                        return false;
                    }
                    return true;
                }, IntPtr.Zero);

                if (found == IntPtr.Zero)
                    Thread.Sleep(50);
            }
            return found;
        }

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

                    if (found != IntPtr.Zero)
                        break;
                }

                if (found == IntPtr.Zero)
                    Thread.Sleep(100);
            }
            return found;
        }

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

                    if (found != IntPtr.Zero)
                        break;
                }

                if (found == IntPtr.Zero)
                    Thread.Sleep(100);
            }
            return found;
        }

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
                // Ignore WMI errors.
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

        private void ProcessExited(object sender, EventArgs e)
        {
            Event_Closed(this);
        }

        public enum Defaults
        {
            Port = 0
        }
    }
}
