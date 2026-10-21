using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Tree
{
    public class DuplicateHostnameFinderTests
    {
        private ConnectionTreeModel _model;
        private ContainerInfo _folder;
        private ConnectionInfo _existing;

        [SetUp]
        public void Setup()
        {
            // root
            // |- Datacenter
            //		|- sql01 (SQL01.corp.local)
            _model = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            _folder = new ContainerInfo { Name = "Datacenter" };
            _existing = new ConnectionInfo { Name = "sql01", Hostname = "SQL01.corp.local" };
            root.AddChild(_folder);
            _folder.AddChild(_existing);
            _model.AddRootNode(root);
        }

        [Test]
        public void FindsConnectionWithSameHostnameIgnoringCaseAndWhitespace()
        {
            var newConnection = new ConnectionInfo { Name = "copy", Hostname = " sql01.CORP.local " };
            _folder.AddChild(newConnection);

            var duplicates = DuplicateHostnameFinder.FindDuplicates(_model.GetRecursiveChildList(), newConnection);

            Assert.That(duplicates, Is.EquivalentTo(new[] { _existing }));
        }

        [Test]
        public void DoesNotReportTheConnectionItself()
        {
            var duplicates = DuplicateHostnameFinder.FindDuplicates(_model.GetRecursiveChildList(), _existing);

            Assert.That(duplicates, Is.Empty);
        }

        [Test]
        public void EmptyHostnameIsNeverADuplicate()
        {
            var first = new ConnectionInfo { Name = "a", Hostname = "" };
            var second = new ConnectionInfo { Name = "b", Hostname = "" };
            _folder.AddChildRange(new[] { first, second });

            var duplicates = DuplicateHostnameFinder.FindDuplicates(_model.GetRecursiveChildList(), second);

            Assert.That(duplicates, Is.Empty);
        }

        [Test]
        public void TreePathOmitsTheRootNode()
        {
            Assert.That(DuplicateHostnameFinder.GetTreePath(_existing), Is.EqualTo("Datacenter / sql01"));
        }
    }
}
