using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Tree
{
    [TestFixture]
    public class NodeSearcherPriorityTests
    {
        private NodeSearcher _nodeSearcher;
        private ConnectionInfo _partialMatch;
        private ConnectionInfo _exactMatch;

        [SetUp]
        public void Setup()
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            
            // "ApplePie" comes before "Apple" alphabetically and in insertion order
            _partialMatch = new ConnectionInfo { Name = "ApplePie", Description = "Partial Match", Hostname = "host1" };
            _exactMatch = new ConnectionInfo { Name = "Apple", Description = "Exact Match", Hostname = "host2" };

            // Turn off inheritance to avoid side effects (mimicking existing tests)
            _partialMatch.Inheritance.TurnOffInheritanceCompletely();
            _exactMatch.Inheritance.TurnOffInheritanceCompletely();

            connectionTreeModel.AddRootNode(root);
            root.AddChild(_partialMatch);
            root.AddChild(_exactMatch);

            _nodeSearcher = new NodeSearcher(connectionTreeModel);
        }

        [Test]
        public void SearchByNamePrioritizesExactMatch()
        {
            var matches = _nodeSearcher.SearchByName("Apple");
            
            // The first match should be the exact match "Apple", not "ApplePie"
            Assert.That(_nodeSearcher.CurrentMatch, Is.EqualTo(_exactMatch), "Expected 'Apple' to be the first match, but got " + _nodeSearcher.CurrentMatch?.Name);
            
            // Also verify the list order
            Assert.That(matches.First(), Is.EqualTo(_exactMatch));
        }
        
        [Test]
        public void SearchByNamePrioritizesExactMatchByHostname()
        {
             var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            
            // "host" is partial for "hostname", "host" is exact for "host"
            // Let's try searching for "server"
            // "myserver1" (partial) vs "server" (exact)
            
            var partial = new ConnectionInfo { Name = "C1", Hostname = "myserver1" };
            var exact = new ConnectionInfo { Name = "C2", Hostname = "server" };
            
            partial.Inheritance.TurnOffInheritanceCompletely();
            exact.Inheritance.TurnOffInheritanceCompletely();

            connectionTreeModel.AddRootNode(root);
            root.AddChild(partial);
            root.AddChild(exact);
            
            var searcher = new NodeSearcher(connectionTreeModel);
            var matches = searcher.SearchByName("server");

            Assert.That(searcher.CurrentMatch, Is.EqualTo(exact), "Expected exact hostname match to be prioritized");
        }
    }
}
