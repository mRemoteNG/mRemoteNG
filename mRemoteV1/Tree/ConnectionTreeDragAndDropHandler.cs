using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Tree
{
    public class ConnectionTreeDragAndDropHandler
    {
        public Color DropAllowedFeedbackColor = Color.Green;
        public Color DropDeniedFeedbackColor = Color.Red;
        private string _infoMessage;
        private Color _currentFeedbackColor;
        private bool _enableFeedback;


        public void HandleEvent_ModelDropped(object sender, ModelDropEventArgs e)
        {
            var dropTarget = e.TargetModel as ConnectionInfo;
            if (dropTarget == null) return;
            var draggedObject = (ConnectionInfo)e.SourceModels[0];
            if (e.DropTargetLocation == DropTargetLocation.Item)
            {
                var dropTargetAsContainer = dropTarget as ContainerInfo;
                if (dropTargetAsContainer == null) return;
                draggedObject.SetParent(dropTargetAsContainer);
            }
            else if (e.DropTargetLocation == DropTargetLocation.AboveItem)
            {
                if (!draggedObject.Parent.Equals(dropTarget.Parent))
                    dropTarget.Parent.AddChildAbove(draggedObject, dropTarget);
                else
                    dropTarget.Parent.SetChildAbove(draggedObject, dropTarget);
            }
            else if (e.DropTargetLocation == DropTargetLocation.BelowItem)
            {
                if (!draggedObject.Parent.Equals(dropTarget.Parent))
                    dropTarget.Parent.AddChildBelow(draggedObject, dropTarget);
                else
                    dropTarget.Parent.SetChildBelow(draggedObject, dropTarget);
            }
            e.Handled = true;
        }

        public void HandleEvent_ModelCanDrop(object sender, ModelDropEventArgs e)
        {

            _enableFeedback = true;
            _currentFeedbackColor = DropDeniedFeedbackColor;
            var dropSource = e.SourceModels.Cast<ConnectionInfo>().First();
            var dropTarget = e.TargetModel as ConnectionInfo;

            e.Effect = CanModelDrop(dropSource, dropTarget, e.DropTargetLocation);
            e.InfoMessage = _infoMessage;
            e.DropSink.EnableFeedback = _enableFeedback;
            e.DropSink.FeedbackColor = _currentFeedbackColor;
            e.Handled = true;
        }

        public DragDropEffects CanModelDrop(ConnectionInfo dropSource, ConnectionInfo dropTarget, DropTargetLocation dropTargetLocation)
        {
            var dragDropEffect = DragDropEffects.None;
            if (!NodeIsDraggable(dropSource))
            {
                _infoMessage = Language.strNodeNotDraggable;
                _enableFeedback = false;
            }
            else if (dropTargetLocation == DropTargetLocation.Item)
                dragDropEffect = HandleCanDropOnItem(dropSource, dropTarget);
            else if (dropTargetLocation == DropTargetLocation.AboveItem || dropTargetLocation == DropTargetLocation.BelowItem)
                dragDropEffect = HandleCanDropBetweenItems(dropSource, dropTarget);
            return dragDropEffect;
        }

        private DragDropEffects HandleCanDropOnItem(ConnectionInfo dropSource, ConnectionInfo dropTarget)
        {
            var dragDropEffect = DragDropEffects.None;
            if (dropTarget is ContainerInfo)
            {
                if (NodeDraggingOntoSelf(dropSource, dropTarget))
                    _infoMessage = Language.strNodeCannotDragOnSelf;
                else if (AncestorDraggingOntoChild(dropSource, dropTarget))
                    _infoMessage = Language.strNodeCannotDragParentOnChild;
                else if (DraggingOntoCurrentParent(dropSource, dropTarget))
                    _infoMessage = Language.strNodeAlreadyInFolder;
                else
                {
                    dragDropEffect = DragDropEffects.Move;
                    _currentFeedbackColor = DropAllowedFeedbackColor;
                }
            }
            else
            {
                _enableFeedback = false;
            }
            return dragDropEffect;
        }

        private DragDropEffects HandleCanDropBetweenItems(ConnectionInfo dropSource, ConnectionInfo dropTarget)
        {
            var dragDropEffect = DragDropEffects.None;
            if (AncestorDraggingOntoChild(dropSource, dropTarget))
                _infoMessage = Language.strNodeCannotDragParentOnChild;
            else
            {
                dragDropEffect = DragDropEffects.Move;
                _currentFeedbackColor = DropAllowedFeedbackColor;
            }
            return dragDropEffect;
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