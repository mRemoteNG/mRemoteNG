using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.App;
using System.Collections;
using mRemoteNG.Connection;

namespace mRemoteNG.UI.Window
{
    public partial class SSHCommandWIndow : BaseWindow
    {

        public SSHCommandWIndow(DockContent panel)
        {
            InitializeComponent();

            WindowType = WindowType.SSHCommandWindow;
            DockPnl = panel;

            HideOnClose = true;
            Icon = Resources.Screenshot_Icon;
            Name = "SSHCommander";
            TabText = "Multi-SSH";
            Text = "Multi-SSH Commander";
        }

        #region Private Fields
        private ArrayList processHandlers = new ArrayList();
        #endregion

        #region Public Methods
        #region Event Handlers
        private void SSHCommandWindow_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        #endregion

        private void txtSSHCommand_Enter(object sender, EventArgs e)
        {
            try
            {
                var connectionInfoList = Runtime.ConnectionTreeModel.GetRecursiveChildList();
                //.Where( node-> !(node is Container.ContainerInfo));
                var previouslyOpenedConnections = connectionInfoList.Where(item => item.OpenConnections.Count > 0);

                //var connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList().Where(node => !(node is ContainerInfo));
                //var previouslyOpenedConnections = connectionInfoList.Where(item => item.PleaseConnect);
                //foreach (var connectionInfo in previouslyOpenedConnections)
                //{
                //    _connectionInitiator.OpenConnection(connectionInfo);
                //}



                processHandlers.Clear();
                foreach (ConnectionInfo connection in previouslyOpenedConnections)
                {
                    foreach (ProtocolBase _base in connection.OpenConnections)
                    {
                        if (_base.GetType().IsSubclassOf(typeof(PuttyBase)))
                        {
                            processHandlers.Add((PuttyBase)_base);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtSSHCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (processHandlers.Count == 0)
            {
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                //string lastCommand = "";
                //if (lstCommands.SelectedIndex == lstCommands.Items.Count)
                //{
                //    lastCommand = lstCommands.Items[lstCommands.Items.Count].ToString();
                //}

                if (e.KeyCode == Keys.Up && lstCommands.SelectedIndex -1 > -1 && lstCommands.SelectedItem.ToString() == txtSSHCommand.Text)
                {
                    lstCommands.SelectedIndex -= 1;
                }

                if (e.KeyCode == Keys.Down && lstCommands.SelectedIndex + 1 < lstCommands.Items.Count)
                {
                    lstCommands.SelectedIndex += 1;
                }

                txtSSHCommand.Text = lstCommands.SelectedItem.ToString();
                txtSSHCommand.Select(txtSSHCommand.TextLength, 0);
            }

            if (e.Control == true && e.KeyCode != Keys.V && e.Alt == false)
            {
                sendAllKey(NativeMethods.WM_KEYDOWN, e.KeyValue);
            }

            if (e.KeyCode == Keys.Enter)
            {
                string strLine = txtSSHCommand.Text;
                foreach (char chr1 in strLine)
                {
                    sendAllKey(NativeMethods.WM_CHAR, Convert.ToByte(chr1));
                }
                sendAllKey(NativeMethods.WM_KEYDOWN, 13); // Enter = char13
            }
        }

        private void gotoEndOfText()
        {
            if (txtSSHCommand.Text.Trim() != "")
            {
                lstCommands.Items.Add(txtSSHCommand.Text.Trim());
            }
            lstCommands.SelectedIndex = lstCommands.Items.Count - 1;
            txtSSHCommand.Clear();
        }

        private void sendAllKey(int keyType, int keyData)
        {
            if (processHandlers.Count == 0)
            {
                return;
            }
            foreach (PuttyBase proc in processHandlers)
            {
                NativeMethods.PostMessage(proc.PuttyHandle, keyType, new IntPtr(keyData), new IntPtr(0));
            }
        }

        private void txtSSHCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gotoEndOfText();
            }
        }
    }
    #endregion
}
