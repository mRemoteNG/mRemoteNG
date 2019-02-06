using mRemoteNG.App;
using mRemoteNG.UI.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    class TabHelper
    { 
        private static readonly Lazy<TabHelper> lazyHelper= new Lazy<TabHelper>(() => new TabHelper());

        public static TabHelper Instance { get { return lazyHelper.Value; } }
          
        private TabHelper()
        {
            
        }
        private ConnectionTab currentTab;
        public ConnectionTab CurrentTab
        {
            get
            {
                return currentTab;
            }
            set
            {
                currentTab = value;
                findCurrentPanel();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, "Tab got focused: " + currentTab.TabText);
            }
        }
        private void findCurrentPanel()
        {
            var currentForm = currentTab.Parent;
            while (currentForm != null && ! (currentForm is ConnectionWindow))
            {
                currentForm = currentForm.Parent;
            }
            if (currentForm != null && currentForm is ConnectionWindow)
                CurrentPanel = (ConnectionWindow)currentForm;
        }
        private ConnectionWindow currentPanel;
        public ConnectionWindow CurrentPanel
        {
            get
            {
                return currentPanel;
            }
            set
            {
                currentPanel = value;
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, "Panel got focused: " + currentPanel.TabText);
            }
        }

    }
}
