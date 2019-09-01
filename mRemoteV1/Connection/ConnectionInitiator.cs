using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Panels;
using mRemoteNG.UI.Tabs;
using mRemoteNG.UI.Window;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.Connection
{
    public class ConnectionInitiator : IConnectionInitiator
    {
        private readonly ProtocolFactory _protocolFactory;
        private readonly List<ProtocolBase> _activeConnections = new List<ProtocolBase>();

        public IEnumerable<ProtocolBase> ActiveConnections => _activeConnections;

        public ConnectionInitiator(ProtocolFactory protocolFactory)
        {
            _protocolFactory = protocolFactory;
        }

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

        public void OpenConnection(
            ConnectionInfo connectionInfo,
            ConnectionInfo.Force force = ConnectionInfo.Force.None,
            ConnectionWindow conForm = null)
        {
            if (connectionInfo == null)
                return;

            try
            {
                if (connectionInfo.Hostname == "" && connectionInfo.Protocol != ProtocolType.IntApp)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                        Language.strConnectionOpenFailedNoHostname);
                    return;
                }

                StartPreConnectionExternalApp(connectionInfo);

                if (!force.HasFlag(ConnectionInfo.Force.DoNotJump))
                {
                    if (SwitchToOpenConnection(connectionInfo))
                        return;
                }

                var newProtocol = _protocolFactory.CreateProtocol(connectionInfo);

                var connectionPanel = SetConnectionPanel(connectionInfo, force);
                if (string.IsNullOrEmpty(connectionPanel)) return;
                var connectionForm = SetConnectionForm(conForm, connectionPanel);
                var connectionContainer = SetConnectionContainer(connectionInfo, connectionForm);
                SetConnectionFormEventHandlers(newProtocol, connectionForm);
                SetConnectionEventHandlers(newProtocol);
                BuildConnectionInterfaceController(connectionInfo, newProtocol, connectionContainer);

                newProtocol.Force = force;

                if (newProtocol.Initialize() == false)
                {
                    newProtocol.Close();
                    return;
                }

                OnConnectionStarting(newProtocol.InterfaceControl.Info, newProtocol);
                if (newProtocol.Connect() == false)
                {
                    newProtocol.Close();
                    return;
                }

                connectionInfo.OpenConnections.Add(newProtocol);
                _activeConnections.Add(newProtocol);
                FrmMain.Default.SelectedConnection = connectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strConnectionOpenFailed, ex);
            }
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
                    if (ic.Info == connectionInfo)
                        return ic;
                }
            }

            return null;
        }

        private string SetConnectionPanel(ConnectionInfo connectionInfo, ConnectionInfo.Force force)
        {
            if (connectionInfo.Panel != "" &&
                !force.HasFlag(ConnectionInfo.Force.OverridePanel) &&
                !Settings.Default.AlwaysShowPanelSelectionDlg)
                return connectionInfo.Panel;

            var frmPnl = new FrmChoosePanel(this);
            return frmPnl.ShowDialog() == DialogResult.OK
                ? frmPnl.Panel
                : null;
        }

        private ConnectionWindow SetConnectionForm(ConnectionWindow conForm, string connectionPanel)
        {
            var connectionForm = conForm ?? Runtime.WindowList.FromString(connectionPanel) as ConnectionWindow;

            if (connectionForm == null)
                connectionForm = new PanelAdder(this).AddPanel(connectionPanel);
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

                Runtime.MessageCollector.AddMessage(msgClass,
                    string.Format(
                        Language.strProtocolEventDisconnected,
                        disconnectedMessage,
                        prot.InterfaceControl.Info.Hostname,
                        prot.InterfaceControl.Info.Protocol.ToString()));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strProtocolEventDisconnectFailed, ex);
            }
        }

        private void Prot_Event_Closed(object sender)
        {
            try
            {
                var prot = (ProtocolBase)sender;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnenctionCloseEvent,
                                                    true);
                string connDetail;
                if (prot.InterfaceControl.Info.Hostname == "" &&
                    prot.InterfaceControl.Info.Protocol == ProtocolType.IntApp)
                    connDetail = prot.InterfaceControl.Info.ExtApp;
                else if (prot.InterfaceControl.Info.Hostname != "")
                    connDetail = prot.InterfaceControl.Info.Hostname;
                else
                    connDetail = "UNKNOWN";

                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                    string.Format(Language.strConnenctionClosedByUser, connDetail,
                                                                  prot.InterfaceControl.Info.Protocol,
                                                                  Environment.UserName));
                prot.InterfaceControl.Info.OpenConnections.Remove(prot);
                if (_activeConnections.Contains(prot))
                    _activeConnections.Remove(prot);

                if (prot.InterfaceControl.Info.PostExtApp == "") return;
                var extA = Runtime.ExternalToolsService.GetExtAppByName(prot.InterfaceControl.Info.PostExtApp);
                extA?.Start(prot.InterfaceControl.Info);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strConnenctionCloseEventFailed, ex);
            }
        }

        private void Prot_Event_Connected(object sender)
        {
            var prot = (ProtocolBase)sender;
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnectionEventConnected,
                                                true);
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                                                string.Format(Language.strConnectionEventConnectedDetail,
                                                              prot.InterfaceControl.Info.Hostname,
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
                                        Language.strConnectionEventErrorOccured,
                                        errorMessage,
                                        prot.InterfaceControl.Info.Hostname,
                                        errorCode?.ToString() ?? "-");
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, msg);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strConnectionEventConnectionFailed, ex);
            }
        }

        #endregion

        public event EventHandler<ConnectionStartingEvent> ConnectionStarting;
        protected virtual void OnConnectionStarting(ConnectionInfo connectionInfo, ProtocolBase protocolBase)
        {
            ConnectionStarting?.Invoke(this, new ConnectionStartingEvent(connectionInfo, protocolBase));
        }
    }
}