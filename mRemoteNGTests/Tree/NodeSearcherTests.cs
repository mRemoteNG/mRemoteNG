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
             * |--- folder1
             * |    |--- con1
             * |    L--- con2
             * |--- folder2
             * |    |--- con3
             * |    L--- con4
             * L--- con5
             * 
             */
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            _folder1 = new ContainerInfo { Name = "folder1"};
            _con1 = new ConnectionInfo { Name = "con1"};
            _con2 = new ConnectionInfo { Name = "con2"};
            _folder2 = new ContainerInfo { Name = "folder2" };
            _con3 = new ConnectionInfo { Name = "con3" };
            _con4 = new ConnectionInfo { Name = "con4" };
            _con5 = new ConnectionInfo { Name = "con5" };

            connectionTreeModel.AddRootNode(root);
            root.AddChildRange(new [] { _folder1, _folder2, _con5 });
            _folder1.AddChildRange(new [] { _con1, _con2 });
            _folder2.AddChildRange(new[] { _con3, _con4 });

            return connectionTreeModel;
        }
    }
}