using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace mRemoteNG.Messages
{
    public class MessageCollector : INotifyCollectionChanged
    {
        private readonly IList<IMessage> _messageList;

        public IEnumerable<IMessage> Messages => _messageList;

        public MessageCollector()
        {
            _messageList = new ObservableCollection<IMessage>();
            ((INotifyCollectionChanged) _messageList).CollectionChanged += RaiseCollectionChangedEvent;
        }

        public void AddMessage(MessageClass messageClass, string messageText, bool onlyLog = false)
        {
            var message = new Message(messageClass, messageText, onlyLog);
            AddMessage(message);
        }

        public void AddMessage(IMessage message)
        {
            if (_messageList.Contains(message)) return;
            _messageList.Add(message);
        }

        public void AddExceptionMessage(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = true)
        {
            AddMessage(msgClass, message + Environment.NewLine + Tools.MiscTools.GetExceptionMessageRecursive(ex));
        }

        public void AddExceptionStackTrace(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg)
        {
            AddMessage(msgClass, message + Environment.NewLine + ex.StackTrace);
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