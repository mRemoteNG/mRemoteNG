using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Messages
{
    [SupportedOSPlatform("windows")]
    public class MessageCollector : INotifyCollectionChanged
    {
        private readonly IList<IMessage> _messageList;

        public IEnumerable<IMessage> Messages => _messageList;

        public MessageCollector()
        {
            _messageList = new List<IMessage>();
        }

        public void AddMessage(MessageClass messageClass, string messageText, bool onlyLog = false)
        {
            var message = new Message(messageClass, messageText, onlyLog);
            AddMessage(message);
        }

        public void AddMessage(IMessage message)
        {
            AddMessages(new[] {message});
        }

        public void AddMessages(IEnumerable<IMessage> messages)
        {
            var newMessages = new List<IMessage>();
            foreach (var message in messages)
            {
                if (_messageList.Contains(message)) continue;
                _messageList.Add(message);
                newMessages.Add(message);
            }

            if (newMessages.Any())
                RaiseCollectionChangedEvent(NotifyCollectionChangedAction.Add, newMessages);
        }

        public void AddExceptionMessage(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = true)
        {
            AddMessage(msgClass, message + Environment.NewLine + Tools.MiscTools.GetExceptionMessageRecursive(ex),
                       logOnly);
        }

        public void AddExceptionStackTrace(string message, Exception ex, MessageClass msgClass = MessageClass.ErrorMsg, bool logOnly = true)
        {
            AddMessage(msgClass, message + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace,
                       logOnly);
        }

        public void ClearMessages()
        {
            _messageList.Clear();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, IList items)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, items));
        }
    }
}