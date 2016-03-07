// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public class Help : Base
			{
				
#region Form Init
				internal System.Windows.Forms.TreeView tvIndex;
				internal System.Windows.Forms.ImageList imgListHelp;
				private System.ComponentModel.Container components = null;
				internal System.Windows.Forms.SplitContainer pnlSplitter;
				internal System.Windows.Forms.Label lblDocName;
				internal System.Windows.Forms.WebBrowser wbHelp;
				
				private void InitializeComponent()
				{
					this.components = new System.ComponentModel.Container();
					this.Load += new System.EventHandler(Help_Load);
					this.Shown += new System.EventHandler(Help_Shown);
					System.Windows.Forms.TreeNode TreeNode1 = new System.Windows.Forms.TreeNode("Introduction");
					System.Windows.Forms.TreeNode TreeNode2 = new System.Windows.Forms.TreeNode("Prerequisites");
					System.Windows.Forms.TreeNode TreeNode3 = new System.Windows.Forms.TreeNode("Installation");
					System.Windows.Forms.TreeNode TreeNode4 = new System.Windows.Forms.TreeNode("Configuration");
					System.Windows.Forms.TreeNode TreeNode5 = new System.Windows.Forms.TreeNode("SQL Configuration");
					System.Windows.Forms.TreeNode TreeNode6 = new System.Windows.Forms.TreeNode("Command-Line Switches");
					System.Windows.Forms.TreeNode TreeNode7 = new System.Windows.Forms.TreeNode("Getting Started", new System.Windows.Forms.TreeNode[] {TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6});
					System.Windows.Forms.TreeNode TreeNode8 = new System.Windows.Forms.TreeNode("Main Menu");
					System.Windows.Forms.TreeNode TreeNode9 = new System.Windows.Forms.TreeNode("Connections");
					System.Windows.Forms.TreeNode TreeNode10 = new System.Windows.Forms.TreeNode("Config");
					System.Windows.Forms.TreeNode TreeNode11 = new System.Windows.Forms.TreeNode("Errors and Infos");
					System.Windows.Forms.TreeNode TreeNode12 = new System.Windows.Forms.TreeNode("Save As / Export");
					System.Windows.Forms.TreeNode TreeNode13 = new System.Windows.Forms.TreeNode("Sessions");
					System.Windows.Forms.TreeNode TreeNode14 = new System.Windows.Forms.TreeNode("Screenshot Manager");
					System.Windows.Forms.TreeNode TreeNode15 = new System.Windows.Forms.TreeNode("Connection");
					System.Windows.Forms.TreeNode TreeNode16 = new System.Windows.Forms.TreeNode("Options");
					System.Windows.Forms.TreeNode TreeNode17 = new System.Windows.Forms.TreeNode("Update");
					System.Windows.Forms.TreeNode TreeNode18 = new System.Windows.Forms.TreeNode("SSH File Transfer");
					System.Windows.Forms.TreeNode TreeNode19 = new System.Windows.Forms.TreeNode("Quick Connect");
					System.Windows.Forms.TreeNode TreeNode20 = new System.Windows.Forms.TreeNode("Import From Active Directory");
					System.Windows.Forms.TreeNode TreeNode21 = new System.Windows.Forms.TreeNode("External Applications");
					System.Windows.Forms.TreeNode TreeNode22 = new System.Windows.Forms.TreeNode("Port Scan");
					System.Windows.Forms.TreeNode TreeNode23 = new System.Windows.Forms.TreeNode("User Interface", new System.Windows.Forms.TreeNode[] {TreeNode8, TreeNode9, TreeNode10, TreeNode11, TreeNode12, TreeNode13, TreeNode14, TreeNode15, TreeNode16, TreeNode17, TreeNode18, TreeNode19, TreeNode20, TreeNode21, TreeNode22});
					System.Windows.Forms.TreeNode TreeNode24 = new System.Windows.Forms.TreeNode("Quick Reference");
					System.Windows.Forms.TreeNode TreeNode25 = new System.Windows.Forms.TreeNode("Help", new System.Windows.Forms.TreeNode[] {TreeNode1, TreeNode7, TreeNode23, TreeNode24});
					this.wbHelp = new System.Windows.Forms.WebBrowser();
					this.wbHelp.DocumentTitleChanged += new System.EventHandler(this.wbHelp_DocumentTitleChanged);
					this.tvIndex = new System.Windows.Forms.TreeView();
					this.tvIndex.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvIndex_NodeMouseClick);
					this.tvIndex.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvIndex_AfterSelect);
					this.imgListHelp = new System.Windows.Forms.ImageList(this.components);
					this.pnlSplitter = new System.Windows.Forms.SplitContainer();
					this.lblDocName = new System.Windows.Forms.Label();
					this.pnlSplitter.Panel1.SuspendLayout();
					this.pnlSplitter.Panel2.SuspendLayout();
					this.pnlSplitter.SuspendLayout();
					this.SuspendLayout();
					//
					//wbHelp
					//
					this.wbHelp.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right);
					this.wbHelp.Location = new System.Drawing.Point(1, 36);
					this.wbHelp.MinimumSize = new System.Drawing.Size(20, 20);
					this.wbHelp.Name = "wbHelp";
					this.wbHelp.ScriptErrorsSuppressed = true;
					this.wbHelp.Size = new System.Drawing.Size(327, 286);
					this.wbHelp.TabIndex = 1;
					//
					//tvIndex
					//
					this.tvIndex.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right);
					this.tvIndex.BorderStyle = System.Windows.Forms.BorderStyle.None;
					this.tvIndex.HideSelection = false;
					this.tvIndex.Location = new System.Drawing.Point(1, 1);
					this.tvIndex.Name = "tvIndex";
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
					TreeNode13.Name = "Node12";
					TreeNode13.Tag = "Sessions";
					TreeNode13.Text = "Sessions";
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
					this.tvIndex.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {TreeNode25});
					this.tvIndex.ShowRootLines = false;
					this.tvIndex.Size = new System.Drawing.Size(207, 321);
					this.tvIndex.TabIndex = 0;
					//
					//imgListHelp
					//
					this.imgListHelp.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
					this.imgListHelp.ImageSize = new System.Drawing.Size(16, 16);
					this.imgListHelp.TransparentColor = System.Drawing.Color.Transparent;
					//
					//pnlSplitter
					//
					this.pnlSplitter.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right);
					this.pnlSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
					this.pnlSplitter.Location = new System.Drawing.Point(0, 0);
					this.pnlSplitter.Name = "pnlSplitter";
					//
					//pnlSplitter.Panel1
					//
					this.pnlSplitter.Panel1.Controls.Add(this.tvIndex);
					//
					//pnlSplitter.Panel2
					//
					this.pnlSplitter.Panel2.Controls.Add(this.lblDocName);
					this.pnlSplitter.Panel2.Controls.Add(this.wbHelp);
					this.pnlSplitter.Size = new System.Drawing.Size(542, 323);
					this.pnlSplitter.SplitterDistance = 209;
					this.pnlSplitter.TabIndex = 2;
					//
					//lblDocName
					//
					this.lblDocName.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right);
					this.lblDocName.BackColor = System.Drawing.Color.DimGray;
					this.lblDocName.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (12.0F), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
					this.lblDocName.ForeColor = System.Drawing.Color.White;
					this.lblDocName.Location = new System.Drawing.Point(1, 1);
					this.lblDocName.Name = "lblDocName";
					this.lblDocName.Size = new System.Drawing.Size(327, 35);
					this.lblDocName.TabIndex = 2;
					this.lblDocName.Text = "Introduction";
					this.lblDocName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
					//
					//Help
					//
					this.ClientSize = new System.Drawing.Size(542, 323);
					this.Controls.Add(this.pnlSplitter);
					this.Icon = My.Resources.Help_Icon;
					this.Name = "Help";
					this.TabText = "Help";
					this.Text = "Help";
					this.pnlSplitter.Panel1.ResumeLayout(false);
					this.pnlSplitter.Panel2.ResumeLayout(false);
					this.pnlSplitter.ResumeLayout(false);
					this.ResumeLayout(false);
					
				}
#endregion
				
#region Public Methods
				public Help(DockContent Panel)
				{
					this.WindowType = Type.Help;
					this.DockPnl = Panel;
					this.InitializeComponent();
					
					this.FillImageList();
					this.tvIndex.ImageList = this.imgListHelp;
					this.SetImages(this.tvIndex.Nodes[0]);
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
				
				private void tvIndex_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
				{
					this.tvIndex.SelectedNode = e.Node;
				}
				
				private void tvIndex_AfterSelect(System.Object sender, System.Windows.Forms.TreeViewEventArgs e)
				{
					if (e.Node.Tag != "")
					{
						this.wbHelp.Navigate((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\Help\\" + System.Convert.ToString(e.Node.Tag) +".htm");
					}
				}
				
				private void wbHelp_DocumentTitleChanged(object sender, System.EventArgs e)
				{
					this.lblDocName.Text = this.wbHelp.DocumentTitle;
				}
				
				private void FillImageList()
				{
					this.imgListHelp.Images.Add("File", My.Resources.Page);
					this.imgListHelp.Images.Add("Folder", My.Resources.Folder);
					this.imgListHelp.Images.Add("Help", My.Resources.Help);
				}
				
				private void SetImages(TreeNode node)
				{
					node.ImageIndex = 2;
					node.SelectedImageIndex = 2;
					
					foreach (TreeNode n in node.Nodes)
					{
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
				}
#endregion
			}
}
