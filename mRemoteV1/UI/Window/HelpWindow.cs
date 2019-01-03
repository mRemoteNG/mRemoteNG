using System;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
	public class HelpWindow : BaseWindow
	{
				
        #region Form Init

	    private TreeView tvIndex;
        private ImageList imgListHelp;
		private System.ComponentModel.Container components;
	    private SplitContainer pnlSplitter;
	    private Label lblDocName;
	    private WebBrowser wbHelp;
				
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // HelpWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HelpWindow";
            this.ResumeLayout(false);

		}
        #endregion
				
        #region Public Methods
		public HelpWindow()
		{
			WindowType = WindowType.Help;
			DockPnl = new DockContent();
			InitializeComponent();
					
			FillImageList();
			tvIndex.ImageList = imgListHelp;
			SetImages(tvIndex.Nodes[0]);
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
			if (!string.IsNullOrEmpty((string)e.Node.Tag))
			{
				wbHelp.Navigate(GeneralAppInfo.HomePath + "\\Help\\" + Convert.ToString(e.Node.Tag) +".htm");
			}
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
				
		private static void SetImages(TreeNode node)
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
