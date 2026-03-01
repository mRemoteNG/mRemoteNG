using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mRemoteNG.PluginSystem
{
    public interface IPluginHost
    {
        Form MainWindow { get; }
        IEnumerable<IConnectionNode> RootNodes { get; }

        event Action<string, string> OnConnectionOpened;
        event Action<string, string> OnConnectionClosed;

        void RegisterMenu(string text, Action onClick);
        void LogInfo(string message);
        void LogError(string message, Exception ex);
    }
}
