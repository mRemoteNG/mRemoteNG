using System;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.App.AgentControl
{
    /// <summary>Bounded, in-memory lifecycle receipts for completed read-only operations and previews.</summary>
    internal sealed class ControlOperationRegistry
    {
        private const int MaximumOperations = 512;
        private static readonly TimeSpan Retention = TimeSpan.FromMinutes(15);
        private readonly Dictionary<string, ControlOperation> _operations = new(StringComparer.Ordinal);
        private readonly object _sync = new();

        public ControlOperation RecordCompleted(string clientId, string grantId, string action, ControlTarget target,
            string resultCode, DateTime utcNow)
        {
            lock (_sync)
            {
                RemoveExpired(utcNow);
                while (_operations.Count >= MaximumOperations)
                {
                    string oldestId = _operations.Values.OrderBy(operation => operation.CompletedUtc)
                        .First().OperationId;
                    _operations.Remove(oldestId);
                }

                string operationId = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(24)).ToLowerInvariant();
                ControlOperation operation = new(operationId, clientId, grantId, action, target.StoreId, target.TargetId,
                    "completed", resultCode, false, utcNow, utcNow);
                _operations.Add(operationId, operation);
                return operation;
            }
        }

        public bool TryGet(string operationId, string clientId, string grantId, ControlTarget target,
            DateTime utcNow, out ControlOperation operation)
        {
            lock (_sync)
            {
                RemoveExpired(utcNow);
                if (_operations.TryGetValue(operationId ?? string.Empty, out operation) &&
                    string.Equals(operation.ClientId, clientId, StringComparison.Ordinal) &&
                    string.Equals(operation.GrantId, grantId, StringComparison.Ordinal) &&
                    string.Equals(operation.StoreId, target.StoreId, StringComparison.Ordinal) &&
                    string.Equals(operation.TargetId, target.TargetId, StringComparison.Ordinal))
                    return true;
                operation = null;
                return false;
            }
        }

        public void Clear()
        {
            lock (_sync) _operations.Clear();
        }

        private void RemoveExpired(DateTime utcNow)
        {
            foreach (string operationId in _operations.Values
                         .Where(operation => utcNow - operation.CompletedUtc >= Retention)
                         .Select(operation => operation.OperationId).ToArray())
                _operations.Remove(operationId);
        }
    }
}
