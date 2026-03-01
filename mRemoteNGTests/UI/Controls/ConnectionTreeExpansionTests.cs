using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls.ConnectionTree;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    [TestFixture]
    public class ConnectionTreeExpansionTests
    {
        private static void RunWithMessagePump(Action<ConnectionTree> action)
        {
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    var form = new Form
                    {
                        Width = 800,
                        Height = 600,
                        ShowInTaskbar = false
                    };
                    var tree = new ConnectionTree
                    {
                        Dock = DockStyle.Fill
                    };
                    form.Controls.Add(tree);
                    form.Show(); // Must show to create handle

                    action(tree);

                    form.Close();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception != null)
                throw exception;
        }

        [Test]
        public void ApplyFilter_ExpandsMatchingFolders()
        {
            RunWithMessagePump(tree =>
            {
                // Setup model
                var model = new ConnectionTreeModel();
                var root = new RootNodeInfo(RootNodeType.Connection);
                
                var folder = new ContainerInfo { Name = "Folder" };
                var connection = new ConnectionInfo { Name = "MatchMe" };
                
                folder.AddChild(connection);
                root.AddChild(folder);
                
                model.AddRootNode(root);
                tree.ConnectionTreeModel = model;

                // Ensure folder is collapsed initially
                tree.CollapseAll();
                Assert.That(tree.IsExpanded(folder), Is.False, "Folder should be collapsed initially");

                // Apply filter
                tree.ApplyFilter("MatchMe");
                
                // Allow UI to process
                Application.DoEvents();

                // Assert folder is expanded
                Assert.That(tree.IsExpanded(folder), Is.True, "Folder should be expanded after filtering");
            });
        }

        [Test]
        public void ApplyFilter_ExpandsDeeplyNestedFolders()
        {
            RunWithMessagePump(tree =>
            {
                // Setup model
                var model = new ConnectionTreeModel();
                var root = new RootNodeInfo(RootNodeType.Connection);
                
                var folderA = new ContainerInfo { Name = "Folder A" };
                var folderB = new ContainerInfo { Name = "Folder B" };
                var connection = new ConnectionInfo { Name = "DeepMatch" };
                
                folderB.AddChild(connection);
                folderA.AddChild(folderB);
                root.AddChild(folderA);
                
                model.AddRootNode(root);
                tree.ConnectionTreeModel = model;

                // Ensure folders are collapsed initially
                tree.CollapseAll();
                Assert.That(tree.IsExpanded(folderA), Is.False, "Folder A should be collapsed initially");
                Assert.That(tree.IsExpanded(folderB), Is.False, "Folder B should be collapsed initially");

                // Apply filter
                tree.ApplyFilter("DeepMatch");
                
                // Allow UI to process
                Application.DoEvents();

                // Assert folders are expanded
                Assert.That(tree.IsExpanded(folderA), Is.True, "Folder A should be expanded after filtering");
                Assert.That(tree.IsExpanded(folderB), Is.True, "Folder B should be expanded after filtering");
            });
        }

        [Test]
        public void UserExpandAll_ExpandsHiddenFolders_AfterFilterRemoval()
        {
            RunWithMessagePump(tree =>
            {
                // Setup model
                // Root
                //  - Folder1 (Container)
                //     - Match (Connection)
                //  - Folder2 (Container)
                //     - NoMatch (Connection)
                
                var model = new ConnectionTreeModel();
                var root = new RootNodeInfo(RootNodeType.Connection);
                
                var folder1 = new ContainerInfo { Name = "Folder1" };
                var match = new ConnectionInfo { Name = "Match" };
                folder1.AddChild(match);
                
                var folder2 = new ContainerInfo { Name = "Folder2" };
                var noMatch = new ConnectionInfo { Name = "NoMatch" };
                folder2.AddChild(noMatch);
                
                root.AddChild(folder1);
                root.AddChild(folder2);
                
                model.AddRootNode(root);
                tree.ConnectionTreeModel = model;

                // 1. Collapse all initially
                tree.CollapseAll();
                
                // 2. Apply filter - only "Match" is visible, so Folder1 expands, Folder2 is hidden
                tree.ApplyFilter("Match");
                Application.DoEvents(); // Process filter

                // 3. User clicks "Expand All"
                tree.UserExpandAll(); 
                Application.DoEvents();

                // 4. Remove filter
                tree.RemoveFilter();
                Application.DoEvents();

                // 5. Assert ALL are expanded
                // Folder1 was expanded by filter, should remain expanded
                Assert.That(tree.IsExpanded(folder1), Is.True, "Folder1 should be expanded");
                
                // Folder2 was NOT expanded by filter (hidden), but UserExpandAll should have marked it for expansion
                Assert.That(tree.IsExpanded(folder2), Is.True, "Folder2 should be expanded");
            });
        }
    }
}
