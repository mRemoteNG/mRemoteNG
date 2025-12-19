using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
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
        private bool _tryIntegrate;
        private bool _showOnToolbar = true;
        private bool _runElevated;

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

        public bool RunElevated
        {
            get => _runElevated;
            set => SetField(ref _runElevated, value, nameof(RunElevated));
        }

        public ConnectionInfo ConnectionInfo { get; set; } = new ConnectionInfo(); // Initialize to avoid CS8618

        public Icon Icon => File.Exists(FileName) ? MiscTools.GetIconFromFile(FileName) ?? Properties.Resources.mRemoteNG_Icon : Properties.Resources.mRemoteNG_Icon;

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

            if (WaitForExit)
            {
                process.WaitForExit();
            }
        }

        private void SetProcessProperties(Process process, ConnectionInfo startConnectionInfo)
        {
            ExternalToolArgumentParser argParser = new(startConnectionInfo);
            string parsedFileName = argParser.ParseArguments(FileName);
            
            // Validate the executable path to prevent command injection
            PathValidator.ValidateExecutablePathOrThrow(parsedFileName, nameof(FileName));
            
            // When RunElevated is true, we must use UseShellExecute = true for the "runas" verb
            // When false, we use UseShellExecute = false for better security with ArgumentList
            process.StartInfo.UseShellExecute = RunElevated;
            process.StartInfo.FileName = parsedFileName;
            
            if (RunElevated)
            {
                // With UseShellExecute = true, we must use Arguments property, not ArgumentList
                // The argument parser already handles escaping properly
                process.StartInfo.Arguments = argParser.ParseArguments(Arguments);
                process.StartInfo.Verb = "runas";
            }
            else
            {
                // With UseShellExecute = false, use ArgumentList for better security
                // Parse arguments using CommandLineArguments for proper splitting
                // Note: argParser will still escape shell metacharacters by default.
                // If calling a program that doesn't expect escaped values, use %!VARIABLE% syntax
                // to disable escaping (e.g., %!PASSWORD% instead of %PASSWORD%).
                var cmdLineArgs = new Cmdline.CommandLineArguments { EscapeForShell = false };
                string parsedArguments = argParser.ParseArguments(Arguments);
                
                // Split arguments respecting quotes
                var argumentParts = SplitCommandLineArguments(parsedArguments);
                foreach (var arg in argumentParts)
                {
                    if (!string.IsNullOrWhiteSpace(arg))
                    {
                        process.StartInfo.ArgumentList.Add(arg);
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
                        if (arg.StartsWith("\"") && arg.EndsWith("\"") && arg.Length > 1)
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
                if (arg.StartsWith("\"") && arg.EndsWith("\"") && arg.Length > 1)
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