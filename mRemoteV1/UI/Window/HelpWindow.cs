using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public class HelpWindow : BaseWindow
    {
        #region Public Methods

        public HelpWindow(DockContent Panel)
        {
            WindowType = WindowType.Help;
            DockPnl = Panel;
            InitializeComponent();

            FillImageList();
            tvIndex.ImageList = imgListHelp;
            SetImages(tvIndex.Nodes[0]);
        }

        #endregion

        #region Form Init

        internal TreeView tvIndex;
        internal ImageList imgListHelp;
        private System.ComponentModel.Container components;
        internal SplitContainer pnlSplitter;
        internal Label lblDocName;
        internal WebBrowser wbHelp;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Load += Help_Load;
            Shown += Help_Shown;
            var TreeNode1 = new TreeNode("Introduction");
            var TreeNode2 = new TreeNode("Prerequisites");
            var TreeNode3 = new TreeNode("Installation");
            var TreeNode4 = new TreeNode("Configuration");
            var TreeNode5 = new TreeNode("SQL Configuration");
            var TreeNode6 = new TreeNode("Command-Line Switches");
            var TreeNode7 = new TreeNode("Getting Started",
                new[] {TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6});
            var TreeNode8 = new TreeNode("Main Menu");
            var TreeNode9 = new TreeNode("Connections");
            var TreeNode10 = new TreeNode("Config");
            var TreeNode11 = new TreeNode("Errors and Infos");
            var TreeNode12 = new TreeNode("Save As / Export");
            var TreeNode14 = new TreeNode("Screenshot Manager");
            var TreeNode15 = new TreeNode("Connection");
            var TreeNode16 = new TreeNode("Options");
            var TreeNode17 = new TreeNode("Update");
            var TreeNode18 = new TreeNode("SSH File Transfer");
            var TreeNode19 = new TreeNode("Quick Connect");
            var TreeNode20 = new TreeNode("Import From Active Directory");
            var TreeNode21 = new TreeNode("External Applications");
            var TreeNode22 = new TreeNode("Port Scan");
            var TreeNode23 = new TreeNode("User Interface",
                new[]
                {
                    TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode14, TreeNode15, TreeNode16,
                    TreeNode17, TreeNode18, TreeNode19, TreeNode20, TreeNode21, TreeNode22
                });
            var TreeNode24 = new TreeNode("Quick Reference");
            var TreeNode25 = new TreeNode("Help", new[] {TreeNode1, TreeNode7, TreeNode23, TreeNode24});
            wbHelp = new WebBrowser();
            wbHelp.DocumentTitleChanged += wbHelp_DocumentTitleChanged;
            tvIndex = new TreeView();
            tvIndex.NodeMouseClick += tvIndex_NodeMouseClick;
            tvIndex.AfterSelect += tvIndex_AfterSelect;
            imgListHelp = new ImageList(components);
            pnlSplitter = new SplitContainer();
            lblDocName = new Label();
            pnlSplitter.Panel1.SuspendLayout();
            pnlSplitter.Panel2.SuspendLayout();
            pnlSplitter.SuspendLayout();
            SuspendLayout();
            //
            //wbHelp
            //
            wbHelp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                            | AnchorStyles.Left
                            | AnchorStyles.Right;
            wbHelp.Location = new Point(1, 36);
            wbHelp.MinimumSize = new Size(20, 20);
            wbHelp.Name = "wbHelp";
            wbHelp.ScriptErrorsSuppressed = true;
            wbHelp.Size = new Size(327, 286);
            wbHelp.TabIndex = 1;
            //
            //tvIndex
            //
            tvIndex.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                             | AnchorStyles.Left
                             | AnchorStyles.Right;
            tvIndex.BorderStyle = BorderStyle.None;
            tvIndex.HideSelection = false;
            tvIndex.Location = new Point(1, 1);
            tvIndex.Name = "tvIndex";
            TreeNode1.Name = "Node0";
            TreeNode1.Tag = "Introduction";
            TreeNode1.Text = "Introduction";
            TreeNode2.Name = "Node0";
            TreeNode2.Tag = "Prerequisites";
            TreeNode2.Text = "Prerequisites";
            TreeNode3.Name = "Node3";
            TreeNode3.Tag = "Installation";
            TreeNode3.Text = "Installation";
            TreeNode4.Name = "Node4";
            TreeNode4.Tag = "Configuration";
            TreeNode4.Text = "Configuration";
            TreeNode5.Name = "Node0";
            TreeNode5.Tag = "ConfigurationSQL";
            TreeNode5.Text = "SQL Configuration";
            TreeNode6.Name = "Node5";
            TreeNode6.Tag = "CMDSwitches";
            TreeNode6.Text = "Command-Line Switches";
            TreeNode7.Name = "Node1";
            TreeNode7.Text = "Getting Started";
            TreeNode8.Name = "Node7";
            TreeNode8.Tag = "MainMenu";
            TreeNode8.Text = "Main Menu";
            TreeNode9.Name = "Node8";
            TreeNode9.Tag = "Connections";
            TreeNode9.Text = "Connections";
            TreeNode10.Name = "Node9";
            TreeNode10.Tag = "Config";
            TreeNode10.Text = "Config";
            TreeNode11.Name = "Node10";
            TreeNode11.Tag = "ErrorsAndInfos";
            TreeNode11.Text = "Errors and Infos";
            TreeNode12.Name = "Node11";
            TreeNode12.Tag = "SaveAsExport";
            TreeNode12.Text = "Save As / Export";
            TreeNode14.Name = "Node13";
            TreeNode14.Tag = "ScreenshotManager";
            TreeNode14.Text = "Screenshot Manager";
            TreeNode15.Name = "Node14";
            TreeNode15.Tag = "Connection";
            TreeNode15.Text = "Connection";
            TreeNode16.Name = "Node15";
            TreeNode16.Tag = "Options";
            TreeNode16.Text = "Options";
            TreeNode17.Name = "Node16";
            TreeNode17.Tag = "Update";
            TreeNode17.Text = "Update";
            TreeNode18.Name = "Node17";
            TreeNode18.Tag = "SSHFileTransfer";
            TreeNode18.Text = "SSH File Transfer";
            TreeNode19.Name = "Node18";
            TreeNode19.Tag = "QuickConnect";
            TreeNode19.Text = "Quick Connect";
            TreeNode20.Name = "Node19";
            TreeNode20.Tag = "ImportFromAD";
            TreeNode20.Text = "Import From Active Directory";
            TreeNode21.Name = "Node1";
            TreeNode21.Tag = "ExternalTools";
            TreeNode21.Text = "External Tools";
            TreeNode22.Name = "Node0";
            TreeNode22.Tag = "PortScan";
            TreeNode22.Text = "Port Scan";
            TreeNode23.Name = "Node6";
            TreeNode23.Text = "User Interface";
            TreeNode24.Name = "Node20";
            TreeNode24.Tag = "QuickReference";
            TreeNode24.Text = "Quick Reference";
            TreeNode25.Name = "Node0";
            TreeNode25.Text = "Help";
            TreeNode25.Tag = "Index";
            tvIndex.Nodes.AddRange(new[] {TreeNode25});
            tvIndex.ShowRootLines = false;
            tvIndex.Size = new Size(207, 321);
            tvIndex.TabIndex = 0;
            //
            //imgListHelp
            //
            imgListHelp.ColorDepth = ColorDepth.Depth32Bit;
            imgListHelp.ImageSize = new Size(16, 16);
            imgListHelp.TransparentColor = Color.Transparent;
            //
            //pnlSplitter
            //
            pnlSplitter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                 | AnchorStyles.Left
                                 | AnchorStyles.Right;
            pnlSplitter.FixedPanel = FixedPanel.Panel1;
            pnlSplitter.Location = new Point(0, 0);
            pnlSplitter.Name = "pnlSplitter";
            //
            //pnlSplitter.Panel1
            //
            pnlSplitter.Panel1.Controls.Add(tvIndex);
            //
            //pnlSplitter.Panel2
            //
            pnlSplitter.Panel2.Controls.Add(lblDocName);
            pnlSplitter.Panel2.Controls.Add(wbHelp);
            pnlSplitter.Size = new Size(542, 323);
            pnlSplitter.SplitterDistance = 209;
            pnlSplitter.TabIndex = 2;
            //
            //lblDocName
            //
            lblDocName.Anchor = AnchorStyles.Top | AnchorStyles.Left
                                | AnchorStyles.Right;
            lblDocName.BackColor = Color.DimGray;
            lblDocName.Font = new Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            lblDocName.ForeColor = Color.White;
            lblDocName.Location = new Point(1, 1);
            lblDocName.Name = "lblDocName";
            lblDocName.Size = new Size(327, 35);
            lblDocName.TabIndex = 2;
            lblDocName.Text = "Introduction";
            lblDocName.TextAlign = ContentAlignment.MiddleLeft;
            //
            //Help
            //
            ClientSize = new Size(542, 323);
            Controls.Add(pnlSplitter);
            Icon = Resources.Help_Icon;
            Name = "Help";
            TabText = "Help";
            Text = "Help";
            pnlSplitter.Panel1.ResumeLayout(false);
            pnlSplitter.Panel2.ResumeLayout(false);
            pnlSplitter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        #region Private Methods

        private void Help_Load(object sender, EventArgs e)
        {
            tvIndex.Nodes[0].ExpandAll();
            tvIndex.SelectedNode = tvIndex.Nodes[0].Nodes[0];
        }

        private void Help_Shown(object sender, EventArgs e)
        {
            // This can only be set once the WebBrowser control is shown, it will throw a COM exception otherwise.
            wbHelp.AllowWebBrowserDrop = false;
        }

        private void tvIndex_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvIndex.SelectedNode = e.Node;
        }

        private void tvIndex_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((string) e.Node.Tag != "") && (e.Node.Tag != null))
                wbHelp.Navigate(GeneralAppInfo.HomePath + "\\Help\\" + Convert.ToString(e.Node.Tag) + ".htm");
        }

        private void wbHelp_DocumentTitleChanged(object sender, EventArgs e)
        {
            lblDocName.Text = wbHelp.DocumentTitle;
        }

        private void FillImageList()
        {
            imgListHelp.Images.Add("File", Resources.Page);
            imgListHelp.Images.Add("Folder", Resources.Folder);
            imgListHelp.Images.Add("Help", Resources.Help);
        }

        private void SetImages(TreeNode node)
        {
            node.ImageIndex = 2;
            node.SelectedImageIndex = 2;

            foreach (TreeNode n in node.Nodes)
                if (n.Nodes.Count > 0)
                {
                    n.ImageIndex = 1;
                    n.SelectedImageIndex = 1;
                }
                else
                {
                    n.ImageIndex = 0;
                    n.SelectedImageIndex = 0;
                }
        }

        #endregion
    }
}