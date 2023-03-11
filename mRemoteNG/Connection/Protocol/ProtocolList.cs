using System;
using System.Collections;
using System.Collections.Specialized;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Connection.Protocol
{
    public class ProtocolList : CollectionBase, INotifyCollectionChanged
    {
        public ProtocolBase this[object index]
        {
            get
            {
                var @base = index as ProtocolBase;
                if (@base != null)
                    return @base;
                if (index is int)
                    return (ProtocolBase)List[Convert.ToInt32(index)];
                return null;
            }
        }

        public new int Count => List.Count;


        public void Add(ProtocolBase cProt)
        {
            List.Add(cProt);
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, cProt));
        }

        public void AddRange(ProtocolBase[] cProt)
        {
            foreach (var cP in cProt)
            {
                List.Add(cP);
            }

            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, cProt));
        }

        public void Remove(ProtocolBase cProt)
        {
            try
            {
                if (!List.Contains(cProt))
                    return;

                List.Remove(cProt);
                RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, cProt));
            }
            catch (Exception)
            {
            }
        }

        public new void Clear()
        {
            if (Count == 0)
                return;

            List.Clear();
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }
    }
}