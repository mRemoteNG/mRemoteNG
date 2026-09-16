#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using mRemoteNG.PluginContracts;
using mRemoteNG.Plugins;
using mRemoteNG.Properties;
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
        Settings.Default.DisabledPlugins = string.Empty;
        Settings.Default.PluginFolderPath = string.Empty;
        Settings.Default.PluginPanelAssignments = string.Empty;
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

    [Test]
    public void DisabledPlugins_SettingRoundTripsThroughGeneratedSettings()
    {
        string disabledPlugins = $" {CompatibleToolWindowPlugin.PluginId} ; {CompatibleConnectionPlugin.PluginId} ";

        Settings.Default.DisabledPlugins = disabledPlugins;

        Assert.That(Settings.Default.DisabledPlugins, Is.EqualTo(disabledPlugins));
    }

    [Test]
    public void DisabledPlugins_SettingCanBeParsedIntoIndividualPluginIds()
    {
        Settings.Default.DisabledPlugins = $" {CompatibleToolWindowPlugin.PluginId} ; ; {CompatibleConnectionPlugin.PluginId} ";

        string[] disabledPluginIds = Settings.Default.DisabledPlugins
            .Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        Assert.That(disabledPluginIds, Is.EqualTo(new[]
        {
            CompatibleToolWindowPlugin.PluginId,
            CompatibleConnectionPlugin.PluginId,
        }));
    }

    [Test]
    public void PluginFolderPath_SettingRoundTripsThroughGeneratedSettings()
    {
        const string pluginFolderPath = @"\\server\shared\mRemoteNG\Plugins";

        Settings.Default.PluginFolderPath = pluginFolderPath;

        Assert.That(Settings.Default.PluginFolderPath, Is.EqualTo(pluginFolderPath));
    }

    [Test]
    public void GetPluginDirectory_ReturnsConfiguredFolderWhenSet()
    {
        PluginService service = new();
        const string pluginFolderPath = @"\\server\shared\mRemoteNG\Plugins";
        Settings.Default.PluginFolderPath = pluginFolderPath;

        string pluginDirectory = service.GetPluginDirectory();

        Assert.That(pluginDirectory, Is.EqualTo(pluginFolderPath));
    }

    [Test]
    public void GetPluginDirectory_FallsBackToApplicationPluginsFolderWhenUnset()
    {
        PluginService service = new();
        Settings.Default.PluginFolderPath = "   ";

        string pluginDirectory = service.GetPluginDirectory();

        Assert.That(pluginDirectory, Is.EqualTo(service.GetDefaultPluginDirectory()));
        Assert.That(Settings.Default.PluginFolderPath, Is.EqualTo(service.GetDefaultPluginDirectory()));
    }

    [Test]
    public void GetPluginDirectories_PopulatesDefaultFolderIntoSettingsWhenUnset()
    {
        PluginService service = new();
        Settings.Default.PluginFolderPath = string.Empty;

        string[] pluginDirectories = service.GetPluginDirectories().ToArray();

        Assert.That(pluginDirectories, Does.Contain(service.GetDefaultPluginDirectory()));
        Assert.That(Settings.Default.PluginFolderPath, Is.EqualTo(service.GetDefaultPluginDirectory()));
    }

    [Test]
    public void GetDefaultPluginDirectory_ReturnsApplicationPluginsFolder()
    {
        PluginService service = new();

        string pluginDirectory = service.GetDefaultPluginDirectory();

        Assert.That(pluginDirectory, Is.EqualTo(System.IO.Path.Combine(AppContext.BaseDirectory, "Plugins")));
    }

    [Test]
    public void GetPluginDirectories_SupportsMultipleConfiguredFolders()
    {
        PluginService service = new();
        string[] expected = ["C:\\FolderA", "D:\\FolderB"];
        Settings.Default.PluginFolderPath = string.Join(";", expected);

        string[] actual = service.GetPluginDirectories().ToArray();

        Assert.That(actual, Is.EqualTo(expected.Select(path => System.IO.Path.GetFullPath(path)).ToArray()));
    }

    [Test]
    public void GetPluginTargetPanel_UsesConfiguredOverrideWhenPresent()
    {
        PluginService service = new();
        Settings.Default.PluginPanelAssignments = $"{CompatibleToolWindowPlugin.PluginId}=CustomPanel";

        string panelName = service.GetPluginTargetPanel(CompatibleToolWindowPlugin.PluginId, "General");

        Assert.That(panelName, Is.EqualTo("CustomPanel"));
    }

    [Test]
    public void GetPluginTargetPanel_FallsBackToGeneralWhenNoOverrideExists()
    {
        PluginService service = new();

        string panelName = service.GetPluginTargetPanel(CompatibleToolWindowPlugin.PluginId, "General");

        Assert.That(panelName, Is.EqualTo("General"));
    }

    [Test]
    public void ToolWindowRegistration_DefaultPanelNameFallsBackToGeneral()
    {
        ToolWindowRegistration registration = new() { MenuText = "Example", WindowTitle = "Example" };

        Assert.That(registration.PanelName, Is.Null);
        Assert.That(string.IsNullOrWhiteSpace(registration.PanelName), Is.True);
    }

    [Test]
    public void ToolWindowRegistration_CanOverrideTargetPanelName()
    {
        ToolWindowRegistration registration = new() { MenuText = "Example", WindowTitle = "Example", PanelName = "CustomPanel" };

        Assert.That(registration.PanelName, Is.EqualTo("CustomPanel"));
    }

    [Test]
    public void GetPluginDirectory_TreatsConfiguredDefaultFolderAsCustomPathValueButReturnsSameDirectory()
    {
        PluginService service = new();
        string defaultPluginDirectory = service.GetDefaultPluginDirectory();
        Settings.Default.PluginFolderPath = defaultPluginDirectory;

        string pluginDirectory = service.GetPluginDirectory();

        Assert.That(pluginDirectory, Is.EqualTo(defaultPluginDirectory));
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

        public void OnBeforeShow(IPluginConnection? connection)
        {
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

        public void OnBeforeShow(IPluginConnection? connection)
        {
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
