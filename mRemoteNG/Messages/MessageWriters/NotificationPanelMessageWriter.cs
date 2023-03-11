using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.UI;
using mRemoteNG.UI.Window;

namespace mRemoteNG.Messages.MessageWriters
{
    [SupportedOSPlatform("windows")]
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
                _messageWindow.lvErrorCollector.Invoke((MethodInvoker)(() => AddToList(lvItem)));
            else
            {
                _messageWindow.lvErrorCollector.Items.Insert(0, lvItem);
                if (_messageWindow.lvErrorCollector.Items.Count > 0)
                    _messageWindow.pbError.Visible = true;
            }
        }
    }
}