using System.Drawing;

namespace mRemoteNG.PluginContracts;

public sealed class TreeContextMenuAction
{
    public required string MenuText { get; init; }

    public Image? Icon { get; init; }

    public int SortOrder { get; init; }
}
