using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools.Sorting;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mRemoteNG.Tree
{
    public static class ConnectionTree
    {
        private static TreeNode SetNodeToolTip_old_node;
        private static TreeNode treeNodeToBeSelected;

        public static TreeView TreeView { get; set; }

        public static TreeNode SelectedNode
        {
            get
            { 
                return TreeView?.SelectedNode;
            }
            set
            {
                treeNodeToBeSelected = value;
                SelectNode();
            }
        }

        //TODO Fix for TreeListView
        public static void CollapseAllNodes()
        {
            TreeView.BeginUpdate();
            foreach (TreeNode treeNode in TreeView.Nodes[0].Nodes)
            {
                treeNode.Collapse(false);
            }
            TreeView.EndUpdate();
        }

        //TODO Fix for TreeListView
        public static void MoveNodeDown()
        {
            try
            {
                if (SelectedNode?.NextNode == null) return;
                TreeView.BeginUpdate();
                TreeView.Sorted = false;

                TreeNode newNode = (TreeNode)SelectedNode.Clone();
                SelectedNode.Parent.Nodes.Insert(SelectedNode.Index + 2, newNode);
                SelectedNode.Remove();
                SelectedNode = newNode;

                TreeView.EndUpdate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "MoveNodeDown failed" + Environment.NewLine + ex.Message, true);
            }
        }

        //TODO Fix for TreeListView
        public static void MoveNodeUp()
        {
            try
            {
                if (SelectedNode?.PrevNode == null) return;
                TreeView.BeginUpdate();
                TreeView.Sorted = false;

                TreeNode newNode = (TreeNode)SelectedNode.Clone();
                SelectedNode.Parent.Nodes.Insert(SelectedNode.Index - 1, newNode);
                SelectedNode.Remove();
                SelectedNode = newNode;

                TreeView.EndUpdate();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "MoveNodeUp failed" + Environment.NewLine + ex.Message, true);
            }
        }

        //TODO Fix for TreeListView
        public static void Sort(TreeNode treeNode, SortOrder sorting)
        {
            if (TreeView == null)
                return;

            TreeView.BeginUpdate();

            if (treeNode == null)
            {
                if (TreeView.Nodes.Count > 0)
                    treeNode = TreeView.Nodes[0];
                else
                    return;
            }
            else if (ConnectionTreeNode.GetNodeType(treeNode) == TreeNodeType.Connection)
            {
                treeNode = treeNode.Parent;
                if (treeNode == null)
                    return;
            }

            Sort(treeNode, new TreeNodeSorter(sorting));
            TreeView.EndUpdate();
        }

        //TODO Fix for TreeListView
        private static void Sort(TreeNode treeNode, TreeNodeSorter nodeSorter)
        {
            // Adapted from http://www.codeproject.com/Tips/252234/ASP-NET-TreeView-Sort
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                Sort(childNode, nodeSorter);
            }

            try
            {
                List<TreeNode> sortedNodes = new List<TreeNode>();
                TreeNode currentNode = null;
                while (treeNode.Nodes.Count > 0)
                {
                    foreach (TreeNode childNode in treeNode.Nodes)
                    {
                        if (currentNode == null || nodeSorter.Compare(childNode, currentNode) < 0)
                        {
                            currentNode = childNode;
                        }
                    }
                    if (currentNode != null)
                    {
                        treeNode.Nodes.Remove(currentNode);
                        sortedNodes.Add(currentNode);
                    }
                    currentNode = null;
                }

                foreach (TreeNode childNode in sortedNodes)
                {
                    treeNode.Nodes.Add(childNode);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Sort nodes failed" + Environment.NewLine + ex.Message, true);
            }
        }

        //TODO Fix for TreeListView
        public static TreeNode Find(TreeNode treeNode, string searchFor)
        {
            
            try
            {
                if (IsThisTheNodeWeAreSearchingFor(treeNode, searchFor))
                    return treeNode;

                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    TreeNode tmpNode = Find(childNode, searchFor);
                    if (tmpNode != null)
                    {
                        return tmpNode;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Find node failed" + Environment.NewLine + ex.Message, true);
            }

            return null;
        }

        //TODO Fix for TreeListView
        private static bool IsThisTheNodeWeAreSearchingFor(TreeNode treeNode, string searchFor)
        {
            return treeNode.Text.ToLower().IndexOf(searchFor.ToLower(), StringComparison.Ordinal) + 1 > 0;
        }

        //TODO Fix for TreeListView
        public static TreeNode Find(TreeNode treeNode, ConnectionInfo conInfo)
        {
            try
            {
                if (treeNode.Tag == conInfo)
                    return treeNode;

                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    TreeNode tmpNode = Find(childNode, conInfo);
                    if (tmpNode != null)
                        return tmpNode;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Find node failed" + Environment.NewLine + ex.Message, true);
            }

            return null;
        }

        //TODO Fix for TreeListView
        private delegate void ResetTreeDelegate();
        public static void ResetTree()
        {
            if (TreeView.InvokeRequired)
            {
                ResetTreeDelegate resetTreeDelegate = ResetTree;
                Windows.treeForm.Invoke(resetTreeDelegate);
            }
            else
            {
                TreeView.BeginUpdate();
                TreeView.Nodes.Clear();
                TreeView.Nodes.Add(Language.strConnections);
                TreeView.EndUpdate();
            }
        }

        //TODO Fix for TreeListView
        private delegate void SelectNodeDelegate();
        private static void SelectNode()
        {
            if (TreeView.InvokeRequired)
            {
                SelectNodeDelegate d = SelectNode;
                TreeView.Invoke(d);
            }
            else
            {
                TreeView.SelectedNode = treeNodeToBeSelected;
            }
        }
    }
}