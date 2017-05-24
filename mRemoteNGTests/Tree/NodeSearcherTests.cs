using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class NodeSearcherTests
    {
        private NodeSearcher _nodeSearcher;
        private ContainerInfo _folder1;
        private ContainerInfo _folder2;
        private ConnectionInfo _con1;
        private ConnectionInfo _con2;
        private ConnectionInfo _con3;
        private ConnectionInfo _con4;
        private ConnectionInfo _con5;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var connectionTreeModel = SetupConnectionTreeModel();
            _nodeSearcher = new NodeSearcher(connectionTreeModel);
        }

        [Test]
        public void SearchByNameReturnsAllExpectedMatches()
        {
            var matches = _nodeSearcher.SearchByName("folder");
            Assert.That(matches.ToList(), Is.EquivalentTo(new[] {_folder1, _folder2}));
        }

        [Test]
        public void SearchByDescriptionReturnsAllExpectedMatches()
        {
            var matches = _nodeSearcher.SearchByName("description");
            Assert.That(matches.ToList(), Is.EquivalentTo(new[] { _folder1, _folder2, _con1, _con2, _con3, _con4, _con5 }));
        }

        [Test]
        public void SearchByDescription1ReturnsAllExpectedMatches()
        {
            var matches = _nodeSearcher.SearchByName("description1");
            Assert.That(matches.ToList(), Is.EquivalentTo(new[] { _folder1 }));
        }

        [Test]
        public void SearchByHostname1ReturnsAllExpectedMatches()
        {
            var matches = _nodeSearcher.SearchByName("hostname1");
            Assert.That(matches.ToList(), Is.EquivalentTo(new[] { _folder1 }));
        }

        [Test]
        public void NextMatchAdvancesTheIterator()
        {
            _nodeSearcher.SearchByName("folder");
            var match1 = _nodeSearcher.CurrentMatch;
            var match2 = _nodeSearcher.NextMatch();
            Assert.That(match1, Is.Not.EqualTo(match2));
        }

        [Test]
        public void PreviousMatchRollsBackTheIterator()
        {
            _nodeSearcher.SearchByName("con");
            var match1 = _nodeSearcher.CurrentMatch;
            _nodeSearcher.NextMatch();
            var match2 = _nodeSearcher.PreviousMatch();
            Assert.That(match1, Is.EqualTo(match2));
        }

        [Test]
        public void SearchingWithEmptyStringReturnsNoMatches()
        {
            var matches = _nodeSearcher.SearchByName("");
            Assert.That(matches.Count(), Is.EqualTo(0));
        }

        private ConnectionTreeModel SetupConnectionTreeModel()
        {
            /*
             * Tree:
             * Root
             * |--- folder1 (description1, hostname1)
             * |    |--- con1 (description2, hostname2)
             * |    L--- con2 (description3, hostname3)
             * |--- folder2 (description4, hostname4)
             * |    |--- con3 (description5, hostname5)
             * |    L--- con4 (description6, hostname6)
             * L--- con5 (description7, hostname7)
             * 
             */
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            _folder1 = new ContainerInfo { Name = "folder1", Description = "description1", Hostname = "hostname1" };
            _con1 = new ConnectionInfo { Name = "con1", Description="description2", Hostname="hostname2" };
            _con2 = new ConnectionInfo { Name = "con2", Description="description3", Hostname="hostname3" };
            _folder2 = new ContainerInfo { Name = "folder2", Description="description4", Hostname="hostname4" };
            _con3 = new ConnectionInfo { Name = "con3", Description="description5", Hostname="hostname5" };
            _con4 = new ConnectionInfo { Name = "con4", Description="description6", Hostname="hostname6" };
            _con5 = new ConnectionInfo { Name = "con5", Description="description7", Hostname="hostname7" };

            connectionTreeModel.AddRootNode(root);
            root.AddChildRange(new [] { _folder1, _folder2, _con5 });
            _folder1.AddChildRange(new [] { _con1, _con2 });
            _folder2.AddChildRange(new[] { _con3, _con4 });

            return connectionTreeModel;
        }
    }
}