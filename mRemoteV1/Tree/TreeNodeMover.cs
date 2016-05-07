using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using System.Windows.Forms;

namespace mRemoteNG.Tree
{
    public class TreeNodeMover
    {
        TreeNode _nodeBeingMoved;

        public TreeNodeMover (TreeNode NodeBeingMoved)
        {
            _nodeBeingMoved = NodeBeingMoved;
        }

        public void MoveNode(TreeNode TargetNode)
        {
            if (WeAreAllowedToMoveThisNode(TargetNode))
            {
                RemoveNodeFromCurrentLocation();
                AddNodeToNewLocation(TargetNode);
                UpdateParentReferences();
                SelectTheNewNode();
                Runtime.SaveConnectionsBG();
            }
        }

        private bool WeAreAllowedToMoveThisNode(TreeNode targetNode)
        {
            bool weShouldMoveThisNode = true;

            if (_nodeBeingMoved == targetNode)
                weShouldMoveThisNode = false;
            if (ConnectionTreeNode.GetNodeType(_nodeBeingMoved) == TreeNodeType.Root)
                weShouldMoveThisNode = false;
            if (_nodeBeingMoved == targetNode.Parent)
                weShouldMoveThisNode = false;

            return weShouldMoveThisNode;
        }

        private void RemoveNodeFromCurrentLocation()
        {
            _nodeBeingMoved.Remove();
        }

        private void AddNodeToNewLocation(TreeNode targetNode)
        {
            //If there is no targetNode add dropNode to the bottom of
            //the TreeView root nodes, otherwise add it to the end of
            //the dropNode child nodes
            if (ConnectionTreeNode.GetNodeType(targetNode) == TreeNodeType.Root | ConnectionTreeNode.GetNodeType(targetNode) == TreeNodeType.Container)
                targetNode.Nodes.Insert(0, _nodeBeingMoved);
            else
                targetNode.Parent.Nodes.Insert(targetNode.Index + 1, _nodeBeingMoved);
        }

        private void UpdateParentReferences()
        {
            UpdateParentReferenceWhenParentIsAContainer();
            UpdateParentReferenceWhenParentIsRoot();
        }

        private void UpdateParentReferenceWhenParentIsAContainer()
        {
            if (ConnectionTreeNode.GetNodeType(_nodeBeingMoved.Parent) == TreeNodeType.Container)
            {
                ((Parent)_nodeBeingMoved.Tag).Parent = (ContainerInfo)_nodeBeingMoved.Parent.Tag;
                ((Inheritance)_nodeBeingMoved.Tag).Inheritance.EnableInheritance();
            }
        }

        private void UpdateParentReferenceWhenParentIsRoot()
        {
            if (ConnectionTreeNode.GetNodeType(_nodeBeingMoved.Parent) == TreeNodeType.Root)
            {
                ((Parent)_nodeBeingMoved.Tag).Parent = null;
                ((Inheritance)_nodeBeingMoved.Tag).Inheritance.DisableInheritance();
            }
        }

        private void SelectTheNewNode()
        {
            _nodeBeingMoved.EnsureVisible();
            _nodeBeingMoved.TreeView.SelectedNode = _nodeBeingMoved;
        }
    }
}