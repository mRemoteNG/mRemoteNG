using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using mRemoteNG.PluginContracts;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Plugins;

public sealed class PluginService
{
    private readonly Dictionary<string, PluginToolWindow> _openWindows = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IToolWindowPlugin> _toolWindowPlugins = [];
    private PluginContext? _pluginContext;

    public IReadOnlyCollection<IToolWindowPlugin> ToolWindowPlugins => _toolWindowPlugins.AsReadOnly();

    public void LoadPlugins()
    {
        _toolWindowPlugins.Clear();
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
                foreach (IToolWindowPlugin plugin in DiscoverPlugins(assembly, _pluginContext, hostVersion))
                {
                    if (_toolWindowPlugins.Any(existingPlugin => string.Equals(existingPlugin.Id, plugin.Id, StringComparison.OrdinalIgnoreCase)))
                    {
                        App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Skipping duplicate plugin '{plugin.Id}'.", true);
                        continue;
                    }

                    _toolWindowPlugins.Add(plugin);
                }
            }
            catch (Exception ex)
            {
                App.Runtime.MessageCollector.AddExceptionMessage($"Failed to load plugin assembly '{pluginPath}'.", ex);
            }
        }

        _toolWindowPlugins.Sort((left, right) => left.ToolWindow.SortOrder.CompareTo(right.ToolWindow.SortOrder));
    }

    public IReadOnlyCollection<IToolWindowPlugin> DiscoverPlugins(Assembly assembly, IPluginContext pluginContext, Version hostVersion)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(pluginContext);
        ArgumentNullException.ThrowIfNull(hostVersion);

        List<IToolWindowPlugin> plugins = [];
        IEnumerable<Type> candidateTypes = GetLoadableTypes(assembly);
        foreach (Type type in candidateTypes)
        {
            if (type.IsAbstract || !typeof(IToolWindowPlugin).IsAssignableFrom(type))
            {
                continue;
            }

            try
            {
                if (Activator.CreateInstance(type) is not IToolWindowPlugin plugin)
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
        return _toolWindowPlugins.Where(plugin => plugin.ToolWindow.ShowInToolsMenu);
    }

    public IEnumerable<IToolWindowPlugin> GetContextMenuPlugins(PluginContextMenuGroup group)
    {
        return _toolWindowPlugins.Where(plugin =>
            plugin.ToolWindow.ContextMenuGroup == group &&
            !string.IsNullOrWhiteSpace(plugin.ToolWindow.ContextMenuText));
    }

    public void ShowToolWindow(string pluginId)
    {
        IToolWindowPlugin? plugin = _toolWindowPlugins.FirstOrDefault(candidate =>
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
}
