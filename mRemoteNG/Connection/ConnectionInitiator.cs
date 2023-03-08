using System;
using System.Collections.Generic;
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

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class ConnectionInitiator : IConnectionInitiator
    {
        private readonly PanelAdder _panelAdder = new PanelAdder();
        private readonly List<string> _activeConnections = new List<string>();

        public IEnumerable<string> ActiveConnections => _activeConnections;

        public bool SwitchToOpenConnection(ConnectionInfo connectionInfo)
        {
            var interfaceControl = FindConnectionContainer(connectionInfo);
            if (interfaceControl == null) return false;
            var connT = (ConnectionTab)interfaceControl.FindForm();
            connT?.Focus();
            var findForm = (ConnectionTab)interfaceControl.FindForm();
            findForm?.Show(findForm.DockPanel);
            return true;
        }

        public void OpenConnection(
            ContainerInfo containerInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow conForm = null)
        {
            if (containerInfo == null || containerInfo.Children.Count == 0)
                return;

            foreach (var child in containerInfo.Children)
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
            ConnectionWindow conForm = null)
        {
            if (connectionInfo == null)
                return;

            try
            {
                if (!string.IsNullOrEmpty(connectionInfo.EC2InstanceId))
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

                if (connectionInfo.Hostname == "" && connectionInfo.Protocol != ProtocolType.IntApp)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                        Language.ConnectionOpenFailedNoHostname);
                    return;
                }

                StartPreConnectionExternalApp(connectionInfo);

                if (!force.HasFlag(ConnectionInfo.Force.DoNotJump))
                {
                    if (SwitchToOpenConnection(connectionInfo))
                        return;
                }

                var protocolFactory = new ProtocolFactory();
                var connectionPanel = SetConnectionPanel(connectionInfo, force);
                if (string.IsNullOrEmpty(connectionPanel)) return;
                var connectionForm = SetConnectionForm(conForm, connectionPanel);
                Control connectionContainer = null;

                // Handle connection through SSH tunnel:
                // in case of connection through SSH tunnel, connectionInfo gets cloned, so that modification of its name, hostname and port do not modify the original connection info
                // connectionInfoOriginal points to the original connection info in either case, for where its needed later on.
                var connectionInfoOriginal = connectionInfo;
                ConnectionInfo connectionInfoSshTunnel = null; // SSH tunnel connection info will be set if SSH tunnel connection is configured, can be found and connected.
                if (!string.IsNullOrEmpty(connectionInfoOriginal.SSHTunnelConnectionName))
                {
                    // Find the connection info specified as SSH tunnel in the connections tree
                    connectionInfoSshTunnel = getSSHConnectionInfoByName(Runtime.ConnectionsService.ConnectionTreeModel.RootNodes, connectionInfoOriginal.SSHTunnelConnectionName);
                    if (connectionInfoSshTunnel == null)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            string.Format(Language.SshTunnelConfigProblem, connectionInfoOriginal.Name, connectionInfoOriginal.SSHTunnelConnectionName));
                        return;
                    }
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                        $"SSH Tunnel connection '{connectionInfoOriginal.SSHTunnelConnectionName}' configured for '{connectionInfoOriginal.Name}' found. Finding free local port for use as local tunnel port ...");
                    // determine a free local port to use as local tunnel port
                    var l = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
                    l.Start();
                    var localSshTunnelPort = ((System.Net.IPEndPoint)l.LocalEndpoint).Port;
                    l.Stop();
                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                        $"{localSshTunnelPort} will be used as local tunnel port. Establishing SSH connection to '{connectionInfoSshTunnel.Hostname}' with additional tunnel options for target connection ...");

                    // clone SSH tunnel connection as tunnel options will be added to it, and those changes shall not be saved to the configuration
                    connectionInfoSshTunnel = connectionInfoSshTunnel.Clone();
                    connectionInfoSshTunnel.SSHOptions += " -L " + localSshTunnelPort + ":" + connectionInfoOriginal.Hostname + ":" + connectionInfoOriginal.Port;

                    // clone target connection info as its hostname will be changed to localhost and port to local tunnel port to establish connection through tunnel, and those changes shall not be saved to the configuration
                    connectionInfo = connectionInfoOriginal.Clone();
                    connectionInfo.Name += " via " + connectionInfoSshTunnel.Name;
                    connectionInfo.Hostname = "localhost";
                    connectionInfo.Port = localSshTunnelPort;

                    // connect the SSH connection to setup the tunnel
                    var protocolSshTunnel = protocolFactory.CreateProtocol(connectionInfoSshTunnel);
                    if (!(protocolSshTunnel is PuttyBase puttyBaseSshTunnel))
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            string.Format(Language.SshTunnelIsNotPutty, connectionInfoOriginal.Name, connectionInfoSshTunnel.Name));
                        return;
                    }

                    SetConnectionFormEventHandlers(protocolSshTunnel, connectionForm);
                    SetConnectionEventHandlers(protocolSshTunnel);
                    connectionContainer = SetConnectionContainer(connectionInfo, connectionForm);
                    BuildConnectionInterfaceController(connectionInfoSshTunnel, protocolSshTunnel, connectionContainer);
                    protocolSshTunnel.InterfaceControl.OriginalInfo = connectionInfoSshTunnel;

                    if (protocolSshTunnel.Initialize() == false)
                    {
                        protocolSshTunnel.Close();
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            string.Format(Language.SshTunnelNotInitialized, connectionInfoOriginal.Name, connectionInfoSshTunnel.Name));
                        return;
                    }

                    if (protocolSshTunnel.Connect() == false)
                    {
                        protocolSshTunnel.Close();
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            string.Format(Language.SshTunnelNotConnected, connectionInfoOriginal.Name, connectionInfoSshTunnel.Name));
                        return;
                    }

                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                        "Putty started for SSH connection for tunnel. Waiting for local tunnel port to become available ...");

                    // wait until SSH tunnel connection is ready, by checking if local port can be connected to, but max 60 sec.
                    var testsock = new System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    while (stopwatch.ElapsedMilliseconds < 60000)
                    {
                        // confirm that SSH connection is still active
                        // works only if putty is connfigured to always close window on exit
                        // else, if connection attempt fails, window remains open and putty process remains running, and we cannot know that connection is already doomed
                        // in this case the timeout will expire and the log message below will be created
                        // awkward for user as he has already acknowledged the putty popup some seconds again when the below notification comes....
                        if (!puttyBaseSshTunnel.isRunning())
                        {
                            protocolSshTunnel.Close();
                            Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                string.Format(Language.SshTunnelFailed, connectionInfoOriginal.Name, connectionInfoSshTunnel.Name));
                            return;
                        }

                        try
                        {
                            testsock.Connect(System.Net.IPAddress.Loopback, localSshTunnelPort);
                            testsock.Close();
                            break;
                        }
                        catch
                        {
                            await System.Threading.Tasks.Task.Delay(1000);
                        }
                    }

                    if (stopwatch.ElapsedMilliseconds >= 60000)
                    {
                        protocolSshTunnel.Close();
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            string.Format(Language.SshTunnelPortNotReadyInTime, connectionInfoOriginal.Name, connectionInfoSshTunnel.Name));
                        return;
                    }

                    Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg,
                        "Local tunnel port is now available. Hiding putty display and setting up target connection via local tunnel port ...");

                    // hide the display of the SSH tunnel connection which has been shown until this time, such that password can be entered if required or errors be seen
                    // it stays invisible in the container however which will be reused for the actual connection and such that if the container is closed the SSH tunnel connection is closed as well
                    protocolSshTunnel.InterfaceControl.Hide();
                }

                var newProtocol = protocolFactory.CreateProtocol(connectionInfo);
                SetConnectionFormEventHandlers(newProtocol, connectionForm);
                SetConnectionEventHandlers(newProtocol);
                // in case of connection through SSH tunnel the container is already defined and must be use, else it needs to be created here
                if (connectionContainer == null) connectionContainer = SetConnectionContainer(connectionInfo, connectionForm);
                BuildConnectionInterfaceController(connectionInfo, newProtocol, connectionContainer);
                // in case of connection through SSH tunnel the connectionInfo was modified but connectionInfoOriginal in all cases retains the original info
                // and is stored in interface control for further use
                newProtocol.InterfaceControl.OriginalInfo = connectionInfoOriginal;
                // SSH tunnel connection is stored in Interface Control to be used in log messages etc
                newProtocol.InterfaceControl.SSHTunnelInfo = connectionInfoSshTunnel;

                newProtocol.Force = force;

                if (newProtocol.Initialize() == false)
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
                _activeConnections.Add(connectionInfo.ConstantID);
                FrmMain.Default.SelectedConnection = connectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionOpenFailed, ex);
            }
        }

        // recursively traverse the tree to find ConnectionInfo of a specific name
        private ConnectionInfo getSSHConnectionInfoByName(IEnumerable<ConnectionInfo> rootnodes, string SSHTunnelConnectionName)
        {
            ConnectionInfo result = null;
            foreach (var node in rootnodes)
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
        private static void StartPreConnectionExternalApp(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.PreExtApp == "") return;
            var extA = Runtime.ExternalToolsService.GetExtAppByName(connectionInfo.PreExtApp);
            extA?.Start(connectionInfo);
        }

        private static InterfaceControl FindConnectionContainer(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.OpenConnections.Count <= 0) return null;
            for (var i = 0; i <= Runtime.WindowList.Count - 1; i++)
            {
                // the new structure is ConnectionWindow.Controls[0].ActiveDocument.Controls[0]
                //                                       DockPanel                  InterfaceControl
                if (!(Runtime.WindowList[i] is ConnectionWindow connectionWindow)) continue;
                if (connectionWindow.Controls.Count < 1) continue;
                if (!(connectionWindow.Controls[0] is DockPanel cwDp)) continue;
                foreach (var dockContent in cwDp.Documents)
                {
                    var tab = (ConnectionTab)dockContent;
                    var ic = InterfaceControl.FindInterfaceControl(tab);
                    if (ic == null) continue;
                    if (ic.Info == connectionInfo || ic.OriginalInfo == connectionInfo)
                        return ic;
                }
            }

            return null;
        }

        private static string SetConnectionPanel(ConnectionInfo connectionInfo, ConnectionInfo.Force force)
        {
            if (connectionInfo.Panel != "" && !force.HasFlag(ConnectionInfo.Force.OverridePanel) && !Properties.OptionsTabsPanelsPage.Default.AlwaysShowPanelSelectionDlg)
                return connectionInfo.Panel;

            var frmPnl = new FrmChoosePanel();
            return frmPnl.ShowDialog() == DialogResult.OK
                ? frmPnl.Panel
                : null;
        }

        private ConnectionWindow SetConnectionForm(ConnectionWindow conForm, string connectionPanel)
        {
            var connectionForm = conForm ?? Runtime.WindowList.FromString(connectionPanel) as ConnectionWindow;

            if (connectionForm == null)
                connectionForm = _panelAdder.AddPanel(connectionPanel);
            else
                connectionForm.Show(FrmMain.Default.pnlDock);

            connectionForm.Focus();
            return connectionForm;
        }

        private static Control SetConnectionContainer(ConnectionInfo connectionInfo, ConnectionWindow connectionForm)
        {
            Control connectionContainer = connectionForm.AddConnectionTab(connectionInfo);

            if (connectionInfo.Protocol != ProtocolType.IntApp) return connectionContainer;

            var extT = Runtime.ExternalToolsService.GetExtAppByName(connectionInfo.ExtApp);

            if (extT == null) return connectionContainer;

            if (extT.Icon != null)
                ((ConnectionTab)connectionContainer).Icon = extT.Icon;

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
                var prot = (ProtocolBase)sender;
                var msgClass = MessageClass.InformationMsg;

                if (prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    if (reasonCode > 3)
                    {
                        msgClass = MessageClass.WarningMsg;
                    }
                }

                var strHostname = prot.InterfaceControl.OriginalInfo.Hostname;
                if (prot.InterfaceControl.SSHTunnelInfo != null)
                {
                    strHostname += " via SSH Tunnel " + prot.InterfaceControl.SSHTunnelInfo.Name;
                }
                Runtime.MessageCollector.AddMessage(msgClass,
                                                    string.Format(
                                                                  Language.ProtocolEventDisconnected,
                                                                  disconnectedMessage,
                                                                  strHostname,
                                                                  prot.InterfaceControl.Info.Protocol.ToString()));
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
                var prot = (ProtocolBase)sender;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ConnenctionCloseEvent,
                                                    true);
                string connDetail;
                if (prot.InterfaceControl.OriginalInfo.Hostname == "" &&
                    prot.InterfaceControl.Info.Protocol == ProtocolType.IntApp)
                    connDetail = prot.InterfaceControl.Info.ExtApp;
                else if (prot.InterfaceControl.OriginalInfo.Hostname != "")
                    connDetail = prot.InterfaceControl.OriginalInfo.Hostname;
                else
                    connDetail = "UNKNOWN";

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    string.Format(Language.ConnenctionClosedByUser, connDetail,
                                                                  prot.InterfaceControl.Info.Protocol,
                                                                  Environment.UserName));
                prot.InterfaceControl.OriginalInfo.OpenConnections.Remove(prot);
                if (_activeConnections.Contains(prot.InterfaceControl.Info.ConstantID))
                    _activeConnections.Remove(prot.InterfaceControl.Info.ConstantID);

                if (prot.InterfaceControl.Info.PostExtApp == "") return;
                var extA = Runtime.ExternalToolsService.GetExtAppByName(prot.InterfaceControl.Info.PostExtApp);
                extA?.Start(prot.InterfaceControl.OriginalInfo);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnenctionCloseEventFailed, ex);
            }
        }

        private static void Prot_Event_Connected(object sender)
        {
            var prot = (ProtocolBase)sender;
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.ConnectionEventConnected,
                                                true);
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                string.Format(Language.ConnectionEventConnectedDetail,
                                                              prot.InterfaceControl.OriginalInfo.Hostname,
                                                              prot.InterfaceControl.Info.Protocol, Environment.UserName,
                                                              prot.InterfaceControl.Info.Description,
                                                              prot.InterfaceControl.Info.UserField));
        }

        private static void Prot_Event_ErrorOccured(object sender, string errorMessage, int? errorCode)
        {
            try
            {
                var prot = (ProtocolBase)sender;

                var msg = string.Format(
                                        Language.ConnectionEventErrorOccured,
                                        errorMessage,
                                        prot.InterfaceControl.OriginalInfo.Hostname,
                                        errorCode?.ToString() ?? "-");
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, msg);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.ConnectionFailed, ex);
            }
        }

        #endregion
    }
}