using System.ComponentModel;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using NUnit.Framework;


namespace mRemoteNGTests.Container
{
    public class ContainerInfoTests
    {
        private ContainerInfo _containerInfo;
        private ConnectionInfo _con1;
        private ConnectionInfo _con2;
        private ConnectionInfo _con3;

        [SetUp]
        public void Setup()
        {
            _containerInfo = new ContainerInfo();
            _con1 = new ConnectionInfo {Name = "a"};
            _con2 = new ConnectionInfo {Name = "b"};
            _con3 = new ConnectionInfo {Name = "c"};
        }

        [TearDown]
        public void Teardown()
        {
            _containerInfo = null;
            _con1 = null;
            _con2 = null;
            _con3 = null;
        }

        [Test]
        public void AddSetsParentPropertyOnTheChild()
        {
            _containerInfo.AddChild(_con1);
            Assert.That(_con1.Parent, Is.EqualTo(_containerInfo));
        }

        [Test]
        public void AddAddsChildToChildrenList()
        {
            _containerInfo.AddChild(_con1);
            Assert.That(_containerInfo.Children, Does.Contain(_con1));
        }

        [Test]
        public void AddRangeAddsAllItems()
        {
            var collection = new[] { _con1, _con2, _con3 };
            _containerInfo.AddChildRange(collection);
            Assert.That(_containerInfo.Children, Is.EquivalentTo(collection));
        }

        [Test]
        public void RemoveUnsetsParentPropertyOnChild()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.RemoveChild(_con1);
            Assert.That(_con1.Parent, Is.Not.EqualTo(_containerInfo));
        }

        [Test]
        public void RemoveRemovesChildFromChildrenList()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.RemoveChild(_con1);
            Assert.That(_containerInfo.Children, Does.Not.Contains(_con1));
        }

        [Test]
        public void RemoveRangeRemovesAllIndicatedItems()
        {
            var collection = new[] { _con1, _con2, new ContainerInfo() };
            _containerInfo.AddChildRange(collection);
            _containerInfo.RemoveChildRange(collection);
            Assert.That(_containerInfo.Children, Does.Not.Contains(collection[0]).And.Not.Contains(collection[1]).And.Not.Contains(collection[2]));
        }

        [Test]
        public void RemoveRangeDoesNotRemoveUntargetedMembers()
        {
            var collection = new[] { _con1, _con2, new ContainerInfo() };
            _containerInfo.AddChildRange(collection);
            _containerInfo.AddChild(_con3);
            _containerInfo.RemoveChildRange(collection);
            Assert.That(_containerInfo.Children, Does.Contain(_con3));
        }

        [Test]
        public void AddingChildTriggersCollectionChangedEvent()
        {
            var wasCalled = false;
            _containerInfo.CollectionChanged += (sender, args) => wasCalled = true;
            _containerInfo.AddChild(_con1);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RemovingChildTriggersCollectionChangedEvent()
        {
            var wasCalled = false;
            _containerInfo.AddChild(_con1);
            _containerInfo.CollectionChanged += (sender, args) => wasCalled = true;
            _containerInfo.RemoveChild(_con1);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ChangingChildPropertyTriggersPropertyChangedEvent()
        {
            var wasCalled = false;
            _containerInfo.AddChild(_con1);
            _containerInfo.PropertyChanged += (sender, args) => wasCalled = true;
            _con1.Name = "somethinghere";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ChangingSubChildPropertyTriggersPropertyChangedEvent()
        {
            var wasCalled = false;
            var container2 = new ContainerInfo();
            _containerInfo.AddChild(container2);
            container2.AddChild(_con1);
            _containerInfo.PropertyChanged += (sender, args) => wasCalled = true;
            _con1.Name = "somethinghere";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void SetChildPositionPutsChildInCorrectPosition()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            _containerInfo.SetChildPosition(_con2, 2);
            Assert.That(_containerInfo.Children.IndexOf(_con2), Is.EqualTo(2));
        }

        [Test]
        public void SettingChildPositionAboveArrayBoundsPutsItAtEndOfList()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var finalIndex = _containerInfo.Children.Count - 1;
            _containerInfo.SetChildPosition(_con2, 5);
            Assert.That(_containerInfo.Children.IndexOf(_con2), Is.EqualTo(finalIndex));
        }

        [Test]
        public void SettingChildPositionBelowArrayBoundsDoesNotMoveTheChild()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var originalIndex = _containerInfo.Children.IndexOf(_con2);
            _containerInfo.SetChildPosition(_con2, -1);
            Assert.That(_containerInfo.Children.IndexOf(_con2), Is.EqualTo(originalIndex));
        }

        [Test]
        public void SetChildAbovePutsChildInCorrectPosition()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var referenceChildIndexBeforeMove = _containerInfo.Children.IndexOf(_con2);
            _containerInfo.SetChildAbove(_con3, _con2);
            var targetsNewIndex = _containerInfo.Children.IndexOf(_con3);
            Assert.That(targetsNewIndex, Is.EqualTo(referenceChildIndexBeforeMove));
        }

        [Test]
        public void SetChildBelowPutsChildInCorrectPosition()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var referenceChildIndexBeforeMove = _containerInfo.Children.IndexOf(_con1);
            _containerInfo.SetChildBelow(_con3, _con1);
            var targetsNewIndex = _containerInfo.Children.IndexOf(_con3);
            Assert.That(targetsNewIndex, Is.EqualTo(referenceChildIndexBeforeMove+1));
        }

        [Test]
        public void PromoteChildMovesTargetUpOne()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var targetsIndexBeforeMove = _containerInfo.Children.IndexOf(_con3);
            _containerInfo.PromoteChild(_con3);
            var targetsNewIndex = _containerInfo.Children.IndexOf(_con3);
            Assert.That(targetsNewIndex, Is.EqualTo(targetsIndexBeforeMove - 1));
        }

        [Test]
        public void PromoteChildDoesNothingWhenAlreadyAtTopOfList()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var targetsIndexBeforeMove = _containerInfo.Children.IndexOf(_con1);
            _containerInfo.PromoteChild(_con1);
            var targetsNewIndex = _containerInfo.Children.IndexOf(_con1);
            Assert.That(targetsNewIndex, Is.EqualTo(targetsIndexBeforeMove));
        }

        [Test]
        public void DemoteChildMovesTargetDownOne()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var targetsIndexBeforeMove = _containerInfo.Children.IndexOf(_con1);
            _containerInfo.DemoteChild(_con1);
            var targetsNewIndex = _containerInfo.Children.IndexOf(_con1);
            Assert.That(targetsNewIndex, Is.EqualTo(targetsIndexBeforeMove + 1));
        }

        [Test]
        public void DemoteChildDoesNothingWhenAlreadyAtTopOfList()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var targetsIndexBeforeMove = _containerInfo.Children.IndexOf(_con3);
            _containerInfo.DemoteChild(_con3);
            var targetsNewIndex = _containerInfo.Children.IndexOf(_con3);
            Assert.That(targetsNewIndex, Is.EqualTo(targetsIndexBeforeMove));
        }

        [Test]
        public void WhenChildAlreadyPresentAddChildAtDoesNothing()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var indexBeforeAttemptedMove = _containerInfo.Children.IndexOf(_con1);
            _containerInfo.AddChildAt(_con1, 2);
            var indexAfterAttemptedMove = _containerInfo.Children.IndexOf(_con1);
            Assert.That(indexAfterAttemptedMove, Is.EqualTo(indexBeforeAttemptedMove));
        }

        [Test]
        public void RemoveChildDoesNothingIfChildNotInList()
        {
            _containerInfo.AddChild(_con1);
            var childListBeforeRemoval = _containerInfo.Children;
            _containerInfo.RemoveChild(_con2);
            var childListAfterRemoval = _containerInfo.Children;
            Assert.That(childListAfterRemoval, Is.EquivalentTo(childListBeforeRemoval));
        }

        [Test]
        public void ClonedContainerHasNewConstantId()
        {
            var clone = _containerInfo.Clone();
            Assert.That(clone.ConstantID, Is.Not.EqualTo(_containerInfo.ConstantID));
        }

        [Test]
        public void ClonedContainerDoesNotHaveParentSet()
        {
            _containerInfo.SetParent(new ContainerInfo());
            var clone = _containerInfo.Clone();
            Assert.That(clone.Parent, Is.Null);
        }

        [Test]
        public void ClonedContainerContainsClonedChildren()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con3);
            var clone = _containerInfo.Clone() as ContainerInfo;
            var clonedChildNames = clone?.Children.Select((node) => node.Name);
            var originalChildNames = _containerInfo?.Children.Select((node) => node.Name);
            Assert.That(clonedChildNames, Is.EquivalentTo(originalChildNames));
        }

        [Test]
        public void HasChildrenReturnsFalseForNoChildren()
        {
            var hasChildren = _containerInfo.HasChildren();
            Assert.That(hasChildren, Is.False);
        }

        [Test]
        public void HasChildrenReturnsTrueWhenChildrenPresent()
        {
            _containerInfo.AddChild(_con1);
            var hasChildren = _containerInfo.HasChildren();
            Assert.That(hasChildren, Is.True);
        }

        [Test]
        public void AddChildAbovePutsNewChildInCorrectLocation()
        {
            _containerInfo.AddChild(_con1);
            var referenceChildIndexBeforeInsertion = _containerInfo.Children.IndexOf(_con1);
            _containerInfo.AddChildAbove(_con2, _con1);
            var newChildIndex = _containerInfo.Children.IndexOf(_con2);
            Assert.That(newChildIndex, Is.EqualTo(referenceChildIndexBeforeInsertion));
        }

        [Test]
        public void AddChildAbovePutsNewChildAtEndOfListIfReferenceChildNotInList()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChildAbove(_con2, _con3);
            var newChildIndex = _containerInfo.Children.IndexOf(_con2);
            var lastIndex = _containerInfo.Children.Count - 1;
            Assert.That(newChildIndex, Is.EqualTo(lastIndex));
        }

        [Test]
        public void AddChildBelowPutsNewChildInCorrectLocation()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con2);
            var referenceChildIndexBeforeInsertion = _containerInfo.Children.IndexOf(_con1);
            _containerInfo.AddChildBelow(_con3, _con1);
            var newChildIndex = _containerInfo.Children.IndexOf(_con3);
            Assert.That(newChildIndex, Is.EqualTo(referenceChildIndexBeforeInsertion + 1));
        }

        [Test]
        public void AddChildBelowPutsNewChildAtEndOfListIfReferenceChildNotInList()
        {
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChildBelow(_con2, _con3);
            var newChildIndex = _containerInfo.Children.IndexOf(_con2);
            var lastIndex = _containerInfo.Children.Count - 1;
            Assert.That(newChildIndex, Is.EqualTo(lastIndex));
        }

        [Test]
        public void SortAscendingSortsCorrectlyByName()
        {
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con3);
            _containerInfo.Sort();
            var orderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(orderAfterSort, Is.Ordered.Ascending.By(nameof(ConnectionInfo.Name)));
        }

        [Test]
        public void SortDescendingSortsCorrectlyByName()
        {
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con3);
            _containerInfo.Sort(ListSortDirection.Descending);
            var orderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(orderAfterSort, Is.Ordered.Descending.By(nameof(ConnectionInfo.Name)));
        }

        [Test]
        public void SortOnConstantIdAscendingSortsCorrectly()
        {
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con3);
            _containerInfo.SortOn(node=> node.ConstantID);
            var orderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(orderAfterSort, Is.Ordered.Ascending.By(nameof(ConnectionInfo.ConstantID)));
        }

        [Test]
        public void SortOnConstantIdDescendingSortsCorrectly()
        {
            _containerInfo.AddChild(_con2);
            _containerInfo.AddChild(_con1);
            _containerInfo.AddChild(_con3);
            _containerInfo.SortOn(node => node.ConstantID, ListSortDirection.Descending);
            var orderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(orderAfterSort, Is.Ordered.Descending.By(nameof(ConnectionInfo.ConstantID)));
        }

        [Test]
        public void SortAscendingRecursiveSortsGrandchildrenCorrectlyByName()
        {
            var childContainer = new ContainerInfo();
            childContainer.AddChild(_con2);
            childContainer.AddChild(_con1);
            childContainer.AddChild(_con3);
            _containerInfo.AddChild(childContainer);
            _containerInfo.SortRecursive();
            var grandchildOrderAfterSort = childContainer.Children.ToArray();
            Assert.That(grandchildOrderAfterSort, Is.Ordered.Ascending.By(nameof(ConnectionInfo.Name)));
        }

        [Test]
        public void SortDescendingRecursiveSortsGrandchildrenCorrectlyByName()
        {
            var childContainer = new ContainerInfo();
            childContainer.AddChild(_con2);
            childContainer.AddChild(_con1);
            childContainer.AddChild(_con3);
            _containerInfo.AddChild(childContainer);
            _containerInfo.SortRecursive(ListSortDirection.Descending);
            var grandchildOrderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(grandchildOrderAfterSort, Is.Ordered.Descending.By(nameof(ConnectionInfo.Name)));
        }

        [Test]
        public void SortOnRecursiveConstantIdAscendingSortsGrandchildrenCorrectly()
        {
            var childContainer = new ContainerInfo();
            childContainer.AddChild(_con2);
            childContainer.AddChild(_con1);
            childContainer.AddChild(_con3);
            _containerInfo.AddChild(childContainer);
            _containerInfo.SortOnRecursive(node => node.ConstantID);
            var grandchildOrderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(grandchildOrderAfterSort, Is.Ordered.Ascending.By(nameof(ConnectionInfo.ConstantID)));
        }

        [Test]
        public void SortOnRecursiveConstantIdDescendingSortsGrandchildrenCorrectly()
        {
            var childContainer = new ContainerInfo();
            childContainer.AddChild(_con2);
            childContainer.AddChild(_con1);
            childContainer.AddChild(_con3);
            _containerInfo.AddChild(childContainer);
            _containerInfo.SortOnRecursive(node => node.ConstantID, ListSortDirection.Descending);
            var grandchildOrderAfterSort = _containerInfo.Children.ToArray();
            Assert.That(grandchildOrderAfterSort, Is.Ordered.Descending.By(nameof(ConnectionInfo.ConstantID)));
        }
    }
}