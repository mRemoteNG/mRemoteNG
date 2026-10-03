using System;
using System.Windows.Forms;
using mRemoteNG.Messages;

namespace mRemoteNG.UI
{
    public class NotificationMessageListViewItem : ListViewItem
    {
        public NotificationMessageListViewItem(IMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            ImageIndex = Convert.ToInt32(message.Class);
            Text = $"[{message.Date:HH:mm:ss}] {message.Text.Replace(Environment.NewLine, "  ")}";
            Tag = message;
        }
    }
}