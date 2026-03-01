using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.UI;
using mRemoteNG.UI.Window;

namespace mRemoteNG.Messages.MessageWriters
{
    [SupportedOSPlatform("windows")]
    public class NotificationPanelMessageWriter(ErrorAndInfoWindow messageWindow) : IMessageWriter
    {
        private readonly ErrorAndInfoWindow _messageWindow = messageWindow ?? throw new ArgumentNullException(nameof(messageWindow));

        public void Write(IMessage message)
        {
            NotificationMessageListViewItem lvItem = new(message);

            AddToList(lvItem);
        }

        private void AddToList(ListViewItem lvItem)
        {
            // Check if the control is disposed or handle not created (during shutdown)
            if (_messageWindow.lvErrorCollector.IsDisposed || !_messageWindow.lvErrorCollector.IsHandleCreated)
            {
                return;
            }

            if (_messageWindow.lvErrorCollector.InvokeRequired)
            {
                try
                {
                    _messageWindow.lvErrorCollector.Invoke((MethodInvoker)(() => AddToList(lvItem)));
                }
                catch (System.ComponentModel.InvalidAsynchronousStateException)
                {
                    // Destination thread no longer exists (application shutting down)
                    return;
                }
                catch (ObjectDisposedException)
                {
                    // Control has been disposed (application shutting down)
                    return;
                }
                catch (InvalidOperationException)
                {
                    // Control handle no longer exists or other invalid operation (application shutting down)
                    return;
                }
            }
            else
            {
                _messageWindow.AddMessage(lvItem);
            }
        }
    }
}