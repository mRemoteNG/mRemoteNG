namespace mRemoteNG.PluginContracts;

public interface ITreeContextActionPlugin : IPlugin
{
    TreeContextMenuAction TreeContextMenuAction { get; }

    bool CanExecute(IPluginConnection connection);

    void Execute(IPluginConnection connection);
}
