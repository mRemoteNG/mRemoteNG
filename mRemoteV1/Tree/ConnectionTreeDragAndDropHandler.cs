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
            var draggedObject = e.SourceModels.Cast<ConnectionInfo>().First();
            var dropTarget = e.TargetModel as ContainerInfo;
            if (AncestorDraggingOntoChild(draggedObject, dropTarget))
                e.InfoMessage = "Cannot drag parent node onto child.";
            else if (!NodeIsDraggable(draggedObject))
                e.InfoMessage = "This node is not draggable";
            else
                e.Effect = DragDropEffects.Move;
            e.Handled = true;
        }

        private bool NodeIsDraggable(ConnectionInfo node)
        {
            if (node == null || node is RootNodeInfo || node is PuttySessionInfo) return false;
            return true;
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