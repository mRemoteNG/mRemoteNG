using System;
using System.Globalization;
using System.Windows.Forms;
using mRemoteNG.Messages;

namespace mRemoteNG.UI
{
    public class NotificationMessageListViewItem : ListViewItem
    {
        public NotificationMessageListViewItem(IMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            ImageIndex = Convert.ToInt32(message.Class, CultureInfo.InvariantCulture);
            Text = message.Text.Replace(Environment.NewLine, "  ");
            Tag = message;
        }
    }
}