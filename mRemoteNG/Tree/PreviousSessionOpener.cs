using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Collections.Generic;
using mRemoteNG.UI.Panels;
using System.Linq;
using mRemoteNG.UI.Controls.ConnectionTree;
using System.Runtime.Versioning;

namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class PreviousSessionOpener : IConnectionTreeDelegate
    {
        private readonly IConnectionInitiator _connectionInitiator;
        private readonly Func<SessionPanels>? _loadSessionPanels;

        public PreviousSessionOpener(IConnectionInitiator connectionInitiator)
            : this(connectionInitiator, null)
        {
        }

        /// <param name="connectionInitiator">Opens the connections.</param>
        /// <param name="loadSessionPanels">
        /// Reads the panel each connection was in at the end of the previous session, and where the
        /// floating panels were. Called only when there are connections to reopen. Connections without a
        /// panel open as usual.
        /// </param>
        public PreviousSessionOpener(IConnectionInitiator connectionInitiator,
                                     Func<SessionPanels>? loadSessionPanels)
        {
            if (connectionInitiator == null)
                throw new ArgumentNullException(nameof(connectionInitiator));
            _connectionInitiator = connectionInitiator;
            _loadSessionPanels = loadSessionPanels;
        }

        public void Execute(IConnectionTree connectionTree)
        {
            System.Collections.Generic.IEnumerable<ConnectionInfo> connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList()
                                                   .Where(node => !(node is ContainerInfo));
            System.Collections.Generic.IEnumerable<ConnectionInfo> previouslyOpenedConnections = connectionInfoList
                .Where(item =>
                           item.PleaseConnect &&
                           //ignore items that have already connected
                           !_connectionInitiator.ActiveConnections.Contains(item.ConstantID));

            List<ConnectionInfo> connectionsToReopen = previouslyOpenedConnections.ToList();
            if (connectionsToReopen.Count == 0)
                return;

            SessionPanels sessionPanels = _loadSessionPanels?.Invoke() ?? SessionPanels.Empty;

            foreach (ConnectionInfo connectionInfo in connectionsToReopen)
            {
                if (sessionPanels.ConnectionPanels.TryGetValue(connectionInfo.ConstantID, out string? panelName) &&
                    !string.IsNullOrEmpty(panelName))
                {
                    sessionPanels.FloatingPanels.TryGetValue(panelName, out FloatingPanelPlacement? placement);
                    _connectionInitiator.OpenConnectionInPanel(connectionInfo, panelName, placement);
                }
                else
                {
                    _connectionInitiator.OpenConnection(connectionInfo);
                }
            }
        }
    }
}