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
        public static void OpenConnection(ConnectionInfo.Force Force)
        {
            try
            {
                if (Windows.treeForm.tvConnections.SelectedNode.Tag == null)
                    return;

                if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
                {
                    OpenConnection((ConnectionInfo)Windows.treeForm.tvConnections.SelectedNode.Tag, Force);
                }
                else if (ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.Container)
                {
                    foreach (TreeNode tNode in ConnectionTree.SelectedNode.Nodes)
                    {
                        if (ConnectionTreeNode.GetNodeType(tNode) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(ConnectionTree.SelectedNode) == TreeNodeType.PuttySession)
                        {
                            if (tNode.Tag != null)
                            {
                                OpenConnection((ConnectionInfo)tNode.Tag, Force);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo ConnectionInfo)
        {
            try
            {
                OpenConnection(ConnectionInfo, ConnectionInfo.Force.None);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo ConnectionInfo, Form ConnectionForm, ConnectionInfo.Force Force)
        {
            try
            {
                OpenConnection(ConnectionInfo, Force, ConnectionForm);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force)
        {
            try
            {
                OpenConnection(ConnectionInfo, Force, null);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        //TODO Fix for TreeListView
        public static void OpenConnection(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force, Form ConForm)
        {
            try
            {
                if (ConnectionInfo.Hostname == "" && ConnectionInfo.Protocol != ProtocolType.IntApp)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strConnectionOpenFailedNoHostname);
                    return;
                }

                StartPreConnectionExternalApp(ConnectionInfo);

                if ((Force & ConnectionInfo.Force.DoNotJump) != ConnectionInfo.Force.DoNotJump)
                {
                    if (SwitchToOpenConnection(ConnectionInfo))
                        return;
                }

                var protocolFactory = new ProtocolFactory();
                var newProtocol = protocolFactory.CreateProtocol(ConnectionInfo);

                string connectionPanel = SetConnectionPanel(ConnectionInfo, Force);
                Form connectionForm = SetConnectionForm(ConForm, connectionPanel);
                Control connectionContainer = SetConnectionContainer(ConnectionInfo, connectionForm);
                SetConnectionFormEventHandlers(newProtocol, connectionForm);
                SetConnectionEventHandlers(newProtocol);
                BuildConnectionInterfaceController(ConnectionInfo, newProtocol, connectionContainer);

                newProtocol.Force = Force;

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

                ConnectionInfo.OpenConnections.Add(newProtocol);
                SetTreeNodeImages(ConnectionInfo);
                frmMain.Default.SelectedConnection = ConnectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionOpenFailed + Environment.NewLine + ex.Message);
            }
        }

        private static void StartPreConnectionExternalApp(ConnectionInfo ConnectionInfo)
        {
            if (ConnectionInfo.PreExtApp != "")
            {
                var extA = GetExtAppByName(ConnectionInfo.PreExtApp);
                extA?.Start(ConnectionInfo);
            }
        }

        public static ExternalTool GetExtAppByName(string Name)
        {
            foreach (ExternalTool extA in Runtime.ExternalTools)
            {
                if (extA.DisplayName == Name)
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

        private static string SetConnectionPanel(ConnectionInfo ConnectionInfo, ConnectionInfo.Force Force)
        {
            string connectionPanel = "";
            if (ConnectionInfo.Panel == "" || (Force & ConnectionInfo.Force.OverridePanel) == ConnectionInfo.Force.OverridePanel | Settings.Default.AlwaysShowPanelSelectionDlg)
            {
                frmChoosePanel frmPnl = new frmChoosePanel();
                if (frmPnl.ShowDialog() == DialogResult.OK)
                {
                    connectionPanel = frmPnl.Panel;
                }
            }
            else
            {
                connectionPanel = ConnectionInfo.Panel;
            }
            return connectionPanel;
        }

        private static Form SetConnectionForm(Form ConForm, string connectionPanel)
        {
            var connectionForm = ConForm ?? Runtime.WindowList.FromString(connectionPanel);

            if (connectionForm == null)
                connectionForm = Runtime.AddPanel(connectionPanel);
            else
                ((ConnectionWindow)connectionForm).Show(frmMain.Default.pnlDock);

            connectionForm.Focus();
            return connectionForm;
        }

        private static Control SetConnectionContainer(ConnectionInfo ConnectionInfo, Form connectionForm)
        {
            Control connectionContainer = ((ConnectionWindow)connectionForm).AddConnectionTab(ConnectionInfo);

            if (ConnectionInfo.Protocol == ProtocolType.IntApp)
            {
                if (GetExtAppByName(ConnectionInfo.ExtApp).Icon != null)
                    ((TabPage)connectionContainer).Icon = GetExtAppByName(ConnectionInfo.ExtApp).Icon;
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

        private static void SetTreeNodeImages(ConnectionInfo ConnectionInfo)
        {
            if (ConnectionInfo.IsQuickConnect == false)
            {
                if (ConnectionInfo.Protocol != ProtocolType.IntApp)
                {
                    ConnectionTreeNode.SetNodeImage(ConnectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                }
                else
                {
                    ExternalTool extApp = GetExtAppByName(ConnectionInfo.ExtApp);
                    if (extApp != null)
                    {
                        if (extApp.TryIntegrate && ConnectionInfo.TreeNode != null)
                        {
                            ConnectionTreeNode.SetNodeImage(ConnectionInfo.TreeNode, TreeImageType.ConnectionOpen);
                        }
                    }
                }
            }
        }

        private static void BuildConnectionInterfaceController(ConnectionInfo ConnectionInfo, ProtocolBase newProtocol, Control connectionContainer)
        {
            newProtocol.InterfaceControl = new InterfaceControl(connectionContainer, newProtocol, ConnectionInfo);
        }




        private static void Prot_Event_Disconnected(object sender, string DisconnectedMessage)
        {
            try
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format(Language.strProtocolEventDisconnected, DisconnectedMessage), true);

                ProtocolBase Prot = (ProtocolBase)sender;
                if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    string ReasonCode = DisconnectedMessage.Split("\r\n".ToCharArray())[0];
                    string desc = DisconnectedMessage.Replace("\r\n", " ");

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

        private static void Prot_Event_ErrorOccured(object sender, string ErrorMessage)
        {
            try
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strConnectionEventErrorOccured, true);
                ProtocolBase Prot = (ProtocolBase)sender;

                if (Prot.InterfaceControl.Info.Protocol == ProtocolType.RDP)
                {
                    if (Convert.ToInt32(ErrorMessage) > -1)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.strConnectionRdpErrorDetail, ErrorMessage, ProtocolRDP.FatalErrors.GetError(ErrorMessage)));
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