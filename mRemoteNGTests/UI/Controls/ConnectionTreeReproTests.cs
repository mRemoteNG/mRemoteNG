using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;
using System.Collections;
using System.Linq;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionTreeReproTests
    {
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
                        var tree = new ConnectionTree { Dock = DockStyle.Fill };
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
        public void AddingRootNodeToModelUpdatesTree() => RunWithMessagePump(tree =>
        {
            var model = new ConnectionTreeModel();
            var initialRoot = new RootNodeInfo(RootNodeType.Connection) { Name = "Initial Root" };
            model.AddRootNode(initialRoot);

            tree.ConnectionTreeModel = model;
            Application.DoEvents();

            // Verify initial state
            Assert.That(tree.Objects, Does.Contain(initialRoot));

            // Add new root node
            var newRoot = new RootNodeInfo(RootNodeType.Connection) { Name = "New Root" };
            model.AddRootNode(newRoot);
            Application.DoEvents();

            // Verify new root node is added to the tree
            // We check Roots because these are top level objects
            Assert.That(tree.Roots, Does.Contain(newRoot));
        });

        [Test]
        public void RemovingRootNodeFromModelUpdatesTree() => RunWithMessagePump(tree =>
        {
            var model = new ConnectionTreeModel();
            var root1 = new RootNodeInfo(RootNodeType.Connection) { Name = "Root 1" };
            var root2 = new RootNodeInfo(RootNodeType.Connection) { Name = "Root 2" };
            model.AddRootNode(root1);
            model.AddRootNode(root2);

            tree.ConnectionTreeModel = model;
            Application.DoEvents();

            // Verify initial state
            Assert.That(tree.Roots, Does.Contain(root1));
            Assert.That(tree.Roots, Does.Contain(root2));

            // Remove root node
            model.RemoveRootNode(root1);
            Application.DoEvents();

            // Verify root node is removed from the tree
            Assert.That(tree.Roots, Does.Not.Contain(root1));
            Assert.That(tree.Roots, Does.Contain(root2));
        });
    }
}
