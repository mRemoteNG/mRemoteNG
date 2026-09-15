using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;
using mRemoteNG.Plugins;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Plugins;

[TestFixture]
public class PluginServiceTests
{
    [SetUp]
    public void SetUp()
    {
        CompatibleToolWindowPlugin.InitializeCount = 0;
        IncompatibleToolWindowPlugin.InitializeCount = 0;
        CompatibleConnectionPlugin.InitializeCount = 0;
    }

    [Test]
    public void IsCompatible_ReturnsTrue_WhenHostVersionMeetsMinimum()
    {
        CompatibleToolWindowPlugin plugin = new();

        bool isCompatible = PluginCompatibility.IsCompatible(plugin, new Version(1, 0, 0));

        Assert.That(isCompatible, Is.True);
    }

    [Test]
    public void DiscoverPlugins_InitializesCompatiblePlugins()
    {
        PluginService service = new();
        IPluginContext context = Substitute.For<IPluginContext>();

        var plugins = service.DiscoverPlugins(Assembly.GetExecutingAssembly(), context, new Version(1, 0, 0));

        Assert.That(plugins.Any(plugin => plugin.Id == CompatibleToolWindowPlugin.PluginId), Is.True);
        Assert.That(CompatibleToolWindowPlugin.InitializeCount, Is.EqualTo(1));
    }

    [Test]
    public void DiscoverPlugins_SkipsIncompatiblePlugins()
    {
        PluginService service = new();
        IPluginContext context = Substitute.For<IPluginContext>();

        var plugins = service.DiscoverPlugins(Assembly.GetExecutingAssembly(), context, new Version(1, 0, 0));

        Assert.That(plugins.Any(plugin => plugin.Id == IncompatibleToolWindowPlugin.PluginId), Is.False);
        Assert.That(IncompatibleToolWindowPlugin.InitializeCount, Is.EqualTo(0));
    }

    [Test]
    public void DiscoverPlugins_ReturnsConnectionExtensionPlugins()
    {
        PluginService service = new();
        IPluginContext context = Substitute.For<IPluginContext>();

        var plugins = service.DiscoverPlugins(Assembly.GetExecutingAssembly(), context, new Version(1, 0, 0)).ToArray();

        Assert.That(plugins.OfType<IConnectionPropertyProviderPlugin>().Any(plugin => plugin.Id == CompatibleConnectionPlugin.PluginId), Is.True);
        Assert.That(plugins.OfType<IConnectionAddressResolverPlugin>().Any(plugin => plugin.Id == CompatibleConnectionPlugin.PluginId), Is.True);
        Assert.That(plugins.OfType<ITreeContextActionPlugin>().Any(plugin => plugin.Id == CompatibleConnectionPlugin.PluginId), Is.True);
        Assert.That(CompatibleConnectionPlugin.InitializeCount, Is.EqualTo(1));
    }

    public sealed class CompatibleToolWindowPlugin : IToolWindowPlugin
    {
        public const string PluginId = "Tests.Compatible";
        public static int InitializeCount { get; set; }

        public string Id => PluginId;
        public string DisplayName => "Compatible";
        public Version Version => new(1, 0, 0);
        public Version MinimumHostVersion => new(1, 0, 0);
        public ToolWindowRegistration ToolWindow => new() { MenuText = "Compatible", WindowTitle = "Compatible" };

        public void Initialize(IPluginContext context)
        {
            InitializeCount++;
        }

        public Control CreateControl()
        {
            return new Control();
        }
    }

    public sealed class IncompatibleToolWindowPlugin : IToolWindowPlugin
    {
        public const string PluginId = "Tests.Incompatible";
        public static int InitializeCount { get; set; }

        public string Id => PluginId;
        public string DisplayName => "Incompatible";
        public Version Version => new(1, 0, 0);
        public Version MinimumHostVersion => new(99, 0, 0);
        public ToolWindowRegistration ToolWindow => new() { MenuText = "Incompatible", WindowTitle = "Incompatible" };

        public void Initialize(IPluginContext context)
        {
            InitializeCount++;
        }

        public Control CreateControl()
        {
            return new Control();
        }
    }

    public sealed class CompatibleConnectionPlugin : IConnectionPropertyProviderPlugin, IConnectionAddressResolverPlugin, ITreeContextActionPlugin
    {
        public const string PluginId = "Tests.Connection";
        public static int InitializeCount { get; set; }

        public string Id => PluginId;
        public string DisplayName => "Connection";
        public Version Version => new(1, 0, 0);
        public Version MinimumHostVersion => new(1, 0, 0);
        public IReadOnlyCollection<ConnectionPropertyDefinition> ConnectionProperties => [new ConnectionPropertyDefinition
        {
            Key = "Tests.Connection.Enabled",
            Category = "Tests",
            DisplayName = "Enabled",
            PropertyType = PluginPropertyType.Boolean,
        }];

        public TreeContextMenuAction TreeContextMenuAction => new()
        {
            MenuText = "Run test action",
            SortOrder = 100,
        };

        public void Initialize(IPluginContext context)
        {
            InitializeCount++;
        }

        public bool CanResolve(IPluginConnection connection)
        {
            return true;
        }

        public Task ResolveAsync(IPluginConnection connection, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public bool CanExecute(IPluginConnection connection)
        {
            return true;
        }

        public void Execute(IPluginConnection connection)
        {
        }
    }
}
