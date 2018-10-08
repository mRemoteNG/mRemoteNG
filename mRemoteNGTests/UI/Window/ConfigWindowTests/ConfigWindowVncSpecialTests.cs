using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.VNC;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Window.ConfigWindowTests
{
    public class ConfigWindowVncSpecialTests : ConfigWindowSpecialTestsBase
    {
        protected override ProtocolType Protocol => ProtocolType.VNC;

        [Test]
        public void UserDomainPropertiesShown_WhenAuthModeIsWindows()
        {
            ConnectionInfo.VNCAuthMode = ProtocolVNC.AuthMode.AuthWin;
            ExpectedPropertyList.AddRange(new []
            {
                nameof(ConnectionInfo.Username),
                nameof(ConnectionInfo.Domain),
            });
        }

        [TestCase(ProtocolVNC.ProxyType.ProxyHTTP)]
        [TestCase(ProtocolVNC.ProxyType.ProxySocks5)]
        [TestCase(ProtocolVNC.ProxyType.ProxyUltra)]
        public void ProxyPropertiesShown_WhenProxyModeIsNotNone(ProtocolVNC.ProxyType proxyType)
        {
            ConnectionInfo.VNCProxyType = proxyType;
            ExpectedPropertyList.AddRange(new[]
            {
                nameof(ConnectionInfo.VNCProxyIP),
                nameof(ConnectionInfo.VNCProxyPort),
                nameof(ConnectionInfo.VNCProxyUsername),
                nameof(ConnectionInfo.VNCProxyPassword),
            });
        }
    }
}
