using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace mRemoteNG.Connection
{
    /// <summary>
    /// Orchestrates opening, closing, and managing remote connections.
    /// Responsible for creating protocol instances, wiring up lifecycle events
    /// (Connected, Disconnected, Closed, Error), hosting protocol controls in
    /// <see cref="ConnectionWindow"/> panels, and logging connection events
    /// via <see cref="ConnectionAuditLogger"/>.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class ConnectionInitiator : IConnectionInitiator
    {
        private readonly List<string> _activeConnections = [];
        private readonly IProtocolFactory _protocolFactory;
        private readonly ITunnelPortValidator _tunnelPortValidator;

        public IEnumerable<string> ActiveConnections => _activeConnections;

        public event Action<string, string> ConnectionOpened;
        public event Action<string, string> ConnectionClosed;

        public ConnectionInitiator(IProtocolFactory? protocolFactory = null, ITunnelPortValidator? tunnelPortValidator = null)
        {
            _protocolFactory = protocolFactory ?? new ProtocolFactory();
            _tunnelPortValidator = tunnelPortValidator ?? new TunnelPortValidator();
        }

        public bool SwitchToOpenConnection(ConnectionInfo connectionInfo)
        {
            InterfaceControl? interfaceControl = FindConnectionContainer(connectionInfo);
            ConnectionTab? connectionTab = interfaceControl?.FindForm() as ConnectionTab;

            if (connectionTab == null)
                connectionTab = FindConnectionTab(connectionInfo);

            if (connectionTab == null) return false;

            // Activate the parent ConnectionWindow (panel) in the main dock first,
            // so switching to a tab on a different panel works correctly (#2207).
            if (connectionTab.DockPanel?.FindForm() is ConnectionWindow parentWindow && FrmMain.IsCreated)
            {
                if (parentWindow.DockState == DockState.Unknown || parentWindow.DockState == DockState.Hidden || !parentWindow.Visible)
                    parentWindow.Show(FrmMain.Default.pnlDock, DockState.Document);
                else
                    parentWindow.Show(FrmMain.Default.pnlDock);
            }

            connectionTab.DockHandler.Activate();
            connectionTab.Focus();
            return true;
        }

        public void OpenConnection(
            ContainerInfo containerInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow? conForm = null)
        {
            if (containerInfo == null)
                return;

            // If the container itself has a hostname, connect it as a connection (#874)
            if (!string.IsNullOrEmpty(containerInfo.Hostname))
                OpenConnection((ConnectionInfo)containerInfo, force, conForm);

            if (containerInfo.Children.Count == 0)
                return;

            foreach (ConnectionInfo child in containerInfo.Children)
            {
                if (child is ContainerInfo childAsContainer)
                    OpenConnection(childAsContainer, force, conForm);
                else
                    OpenConnection(child, force, conForm);
            }
        }

        // async is necessary so UI can update while OpenConnection waits for tunnel connection to get ready in case of connection through SSH tunnel
        public async void OpenConnection(
            ConnectionInfo connectionInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow? conForm = null)
        {
            if (connectionInfo == null)
                return;

            if (connectionInfo.IsTemplate)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Connection '{connectionInfo.Name}' is a template and cannot be opened.");
                return;
            }

            try
            {
                ConnectionInfo connectionInfoOriginal = connectionInfo;
                bool useAlternativeAddress = force.HasFlag(ConnectionInfo.Force.UseAlternativeAddress) &&
                                             !string.IsNullOrWhiteSpace(connectionInfoOriginal.AlternativeAddress);

                if (useAlternativeAddress)
                {
                    connectionInfo = connectionInfoOriginal.Clone();
                    connectionInfo.Hostname = connectionInfoOriginal.AlternativeAddress;
                }

                if (force.HasFlag(ConnectionInfo.Force.NoCredentials))
                {
                    if (ReferenceEquals(connectionInfo, connectionInfoOriginal))
                    {
                        connectionInfo = connectionInfoOriginal.Clone();
                    }

                    connectionInfo.Parent = connectionInfoOriginal.Parent;
                    connectionInfo.Inheritance.Username = false;
                    connectionInfo.Inheritance.Password = false;
                    connectionInfo.Inheritance.Domain = false;
                    connectionInfo.Username = string.Empty;
                    connectionInfo.Password = string.Empty;
                    connectionInfo.Domain = string.Empty;
                }

                if (!useAlternativeAddress && !string.IsNullOrEmpty(connectionInfo.EC2InstanceId))
                {
                    try
                    {
                        string host = await ExternalConnectors.AWS.EC2FetchDataService.GetEC2InstanceDataAsync("AWSAPI:" + connectionInfo.EC2InstanceId, connectionInfo.EC2Region);
                        if (!string.IsNullOrEmpty(host))
                            connectionInfo.Hostname = host;
                    }
                    catch
                    {
                    }
                }

                if (string.IsNullOrEmpty(connectionInfo.Hostname))
                {
                    if (!string.IsNullOrEmpty(connectionInfo.Name))
                    {
                        if (ReferenceEquals(connectionInfo, connectionInfoOriginal))
                        {
                            connectionInfo = connectionInfoOriginal.Clone();
                            connectionInfo.Parent = connectionInfoOriginal.Parent;
                        }

                        connectionInfo.Hostname = connectionInfo.Name;
                        Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                            $"No hostname specified for '{connectionInfo.Name}', using connection name as hostname.");
                    }
                    else if (!ProtocolFeature.SupportBlankHostname(connectionInfo.Protocol))
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.ConnectionOpenFailedNoHostname);
                        return;
                    }
                }

                if (connectionInfo.WaitForIPAvailability && !string.IsNullOrWhiteSpace(connectionInfo.Hostname))
                {
                    int portToCheck = connectionInfo.Port > 0 ? connectionInfo.Port : connectionInfo.GetDefaultPort();
                    int timeoutSeconds = connectionInfo.WaitForIPTimeout > 0 ? connectionInfo.WaitForIPTimeout : 60;
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"Waiting for '{connectionInfo.Hostname}:{portToCheck}' to become reachable (timeout: {timeoutSeconds}s)...");
                    bool reachable = await WaitForIPAvailabilityAsync(connectionInfo.Hostname, portToCheck, timeoutSeconds).ConfigureAwait(false);
                    if (!reachable)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            $"'{connectionInfo.Hostname}' did not become reachable within {timeoutSeconds} seconds. Aborting connection.");
                        return;
                    }
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                        $"Host '{connectionInfo.Hostname}' is now reachable. Proceeding with connection.");
                }

                StartPreConnectionExternalApp(connectionInfo);

                bool switchToConnection = !force.HasFlag(ConnectionInfo.Force.DoNotJump);
                if (switchToConnection)
                {
                    if (SwitchToOpenConnection(connectionInfo))
                        return;
                }

                string? connectionPanel = SetConnectionPanel(connectionInfoOriginal, force);
                if (string.IsNullOrEmpty(connectionPanel)) return;
                ConnectionWindow? connectionForm = SetConnectionForm(conForm, connectionPanel, switchToConnection);
                if (connectionForm == null) return;
                Control? connectionContainer = null;

                // Handle connection through SSH tunnel:
                // in case of connection through SSH tunnel, connectionInfo gets cloned, so that modification of its name, hostname and port do not modify the original connection info
                // connectionInfoOriginal points to the original connection info in either case, for where its needed later on.
                ConnectionInfo? connectionInfoSshTunnel = null; // SSH tunnel connection info will be set if SSH tunnel connection is configured, can be found and connected.
                if (!string.IsNullOrEmpty(connectionInfoOriginal.SSHTunnelConnectionName))
                {
                    // Find the connection info specified as SSH tunnel in the connections tree
                    var treeModel = Runtime.ConnectionsService.ConnectionTreeModel;
                    if (treeModel == null)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(CultureInfo.InvariantCulture, Language.SshTunnelConfigProblem, connectionInfoOriginal.Name, connectionInfoOriginal.SSHTunnelConnectionName));
                        return;
                    }
                    connectionInfoSshTunnel = getSSHConnectionInfoByName(treeModel.RootNodes, connectionInfoOriginal.SSHTunnelConnectionName!);
                    if (connectionInfoSshTunnel == null)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(CultureInfo.InvariantCulture, Language.SshTunnelConfigProblem, connectionInfoOriginal.Name, connectionInfoOriginal.SSHTunnelConnectionName));
                        return;
                    }
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                        $"SSH Tunnel connection '{connectionInfoOriginal.SSHTunnelConnectionName}' configured for '{connectionInfoOriginal.Name}' found. Finding free local port for use as local tunnel port ...");

                    int localSshTunnelPort = 0;
                    bool tunnelStarted = false;
                    int retryCount = 0;
                    const int maxRetries = 3;

                    while (!tunnelStarted && retryCount < maxRetries)
                    {
                        retryCount++;
                        
                        // Determine a free local port to use as local tunnel port.
                        System.Net.Sockets.TcpListener l = new(System.Net.IPAddress.Loopback, 0);
                        l.Start();
                        localSshTunnelPort = ((System.Net.IPEndPoint)l.LocalEndpoint).Port;
                        l.Stop();
                        
                        Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                            $"Attempt {retryCount}: {localSshTunnelPort} will be used as local tunnel port. Establishing SSH connection to '{connectionInfoSshTunnel.Hostname}'...");

                        // clone SSH tunnel connection as tunnel options will be added to it, and those changes shall not be saved to the configuration
                        var currentTunnelInfo = connectionInfoSshTunnel.Clone();
                        currentTunnelInfo.SSHOptions += " -L " + localSshTunnelPort + ":" + connectionInfo.Hostname + ":" + connectionInfo.Port;

                        // connect the SSH connection to setup the tunnel
                        ProtocolBase protocolSshTunnel = _protocolFactory.CreateProtocol(currentTunnelInfo);
                        if (!(protocolSshTunnel is PuttyBase puttyBaseSshTunnel))
                        {
                            Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                string.Format(CultureInfo.InvariantCulture, Language.SshTunnelIsNotPutty, connectionInfoOriginal.Name, currentTunnelInfo.Name));
                            return;
                        }

                        SetConnectionFormEventHandlers(protocolSshTunnel, connectionForm);
                        SetConnectionEventHandlers(protocolSshTunnel);
                        connectionContainer = SetConnectionContainer(connectionInfo, connectionForm, switchToConnection);
                        if (connectionContainer == null) return;
                        BuildConnectionInterfaceController(currentTunnelInfo, protocolSshTunnel, connectionContainer);
                        protocolSshTunnel.InterfaceControl.OriginalInfo = currentTunnelInfo;

                        if (await protocolSshTunnel.InitializeAsync() == false)
                        {
                            protocolSshTunnel.Close();
                            continue;
                        }

                        if (protocolSshTunnel.Connect() == false)
                        {
                            protocolSshTunnel.Close();
                            continue;
                        }

                        // wait until SSH tunnel connection is ready, by checking if local port can be connected to
                        tunnelStarted = await _tunnelPortValidator.ValidatePortAsync(localSshTunnelPort);

                        if (tunnelStarted)
                        {
                            // Success! Now prepare the target connection info
                            connectionInfo = connectionInfoOriginal.Clone();
                            connectionInfo.Name += " via " + connectionInfoSshTunnel.Name;
                            connectionInfo.Hostname = "localhost";
                            connectionInfo.Port = localSshTunnelPort;

                            // Store references for cleanup and logging
                            connectionInfoSshTunnel = currentTunnelInfo;
                            
                            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                                "Local tunnel port is now available. Hiding putty display and setting up target connection via local tunnel port ...");

                            protocolSshTunnel.InterfaceControl.Hide();
                        }
                        else
                        {
                            protocolSshTunnel.Close();
                            Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                $"SSH Tunnel attempt {retryCount} failed on port {localSshTunnelPort}. Retrying with another port...");
                        }
                    }

                    if (!tunnelStarted)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                            string.Format(CultureInfo.InvariantCulture, Language.SshTunnelFailed, connectionInfoOriginal.Name, connectionInfoSshTunnel.Name));
                        return;
                    }
                }

                ProtocolBase newProtocol = _protocolFactory.CreateProtocol(connectionInfo);
                SetConnectionFormEventHandlers(newProtocol, connectionForm);
                SetConnectionEventHandlers(newProtocol);
                // in case of connection through SSH tunnel the container is already defined and must be use, else it needs to be created here
                if (connectionContainer == null) connectionContainer = SetConnectionContainer(connectionInfo, connectionForm, switchToConnection);
                if (connectionContainer == null) return;
                BuildConnectionInterfaceController(connectionInfo, newProtocol, connectionContainer);
                // in case of connection through SSH tunnel the connectionInfo was modified but connectionInfoOriginal in all cases retains the original info
                // and is stored in interface control for further use
                newProtocol.InterfaceControl.OriginalInfo = connectionInfoOriginal;
                // SSH tunnel connection is stored in Interface Control to be used in log messages etc
                newProtocol.InterfaceControl.SSHTunnelInfo = connectionInfoSshTunnel;

                newProtocol.Force = force;

                if (await newProtocol.InitializeAsync() == false)
                {
                    newProtocol.Close();
                    return;
                }

                if (newProtocol.Connect() == false)
                {
                    newProtocol.Close();
                    return;
                }

                connectionInfoOriginal.OpenConnections.Add(newProtocol);
                _activeConnections.Add(useAlternativeAddress ? connectionInfoOriginal.ConstantID : connectionInfo.ConstantID);
                if (FrmMain.IsCreated)
                    FrmMain.Default.SelectedConnection = useAlternativeAddress ? connectionInfoOriginal : connectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionOpenFailed, ex);
            }
        }

        // recursively traverse the tree to find ConnectionInfo of a specific name
        private static ConnectionInfo? getSSHConnectionInfoByName(IEnumerable<ConnectionInfo> rootnodes, string SSHTunnelConnectionName)
        {
            ConnectionInfo? result = null;
            foreach (ConnectionInfo node in rootnodes)
            {
                if (node is ContainerInfo container)
                {
                    result = getSSHConnectionInfoByName(container.Children, SSHTunnelConnectionName);
                }
                else
                {
                    if (node.Name == SSHTunnelConnectionName && (node.Protocol == ProtocolType.SSH1 || node.Protocol == ProtocolType.SSH2)) result = node;
                }
                if (result != null) break;
            }
            return result;
        }

        #region Private
        private static async Task<bool> WaitForIPAvailabilityAsync(string hostname, int port, int timeoutSeconds)
        {
            var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);
            const int retryIntervalMs = 2000;
            const int connectTimeoutMs = 1000;

            while (DateTime.UtcNow < deadline)
            {
                try
                {
                    using var tcpClient = new System.Net.Sockets.TcpClient();
                    using var cts = new System.Threading.CancellationTokenSource(connectTimeoutMs);
                    await tcpClient.ConnectAsync(hostname, port, cts.Token).ConfigureAwait(false);
                    return true;
                }
                catch
                {
                    // Host not reachable yet — wait before retrying.
                }

                int remainingMs = (int)(deadline - DateTime.UtcNow).TotalMilliseconds;
                if (remainingMs <= 0) break;
                await Task.Delay(Math.Min(retryIntervalMs, remainingMs)).ConfigureAwait(false);
            }

            return false;
        }

        private static void StartPreConnectionExternalApp(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.PreExtApp == "") return;
            Tools.ExternalTool? extA = Runtime.ExternalToolsService.GetExtAppByName(connectionInfo.PreExtApp);
            extA?.Start(connectionInfo);
        }

        private static InterfaceControl? FindConnectionContainer(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.OpenConnections.Count <= 0) return null;
            for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                // the new structure is ConnectionWindow.Controls[0].ActiveDocument.Controls[0]
                //                                       DockPanel                  InterfaceControl
                if (!(Runtime.WindowList[i] is ConnectionWindow connectionWindow)) continue;
                if (connectionWindow.Controls.Count < 1) continue;
                if (!(connectionWindow.Controls[0] is DockPanel cwDp)) continue;
                foreach (IDockContent dockContent in cwDp.Documents)
                {
                    ConnectionTab tab = (ConnectionTab)dockContent;
                    InterfaceControl? ic = InterfaceControl.FindInterfaceControl(tab);
                    if (ic == null) continue;
                    if (ic.Info == connectionInfo || ic.OriginalInfo == connectionInfo)
                        return ic;
                }
            }

            return null;
        }

        private static ConnectionTab? FindConnectionTab(ConnectionInfo connectionInfo)
        {
            for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                if (!(Runtime.WindowList[i] is ConnectionWindow connectionWindow)) continue;
                if (connectionWindow.Controls.Count < 1) continue;
                if (!(connectionWindow.Controls[0] is DockPanel dockPanel)) continue;

                foreach (IDockContent dockContent in dockPanel.Documents)
                {
                    if (dockContent is not ConnectionTab connectionTab) continue;

                    if (connectionTab.Tag is InterfaceControl interfaceControl)
                    {
                        if (AreEquivalentConnections(interfaceControl.Info, connectionInfo) ||
                            AreEquivalentConnections(interfaceControl.OriginalInfo, connectionInfo))
                            return connectionTab;
                        continue;
                    }

                    ConnectionInfo? trackedConnectionInfo = connectionTab.Tag as ConnectionInfo ?? connectionTab.TrackedConnectionInfo;
                    if (AreEquivalentConnections(trackedConnectionInfo, connectionInfo))
                        return connectionTab;
                }
            }

            return null;
        }

        /// <summary>
        /// Compares two connections by their effective target (hostname, port, protocol,
        /// username, domain) so that identical nodes in different tree branches are
        /// recognized as the same open connection (#2414).
        /// </summary>
        private static bool AreEquivalentConnections(ConnectionInfo? a, ConnectionInfo? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return string.Equals(a.Hostname, b.Hostname, StringComparison.OrdinalIgnoreCase) &&
                   a.Port == b.Port &&
                   a.Protocol == b.Protocol &&
                   string.Equals(a.Username, b.Username, StringComparison.Ordinal) &&
                   string.Equals(a.Domain, b.Domain, StringComparison.OrdinalIgnoreCase);
        }

        private static string? SetConnectionPanel(ConnectionInfo connectionInfo, ConnectionInfo.Force force)
        {
            if (connectionInfo.Panel != "" && !force.HasFlag(ConnectionInfo.Force.OverridePanel) && !Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg)
                return connectionInfo.Panel;

            FrmChoosePanel frmPnl = new();
            return frmPnl.ShowDialog() == DialogResult.OK
                ? frmPnl.Panel
                : null;
        }

        private static ConnectionWindow? SetConnectionForm(ConnectionWindow? conForm, string connectionPanel, bool switchToConnection)
        {
            ConnectionWindow? connectionForm = conForm ?? Runtime.WindowList.FromString(connectionPanel) as ConnectionWindow;

            if (connectionForm == null)
                // Don't show the panel immediately - it will be shown when first tab is added
                connectionForm = PanelAdder.AddPanel(connectionPanel, showImmediately: false);
            else if (switchToConnection)
                connectionForm.Show(FrmMain.Default.pnlDock);

            if (switchToConnection)
                connectionForm?.Focus();
            return connectionForm;
        }

        private static Control? SetConnectionContainer(ConnectionInfo connectionInfo, ConnectionWindow connectionForm, bool switchToConnection)
        {
            Control? connectionContainer = connectionForm.GetOrAddConnectionTab(connectionInfo, switchToConnection);

            if (connectionInfo.Protocol != ProtocolType.IntApp) return connectionContainer;

            Tools.ExternalTool? extT = Runtime.ExternalToolsService.GetExtAppByName(connectionInfo.ExtApp);

            if (extT == null) return connectionContainer;

            if (extT.Icon != null && connectionContainer is ConnectionTab connTab)
                connTab.Icon = extT.Icon;

            return connectionContainer;
        }

        private static void SetConnectionFormEventHandlers(ProtocolBase newProtocol, Form connectionForm)
        {
            newProtocol.Closed += ((ConnectionWindow)connectionForm).Prot_Event_Closed;
        }

        private void SetConnectionEventHandlers(ProtocolBase newProtocol)
        {
            newProtocol.Disconnected += Prot_Event_Disconnected;
            newProtocol.Connected += Prot_Event_Connected;
            newProtocol.Closed += Prot_Event_Closed;
            newProtocol.ErrorOccured += Prot_Event_ErrorOccured;
        }

        private static void BuildConnectionInterfaceController(ConnectionInfo connectionInfo,
                                                               ProtocolBase newProtocol,
                                                               Control connectionContainer)
        {
            newProtocol.InterfaceControl = new InterfaceControl(connectionContainer, newProtocol, connectionInfo);
        }

        #endregion

        #region Event handlers

        private static void Prot_Event_Disconnected(object sender, string disconnectedMessage, int? reasonCode)
        {
            try
            {
                ProtocolBase prot = (ProtocolBase)sender;
                MessageClass msgClass = MessageClass.InformationMsg;

                if (prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    if (reasonCode > 3)
                    {
                        msgClass = MessageClass.WarningMsg;
                    }
                }

                string strHostname = prot.InterfaceControl.OriginalInfo.Hostname;
                if (prot.InterfaceControl.SSHTunnelInfo != null)
                {
                    strHostname += " via SSH Tunnel " + prot.InterfaceControl.SSHTunnelInfo.Name;
                }
                Runtime.MessageCollector.AddMessage(msgClass,
                                                    string.Format(
                                                                  CultureInfo.InvariantCulture,
                                                                  Language.ProtocolEventDisconnected,
                                                                  disconnectedMessage,
                                                                  strHostname,
                                                                  prot.InterfaceControl.Info.Protocol.ToString()));
                ConnectionAuditLogger.LogConnectionDisconnected(strHostname, prot.InterfaceControl.Info.Protocol.ToString(), disconnectedMessage);

                prot.InterfaceControl.OriginalInfo.NotifyDisconnectedStateChanged();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ProtocolEventDisconnectFailed, ex);
            }
        }

        private void Prot_Event_Closed(object sender)
        {
            try
            {
                ProtocolBase prot = (ProtocolBase)sender;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ConnenctionCloseEvent, true);
                string connDetail;
                if (prot.InterfaceControl.OriginalInfo.Hostname == "" && prot.InterfaceControl.Info.Protocol == ProtocolType.IntApp)
                    connDetail = prot.InterfaceControl.Info.ExtApp;
                else if (prot.InterfaceControl.OriginalInfo.Hostname != "")
                    connDetail = prot.InterfaceControl.OriginalInfo.Hostname;
                else
                    connDetail = "UNKNOWN";

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(CultureInfo.InvariantCulture, Language.ConnenctionClosedByUser, connDetail, prot.InterfaceControl.Info.Protocol, Environment.UserName));
                ConnectionAuditLogger.LogConnectionClosed(connDetail, prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName);
                prot.InterfaceControl.OriginalInfo.OpenConnections.Remove(prot);
                if (_activeConnections.Contains(prot.InterfaceControl.Info.ConstantID))
                    _activeConnections.Remove(prot.InterfaceControl.Info.ConstantID);
                if (_activeConnections.Contains(prot.InterfaceControl.OriginalInfo.ConstantID))
                    _activeConnections.Remove(prot.InterfaceControl.OriginalInfo.ConstantID);

                ConnectionClosed?.Invoke(prot.InterfaceControl.OriginalInfo.Hostname, prot.InterfaceControl.Info.Protocol.ToString());

                if (prot.InterfaceControl.Info.PostExtApp == "") return;
                Tools.ExternalTool? extA = Runtime.ExternalToolsService.GetExtAppByName(prot.InterfaceControl.Info.PostExtApp);
                extA?.Start(prot.InterfaceControl.OriginalInfo);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnenctionCloseEventFailed, ex);
            }
        }

        private void Prot_Event_Connected(object sender)
        {
            try
            {
                if (sender is not ProtocolBase prot)
                    return;

                InterfaceControl? interfaceControl = prot.InterfaceControl;
                ConnectionInfo? originalInfo = interfaceControl?.OriginalInfo;
                ConnectionInfo? connectionInfo = interfaceControl?.Info;
                if (originalInfo == null || connectionInfo == null)
                    return;

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ConnectionEventConnected, true);
                Runtime.MessageCollector.AddMessage(
                    MessageClass.InformationMsg,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        Language.ConnectionEventConnectedDetail,
                        originalInfo.Hostname,
                        connectionInfo.Protocol,
                        Environment.UserName,
                        connectionInfo.Description,
                        connectionInfo.UserField));

                ConnectionAuditLogger.LogConnectionEstablished(
                    originalInfo.Hostname,
                    connectionInfo.Protocol.ToString(),
                    Environment.UserName);

                RecentConnectionsService.Instance.Add(originalInfo);

                // Update the Connections pane to highlight the newly established connection (#1869)
                if (FrmMain.IsCreated)
                {
                    FrmMain.Default.SelectedConnection = originalInfo;
                    ConnectionTreeWindow? treeForm = AppWindows.TreeForm;
                    if (treeForm != null && !treeForm.IsDisposed)
                    {
                        if (treeForm.InvokeRequired)
                        {
                            try
                            {
                                treeForm.BeginInvoke(new MethodInvoker(() =>
                                {
                                    try
                                    {
                                        if (!treeForm.IsDisposed)
                                            treeForm.JumpToNode(originalInfo, suppressPreview: true);
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        // Tree form was disposed before the queued callback executed.
                                    }
                                    catch (InvalidOperationException)
                                    {
                                        // Tree form handle became invalid before callback execution.
                                    }
                                }));
                            }
                            catch (ObjectDisposedException)
                            {
                                // Tree form was disposed while marshaling.
                            }
                            catch (InvalidOperationException)
                            {
                                // Tree form handle is no longer valid.
                            }
                        }
                        else
                        {
                            treeForm.JumpToNode(originalInfo, suppressPreview: true);
                        }
                    }
                }

                ConnectionOpened?.Invoke(originalInfo.Hostname, connectionInfo.Protocol.ToString());
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionFailed, ex);
            }
        }

        private static void Prot_Event_ErrorOccured(object sender, string errorMessage, int? errorCode)
        {
            try
            {
                ProtocolBase prot = (ProtocolBase)sender;

                string msg = string.Format(
                                        CultureInfo.InvariantCulture,
                                        Language.ConnectionEventErrorOccured,
                                        errorMessage,
                                        prot.InterfaceControl.OriginalInfo.Hostname,
                                        errorCode?.ToString(CultureInfo.InvariantCulture) ?? "-");
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, msg);
                ConnectionAuditLogger.LogConnectionError(prot.InterfaceControl.OriginalInfo.Hostname, prot.InterfaceControl.Info.Protocol.ToString(), errorMessage, errorCode);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionFailed, ex);
            }
        }

        #endregion
    }
}
