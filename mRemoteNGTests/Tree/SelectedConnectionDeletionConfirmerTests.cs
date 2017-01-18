using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class SelectedConnectionDeletionConfirmerTests
    {
        private SelectedConnectionDeletionConfirmer _deletionConfirmer;
        private IConnectionTree _connectionTree;

        [SetUp]
        public void Setup()
        {
            _connectionTree = Substitute.For<IConnectionTree>();
            _connectionTree.SelectedNode.Returns(new ConnectionInfo());
        }

        [Test]
        public void ClickingYesReturnsTrue()
        {
            _deletionConfirmer = new SelectedConnectionDeletionConfirmer(_connectionTree, MockClickYes);
            Assert.That(_deletionConfirmer.Confirm(), Is.True);
        }

        [Test]
        public void ClickingNoReturnsFalse()
        {
            _deletionConfirmer = new SelectedConnectionDeletionConfirmer(_connectionTree, MockClickNo);
            Assert.That(_deletionConfirmer.Confirm(), Is.False);
        }

        private DialogResult MockClickYes(string promptMessage, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return DialogResult.Yes;
        }

        private DialogResult MockClickNo(string promptMessage, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return DialogResult.No;
        }
    }
}