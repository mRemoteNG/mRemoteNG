using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Connection
{
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
            connectionInfo.IsQuickConnect = true;
            var connectionInitiator = new ConnectionInitiator();
            connectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
        }
    }
}