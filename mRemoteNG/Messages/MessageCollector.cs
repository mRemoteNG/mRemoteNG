using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Messages
{
    [SupportedOSPlatform("windows")]
    public class MessageCollector : INotifyCollectionChanged
    {
        private const int MaxMessages = 10_000;
        private readonly IList<IMessage> _messageList;

        public IEnumerable<IMessage> Messages => _messageList;

        public MessageCollector()
        {
            _messageList = new List<IMessage>();
        }

        public void AddMessage(MessageClass messageClass, string messageText, bool onlyLog = false)
        {
            Message message = new(messageClass, messageText, onlyLog);
            AddMessage(message);
        }

        public void AddMessage(IMessage message)
        {
            AddMessages(new[] {message});
        }

        public void AddMessages(IEnumerable<IMessage> messages)
        {
            List<IMessage> newMessages = new();
            foreach (IMessage message in messages)
            {
                if (_messageList.Contains(message)) continue;
                _messageList.Add(message);
                newMessages.Add(message);
            }

            // Prevent unbounded growth in long-running sessions
            while (_messageList.Count > MaxMessages)
                _messageList.RemoveAt(0);

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
            AddMessage(msgClass, message + Environment.NewLine + ex.Message + Environment.NewLine + ex.Demystify().StackTrace,
                       logOnly);
        }

        public void ClearMessages()
        {
            _messageList.Clear();
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void RaiseCollectionChangedEvent(NotifyCollectionChangedAction action, IList items)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, items));
        }
    }
}