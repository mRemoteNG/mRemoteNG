using System.Collections.Generic;

namespace mRemoteNG.PluginSystem
{
    public interface IConnectionNode
    {
        string Name { get; }
        string Hostname { get; }
        string Protocol { get; }
        string Password { get; }
        string Username { get; }
        string Domain { get; }
        string Description { get; }
        IEnumerable<IConnectionNode> Children { get; }
        IConnectionNode Parent { get; }
    }
}
