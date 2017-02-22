using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;

namespace mRemoteNG.Tools
{
	public class ExternalTool
	{
        private readonly IConnectionInitiator _connectionInitiator = new ConnectionInitiator();
        #region Public Properties
		public string DisplayName { get; set; }
		public string FileName { get; set; }
		public bool WaitForExit { get; set; }
		public string Arguments { get; set; }
		public bool TryIntegrate { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }
		
        public Icon Icon => File.Exists(FileName) ? MiscTools.GetIconFromFile(FileName) : Resources.mRemote_Icon;

	    public Image Image => Icon?.ToBitmap() ?? Resources.mRemote_Icon.ToBitmap();

	    #endregion
		
		public ExternalTool(string displayName = "", string fileName = "", string arguments = "")
		{
			DisplayName = displayName;
			FileName = fileName;
			Arguments = arguments;
		}

        public void Start(ConnectionInfo startConnectionInfo = null)
		{
			try
			{
			    if (string.IsNullOrEmpty(FileName))
			    {
			        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "ExternalApp.Start() failed: FileName cannot be blank.");
			        return;
			    }
				
				ConnectionInfo = startConnectionInfo;
				
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
        }

        private void StartIntegrated()
		{
			try
			{
                var newConnectionInfo = BuildConnectionInfoForIntegratedApp();
                _connectionInitiator.OpenConnection(newConnectionInfo);
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
            newConnectionInfo.Panel = Language.strMenuExternalTools;
        }
	}
}