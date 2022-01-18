using mRemoteNG.App;
using mRemoteNG.UI.Window;
using System;

namespace mRemoteNG.UI.Tabs
{
    class TabHelper
    {
        private static readonly Lazy<TabHelper> lazyHelper = new Lazy<TabHelper>(() => new TabHelper());

        public static TabHelper Instance => lazyHelper.Value;

        private TabHelper()
        {
        }

        private ConnectionTab currentTab;

        public ConnectionTab CurrentTab
        {
            get => currentTab;
            set
            {
                currentTab = value;
                findCurrentPanel();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                    "Tab got focused: " + currentTab.TabText);
            }
        }

        private void findCurrentPanel()
        {
            var currentForm = currentTab.Parent;
            while (currentForm != null && !(currentForm is ConnectionWindow))
            {
                currentForm = currentForm.Parent;
            }

            if (currentForm != null)
                CurrentPanel = (ConnectionWindow)currentForm;
        }

        private ConnectionWindow currentPanel;

        public ConnectionWindow CurrentPanel
        {
            get => currentPanel;
            set
            {
                currentPanel = value;
                //Disabled due to interaction with popups that would show this information and cause a softlock
                //Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                //                                    "Panel got focused: " + currentPanel.TabText);
            }
        }
    }
}