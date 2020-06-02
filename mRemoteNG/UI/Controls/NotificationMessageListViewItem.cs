using System;
using System.Windows.Forms;
using mRemoteNG.Messages;

namespace mRemoteNG.UI.Controls
{
    public class NotificationMessageListViewItem : ListViewItem
    {
        public NotificationMessageListViewItem(IMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            ImageIndex = Convert.ToInt32(message.Class);
            Text = message.Text.Replace(Environment.NewLine, "  ");
            Tag = message;
        }
    }
}