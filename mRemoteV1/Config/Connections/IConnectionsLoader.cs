using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public interface IConnectionsLoader
    {
        ConnectionTreeModel Load();
    }
}