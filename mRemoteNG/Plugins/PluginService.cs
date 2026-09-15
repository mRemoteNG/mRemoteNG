using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Loader;
using mRemoteNG.PluginContracts;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Plugins;

public sealed class PluginService
{
    private readonly Dictionary<string, PluginToolWindow> _openWindows = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IPlugin> _plugins = [];
    private PluginContext? _pluginContext;

    public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();

    public void LoadPlugins()
    {
        _plugins.Clear();
        _pluginContext = new PluginContext(new PluginConnectionImportService(), new PluginMessageWriter(), new PluginResources());

        string pluginDirectory = Path.Combine(AppContext.BaseDirectory, "Plugins");
        if (!Directory.Exists(pluginDirectory))
        {
            return;
        }

        Version hostVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0);

        foreach (string pluginPath in Directory.EnumerateFiles(pluginDirectory, "*.dll", SearchOption.TopDirectoryOnly))
        {
            try
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(pluginPath);
                foreach (IPlugin plugin in DiscoverPlugins(assembly, _pluginContext, hostVersion))
                {
                    if (_plugins.Any(existingPlugin => string.Equals(existingPlugin.Id, plugin.Id, StringComparison.OrdinalIgnoreCase)))
                    {
                        App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Skipping duplicate plugin '{plugin.Id}'.", true);
                        continue;
                    }

                    _plugins.Add(plugin);
                }
            }
            catch (Exception ex)
            {
                App.Runtime.MessageCollector.AddExceptionMessage($"Failed to load plugin assembly '{pluginPath}'.", ex);
            }
        }

        _plugins.Sort(ComparePlugins);
    }

    public IReadOnlyCollection<IPlugin> DiscoverPlugins(Assembly assembly, IPluginContext pluginContext, Version hostVersion)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(pluginContext);
        ArgumentNullException.ThrowIfNull(hostVersion);

        List<IPlugin> plugins = [];
        IEnumerable<Type> candidateTypes = GetLoadableTypes(assembly);
        foreach (Type type in candidateTypes)
        {
            if (type.IsAbstract || !typeof(IPlugin).IsAssignableFrom(type))
            {
                continue;
            }

            try
            {
                if (Activator.CreateInstance(type) is not IPlugin plugin)
                {
                    continue;
                }

                if (!PluginCompatibility.IsCompatible(plugin, hostVersion))
                {
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Skipping plugin '{plugin.Id}' because it requires host version {plugin.MinimumHostVersion} or newer.", true);
                    continue;
                }

                plugin.Initialize(pluginContext);
                plugins.Add(plugin);
            }
            catch (Exception ex)
            {
                App.Runtime.MessageCollector.AddExceptionMessage($"Failed to initialize plugin type '{type.FullName}'.", ex);
            }
        }

        return plugins;
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(type => type is not null).Cast<Type>();
        }
    }

    public IEnumerable<IToolWindowPlugin> GetToolsMenuPlugins()
    {
        return _plugins.OfType<IToolWindowPlugin>().Where(plugin => plugin.ToolWindow.ShowInToolsMenu);
    }

    public IEnumerable<IToolWindowPlugin> GetContextMenuPlugins(PluginContextMenuGroup group)
    {
        return _plugins.OfType<IToolWindowPlugin>().Where(plugin =>
            plugin.ToolWindow.ContextMenuGroup == group &&
            !string.IsNullOrWhiteSpace(plugin.ToolWindow.ContextMenuText));
    }

    public IEnumerable<ConnectionPropertyDefinition> GetConnectionPropertyDefinitions(Connection.ConnectionInfo connectionInfo)
    {
        PluginConnectionAdapter connection = new(connectionInfo);
        return _plugins
            .OfType<IConnectionPropertyProviderPlugin>()
            .SelectMany(plugin => plugin.ConnectionProperties)
            .Where(definition => PluginProtocolMapper.SupportsProtocol(definition, connectionInfo.Protocol))
            .GroupBy(definition => definition.Key, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First());
    }

    public IEnumerable<ITreeContextActionPlugin> GetTreeContextActionPlugins(Connection.ConnectionInfo connectionInfo)
    {
        PluginConnectionAdapter connection = new(connectionInfo);
        return _plugins
            .OfType<ITreeContextActionPlugin>()
            .Where(plugin => plugin.CanExecute(connection))
            .OrderBy(plugin => plugin.TreeContextMenuAction.SortOrder);
    }

    public void ExecuteTreeContextAction(string pluginId, Connection.ConnectionInfo connectionInfo)
    {
        ITreeContextActionPlugin? plugin = _plugins
            .OfType<ITreeContextActionPlugin>()
            .FirstOrDefault(candidate => string.Equals(candidate.Id, pluginId, StringComparison.OrdinalIgnoreCase));

        if (plugin == null)
        {
            App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Could not find plugin '{pluginId}'.", true);
            return;
        }

        PluginConnectionAdapter connection = new(connectionInfo);
        if (plugin.CanExecute(connection))
        {
            plugin.Execute(connection);
        }
    }

    public async Task ResolveConnectionAddressAsync(Connection.ConnectionInfo connectionInfo, CancellationToken cancellationToken = default)
    {
        PluginConnectionAdapter connection = new(connectionInfo);
        foreach (IConnectionAddressResolverPlugin plugin in _plugins.OfType<IConnectionAddressResolverPlugin>())
        {
            if (!plugin.CanResolve(connection))
            {
                continue;
            }

            await plugin.ResolveAsync(connection, cancellationToken).ConfigureAwait(true);
        }
    }

    public void ShowToolWindow(string pluginId)
    {
        IToolWindowPlugin? plugin = _plugins.OfType<IToolWindowPlugin>().FirstOrDefault(candidate =>
            string.Equals(candidate.Id, pluginId, StringComparison.OrdinalIgnoreCase));

        if (plugin == null)
        {
            App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Could not find plugin '{pluginId}'.", true);
            return;
        }

        if (!_openWindows.TryGetValue(pluginId, out PluginToolWindow? window) || window.IsDisposed)
        {
            window = new PluginToolWindow(plugin);
            window.FormClosed += (_, _) => _openWindows.Remove(pluginId);
            _openWindows[pluginId] = window;
        }

        window.Show(FrmMain.Default.pnlDock);
        window.Activate();
    }

    private static int ComparePlugins(IPlugin left, IPlugin right)
    {
        int leftSortOrder = left is IToolWindowPlugin leftToolWindow ? leftToolWindow.ToolWindow.SortOrder :
            left is ITreeContextActionPlugin leftTreeAction ? leftTreeAction.TreeContextMenuAction.SortOrder : 0;
        int rightSortOrder = right is IToolWindowPlugin rightToolWindow ? rightToolWindow.ToolWindow.SortOrder :
            right is ITreeContextActionPlugin rightTreeAction ? rightTreeAction.TreeContextMenuAction.SortOrder : 0;

        return leftSortOrder.CompareTo(rightSortOrder);
    }
}
