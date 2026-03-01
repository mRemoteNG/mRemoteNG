using System.Drawing;
using System.Runtime.Versioning;
using mRemoteNG.UI.Tabs;
using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.ThemeVS2015;

namespace mRemoteNG.Themes
{
    [SupportedOSPlatform("windows")]

    /// <summary>
    /// Visual Studio 2015 Light theme.
    /// </summary>
    public class MremoteNGThemeBase : VS2015ThemeBase
    {
        public MremoteNGThemeBase(byte[] themeResource)
            : base(themeResource)
        {
            Measures.SplitterSize = Properties.OptionsTabsPanelsPage.Default.SplitterSize;
            Measures.AutoHideSplitterSize = Properties.OptionsTabsPanelsPage.Default.SplitterSize;
            Measures.DockPadding = Properties.OptionsTabsPanelsPage.Default.DockPadding;
            ShowAutoHideContentOnHover = false;
        }
    }

    [SupportedOSPlatform("windows")]
    public class MremoteDockPaneStripFactory : DockPanelExtender.IDockPaneStripFactory
    {
        public DockPaneStripBase CreateDockPaneStrip(DockPane pane) => new DockPaneStripNG(pane);
    }

    public class MremoteFloatWindowFactory : DockPanelExtender.IFloatWindowFactory
    {
        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            Rectangle? activeDocumentBounds = (dockPanel.ActiveDocument as ConnectionTab)?.Bounds;

            return new FloatWindowNG(dockPanel, pane, activeDocumentBounds ?? bounds);
        }

        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane)
        {
            return new FloatWindowNG(dockPanel, pane);
        }
    }
}