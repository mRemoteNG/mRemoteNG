using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using mRemoteNG.Connection;
using mRemoteNG.UI.Forms;
using mRemoteNG.Tree;

namespace mRemoteNG.App.AgentControl
{
    internal sealed record AgentControlTargetOption(string TargetId, string DisplayName, string Protocol)
    {
        public override string ToString() => $"{DisplayName} ({Protocol})";
    }

    /// <summary>Process-local opt-in coordinator. Nothing starts until the user enables it in the options UI.</summary>
    internal sealed class AgentControlRuntime
    {
        private readonly CapabilityGrantService _grants = new();
        private readonly ControlOperationRegistry _operations = new();
        private NamedPipeControlHost _host;
        private const string ActiveStoreId = "active";
        private static readonly ControlTarget GlobalTarget = new(string.Empty, string.Empty);

        public bool IsEnabled => _host?.IsRunning == true;
        public string PipeName { get; private set; } = string.Empty;

        public void EnableForCurrentSession()
        {
            if (IsEnabled) return;
            string pipeName = $"mRemoteNG.AgentControl.{Environment.ProcessId}.{System.Diagnostics.Process.GetCurrentProcess().SessionId}";
            PipeName = pipeName;
            ControlRequestProcessor processor = new(_grants, GetPolicyRevision, HandleReadOnlyAction, _operations);
            _host = new NamedPipeControlHost(pipeName, processor);
            _host.Start();
        }

        public async Task DisableAndRevokeAsync()
        {
            NamedPipeControlHost host = _host;
            _host = null;
            _grants.RevokeAll();
            _operations.Clear();
            if (host != null)
                await host.DisposeAsync().ConfigureAwait(false);
            PipeName = string.Empty;
        }

        public CapabilityGrant IssueActionsDiscoveryGrant()
        {
            if (!IsEnabled)
                throw new InvalidOperationException("Enable local control before granting access.");
            return _grants.Issue(GetCurrentClientId(), [ControlCapability.ActionsRead], [GlobalTarget],
                TimeSpan.FromMinutes(5), GetPolicyRevision(), DateTime.UtcNow);
        }

        public CapabilityGrant IssueScopedReadGrant(string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId) || targetId.Length > 128 || !TryGetConnection(ActiveStoreId, targetId, out _))
                throw new InvalidOperationException("Choose an existing connection target.");
            ControlTarget target = new(ActiveStoreId, targetId);
            return _grants.Issue(GetCurrentClientId(),
                [ControlCapability.ActionsRead, ControlCapability.ConnectionsRead, ControlCapability.ActionsPreview,
                    ControlCapability.OperationsRead], [GlobalTarget, target], TimeSpan.FromMinutes(5),
                GetPolicyRevision(), DateTime.UtcNow);
        }

        public static IReadOnlyList<AgentControlTargetOption> GetAvailableTargets()
        {
            // The options page calls this on its UI thread.
            ConnectionTreeModel model = Runtime.ConnectionsService?.ConnectionTreeModel;
            if (model == null)
                return Array.Empty<AgentControlTargetOption>();
            return model.GetRecursiveChildList()
                .Select(connection => new AgentControlTargetOption(connection.ConstantID,
                    new string((connection.Name ?? string.Empty).Where(character => !char.IsControl(character)).Take(80).ToArray()),
                    connection.Protocol.ToString()))
                .ToArray();
        }

        private static object HandleReadOnlyAction(ControlRequest request, ControlTarget target)
        {
            if (request.Action == ControlActionNames.ActionsList)
                return ActionRegistry.List();
            if (request.Action == ControlActionNames.ConnectionsList)
                return TryGetConnection(target.StoreId, target.TargetId, out SafeConnectionSummary connection)
                    ? connection
                    : new ControlError("TARGET_NOT_FOUND");
            if (request.Action == ControlActionNames.ActionsPreview)
            {
                string action = request.PreviewAction;
                if (action is "connections.open" or "connections.close")
                    return new ActionPreview(string.Empty, action, target.StoreId, target.TargetId, false,
                        "SIDE_EFFECT_REVIEW_REQUIRED", true, DateTime.UtcNow.AddMinutes(1));
                if (action == ControlActionNames.ConnectionsList)
                    return new ActionPreview(string.Empty, action, target.StoreId, target.TargetId, true,
                        "READ_ONLY", false, DateTime.UtcNow.AddMinutes(1));
                return new ControlError("UNKNOWN_ACTION");
            }
            return null;
        }

        private static bool TryGetConnection(string storeId, string targetId, out SafeConnectionSummary summary)
        {
            summary = null;
            if (storeId != ActiveStoreId || string.IsNullOrWhiteSpace(targetId))
                return false;
            summary = ReadInventory().FirstOrDefault(connection => connection.ConnectionId == targetId);
            return summary != null;
        }

        private static IReadOnlyList<SafeConnectionSummary> ReadInventory()
        {
            if (Runtime.ConnectionsService.ConnectionTreeModel == null)
                return Array.Empty<SafeConnectionSummary>();

            static IReadOnlyList<SafeConnectionSummary> ReadOnUiThread()
            {
                return Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveChildList()
                    .Select(connection => new SafeConnectionSummary(ActiveStoreId, connection.ConstantID,
                        connection.Protocol.ToString(), "Configured", [ControlActionNames.ConnectionsList]))
                    .ToArray();
            }

            try
            {
                FrmMain form = FrmMain.Default;
                if (form.IsHandleCreated && form.InvokeRequired)
                    return (IReadOnlyList<SafeConnectionSummary>)form.Invoke(new Func<IReadOnlyList<SafeConnectionSummary>>(ReadOnUiThread));
            }
            catch (InvalidOperationException)
            {
                // Startup/tests without a created main form use the current model directly.
            }
            return ReadOnUiThread();
        }

        private static long GetPolicyRevision()
        {
            ConnectionTreeModel model = Runtime.ConnectionsService.ConnectionTreeModel;
            if (model == null)
                return 0;
            StringBuilder fingerprint = new(model.GetHashCode().ToString(System.Globalization.CultureInfo.InvariantCulture));
            foreach (SafeConnectionSummary connection in ReadInventory().OrderBy(item => item.ConnectionId, StringComparer.Ordinal))
                fingerprint.Append('|').Append(connection.ConnectionId).Append(':').Append(connection.Protocol);
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(fingerprint.ToString()));
            return BitConverter.ToInt64(hash, 0) & long.MaxValue;
        }

        private static string GetCurrentClientId()
        {
            string sid = WindowsIdentity.GetCurrent().User?.Value ?? throw new InvalidOperationException("Windows user identity is unavailable.");
            int sessionId = System.Diagnostics.Process.GetCurrentProcess().SessionId;
            return $"{sid}|{sessionId}";
        }

        internal static string GetCurrentClientIdForTesting() => GetCurrentClientId();
    }
}
