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
            if (_messageWindow.lvErrorCollector.IsDisposed)
                return;

            // If handle not yet created, force creation on the UI thread
            if (!_messageWindow.lvErrorCollector.IsHandleCreated)
            {
                if (_messageWindow.InvokeRequired)
                {
                    try
                    {
                        _messageWindow.Invoke((MethodInvoker)(() => EnsureHandleAndInsert(lvItem)));
                    }
                    catch (ObjectDisposedException) { return; }
                    catch (InvalidOperationException) { return; }
                    return;
                }

                // We're on the UI thread — force handle creation
                try { _ = _messageWindow.lvErrorCollector.Handle; }
                catch (ObjectDisposedException) { return; }
            }

            if (_messageWindow.lvErrorCollector.InvokeRequired)
            {
                try
                {
                    _messageWindow.lvErrorCollector.Invoke((MethodInvoker)(() => InsertItem(lvItem)));
                }
                catch (System.ComponentModel.InvalidAsynchronousStateException) { return; }
                catch (ObjectDisposedException) { return; }
                catch (InvalidOperationException) { return; }
            }
            else
            {
                InsertItem(lvItem);
            }
        }

        private void EnsureHandleAndInsert(ListViewItem lvItem)
        {
            if (_messageWindow.lvErrorCollector.IsDisposed) return;
            if (!_messageWindow.lvErrorCollector.IsHandleCreated)
            {
                try { _ = _messageWindow.lvErrorCollector.Handle; }
                catch (ObjectDisposedException) { return; }
            }
            InsertItem(lvItem);
        }

        private void InsertItem(ListViewItem lvItem)
        {
            if (_messageWindow.lvErrorCollector.IsDisposed) return;
            _messageWindow.lvErrorCollector.Items.Insert(0, lvItem);
            _messageWindow.pbError.Visible = true;
        }
    }
}
