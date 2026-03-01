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
    public class MessageFocusDecorator(ErrorAndInfoWindow messageWindow, IMessageTypeFilteringOptions filter, IMessageWriter decoratedWriter) : IMessageWriter
    {
        private readonly IMessageTypeFilteringOptions _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        private readonly IMessageWriter _decoratedWriter = decoratedWriter ?? throw new ArgumentNullException(nameof(decoratedWriter));
        private readonly ErrorAndInfoWindow _messageWindow = messageWindow ?? throw new ArgumentNullException(nameof(messageWindow));
        private readonly FrmMain _frmMain = FrmMain.Default;

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
            // do not attempt to focus the notification panel if the application is closing
            if (_frmMain == null || _frmMain.IsClosing || !_frmMain.IsAccessible || _frmMain.IsDisposed)
            {
                return;
            }

            if (_messageWindow.InvokeRequired)
            {
                _frmMain.Invoke((MethodInvoker)SwitchToMessage);
                return;
            }

            // do not attempt to focus the notification panel if it is in an inconsistent state
            if (_messageWindow.DockState == DockState.Unknown)
                return;

            _messageWindow.PreviousActiveForm = (DockContent)_frmMain.pnlDock.ActiveContent;

            // Use Activate() which properly handles all dock states including auto-hide.
            // Previously, setting ActiveAutoHideContent for auto-hidden panels would open
            // the peek popup but subsequent Focus() calls could cause it to close again.
            // Activate() integrates with DockPanel's focus management correctly (#399).
            _messageWindow.Activate();

            _messageWindow.lvErrorCollector.Focus();
            _messageWindow.lvErrorCollector.SelectedItems.Clear();
            _messageWindow.lvErrorCollector.Items[0].Selected = true;
            _messageWindow.lvErrorCollector.FocusedItem = _messageWindow.lvErrorCollector.Items[0];
        }
    }
}