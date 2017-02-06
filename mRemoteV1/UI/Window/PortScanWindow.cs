using System.Collections.Generic;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using static mRemoteNG.Tools.MiscTools;


namespace mRemoteNG.UI.Window
{
	public partial class PortScanWindow
	{
        #region Constructors
		public PortScanWindow()
		{
			InitializeComponent();
					
			WindowType = WindowType.PortScan;
			DockPnl = new DockContent();
		}
        #endregion
				
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
	            lvHosts.Columns.AddRange(new[]{clmHost, clmSSH, clmTelnet, clmHTTP, clmHTTPS, clmRlogin, clmRDP, clmVNC, clmOpenPorts, clmClosedPorts});
	            ShowImportControls(true);
	            cbProtocol.SelectedIndex = 0;
	        }
	        catch (Exception ex)
	        {
	            Runtime.MessageCollector.AddExceptionMessage(Language.strPortScanCouldNotLoadPanel, ex);
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
					Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strCannotStartPortScan);
				}
			}
		}

	    private void btnImport_Click(object sender, EventArgs e)
		{
            ProtocolType protocol = (ProtocolType)StringToEnum(typeof(ProtocolType), Convert.ToString(cbProtocol.SelectedItem));
		    importSelectedHosts(protocol);
            DialogResult = DialogResult.OK;
			Close();
		}
        #endregion
				
		private void ApplyLanguage()
		{
			lblStartIP.Text = $"{Language.strStartIP}:";
			lblEndIP.Text = $"{Language.strEndIP}:";
			btnScan.Text = Language.strButtonScan;
			btnImport.Text = Language.strButtonImport;
			lblOnlyImport.Text = $"{Language.strProtocolToImport}:";
			clmHost.Text = Language.strColumnHostnameIP;
			clmOpenPorts.Text = Language.strOpenPorts;
			clmClosedPorts.Text = Language.strClosedPorts;
			Label2.Text = $"{Language.strEndPort}:";
			Label1.Text = $"{Language.strStartPort}:";
			TabText = Language.strMenuPortScan;
			Text = Language.strMenuPortScan;
		}
				
		private void ShowImportControls(bool controlsVisible)
		{
			pnlPorts.Visible = controlsVisible;
			pnlImport.Visible = controlsVisible;
			if (controlsVisible)
			{
				lvHosts.Height = pnlImport.Top - lvHosts.Top;
			}
			else
			{
				lvHosts.Height = pnlImport.Bottom - lvHosts.Top;
			}
		}
				
		private void StartScan()
		{
			try
			{
				_scanning = true;
				SwitchButtonText();
				lvHosts.Items.Clear();
						
				System.Net.IPAddress ipAddressStart = System.Net.IPAddress.Parse(ipStart.Text);
				System.Net.IPAddress ipAddressEnd = System.Net.IPAddress.Parse(ipEnd.Text);
				
				_portScanner = new PortScanner(ipAddressStart, ipAddressEnd, (int) portStart.Value, (int) portEnd.Value);
						
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
		    _portScanner?.StopScan();
		    _scanning = false;
			SwitchButtonText();
		}
				
		private void SwitchButtonText()
		{
			btnScan.Text = _scanning ? Language.strButtonStop : Language.strButtonScan;
					
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
				Invoke(new PortScannerHostScannedDelegate(PortScanner_HostScanned), new object[] {host, scannedCount, totalCount});
				return;
			}
					
			Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Host scanned " + host.HostIp, true);
					
			ListViewItem listViewItem = host.ToListViewItem();
			if (listViewItem != null)
			{
				lvHosts.Items.Add(listViewItem);
				listViewItem.EnsureVisible();
			}
					
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
					
			Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strPortScanComplete);
					
			_scanning = false;
			SwitchButtonText();
		}
        #endregion

        private void importSelectedHosts(ProtocolType protocol)
        {
            var hosts = new List<ScanHost>();
            foreach (ListViewItem item in lvHosts.SelectedItems)
            {
                var scanHost = (ScanHost)item.Tag;
                if (scanHost != null)
                {
                    hosts.Add(scanHost);
                }
            }

            if (hosts.Count < 1)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, "Could not import host(s) from port scan context menu");
                return;
            }

            var selectedTreeNodeAsContainer = Windows.TreeForm.SelectedNode as ContainerInfo ?? Windows.TreeForm.SelectedNode.Parent;
            Import.ImportFromPortScan(hosts, protocol, selectedTreeNodeAsContainer);
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