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
        private readonly ActionRegistry _actions;
        private readonly CapabilityGrantService _grants;
        private readonly ControlOperationRegistry _operations;
        private readonly Func<long> _getPolicyRevision;
        private readonly Func<ControlRequest, ControlTarget, object?> _readOnlyHandler;

        public ControlRequestProcessor(ActionRegistry actions, CapabilityGrantService grants,
            Func<long> getPolicyRevision, Func<ControlRequest, ControlTarget, object?> readOnlyHandler,
            ControlOperationRegistry operations = null)
        {
            _actions = actions ?? throw new ArgumentNullException(nameof(actions));
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
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("INVALID_REQUEST")));

                ControlRequest request = JsonSerializer.Deserialize<ControlRequest>(requestBytes, JsonOptions);
                requestId = request?.RequestId ?? string.Empty;
                ControlError validationError = ControlRequestValidator.Validate(request);
                if (validationError != null)
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, validationError));

                if (!_actions.TryGet(request.Action, out ControlActionDescriptor action))
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("UNKNOWN_ACTION")));

                if (string.IsNullOrWhiteSpace(request.GrantId))
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("ACCESS_DENIED")));

                ControlTarget target = request.Action == ControlActionNames.ActionsList
                    ? new ControlTarget(string.Empty, string.Empty)
                    : new ControlTarget(request.StoreId ?? string.Empty, request.TargetId ?? string.Empty);
                bool isGlobalOperationLookup = request.Action == ControlActionNames.OperationsGet &&
                                                target.StoreId.Length == 0 && target.TargetId.Length == 0;
                if (target.StoreId.Length == 0 && request.Action != ControlActionNames.ActionsList && !isGlobalOperationLookup ||
                    target.TargetId.Length == 0 && request.Action != ControlActionNames.ActionsList && !isGlobalOperationLookup)
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("INVALID_REQUEST")));

                long policyRevision = _getPolicyRevision();
                if (!TryGetCapability(action.Capability, out ControlCapability capability) ||
                    !_grants.IsAuthorized(request.GrantId, authenticatedClientId, capability, target, policyRevision, utcNow))
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("ACCESS_DENIED")));

                if (request.Action == ControlActionNames.OperationsGet)
                {
                    if (string.IsNullOrWhiteSpace(request.OperationId))
                        return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("INVALID_REQUEST")));
                    if (!_operations.TryGet(request.OperationId, authenticatedClientId, request.GrantId, target, utcNow,
                            out ControlOperation operation))
                        return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("OPERATION_NOT_FOUND")));
                    return policyRevision == _getPolicyRevision()
                        ? Serialize(new ControlResponse(ControlSchema.Version, requestId, operation, null))
                        : Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("STALE_POLICY")));
                }

                if (request.Action == ControlActionNames.ActionsPreview && string.IsNullOrWhiteSpace(request.PreviewAction))
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("INVALID_REQUEST")));

                object result = _readOnlyHandler(request, target);
                if (result == null)
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("UNSUPPORTED_ACTION")));
                if (result is ControlError handlerError)
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, handlerError));
                if (policyRevision != _getPolicyRevision())
                    return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("STALE_POLICY")));
                ControlOperation recorded = _operations.RecordCompleted(authenticatedClientId, request.GrantId,
                    request.Action, target, request.Action == ControlActionNames.ActionsPreview ? "BLOCKED" : "SUCCESS", utcNow);
                if (result is ActionPreview preview)
                    result = preview with { PreviewId = recorded.OperationId };
                return Serialize(new ControlResponse(ControlSchema.Version, requestId,
                    new ControlActionResult(result, recorded), null));
            }
            catch (JsonException)
            {
                return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("INVALID_REQUEST")));
            }
            catch
            {
                // Deliberately do not return exception messages or request content to an untrusted caller.
                return Serialize(new ControlResponse(ControlSchema.Version, requestId, null, new ControlError("INTERNAL_ERROR")));
            }
        }

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
            return JsonSerializer.SerializeToUtf8Bytes(new ControlResponse(ControlSchema.Version, response.RequestId,
                null, new ControlError("RESPONSE_TOO_LARGE")), JsonOptions);
        }
    }
}
