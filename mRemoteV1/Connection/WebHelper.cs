using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
    public class WebHelper
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public WebHelper(IConnectionInitiator connectionInitiator)
        {
            _connectionInitiator = connectionInitiator.ThrowIfNull(nameof(connectionInitiator));
        }

        public void GoToUrl(string url)
        {
            var connectionInfo = new ConnectionInfo();
            connectionInfo.CopyFrom(DefaultConnectionInfo.Instance);

            connectionInfo.Name = "";
            connectionInfo.Hostname = url;
            connectionInfo.Protocol = url.StartsWith("https:") ? ProtocolType.HTTPS : ProtocolType.HTTP;
            connectionInfo.SetDefaultPort();
            connectionInfo.IsQuickConnect = true;
            _connectionInitiator.OpenConnection(connectionInfo, ConnectionInfo.Force.DoNotJump);
        }
    }
}