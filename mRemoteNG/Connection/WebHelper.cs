using System;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Resources.Language;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public static class WebHelper
    {
        public static void GoToUrl(string url)
        {
            ConnectionInfo connectionInfo = new();
            connectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

            connectionInfo.Name = "";
            connectionInfo.Hostname = url;
            connectionInfo.Protocol = url.StartsWith("https:", StringComparison.Ordinal) ? ProtocolType.HTTPS : ProtocolType.HTTP;
            connectionInfo.SetDefaultPort();
            if (string.IsNullOrEmpty(connectionInfo.Panel))
            {
                // Use the currently active panel instead of hardcoding "General" (#1682)
                if (FrmMain.IsCreated && FrmMain.Default.pnlDock.ActiveDocument is ConnectionWindow activeCw)
                    connectionInfo.Panel = activeCw.TabText;
                else
                    connectionInfo.Panel = "General";
            }
            connectionInfo.IsQuickConnect = true;
            Runtime.ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
        }
    }
}