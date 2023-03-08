using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public partial class PortScanWindow
    {
        #region Constructors

        public PortScanWindow()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.SearchAndApps_16x);
            WindowType = WindowType.PortScan;
            DockPnl = new DockContent();
            ApplyTheme();
            var display = new DisplayProperties();
            btnScan.Image = display.ScaleImage(btnScan.Image);
        }

        #endregion

        private new void ApplyTheme()
        {
            base.ApplyTheme();
        }

        #region Private Properties

        private bool IpsValid
        {
            get
            {
                if (string.IsNullOrEmpty(ipStart.Octet1.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipStart.Octet2.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipStart.Octet3.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipStart.Octet4.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipEnd.Octet1.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipEnd.Octet2.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipEnd.Octet3.Text))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(ipEnd.Octet4.Text))
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Private Fields

        private PortScanner _portScanner;
        private bool _scanning;

        #endregion

        #region Private Methods

        #region Event Handlers

        private void PortScan_Load(object sender, EventArgs e)
        {
            ApplyLanguage();

            try
            {
                olvHosts.Columns.AddRange(new ColumnHeader[]
                {
                    clmHost, clmSSH, clmTelnet, clmHTTP, clmHTTPS, clmRlogin, clmRDP, clmVNC, clmOpenPorts,
                    clmClosedPorts
                });
                ShowImportControls(true);
                cbProtocol.SelectedIndex = 0;
                numericSelectorTimeout.Value = 5;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.PortScanCouldNotLoadPanel, ex);
            }
        }

        private void portStart_Enter(object sender, EventArgs e)
        {
            portStart.Select(0, portStart.Text.Length);
        }

        private void portEnd_Enter(object sender, EventArgs e)
        {
            portEnd.Select(0, portEnd.Text.Length);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (_scanning)
            {
                StopScan();
            }
            else
            {
                if (IpsValid)
                {
                    StartScan();
                }
                else
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.CannotStartPortScan);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var protocol =
                (ProtocolType)Enum.Parse(typeof(ProtocolType), Convert.ToString(cbProtocol.SelectedItem), true);
            importSelectedHosts(protocol);
        }

        #endregion

        private void ApplyLanguage()
        {
            lblStartIP.Text = Language.FirstIp;
            lblEndIP.Text = Language.LastIp;
            btnScan.Text = Language._Scan;
            btnImport.Text = Language._Import;
            lblOnlyImport.Text = Language.ProtocolToImport;
            clmHost.Text = Language.HostnameIp;
            clmOpenPorts.Text = Language.OpenPorts;
            clmClosedPorts.Text = Language.ClosedPorts;
            ngCheckFirstPort.Text = Language.FirstPort;
            ngCheckLastPort.Text = Language.LastPort;
            lblTimeout.Text = Language.TimeoutInSeconds;
            TabText = Language.PortScan;
            Text = Language.PortScan;
        }

        private void ShowImportControls(bool controlsVisible)
        {
            pnlImport.Visible = controlsVisible;
            if (controlsVisible)
                olvHosts.Height = pnlImport.Top - olvHosts.Top;
            else
                olvHosts.Height = pnlImport.Bottom - olvHosts.Top;
        }

        private void StartScan()
        {
            try
            {
                _scanning = true;
                SwitchButtonText();
                olvHosts.Items.Clear();

                var ipAddressStart = IPAddress.Parse(ipStart.Text);
                var ipAddressEnd = IPAddress.Parse(ipEnd.Text);

                if (!ngCheckFirstPort.Checked && !ngCheckLastPort.Checked)
                    _portScanner = new PortScanner(ipAddressStart, ipAddressEnd, (int)portStart.Value,
                                                   (int)portEnd.Value, (int)numericSelectorTimeout.Value * 1000, true);
                else
                    _portScanner = new PortScanner(ipAddressStart, ipAddressEnd, (int)portStart.Value,
                                                   (int)portEnd.Value, (int)numericSelectorTimeout.Value * 1000);

                _portScanner.BeginHostScan += PortScanner_BeginHostScan;
                _portScanner.HostScanned += PortScanner_HostScanned;
                _portScanner.ScanComplete += PortScanner_ScanComplete;

                _portScanner.StartScan();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("StartScan failed (UI.Window.PortScan)", ex);
            }
        }

        private void StopScan()
        {
            _portScanner.BeginHostScan -= PortScanner_BeginHostScan;
            _portScanner.HostScanned -= PortScanner_HostScanned;
            _portScanner.ScanComplete -= PortScanner_ScanComplete;

            _portScanner?.StopScan();
            _scanning = false;
            SwitchButtonText();
        }

        private void SwitchButtonText()
        {
            btnScan.Text = _scanning ? Language._Stop : Language._Scan;

            prgBar.Maximum = 100;
            prgBar.Value = 0;
        }

        private static void PortScanner_BeginHostScan(string host)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Scanning " + host, true);
        }

        private delegate void PortScannerHostScannedDelegate(ScanHost host, int scannedCount, int totalCount);

        private void PortScanner_HostScanned(ScanHost host, int scannedCount, int totalCount)
        {
            if (InvokeRequired)
            {
                Invoke(new PortScannerHostScannedDelegate(PortScanner_HostScanned),
                       new object[] {host, scannedCount, totalCount});
                return;
            }

            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Host scanned " + host.HostIp, true);

            olvHosts.AddObject(host);
            prgBar.Maximum = totalCount;
            prgBar.Value = scannedCount;
        }

        private delegate void PortScannerScanComplete(List<ScanHost> hosts);

        private void PortScanner_ScanComplete(List<ScanHost> hosts)
        {
            if (InvokeRequired)
            {
                Invoke(new PortScannerScanComplete(PortScanner_ScanComplete), new object[] {hosts});
                return;
            }

            Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.PortScanComplete);

            _scanning = false;
            SwitchButtonText();
        }

        #endregion

        private void importSelectedHosts(ProtocolType protocol)
        {
            var hosts = new List<ScanHost>();
            foreach (ScanHost host in olvHosts.SelectedObjects)
            {
                hosts.Add(host);
            }

            if (hosts.Count < 1)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                                                    "Could not import host(s) from port scan context menu");
                return;
            }

            var destinationContainer = GetDestinationContainerForImportedHosts();
            Import.ImportFromPortScan(hosts, protocol, destinationContainer);
        }

        /// <summary>
        /// Determines where the imported hosts will be placed
        /// in the connection tree.
        /// </summary>
        private ContainerInfo GetDestinationContainerForImportedHosts()
        {
            var selectedNode = Windows.TreeForm.SelectedNode
                            ?? Windows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>()
                                      .First();

            // if a putty node is selected, place imported connections in the root connection node
            if (selectedNode is RootPuttySessionsNodeInfo || selectedNode is PuttySessionInfo)
                selectedNode = Windows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>()
                                      .First();

            // if the selected node is a connection, use its parent container
            var selectedTreeNodeAsContainer = selectedNode as ContainerInfo ?? selectedNode.Parent;

            return selectedTreeNodeAsContainer;
        }

        private void importVNCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.VNC);
        }

        private void importTelnetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.Telnet);
        }

        private void importSSH2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.SSH2);
        }

        private void importRloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.Rlogin);
        }

        private void importRDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.RDP);
        }

        private void importHTTPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.HTTPS);
        }

        private void importHTTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importSelectedHosts(ProtocolType.HTTP);
        }

        private void NgCheckFirstPort_CheckedChanged(object sender, EventArgs e)
        {
            portStart.Enabled = ngCheckFirstPort.Checked;
        }

        private void NgCheckLastPort_CheckedChanged(object sender, EventArgs e)
        {
            portEnd.Enabled = ngCheckLastPort.Checked;

            portEnd.Value = portEnd.Enabled ? 65535 : 0;
        }
    }
}