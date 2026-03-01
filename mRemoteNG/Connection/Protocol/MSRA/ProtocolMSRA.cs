using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.MSRA
{
    [SupportedOSPlatform("windows")]
    public class ProtocolMSRA : ProtocolBase
    {
        #region Private Fields

        private IntPtr _handle;
        private readonly ConnectionInfo _connectionInfo;
        private Process? _process;

        #endregion

        #region Constructor

        public ProtocolMSRA(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;
        }

        #endregion

        #region Public Methods

        public override bool Initialize()
        {
            return base.Initialize();
        }

        public override bool Connect()
        {
            try
            {
                Runtime.MessageCollector?.AddMessage(MessageClass.InformationMsg,
                    "Attempting to start MSRA connection.", true);

                // Validate connection info
                if (string.IsNullOrEmpty(_connectionInfo.Hostname))
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                        "Hostname is required for MSRA.", true);
                    return false;
                }

                // Start MSRA connection
                if (!StartMsraConnection())
                {
                    return false;
                }

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
                Runtime.MessageCollector?.AddExceptionMessage(Language.IntAppResizeFailed, ex);
            }
        }

        public override void Close()
        {
            try
            {
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
                        Runtime.MessageCollector?.AddExceptionMessage(Language.IntAppKillFailed, ex);
                    }
                    finally
                    {
                        _process?.Dispose();
                        _process = null;
                    }
                }

                if (_handle != IntPtr.Zero)
                {
                    try
                    {
                        NativeMethods.SendMessage(_handle, 0x0010, IntPtr.Zero, IntPtr.Zero); // WM_CLOSE
                    }
                    catch
                    {
                        // Ignore errors when closing by handle
                    }
                    _handle = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("Error closing MSRA connection.", ex);
            }

            base.Close();
        }

        #endregion

        #region Private Methods

        private bool StartMsraConnection()
        {
            try
            {
                string hostname = _connectionInfo.Hostname.Trim();
                
                // Validate hostname
                if (!IsValidHostname(hostname))
                {
                     Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg,
                        "Invalid hostname format. Only alphanumeric characters, dashes, dots and underscores are allowed.", true);
                    return false;
                }

                string systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                string msraPath = Path.Combine(systemDirectory, "msra.exe");
                if (string.IsNullOrWhiteSpace(systemDirectory) || !File.Exists(msraPath))
                {
                    Runtime.MessageCollector?.AddMessage(MessageClass.ErrorMsg, "MSRA executable not found in the system directory.", true);
                    return false;
                }
                
                string arguments = $"/offerra \"{hostname}\"";

                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = msraPath,
                        Arguments = arguments,
                        UseShellExecute = false
                    },
                    EnableRaisingEvents = true
                };

                _process.Exited += ProcessExited;
                _process.Start();

                try
                {
                    _process.WaitForInputIdle(5000); // Wait up to 5 seconds for input idle
                }
                catch
                {
                    // Ignore if WaitForInputIdle fails (e.g. process exited or not GUI)
                }

                // Wait for the MSRA window to appear
                if (!WaitForMsraWindow())
                {
                     // MSRA might take some time or require user interaction if permissions are an issue
                     // But we report success as the process started
                }

                base.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("Failed to start MSRA connection.", ex);
                return false;
            }
        }

        private static bool IsValidHostname(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname)) return false;
            foreach (char c in hostname)
            {
                if (!char.IsLetterOrDigit(c) && c != '-' && c != '.' && c != '_')
                    return false;
            }
            return true;
        }

        private bool WaitForMsraWindow()
        {
            // Wait up to 10 seconds for MSRA window to appear
            int maxWaitTime = 10000; // 10 seconds
            int waitInterval = 100; // 100 ms
            int elapsedTime = 0;

            while (elapsedTime < maxWaitTime)
            {
                // Find MSRA process by name
                Process[] msraProcesses = Process.GetProcessesByName("msra");
                try
                {
                    foreach (Process msraProcess in msraProcesses)
                    {
                        try
                        {
                            msraProcess.Refresh();

                            // Try to get the main window handle
                            if (msraProcess.MainWindowHandle != IntPtr.Zero)
                            {
                                _handle = msraProcess.MainWindowHandle;
                                
                                // Update _process if we found the window in a different process instance
                                if (_process != null && _process.Id != msraProcess.Id)
                                {
                                     try { _process.Exited -= ProcessExited; } catch {}
                                     try { _process.Dispose(); } catch {} // Dispose old process
                                     
                                     _process = msraProcess;
                                     _process.EnableRaisingEvents = true;
                                     _process.Exited += ProcessExited;
                                }

                                // Try to integrate the window
                                if (InterfaceControl != null)
                                {
                                    NativeMethods.SetParent(_handle, InterfaceControl.Handle);
                                    Resize(this, EventArgs.Empty);
                                }

                                return true;
                            }
                        }
                        catch
                        {
                            // Ignore errors for individual processes
                        }
                    }
                }
                finally
                {
                    // Cleanup checks if we created new Process objects from GetProcessesByName
                    // But we don't want to dispose _process if it's one of them.
                    foreach (Process p in msraProcesses)
                    {
                        if (_process == null || p.Id != _process.Id)
                        {
                            p.Dispose();
                        }
                    }
                }

                Thread.Sleep(waitInterval);
                elapsedTime += waitInterval;
            }

            return false;
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            Event_Closed(this);
        }

        #endregion
    }
}
