using System;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;
using mRemoteNG.Resources;
using mRemoteNG.UI.Window;

namespace mRemoteNG.Plugins;

internal sealed class PluginToolWindow : BaseWindow
{
    private readonly IToolWindowPlugin _plugin;

    public PluginToolWindow(IToolWindowPlugin plugin)
    {
        _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));

        ToolWindowRegistration registration = _plugin.ToolWindow;
        Text = registration.WindowTitle;
        TabText = registration.WindowTitle;
        CloseButton = true;
        CloseButtonVisible = true;
        HideOnClose = !registration.ShowAsDocument;
        DockAreas = registration.ShowAsDocument ? WeifenLuo.WinFormsUI.Docking.DockAreas.Document | WeifenLuo.WinFormsUI.Docking.DockAreas.Float : DockAreas;

        if (registration.Icon is System.Drawing.Bitmap bitmap)
        {
            Icon = ImageConverter.GetImageAsIcon(bitmap);
        }

        EnsurePluginControl();
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        EnsurePluginControl();
        base.OnVisibleChanged(e);
    }

    private void EnsurePluginControl()
    {
        Control? existingControl = Controls.Count > 0 ? Controls[0] : null;
        if (existingControl != null && !existingControl.IsDisposed)
        {
            return;
        }

        Controls.Clear();
        Control control = _plugin.CreateControl();
        control.Dock = DockStyle.Fill;
        Controls.Add(control);
    }
}
