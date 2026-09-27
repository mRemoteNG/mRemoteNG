using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mRemoteNG.PluginContracts;
using mRemoteNG.Plugins.MultiAddress;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Plugins;

[TestFixture]
public class MultiAddressPluginTests
{
    private const string EnabledKey = "mRp.MultiAddress.Enabled";
    private const string HostnameKey = "mRp.MultiAddress.Hostname";
    private const string IpAddressKey = "mRp.MultiAddress.IpAddress";
    private const string UseIpAddressAsPrimaryKey = "mRp.MultiAddress.UseIpAddressAsPrimary";
    private const string VerifyHostnameMatchesIpKey = "mRp.MultiAddress.VerifyHostnameMatchesIp";

    [Test]
    public async Task ResolveAsync_UsesIpAddressWhenConfiguredAsPrimary()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter);
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "server01.contoso.local");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.25");
        connection.SetPluginProperty(UseIpAddressAsPrimaryKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("192.0.2.25"));
        messageWriter.DidNotReceive().Warning(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_UsesHostnameWhenHostnameIsPrimary()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter);
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "server01.contoso.local");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.25");

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("server01.contoso.local"));
        messageWriter.DidNotReceive().Warning(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_FallsBackToIpAddressWhenHostnameCannotBeResolved()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => throw new System.Net.Sockets.SocketException());
        TestPluginConnection connection = CreateEnabledConnection();
        connection.Name = "Missing host";
        connection.SetPluginProperty(HostnameKey, "missing-hostname");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.50");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("192.0.2.50"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("could not be resolved")), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_KeepsIpAddressWhenHostnameCannotBeResolvedAndIpIsPrimary()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => throw new System.Net.Sockets.SocketException());
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "missing-hostname");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.50");
        connection.SetPluginProperty(UseIpAddressAsPrimaryKey, bool.TrueString);
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("192.0.2.50"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("could not be resolved")), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_FallsBackToIpAddressWhenHostnameLookupReturnsNoAddresses()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => Task.FromResult(Array.Empty<System.Net.IPAddress>()));
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "missing-hostname");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.50");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("192.0.2.50"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("Using the saved IP address instead")), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_UsesIpAddressAndWarnsWhenHostnameResolvesToDifferentIp()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => Task.FromResult(new[] { System.Net.IPAddress.Parse("127.0.0.1") }));
        TestPluginConnection connection = CreateEnabledConnection();
        connection.Name = "Localhost";
        connection.SetPluginProperty(HostnameKey, "server01");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.10");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("192.0.2.10"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("did not match")), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_WarnsWhenSavedIpAddressIsInvalidAndKeepsPreferredTarget()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter);
        TestPluginConnection connection = CreateEnabledConnection();
        connection.Name = "Broken IP";
        connection.SetPluginProperty(HostnameKey, "server01.contoso.local");
        connection.SetPluginProperty(IpAddressKey, "not-an-ip");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("server01.contoso.local"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("invalid saved IP address")), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_FallsBackToHostnameWhenPreferredIpAddressIsInvalid()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter);
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "server01.contoso.local");
        connection.SetPluginProperty(IpAddressKey, "not-an-ip");
        connection.SetPluginProperty(UseIpAddressAsPrimaryKey, bool.TrueString);
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("server01.contoso.local"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("invalid saved IP address")), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_AcceptsIpv4MappedIpv6MatchWithoutWarning()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => Task.FromResult(new[] { System.Net.IPAddress.Parse("::ffff:192.0.2.10") }));
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "server01");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.10");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("server01"));
        messageWriter.DidNotReceive().Warning(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public async Task ResolveAsync_WarnsWhenSavedHostnameIsInvalidAndFallsBackToIpAddress()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => throw new InvalidOperationException("resolver should not run"));
        TestPluginConnection connection = CreateEnabledConnection();
        connection.Name = "Broken host";
        connection.SetPluginProperty(HostnameKey, "bad host name");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.60");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        await plugin.ResolveAsync(connection, CancellationToken.None);

        Assert.That(connection.Hostname, Is.EqualTo("192.0.2.60"));
        messageWriter.Received().Warning(Arg.Is<string>(message => message.Contains("invalid saved hostname")), Arg.Any<bool>());
    }

    [Test]
    public void ResolveAsync_PropagatesUnexpectedResolverExceptions()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, _) => throw new InvalidOperationException("unexpected"));
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "server01");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.60");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await plugin.ResolveAsync(connection, CancellationToken.None));
        messageWriter.DidNotReceive().Warning(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public void ResolveAsync_PropagatesCancellation()
    {
        IMessageWriter messageWriter = Substitute.For<IMessageWriter>();
        MultiAddressPlugin plugin = CreatePlugin(messageWriter, (_, cancellationToken) => Task.FromCanceled<System.Net.IPAddress[]>(cancellationToken));
        TestPluginConnection connection = CreateEnabledConnection();
        connection.SetPluginProperty(HostnameKey, "server01");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.60");
        connection.SetPluginProperty(VerifyHostnameMatchesIpKey, bool.TrueString);

        using CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(async () => await plugin.ResolveAsync(connection, cancellationTokenSource.Token));
        messageWriter.DidNotReceive().Warning(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Test]
    public void CanResolve_ReturnsFalseWhenFeatureIsDisabled()
    {
        MultiAddressPlugin plugin = new();
        TestPluginConnection connection = new();
        connection.SetPluginProperty(HostnameKey, "server01");
        connection.SetPluginProperty(IpAddressKey, "192.0.2.1");

        bool canResolve = plugin.CanResolve(connection);

        Assert.That(canResolve, Is.False);
    }

    [Test]
    public void ConnectionProperties_ExposeExpectedKeysAndTypes()
    {
        MultiAddressPlugin plugin = CreatePlugin(Substitute.For<IMessageWriter>());

        Dictionary<string, PluginPropertyType> properties = plugin.ConnectionProperties
            .ToDictionary(property => property.Key, property => property.PropertyType, StringComparer.OrdinalIgnoreCase);

        Assert.That(properties, Has.Count.EqualTo(5));
        Assert.That(properties[EnabledKey], Is.EqualTo(PluginPropertyType.Boolean));
        Assert.That(properties[HostnameKey], Is.EqualTo(PluginPropertyType.String));
        Assert.That(properties[IpAddressKey], Is.EqualTo(PluginPropertyType.String));
        Assert.That(properties[UseIpAddressAsPrimaryKey], Is.EqualTo(PluginPropertyType.Boolean));
        Assert.That(properties[VerifyHostnameMatchesIpKey], Is.EqualTo(PluginPropertyType.Boolean));
    }

    [Test]
    public void ConnectionProperties_UseLocalizedResourceValuesWhenAvailable()
    {
        MultiAddressPlugin plugin = CreatePlugin(
            Substitute.For<IMessageWriter>(),
            localize: (resourceName, fallback) => resourceName == "MultiAddressEnableSeparateHostnameIp"
                ? "Localized value"
                : fallback);

        ConnectionPropertyDefinition property = plugin.ConnectionProperties.Single(definition => definition.Key == EnabledKey);

        Assert.That(property.DisplayName, Is.EqualTo("Localized value"));
    }

    [Test]
    public void CanResolve_ReturnsFalseForContainers()
    {
        MultiAddressPlugin plugin = new();
        TestPluginConnection connection = CreateEnabledConnection();
        connection.IsContainerValue = true;
        connection.SetPluginProperty(HostnameKey, "server01");

        bool canResolve = plugin.CanResolve(connection);

        Assert.That(canResolve, Is.False);
    }

    [Test]
    public void CanResolve_ReturnsFalseWhenNoSeparateAddressIsConfigured()
    {
        MultiAddressPlugin plugin = new();
        TestPluginConnection connection = CreateEnabledConnection();

        bool canResolve = plugin.CanResolve(connection);

        Assert.That(canResolve, Is.False);
    }

    private static MultiAddressPlugin CreatePlugin(
        IMessageWriter messageWriter,
        Func<string, CancellationToken, Task<System.Net.IPAddress[]>>? addressResolver = null,
        Func<string, string, string>? localize = null)
    {
        IPluginContext context = Substitute.For<IPluginContext>();
        IPluginResources resources = Substitute.For<IPluginResources>();
        context.Messages.Returns(messageWriter);
        resources.GetString(Arg.Any<string>(), Arg.Any<string>())
            .Returns(callInfo =>
            {
                string resourceName = callInfo.ArgAt<string>(0);
                string fallback = callInfo.ArgAt<string>(1);
                return localize?.Invoke(resourceName, fallback) ?? fallback;
            });
        context.Resources.Returns(resources);

        MultiAddressPlugin plugin = addressResolver is null
            ? new MultiAddressPlugin()
            : new MultiAddressPlugin(addressResolver);
        plugin.Initialize(context);
        return plugin;
    }

    private static TestPluginConnection CreateEnabledConnection()
    {
        TestPluginConnection connection = new()
        {
            Name = "Test connection",
        };
        connection.SetPluginProperty(EnabledKey, bool.TrueString);
        return connection;
    }

    private sealed class TestPluginConnection : IPluginConnection
    {
        private readonly Dictionary<string, string> _pluginProperties = new(StringComparer.OrdinalIgnoreCase);

        public string Hostname { get; set; } = string.Empty;

        public bool IsContainer => IsContainerValue;

        public bool IsContainerValue { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ProtocolId => "RDP";

        public int Port { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Domain { get; set; } = string.Empty;

        public string GetPluginProperty(string key, string defaultValue = "")
        {
            return _pluginProperties.TryGetValue(key, out string? value) ? value : defaultValue;
        }

        public IReadOnlyDictionary<string, string> GetPluginProperties()
        {
            return _pluginProperties;
        }

        public void SetPluginProperty(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _pluginProperties.Remove(key);
                return;
            }

            _pluginProperties[key] = value;
        }

        public bool TryGetPluginProperty(string key, out string value)
        {
            return _pluginProperties.TryGetValue(key, out value!);
        }
    }
}
