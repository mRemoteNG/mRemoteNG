using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Tree;

namespace mRemoteNGTests.TestHelpers
{
    /// <summary>
    /// Captures and restores the state of all shared mutable singletons used by
    /// production code. Use in [SetUpFixture] or [SetUp]/[TearDown] to prevent
    /// state leaks between test fixtures running in the same testhost process.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class TestScope : IDisposable
    {
        private SecureString? _encryptionKey;
        private ConnectionTreeModel? _connectionTreeModel;
        private Dictionary<string, object?>? _defaultConnectionInfoSnapshot;
        private Dictionary<string, object?>? _defaultInheritanceSnapshot;

        // Cached reflection members
        private static readonly FieldInfo? s_connectionTreeModelField =
            typeof(ConnectionsService).GetField("<ConnectionTreeModel>k__BackingField",
                BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly PropertyInfo? s_connectionTreeModelProperty =
            typeof(ConnectionsService).GetProperty(nameof(ConnectionsService.ConnectionTreeModel));

        /// <summary>
        /// Creates a new TestScope and immediately captures the current singleton state.
        /// Dispose the returned object to restore the captured state.
        /// </summary>
        public static TestScope Begin()
        {
            var scope = new TestScope();
            scope.Capture();
            return scope;
        }

        private void Capture()
        {
            // 1. Runtime.EncryptionKey
            _encryptionKey = Runtime.EncryptionKey?.Copy();

            // 2. ConnectionTreeModel (private set — requires reflection)
            _connectionTreeModel = Runtime.ConnectionsService.ConnectionTreeModel;

            // 3. DefaultConnectionInfo.Instance — all serializable properties
            _defaultConnectionInfoSnapshot = SnapshotProperties(
                DefaultConnectionInfo.Instance,
                DefaultConnectionInfo.Instance.GetSerializableProperties());

            // 4. DefaultConnectionInheritance.Instance — all inheritable properties
            _defaultInheritanceSnapshot = SnapshotProperties(
                DefaultConnectionInheritance.Instance,
                ConnectionInfoInheritance.GetProperties());
        }

        public void Dispose()
        {
            Restore();
        }

        private void Restore()
        {
            // Restore in reverse capture order

            // 4. DefaultConnectionInheritance.Instance
            if (_defaultInheritanceSnapshot != null)
                RestoreProperties(DefaultConnectionInheritance.Instance, _defaultInheritanceSnapshot);

            // 3. DefaultConnectionInfo.Instance
            if (_defaultConnectionInfoSnapshot != null)
                RestoreProperties(DefaultConnectionInfo.Instance, _defaultConnectionInfoSnapshot);

            // 2. ConnectionTreeModel
            SetConnectionTreeModel(_connectionTreeModel);

            // 1. Runtime.EncryptionKey
            if (_encryptionKey != null)
                Runtime.EncryptionKey = _encryptionKey;

            // Always clear messages accumulated during tests
            Runtime.MessageCollector.ClearMessages();
        }

        private static Dictionary<string, object?> SnapshotProperties(
            object instance, IEnumerable<PropertyInfo> properties)
        {
            var snapshot = new Dictionary<string, object?>();
            foreach (var prop in properties)
            {
                if (!prop.CanRead) continue;
                snapshot[prop.Name] = prop.GetValue(instance);
            }
            return snapshot;
        }

        private static void RestoreProperties(
            object instance, Dictionary<string, object?> snapshot)
        {
            var type = instance.GetType();
            foreach (var kvp in snapshot)
            {
                var prop = type.GetProperty(kvp.Key);
                if (prop is { CanWrite: true })
                    prop.SetValue(instance, kvp.Value);
            }
        }

        private static void SetConnectionTreeModel(ConnectionTreeModel? model)
        {
            var service = Runtime.ConnectionsService;

            if (s_connectionTreeModelField != null)
            {
                s_connectionTreeModelField.SetValue(service, model);
            }
            else if (s_connectionTreeModelProperty != null)
            {
                var setter = s_connectionTreeModelProperty.GetSetMethod(true);
                setter?.Invoke(service, [model]);
            }
        }
    }
}
