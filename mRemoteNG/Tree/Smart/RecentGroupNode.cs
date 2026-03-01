using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.Tree.Smart
{
    [SupportedOSPlatform("windows")]
    public class RecentGroupNode : ContainerInfo
    {
        public RecentGroupNode()
        {
            Name = "Recent";
            IsExpanded = true;
        }

        public void Initialize()
        {
            RecentConnectionsService.Instance.RecentConnectionsChanged += OnRecentConnectionsChanged;
            RefreshList();
        }

        private void OnRecentConnectionsChanged(object? sender, EventArgs e)
        {
            // Must invoke on UI thread if needed? 
            // RecentConnectionsService raises event.
            // If we manipulate Children, CollectionChanged fires -> UI update.
            // This usually happens on UI thread if triggered by UI action.
            RefreshList();
        }

        private void RefreshList()
        {
            var recents = RecentConnectionsService.Instance.GetRecentConnections();
            
            // Rebuild list to match order
            Children.Clear();
            
            foreach (var c in recents)
            {
                if (c == null) continue;
                AddChild(new SmartConnectionInfo(c));
            }
        }
    }
}
