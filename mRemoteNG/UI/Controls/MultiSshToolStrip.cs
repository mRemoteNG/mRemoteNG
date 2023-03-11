using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System;
using System.Collections;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class MultiSshToolStrip : ToolStrip
    {
        private IContainer components;
        private ToolStripLabel lblMultiSsh;
        private ToolStripTextBox txtMultiSsh;
        private int previousCommandIndex = 0;
        private readonly ArrayList processHandlers = new ArrayList();
        private readonly ArrayList quickConnectConnections = new ArrayList();
        private readonly ArrayList previousCommands = new ArrayList();
        private readonly ThemeManager _themeManager;

        private int CommandHistoryLength { get; set; } = 100;

        public MultiSshToolStrip()
        {
            InitializeComponent();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ActiveAndExtended) return;
            txtMultiSsh.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            txtMultiSsh.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
        }

        private ArrayList ProcessOpenConnections(ConnectionInfo connection)
        {
            var handlers = new ArrayList();

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
            if (processHandlers.Count == 0) return;

            foreach (PuttyBase proc in processHandlers)
            {
                NativeMethods.PostMessage(proc.PuttyHandle, keyType, new IntPtr(keyData), new IntPtr(0));
            }
        }

        #region Key Event Handler

        private void RefreshActiveConnections(object sender, EventArgs e)
        {
            processHandlers.Clear();
            foreach (ConnectionInfo connection in quickConnectConnections)
            {
                processHandlers.AddRange(ProcessOpenConnections(connection));
            }

            var connectionTreeConnections = Runtime.ConnectionsService.ConnectionTreeModel.GetRecursiveChildList().Where(item => item.OpenConnections.Count > 0);

            foreach (var connection in connectionTreeConnections)
            {
                processHandlers.AddRange(ProcessOpenConnections(connection));
            }
        }

        private void ProcessKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                try
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up when previousCommandIndex - 1 >= 0:
                            previousCommandIndex -= 1;
                            break;
                        case Keys.Down when previousCommandIndex + 1 < previousCommands.Count:
                            previousCommandIndex += 1;
                            break;
                        default:
                            return;
                    }
                }
                catch { }

                txtMultiSsh.Text = previousCommands[previousCommandIndex].ToString();
                txtMultiSsh.SelectAll();
            }

            if (e.Control && e.KeyCode != Keys.V && e.Alt == false)
            {
                SendAllKeystrokes(NativeMethods.WM_KEYDOWN, e.KeyValue);
            }

            if (e.KeyCode == Keys.Enter)
            {
                foreach (var chr1 in txtMultiSsh.Text)
                {
                    SendAllKeystrokes(NativeMethods.WM_CHAR, Convert.ToByte(chr1));
                }

                SendAllKeystrokes(NativeMethods.WM_KEYDOWN, 13); // Enter = char13
            }
        }

        private void ProcessKeyRelease(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (string.IsNullOrWhiteSpace(txtMultiSsh.Text)) return;

            previousCommands.Add(txtMultiSsh.Text.Trim());

            if (previousCommands.Count >= CommandHistoryLength) previousCommands.RemoveAt(0);

            previousCommandIndex = previousCommands.Count - 1;
            txtMultiSsh.Clear();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMultiSsh = new ToolStripLabel();
            this.txtMultiSsh = new ToolStripTextBox();
            this.SuspendLayout();
            // 
            // lblMultiSSH
            // 
            this.lblMultiSsh.Name = "_lblMultiSsh";
            this.lblMultiSsh.Size = new System.Drawing.Size(77, 22);
            this.lblMultiSsh.Text = Language.MultiSsh;
            // 
            // txtMultiSsh
            // 
            this.txtMultiSsh.Name = "_txtMultiSsh";
            this.txtMultiSsh.Size = new System.Drawing.Size(new DisplayProperties().ScaleWidth(300), 25);
            this.txtMultiSsh.ToolTipText = Language.MultiSshToolTip;
            this.txtMultiSsh.Enter += RefreshActiveConnections;
            this.txtMultiSsh.KeyDown += ProcessKeyPress;
            this.txtMultiSsh.KeyUp += ProcessKeyRelease;

            this.Items.AddRange(new ToolStripItem[]
            {
                lblMultiSsh,
                txtMultiSsh
            });
            this.ResumeLayout(false);
        }

        #endregion

    }
}