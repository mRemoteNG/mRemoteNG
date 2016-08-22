
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Window
{
	public partial class PortScanWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
				
		internal System.Windows.Forms.Label lblEndIP;
		internal System.Windows.Forms.Label lblStartIP;
		internal System.Windows.Forms.Button btnScan;
		internal IPTextBox ipEnd;
		internal System.Windows.Forms.ListView lvHosts;
		internal System.Windows.Forms.ColumnHeader clmHost;
		internal System.Windows.Forms.ColumnHeader clmSSH;
		internal System.Windows.Forms.ColumnHeader clmTelnet;
		internal System.Windows.Forms.ColumnHeader clmHTTP;
		internal System.Windows.Forms.ColumnHeader clmHTTPS;
		internal System.Windows.Forms.ColumnHeader clmRlogin;
		internal System.Windows.Forms.ColumnHeader clmRDP;
		internal System.Windows.Forms.ColumnHeader clmVNC;
		internal System.Windows.Forms.ColumnHeader clmOpenPorts;
		internal System.Windows.Forms.ColumnHeader clmClosedPorts;
		internal System.Windows.Forms.ProgressBar prgBar;
		internal System.Windows.Forms.Label lblOnlyImport;
		internal System.Windows.Forms.ComboBox cbProtocol;
		internal System.Windows.Forms.Panel pnlPorts;
		internal System.Windows.Forms.NumericUpDown portEnd;
		internal System.Windows.Forms.NumericUpDown portStart;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Button btnImport;
		internal IPTextBox ipStart;
				
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortScanWindow));
            this.ipStart = new mRemoteNG.UI.Controls.IPTextBox();
            this.ipEnd = new mRemoteNG.UI.Controls.IPTextBox();
            this.lblStartIP = new System.Windows.Forms.Label();
            this.lblEndIP = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.lvHosts = new System.Windows.Forms.ListView();
            this.resultsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importHTTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importHTTPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRloginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSSH2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importTelnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importVNCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new System.Windows.Forms.Button();
            this.cbProtocol = new System.Windows.Forms.ComboBox();
            this.lblOnlyImport = new System.Windows.Forms.Label();
            this.clmHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSSH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmTelnet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmHTTP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmHTTPS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmRlogin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmRDP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmVNC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmOpenPorts = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmClosedPorts = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.pnlPorts = new System.Windows.Forms.Panel();
            this.portEnd = new System.Windows.Forms.NumericUpDown();
            this.portStart = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.pnlImport = new System.Windows.Forms.Panel();
            this.resultsMenuStrip.SuspendLayout();
            this.pnlPorts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portStart)).BeginInit();
            this.pnlImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipStart
            // 
            this.ipStart.Location = new System.Drawing.Point(12, 25);
            this.ipStart.Name = "ipStart";
            this.ipStart.Size = new System.Drawing.Size(130, 20);
            this.ipStart.TabIndex = 10;
            this.ipStart.ToolTipText = "";
            // 
            // ipEnd
            // 
            this.ipEnd.Location = new System.Drawing.Point(148, 25);
            this.ipEnd.Name = "ipEnd";
            this.ipEnd.Size = new System.Drawing.Size(130, 20);
            this.ipEnd.TabIndex = 15;
            this.ipEnd.ToolTipText = "";
            // 
            // lblStartIP
            // 
            this.lblStartIP.AutoSize = true;
            this.lblStartIP.Location = new System.Drawing.Point(12, 7);
            this.lblStartIP.Name = "lblStartIP";
            this.lblStartIP.Size = new System.Drawing.Size(46, 13);
            this.lblStartIP.TabIndex = 0;
            this.lblStartIP.Text = "Start IP:";
            // 
            // lblEndIP
            // 
            this.lblEndIP.AutoSize = true;
            this.lblEndIP.Location = new System.Drawing.Point(148, 7);
            this.lblEndIP.Name = "lblEndIP";
            this.lblEndIP.Size = new System.Drawing.Size(42, 13);
            this.lblEndIP.TabIndex = 5;
            this.lblEndIP.Text = "End IP:";
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Image = global::mRemoteNG.Resources.Search;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnScan.Location = new System.Drawing.Point(592, 7);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(100, 60);
            this.btnScan.TabIndex = 20;
            this.btnScan.Text = "&Scan";
            this.btnScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lvHosts
            // 
            this.lvHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvHosts.ContextMenuStrip = this.resultsMenuStrip;
            this.lvHosts.FullRowSelect = true;
            this.lvHosts.GridLines = true;
            this.lvHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvHosts.HideSelection = false;
            this.lvHosts.Location = new System.Drawing.Point(12, 73);
            this.lvHosts.Name = "lvHosts";
            this.lvHosts.Size = new System.Drawing.Size(680, 290);
            this.lvHosts.TabIndex = 26;
            this.lvHosts.UseCompatibleStateImageBehavior = false;
            this.lvHosts.View = System.Windows.Forms.View.Details;
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
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(594, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 31);
            this.btnImport.TabIndex = 101;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cbProtocol
            // 
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
            this.cbProtocol.Location = new System.Drawing.Point(157, 6);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(122, 21);
            this.cbProtocol.TabIndex = 28;
            // 
            // lblOnlyImport
            // 
            this.lblOnlyImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOnlyImport.AutoSize = true;
            this.lblOnlyImport.Location = new System.Drawing.Point(5, 13);
            this.lblOnlyImport.Name = "lblOnlyImport";
            this.lblOnlyImport.Size = new System.Drawing.Size(104, 13);
            this.lblOnlyImport.TabIndex = 1;
            this.lblOnlyImport.Text = "Protocol to import:";
            // 
            // clmHost
            // 
            this.clmHost.Text = "Hostname/IP";
            this.clmHost.Width = 130;
            // 
            // clmSSH
            // 
            this.clmSSH.Text = "SSH";
            this.clmSSH.Width = 50;
            // 
            // clmTelnet
            // 
            this.clmTelnet.Text = "Telnet";
            this.clmTelnet.Width = 50;
            // 
            // clmHTTP
            // 
            this.clmHTTP.Text = "HTTP";
            this.clmHTTP.Width = 50;
            // 
            // clmHTTPS
            // 
            this.clmHTTPS.Text = "HTTPS";
            this.clmHTTPS.Width = 50;
            // 
            // clmRlogin
            // 
            this.clmRlogin.Text = "Rlogin";
            this.clmRlogin.Width = 50;
            // 
            // clmRDP
            // 
            this.clmRDP.Text = "RDP";
            this.clmRDP.Width = 50;
            // 
            // clmVNC
            // 
            this.clmVNC.Text = "VNC";
            this.clmVNC.Width = 50;
            // 
            // clmOpenPorts
            // 
            this.clmOpenPorts.Text = "Open Ports";
            this.clmOpenPorts.Width = 150;
            // 
            // clmClosedPorts
            // 
            this.clmClosedPorts.Text = "Closed Ports";
            this.clmClosedPorts.Width = 150;
            // 
            // prgBar
            // 
            this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgBar.Location = new System.Drawing.Point(12, 50);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(579, 16);
            this.prgBar.Step = 1;
            this.prgBar.TabIndex = 28;
            // 
            // pnlPorts
            // 
            this.pnlPorts.Controls.Add(this.portEnd);
            this.pnlPorts.Controls.Add(this.portStart);
            this.pnlPorts.Controls.Add(this.Label2);
            this.pnlPorts.Controls.Add(this.Label1);
            this.pnlPorts.Location = new System.Drawing.Point(284, 7);
            this.pnlPorts.Name = "pnlPorts";
            this.pnlPorts.Size = new System.Drawing.Size(307, 38);
            this.pnlPorts.TabIndex = 18;
            // 
            // portEnd
            // 
            this.portEnd.Location = new System.Drawing.Point(232, 5);
            this.portEnd.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portEnd.Name = "portEnd";
            this.portEnd.Size = new System.Drawing.Size(67, 22);
            this.portEnd.TabIndex = 15;
            this.portEnd.Enter += new System.EventHandler(this.portEnd_Enter);
            // 
            // portStart
            // 
            this.portStart.Location = new System.Drawing.Point(79, 5);
            this.portStart.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portStart.Name = "portStart";
            this.portStart.Size = new System.Drawing.Size(67, 22);
            this.portStart.TabIndex = 5;
            this.portStart.Enter += new System.EventHandler(this.portStart_Enter);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(162, 7);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(54, 13);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "End Port:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(3, 7);
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
            this.pnlImport.Location = new System.Drawing.Point(12, 369);
            this.pnlImport.Name = "pnlImport";
            this.pnlImport.Size = new System.Drawing.Size(680, 40);
            this.pnlImport.TabIndex = 102;
            // 
            // PortScanWindow
            // 
            this.AcceptButton = this.btnImport;
            this.ClientSize = new System.Drawing.Size(704, 421);
            this.Controls.Add(this.pnlImport);
            this.Controls.Add(this.lvHosts);
            this.Controls.Add(this.pnlPorts);
            this.Controls.Add(this.prgBar);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.lblEndIP);
            this.Controls.Add(this.lblStartIP);
            this.Controls.Add(this.ipEnd);
            this.Controls.Add(this.ipStart);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PortScanWindow";
            this.TabText = "Port Scan";
            this.Text = "Port Scan";
            this.Load += new System.EventHandler(this.PortScan_Load);
            this.resultsMenuStrip.ResumeLayout(false);
            this.pnlPorts.ResumeLayout(false);
            this.pnlPorts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portStart)).EndInit();
            this.pnlImport.ResumeLayout(false);
            this.pnlImport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}
