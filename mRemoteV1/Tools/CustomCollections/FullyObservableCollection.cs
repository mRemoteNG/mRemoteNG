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
            foreach (var item in items)
                Add(item);
        }

	    public void Add(T item)
	    {
	        _list.Add(item);
	        SubscribeToChildEvents(item);
            RaiseCredentialChangedEvent(ActionType.Added, new[] {item});
	    }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            SubscribeToChildEvents(item);
            RaiseCredentialChangedEvent(ActionType.Added, new[] { item });
        }

	    public bool Remove(T item)
	    {
	        var worked = _list.Remove(item);
	        if (!worked) return worked;
	        RaiseCredentialChangedEvent(ActionType.Removed, new[] {item});
	        UnsubscribeFromChildEvents(item);
	        return worked;
	    }

	    public void RemoveAt(int index)
	    {
	        var item = _list[index];
	        _list.RemoveAt(index);
	        UnsubscribeFromChildEvents(item);
            RaiseCredentialChangedEvent(ActionType.Removed, new[] { item });
	    }

        public void Clear()
        {
            var oldItems = _list.ToArray();
            _list.Clear();
            foreach (var item in oldItems)
                UnsubscribeFromChildEvents(item);
            RaiseCredentialChangedEvent(ActionType.Removed, oldItems);
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
	            RaiseCredentialChangedEvent(ActionType.Updated, new []{ (T)sender });
	    }

	    public event EventHandler<CollectionUpdatedEventArgs<T>> CollectionUpdated;

        private void RaiseCredentialChangedEvent(ActionType action, IEnumerable<T> changedItems)
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