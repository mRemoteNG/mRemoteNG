using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace mRemoteNG.Tools.CustomCollections
{
	public class FullyObservableCollection<T> : IFullyNotifiableList<T>
        where T : INotifyPropertyChanged
	{
	    private readonly IList<T> _list = new List<T>();
	    private bool _eventsAllowed;

        public int Count => _list.Count;
	    public bool IsReadOnly => _list.IsReadOnly;

        public T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

	    public FullyObservableCollection()
	    {
	    }

	    public FullyObservableCollection(IEnumerable<T> items)
	    {
            AddRange(items);
        }

	    public void Add(T item)
	    {
	        _list.Add(item);
	        SubscribeToChildEvents(item);
            if (_eventsAllowed)
                RaiseCollectionChangedEvent(ActionType.Added, new[] {item});
	    }

        /// <summary>
        /// Adds a range of items to the collection.
        /// Only raises a single <see cref="CollectionUpdated"/> event
        /// after all new items are added.
        /// </summary>
        /// <param name="items"></param>
	    public void AddRange(IEnumerable<T> items)
        {
            var itemsAsList = items.ToList();
	        _eventsAllowed = false;

            foreach (var item in itemsAsList)
                Add(item);

	        _eventsAllowed = true;
            RaiseCollectionChangedEvent(ActionType.Added, itemsAsList);
	    }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            SubscribeToChildEvents(item);
            RaiseCollectionChangedEvent(ActionType.Added, new[] { item });
        }

	    public bool Remove(T item)
	    {
	        var worked = _list.Remove(item);
	        if (!worked) return worked;
	        UnsubscribeFromChildEvents(item);
            RaiseCollectionChangedEvent(ActionType.Removed, new[] {item});
	        return worked;
	    }

	    public void RemoveAt(int index)
	    {
	        var item = _list[index];
	        _list.RemoveAt(index);
	        UnsubscribeFromChildEvents(item);
            RaiseCollectionChangedEvent(ActionType.Removed, new[] { item });
	    }

        public void Clear()
        {
            var oldItems = _list.ToArray();
            _list.Clear();
            foreach (var item in oldItems)
                UnsubscribeFromChildEvents(item);
            RaiseCollectionChangedEvent(ActionType.Removed, oldItems);
        }

        private void SubscribeToChildEvents(INotifyPropertyChanged item)
	    {
	        item.PropertyChanged += ItemOnPropertyChanged;
	    }

        private void UnsubscribeFromChildEvents(INotifyPropertyChanged item)
        {
            item.PropertyChanged -= ItemOnPropertyChanged;
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
	    {
            if (sender is T)
	            RaiseCollectionChangedEvent(ActionType.Updated, new []{ (T)sender });
	    }

	    public event EventHandler<CollectionUpdatedEventArgs<T>> CollectionUpdated;

        private void RaiseCollectionChangedEvent(ActionType action, IEnumerable<T> changedItems)
        {
            CollectionUpdated?.Invoke(this, new CollectionUpdatedEventArgs<T>(action, changedItems));
        }

        #region Forwarded method calls
        public int IndexOf(T item) => _list.IndexOf(item);
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
        public bool Contains(T item) => _list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        #endregion
    }
}