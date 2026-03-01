using System;

namespace mRemoteNG.PluginSystem
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }
        void Initialize(IPluginHost host);
        void Shutdown();
    }
}
