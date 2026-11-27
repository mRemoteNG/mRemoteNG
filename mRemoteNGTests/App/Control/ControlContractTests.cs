using System;
using System.Text.Json;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using mRemoteNG.App.AgentControl;
using NUnit.Framework;

namespace mRemoteNGTests.App.Control
{
    [TestFixture]
    public class ControlContractTests
    {
        [Test]
        public void Registry_ExposesOnlyAllowlistedReadOnlyActions()
        {
            Assert.That(ActionRegistry.List(), Has.Count.EqualTo(4));
            Assert.That(ActionRegistry.List(), Has.All.Property(nameof(ControlActionDescriptor.ReadOnly)).True);
            Assert.That(ActionRegistry.TryGet("connection.connect", out _), Is.False);
        }

        [Test]
        public void Request_RejectsUnknownJsonFields()
        {
            const string request = "{\"SchemaVersion\":1,\"RequestId\":\"r1\",\"Action\":\"actions.list\",\"Shell\":\"whoami\"}";

            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ControlRequest>(request));
        }

        [TestCase("null", ControlError.InvalidRequest)]
        [TestCase("{}", "UNSUPPORTED_SCHEMA")]
        [TestCase("{\"SchemaVersion\":1,\"RequestId\":\"r1\",\"Action\":null}", ControlError.InvalidRequest)]
        public void Processor_RejectsNullOrIncompleteRequestBeforeCallingAdapter(string json, string expectedError)
        {
            int calls = 0;
            ControlRequestProcessor processor = new(new CapabilityGrantService(), () => 1,
                (_, _) => { calls++; return new { safe = true }; });

            using JsonDocument response = JsonDocument.Parse(processor.Process(
                Encoding.UTF8.GetBytes(json), "synthetic-client", DateTime.UtcNow));

            Assert.That(response.RootElement.GetProperty("Error").GetProperty("Code").GetString(),
                Is.EqualTo(expectedError));
            Assert.That(calls, Is.Zero);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task NamedPipeHost_DisposeBeforeOrImmediatelyAfterStart_CompletesShutdown(bool start)
        {
            ControlRequestProcessor processor = new(new CapabilityGrantService(), () => 1, (_, _) => null);
            NamedPipeControlHost host = new("mRemoteNG-test-" + Guid.NewGuid().ToString("N"), processor);
            if (start)
                host.Start();

            await host.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));

            Assert.That(host.IsRunning, Is.False);
        }

        [Test]
        public void Grant_IsBoundToClientTargetPolicyAndExpiry()
        {
            CapabilityGrantService service = new();
            DateTime now = DateTime.UtcNow;
            ControlTarget target = new("store-1", "connection-1");
            CapabilityGrant grant = service.Issue("client-1", [ControlCapability.ConnectionsRead], [target],
                TimeSpan.FromMinutes(5), 3, now);

            Assert.That(service.IsAuthorized(grant.GrantId, "client-1", ControlCapability.ConnectionsRead, target, 3, now), Is.True);
            Assert.That(service.IsAuthorized(grant.GrantId, "client-2", ControlCapability.ConnectionsRead, target, 3, now), Is.False);
            Assert.That(service.IsAuthorized(grant.GrantId, "client-1", ControlCapability.ConnectionsRead, target, 4, now), Is.False);
            Assert.That(service.IsAuthorized(grant.GrantId, "client-1", ControlCapability.ConnectionsRead, target, 3, now.AddMinutes(5)), Is.False);
        }

        [Test]
        public void Grant_RejectsLifetimeLongerThanFifteenMinutes()
        {
            CapabilityGrantService service = new();

            Assert.Throws<ArgumentOutOfRangeException>(() => service.Issue("client-1", [], [], TimeSpan.FromMinutes(16), 1, DateTime.UtcNow));
        }

        [Test]
        public void Processor_RejectsMissingGrantAndMalformedRequestWithoutCallingAdapter()
        {
            int calls = 0;
            ControlRequestProcessor processor = new(new CapabilityGrantService(), () => 1,
                (_, _) => { calls++; return new { safe = true }; });

            string missingGrant = Encoding.UTF8.GetString(processor.Process(
                Encoding.UTF8.GetBytes("{\"SchemaVersion\":1,\"RequestId\":\"r1\",\"Action\":\"actions.list\"}"), "S-1-test", DateTime.UtcNow));
            string malformed = Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes("{bad"), "S-1-test", DateTime.UtcNow));
            string unknownAction = Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(
                "{\"SchemaVersion\":1,\"RequestId\":\"r3\",\"Action\":\"connection.shell\"}"), "S-1-test", DateTime.UtcNow));

            Assert.That(missingGrant, Does.Contain("ACCESS_DENIED"));
            Assert.That(malformed, Does.Contain("INVALID_REQUEST"));
            Assert.That(unknownAction, Does.Contain("UNKNOWN_ACTION"));
            Assert.That(calls, Is.Zero);
        }

        [Test]
        public void Processor_RequiresExactClientCapabilityTargetAndCurrentPolicy()
        {
            CapabilityGrantService grants = new();
            DateTime now = DateTime.UtcNow;
            ControlTarget target = new(string.Empty, string.Empty);
            CapabilityGrant grant = grants.Issue("S-1-client", [ControlCapability.ActionsRead], [target],
                TimeSpan.FromMinutes(3), 8, now);
            long currentPolicy = 8;
            int calls = 0;
            ControlRequestProcessor processor = new(grants, () => currentPolicy,
                (_, _) => { calls++; return new { safe = true }; });
            string request = JsonSerializer.Serialize(new ControlRequest(1, "r2", ControlActionNames.ActionsList, grant.GrantId));

            Assert.That(Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(request), "S-1-other", now)), Does.Contain("ACCESS_DENIED"));
            currentPolicy++;
            Assert.That(Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(request), "S-1-client", now)), Does.Contain("ACCESS_DENIED"));
            currentPolicy--;
            grants.Revoke(grant.GrantId);
            Assert.That(Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(request), "S-1-client", now)), Does.Contain("ACCESS_DENIED"));
            Assert.That(calls, Is.Zero);
        }

        [Test]
        public void Processor_MapsInternalFailureToRedactedError()
        {
            CapabilityGrantService grants = new();
            DateTime now = DateTime.UtcNow;
            CapabilityGrant grant = grants.Issue("S-1-client", [ControlCapability.ActionsRead],
                [new ControlTarget(string.Empty, string.Empty)], TimeSpan.FromMinutes(2), 1, now);
            ControlRequestProcessor processor = new(grants, () => 1,
                (_, _) => throw new InvalidOperationException("credential=fixture-secret"));
            string request = JsonSerializer.Serialize(new ControlRequest(1, "r3", ControlActionNames.ActionsList, grant.GrantId));

            string response = Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(request), "S-1-client", now));

            Assert.That(response, Does.Contain("INTERNAL_ERROR"));
            Assert.That(response, Does.Not.Contain("fixture-secret"));
            grants.RevokeAll();
            Assert.That(grants.IsAuthorized(grant.GrantId, "S-1-client", ControlCapability.ActionsRead,
                new ControlTarget(string.Empty, string.Empty), 1, now), Is.False);
        }

        [Test]
        public void Processor_ListsOnlyScopedSafeMetadataAndTracksCompletedRead()
        {
            CapabilityGrantService grants = new();
            DateTime now = DateTime.UtcNow;
            ControlTarget target = new("active", "connection-7");
            CapabilityGrant grant = grants.Issue("S-1-client", [ControlCapability.ActionsRead,
                    ControlCapability.ConnectionsRead, ControlCapability.ActionsPreview, ControlCapability.OperationsRead],
                [new ControlTarget(string.Empty, string.Empty), target], TimeSpan.FromMinutes(5), 17, now);
            int policyRevision = 17;
            ControlRequestProcessor processor = new(grants, () => policyRevision,
                (request, scopedTarget) => request.Action switch
                {
                    ControlActionNames.ConnectionsList => new SafeConnectionSummary(scopedTarget.StoreId,
                        scopedTarget.TargetId, "RDP", "Configured", [ControlActionNames.ConnectionsList]),
                    ControlActionNames.ActionsPreview => new ActionPreview(string.Empty, request.PreviewAction,
                        scopedTarget.StoreId, scopedTarget.TargetId, false, "SIDE_EFFECT_REVIEW_REQUIRED", true,
                        now.AddMinutes(1)),
                    _ => null
                });

            string listed = Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(
                new ControlRequest(1, "list-1", ControlActionNames.ConnectionsList, grant.GrantId,
                    target.StoreId, target.TargetId))), "S-1-client", now));
            using JsonDocument listDocument = JsonDocument.Parse(listed);
            JsonElement listResult = listDocument.RootElement.GetProperty("Result");
            string operationId = listResult.GetProperty("Operation").GetProperty("OperationId").GetString();
            string listData = listResult.GetProperty("Data").GetRawText();

            Assert.That(listData, Does.Contain("connection-7"));
            Assert.That(listData, Does.Not.Contain("hostname"));
            Assert.That(listData, Does.Not.Contain("username"));
            Assert.That(listData, Does.Not.Contain("credential"));

            ControlRequest getOperation = new(1, "op-1", ControlActionNames.OperationsGet, grant.GrantId,
                target.StoreId, target.TargetId, OperationId: operationId);
            string operation = Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(getOperation)),
                "S-1-client", now));
            Assert.That(operation, Does.Contain("completed"));
            Assert.That(operation, Does.Contain("SUCCESS"));
            Assert.That(operation, Does.Contain("connection-7"));

            ControlRequest blockedPreview = new(1, "preview-1", ControlActionNames.ActionsPreview,
                grant.GrantId, target.StoreId, target.TargetId, PreviewAction: "connections.open");
            string preview = Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(blockedPreview)),
                "S-1-client", now));
            Assert.That(preview, Does.Contain("SIDE_EFFECT_REVIEW_REQUIRED"));
            Assert.That(preview, Does.Contain("BLOCKED"));

            ControlRequest otherTarget = getOperation with { RequestId = "wrong-target", TargetId = "connection-8" };
            Assert.That(Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(otherTarget)),
                "S-1-client", now)), Does.Contain("ACCESS_DENIED"));

            policyRevision++;
            Assert.That(Encoding.UTF8.GetString(processor.Process(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(getOperation)),
                "S-1-client", now)), Does.Contain("ACCESS_DENIED"));
        }

        [Test]
        public async Task NamedPipeHost_UsesScopedGrantAndStopsOnDispose()
        {
            string pipeName = "mRemoteNG.ControlTest." + Guid.NewGuid().ToString("N");
            string clientId = AgentControlRuntime.GetCurrentClientIdForTesting();
            DateTime now = DateTime.UtcNow;
            CapabilityGrantService grants = new();
            CapabilityGrant grant = grants.Issue(clientId, [ControlCapability.ActionsRead],
                [new ControlTarget(string.Empty, string.Empty)], TimeSpan.FromMinutes(1), 1, now);
            ControlRequestProcessor processor = new(grants, () => 1,
                (request, _) => request.Action == ControlActionNames.ActionsList ? ActionRegistry.List() : null);
            NamedPipeControlHost host = new(pipeName, processor);
            host.Start();

            await using (host)
            await using (NamedPipeClientStream client = new(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                await client.ConnectAsync(3000);
                string request = JsonSerializer.Serialize(new ControlRequest(1, "pipe-test", ControlActionNames.ActionsList, grant.GrantId));
                byte[] requestBytes = Encoding.UTF8.GetBytes(request + "\n");
                try
                {
                    await client.WriteAsync(requestBytes);
                    await client.FlushAsync();
                }
                catch (IOException ex)
                {
                    Assert.Fail($"The pipe closed before responding ({host.LastTransportFailureType}): {ex.GetType().Name}");
                }
                using MemoryStream response = new();
                byte[] oneByte = new byte[1];
                while (await client.ReadAsync(oneByte) > 0 && oneByte[0] != (byte)'\n')
                    response.WriteByte(oneByte[0]);

                string responseText = Encoding.UTF8.GetString(response.ToArray());
                Assert.That(responseText, Does.Contain("pipe-test"));
                Assert.That(responseText, Does.Contain("actions.list"));
                Assert.That(responseText, Does.Not.Contain("ACCESS_DENIED"));
            }

            Assert.That(host.IsRunning, Is.False);
            Assert.That(grants.IsAuthorized(grant.GrantId, clientId, ControlCapability.ActionsRead,
                new ControlTarget(string.Empty, string.Empty), 1, now), Is.True);
            grants.RevokeAll();
            Assert.That(grants.IsAuthorized(grant.GrantId, clientId, ControlCapability.ActionsRead,
                new ControlTarget(string.Empty, string.Empty), 1, now), Is.False);
        }

        [Test]
        public async Task Runtime_IsDisabledByDefaultAndRevokesOnStop()
        {
            AgentControlRuntime runtime = new();
            Assert.That(runtime.IsEnabled, Is.False);
            Assert.Throws<InvalidOperationException>(() => runtime.IssueActionsDiscoveryGrant());

            runtime.EnableForCurrentSession();
            Assert.That(runtime.IsEnabled, Is.True);
            CapabilityGrant grant = runtime.IssueActionsDiscoveryGrant();
            Assert.That(grant.ExpiresUtc, Is.GreaterThan(DateTime.UtcNow));

            await runtime.DisableAndRevokeAsync();

            Assert.That(runtime.IsEnabled, Is.False);
            Assert.That(runtime.PipeName, Is.Empty);
            Assert.Throws<InvalidOperationException>(() => runtime.IssueActionsDiscoveryGrant());
        }
    }
}
