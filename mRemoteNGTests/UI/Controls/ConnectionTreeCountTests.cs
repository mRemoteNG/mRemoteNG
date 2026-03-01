using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionTreeCountTests
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
                        var tree = new ConnectionTree { UseFiltering = false, Dock = DockStyle.Fill };
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
        public void AddingConnectionUpdatesAncestorCounts() => RunWithMessagePump(tree =>
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection) { Name = "Root" };
            var folder1 = new ContainerInfo { Name = "Folder1" };
            var folder2 = new ContainerInfo { Name = "Folder2" };
            
            root.AddChild(folder1);
            folder1.AddChild(folder2);
            connectionTreeModel.AddRootNode(root);

            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.ExpandAll();
            Application.DoEvents();

            // Initial check: no connections, so no counts
            // Note: Implementation of NameColumn checks for recursive child count of type Connection or PuttySession
            
            // Accessing items via GetItem(index) might be tricky if we don't know the order.
            // But we can find the item by model object.
            
            // Helper to get text for a model object
            string GetText(object model)
            {
                 int index = tree.IndexOf(model);
                 if (index < 0) return null;
                 return tree.GetItem(index).Text;
            }

            Assert.That(GetText(folder1), Is.EqualTo("Folder1"));
            Assert.That(GetText(folder2), Is.EqualTo("Folder2"));

            // Add a connection to the deep folder
            var con1 = new ConnectionInfo { Name = "Con1" };
            folder2.AddChild(con1);
            Application.DoEvents();

            // Check if counts updated
            // Folder2 should have (1)
            // Folder1 should have (1) because it contains Folder2 which contains Con1
            Assert.That(GetText(folder2), Is.EqualTo("Folder2 (1)"));
            Assert.That(GetText(folder1), Is.EqualTo("Folder1 (1)"));

            // Add another connection to Folder1 directly
            var con2 = new ConnectionInfo { Name = "Con2" };
            folder1.AddChild(con2);
            Application.DoEvents();

            // Folder2 still (1)
            // Folder1 now (2) (Con2 + Folder2's Con1)
            Assert.That(GetText(folder2), Is.EqualTo("Folder2 (1)"));
            Assert.That(GetText(folder1), Is.EqualTo("Folder1 (2)"));
        });

        [Test]
        public void RemovingConnectionUpdatesAncestorCounts() => RunWithMessagePump(tree =>
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection) { Name = "Root" };
            var folder1 = new ContainerInfo { Name = "Folder1" };
            var con1 = new ConnectionInfo { Name = "Con1" };
            
            folder1.AddChild(con1);
            root.AddChild(folder1);
            connectionTreeModel.AddRootNode(root);

            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.ExpandAll();
            Application.DoEvents();

            string GetText(object model)
            {
                 int index = tree.IndexOf(model);
                 if (index < 0) return null;
                 return tree.GetItem(index).Text;
            }

            Assert.That(GetText(folder1), Is.EqualTo("Folder1 (1)"));

            // Remove connection
            folder1.RemoveChild(con1);
            Application.DoEvents();

            Assert.That(GetText(folder1), Is.EqualTo("Folder1"));
        });
    }
}
