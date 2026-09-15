using System.Drawing;

namespace mRemoteNG.PluginContracts;

public sealed class ToolWindowRegistration
{
    public required string MenuText { get; init; }

    public required string WindowTitle { get; init; }

    public string? ContextMenuText { get; init; }

    public PluginContextMenuGroup ContextMenuGroup { get; init; }

    public Image? Icon { get; init; }

    public bool ShowInToolsMenu { get; init; } = true;

    public bool RequiresConnectionSelection { get; init; }

    public bool ShowAsDocument { get; init; } = true;

    public string? PanelName { get; init; }

    public int SortOrder { get; init; }
}
