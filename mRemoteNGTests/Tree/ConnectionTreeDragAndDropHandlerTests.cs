using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class ConnectionTreeDragAndDropHandlerTests
    {
        private ConnectionTreeDragAndDropHandler _dragAndDropHandler;
        private RootNodeInfo _rootNode;
        private ContainerInfo _container1;
        private ContainerInfo _container2;
        private ContainerInfo _container3;
        private ConnectionInfo _connection1;
        private ConnectionInfo _connection2;
        private ConnectionInfo _connection3;
        private ConnectionInfo _connection4;
        private ConnectionInfo _connection5;

        [SetUp]
        public void Setup()
        {
            _dragAndDropHandler = new ConnectionTreeDragAndDropHandler();
            InitializeNodes();
            CreateSimpleTreeModel();
        }

        [TearDown]
        public void Teardown()
        {
            _dragAndDropHandler = null;
            DestroyNodes();
        }

        [Test]
        public void CanDragConnectionOntoContainer()
        {
            var source = _connection3;
            var target = _container1;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.Move));
        }

        [Test]
        public void CanDragContainerOntoContainer()
        {
            var source = _container1;
            var target = _container2;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.Move));
        }

        [Test]
        public void CanDragContainerBetweenContainers()
        {
            var source = _container3;
            var target = _container2;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.AboveItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.Move));
        }

        [Test]
        public void CanDragConnectionBetweenConnections()
        {
            var source = _connection1;
            var target = _connection4;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.AboveItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.Move));
        }

        [Test]
        public void CantDragConnectionOntoItself()
        {
            var source = _connection1;
            var target = _connection1;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragContainerOntoItself()
        {
            var source = _container1;
            var target = _container1;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragContainerOntoItsChild()
        {
            var source = _container2;
            var target = _container3;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragContainerBetweenItsChildren()
        {
            var source = _container3;
            var target = _connection4;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.BelowItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragNodeOntoItsCurrentParent()
        {
            var source = _container3;
            var target = _container2;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragRootNodeInfo()
        {
            var source = _rootNode;
            var target = new ContainerInfo();
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragNodeAboveRootNodeInfo()
        {
            var source = _connection1;
            var target = _rootNode;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.AboveItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragNodeBelowRootNodeInfo()
        {
            var source = _connection1;
            var target = _rootNode;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.BelowItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragPuttySessionInfo()
        {
            var source = new PuttySessionInfo();
            var target = _container2;
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragConnectionOntoRootPuttySessionNode()
        {
            var source = _connection1;
            var target = new RootPuttySessionsNodeInfo();
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragContainerOntoRootPuttySessionNode()
        {
            var source = _container1;
            var target = new RootPuttySessionsNodeInfo();
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.Item);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragNodeAbovePuttySessionNodes()
        {
            var source = _connection1;
            var target = new PuttySessionInfo();
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.AboveItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void CantDragNodeBelowPuttySessionNodes()
        {
            var source = _connection1;
            var target = new PuttySessionInfo();
            var dragDropEffects = _dragAndDropHandler.CanModelDrop(source, target, DropTargetLocation.BelowItem);
            Assert.That(dragDropEffects, Is.EqualTo(DragDropEffects.None));
        }

        [Test]
        public void DraggingNodeBelowSiblingRearrangesTheUnderlyingModelCorrectly()
        {
            var source = _connection3;
            var target = _connection5;
            var location = DropTargetLocation.BelowItem;
            _dragAndDropHandler.DropModel(source, target, location);
            var actualIndex = _container3.Children.IndexOf(source);
            var expectedIndex = _container3.Children.IndexOf(target) + 1;
            Assert.That(actualIndex, Is.EqualTo(expectedIndex));
        }

        [Test]
        public void DraggingNodeAboveSiblingRearrangesTheUnderlyingModelCorrectly()
        {
            var source = _connection3;
            var target = _connection5;
            var location = DropTargetLocation.AboveItem;
            _dragAndDropHandler.DropModel(source, target, location);
            var actualIndex = _container3.Children.IndexOf(source);
            var expectedIndex = _container3.Children.IndexOf(target) - 1;
            Assert.That(actualIndex, Is.EqualTo(expectedIndex));
        }

        [Test]
        public void DraggingNodeToNewContainerAddsNodeToTheNewContainer()
        {
            var source = _connection3;
            var target = _container1;
            var location = DropTargetLocation.Item;
            _dragAndDropHandler.DropModel(source, target, location);
            Assert.That(target.Children.Contains(source));
        }

        [Test]
        public void DraggingNodeToNewContainerRemovesNodeFromOldContainer()
        {
            var source = _connection3;
            var target = _container1;
            var location = DropTargetLocation.Item;
            _dragAndDropHandler.DropModel(source, target, location);
            Assert.That(!_container3.Children.Contains(source));
        }


        private void InitializeNodes()
        {
            _rootNode = new RootNodeInfo(RootNodeType.Connection);
            _container1 = new ContainerInfo();
            _container2 = new ContainerInfo();
            _container3 = new ContainerInfo();
            _connection1 = new ConnectionInfo();
            _connection2 = new ConnectionInfo();
            _connection3 = new ConnectionInfo();
            _connection4 = new ConnectionInfo();
            _connection5 = new ConnectionInfo();
        }

        private void CreateSimpleTreeModel()
        {
            /*
             * Root
             * |-- container1
             * |   L-- connection1
             * L-- container2
             *     |-- container3
             *     |   |-- connection3
             *     |   |-- connection4
             *     |   L-- connection5
             *     L-- connection2
             */
            _rootNode.AddChild(_container1);
            _rootNode.AddChild(_container2);
            _container1.AddChild(_connection1);
            _container2.AddChild(_container3);
            _container2.AddChild(_connection2);
            _container3.AddChild(_connection3);
            _container3.AddChild(_connection4);
            _container3.AddChild(_connection5);
        }

        private void DestroyNodes()
        {
            _rootNode = null;
            _container1 = null;
            _container2 = null;
            _container3 = null;
            _connection1 = null;
            _connection2 = null;
            _connection3 = null;
            _connection4 = null;
            _connection5 = null;
        }
    }
}