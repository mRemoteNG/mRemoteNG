using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace mRemoteNG.Messages
{
    public class MessageCollector2 : INotifyCollectionChanged
    {
        private readonly IList<IMessage> _messageList;

        public IEnumerable<IMessage> Messages => _messageList;

        public MessageCollector2()
        {
            _messageList = new ObservableCollection<IMessage>();
            ((INotifyCollectionChanged) _messageList).CollectionChanged += RaiseCollectionChangedEvent;
        }

        public void AddMessage(MessageClass messageClass, string messageText)
        {
            var message = new Message(messageClass, messageText, DateTime.Now);
            AddMessage(message);
        }

        public void AddMessage(IMessage message)
        {
            if (_messageList.Contains(message)) return;
            _messageList.Add(message);
        }

        public void ClearMessages()
        {
            _messageList.Clear();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void RaiseCollectionChangedEvent(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }
    }
}