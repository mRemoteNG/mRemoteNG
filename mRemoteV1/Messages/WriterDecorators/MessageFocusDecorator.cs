using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Messages.WriterDecorators
{
    public class MessageFocusDecorator : IMessageWriter
    {
        private readonly IMessageTypeFilteringOptions _filter;
        private readonly IMessageWriter _decoratedWriter;
        private readonly ErrorAndInfoWindow _messageWindow;
        private readonly FrmMain _frmMain = FrmMain.Default;

        public MessageFocusDecorator(ErrorAndInfoWindow messageWindow, IMessageTypeFilteringOptions filter, IMessageWriter decoratedWriter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (messageWindow == null)
                throw new ArgumentNullException(nameof(messageWindow));
            if (decoratedWriter == null)
                throw new ArgumentNullException(nameof(decoratedWriter));

            _filter = filter;
            _messageWindow = messageWindow;
            _decoratedWriter = decoratedWriter;
        }

        public async void Write(IMessage message)
        {
            _decoratedWriter.Write(message);

            if (WeShouldFocusNotificationPanel(message))
                await SwitchToMessageAsync();
        }

        private bool WeShouldFocusNotificationPanel(IMessage message)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (_filter.AllowInfoMessages)
                        return true;
                    break;
                case MessageClass.WarningMsg:
                    if (_filter.AllowWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (_filter.AllowErrorMessages) return true;
                    break;
            }
            return false;
        }

        private async Task SwitchToMessageAsync()
        {
            await Task
                .Delay(TimeSpan.FromMilliseconds(300))
                .ContinueWith(task => SwitchToMessage());
        }

        private void SwitchToMessage()
        {
            if (_messageWindow.InvokeRequired)
            {
                _frmMain.Invoke((MethodInvoker)SwitchToMessage);
                return;
            }

            _messageWindow.PreviousActiveForm = (DockContent)_frmMain.pnlDock.ActiveContent;
            _messageWindow.Show(_frmMain.pnlDock);
            _messageWindow.lvErrorCollector.Focus();
            _messageWindow.lvErrorCollector.SelectedItems.Clear();
            _messageWindow.lvErrorCollector.Items[0].Selected = true;
            _messageWindow.lvErrorCollector.FocusedItem = _messageWindow.lvErrorCollector.Items[0];
        }
    }
}