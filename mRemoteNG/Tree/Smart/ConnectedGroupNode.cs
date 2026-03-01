using System.ComponentModel;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.Tree.Smart
{
    [SupportedOSPlatform("windows")]
    public class ConnectedGroupNode : ContainerInfo
    {
        public ConnectedGroupNode()
        {
            Name = "Connected";
            IsExpanded = true;
        }

        public void Initialize()
        {
            var model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model != null)
            {
                // Subscribe to property changes to detect connection status changes
                model.PropertyChanged += OnModelPropertyChanged;
                
                // Initial population
                RefreshList();
            }
            
            // Also listen if model changes (ConnectionsLoaded event in Runtime?)
            Runtime.ConnectionsService.ConnectionsLoaded += OnConnectionsLoaded;
        }

        private void OnConnectionsLoaded(object? sender, ConnectionsLoadedEventArgs e)
        {
             // Re-subscribe if model changed
             if (e.NewConnectionTreeModel != null)
             {
                 e.NewConnectionTreeModel.PropertyChanged -= OnModelPropertyChanged; // Safety
                 e.NewConnectionTreeModel.PropertyChanged += OnModelPropertyChanged;
                 RefreshList();
             }
        }

        private void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
             // We are looking for OpenConnections changes on ConnectionInfo objects
             if (sender is ConnectionInfo ci && e.PropertyName == nameof(ConnectionInfo.OpenConnections))
             {
                 // Check if it's already in our list (to avoid infinite loop if SmartConnectionInfo triggers this)
                 // SmartConnectionInfo wraps ConnectionInfo.
                 // The sender is the ORIGINAL ConnectionInfo (or SmartConnectionInfo).
                 // If sender is SmartConnectionInfo, we ignore it (it's our child).
                 if (ci is SmartConnectionInfo) return;

                 UpdateConnection(ci);
             }
        }

        private void RefreshList()
        {
            // Clear children (careful: do not dispose SmartConnectionInfo if we want to reuse? 
            // For now, recreate. SmartConnectionInfo doesn't hold heavy resources other than event subscription).
            // We should unsubscribe SmartConnectionInfo from original when removing!
            // SmartConnectionInfo needs Dispose or we rely on GC (event reference might keep it alive).
            // Since SmartConnectionInfo subscribes to _original, _original keeps SmartConnectionInfo alive!
            // So we MUST Unsubscribe.
            // I'll add Dispose to SmartConnectionInfo later or handle it here.
            
            // Better: Scan all connections and update.
            var model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null) return;

            var allConnections = model.GetRecursiveChildList();
            
            // Remove those that are no longer connected
            var toRemove = Children.OfType<SmartConnectionInfo>()
                .Where(s => s.GetOriginal().OpenConnections.Count == 0 || !allConnections.Contains(s.GetOriginal()))
                .ToList();
            
            foreach (var item in toRemove)
            {
                RemoveChild(item);
            }

            // Add those that are connected and not in list
            var connected = allConnections.Where(c => c.OpenConnections.Count > 0 && !(c is SmartConnectionInfo));
            foreach (var c in connected)
            {
                if (!Children.OfType<SmartConnectionInfo>().Any(s => s.GetOriginal() == c))
                {
                    AddChild(new SmartConnectionInfo(c));
                }
            }
        }
        
        private void UpdateConnection(ConnectionInfo ci)
        {
            bool isConnected = ci.OpenConnections.Count > 0;
            var existing = Children.OfType<SmartConnectionInfo>().FirstOrDefault(s => s.GetOriginal() == ci);
            
            if (isConnected)
            {
                if (existing == null)
                {
                    AddChild(new SmartConnectionInfo(ci));
                }
            }
            else
            {
                if (existing != null)
                {
                    RemoveChild(existing);
                }
            }
        }
    }
}
