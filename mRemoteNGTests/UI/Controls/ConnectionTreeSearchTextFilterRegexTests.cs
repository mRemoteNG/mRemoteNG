using System;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionTreeSearchTextFilterRegexTests
    {
        private ConnectionTreeSearchTextFilter _filter;
        private ConnectionInfo _rdpConnection;
        private ConnectionInfo _sshConnection;

        [SetUp]
        public void Setup()
        {
            _filter = new ConnectionTreeSearchTextFilter();
            
            _rdpConnection = new ConnectionInfo
            {
                Name = "Windows Server",
                Hostname = "rdp-host",
                Description = "Prod RDP Server",
                Protocol = ProtocolType.RDP,
                EnvironmentTags = "production, core"
            };

            _sshConnection = new ConnectionInfo
            {
                Name = "Linux Box",
                Hostname = "ssh-host",
                Description = "Dev SSH Server",
                Protocol = ProtocolType.SSH2,
                EnvironmentTags = "staging, web"
            };
        }

        [Test]
        public void Filter_ByRegex_MatchesName()
        {
            _filter.FilterText = "regex:Win.*Server";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_ByRegex_MatchesHostname()
        {
            _filter.FilterText = "regex:rdp-.*";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_ByRegex_MatchesDescription()
        {
            _filter.FilterText = "regex:Prod.*Server";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }
        
        [Test]
        public void Filter_ByRegex_MatchesTags()
        {
            _filter.FilterText = "regex:prod.*";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_ByRegex_CaseInsensitiveByDefault()
        {
            _filter.FilterText = "regex:windows";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_ByRegex_InvalidRegex_ReturnsFalseSafely()
        {
            _filter.FilterText = "regex:["; // Invalid regex
            Assert.DoesNotThrow(() => _filter.Filter(_rdpConnection));
            Assert.That(_filter.Filter(_rdpConnection), Is.False);
        }
    }
}
