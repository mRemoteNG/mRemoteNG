using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Root.PuttySessions;
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
            DropModel(draggedObject, dropTarget, e.DropTargetLocation);
            e.Handled = true;
        }

        public void DropModel(ConnectionInfo dropSource, ConnectionInfo dropTarget, DropTargetLocation dropTargetLocation)
        {
            if (dropTargetLocation == DropTargetLocation.Item)
                DropModelOntoTarget(dropSource, dropTarget);
            else if (dropTargetLocation == DropTargetLocation.AboveItem)
                DropModelAboveTarget(dropSource, dropTarget);
            else if (dropTargetLocation == DropTargetLocation.BelowItem)
                DropModelBelowTarget(dropSource, dropTarget);
        }

        private void DropModelOntoTarget(ConnectionInfo dropSource, ConnectionInfo dropTarget)
        {
            var dropTargetAsContainer = dropTarget as ContainerInfo;
            if (dropTargetAsContainer == null) return;
            dropSource.SetParent(dropTargetAsContainer);
        }

        private void DropModelAboveTarget(ConnectionInfo dropSource, ConnectionInfo dropTarget)
        {
            if (!dropSource.Parent.Equals(dropTarget.Parent))
                dropTarget.Parent.AddChildAbove(dropSource, dropTarget);
            else
                dropTarget.Parent.SetChildAbove(dropSource, dropTarget);
        }

        private void DropModelBelowTarget(ConnectionInfo dropSource, ConnectionInfo dropTarget)
        {
            if (!dropSource.Parent.Equals(dropTarget.Parent))
                dropTarget.Parent.AddChildBelow(dropSource, dropTarget);
            else
                dropTarget.Parent.SetChildBelow(dropSource, dropTarget);
        }

        public void HandleEvent_ModelCanDrop(object sender, ModelDropEventArgs e)
        {
            _enableFeedback = true;
            _currentFeedbackColor = DropDeniedFeedbackColor;
            _infoMessage = null;
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
            if (dropTarget is ContainerInfo && !(dropTarget is RootPuttySessionsNodeInfo))
            {
                if (!IsValidDrag(dropSource, dropTarget)) return dragDropEffect;
                dragDropEffect = DragDropEffects.Move;
                _currentFeedbackColor = DropAllowedFeedbackColor;
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

        private bool IsValidDrag(ConnectionInfo dropSource, ConnectionInfo dropTarget)
        {
            var validDrag = false;
            if (NodeDraggingOntoSelf(dropSource, dropTarget))
                _infoMessage = Language.strNodeCannotDragOnSelf;
            else if (AncestorDraggingOntoChild(dropSource, dropTarget))
                _infoMessage = Language.strNodeCannotDragParentOnChild;
            else if (DraggingOntoCurrentParent(dropSource, dropTarget))
                _infoMessage = Language.strNodeAlreadyInFolder;
            else
                validDrag = true;
            return validDrag;
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