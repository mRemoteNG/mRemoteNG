using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Tree
{
    internal class ConnectionTreeDragAndDropHandler
    {
        internal void OnModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            var dropSource = e.SourceModels.Cast<ConnectionInfo>().First();
            var dropTarget = e.TargetModel as ContainerInfo;
            if (!NodeIsDraggable(dropSource))
                e.InfoMessage = "This node is not draggable";
            else if (NodeDraggingOntoSelf(dropSource, dropTarget))
                e.InfoMessage = "Cannot drag node onto itself";
            else if (AncestorDraggingOntoChild(dropSource, dropTarget))
                e.InfoMessage = "Cannot drag parent node onto child.";
            else
                e.Effect = DragDropEffects.Move;
            e.Handled = true;
        }

        private bool NodeIsDraggable(ConnectionInfo node)
        {
            if (node == null || node is RootNodeInfo || node is PuttySessionInfo) return false;
            return true;
        }

        private bool NodeDraggingOntoSelf(ConnectionInfo source, ConnectionInfo target)
        {
            return source.Equals(target);
        }

        private bool AncestorDraggingOntoChild(ConnectionInfo source, ConnectionInfo target)
        {
            var sourceAsContainer = source as ContainerInfo;
            return sourceAsContainer != null && sourceAsContainer.GetRecursiveChildList().Contains(target);
        }

        internal void OnModelDropped(object sender, ModelDropEventArgs e)
        {
            var draggedObject = (IHasParent)e.SourceModels[0];
            var dropTarget = e.TargetModel as ContainerInfo;
            if (dropTarget != null)
                draggedObject.SetParent(dropTarget);
            e.Handled = true;
            e.RefreshObjects();
        }
    }
}