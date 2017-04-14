using System;
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
        private Timer _ecTimer;

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
            CreateTimer();
        }

        public void Write(IMessage message)
        {
            if (WeShouldFocusNotificationPanel(message))
                BeginSwitchToPanel();
            _decoratedWriter.Write(message);
        }

        private bool WeShouldFocusNotificationPanel(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (_filter.AllowInfoMessages) return true;
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

        private void CreateTimer()
        {
            _ecTimer = new Timer
            {
                Enabled = false,
                Interval = 300
            };
            _ecTimer.Tick += SwitchTimerTick;
        }

        private void BeginSwitchToPanel()
        {
            _ecTimer.Enabled = true;
        }

        private void SwitchTimerTick(object sender, EventArgs e)
        {
            SwitchToMessage();
            _ecTimer.Enabled = false;
        }

        private void SwitchToMessage()
        {
            _messageWindow.PreviousActiveForm = (DockContent)FrmMain.Default.pnlDock.ActiveContent;
            ShowMcForm();
            _messageWindow.lvErrorCollector.Focus();
            _messageWindow.lvErrorCollector.SelectedItems.Clear();
            _messageWindow.lvErrorCollector.Items[0].Selected = true;
            _messageWindow.lvErrorCollector.FocusedItem = _messageWindow.lvErrorCollector.Items[0];
        }

        private void ShowMcForm()
        {
            if (FrmMain.Default.pnlDock.InvokeRequired)
                FrmMain.Default.pnlDock.Invoke((MethodInvoker)ShowMcForm);
            else
                _messageWindow.Show(FrmMain.Default.pnlDock);
        }
    }
}