using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class ConnectionTreeTests
    {
        /// <summary>
        /// Runs the given action on a dedicated STA thread with a WinForms message pump.
        /// Required because ConnectionTree inherits from TreeListView/ObjectListView
        /// which forces native Win32 handle creation in its constructor.
        /// On .NET (Core), this deadlocks without an active message pump on the owning STA thread.
        /// </summary>
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
                        var tree = new ConnectionTree
                        {
                            PostSetupActions = new IConnectionTreeDelegate[] {new RootNodeExpander()},
                            Dock = DockStyle.Fill
                        };
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
        public void CanDeleteLastFolderInTheTree() => RunWithMessagePump(tree =>
        {
            var connectionTreeModel = CreateConnectionTreeModel();
            var lastFolder = new ContainerInfo();
            connectionTreeModel.RootNodes[0].AddChild(lastFolder);
            tree.ConnectionTreeModel = connectionTreeModel;
            Application.DoEvents();
            tree.SelectObject(lastFolder);
            Application.DoEvents();
            tree.DeleteSelectedNode();
            Assert.That(tree.GetRootConnectionNode().HasChildren, Is.False);
        });

        private static ConnectionTreeModel CreateConnectionTreeModel()
        {
            var connectionTreeModel = new ConnectionTreeModel();
            connectionTreeModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
            return connectionTreeModel;
        }
    }
}
