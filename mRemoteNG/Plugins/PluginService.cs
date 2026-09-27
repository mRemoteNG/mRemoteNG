using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Loader;
using mRemoteNG.App;
using mRemoteNG.PluginContracts;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Plugins;

public sealed class PluginService
{
    private readonly Dictionary<string, PluginToolWindow> _openWindows = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IPlugin> _plugins = [];
    private PluginContext? _pluginContext;

    public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();

    public string GetPluginDirectory()
    {
        return GetPluginDirectories().FirstOrDefault() ?? GetDefaultPluginDirectory();
    }

    public IReadOnlyCollection<string> GetPluginDirectories()
    {
        string configuredDirectories = Properties.Settings.Default.PluginFolderPath ?? string.Empty;
        List<string> directories = [];

        if (string.IsNullOrWhiteSpace(configuredDirectories))
        {
            string defaultPluginDirectory = GetDefaultPluginDirectory();
            Properties.Settings.Default.PluginFolderPath = defaultPluginDirectory;
            try
            {
                Properties.Settings.Default.Save();
            }
            catch
            {
                // Some environments disallow writing user settings; the in-memory default is still valid.
            }

            configuredDirectories = defaultPluginDirectory;
        }

        if (!string.IsNullOrWhiteSpace(configuredDirectories))
        {
            directories.AddRange(configuredDirectories
                .Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(directory => directory.Trim())
                .Where(directory => !string.IsNullOrWhiteSpace(directory))
                .Select(directory => Path.GetFullPath(directory))
                .Distinct(StringComparer.OrdinalIgnoreCase));
        }

        if (directories.Count == 0)
        {
            directories.Add(GetDefaultPluginDirectory());
        }

        return directories.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    public string GetDefaultPluginDirectory()
    {
        return Path.Combine(AppContext.BaseDirectory, "Plugins");
    }

    public void ReloadPlugins()
    {
        LoadPlugins();
    }

    public IReadOnlyCollection<PluginCatalogEntry> GetPluginCatalog()
    {
        List<PluginCatalogEntry> entries = [];
        HashSet<string> disabledPluginIds = GetDisabledPluginIds();
        Version hostVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0);
        PluginContext pluginContext = _pluginContext ?? new PluginContext(new PluginConnectionImportService(), new PluginMessageWriter(), new PluginResources());

        foreach (string pluginDirectory in GetPluginSearchDirectories())
        {
            if (!Directory.Exists(pluginDirectory))
            {
                App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, $"Plugin scan directory not found: '{pluginDirectory}'.", true);
                continue;
            }

            string[] pluginFiles = Directory.EnumerateFiles(pluginDirectory, "*.dll", SearchOption.TopDirectoryOnly).ToArray();
            App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Plugin scan directory '{pluginDirectory}' found {pluginFiles.Length} DLL(s).", true);

            foreach (string pluginPath in pluginFiles)
            {
                App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Plugin candidate: '{pluginPath}'.", true);

                try
                {
                    Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(pluginPath);
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Loaded plugin assembly '{pluginPath}' successfully.", true);

                    List<IPlugin> discoveredPlugins = DiscoverPlugins(assembly, pluginContext, hostVersion).ToList();
                    App.Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, $"Assembly '{pluginPath}' discovered {discoveredPlugins.Count} plugin(s).", true);

                    foreach (IPlugin plugin in discoveredPlugins)
                    {
                        entries.Add(new PluginCatalogEntry(
                            plugin.Id,
                            plugin.Id,
                            plugin.Version?.ToString() ?? string.Empty,
                            !disabledPluginIds.Contains(plugin.Id)));
                    }
                }
                catch (Exception ex)
                {
                    App.Runtime.MessageCollector.AddExceptionMessage($"Failed to inspect plugin assembly '{pluginPath}'.", ex);
                }
            }
        }

        return entries
            .GroupBy(entry => entry.PluginId, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .OrderBy(entry => entry.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public void LoadPlugins()
    {
        _plugins.Clear();
        _pluginContext = new PluginContext(new PluginConnectionImportService(), new PluginMessageWriter(), new PluginResources());

        HashSet<string> disabledPluginIds = GetDisabledPluginIds();
        Version hostVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0);

        foreach (string pluginDirectory in GetPluginSearchDirectories())
        {
            if (!Directory.Exists(pluginDirectory))
            {
                continue;
            }

            foreach (string pluginPath in Directory.EnumerateFiles(pluginDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(pluginPath);
                    foreach (IPlugin plugin in DiscoverPlugins(assembly, _pluginContext, hostVersion))
                    {
                        if (disabledPluginIds.Contains(plugin.Id))
                        {
                            continue;
                        }

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

    private IEnumerable<string> GetPluginSearchDirectories()
    {
        List<string> directories = [.. GetPluginDirectories()];
        string appBaseDirectory = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (!directories.Any(directory => string.Equals(directory, appBaseDirectory, StringComparison.OrdinalIgnoreCase)))
        {
            directories.Add(appBaseDirectory);
        }

        return directories.Distinct(StringComparer.OrdinalIgnoreCase);
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

    public void ShowToolWindow(string pluginId, Connection.ConnectionInfo? connectionInfo = null)
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

        ToolWindowRegistration registration = plugin.ToolWindow;
        plugin.OnBeforeShow(connectionInfo is null ? null : new PluginConnectionAdapter(connectionInfo));

        if (registration.ShowAsDocument)
        {
            string targetPanelName = GetPluginTargetPanel(pluginId, registration.PanelName);

            ConnectionWindow targetPanel = Runtime.WindowList
                .OfType<ConnectionWindow>()
                .FirstOrDefault(candidate => string.Equals(candidate.TabText, targetPanelName, StringComparison.OrdinalIgnoreCase))
                ?? new PanelAdder().AddPanel(targetPanelName);

            if (targetPanel.DockState == DockState.Unknown || targetPanel.DockState == DockState.Hidden || !targetPanel.Visible)
            {
                targetPanel.Show(FrmMain.Default.pnlDock, DockState.Document);
            }

            targetPanel.Activate();
            if (window.DockPanel != targetPanel.connDock || window.DockState == DockState.Unknown || window.DockState == DockState.Hidden || !window.Visible)
            {
                window.Show(targetPanel.connDock, DockState.Document);
            }
        }
        else
        {
            window.Show(FrmMain.Default.pnlDock);
        }

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

    public string? GetPluginTargetPanel(string pluginId, string? fallbackPanelName = null)
    {
        string resolvedFallback = string.IsNullOrWhiteSpace(fallbackPanelName)
            ? PanelAdder.DefaultPanelName
            : fallbackPanelName.Trim();

        string settingsValue = Properties.Settings.Default.PluginPanelAssignments ?? string.Empty;
        foreach (string assignment in settingsValue.Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            int separatorIndex = assignment.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            string configuredPluginId = assignment.Substring(0, separatorIndex).Trim();
            if (!string.Equals(configuredPluginId, pluginId, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string configuredPanelName = assignment.Substring(separatorIndex + 1).Trim();
            return string.IsNullOrWhiteSpace(configuredPanelName) ? resolvedFallback : configuredPanelName;
        }

        return resolvedFallback;
    }

    public void SetPluginTargetPanel(string pluginId, string? panelName)
    {
        if (string.IsNullOrWhiteSpace(pluginId))
        {
            return;
        }

        Dictionary<string, string> assignments = ReadPluginPanelAssignments();
        if (string.IsNullOrWhiteSpace(panelName))
        {
            assignments.Remove(pluginId);
        }
        else
        {
            assignments[pluginId] = panelName.Trim();
        }

        Properties.Settings.Default.PluginPanelAssignments = WritePluginPanelAssignments(assignments);
    }

    private static Dictionary<string, string> ReadPluginPanelAssignments()
    {
        Dictionary<string, string> assignments = new(StringComparer.OrdinalIgnoreCase);
        string value = Properties.Settings.Default.PluginPanelAssignments ?? string.Empty;

        foreach (string assignment in value.Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            int separatorIndex = assignment.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            string pluginId = assignment.Substring(0, separatorIndex).Trim();
            string panelName = assignment.Substring(separatorIndex + 1).Trim();
            if (!string.IsNullOrWhiteSpace(pluginId) && !string.IsNullOrWhiteSpace(panelName))
            {
                assignments[pluginId] = panelName;
            }
        }

        return assignments;
    }

    private static string WritePluginPanelAssignments(Dictionary<string, string> assignments)
    {
        return string.Join(";", assignments
            .OrderBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
            .Select(pair => $"{pair.Key}={pair.Value.Trim()}"));
    }

    private static HashSet<string> GetDisabledPluginIds()
    {
        string disabledPlugins = ReadDisabledPluginsSetting();
        return disabledPlugins
            .Split([';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static string ReadDisabledPluginsSetting()
    {
        return Properties.Settings.Default["DisabledPlugins"] as string ?? string.Empty;
    }
}
