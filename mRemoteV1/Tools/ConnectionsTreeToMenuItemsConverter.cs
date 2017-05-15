using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.UI.Controls;


namespace mRemoteNG.Tools
{
    public class ConnectionsTreeToMenuItemsConverter
    {
        private readonly StatusImageList _statusImageList = new StatusImageList();
        public MouseEventHandler MouseUpEventHandler { get; set; }

        public IEnumerable<ToolStripDropDownItem> CreateToolStripDropDownItems(ConnectionTreeModel connectionTreeModel)
        {
            var rootNodes = connectionTreeModel.RootNodes;
            return CreateToolStripDropDownItems(rootNodes);
        }

        public IEnumerable<ToolStripDropDownItem> CreateToolStripDropDownItems(IEnumerable<ConnectionInfo> nodes)
        {
            var dropDownList = new List<ToolStripDropDownItem>();
            try
            {
                dropDownList.AddRange(nodes.Select(CreateMenuItem));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("frmMain.AddNodeToMenu() failed", ex);
            }
            return dropDownList;
        }

        private void AddSubMenuNodes(IEnumerable<ConnectionInfo> nodes, ToolStripDropDownItem toolStripMenuItem)
        {
            foreach (var connectionInfo in nodes)
            {
                var newItem = CreateMenuItem(connectionInfo);
                toolStripMenuItem.DropDownItems.Add(newItem);
            }
        }

        private ToolStripDropDownItem CreateMenuItem(ConnectionInfo node)
        {
            var menuItem = new ToolStripMenuItem
            {
                Text = node.Name,
                Tag = node,
                Image = _statusImageList.GetImage(node)
            };

            var nodeAsContainer = node as ContainerInfo;
            if (nodeAsContainer != null)
            {
                AddSubMenuNodes(nodeAsContainer.Children, menuItem);
            }
            
            menuItem.MouseUp += MouseUpEventHandler;
            return menuItem;
        }
    }
}