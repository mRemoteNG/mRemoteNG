using System;
using System.Windows.Forms;
using mRemoteNG.PluginContracts;
using mRemoteNG.Resources;
using mRemoteNG.UI.Window;

namespace mRemoteNG.Plugins;

internal sealed class PluginToolWindow : BaseWindow
{
    public PluginToolWindow(IToolWindowPlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        ToolWindowRegistration registration = plugin.ToolWindow;
        Control control = plugin.CreateControl();

        Text = registration.WindowTitle;
        TabText = registration.WindowTitle;

        if (registration.Icon is System.Drawing.Bitmap bitmap)
        {
            Icon = ImageConverter.GetImageAsIcon(bitmap);
        }

        control.Dock = DockStyle.Fill;
        Controls.Add(control);
    }
}
