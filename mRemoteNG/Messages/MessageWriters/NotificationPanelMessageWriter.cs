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
        private bool _handleEnsured;

        public void Write(IMessage message)
        {
            NotificationMessageListViewItem lvItem = new(message);

            AddToList(lvItem);
        }

        private void AddToList(ListViewItem lvItem)
        {
            // Ensure the handle is created on first use so early messages aren't dropped
            if (!_handleEnsured && !_messageWindow.lvErrorCollector.IsDisposed)
            {
                if (!_messageWindow.lvErrorCollector.IsHandleCreated)
                {
                    try { _ = _messageWindow.lvErrorCollector.Handle; } catch { }
                }
                _handleEnsured = true;
            }

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
                _messageWindow.lvErrorCollector.Items.Insert(0, lvItem);

                if (_messageWindow.lvErrorCollector.Items.Count > 0)
                {
                    _messageWindow.pbError.Visible = true;
                }
            }
        }
    }
}