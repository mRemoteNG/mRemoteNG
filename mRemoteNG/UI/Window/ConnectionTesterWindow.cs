using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using mRemoteNG.PluginSystem;
using mRemoteNG.UI.Controls;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class ConnectionTesterWindow : BaseWindow
    {
        private TableLayoutPanel pnlMain;
        private MrngListView olvResults;
        private OLVColumn clmName;
        private OLVColumn clmHost;
        private OLVColumn clmPort;
        private OLVColumn clmStatus;
        private Panel pnlControls;
        private MrngButton btnStart;
        private MrngProgressBar prgBar;
        private bool _isTesting;
        private System.Threading.CancellationTokenSource? _cancellationTokenSource;

        public ConnectionTesterWindow()
        {
            InitializeComponent();
            WindowType = WindowType.ConnectionTester;
            DockPnl = new DockContent();
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.olvResults = new mRemoteNG.UI.Controls.MrngListView();
            this.clmName = new BrightIdeasSoftware.OLVColumn();
            this.clmHost = new BrightIdeasSoftware.OLVColumn();
            this.clmPort = new BrightIdeasSoftware.OLVColumn();
            this.clmStatus = new BrightIdeasSoftware.OLVColumn();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnStart = new mRemoteNG.UI.Controls.MrngButton();
            this.prgBar = new mRemoteNG.UI.Controls.MrngProgressBar();

            ((System.ComponentModel.ISupportInitialize)(this.olvResults)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlMain
            // 
            this.pnlMain.ColumnCount = 1;
            this.pnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.Controls.Add(this.pnlControls, 0, 0);
            this.pnlMain.Controls.Add(this.olvResults, 0, 1);
            this.pnlMain.Controls.Add(this.prgBar, 0, 2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.RowCount = 3;
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnStart);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Fill;
            
            // 
            // btnStart
            // 
            this.btnStart.Text = "Start Test";
            this.btnStart.Location = new System.Drawing.Point(10, 8);
            this.btnStart.Size = new System.Drawing.Size(100, 24);
            this.btnStart.Click += BtnStart_Click;
            this.btnStart.UseVisualStyleBackColor = true;

            // 
            // olvResults
            // 
            this.olvResults.AllColumns.Add(this.clmName);
            this.olvResults.AllColumns.Add(this.clmHost);
            this.olvResults.AllColumns.Add(this.clmPort);
            this.olvResults.AllColumns.Add(this.clmStatus);
            this.olvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmHost,
            this.clmPort,
            this.clmStatus});
            this.olvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvResults.View = View.Details;
            this.olvResults.ShowGroups = false;
            this.olvResults.GridLines = true;
            this.olvResults.FullRowSelect = true;

            // 
            // Columns
            // 
            this.clmName.AspectName = "Name";
            this.clmName.Text = "Name";
            this.clmName.Width = 200;

            this.clmHost.AspectName = "Hostname";
            this.clmHost.Text = "Hostname/IP";
            this.clmHost.Width = 200;

            this.clmPort.AspectName = "Port";
            this.clmPort.Text = "Port";
            this.clmPort.Width = 80;

            this.clmStatus.AspectName = "Status";
            this.clmStatus.Text = "Status";
            this.clmStatus.Width = 150;

            // 
            // prgBar
            // 
            this.prgBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgBar.Step = 1;
            
            this.Controls.Add(this.pnlMain);
            this.Text = "Connection Tester";
            this.TabText = "Connection Tester";
            this.ClientSize = new System.Drawing.Size(800, 600);
            
            ((System.ComponentModel.ISupportInitialize)(this.olvResults)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (_isTesting)
            {
                CancelTest();
            }
            else
            {
                await StartTest();
            }
        }

        private void CancelTest()
        {
            _cancellationTokenSource?.Cancel();
            _isTesting = false;
            btnStart.Text = "Start Test";
        }

        private async Task StartTest()
        {
            _isTesting = true;
            btnStart.Text = "Stop Test";
            olvResults.Items.Clear();
            
            var connections = GetAllConnections();
            prgBar.Maximum = connections.Count;
            prgBar.Value = 0;

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new System.Threading.CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // Populate initial list with "Pending" status
            var results = connections.Select(c => new ConnectionTestResult(c)).ToList();
            olvResults.SetObjects(results);

            try
            {
                 foreach (var result in results)
                 {
                     if (token.IsCancellationRequested) break;
                     
                     bool isOpen = await Task.Run(() => 
                     {
                         try
                         {
                             if (token.IsCancellationRequested) return false;
                             
                             int port = result.Connection.Port;
                             if (port == 0) port = result.Connection.GetDefaultPort();
                             
                             if (string.IsNullOrEmpty(result.Connection.Hostname)) return false;

                             return PortScanner.IsPortOpen(result.Connection.Hostname, port.ToString(CultureInfo.InvariantCulture));
                         }
                         catch
                         {
                             return false;
                         }
                     }, token);

                     if (token.IsCancellationRequested) break;

                     result.Status = isOpen ? "Open" : "Closed";
                     result.Port = result.Connection.Port == 0 ? result.Connection.GetDefaultPort() : result.Connection.Port;
                     
                     olvResults.RefreshObject(result);
                     prgBar.Value++;
                 }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("Connection Tester failed", ex);
            }
            finally
            {
                _isTesting = false;
                btnStart.Text = "Start Test";
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private static List<ConnectionInfo> GetAllConnections()
        {
            var list = new List<ConnectionInfo>();
            if (AppWindows.TreeForm?.ConnectionTree?.ConnectionTreeModel?.RootNodes == null) return list;

            foreach (var rootNode in AppWindows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>())
            {
                 Traverse(rootNode, list);
            }
            return list;
        }

        private static void Traverse(IConnectionNode node, List<ConnectionInfo> list)
        {
            if (node is ConnectionInfo c && !c.IsContainer)
            {
                if (!string.IsNullOrWhiteSpace(c.Hostname))
                {
                    list.Add(c);
                }
            }
            
            foreach (var child in node.Children)
            {
                Traverse(child, list);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
            base.Dispose(disposing);
        }

        public class ConnectionTestResult
        {
            public ConnectionInfo Connection { get; }
            public string Name => Connection.Name;
            public string Hostname => Connection.Hostname;
            public int Port { get; set; }
            public string Status { get; set; }

            public ConnectionTestResult(ConnectionInfo connection)
            {
                Connection = connection;
                Port = connection.Port == 0 ? connection.GetDefaultPort() : connection.Port;
                Status = "Pending";
            }
        }
    }
}
