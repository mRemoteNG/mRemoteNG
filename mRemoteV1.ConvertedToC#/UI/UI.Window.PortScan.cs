using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.Tools.PortScan;
using mRemoteNG.My;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public partial class PortScan : Base
		{
			#region "Constructors"
			public PortScan(DockContent panel, bool import)
			{
				Load += PortScan_Load;
				InitializeComponent();

				WindowType = Type.PortScan;
				DockPnl = panel;
				_import = import;
			}
			#endregion

			#region "Private Properties"
			private bool IpsValid {
				get {
					if (string.IsNullOrEmpty(ipStart.Octet1))
						return false;
					if (string.IsNullOrEmpty(ipStart.Octet2))
						return false;
					if (string.IsNullOrEmpty(ipStart.Octet3))
						return false;
					if (string.IsNullOrEmpty(ipStart.Octet4))
						return false;

					if (string.IsNullOrEmpty(ipEnd.Octet1))
						return false;
					if (string.IsNullOrEmpty(ipEnd.Octet2))
						return false;
					if (string.IsNullOrEmpty(ipEnd.Octet3))
						return false;
					if (string.IsNullOrEmpty(ipEnd.Octet4))
						return false;

					return true;
				}
			}
			#endregion

			#region "Private Fields"
			private readonly bool _import;
			private Scanner _portScanner;
				#endregion
			private bool _scanning = false;

			#region "Private Methods"
			#region "Event Handlers"
			private void PortScan_Load(System.Object sender, EventArgs e)
			{
				ApplyLanguage();

				try {
					if (_import) {
						lvHosts.Columns.AddRange(new ColumnHeader[] {
							clmHost,
							clmSSH,
							clmTelnet,
							clmHTTP,
							clmHTTPS,
							clmRlogin,
							clmRDP,
							clmVNC
						});
						ShowImportControls(true);
						cbProtocol.SelectedIndex = 0;
					} else {
						lvHosts.Columns.AddRange(new ColumnHeader[] {
							clmHost,
							clmOpenPorts,
							clmClosedPorts
						});
						ShowImportControls(false);
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage(mRemoteNG.My.Language.strPortScanCouldNotLoadPanel, ex);
				}
			}

			private void portStart_Enter(System.Object sender, EventArgs e)
			{
				portStart.Select(0, portStart.Text.Length);
			}

			private void portEnd_Enter(System.Object sender, EventArgs e)
			{
				portEnd.Select(0, portEnd.Text.Length);
			}

			private void btnScan_Click(System.Object sender, EventArgs e)
			{
				if (_scanning) {
					StopScan();
				} else {
					if (IpsValid) {
						StartScan();
					} else {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, mRemoteNG.My.Language.strCannotStartPortScan);
					}
				}
			}

			private void btnImport_Click(System.Object sender, EventArgs e)
			{
				mRemoteNG.Connection.Protocol.Protocols protocol = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.Protocols), cbProtocol.SelectedItem);

				List<ScanHost> hosts = new List<ScanHost>();
				foreach (ListViewItem item in lvHosts.SelectedItems) {
					ScanHost scanHost = item.Tag as ScanHost;
					if (scanHost != null)
						hosts.Add(item.Tag);
				}

				mRemoteNG.App.Import.ImportFromPortScan(hosts, protocol);

				DialogResult = DialogResult.OK;
				Close();
			}
			#endregion

			private void ApplyLanguage()
			{
				lblStartIP.Text = string.Format("{0}:", mRemoteNG.My.Language.strStartIP);
				lblEndIP.Text = string.Format("{0}:", mRemoteNG.My.Language.strEndIP);
				btnScan.Text = mRemoteNG.My.Language.strButtonScan;
				btnImport.Text = mRemoteNG.My.Language.strButtonImport;
				lblOnlyImport.Text = string.Format("{0}:", mRemoteNG.My.Language.strProtocolToImport);
				clmHost.Text = mRemoteNG.My.Language.strColumnHostnameIP;
				clmOpenPorts.Text = mRemoteNG.My.Language.strOpenPorts;
				clmClosedPorts.Text = mRemoteNG.My.Language.strClosedPorts;
				Label2.Text = string.Format("{0}:", mRemoteNG.My.Language.strEndPort);
				Label1.Text = string.Format("{0}:", mRemoteNG.My.Language.strStartPort);
				TabText = mRemoteNG.My.Language.strMenuPortScan;
				Text = mRemoteNG.My.Language.strMenuPortScan;
			}

			private void ShowImportControls(bool controlsVisible)
			{
				pnlPorts.Visible = controlsVisible;
				pnlImport.Visible = controlsVisible;
				if (controlsVisible) {
					lvHosts.Height = pnlImport.Top - lvHosts.Top;
				} else {
					lvHosts.Height = pnlImport.Bottom - lvHosts.Top;
				}
			}

			private void StartScan()
			{
				try {
					_scanning = true;
					SwitchButtonText();
					lvHosts.Items.Clear();

					System.Net.IPAddress ipAddressStart = System.Net.IPAddress.Parse(ipStart.Text);
					System.Net.IPAddress ipAddressEnd = System.Net.IPAddress.Parse(ipEnd.Text);

					if (_import) {
						_portScanner = new Scanner(ipAddressStart, ipAddressEnd);
					} else {
						_portScanner = new Scanner(ipAddressStart, ipAddressEnd, portStart.Value, portEnd.Value);
					}

					_portScanner.BeginHostScan += PortScanner_BeginHostScan;
					_portScanner.HostScanned += PortScanner_HostScanned;
					_portScanner.ScanComplete += PortScanner_ScanComplete;

					_portScanner.StartScan();
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "StartScan failed (UI.Window.PortScan)" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void StopScan()
			{
				if (_portScanner != null)
					_portScanner.StopScan();
				_scanning = false;
				SwitchButtonText();
			}

			private void SwitchButtonText()
			{
				if (_scanning) {
					btnScan.Text = mRemoteNG.My.Language.strButtonStop;
				} else {
					btnScan.Text = mRemoteNG.My.Language.strButtonScan;
				}

				prgBar.Maximum = 100;
				prgBar.Value = 0;
			}

			private static void PortScanner_BeginHostScan(string host)
			{
				mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, "Scanning " + host, true);
			}

			private delegate void PortScannerHostScannedDelegate(ScanHost host, int scannedCount, int totalCount);
			private void PortScanner_HostScanned(ScanHost host, int scannedCount, int totalCount)
			{
				if (InvokeRequired) {
					Invoke(new PortScannerHostScannedDelegate(PortScanner_HostScanned), new object[] {
						host,
						scannedCount,
						totalCount
					});
					return;
				}

				mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, "Host scanned " + host.HostIp, true);

				ListViewItem listViewItem = host.ToListViewItem(_import);
				if (listViewItem != null) {
					lvHosts.Items.Add(listViewItem);
					listViewItem.EnsureVisible();
				}

				prgBar.Maximum = totalCount;
				prgBar.Value = scannedCount;
			}

			private delegate void PortScannerScanComplete(List<ScanHost> hosts);
			private void PortScanner_ScanComplete(List<ScanHost> hosts)
			{
				if (InvokeRequired) {
					Invoke(new PortScannerScanComplete(PortScanner_ScanComplete), new object[] { hosts });
					return;
				}

				mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, Language.strPortScanComplete);

				_scanning = false;
				SwitchButtonText();
			}
			#endregion
		}
	}
}
