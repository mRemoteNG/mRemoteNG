using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace mRemoteNG.Tools
{
    public class ObservablePropertyCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        private readonly ObservableCollection<T> _collection = new ObservableCollection<T>();

        public int Count => _collection.Count;
        public bool IsReadOnly { get; }

        public T this[int index]
        {
            get { return _collection[index]; }
            set { _collection[index] = value; }
        }

        public ObservablePropertyCollection()
        {
            _collection.CollectionChanged += RaiseCollectionChangedEvent;
        }

        public ObservablePropertyCollection(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            foreach(var item in list)
                Add(item);
            _collection.CollectionChanged += RaiseCollectionChangedEvent;
        }

        public void Add(T item)
        {
            if (_collection.Contains(item)) return;
            _collection.Add(item);
            item.PropertyChanged += RaisePropertyChangedEvent;
        }

        public void Insert(int index, T item)
        {
            if (_collection.Contains(item)) return;
            _collection.Insert(index, item);
            item.PropertyChanged += RaisePropertyChangedEvent;
        }

        public bool Remove(T item)
        {
            if (!_collection.Contains(item)) return false;
            item.PropertyChanged -= RaisePropertyChangedEvent;
            return _collection.Remove(item);
        }

        public void RemoveAt(int index)
        {
            var removalTarget = _collection[index];
            if (removalTarget == null) return;
            _collection.Remove(removalTarget);
        }

        public void Clear()
        {
            foreach (var item in _collection)
                item.PropertyChanged -= RaisePropertyChangedEvent;
            _collection.Clear();
        }

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(T item) => _collection.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);
        public int IndexOf(T item) => _collection.IndexOf(item);


        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        private void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            PropertyChanged?.Invoke(sender, propertyChangedEventArgs);
        }
    }
}