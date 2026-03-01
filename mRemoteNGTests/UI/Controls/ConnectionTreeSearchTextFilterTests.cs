using System;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionTreeSearchTextFilterTests
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
                Protocol = ProtocolType.RDP,
                EnvironmentTags = "production, core"
            };

            _sshConnection = new ConnectionInfo
            {
                Name = "Linux Box",
                Hostname = "ssh-host",
                Protocol = ProtocolType.SSH2,
                EnvironmentTags = "staging, web"
            };
        }

        [Test]
        public void Filter_WithEmptyText_ReturnsTrue()
        {
            _filter.FilterText = "";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_ByName_MatchesSubstring()
        {
            _filter.FilterText = "Windows";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_ByHostname_MatchesSubstring()
        {
            _filter.FilterText = "rdp";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_ByProtocolPrefix_MatchesProtocolName()
        {
            _filter.FilterText = "protocol:rdp";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_ByProtocolPrefix_MatchesSSH2()
        {
            _filter.FilterText = "protocol:ssh2";
            Assert.That(_filter.Filter(_rdpConnection), Is.False);
            Assert.That(_filter.Filter(_sshConnection), Is.True);
        }

        [Test]
        public void Filter_ByTagPrefix_MatchesSingleTag()
        {
            _filter.FilterText = "tag:production";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_ByTagPrefix_MatchesPartialTag()
        {
            _filter.FilterText = "tag:prod";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_ByTagsInNormalSearch_MatchesTags()
        {
            _filter.FilterText = "production";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_WithProtocolFilterSet_ExcludesNonMatchingProtocols()
        {
            _filter.FilterProtocol = ProtocolType.SSH2;
            _filter.FilterText = "";
            Assert.That(_filter.Filter(_rdpConnection), Is.False);
            Assert.That(_filter.Filter(_sshConnection), Is.True);
        }

        [Test]
        public void Filter_WithSpecialInclusionList_AlwaysReturnsTrueForIncludedItems()
        {
            _filter.SpecialInclusionList.Add(_rdpConnection);
            _filter.FilterText = "something-that-wont-match";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        // --- Issue #2178: searching a folder name should reveal its children ---

        [Test]
        public void Filter_FolderNameMatch_ShowsDirectChildConnections()
        {
            var folder = new ContainerInfo { Name = "Production" };
            folder.AddChild(_rdpConnection);

            _filter.FilterText = "Production";

            // The folder itself is visible
            Assert.That(_filter.Filter(folder), Is.True);
            // Its child connection is also visible (issue #2178)
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_FolderNameMatch_ShowsNestedChildConnections()
        {
            var root = new ContainerInfo { Name = "DataCenter" };
            var sub = new ContainerInfo { Name = "Servers" };
            root.AddChild(sub);
            sub.AddChild(_rdpConnection);

            _filter.FilterText = "DataCenter";

            Assert.That(_filter.Filter(root), Is.True);
            Assert.That(_filter.Filter(sub), Is.True);
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }

        [Test]
        public void Filter_FolderNameNoMatch_HidesChildrenThatDontMatch()
        {
            var folder = new ContainerInfo { Name = "OtherFolder" };
            folder.AddChild(_rdpConnection);

            _filter.FilterText = "xyz-no-match";

            Assert.That(_filter.Filter(folder), Is.False);
            Assert.That(_filter.Filter(_rdpConnection), Is.False);
        }

        // --- Issue #2293: folder-name ancestor check is name-only (not hostname/description/tags) ---

        [Test]
        public void Filter_FolderNameMatch_ShowsAllChildrenViaAncestorRule_Issue2293()
        {
            // Folder name matches; child connection name does NOT match by itself
            var folder = new ContainerInfo { Name = "WebServers" };
            var conn = new ConnectionInfo { Name = "Apache", Hostname = "web01" };
            folder.AddChild(conn);

            _filter.FilterText = "WebServers";

            Assert.That(_filter.Filter(folder), Is.True, "folder itself must be visible");
            Assert.That(_filter.Filter(conn), Is.True, "child must be visible because parent folder name matches");
        }

        [Test]
        public void Filter_FolderHostnameMatch_DoesNotRevealChildrenViaAncestorRule_Issue2293()
        {
            // Folder hostname matches the filter text, but folder NAME does NOT.
            // The ancestor rule (issue #2293) must check name only — a hostname match
            // on a folder must NOT cause its children to appear via the ancestor path.
            var folder = new ContainerInfo { Name = "Unrelated", Hostname = "webservers-host" };
            var conn = new ConnectionInfo { Name = "Apache", Hostname = "web01" };
            folder.AddChild(conn);

            _filter.FilterText = "webservers";

            // conn itself does not match ("Apache" / "web01" don't contain "webservers")
            Assert.That(_filter.Filter(conn), Is.False,
                "child must not appear — folder matched via hostname, not name");
        }

        // --- Issue #2180: multiple space-separated search terms (AND logic) ---

        [Test]
        public void Filter_MultipleSpaceTerms_MatchesWhenAllTermsMatch()
        {
            // _rdpConnection: Name="Windows Server", Hostname="rdp-host"
            _filter.FilterText = "windows rdp";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_MultipleSpaceTerms_NoMatchWhenOnlyOneTermMatches()
        {
            // "Windows" matches _rdpConnection name but "ssh" does not
            _filter.FilterText = "windows ssh";
            Assert.That(_filter.Filter(_rdpConnection), Is.False);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_MultipleSpaceTerms_SingleTermBehaviorUnchanged()
        {
            _filter.FilterText = "windows";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
            Assert.That(_filter.Filter(_sshConnection), Is.False);
        }

        [Test]
        public void Filter_MultipleSpaceTerms_ExtraSpacesIgnored()
        {
            _filter.FilterText = "  windows   rdp  ";
            Assert.That(_filter.Filter(_rdpConnection), Is.True);
        }
    }
}
