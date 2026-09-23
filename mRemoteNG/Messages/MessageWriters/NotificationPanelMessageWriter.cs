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
            var lvItem = new NotificationMessageListViewItem(message);
            AddToList(lvItem);
        }

        private void AddToList(ListViewItem lvItem)
        {
            var lv = _messageWindow.lvErrorCollector;
            if (lv.IsDisposed) return;

            if (!EnsureHandleCreated())
                return;

            InvokeOnCorrectThread(() => InsertItem(lvItem));
        }

        private bool EnsureHandleCreated()
        {
            var lv = _messageWindow.lvErrorCollector;
            if (lv.IsHandleCreated) return true;

            // Handle must be created on the UI thread
            try
            {
                if (_messageWindow.InvokeRequired)
                    _messageWindow.Invoke((MethodInvoker)(() => { _ = lv.Handle; }));
                else
                    _ = lv.Handle;
                return true;
            }
            catch (ObjectDisposedException) { return false; }
            catch (InvalidOperationException) { return false; }
        }

        private void InvokeOnCorrectThread(Action action)
        {
            var lv = _messageWindow.lvErrorCollector;
            if (lv.IsDisposed) return;

            try
            {
                if (lv.InvokeRequired)
                    lv.Invoke((MethodInvoker)(() => action()));
                else
                    action();
            }
            catch (ObjectDisposedException) { /* Control disposed during shutdown — safe to ignore */ }
            catch (InvalidOperationException) { /* Handle no longer valid during shutdown — safe to ignore */ }
            catch (System.ComponentModel.InvalidAsynchronousStateException) { /* UI thread gone during shutdown — safe to ignore */ }
        }

        private void InsertItem(ListViewItem lvItem)
        {
            var lv = _messageWindow.lvErrorCollector;
            if (lv.IsDisposed) return;
            lv.Items.Insert(0, lvItem);
            _messageWindow.pbError.Visible = true;
        }
    }
}
