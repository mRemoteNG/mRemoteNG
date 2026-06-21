using System;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    [SupportedOSPlatform("windows")]
    public interface ISlowClickRenameTimer : IDisposable
    {
        int Interval { get; set; }
        bool Enabled { get; }
        event EventHandler Tick;
        void Start();
        void Stop();
    }
}