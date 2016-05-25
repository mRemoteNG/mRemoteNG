using System.Collections.Generic;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public partial class PortScanWindow : BaseWindow
	{
        #region Constructors
		public PortScanWindow(DockContent panel, bool import)
		{
			InitializeComponent();
					
			WindowType = WindowType.PortScan;
			DockPnl = panel;
			_import = import;
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
		private bool _import;
		private Tools.PortScan.Scanner _portScanner;
		private bool _scanning = false;
        #endregion
				
        #region Private Methods
        #region Event Handlers
		public void PortScan_Load(System.Object sender, EventArgs e)
		{
			ApplyLanguage();
					
			try
			{
				if (_import)
				{
					lvHosts.Columns.AddRange(new ColumnHeader[] {clmHost, clmSSH, clmTelnet, clmHTTP, clmHTTPS, clmRlogin, clmRDP, clmVNC});
					ShowImportControls(true);
					cbProtocol.SelectedIndex = 0;
				}
				else
				{
					lvHosts.Columns.AddRange(new ColumnHeader[] {clmHost, clmOpenPorts, clmClosedPorts});
					ShowImportControls(false);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(Language.strPortScanCouldNotLoadPanel, ex);
			}
		}
				
		public void portStart_Enter(System.Object sender, EventArgs e)
		{
			portStart.Select(0, portStart.Text.Length);
		}
				
		public void portEnd_Enter(System.Object sender, EventArgs e)
		{
			portEnd.Select(0, portEnd.Text.Length);
		}
				
		public void btnScan_Click(System.Object sender, EventArgs e)
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
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.strCannotStartPortScan);
				}
			}
		}
				
		public void btnImport_Click(System.Object sender, EventArgs e)
		{
            mRemoteNG.Connection.Protocol.ProtocolType protocol = (mRemoteNG.Connection.Protocol.ProtocolType)Tools.MiscTools.StringToEnum(typeof(mRemoteNG.Connection.Protocol.ProtocolType), Convert.ToString(cbProtocol.SelectedItem));
					
			List<Tools.PortScan.ScanHost> hosts = new List<Tools.PortScan.ScanHost>();
			foreach (ListViewItem item in lvHosts.SelectedItems)
			{
                Tools.PortScan.ScanHost scanHost = (Tools.PortScan.ScanHost)item.Tag;
				if (scanHost != null)
				{
					hosts.Add(scanHost);
				}
			}
			App.Import.ImportFromPortScan(hosts, protocol);
			DialogResult = DialogResult.OK;
			Close();
		}
        #endregion
				
		private void ApplyLanguage()
		{
			lblStartIP.Text = string.Format("{0}:", Language.strStartIP);
			lblEndIP.Text = string.Format("{0}:", Language.strEndIP);
			btnScan.Text = Language.strButtonScan;
			btnImport.Text = Language.strButtonImport;
			lblOnlyImport.Text = string.Format("{0}:", Language.strProtocolToImport);
			clmHost.Text = Language.strColumnHostnameIP;
			clmOpenPorts.Text = Language.strOpenPorts;
			clmClosedPorts.Text = Language.strClosedPorts;
			Label2.Text = string.Format("{0}:", Language.strEndPort);
			Label1.Text = string.Format("{0}:", Language.strStartPort);
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
						
				if (_import)
				{
					_portScanner = new Tools.PortScan.Scanner(ipAddressStart, ipAddressEnd);
				}
				else
				{
					_portScanner = new Tools.PortScan.Scanner(ipAddressStart, ipAddressEnd, (int) portStart.Value, (int) portEnd.Value);
				}
						
				_portScanner.BeginHostScan += PortScanner_BeginHostScan;
				_portScanner.HostScanned += PortScanner_HostScanned;
				_portScanner.ScanComplete += PortScanner_ScanComplete;
						
				_portScanner.StartScan();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "StartScan failed (UI.Window.PortScan)" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void StopScan()
		{
			if (_portScanner != null)
			{
				_portScanner.StopScan();
			}
			_scanning = false;
			SwitchButtonText();
		}
				
		private void SwitchButtonText()
		{
			if (_scanning)
			{
				btnScan.Text = Language.strButtonStop;
			}
			else
			{
				btnScan.Text = Language.strButtonScan;
			}
					
			prgBar.Maximum = 100;
			prgBar.Value = 0;
		}
				
		private static void PortScanner_BeginHostScan(string host)
		{
			Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Scanning " + host, true);
		}
				
		private delegate void PortScannerHostScannedDelegate(Tools.PortScan.ScanHost host, int scannedCount, int totalCount);
		private void PortScanner_HostScanned(Tools.PortScan.ScanHost host, int scannedCount, int totalCount)
		{
			if (InvokeRequired)
			{
				Invoke(new PortScannerHostScannedDelegate(PortScanner_HostScanned), new object[] {host, scannedCount, totalCount});
				return ;
			}
					
			Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, "Host scanned " + host.HostIp, true);
					
			ListViewItem listViewItem = host.ToListViewItem(_import);
			if (listViewItem != null)
			{
				lvHosts.Items.Add(listViewItem);
				listViewItem.EnsureVisible();
			}
					
			prgBar.Maximum = totalCount;
			prgBar.Value = scannedCount;
		}
				
		private delegate void PortScannerScanComplete(List<Tools.PortScan.ScanHost> hosts);
		private void PortScanner_ScanComplete(List<Tools.PortScan.ScanHost> hosts)
		{
			if (InvokeRequired)
			{
				Invoke(new PortScannerScanComplete(PortScanner_ScanComplete), new object[] {hosts});
				return ;
			}
					
			Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.strPortScanComplete);
					
			_scanning = false;
			SwitchButtonText();
		}
        #endregion
	}
}