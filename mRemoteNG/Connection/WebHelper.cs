using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class WebHelper
    {
        public static void GoToUrl(string url)
        {
            var connectionInfo = new ConnectionInfo();
            connectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

            connectionInfo.Name = "";
            connectionInfo.Hostname = url;
            connectionInfo.Protocol = url.StartsWith("https:") ? ProtocolType.HTTPS : ProtocolType.HTTP;
            connectionInfo.SetDefaultPort();
            if (string.IsNullOrEmpty(connectionInfo.Panel))
                connectionInfo.Panel = Language.General;
            connectionInfo.IsQuickConnect = true;
            Runtime.ConnectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
        }
    }
}