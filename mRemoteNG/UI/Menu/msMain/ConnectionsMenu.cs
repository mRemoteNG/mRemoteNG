using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Menu
{
    [SupportedOSPlatform("windows")]
    public class ConnectionsMenu : ToolStripMenuItem
    {
        private StatusImageList _statusImageList;

        public ConnectionsMenu()
        {
            Initialize();
        }

        private void Initialize()
        {
            Name = "mMenConnections";
            Text = Language.Connections;
            // Lazy initialization of StatusImageList to avoid issues during early startup or if not needed
            DropDownOpening += OnDropDownOpening;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _statusImageList?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnDropDownOpening(object sender, EventArgs e)
        {
             if (_statusImageList == null)
            {
                _statusImageList = new StatusImageList();
            }

            DropDownItems.Clear();

            var model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null) return;

            foreach (var rootNode in model.RootNodes)
            {
                AddNodeToMenu(rootNode, this);
            }
        }

        private void AddNodeToMenu(ConnectionInfo node, ToolStripDropDownItem parent)
        {
            var item = new ToolStripMenuItem();
            item.Text = node.Name;
            item.Image = _statusImageList.GetImage(node);
            item.Tag = node;

            if (node is ContainerInfo container)
            {
                // It is a folder/container
                if (container.Children.Count > 0)
                {
                    foreach (var child in container.Children)
                    {
                        AddNodeToMenu(child, item);
                    }
                }
            }
            else
            {
                // It is a connection (leaf)
                item.Click += OnConnectionClick;
            }

            // Always add the item, even if it's an empty folder
            parent.DropDownItems.Add(item);
        }

        private void OnConnectionClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is ConnectionInfo connection)
            {
                 Runtime.ConnectionInitiator.OpenConnection(connection);
            }
        }
        
        public void ApplyLanguage()
        {
             Text = Language.Connections;
        }
    }
}
