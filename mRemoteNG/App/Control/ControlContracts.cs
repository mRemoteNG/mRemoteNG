using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace mRemoteNG.App.AgentControl
{
    public static class ControlSchema
    {
        public const int Version = 1;
        public const int MaximumRequestBytes = 16 * 1024;
        public const int MaximumResponseBytes = 256 * 1024;
    }

    public static class ControlActionNames
    {
        public const string ActionsList = "actions.list";
        public const string ConnectionsList = "connections.list";
        public const string SessionsList = "sessions.list";
        public const string ActionsPreview = "actions.preview";
        public const string OperationsGet = "operations.get";
    }

    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    public sealed record ControlRequest(int SchemaVersion, string RequestId, string Action, string? GrantId = null,
        string? StoreId = null, string? TargetId = null, string? PreviewAction = null, string? OperationId = null);

    public sealed record ControlActionDescriptor(string Name, string Capability, bool ReadOnly);

    /// <summary>Default inventory projection; deliberately excludes labels, addresses and credentials.</summary>
    public sealed record SafeConnectionSummary(string StoreId, string ConnectionId, string Protocol, string State,
        IReadOnlyList<string> AllowedActions);

    public sealed record SafeSessionSummary(string SessionId, string ConnectionId, string Protocol, string State);

    public sealed record ActionPreview(string PreviewId, string Action, string StoreId, string TargetId,
        bool Allowed, string ReasonCode, bool HasSideEffects, DateTime ExpiresUtc);

    public sealed record ControlOperation(string OperationId, string ClientId, string GrantId, string Action,
        string StoreId, string TargetId, string State, string ResultCode, bool ChangedState, DateTime CreatedUtc,
        DateTime CompletedUtc);

    public sealed record ControlError(string Code)
    {
        public const string InvalidRequest = "INVALID_REQUEST";
    }

    internal static class ControlRequestValidator
    {
        public static ControlError? Validate(ControlRequest request)
        {
            if (request == null)
                return new ControlError(ControlError.InvalidRequest);
            if (request.SchemaVersion != ControlSchema.Version)
                return new ControlError("UNSUPPORTED_SCHEMA");
            if (string.IsNullOrWhiteSpace(request.RequestId) || request.RequestId.Length > 128 ||
                string.IsNullOrWhiteSpace(request.Action) || request.Action.Length > 64)
                return new ControlError(ControlError.InvalidRequest);
            if (request.GrantId?.Length > 128 || request.StoreId?.Length > 128 || request.TargetId?.Length > 128)
                return new ControlError(ControlError.InvalidRequest);
            if (request.PreviewAction?.Length > 64 || request.OperationId?.Length > 128)
                return new ControlError(ControlError.InvalidRequest);
            return null;
        }
    }
}
