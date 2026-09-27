using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.SshTransfer;

public sealed class SshTransferPlugin : IToolWindowPlugin
{
    private IPluginContext? _context;
    private SshTransferControl? _control;

    public string Id => "mRp.SshTransfer";

    public string DisplayName => Resources.GetString("SshFileTransfer", "SSH File Transfer");

    public Version Version => new(1, 0, 0);

    public Version MinimumHostVersion => new(1, 0, 0);

    private IPluginResources Resources => _context?.Resources ?? throw new InvalidOperationException("Plugin has not been initialized.");

    public ToolWindowRegistration ToolWindow => new()
    {
        ContextMenuGroup = PluginContextMenuGroup.None,
        ContextMenuText = null,
        Icon = Resources.GetImage("SyncArrow_16x") as Image,
        MenuText = Resources.GetString("SshFileTransfer", "SSH File Transfer"),
        PanelName = "General",
        RequiresConnectionSelection = false,
        ShowAsDocument = true,
        SortOrder = 150,
        WindowTitle = Resources.GetString("SshFileTransfer", "SSH File Transfer"),
    };

    public void Initialize(IPluginContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Control CreateControl()
    {
        if (_control == null || _control.IsDisposed)
        {
            _control = new SshTransferControl(_context ?? throw new InvalidOperationException("Plugin has not been initialized."));
        }

        return _control;
    }

    public void OnBeforeShow(IPluginConnection? connection)
    {
        (_control ?? throw new InvalidOperationException("Plugin control has not been created.")).PopulateFromConnection(connection);
    }
}
