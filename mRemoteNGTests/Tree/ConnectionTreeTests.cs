using mRemoteNG.UI.Controls;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class ConnectionTreeTests
    {
        private ConnectionTree _connectionTree;

        [SetUp]
        public void Setup()
        {
            _connectionTree = new ConnectionTree();
        }

        [TearDown]
        public void Teardown()
        {
            _connectionTree.Dispose();
        }


        [Test]
        public void test()
        {
        }
    }
}