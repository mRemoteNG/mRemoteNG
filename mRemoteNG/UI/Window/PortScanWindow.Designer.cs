
using mRemoteNG.Themes;
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Window
{
	public partial class PortScanWindow : BaseWindow
	{
        #region  Windows Form Designer generated code
				
		internal Controls.MrngLabel lblStartIP;
		internal Controls.MrngListView olvHosts;
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
		internal Controls.MrngProgressBar prgBar;
		internal Controls.MrngLabel lblOnlyImport;
		internal MrngComboBox cbProtocol;
		internal MrngButton btnImport;
		internal MrngIpTextBox txtIpRange;
				
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PortScanWindow));
            this.txtIpRange = new mRemoteNG.UI.Controls.MrngIpTextBox();
            this.lblStartIP = new mRemoteNG.UI.Controls.MrngLabel();
            this.olvHosts = new mRemoteNG.UI.Controls.MrngListView();
            this.resultsMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importHTTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importHTTPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRloginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSSH2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importTelnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importVNCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new MrngButton();
            this.cbProtocol = new MrngComboBox();
            this.lblOnlyImport = new mRemoteNG.UI.Controls.MrngLabel();
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
            this.prgBar = new mRemoteNG.UI.Controls.MrngProgressBar();
            this.numericSelectorTimeout = new mRemoteNG.UI.Controls.MrngNumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.pnlIp = new System.Windows.Forms.TableLayoutPanel();
            this.btnScan = new MrngButton();
            this.lblPorts = new mRemoteNG.UI.Controls.MrngLabel();
            this.pnlPortMode = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoCommonPorts = new MrngRadioButton();
            this.rdoAllPorts = new MrngRadioButton();
            this.rdoCustomPorts = new MrngRadioButton();
            this.txtCustomPorts = new mRemoteNG.UI.Controls.MrngTextBox();
            this.pnlImport = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.portScanToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.olvHosts)).BeginInit();
            this.resultsMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSelectorTimeout)).BeginInit();
            this.pnlIp.SuspendLayout();
            this.pnlPortMode.SuspendLayout();
            this.pnlImport.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIpRange
            //
            this.txtIpRange.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtIpRange.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIpRange.Margin = new System.Windows.Forms.Padding(0);
            this.txtIpRange.Name = "txtIpRange";
            // Wide enough for a range of two full-length IPv6 addresses (39 chars each).
            this.txtIpRange.Size = new System.Drawing.Size(460, 22);
            this.txtIpRange.TabIndex = 1;
            this.txtIpRange.ToolTipText = "";
            //
            // lblStartIP
            //
            this.lblStartIP.AutoSize = true;
            this.lblStartIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStartIP.Location = new System.Drawing.Point(3, 0);
            this.lblStartIP.Name = "lblStartIP";
            this.lblStartIP.Size = new System.Drawing.Size(124, 28);
            this.lblStartIP.TabIndex = 0;
            this.lblStartIP.Text = "IP / Range / CIDR";
            this.lblStartIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblPorts
            //
            this.lblPorts.AutoSize = true;
            this.lblPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPorts.Name = "lblPorts";
            this.lblPorts.Size = new System.Drawing.Size(124, 28);
            this.lblPorts.TabIndex = 5;
            this.lblPorts.Text = "Ports";
            this.lblPorts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlPortMode
            //
            this.pnlPortMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pnlPortMode.AutoSize = true;
            this.pnlPortMode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPortMode.Controls.Add(this.rdoCommonPorts);
            this.pnlPortMode.Controls.Add(this.rdoAllPorts);
            this.pnlPortMode.Controls.Add(this.rdoCustomPorts);
            this.pnlPortMode.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.pnlPortMode.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPortMode.Name = "pnlPortMode";
            this.pnlPortMode.Size = new System.Drawing.Size(400, 22);
            this.pnlPortMode.TabIndex = 2;
            this.pnlPortMode.WrapContents = false;
            //
            // rdoCommonPorts
            //
            this.rdoCommonPorts.Checked = true;
            this.rdoCommonPorts.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoCommonPorts.Margin = new System.Windows.Forms.Padding(0, 3, 12, 0);
            this.rdoCommonPorts.Name = "rdoCommonPorts";
            this.rdoCommonPorts.Size = new System.Drawing.Size(105, 17);
            this.rdoCommonPorts.TabIndex = 2;
            this.rdoCommonPorts.TabStop = true;
            this.rdoCommonPorts.Text = "Common ports";
            this.rdoCommonPorts.UseVisualStyleBackColor = true;
            this.rdoCommonPorts.CheckedChanged += new System.EventHandler(this.PortMode_CheckedChanged);
            //
            // rdoAllPorts
            //
            this.rdoAllPorts.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAllPorts.Margin = new System.Windows.Forms.Padding(0, 3, 12, 0);
            this.rdoAllPorts.Name = "rdoAllPorts";
            this.rdoAllPorts.Size = new System.Drawing.Size(75, 17);
            this.rdoAllPorts.TabIndex = 3;
            this.rdoAllPorts.Text = "All ports";
            this.rdoAllPorts.UseVisualStyleBackColor = true;
            this.rdoAllPorts.CheckedChanged += new System.EventHandler(this.PortMode_CheckedChanged);
            //
            // rdoCustomPorts
            //
            this.rdoCustomPorts.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoCustomPorts.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.rdoCustomPorts.Name = "rdoCustomPorts";
            this.rdoCustomPorts.Size = new System.Drawing.Size(70, 17);
            this.rdoCustomPorts.TabIndex = 4;
            this.rdoCustomPorts.Text = "Custom";
            this.rdoCustomPorts.UseVisualStyleBackColor = true;
            this.rdoCustomPorts.CheckedChanged += new System.EventHandler(this.PortMode_CheckedChanged);
            //
            // txtCustomPorts
            //
            this.txtCustomPorts.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCustomPorts.Enabled = false;
            this.txtCustomPorts.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomPorts.Margin = new System.Windows.Forms.Padding(0);
            this.txtCustomPorts.Name = "txtCustomPorts";
            this.txtCustomPorts.Size = new System.Drawing.Size(460, 22);
            this.txtCustomPorts.TabIndex = 5;
            //
            // olvHosts
            // 
            this.olvHosts.CellEditUseWholeCell = false;
            this.olvHosts.ContextMenuStrip = this.resultsMenuStrip;
            this.olvHosts.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvHosts.DecorateLines = true;
            this.olvHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvHosts.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvHosts.FullRowSelect = true;
            this.olvHosts.GridLines = true;
            this.olvHosts.HideSelection = false;
            this.olvHosts.Location = new System.Drawing.Point(3, 168);
            this.olvHosts.Name = "olvHosts";
            this.olvHosts.ShowGroups = false;
            this.olvHosts.Size = new System.Drawing.Size(878, 230);
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
            this.resultsMenuStrip.Size = new System.Drawing.Size(148, 158);
            // 
            // importHTTPToolStripMenuItem
            // 
            this.importHTTPToolStripMenuItem.Name = "importHTTPToolStripMenuItem";
            this.importHTTPToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importHTTPToolStripMenuItem.Text = "Import HTTP";
            this.importHTTPToolStripMenuItem.Click += new System.EventHandler(this.importHTTPToolStripMenuItem_Click);
            // 
            // importHTTPSToolStripMenuItem
            // 
            this.importHTTPSToolStripMenuItem.Name = "importHTTPSToolStripMenuItem";
            this.importHTTPSToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importHTTPSToolStripMenuItem.Text = "Import HTTPS";
            this.importHTTPSToolStripMenuItem.Click += new System.EventHandler(this.importHTTPSToolStripMenuItem_Click);
            // 
            // importRDPToolStripMenuItem
            // 
            this.importRDPToolStripMenuItem.Name = "importRDPToolStripMenuItem";
            this.importRDPToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importRDPToolStripMenuItem.Text = "Import RDP";
            this.importRDPToolStripMenuItem.Click += new System.EventHandler(this.importRDPToolStripMenuItem_Click);
            // 
            // importRloginToolStripMenuItem
            // 
            this.importRloginToolStripMenuItem.Name = "importRloginToolStripMenuItem";
            this.importRloginToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importRloginToolStripMenuItem.Text = "Import Rlogin";
            this.importRloginToolStripMenuItem.Click += new System.EventHandler(this.importRloginToolStripMenuItem_Click);
            // 
            // importSSH2ToolStripMenuItem
            // 
            this.importSSH2ToolStripMenuItem.Name = "importSSH2ToolStripMenuItem";
            this.importSSH2ToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importSSH2ToolStripMenuItem.Text = "Import SSH2";
            this.importSSH2ToolStripMenuItem.Click += new System.EventHandler(this.importSSH2ToolStripMenuItem_Click);
            // 
            // importTelnetToolStripMenuItem
            // 
            this.importTelnetToolStripMenuItem.Name = "importTelnetToolStripMenuItem";
            this.importTelnetToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importTelnetToolStripMenuItem.Text = "Import Telnet";
            this.importTelnetToolStripMenuItem.Click += new System.EventHandler(this.importTelnetToolStripMenuItem_Click);
            // 
            // importVNCToolStripMenuItem
            // 
            this.importVNCToolStripMenuItem.Name = "importVNCToolStripMenuItem";
            this.importVNCToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importVNCToolStripMenuItem.Text = "Import VNC";
            this.importVNCToolStripMenuItem.Click += new System.EventHandler(this.importVNCToolStripMenuItem_Click);
            // 
            // btnImport
            // 
            this.btnImport._mice = MrngButton.MouseState.OUT;
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(765, 27);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(110, 24);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cbProtocol
            // 
            this.cbProtocol._mice = MrngComboBox.MouseState.HOVER;
            this.cbProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.cbProtocol.Location = new System.Drawing.Point(3, 27);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(144, 21);
            this.cbProtocol.TabIndex = 7;
            // 
            // lblOnlyImport
            // 
            this.lblOnlyImport.AutoSize = true;
            this.lblOnlyImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOnlyImport.Location = new System.Drawing.Point(3, 0);
            this.lblOnlyImport.Name = "lblOnlyImport";
            this.lblOnlyImport.Size = new System.Drawing.Size(144, 24);
            this.lblOnlyImport.TabIndex = 1;
            this.lblOnlyImport.Text = "Protocol to import";
            this.lblOnlyImport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.prgBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgBar.Location = new System.Drawing.Point(3, 138);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(878, 24);
            this.prgBar.Step = 1;
            this.prgBar.TabIndex = 28;
            // 
            // numericSelectorTimeout
            // 
            this.numericSelectorTimeout.Location = new System.Drawing.Point(133, 99);
            this.numericSelectorTimeout.Maximum = new decimal(new int[] {
            2147482,
            0,
            0,
            0});
            this.numericSelectorTimeout.Name = "numericSelectorTimeout";
            this.numericSelectorTimeout.Size = new System.Drawing.Size(67, 22);
            this.numericSelectorTimeout.TabIndex = 6;
            this.numericSelectorTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimeout.Location = new System.Drawing.Point(3, 96);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(124, 33);
            this.lblTimeout.TabIndex = 16;
            this.lblTimeout.Text = "Timeout [seconds]";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlIp
            //
            this.pnlIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIp.ColumnCount = 3;
            this.pnlIp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.pnlIp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.pnlIp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlIp.Controls.Add(this.lblStartIP, 0, 0);
            this.pnlIp.Controls.Add(this.txtIpRange, 1, 0);
            this.pnlIp.SetColumnSpan(this.txtIpRange, 2);
            this.pnlIp.Controls.Add(this.lblPorts, 0, 1);
            this.pnlIp.Controls.Add(this.pnlPortMode, 1, 1);
            this.pnlIp.SetColumnSpan(this.pnlPortMode, 2);
            this.pnlIp.Controls.Add(this.txtCustomPorts, 1, 2);
            this.pnlIp.SetColumnSpan(this.txtCustomPorts, 2);
            this.pnlIp.Controls.Add(this.lblTimeout, 0, 3);
            this.pnlIp.Controls.Add(this.numericSelectorTimeout, 1, 3);
            this.pnlIp.Controls.Add(this.btnScan, 2, 3);
            this.pnlIp.Location = new System.Drawing.Point(3, 3);
            this.pnlIp.Name = "pnlIp";
            this.pnlIp.RowCount = 4;
            this.pnlIp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.pnlIp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.pnlIp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.pnlIp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.pnlIp.Size = new System.Drawing.Size(878, 110);
            this.pnlIp.TabIndex = 103;
            // 
            // btnScan
            // 
            this.btnScan._mice = MrngButton.MouseState.OUT;
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Image = global::mRemoteNG.Properties.Resources.Search_16x;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnScan.Location = new System.Drawing.Point(765, 99);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(110, 24);
            this.btnScan.TabIndex = 7;
            this.btnScan.Text = "&Scan";
            this.btnScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            //
            // pnlImport
            // 
            this.pnlImport.ColumnCount = 2;
            this.pnlImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.pnlImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlImport.Controls.Add(this.lblOnlyImport, 0, 0);
            this.pnlImport.Controls.Add(this.cbProtocol, 0, 1);
            this.pnlImport.Controls.Add(this.btnImport, 1, 1);
            this.pnlImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImport.Location = new System.Drawing.Point(3, 404);
            this.pnlImport.Name = "pnlImport";
            this.pnlImport.RowCount = 2;
            this.pnlImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.pnlImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.pnlImport.Size = new System.Drawing.Size(878, 54);
            this.pnlImport.TabIndex = 104;
            // 
            // pnlMain
            // 
            this.pnlMain.ColumnCount = 1;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.Controls.Add(this.pnlIp, 0, 0);
            this.pnlMain.Controls.Add(this.prgBar, 0, 1);
            this.pnlMain.Controls.Add(this.pnlImport, 0, 3);
            this.pnlMain.Controls.Add(this.olvHosts, 0, 2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.RowCount = 4;
            // Sized to pnlIp's four rows (28+28+26+28) plus its 3px top/bottom margins. Keep in step
            // with pnlIp's RowStyles, otherwise a gap opens up above the progress bar.
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.pnlMain.Size = new System.Drawing.Size(884, 461);
            this.pnlMain.TabIndex = 105;
            // 
            // PortScanWindow
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PortScanWindow";
            this.TabText = "Port Scan";
            this.Text = "Port Scan";
            this.Load += new System.EventHandler(this.PortScan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvHosts)).EndInit();
            this.resultsMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericSelectorTimeout)).EndInit();
            this.pnlPortMode.ResumeLayout(false);
            this.pnlPortMode.PerformLayout();
            this.pnlIp.ResumeLayout(false);
            this.pnlIp.PerformLayout();
            this.pnlImport.ResumeLayout(false);
            this.pnlImport.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}
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
		private Controls.MrngNumericUpDown numericSelectorTimeout;
        private System.Windows.Forms.TableLayoutPanel pnlIp;
        private System.Windows.Forms.TableLayoutPanel pnlImport;
        internal MrngButton btnScan;
        private System.Windows.Forms.TableLayoutPanel pnlMain;
        private System.Windows.Forms.ToolTip portScanToolTip;
        private Controls.MrngLabel lblPorts;
        private System.Windows.Forms.FlowLayoutPanel pnlPortMode;
        internal MrngRadioButton rdoCommonPorts;
        internal MrngRadioButton rdoAllPorts;
        internal MrngRadioButton rdoCustomPorts;
        private Controls.MrngTextBox txtCustomPorts;
    }
}
