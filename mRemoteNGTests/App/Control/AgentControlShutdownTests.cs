using System;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App.AgentControl;
using mRemoteNG.UI.Forms.OptionsPages;
using NUnit.Framework;

namespace mRemoteNGTests.App.Control;

[NonParallelizable]
public class AgentControlShutdownTests
{
    [Test]
    public async Task Revoke_PendingRequestNeedsUi_DoesNotBlockUiOrRetainGrants()
    {
        TaskCompletionSource ready = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TaskCompletionSource requestEntered = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TaskCompletionSource uiWork = new(TaskCreationOptions.RunContinuationsAsynchronously);
        TaskCompletionSource stopped = new(TaskCreationOptions.RunContinuationsAsynchronously);
        using ManualResetEventSlim permitUiWork = new(false);
        AgentControlRuntime runtime = new();
        Form pump = null;
        AgentControlPage page = null;
        string clientId = AgentControlRuntime.GetCurrentClientIdForTesting();
        CapabilityGrantService grants = (CapabilityGrantService)typeof(AgentControlRuntime)
            .GetField("_grants", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(runtime);
        CapabilityGrant grant = grants.Issue(clientId, [ControlCapability.ActionsRead],
            [new ControlTarget(string.Empty, string.Empty)], TimeSpan.FromMinutes(1), 1, DateTime.UtcNow);
        ControlRequestProcessor processor = new(grants, () => 1, (_, _) =>
        {
            requestEntered.TrySetResult();
            if (!permitUiWork.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
            pump.BeginInvoke(new Action(() => uiWork.TrySetResult()));
            if (!uiWork.Task.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
            return ActionRegistry.List();
        });
        string pipeName = "mRemoteNG.ShutdownTest." + Guid.NewGuid().ToString("N");
        NamedPipeControlHost host = new(pipeName, processor);
        typeof(AgentControlRuntime).GetField("_host", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(runtime, host);
        host.Start();
        Thread thread = new(() =>
        {
            try
            {
                using Form form = new() { ShowInTaskbar = false };
                pump = form;
                using AgentControlPage options = new(runtime);
                page = options;
                _ = form.Handle;
                form.BeginInvoke(new Action(() => ready.TrySetResult()));
                Application.Run(new ApplicationContext());
            }
            catch (Exception error) { ready.TrySetException(error); stopped.TrySetException(error); }
        }) { IsBackground = true };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        try
        {
            await ready.Task.WaitAsync(TimeSpan.FromSeconds(10));
            await using NamedPipeClientStream client = new(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            await client.ConnectAsync(3000);
            string request = JsonSerializer.Serialize(new ControlRequest(1, "shutdown", ControlActionNames.ActionsList, grant.GrantId));
            await client.WriteAsync(Encoding.UTF8.GetBytes(request + "\n"));
            await client.FlushAsync();
            await requestEntered.Task.WaitAsync(TimeSpan.FromSeconds(5));
            pump.BeginInvoke(new Action(async () =>
            {
                try
                {
                    Task stop = page.RevokeAndStopAsync();
                    Assert.That(grants.IsAuthorized(grant.GrantId, clientId, ControlCapability.ActionsRead,
                        new ControlTarget(string.Empty, string.Empty), 1, DateTime.UtcNow), Is.False);
                    permitUiWork.Set();
                    await stop;
                    stopped.TrySetResult();
                }
                catch (Exception error) { stopped.TrySetException(error); }
            }));
            await stopped.Task.WaitAsync(TimeSpan.FromSeconds(3));
            Assert.That(uiWork.Task.IsCompletedSuccessfully, Is.True);
            Assert.That(runtime.IsEnabled, Is.False);
            Assert.That(runtime.PipeName, Is.Empty);
        }
        finally
        {
            permitUiWork.Set();
            await runtime.DisableAndRevokeAsync().WaitAsync(TimeSpan.FromSeconds(8));
            if (pump is { IsDisposed: false }) pump.BeginInvoke(new Action(Application.ExitThread));
            Assert.That(thread.Join(TimeSpan.FromSeconds(8)), Is.True, "The test UI pump must stop.");
        }
    }
}
