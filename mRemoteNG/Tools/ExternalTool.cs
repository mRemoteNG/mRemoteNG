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
        private string _displayName;
        private string _fileName;
        private bool _waitForExit;
        private string _arguments;
        private string _workingDir;
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

        public ConnectionInfo ConnectionInfo { get; set; }

        public Icon Icon => File.Exists(FileName) ? MiscTools.GetIconFromFile(FileName) : Properties.Resources.mRemoteNG_Icon;

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

        public void Start(ConnectionInfo startConnectionInfo = null)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                                                        "ExternalApp.Start() failed: FileName cannot be blank.");
                    return;
                }

                ConnectionInfo = startConnectionInfo;
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
            var process = new Process();
            SetProcessProperties(process, ConnectionInfo);
            process.Start();

            if (WaitForExit)
            {
                process.WaitForExit();
            }
        }

        private void SetProcessProperties(Process process, ConnectionInfo startConnectionInfo)
        {
            var argParser = new ExternalToolArgumentParser(startConnectionInfo);
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = argParser.ParseArguments(FileName);
            process.StartInfo.Arguments = argParser.ParseArguments(Arguments);
            if (WorkingDir != "") process.StartInfo.WorkingDirectory = argParser.ParseArguments(WorkingDir);
            if (RunElevated) process.StartInfo.Verb = "runas";
        }

        private void StartIntegrated()
        {
            try
            {
                var newConnectionInfo = BuildConnectionInfoForIntegratedApp();
                Runtime.ConnectionInitiator.OpenConnection(newConnectionInfo);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("ExternalApp.StartIntegrated() failed.", ex);
            }
        }

        private ConnectionInfo BuildConnectionInfoForIntegratedApp()
        {
            var newConnectionInfo = GetAppropriateInstanceOfConnectionInfo();
            SetConnectionInfoFields(newConnectionInfo);
            return newConnectionInfo;
        }

        private ConnectionInfo GetAppropriateInstanceOfConnectionInfo()
        {
            var newConnectionInfo = ConnectionInfo == null ? new ConnectionInfo() : ConnectionInfo.Clone();
            return newConnectionInfo;
        }

        private void SetConnectionInfoFields(ConnectionInfo newConnectionInfo)
        {
            newConnectionInfo.Protocol = ProtocolType.IntApp;
            newConnectionInfo.ExtApp = DisplayName;
            newConnectionInfo.Name = DisplayName;
            newConnectionInfo.Panel = Language._Tools;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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