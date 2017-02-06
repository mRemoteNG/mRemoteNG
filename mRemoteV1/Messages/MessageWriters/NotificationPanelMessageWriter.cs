using System;
using System.Windows.Forms;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Window;

namespace mRemoteNG.Messages.MessageWriters
{
    public class NotificationPanelMessageWriter : IMessageWriter
    {
        private readonly ErrorAndInfoWindow _messageWindow;

        public NotificationPanelMessageWriter(ErrorAndInfoWindow messageWindow)
        {
            if (messageWindow == null)
                throw new ArgumentNullException(nameof(messageWindow));

            _messageWindow = messageWindow;
        }

        public void Write(IMessage message)
        {
            var lvItem = new NotificationMessageListViewItem(message);
            AddToList(lvItem);
        }

        private void AddToList(ListViewItem lvItem)
        {
            if (_messageWindow.lvErrorCollector.InvokeRequired)
                _messageWindow.lvErrorCollector.Invoke((MethodInvoker) (() => AddToList(lvItem)));
            else
                _messageWindow.lvErrorCollector.Items.Insert(0, lvItem);
        }
    }
}