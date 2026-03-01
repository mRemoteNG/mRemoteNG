using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Connection.Protocol
{
    [SupportedOSPlatform("windows")]
    public class ProtocolList : CollectionBase, INotifyCollectionChanged
    {
        public ProtocolBase? this[object index]
        {
            get
            {
                if (index is ProtocolBase @base)
                    return @base;
                if (index is int)
                    return List[Convert.ToInt32(index, CultureInfo.InvariantCulture)] as ProtocolBase;
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
            foreach (ProtocolBase cP in cProt)
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
                _ = 0; // Intentionally empty
            }
        }

        public new void Clear()
        {
            if (Count == 0)
                return;

            List.Clear();
            RaiseCollectionChangedEvent(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged; // Fix for CS8612: Declare the event as nullable to match the interface.

        private void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}