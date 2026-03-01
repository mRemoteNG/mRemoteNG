using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionTreeSqlReadOnlyTests
    {
        private bool _originalSqlReadOnly;

        [SetUp]
        public void SetUp()
        {
            _originalSqlReadOnly = OptionsDBsPage.Default.SQLReadOnly;
        }

        [TearDown]
        public void TearDown()
        {
            OptionsDBsPage.Default.SQLReadOnly = _originalSqlReadOnly;
        }

        private static void RunWithMessagePump(Action<ConnectionTree> testAction)
        {
            Exception caught = null;
            var thread = new Thread(() =>
            {
                var form = new Form
                {
                    Width = 400, Height = 300,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    Location = new System.Drawing.Point(-10000, -10000)
                };
                form.Load += (s, e) =>
                {
                    try
                    {
                        var tree = new ConnectionTree { UseFiltering = true, Dock = DockStyle.Fill };
                        form.Controls.Add(tree);
                        Application.DoEvents();
                        testAction(tree);
                    }
                    catch (Exception ex)
                    {
                        caught = ex;
                    }
                    finally
                    {
                        form.Close();
                    }
                };
                Application.Run(form);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            if (!thread.Join(TimeSpan.FromSeconds(30)))
            {
                thread.Interrupt();
                Assert.Fail("Test timed out after 30 seconds (message pump deadlock)");
            }
            if (caught != null)
                throw caught;
        }

        [Test]
        public void AddConnection_DoesNotAdd_WhenSqlReadOnlyIsTrue() => RunWithMessagePump(tree =>
        {
            OptionsDBsPage.Default.SQLReadOnly = true;

            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.ExpandAll();
            Application.DoEvents();

            tree.SelectedObject = root;
            tree.AddConnection();

            Assert.That(root.Children, Is.Empty, "AddConnection should not add a node when SQLReadOnly is true");
        });

        [Test]
        public void DeleteSelectedNode_DoesNotDelete_WhenSqlReadOnlyIsTrue() => RunWithMessagePump(tree =>
        {
            OptionsDBsPage.Default.SQLReadOnly = true;

            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            var con1 = new ConnectionInfo { Name = "con1" };
            root.AddChild(con1);
            connectionTreeModel.AddRootNode(root);

            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.ExpandAll();
            Application.DoEvents();

            tree.SelectedObject = con1;
            tree.DeleteSelectedNode();

            Assert.That(root.Children, Has.Count.EqualTo(1), "DeleteSelectedNode should not delete a node when SQLReadOnly is true");
        });

        [Test]
        public void DuplicateSelectedNode_DoesNotDuplicate_WhenSqlReadOnlyIsTrue() => RunWithMessagePump(tree =>
        {
            OptionsDBsPage.Default.SQLReadOnly = true;

            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            var con1 = new ConnectionInfo { Name = "con1" };
            root.AddChild(con1);
            connectionTreeModel.AddRootNode(root);

            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.ExpandAll();
            Application.DoEvents();

            tree.SelectedObject = con1;
            tree.DuplicateSelectedNode();

            Assert.That(root.Children, Has.Count.EqualTo(1), "DuplicateSelectedNode should not duplicate a node when SQLReadOnly is true");
        });

        [Test]
        public void SortRecursive_DoesNotSort_WhenSqlReadOnlyIsTrue() => RunWithMessagePump(tree =>
        {
            OptionsDBsPage.Default.SQLReadOnly = true;

            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            var conA = new ConnectionInfo { Name = "A" };
            var conB = new ConnectionInfo { Name = "B" };
            // Add in reverse order
            root.AddChild(conB);
            root.AddChild(conA);
            connectionTreeModel.AddRootNode(root);

            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.ExpandAll();
            Application.DoEvents();

            tree.SortRecursive(root, System.ComponentModel.ListSortDirection.Ascending);

            Assert.That(root.Children[0].Name, Is.EqualTo("B"), "SortRecursive should not sort when SQLReadOnly is true (first item should still be B)");
            Assert.That(root.Children[1].Name, Is.EqualTo("A"), "SortRecursive should not sort when SQLReadOnly is true (second item should still be A)");
        });
    }
}
