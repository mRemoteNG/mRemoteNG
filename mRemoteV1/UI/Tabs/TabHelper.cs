using mRemoteNG.App;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using System;
using System.Windows.Forms;

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
                findCurrentWindow();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                    "Tab got focused: " + currentTab.TabText);
            }
        }

        private ConnectionWindow currentPanel;

        public ConnectionWindow CurrentPanel
        {
            get => currentPanel;
            set
            {
                currentPanel = value;
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                    "Panel got focused: " + currentPanel.TabText);
            }
        }

        private Form currentWindow;

        public Form CurrentWindow
        {
            get => currentPanel;
            set
            {
                currentWindow = value;
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                    "Window got focused: " + currentWindow);
            }
        }

        /// <summary>
        /// Finds the current ConnectionWindow that contains the ConnectionTab
        /// </summary>
        private void findCurrentPanel()
        {
            var currentForm = currentTab?.Parent;
            while (currentForm != null && !(currentForm is ConnectionWindow))
            {
                currentForm = currentForm.Parent;
            }

            if (currentForm != null)
                CurrentPanel = (ConnectionWindow)currentForm;
        }

        /// <summary>
        /// Find the current window for a given ConnectionWindow
        /// </summary>
        private void findCurrentWindow()
        {
            var currentForm = currentTab?.Parent;
            while (currentForm != null && !(currentForm is FloatWindowNG) && !(currentForm is FrmMain))
            {
                currentForm = currentForm.Parent;
            }

            if (currentForm != null)
                currentWindow = (Form)currentForm;
            if(currentForm is FloatWindowNG)
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                  "Focused on floating window");
            if (currentForm is FrmMain)
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                  "Focused on MainForm");
        }


    }
}