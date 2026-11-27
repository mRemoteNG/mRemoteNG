using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;

namespace mRemoteNG.App.AgentControl
{
    internal enum ControlCapability
    {
        ActionsRead,
        ConnectionsRead,
        SessionsRead,
        ActionsPreview,
        OperationsRead
    }

    internal sealed record ControlTarget(string StoreId, string TargetId);

    internal sealed record CapabilityGrant(string GrantId, string ClientId, IReadOnlySet<ControlCapability> Capabilities,
        IReadOnlySet<ControlTarget> Targets, DateTime ExpiresUtc, long PolicyRevision);

    /// <summary>In-memory grant checks. Grants are not persisted and must be issued by an explicit UI approval flow.</summary>
    internal sealed class CapabilityGrantService
    {
        private readonly Dictionary<string, CapabilityGrant> _grants = new(StringComparer.Ordinal);
        private readonly Lock _sync = new();

        public CapabilityGrant Issue(string clientId, IEnumerable<ControlCapability> capabilities,
            IEnumerable<ControlTarget> targets, TimeSpan lifetime, long policyRevision, DateTime utcNow)
        {
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException("Client identity is required.", nameof(clientId));
            if (lifetime <= TimeSpan.Zero || lifetime > TimeSpan.FromMinutes(15))
                throw new ArgumentOutOfRangeException(nameof(lifetime));
            if (policyRevision < 0) throw new ArgumentOutOfRangeException(nameof(policyRevision));

            CapabilityGrant grant = new(Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant(), clientId,
                new HashSet<ControlCapability>(capabilities ?? throw new ArgumentNullException(nameof(capabilities))),
                new HashSet<ControlTarget>(targets ?? throw new ArgumentNullException(nameof(targets))),
                utcNow.Add(lifetime), policyRevision);
            lock (_sync) _grants.Add(grant.GrantId, grant);
            return grant;
        }

        public bool IsAuthorized(string grantId, string clientId, ControlCapability capability,
            ControlTarget target, long policyRevision, DateTime utcNow)
        {
            lock (_sync)
            {
                if (!_grants.TryGetValue(grantId ?? string.Empty, out CapabilityGrant grant)) return false;
                return string.Equals(grant.ClientId, clientId, StringComparison.Ordinal) &&
                       grant.ExpiresUtc > utcNow && grant.PolicyRevision == policyRevision &&
                       grant.Capabilities.Contains(capability) && grant.Targets.Contains(target);
            }
        }

        public void Revoke(string grantId)
        {
            lock (_sync) _grants.Remove(grantId ?? string.Empty);
        }

        public void RevokeAll()
        {
            lock (_sync) _grants.Clear();
        }
    }
}
