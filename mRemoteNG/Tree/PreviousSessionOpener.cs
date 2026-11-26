using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Collections.Generic;
using mRemoteNG.UI.Panels;
using System.Linq;
using System.Threading.Tasks;
using mRemoteNG.UI.Controls.ConnectionTree;
using System.Runtime.Versioning;

namespace mRemoteNG.Tree
{
    /// <summary>
    /// Reopens the connections that were open at the end of the previous session, in the panels they were in.
    /// </summary>
    /// <remarks>
    /// The connections are opened one at a time, each once the one before has connected, failed or been
    /// closed. A connection can ask for credentials, and an RDP connection that waits behind another
    /// connection's login prompt runs out of time (the RDP connection timeout counts the time its own
    /// prompt waits too). Opened all at once, several RDP connections without saved credentials showed
    /// their prompts together and most of them timed out before they could be answered.
    /// </remarks>
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

        /// <summary>
        /// Starts reopening the connections and returns; they open one after another in the background
        /// of the UI thread.
        /// </summary>
        public void Execute(IConnectionTree connectionTree)
        {
            _ = ReopenAsync(connectionTree);
        }

        /// <summary>Reopens the connections one at a time. Completes when the last one is done.</summary>
        internal async Task ReopenAsync(IConnectionTree connectionTree)
        {
            IEnumerable<ConnectionInfo> connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList()
                                                   .Where(node => !(node is ContainerInfo));
            List<ConnectionInfo> connectionsToReopen = connectionInfoList
                .Where(item =>
                           item.PleaseConnect &&
                           //ignore items that have already connected
                           !_connectionInitiator.ActiveConnections.Contains(item.ConstantID))
                .ToList();
            if (connectionsToReopen.Count == 0)
                return;

            SessionPanels sessionPanels = _loadSessionPanels?.Invoke() ?? SessionPanels.Empty;

            foreach (ConnectionInfo connectionInfo in connectionsToReopen)
            {
                string? panelName = null;
                FloatingPanelPlacement? placement = null;
                if (sessionPanels.ConnectionPanels.TryGetValue(connectionInfo.ConstantID, out string? savedPanel) &&
                    !string.IsNullOrEmpty(savedPanel))
                {
                    panelName = savedPanel;
                    sessionPanels.FloatingPanels.TryGetValue(savedPanel, out placement);
                }

                try
                {
                    await _connectionInitiator.OpenConnectionAndWaitAsync(connectionInfo, panelName, placement);
                }
                catch (Exception ex)
                {
                    // One connection that cannot be reopened must not keep the others closed
                    Runtime.MessageCollector?.AddExceptionMessage(
                        "Reopening " + connectionInfo.Name + " from the previous session failed", ex);
                }
            }
        }
    }
}
