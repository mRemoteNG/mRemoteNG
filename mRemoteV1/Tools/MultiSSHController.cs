using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Tools
{
	public class MultiSSHController
    {
        private ArrayList processHandlers = new ArrayList();
        private ArrayList quickConnectConnections = new ArrayList();
        private ArrayList previousCommands = new ArrayList();
        private int previousCommandIndex = 0;

        public int CommandHistoryLength { get; set; } = 100;

        public MultiSSHController(TextBox txtBox)
        {
            DecorateTextBox(txtBox);
        }

        public MultiSSHController(ToolStripTextBox txtBox)
        {
            DecorateTextBox(txtBox.TextBox);
        }

        public void ProcessNewQuickConnect(ConnectionInfo connection)
        {
            quickConnectConnections.Add(connection);
        }

        private void DecorateTextBox(TextBox toBeDecorated)
        {
            toBeDecorated.Enter += refreshActiveConnections;
            toBeDecorated.KeyDown += processKeyPress;
            toBeDecorated.KeyUp += processKeyRelease;
        }

        private ArrayList ProcessOpenConnections(ConnectionInfo connection)
        {
            ArrayList handlers = new ArrayList();

            foreach (ProtocolBase _base in connection.OpenConnections)
            {
                if (_base.GetType().IsSubclassOf(typeof(PuttyBase)))
                {
                    handlers.Add((PuttyBase)_base);
                }
            }

            return handlers;
        }

        private void SendAllKeystrokes(int keyType, int keyData)
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

#region Event Processors
        private void refreshActiveConnections(object sender, EventArgs e)
        {
            processHandlers.Clear();
            foreach (ConnectionInfo connection in quickConnectConnections)
            {
                processHandlers.AddRange(ProcessOpenConnections(connection));
            }

            var connectionTreeConnections = Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveChildList().Where(item => item.OpenConnections.Count > 0);

            foreach (ConnectionInfo connection in connectionTreeConnections)
            {
                processHandlers.AddRange(ProcessOpenConnections(connection));
            }
        }

        private void processKeyPress(object sender, KeyEventArgs e)
        {
            TextBox txtMultiSSH = sender as TextBox;
            if (txtMultiSSH == null) return;            

            if (processHandlers.Count == 0)
            {
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                if (e.KeyCode == Keys.Up && previousCommandIndex - 1 >= 0)
                {
                    previousCommandIndex -= 1;
                }

                if (e.KeyCode == Keys.Down && previousCommandIndex + 1 < previousCommands.Count)
                {
                    previousCommandIndex += 1;
                }

                txtMultiSSH.Text = previousCommands[previousCommandIndex].ToString();
                txtMultiSSH.Select(txtMultiSSH.TextLength, 0);
            }

            if (e.Control == true && e.KeyCode != Keys.V && e.Alt == false)
            {
                SendAllKeystrokes(NativeMethods.WM_KEYDOWN, e.KeyValue);
            }

            if (e.KeyCode == Keys.Enter)
            {
                string strLine = txtMultiSSH.Text;
                foreach (char chr1 in strLine)
                {
                    SendAllKeystrokes(NativeMethods.WM_CHAR, Convert.ToByte(chr1));
                }
                SendAllKeystrokes(NativeMethods.WM_KEYDOWN, 13); // Enter = char13
            }
        }

        private void processKeyRelease(object sender, KeyEventArgs e)
        {
            TextBox txtMultiSSH = sender as TextBox;
            if (txtMultiSSH == null) return;

            if (e.KeyCode == Keys.Enter)
            {
                    if (txtMultiSSH.Text.Trim() != "")
                    {
                        previousCommands.Add(txtMultiSSH.Text.Trim());
                    }
                    if (previousCommands.Count >= CommandHistoryLength)
                    {
                        previousCommands.RemoveAt(0);
                    }

                    previousCommandIndex = previousCommands.Count - 1;
                    txtMultiSSH.Clear();
                }
            }
#endregion
    }
}
