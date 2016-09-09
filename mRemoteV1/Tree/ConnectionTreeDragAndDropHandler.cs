using System.Drawing;
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
        internal Color DropAllowedFeedbackColor = Color.Green;
        internal Color DropDeniedFeedbackColor = Color.Red;

        internal void HandleEvent_ModelDropped(object sender, ModelDropEventArgs e)
        {
            var draggedObject = (ConnectionInfo)e.SourceModels[0];
            var dropTarget = e.TargetModel as ConnectionInfo;
            if (dropTarget == null) return;
            if (e.DropTargetLocation == DropTargetLocation.Item)
            {
                var dropTargetAsContainer = dropTarget as ContainerInfo;
                if (dropTargetAsContainer == null) return;
                draggedObject.SetParent(dropTargetAsContainer);
            }
            else if (e.DropTargetLocation == DropTargetLocation.AboveItem)
            {
                if (!draggedObject.Parent.Equals(dropTarget.Parent))
                    draggedObject.SetParent(dropTarget.Parent);

                dropTarget.Parent.SetChildAbove(draggedObject, dropTarget);
            }
            else if (e.DropTargetLocation == DropTargetLocation.BelowItem)
            {
                if (!draggedObject.Parent.Equals(dropTarget.Parent))
                    draggedObject.SetParent(dropTarget.Parent);
                dropTarget.Parent.SetChildBelow(draggedObject, dropTarget);
            }
            e.Handled = true;
            e.RefreshObjects();
        }

        internal void HandleEvent_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            e.DropSink.EnableFeedback = true;
            e.DropSink.FeedbackColor = DropDeniedFeedbackColor;
            var dropSource = e.SourceModels.Cast<ConnectionInfo>().First();
            var dropTarget = e.TargetModel as ConnectionInfo;

            if (!NodeIsDraggable(dropSource))
            {
                e.InfoMessage = Language.strNodeNotDraggable;
                e.DropSink.EnableFeedback = false;
            }
            else if (e.DropTargetLocation == DropTargetLocation.Item)
                HandleCanDropOnItem(dropSource, dropTarget, e);
            else if (e.DropTargetLocation == DropTargetLocation.AboveItem || e.DropTargetLocation == DropTargetLocation.BelowItem)
                HandleCanDropBetweenItems(dropSource, dropTarget, e);
            else
                return;
            e.Handled = true;
        }

        private void HandleCanDropOnItem(ConnectionInfo dropSource, ConnectionInfo dropTarget, ModelDropEventArgs e)
        {
            if (dropTarget is ContainerInfo)
            {
                if (NodeDraggingOntoSelf(dropSource, dropTarget))
                    e.InfoMessage = Language.strNodeCannotDragOnSelf;
                else if (AncestorDraggingOntoChild(dropSource, dropTarget))
                    e.InfoMessage = Language.strNodeCannotDragParentOnChild;
                else if (DraggingOntoCurrentParent(dropSource, dropTarget))
                    e.InfoMessage = Language.strNodeAlreadyInFolder;
                else
                {
                    e.Effect = DragDropEffects.Move;
                    e.DropSink.FeedbackColor = DropAllowedFeedbackColor;
                }
            }
            else
            {
                e.DropSink.EnableFeedback = false;
            }
        }

        private void HandleCanDropBetweenItems(ConnectionInfo dropSource, ConnectionInfo dropTarget, ModelDropEventArgs e)
        {
            if (AncestorDraggingOntoChild(dropSource, dropTarget))
                e.InfoMessage = Language.strNodeCannotDragParentOnChild;
            else
            {
                e.Effect = DragDropEffects.Move;
                e.DropSink.FeedbackColor = DropAllowedFeedbackColor;
            }
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

        private bool DraggingOntoCurrentParent(ConnectionInfo source, ConnectionInfo target)
        {
            var targetAsContainer = target as ContainerInfo;
            return targetAsContainer != null && targetAsContainer.Children.Contains(source);
        }
    }
}