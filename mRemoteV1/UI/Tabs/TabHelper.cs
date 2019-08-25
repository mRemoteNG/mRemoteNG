using mRemoteNG.App;
using mRemoteNG.UI.Window;
using System;

namespace mRemoteNG.UI.Tabs
{
    public class TabHelper
    {
        private static readonly Lazy<TabHelper> lazyHelper = new Lazy<TabHelper>(() => new TabHelper());

        public static TabHelper Instance => lazyHelper.Value;

        /// <summary>
        /// Should focus events on a connection tab automatically focus
        /// its child connection?
        /// </summary>
        public bool FocusConnection { get; set; } = true;

        private TabHelper()
        {
        }

        private ConnectionTab currentTab;

        public ConnectionTab CurrentTab
        {
            get => currentTab;
            set
            {
                if (currentTab == value)
                {
                    Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, $"Tab already current: '{currentTab.TabText}'");
                    return;
                }

                currentTab = value;
                findCurrentPanel();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, $"Current tab changed: '{currentTab.TabText}'");
                RaiseActiveConnectionTabChangedEvent();
            }
        }

        private void findCurrentPanel()
        {
            var currentForm = currentTab.Parent;
            while (currentForm != null && !(currentForm is ConnectionWindow))
            {
                currentForm = currentForm.Parent;
            }

            if (currentForm != null && CurrentPanel != currentForm)
                CurrentPanel = (ConnectionWindow)currentForm;
        }

        private ConnectionWindow currentPanel;

        public ConnectionWindow CurrentPanel
        {
            get => currentPanel;
            set
            {
                if (currentPanel == value)
                    return;

                currentPanel = value;
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, $"Current panel changed: '{currentPanel.TabText}'");
                RaiseActivePanelChangedEvent();
            }
        }

        public event EventHandler ActivePanelChanged;
        protected virtual void RaiseActivePanelChangedEvent()
        {
            ActivePanelChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ActiveConnectionTabChanged;
        protected virtual void RaiseActiveConnectionTabChangedEvent()
        {
            ActiveConnectionTabChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TabClicked;
        public void RaiseTabClickedEvent()
        {
            TabClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}