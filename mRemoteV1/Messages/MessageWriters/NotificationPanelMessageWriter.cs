using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Messages.MessageWriters
{
    public class NotificationPanelMessageWriter : IMessageWriter
    {
        private readonly ErrorAndInfoWindow _messageWindow;
        private Timer _ecTimer;

        public bool PrintDebugMessages { get; set; } = true;
        public bool PrintInfoMessages { get; set; } = true;
        public bool PrintWarningMessages { get; set; } = true;
        public bool PrintErrorMessages { get; set; } = true;
        public bool FocusOnInfoMessages { get; set; } = true;
        public bool FocusOnWarningMessages { get; set; } = true;
        public bool FocusOnErrorMessages { get; set; } = true;

        public NotificationPanelMessageWriter(ErrorAndInfoWindow messageWindow)
        {
            if (messageWindow == null)
                throw new ArgumentNullException(nameof(messageWindow));

            _messageWindow = messageWindow;
            CreateTimer();
        }

        public void Print(IMessage message)
        {
            if (!WeShouldPrint(message))
                return;

            if (WeShouldFocusNotificationPanel(message))
                BeginSwitchToPanel();

            var lvItem = BuildListViewItem(message);
            AddToList(lvItem);
        }

        public void Print(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
                Print(message);
        }

        private bool WeShouldPrint(IMessage message)
        {
            if (message.OnlyLog)
                return false;

            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (PrintInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (PrintWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (PrintErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (PrintDebugMessages) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Class), message.Class, null);
            }
            return false;
        }

        private bool WeShouldFocusNotificationPanel(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (FocusOnInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (FocusOnWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (FocusOnErrorMessages) return true;
                    break;
            }
            return false;
        }

        private static ListViewItem BuildListViewItem(IMessage nMsg)
        {
            var lvItem = new ListViewItem
            {
                ImageIndex = Convert.ToInt32(nMsg.Class), Text = nMsg.Text.Replace(Environment.NewLine, "  "), Tag = nMsg
            };
            return lvItem;
        }

        private void CreateTimer()
        {
            _ecTimer = new Timer
            {
                Enabled = false, Interval = 300
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
            _messageWindow.PreviousActiveForm = (DockContent) frmMain.Default.pnlDock.ActiveContent;
            ShowMcForm();
            _messageWindow.lvErrorCollector.Focus();
            _messageWindow.lvErrorCollector.SelectedItems.Clear();
            _messageWindow.lvErrorCollector.Items[0].Selected = true;
            _messageWindow.lvErrorCollector.FocusedItem = _messageWindow.lvErrorCollector.Items[0];
        }

        private void ShowMcForm()
        {
            if (frmMain.Default.pnlDock.InvokeRequired)
                frmMain.Default.pnlDock.Invoke((MethodInvoker) ShowMcForm);
            else
                _messageWindow.Show(frmMain.Default.pnlDock);
        }

        private void AddToList(ListViewItem lvItem)
        {
            if (_messageWindow.lvErrorCollector.InvokeRequired)
                _messageWindow.lvErrorCollector.Invoke((MethodInvoker) (() => AddToList(lvItem)));
            else
                _messageWindow.lvErrorCollector.Items.Insert(0, lvItem);
        }
    }
}