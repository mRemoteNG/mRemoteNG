using mRemoteNG.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg, "Tab got focused: " + currentTab.TabText);
            }
        }
    }
}
