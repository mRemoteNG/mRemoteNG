using System.Collections.Generic;
using mRemoteNG.Container;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Connection
{
    public interface IConnectionInitiator
    {
        IEnumerable<string> ActiveConnections { get; }

        void OpenConnection(
            ContainerInfo containerInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow? conForm = null,
            DockPane? targetPane = null,
            int? targetContentIndex = null);

        void OpenConnection(
            ConnectionInfo connectionInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow? conForm = null,
            DockPane? targetPane = null,
            int? targetContentIndex = null,
            DockContent? targetPanePlaceholder = null);

        void OpenConnection(
            ConnectionInfo originalConnectionInfo,
            ConnectionInfo connectionInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow? conForm = null,
            DockPane? targetPane = null,
            int? targetContentIndex = null,
            DockContent? targetPanePlaceholder = null);

        bool SwitchToOpenConnection(ConnectionInfo connectionInfo);
    }
}