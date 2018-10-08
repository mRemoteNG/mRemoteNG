
using mRemoteNG.Themes;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Window
{
	public partial class PortScanWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
				
		internal Controls.Base.NGLabel lblEndIP;
		internal Controls.Base.NGLabel lblStartIP;
		internal Controls.Base.NGButton btnScan;
		internal IPTextBox ipEnd;
		internal Controls.Base.NGListView olvHosts;
		internal BrightIdeasSoftware.OLVColumn clmHost;
		internal BrightIdeasSoftware.OLVColumn clmSSH;
		internal BrightIdeasSoftware.OLVColumn clmTelnet;
		internal BrightIdeasSoftware.OLVColumn clmHTTP;
		internal BrightIdeasSoftware.OLVColumn clmHTTPS;
		internal BrightIdeasSoftware.OLVColumn clmRlogin;
		internal BrightIdeasSoftware.OLVColumn clmRDP;
		internal BrightIdeasSoftware.OLVColumn clmVNC;
		internal BrightIdeasSoftware.OLVColumn clmOpenPorts;
		internal BrightIdeasSoftware.OLVColumn clmClosedPorts;
		internal Controls.Base.NGProgressBar prgBar;
		internal Controls.Base.NGLabel lblOnlyImport;
		internal Controls.Base.NGComboBox cbProtocol;
		internal System.Windows.Forms.Panel pnlScan;
		internal Controls.Base.NGNumericUpDown portEnd;
		internal Controls.Base.NGNumericUpDown portStart;
		internal Controls.Base.NGLabel Label2;
		internal Controls.Base.NGLabel Label1;
		internal Controls.Base.NGButton btnImport;
		internal IPTextBox ipStart;
				
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortScanWindow));
            this.ipStart = new mRemoteNG.UI.Controls.IPTextBox();
            this.ipEnd = new mRemoteNG.UI.Controls.IPTextBox();
            this.lblStartIP = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblEndIP = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnScan = new mRemoteNG.UI.Controls.Base.NGButton();
            this.olvHosts = new mRemoteNG.UI.Controls.Base.NGListView();
            this.resultsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importHTTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importHTTPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRloginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSSH2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importTelnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importVNCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new mRemoteNG.UI.Controls.Base.NGButton();
            this.cbProtocol = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.lblOnlyImport = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.clmHost = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmSSH = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmTelnet = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmHTTP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmHTTPS = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmRlogin = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmRDP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmVNC = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmOpenPorts = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.clmClosedPorts = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.prgBar = new mRemoteNG.UI.Controls.Base.NGProgressBar();
            this.pnlScan = new System.Windows.Forms.Panel();
            this.numericSelectorTimeout = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.portEnd = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.portStart = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.Label2 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.Label1 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pnlImport = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.olvHosts)).BeginInit();
            this.resultsMenuStrip.SuspendLayout();
            this.pnlScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSelectorTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portStart)).BeginInit();
            this.pnlImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipStart
            // 
            this.ipStart.Location = new System.Drawing.Point(5, 19);
            this.ipStart.Name = "ipStart";
            this.ipStart.Size = new System.Drawing.Size(130, 20);
            this.ipStart.TabIndex = 1;
            this.ipStart.ToolTipText = "";
            // 
            // ipEnd
            // 
            this.ipEnd.Location = new System.Drawing.Point(155, 19);
            this.ipEnd.Name = "ipEnd";
            this.ipEnd.Size = new System.Drawing.Size(130, 20);
            this.ipEnd.TabIndex = 2;
            this.ipEnd.ToolTipText = "";
            // 
            // lblStartIP
            // 
            this.lblStartIP.AutoSize = true;
            this.lblStartIP.Location = new System.Drawing.Point(3, 5);
            this.lblStartIP.Name = "lblStartIP";
            this.lblStartIP.Size = new System.Drawing.Size(46, 13);
            this.lblStartIP.TabIndex = 0;
            this.lblStartIP.Text = "Start IP:";
            // 
            // lblEndIP
            // 
            this.lblEndIP.AutoSize = true;
            this.lblEndIP.Location = new System.Drawing.Point(152, 5);
            this.lblEndIP.Name = "lblEndIP";
            this.lblEndIP.Size = new System.Drawing.Size(42, 13);
            this.lblEndIP.TabIndex = 5;
            this.lblEndIP.Text = "End IP:";
            // 
            // btnScan
            // 
            this.btnScan._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Image = global::mRemoteNG.Resources.Search;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnScan.Location = new System.Drawing.Point(769, 5);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(110, 55);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "&Scan";
            this.btnScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // olvHosts
            // 
            this.olvHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvHosts.CellEditUseWholeCell = false;
            this.olvHosts.ContextMenuStrip = this.resultsMenuStrip;
            this.olvHosts.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvHosts.DecorateLines = true;
            this.olvHosts.FullRowSelect = true;
            this.olvHosts.GridLines = true;
            this.olvHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.olvHosts.HideSelection = false;
            this.olvHosts.Location = new System.Drawing.Point(12, 73);
            this.olvHosts.Name = "olvHosts";
            this.olvHosts.ShowGroups = false;
            this.olvHosts.Size = new System.Drawing.Size(883, 290);
            this.olvHosts.TabIndex = 26;
            this.olvHosts.UseCompatibleStateImageBehavior = false;
            this.olvHosts.View = System.Windows.Forms.View.Details;
            // 
            // resultsMenuStrip
            // 
            this.resultsMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importHTTPToolStripMenuItem,
            this.importHTTPSToolStripMenuItem,
            this.importRDPToolStripMenuItem,
            this.importRloginToolStripMenuItem,
            this.importSSH2ToolStripMenuItem,
            this.importTelnetToolStripMenuItem,
            this.importVNCToolStripMenuItem});
            this.resultsMenuStrip.Name = "resultsMenuStrip";
            this.resultsMenuStrip.Size = new System.Drawing.Size(150, 158);
            // 
            // importHTTPToolStripMenuItem
            // 
            this.importHTTPToolStripMenuItem.Name = "importHTTPToolStripMenuItem";
            this.importHTTPToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importHTTPToolStripMenuItem.Text = "Import HTTP";
            this.importHTTPToolStripMenuItem.Click += new System.EventHandler(this.importHTTPToolStripMenuItem_Click);
            // 
            // importHTTPSToolStripMenuItem
            // 
            this.importHTTPSToolStripMenuItem.Name = "importHTTPSToolStripMenuItem";
            this.importHTTPSToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importHTTPSToolStripMenuItem.Text = "Import HTTPS";
            this.importHTTPSToolStripMenuItem.Click += new System.EventHandler(this.importHTTPSToolStripMenuItem_Click);
            // 
            // importRDPToolStripMenuItem
            // 
            this.importRDPToolStripMenuItem.Name = "importRDPToolStripMenuItem";
            this.importRDPToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importRDPToolStripMenuItem.Text = "Import RDP";
            this.importRDPToolStripMenuItem.Click += new System.EventHandler(this.importRDPToolStripMenuItem_Click);
            // 
            // importRloginToolStripMenuItem
            // 
            this.importRloginToolStripMenuItem.Name = "importRloginToolStripMenuItem";
            this.importRloginToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importRloginToolStripMenuItem.Text = "Import Rlogin";
            this.importRloginToolStripMenuItem.Click += new System.EventHandler(this.importRloginToolStripMenuItem_Click);
            // 
            // importSSH2ToolStripMenuItem
            // 
            this.importSSH2ToolStripMenuItem.Name = "importSSH2ToolStripMenuItem";
            this.importSSH2ToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importSSH2ToolStripMenuItem.Text = "Import SSH2";
            this.importSSH2ToolStripMenuItem.Click += new System.EventHandler(this.importSSH2ToolStripMenuItem_Click);
            // 
            // importTelnetToolStripMenuItem
            // 
            this.importTelnetToolStripMenuItem.Name = "importTelnetToolStripMenuItem";
            this.importTelnetToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importTelnetToolStripMenuItem.Text = "Import Telnet";
            this.importTelnetToolStripMenuItem.Click += new System.EventHandler(this.importTelnetToolStripMenuItem_Click);
            // 
            // importVNCToolStripMenuItem
            // 
            this.importVNCToolStripMenuItem.Name = "importVNCToolStripMenuItem";
            this.importVNCToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.importVNCToolStripMenuItem.Text = "Import VNC";
            this.importVNCToolStripMenuItem.Click += new System.EventHandler(this.importVNCToolStripMenuItem_Click);
            // 
            // btnImport
            // 
            this.btnImport._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(800, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 40);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cbProtocol
            // 
            this.cbProtocol._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cbProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtocol.FormattingEnabled = true;
            this.cbProtocol.Items.AddRange(new object[] {
            "SSH2",
            "Telnet",
            "HTTP",
            "HTTPS",
            "Rlogin",
            "RDP",
            "VNC"});
            this.cbProtocol.Location = new System.Drawing.Point(5, 25);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(122, 21);
            this.cbProtocol.TabIndex = 7;
            // 
            // lblOnlyImport
            // 
            this.lblOnlyImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOnlyImport.AutoSize = true;
            this.lblOnlyImport.Location = new System.Drawing.Point(2, 5);
            this.lblOnlyImport.Name = "lblOnlyImport";
            this.lblOnlyImport.Size = new System.Drawing.Size(104, 13);
            this.lblOnlyImport.TabIndex = 1;
            this.lblOnlyImport.Text = "Protocol to import:";
            // 
            // clmHost
            // 
            this.clmHost.AspectName = "HostIPorName";
            this.clmHost.FillsFreeSpace = true;
            this.clmHost.Text = "Hostname/IP";
            this.clmHost.Width = 130;
            // 
            // clmSSH
            // 
            this.clmSSH.AspectName = "SshName";
            this.clmSSH.Text = "SSH";
            this.clmSSH.Width = 50;
            // 
            // clmTelnet
            // 
            this.clmTelnet.AspectName = "TelnetName";
            this.clmTelnet.Text = "Telnet";
            this.clmTelnet.Width = 50;
            // 
            // clmHTTP
            // 
            this.clmHTTP.AspectName = "HttpName";
            this.clmHTTP.Text = "HTTP";
            this.clmHTTP.Width = 50;
            // 
            // clmHTTPS
            // 
            this.clmHTTPS.AspectName = "HttpsName";
            this.clmHTTPS.Text = "HTTPS";
            this.clmHTTPS.Width = 50;
            // 
            // clmRlogin
            // 
            this.clmRlogin.AspectName = "RloginName";
            this.clmRlogin.Text = "Rlogin";
            this.clmRlogin.Width = 50;
            // 
            // clmRDP
            // 
            this.clmRDP.AspectName = "RdpName";
            this.clmRDP.Text = "RDP";
            this.clmRDP.Width = 50;
            // 
            // clmVNC
            // 
            this.clmVNC.AspectName = "VncName";
            this.clmVNC.Text = "VNC";
            this.clmVNC.Width = 50;
            // 
            // clmOpenPorts
            // 
            this.clmOpenPorts.AspectName = "OpenPortsName";
            this.clmOpenPorts.FillsFreeSpace = true;
            this.clmOpenPorts.Text = "Open Ports";
            this.clmOpenPorts.Width = 150;
            // 
            // clmClosedPorts
            // 
            this.clmClosedPorts.AspectName = "ClosedPortsName";
            this.clmClosedPorts.FillsFreeSpace = true;
            this.clmClosedPorts.Text = "Closed Ports";
            this.clmClosedPorts.Width = 150;
            // 
            // prgBar
            // 
            this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgBar.Location = new System.Drawing.Point(5, 45);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(760, 15);
            this.prgBar.Step = 1;
            this.prgBar.TabIndex = 28;
            // 
            // pnlScan
            // 
            this.pnlScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlScan.Controls.Add(this.numericSelectorTimeout);
            this.pnlScan.Controls.Add(this.lblTimeout);
            this.pnlScan.Controls.Add(this.portEnd);
            this.pnlScan.Controls.Add(this.portStart);
            this.pnlScan.Controls.Add(this.prgBar);
            this.pnlScan.Controls.Add(this.Label2);
            this.pnlScan.Controls.Add(this.lblStartIP);
            this.pnlScan.Controls.Add(this.lblEndIP);
            this.pnlScan.Controls.Add(this.ipStart);
            this.pnlScan.Controls.Add(this.btnScan);
            this.pnlScan.Controls.Add(this.Label1);
            this.pnlScan.Controls.Add(this.ipEnd);
            this.pnlScan.Location = new System.Drawing.Point(12, 5);
            this.pnlScan.Name = "pnlScan";
            this.pnlScan.Size = new System.Drawing.Size(883, 65);
            this.pnlScan.TabIndex = 18;
            // 
            // numericSelectorTimeout
            // 
            this.numericSelectorTimeout.Location = new System.Drawing.Point(600, 17);
            this.numericSelectorTimeout.Maximum = new decimal(new int[] {
            2147482,
            0,
            0,
            0});
            this.numericSelectorTimeout.Name = "numericSelectorTimeout";
            this.numericSelectorTimeout.Size = new System.Drawing.Size(67, 22);
            this.numericSelectorTimeout.TabIndex = 5;
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(597, 1);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(102, 13);
            this.lblTimeout.TabIndex = 16;
            this.lblTimeout.Text = "Timeout (seconds):";
            // 
            // portEnd
            // 
            this.portEnd.Location = new System.Drawing.Point(490, 17);
            this.portEnd.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portEnd.Name = "portEnd";
            this.portEnd.Size = new System.Drawing.Size(67, 22);
            this.portEnd.TabIndex = 4;
            this.portEnd.Enter += new System.EventHandler(this.portEnd_Enter);
            // 
            // portStart
            // 
            this.portStart.Location = new System.Drawing.Point(375, 17);
            this.portStart.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portStart.Name = "portStart";
            this.portStart.Size = new System.Drawing.Size(67, 22);
            this.portStart.TabIndex = 3;
            this.portStart.Enter += new System.EventHandler(this.portStart_Enter);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(487, 1);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(54, 13);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "End Port:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(372, 1);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(58, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Start Port:";
            // 
            // pnlImport
            // 
            this.pnlImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlImport.Controls.Add(this.btnImport);
            this.pnlImport.Controls.Add(this.lblOnlyImport);
            this.pnlImport.Controls.Add(this.cbProtocol);
            this.pnlImport.Location = new System.Drawing.Point(12, 364);
            this.pnlImport.Name = "pnlImport";
            this.pnlImport.Size = new System.Drawing.Size(883, 50);
            this.pnlImport.TabIndex = 102;
            // 
            // PortScanWindow
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(908, 421);
            this.Controls.Add(this.pnlImport);
            this.Controls.Add(this.olvHosts);
            this.Controls.Add(this.pnlScan);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(924, 460);
            this.Name = "PortScanWindow";
            this.TabText = "Port Scan";
            this.Text = "Port Scan";
            this.Load += new System.EventHandler(this.PortScan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvHosts)).EndInit();
            this.resultsMenuStrip.ResumeLayout(false);
            this.pnlScan.ResumeLayout(false);
            this.pnlScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSelectorTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portStart)).EndInit();
            this.pnlImport.ResumeLayout(false);
            this.pnlImport.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.Panel pnlImport;
        #endregion

        private System.Windows.Forms.ContextMenuStrip resultsMenuStrip;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolStripMenuItem importHTTPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importHTTPSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importRDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importRloginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSSH2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importTelnetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importVNCToolStripMenuItem;
		private System.Windows.Forms.Label lblTimeout;
		private Controls.Base.NGNumericUpDown numericSelectorTimeout;
	}
}
