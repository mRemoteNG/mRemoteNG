using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.Messages.MessageFilteringOptions;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Messages.WriterDecorators
{
    [SupportedOSPlatform("windows")]
    public class MessageFocusDecorator : IMessageWriter
    {
        private readonly IMessageTypeFilteringOptions _filter;
        private readonly IMessageWriter _decoratedWriter;
        private readonly ErrorAndInfoWindow _messageWindow;
        private readonly FrmMain _frmMain = FrmMain.Default;

        public MessageFocusDecorator(ErrorAndInfoWindow messageWindow, IMessageTypeFilteringOptions filter, IMessageWriter decoratedWriter)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _messageWindow = messageWindow ?? throw new ArgumentNullException(nameof(messageWindow));
            _decoratedWriter = decoratedWriter ?? throw new ArgumentNullException(nameof(decoratedWriter));
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

            // do not attempt to focus the notification panel if it is in an inconsistent state
            if (_messageWindow.DockState == DockState.Unknown)
                return;

            _messageWindow.PreviousActiveForm = (DockContent)_frmMain.pnlDock.ActiveContent;

            // Show the notifications panel solution:
            // https://stackoverflow.com/questions/13843604/calling-up-dockpanel-suites-autohidden-dockcontent-programmatically
            if (AutoHideEnabled(_messageWindow))
                _frmMain.pnlDock.ActiveAutoHideContent = _messageWindow;
            else
                _messageWindow.Show(_frmMain.pnlDock);

            _messageWindow.lvErrorCollector.Focus();
            _messageWindow.lvErrorCollector.SelectedItems.Clear();
            _messageWindow.lvErrorCollector.Items[0].Selected = true;
            _messageWindow.lvErrorCollector.FocusedItem = _messageWindow.lvErrorCollector.Items[0];
        }

        private bool AutoHideEnabled(DockContent content)
        {
            return content.DockState == DockState.DockBottomAutoHide ||
                   content.DockState == DockState.DockTopAutoHide ||
                   content.DockState == DockState.DockLeftAutoHide ||
                   content.DockState == DockState.DockRightAutoHide;
        }
    }
}