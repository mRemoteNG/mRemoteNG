using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.PortScan;

public sealed class PortScanPlugin : IToolWindowPlugin
{
    private IPluginContext? _context;

    public string Id => "mRemoteNG.PortScan";

    public string DisplayName => Resources.GetString("PortScan", "Port Scan");

    public Version Version => new(1, 0, 0);

    public Version MinimumHostVersion => new(1, 0, 0);

    private IPluginResources Resources => _context?.Resources ?? throw new InvalidOperationException("Plugin has not been initialized.");

    public ToolWindowRegistration ToolWindow => new()
    {
        ContextMenuGroup = PluginContextMenuGroup.Import,
        ContextMenuText = Resources.GetString("ImportPortScan", "Import from Port Scan..."),
        Icon = Resources.GetImage("SearchAndApps_16x") as Image,
        MenuText = Resources.GetString("PortScan", "Port Scan"),
        SortOrder = 100,
        WindowTitle = Resources.GetString("PortScan", "Port Scan"),
    };

    public void Initialize(IPluginContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Control CreateControl()
    {
        return new PortScanControl(_context ?? throw new InvalidOperationException("Plugin has not been initialized."));
    }
}
