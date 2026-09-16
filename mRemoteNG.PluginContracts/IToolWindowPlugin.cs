using System.Windows.Forms;

namespace mRemoteNG.PluginContracts;

public interface IToolWindowPlugin : IPlugin
{
    ToolWindowRegistration ToolWindow { get; }

    Control CreateControl();

    void OnBeforeShow(IPluginConnection? connection);
}
