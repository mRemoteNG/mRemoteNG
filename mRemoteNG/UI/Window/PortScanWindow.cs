using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (btnScan.Image is not null)
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

        private PortScanner? _portScanner;
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
                    clmHostIP, clmHostName, clmSSH, clmTelnet, clmHTTP, clmHTTPS, clmRlogin, clmRDP, clmVNC, clmOpenPorts,
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
            string selectedItem = Convert.ToString(cbProtocol.SelectedItem, CultureInfo.InvariantCulture) ?? string.Empty;
            if (string.Equals(selectedItem, "All detected", StringComparison.Ordinal))
            {
                importAllDetectedProtocols();
                return;
            }

            ProtocolType protocol = Enum.Parse<ProtocolType>(selectedItem, true);
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
            clmHostIP.Text = "IP Address";
            clmHostName.Text = "Hostname";
            clmOpenPorts.Text = Language.OpenPorts;
            clmClosedPorts.Text = Language.ClosedPorts;
            ngCheckFirstPort.Text = Language.FirstPort;
            ngCheckLastPort.Text = Language.LastPort;
            lblCustomPorts.Text = "Custom ports (e.g. 22,80,443):";
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

                IPAddress ipAddressStart = IPAddress.Parse(ipStart.Text);
                IPAddress ipAddressEnd = IPAddress.Parse(ipEnd.Text);

                string customPortsText = txtCustomPorts.Text.Trim();
                if (!string.IsNullOrEmpty(customPortsText))
                {
                    List<int> customPorts = ParsePortList(customPortsText);
                    if (customPorts.Count == 0)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.CannotStartPortScan);
                        _scanning = false;
                        SwitchButtonText();
                        return;
                    }
                    _portScanner = new PortScanner(ipAddressStart, ipAddressEnd, customPorts,
                                                   (int)numericSelectorTimeout.Value * 1000);
                }
                else if (!ngCheckFirstPort.Checked && !ngCheckLastPort.Checked)
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
            if (_portScanner is not null)
            {
                _portScanner.BeginHostScan -= PortScanner_BeginHostScan;
                _portScanner.HostScanned -= PortScanner_HostScanned;
                _portScanner.ScanComplete -= PortScanner_ScanComplete;
                _portScanner.StopScan();
            }
            _scanning = false;
            SwitchButtonText();
        }

        private static List<int> ParsePortList(string portListText)
        {
            List<int> ports = new();
            foreach (string part in portListText.Split(',', ';', ' '))
            {
                string trimmed = part.Trim();
                if (int.TryParse(trimmed, out int port) && port >= 1 && port <= 65535)
                    ports.Add(port);
            }
            return ports;
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

        private delegate void PortScannerScanComplete(IList<ScanHost> hosts);

        private void PortScanner_ScanComplete(IList<ScanHost> hosts)
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

            ContainerInfo? destinationContainer = GetDestinationContainerForImportedHosts();
            if (destinationContainer is null)
                return;
            Import.ImportFromPortScan(hosts, protocol, destinationContainer);
        }

        private void importAllDetectedProtocols()
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

            ContainerInfo? destinationContainer = GetDestinationContainerForImportedHosts();
            if (destinationContainer is null)
                return;

            (ProtocolType protocol, Func<ScanHost, bool> detected)[] protocolFilters =
            [
                (ProtocolType.SSH2, h => h.Ssh),
                (ProtocolType.Telnet, h => h.Telnet),
                (ProtocolType.HTTP, h => h.Http),
                (ProtocolType.HTTPS, h => h.Https),
                (ProtocolType.Rlogin, h => h.Rlogin),
                (ProtocolType.RDP, h => h.Rdp),
                (ProtocolType.VNC, h => h.Vnc),
            ];

            foreach (var (protocol, detected) in protocolFilters)
            {
                List<ScanHost> filtered = hosts.Where(detected).ToList();
                if (filtered.Count > 0)
                    Import.ImportFromPortScan(filtered, protocol, destinationContainer);
            }
        }

        /// <summary>
        /// Determines where the imported hosts will be placed
        /// in the connection tree.
        /// </summary>
        private static ContainerInfo? GetDestinationContainerForImportedHosts()
        {
            ConnectionInfo? selectedNode = AppWindows.TreeForm?.SelectedNode ?? AppWindows.TreeForm?.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

            if (selectedNode is null)
                return null;

            // if a putty node is selected, place imported connections in the root connection node
            if (selectedNode is RootPuttySessionsNodeInfo || selectedNode is PuttySessionInfo)
                selectedNode = AppWindows.TreeForm!.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>()
                                      .First();

            // if the selected node is a connection, use its parent container
            ContainerInfo? selectedTreeNodeAsContainer = selectedNode as ContainerInfo ?? selectedNode.Parent;

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