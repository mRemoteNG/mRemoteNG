using System;
using System.Windows.Forms;
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
            var lvItem = BuildListViewItem(message);
            AddToList(lvItem);
        }

        private static ListViewItem BuildListViewItem(IMessage nMsg)
        {
            var lvItem = new ListViewItem
            {
                ImageIndex = Convert.ToInt32(nMsg.Class), Text = nMsg.Text.Replace(Environment.NewLine, "  "), Tag = nMsg
            };
            return lvItem;
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