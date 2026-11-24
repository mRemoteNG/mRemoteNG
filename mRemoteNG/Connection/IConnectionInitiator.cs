using System.Collections.Generic;
using mRemoteNG.Container;
using mRemoteNG.UI.Panels;
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

        /// <summary>
        /// Opens a connection and completes once it has connected, failed or been closed, or after a time limit.
        /// </summary>
        /// <param name="panelName">The panel to open it in, created when needed; null to choose as usual.</param>
        /// <param name="floatingPlacement">Where that panel's window was when it was floating; null when docked.</param>
        /// <returns>True when the connection connected.</returns>
        System.Threading.Tasks.Task<bool> OpenConnectionAndWaitAsync(ConnectionInfo connectionInfo,
                                                                     string? panelName = null,
                                                                     FloatingPanelPlacement? floatingPlacement = null);

        bool SwitchToOpenConnection(ConnectionInfo connectionInfo);
    }
}