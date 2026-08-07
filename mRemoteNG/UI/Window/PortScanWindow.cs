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
            DisplayProperties display = new();
            btnScan.Image = display.ScaleImage(btnScan.Image);
        }

        #endregion

        private new void ApplyTheme()
        {
            base.ApplyTheme();
        }

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
                UpdatePortModeControls();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage(Language.PortScanCouldNotLoadPanel, ex);
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (_scanning)
                StopScan();
            else
                StartScan();
        }

        /// <summary>
        /// The custom port list is only editable while the "Custom" option is selected, so the three
        /// port options can never be left in a half-configured state.
        /// </summary>
        private void PortMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePortModeControls();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ProtocolType protocol =
                (ProtocolType)Enum.Parse(typeof(ProtocolType), Convert.ToString(cbProtocol.SelectedItem), true);
            importSelectedHosts(protocol);
        }

        #endregion

        private void ApplyLanguage()
        {
            // One field takes a single address, an explicit range or a CIDR block (IPv4 or IPv6).
            lblStartIP.Text = Language.PortScanAddressRange;
            txtIpRange.ToolTipText = Language.PortScanAddressRangeHint;
            txtIpRange.PlaceholderText = "192.168.1.1  |  192.168.1.1 - 192.168.1.254  |  192.168.1.0/24";
            btnScan.Text = Language._Scan;
            btnImport.Text = Language._Import;
            lblOnlyImport.Text = Language.ProtocolToImport;
            clmHost.Text = Language.HostnameIp;
            clmOpenPorts.Text = Language.OpenPorts;
            clmClosedPorts.Text = Language.ClosedPorts;
            lblPorts.Text = Language.Ports;
            rdoCommonPorts.Text = Language.PortScanCommonPorts;
            rdoAllPorts.Text = Language.PortScanAllPorts;
            rdoCustomPorts.Text = Language.PortScanCustomPorts;
            txtCustomPorts.PlaceholderText = "22, 80, 443, 3389, 8000-8100";
            portScanToolTip.SetToolTip(rdoCommonPorts, string.Join(", ", CommonPorts));
            portScanToolTip.SetToolTip(rdoCustomPorts, Language.PortScanCustomPortsHint);
            portScanToolTip.SetToolTip(txtCustomPorts, Language.PortScanCustomPortsHint);
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
            if (!IpRangeParser.TryParse(txtIpRange.Text, out IPAddress ipAddressStart, out IPAddress ipAddressEnd,
                                        out string ipError))
            {
                ReportInvalidInput(ipError);
                return;
            }

            if (!TryGetSelectedPorts(out List<int> ports, out string portError))
            {
                ReportInvalidInput(portError);
                return;
            }

            // Build the scanner FIRST. Constructing it enumerates the address range and can throw
            // (e.g. the range exceeds the scan limit), so a failure must not leave the Scan/Stop
            // button stuck on "Stop" with nothing running.
            PortScanner scanner;
            try
            {
                scanner = new PortScanner(ipAddressStart, ipAddressEnd, ports,
                                          (int)numericSelectorTimeout.Value * 1000);
            }
            catch (ArgumentException ex)
            {
                ReportInvalidInput(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("StartScan failed (UI.Window.PortScan)", ex);
                return;
            }

            _portScanner = scanner;
            _portScanner.BeginHostScan += PortScanner_BeginHostScan;
            _portScanner.HostScanned += PortScanner_HostScanned;
            _portScanner.ScanComplete += PortScanner_ScanComplete;

            _scanning = true;
            SwitchButtonText();
            olvHosts.Items.Clear();

            _portScanner.StartScan();
        }

        /// <summary>
        /// Commonly scanned service ports: FTP/SSH/Telnet/SMTP/DNS/HTTP(S), Windows RPC/NetBIOS/SMB,
        /// LDAP(S), IMAP/POP3 (incl. TLS), rlogin, the usual databases, RDP, VNC, WinRM and common app
        /// ports. Every port backing a protocol column in the results list is included, so the
        /// SSH/Telnet/HTTP/HTTPS/Rlogin/RDP/VNC columns are still populated in this mode.
        /// </summary>
        private static readonly int[] CommonPorts =
        [
            21, 22, 23, 25, 53, 80, 110, 111, 135, 139, 143, 389, 443, 445, 465, 513, 587, 636, 993, 995,
            1433, 1521, 2049, 3306, 3389, 5432, 5900, 5985, 5986, 6379, 8080, 8443, 9200, 27017
        ];

        private void UpdatePortModeControls()
        {
            txtCustomPorts.Enabled = rdoCustomPorts.Checked;
        }

        /// <summary>
        /// Resolves the ports to probe from the selected port option. Returns false, with a
        /// user-readable reason, when the custom list is empty or malformed.
        /// </summary>
        private bool TryGetSelectedPorts(out List<int> ports, out string error)
        {
            error = string.Empty;

            if (rdoAllPorts.Checked)
            {
                ports = PortListParser.AllPorts();
                return true;
            }

            if (!rdoCustomPorts.Checked)
            {
                ports = [.. CommonPorts];
                return true;
            }

            return PortListParser.TryParse(txtCustomPorts.Text, out ports, out error);
        }

        private void ReportInvalidInput(string message)
        {
            Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, message);
            MessageBox.Show(this, message, Language.PortScan, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            List<ScanHost> hosts = new();
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

            ContainerInfo destinationContainer = GetDestinationContainerForImportedHosts();
            Import.ImportFromPortScan(hosts, protocol, destinationContainer);
        }

        /// <summary>
        /// Determines where the imported hosts will be placed
        /// in the connection tree.
        /// </summary>
        private ContainerInfo GetDestinationContainerForImportedHosts()
        {
            ConnectionInfo selectedNode = AppWindows.TreeForm.SelectedNode ?? AppWindows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

            // if a putty node is selected, place imported connections in the root connection node
            if (selectedNode is RootPuttySessionsNodeInfo || selectedNode is PuttySessionInfo)
                selectedNode = AppWindows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>()
                                      .First();

            // if the selected node is a connection, use its parent container
            ContainerInfo selectedTreeNodeAsContainer = selectedNode as ContainerInfo ?? selectedNode.Parent;

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
    }
}