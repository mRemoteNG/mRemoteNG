using System.ComponentModel;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.Tree.Smart
{
    [SupportedOSPlatform("windows")]
    public class SmartConnectionInfo : ConnectionInfo
    {
        private readonly ConnectionInfo _original;

        public SmartConnectionInfo(ConnectionInfo original)
        {
            _original = original;
            // Copy initial state
            CopyFrom(_original);
            
            // Share the OpenConnections list so status is shared
            OpenConnections = _original.OpenConnections;

            // Subscribe to original events to sync changes
            _original.PropertyChanged += OnOriginalPropertyChanged;
        }

        private void OnOriginalPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName)) return;

            // Sync property if needed
            // For simple properties, CopyFrom might be overkill or recursive if not careful.
            // But since we are a separate instance, updating our property is fine.
            
            // We can't easily know which property changed value without reflection or switch.
            // But we can just use CopyFrom for everything, or let the getter handle it?
            // No, properties are stored in fields in base class. We must update them.
            
            // Optimization: Only update the specific property?
            // AbstractConnectionRecord doesn't expose a "SetProperty" by name easily.
            // But it has `GetPropertyValue` / `SetField`.
            
            // Easier approach: Re-copy everything on any change? Might be slow.
            // But PropertyChanged usually happens for one property.
            
            // Let's try to just CopyFrom for now. It copies all fields.
            // Check if CopyFrom triggers PropertyChanged on 'this'.
            // AbstractConnectionRecord.CopyFrom uses setters, so yes, it triggers PropertyChanged.
            // This is what we want!
            
            // BUT: Avoid infinite loops if we were somehow bound two-way (we are not).
            
            // One caveat: CopyFrom might create a NEW OpenConnections list.
            // We must restore the shared one if CopyFrom overwrites it.
            var sharedConnections = OpenConnections;
            CopyFrom(_original);
            OpenConnections = sharedConnections; // Restore shared list
        }

        public ConnectionInfo GetOriginal()
        {
            return _original;
        }
    }
}
