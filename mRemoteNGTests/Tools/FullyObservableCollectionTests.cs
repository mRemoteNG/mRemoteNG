using System.Collections.Generic;
using System.ComponentModel;
using mRemoteNG.Tools.CustomCollections;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public class FullyObservableCollectionTests
    {
        [Test]
        public void CollectionBeginsEmpty()
        {
            var list = new FullyObservableCollection<INotifyPropertyChanged>();
            Assert.That(list, Is.Empty);
        }

        [Test]
        public void CanCreateWithExistingList()
        {
            var existingList = new List<INotifyPropertyChanged>
            {
                Substitute.For<INotifyPropertyChanged>(),
                Substitute.For<INotifyPropertyChanged>(),
                Substitute.For<INotifyPropertyChanged>()
            };
            var list = new FullyObservableCollection<INotifyPropertyChanged>(existingList);
            Assert.That(list, Has.Count.EqualTo(3));
        }

        [Test]
        public void ItemAdded()
        {
            var list = new FullyObservableCollection<INotifyPropertyChanged>();
            var item = Substitute.For<INotifyPropertyChanged>();
            list.Add(item);
            Assert.That(list, Has.Member(item));
        }

        [Test]
        public void ItemInserted()
        {
            var list = new FullyObservableCollection<INotifyPropertyChanged>();
            var item = Substitute.For<INotifyPropertyChanged>();
            list.Insert(0, item);
            Assert.That(list[0], Is.EqualTo(item));
        }

        [Test]
        public void ItemRemoved()
        {
            var item = Substitute.For<INotifyPropertyChanged>();
            var list = new FullyObservableCollection<INotifyPropertyChanged>
            {
                item
            };
            list.Remove(item);
            Assert.That(list, Does.Not.Contains(item));
        }

        [Test]
        public void ItemRemovedAtIndex()
        {
            var item = Substitute.For<INotifyPropertyChanged>();
            var list = new FullyObservableCollection<INotifyPropertyChanged>
            {
                item
            };
            list.RemoveAt(0);
            Assert.That(list, Does.Not.Contains(item));
        }

        [Test]
        public void ClearRemovesAllItems()
        {
            var list = new FullyObservableCollection<INotifyPropertyChanged>
            {
                Substitute.For<INotifyPropertyChanged>(),
                Substitute.For<INotifyPropertyChanged>(),
                Substitute.For<INotifyPropertyChanged>()
            };
            list.Clear();
            Assert.That(list, Is.Empty);
        }

        [Test]
        public void ChildItemEventsTriggerListEvents()
        {
            var wasCalled = false;
            var item = Substitute.For<INotifyPropertyChanged>();
            var list = new FullyObservableCollection<INotifyPropertyChanged> {item};
            list.CollectionUpdated += (sender, args) => wasCalled = true;
            RaiseEvent(item);
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ListUnsubscribesFromRemovedItems()
        {
            var wasCalled = false;
            var item = Substitute.For<INotifyPropertyChanged>();
            var list = new FullyObservableCollection<INotifyPropertyChanged> { item };
            list.Remove(item);
            list.CollectionUpdated += (sender, args) => wasCalled = true;
            RaiseEvent(item);
            Assert.That(wasCalled, Is.False);
        }

        private void RaiseEvent(INotifyPropertyChanged item)
        {
            item.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(item, new PropertyChangedEventArgs("test"));
        }
    }
}