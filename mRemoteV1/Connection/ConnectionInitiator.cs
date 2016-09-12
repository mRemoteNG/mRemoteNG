using System;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.Window;
using TabPage = Crownwood.Magic.Controls.TabPage;


namespace mRemoteNG.Connection
{
    public static class ConnectionInitiator
    {
        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo connectionInfo)
        {
            try
            {
                OpenConnection(connectionInfo, ConnectionInfo.Force.None);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo connectionInfo, Form connectionForm, ConnectionInfo.Force force)
        {
            try
            {
                OpenConnection(connectionInfo, force, connectionForm);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo connectionInfo, ConnectionInfo.Force force)
        {
            try
            {
                OpenConnection(connectionInfo, force, null);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo connectionInfo, ConnectionInfo.Force force, Form conForm)
        {
            try
            {
                if (connectionInfo.Hostname == "" && connectionInfo.Protocol != ProtocolType.IntApp)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strConnectionOpenFailedNoHostname);
                    return;
                }

                StartPreConnectionExternalApp(connectionInfo);

                if ((force & ConnectionInfo.Force.DoNotJump) != ConnectionInfo.Force.DoNotJump)
                {
                    if (SwitchToOpenConnection(connectionInfo))
                        return;
                }

                var protocolFactory = new ProtocolFactory();
                var newProtocol = protocolFactory.CreateProtocol(connectionInfo);

                var connectionPanel = SetConnectionPanel(connectionInfo, force);
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

                if (newProtocol.Connect() == false)
                {
                    newProtocol.Close();
                    return;
                }

                connectionInfo.OpenConnections.Add(newProtocol);
                SetTreeNodeImages(connectionInfo);
                frmMain.Default.SelectedConnection = connectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        private static void StartPreConnectionExternalApp(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.PreExtApp != "")
            {
                var extA = GetExtAppByName(connectionInfo.PreExtApp);
                extA?.Start(connectionInfo);
            }
        }

        public static ExternalTool GetExtAppByName(string name)
        {
            foreach (ExternalTool extA in Runtime.ExternalTools)
            {
                if (extA.DisplayName == name)
                    return extA;
            }
            return null;
        }

        public static bool SwitchToOpenConnection(ConnectionInfo nCi)
        {
            var IC = FindConnectionContainer(nCi);
            if (IC != null)
            {
                var connectionWindow = (ConnectionWindow)IC.FindForm();
                connectionWindow?.Focus();
                var findForm = (ConnectionWindow)IC.FindForm();
                findForm?.Show(frmMain.Default.pnlDock);
                var tabPage = (TabPage)IC.Parent;
                tabPage.Selected = true;
                return true;
            }
            return false;
        }

        private static InterfaceControl FindConnectionContainer(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.OpenConnections.Count > 0)
            {
                for (int i = 0; i <= Runtime.WindowList.Count - 1; i++)
                {
                    if (Runtime.WindowList[i] is ConnectionWindow)
                    {
                        var connectionWindow = (ConnectionWindow)Runtime.WindowList[i];
                        if (connectionWindow.TabController != null)
                        {
                            foreach (TabPage t in connectionWindow.TabController.TabPages)
                            {
                                if (t.Controls[0] != null && t.Controls[0] is InterfaceControl)
                                {
                                    var IC = (InterfaceControl)t.Controls[0];
                                    if (IC.Info == connectionInfo)
                                    {
                                        return IC;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static string SetConnectionPanel(ConnectionInfo connectionInfo, ConnectionInfo.Force Force)
        {
            string connectionPanel = "";
            if (connectionInfo.Panel == "" || (Force & ConnectionInfo.Force.OverridePanel) == ConnectionInfo.Force.OverridePanel | Settings.Default.AlwaysShowPanelSelectionDlg)
            {
                frmChoosePanel frmPnl = new frmChoosePanel();
                if (frmPnl.ShowDialog() == DialogResult.OK)
                {
                    connectionPanel = frmPnl.Panel;
                }
            }
            else
            {
                connectionPanel = connectionInfo.Panel;
            }
            return connectionPanel;
        }

        private static Form SetConnectionForm(Form conForm, string connectionPanel)
        {
            var connectionForm = conForm ?? Runtime.WindowList.FromString(connectionPanel);

            if (connectionForm == null)
                connectionForm = Runtime.AddPanel(connectionPanel);
            else
                ((ConnectionWindow)connectionForm).Show(frmMain.Default.pnlDock);

            connectionForm.Focus();
            return connectionForm;
        }

        private static Control SetConnectionContainer(ConnectionInfo connectionInfo, Form connectionForm)
        {
            Control connectionContainer = ((ConnectionWindow)connectionForm).AddConnectionTab(connectionInfo);

            if (connectionInfo.Protocol == ProtocolType.IntApp)
            {
                if (GetExtAppByName(connectionInfo.ExtApp).Icon != null)
                    ((TabPage)connectionContainer).Icon = GetExtAppByName(connectionInfo.ExtApp).Icon;
            }
            return connectionContainer;
        }

        private static void SetConnectionFormEventHandlers(ProtocolBase newProtocol, Form connectionForm)
        {
            newProtocol.Closed += ((ConnectionWindow)connectionForm).Prot_Event_Closed;
        }

        private static void SetConnectionEventHandlers(ProtocolBase newProtocol)
        {
            newProtocol.Disconnected += Prot_Event_Disconnected;
            newProtocol.Connected += Prot_Event_Connected;
            newProtocol.Closed += Prot_Event_Closed;
            newProtocol.ErrorOccured += Prot_Event_ErrorOccured;
        }

        private static void SetTreeNodeImages(ConnectionInfo connectionInfo)
        {
            if (connectionInfo.IsQuickConnect == false)
            {
                if (connectionInfo.Protocol != ProtocolType.IntApp)
                {
                    ConnectionTreeNode.SetNodeImage(connectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                }
                else
                {
                    ExternalTool extApp = GetExtAppByName(connectionInfo.ExtApp);
                    if (extApp != null)
                    {
                        if (extApp.TryIntegrate && connectionInfo.TreeNode != null)
                        {
                            ConnectionTreeNode.SetNodeImage(connectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                        }
                    }
                }
            }
        }

        private static void BuildConnectionInterfaceController(ConnectionInfo connectionInfo, ProtocolBase newProtocol, Control connectionContainer)
        {
            newProtocol.InterfaceControl = new InterfaceControl(connectionContainer, newProtocol, connectionInfo);
        }




        private static void Prot_Event_Disconnected(object sender, string disconnectedMessage)
        {
            try
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strProtocolEventDisconnected, disconnectedMessage), true);

                ProtocolBase Prot = (ProtocolBase)sender;
                if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    string ReasonCode = disconnectedMessage.Split("\r\n".ToCharArray())[0];
                    string desc = disconnectedMessage.Replace("\r\n", " ");

                    if (Convert.ToInt32(ReasonCode) > 3)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strRdpDisconnected + Environment.NewLine + desc);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strProtocolEventDisconnectFailed, ex.Message), true);
            }
        }

        private static void Prot_Event_Closed(object sender)
        {
            try
            {
                ProtocolBase Prot = (ProtocolBase)sender;
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnenctionCloseEvent, true);
                Runtime.MessageCollector.AddMessage(MessageClass.ReportMsg, string.Format(Language.strConnenctionClosedByUser, Prot.InterfaceControl.Info.Hostname, Prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName));
                Prot.InterfaceControl.Info.OpenConnections.Remove(Prot);

                if (Prot.InterfaceControl.Info.OpenConnections.Count < 1 && Prot.InterfaceControl.Info.IsQuickConnect == false)
                {
                    ConnectionTreeNode.SetNodeImage(Prot.InterfaceControl.Info.TreeNode, TreeImageType.ConnectionClosed);
                }

                if (Prot.InterfaceControl.Info.PostExtApp != "")
                {
                    ExternalTool extA = GetExtAppByName(Prot.InterfaceControl.Info.PostExtApp);
                    extA?.Start(Prot.InterfaceControl.Info);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnenctionCloseEventFailed + Environment.NewLine + ex.Message, true);
            }
        }

        private static void Prot_Event_Connected(object sender)
        {
            ProtocolBase prot = (ProtocolBase)sender;
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnectionEventConnected, true);
            Runtime.MessageCollector.AddMessage(MessageClass.ReportMsg, string.Format(Language.strConnectionEventConnectedDetail, prot.InterfaceControl.Info.Hostname, prot.InterfaceControl.Info.Protocol.ToString(), Environment.UserName, prot.InterfaceControl.Info.Description, prot.InterfaceControl.Info.UserField));
        }

        private static void Prot_Event_ErrorOccured(object sender, string errorMessage)
        {
            try
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnectionEventErrorOccured, true);
                ProtocolBase Prot = (ProtocolBase)sender;

                if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    if (Convert.ToInt32(errorMessage) > -1)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.strConnectionRdpErrorDetail, errorMessage, ProtocolRDP.FatalErrors.GetError(errorMessage)));
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionEventConnectionFailed + Environment.NewLine + ex.Message, true);
            }
        }
    }
}