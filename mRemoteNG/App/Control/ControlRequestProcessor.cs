using System;
using System.Text.Json;

namespace mRemoteNG.App.AgentControl
{
    internal sealed record ControlResponse(int SchemaVersion, string RequestId, object? Result, ControlError? Error);
    internal sealed record ControlActionResult(object Data, ControlOperation Operation);

    /// <summary>Applies schema, allowlist and ephemeral grant policy before calling a read-only adapter.</summary>
    internal sealed class ControlRequestProcessor
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = false };
        private readonly CapabilityGrantService _grants;
        private readonly ControlOperationRegistry _operations;
        private readonly Func<long> _getPolicyRevision;
        private readonly Func<ControlRequest, ControlTarget, object?> _readOnlyHandler;

        public ControlRequestProcessor(CapabilityGrantService grants,
            Func<long> getPolicyRevision, Func<ControlRequest, ControlTarget, object?> readOnlyHandler,
            ControlOperationRegistry operations = null)
        {
            _grants = grants ?? throw new ArgumentNullException(nameof(grants));
            _operations = operations ?? new ControlOperationRegistry();
            _getPolicyRevision = getPolicyRevision ?? throw new ArgumentNullException(nameof(getPolicyRevision));
            _readOnlyHandler = readOnlyHandler ?? throw new ArgumentNullException(nameof(readOnlyHandler));
        }

        public byte[] Process(ReadOnlySpan<byte> requestBytes, string authenticatedClientId, DateTime utcNow)
        {
            string requestId = string.Empty;
            try
            {
                if (requestBytes.Length == 0 || requestBytes.Length > ControlSchema.MaximumRequestBytes)
                    return Serialize(Failure(requestId, ControlError.InvalidRequest));

                ControlRequest request = JsonSerializer.Deserialize<ControlRequest>(requestBytes, JsonOptions);
                if (request == null)
                    return Serialize(Failure(requestId, ControlError.InvalidRequest));
                requestId = request.RequestId ?? string.Empty;
                ControlError validationError = ControlRequestValidator.Validate(request);
                if (validationError != null)
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, validationError));

                return Serialize(ProcessValidatedRequest(request, authenticatedClientId, utcNow));
            }
            catch (JsonException)
            {
                return Serialize(Failure(requestId, ControlError.InvalidRequest));
            }
            catch
            {
                // Do not return exception messages or request content to an untrusted caller.
                return Serialize(Failure(requestId, "INTERNAL_ERROR"));
            }
        }

        private ControlResponse ProcessValidatedRequest(ControlRequest request, string clientId, DateTime utcNow)
        {
            if (!ActionRegistry.TryGet(request.Action, out ControlActionDescriptor action))
                return Failure(request.RequestId, "UNKNOWN_ACTION");
            if (string.IsNullOrWhiteSpace(request.GrantId))
                return Failure(request.RequestId, "ACCESS_DENIED");

            ControlTarget target = request.Action == ControlActionNames.ActionsList
                ? new ControlTarget(string.Empty, string.Empty)
                : new ControlTarget(request.StoreId ?? string.Empty, request.TargetId ?? string.Empty);
            if (!IsValidTarget(request.Action, target))
                return Failure(request.RequestId, ControlError.InvalidRequest);

            long policyRevision = _getPolicyRevision();
            if (!TryGetCapability(action.Capability, out ControlCapability capability) ||
                !_grants.IsAuthorized(request.GrantId, clientId, capability, target, policyRevision, utcNow))
                return Failure(request.RequestId, "ACCESS_DENIED");

            return request.Action == ControlActionNames.OperationsGet
                ? ReadOperation(request, clientId, target, policyRevision, utcNow)
                : ExecuteReadOnly(request, clientId, target, policyRevision, utcNow);
        }

        private static bool IsValidTarget(string action, ControlTarget target)
        {
            if (target.StoreId.Length > 0 && target.TargetId.Length > 0)
                return true;
            return target.StoreId.Length == 0 && target.TargetId.Length == 0 &&
                (action == ControlActionNames.ActionsList || action == ControlActionNames.OperationsGet);
        }

        private ControlResponse ReadOperation(ControlRequest request, string clientId, ControlTarget target,
            long policyRevision, DateTime utcNow)
        {
            if (string.IsNullOrWhiteSpace(request.OperationId))
                return Failure(request.RequestId, ControlError.InvalidRequest);
            if (!_operations.TryGet(request.OperationId, clientId, request.GrantId, target, utcNow,
                    out ControlOperation operation))
                return Failure(request.RequestId, "OPERATION_NOT_FOUND");
            return policyRevision == _getPolicyRevision()
                ? Success(request.RequestId, operation)
                : Failure(request.RequestId, "STALE_POLICY");
        }

        private ControlResponse ExecuteReadOnly(ControlRequest request, string clientId, ControlTarget target,
            long policyRevision, DateTime utcNow)
        {
            if (request.Action == ControlActionNames.ActionsPreview && string.IsNullOrWhiteSpace(request.PreviewAction))
                return Failure(request.RequestId, ControlError.InvalidRequest);

            object result = _readOnlyHandler(request, target);
            if (result == null)
                return Failure(request.RequestId, "UNSUPPORTED_ACTION");
            if (result is ControlError handlerError)
                return new ControlResponse(ControlSchema.Version, request.RequestId, null, handlerError);
            if (policyRevision != _getPolicyRevision())
                return Failure(request.RequestId, "STALE_POLICY");
            ControlOperation recorded = _operations.RecordCompleted(clientId, request.GrantId,
                request.Action, target, request.Action == ControlActionNames.ActionsPreview ? "BLOCKED" : "SUCCESS", utcNow);
            if (result is ActionPreview preview)
                result = preview with { PreviewId = recorded.OperationId };
            return Success(request.RequestId, new ControlActionResult(result, recorded));
        }

        private static ControlResponse Failure(string requestId, string code) =>
            new(ControlSchema.Version, requestId, null, new ControlError(code));

        private static ControlResponse Success(string requestId, object result) =>
            new(ControlSchema.Version, requestId, result, null);

        private static bool TryGetCapability(string name, out ControlCapability capability)
        {
            capability = name switch
            {
                "actions.read" => ControlCapability.ActionsRead,
                "connections.read" => ControlCapability.ConnectionsRead,
                "sessions.read" => ControlCapability.SessionsRead,
                "actions.preview" => ControlCapability.ActionsPreview,
                "operations.read" => ControlCapability.OperationsRead,
                _ => (ControlCapability)(-1)
            };
            return Enum.IsDefined(capability);
        }

        private static byte[] Serialize(ControlResponse response)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(response, JsonOptions);
            if (bytes.Length <= ControlSchema.MaximumResponseBytes)
                return bytes;
            return JsonSerializer.SerializeToUtf8Bytes(Failure(response.RequestId, "RESPONSE_TOO_LARGE"), JsonOptions);
        }
    }
}
