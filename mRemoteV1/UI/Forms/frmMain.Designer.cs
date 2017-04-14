using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmMain : System.Windows.Forms.Form
	{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlDock = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mMenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenView = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mMenSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsContainer = new System.Windows.Forms.ToolStripContainer();
            this.tmrAutoSave = new System.Windows.Forms.Timer(this.components);
            this._externalToolsToolStrip = new mRemoteNG.UI.Controls.ExternalToolsToolStrip();
            this._quickConnectToolStrip = new mRemoteNG.UI.Controls.QuickConnectToolStrip();
            this.mainFileMenu1 = new mRemoteNG.UI.Menu.MainFileMenu();
            this.viewMenu1 = new mRemoteNG.UI.Menu.ViewMenu();
            this.toolsMenu1 = new mRemoteNG.UI.Menu.ToolsMenu();
            this.helpMenu1 = new mRemoteNG.UI.Menu.HelpMenu();
            this.msMain.SuspendLayout();
            this.tsContainer.ContentPanel.SuspendLayout();
            this.tsContainer.TopToolStripPanel.SuspendLayout();
            this.tsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDock
            // 
            this.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDock.DockBackColor = System.Drawing.SystemColors.Control;
            this.pnlDock.DockLeftPortion = 230D;
            this.pnlDock.DockRightPortion = 230D;
            this.pnlDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.pnlDock.Location = new System.Drawing.Point(0, 0);
            this.pnlDock.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDock.Name = "pnlDock";
            this.pnlDock.Size = new System.Drawing.Size(1288, 594);
            this.pnlDock.TabIndex = 13;
            this.pnlDock.ActiveDocumentChanged += new System.EventHandler(this.pnlDock_ActiveDocumentChanged);
            // 
            // msMain
            // 
            this.msMain.GripMargin = new System.Windows.Forms.Padding(0);
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainFileMenu1,
            this.viewMenu1,
            this.toolsMenu1,
            this.helpMenu1});
            this.msMain.Location = new System.Drawing.Point(3, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.msMain.Size = new System.Drawing.Size(268, 24);
            this.msMain.Stretch = false;
            this.msMain.TabIndex = 16;
            this.msMain.Text = "Main Toolbar";
            // 
            // mMenFile
            // 
            this.mMenFile.Name = "mMenFile";
            this.mMenFile.Size = new System.Drawing.Size(32, 19);
            // 
            // mMenView
            // 
            this.mMenView.Name = "mMenView";
            this.mMenView.Size = new System.Drawing.Size(32, 19);
            // 
            // mMenTools
            // 
            this.mMenTools.Name = "mMenTools";
            this.mMenTools.Size = new System.Drawing.Size(32, 19);
            // 
            // mMenInfo
            // 
            this.mMenInfo.Name = "mMenInfo";
            this.mMenInfo.Size = new System.Drawing.Size(32, 19);
            // 
            // mMenSep3
            // 
            this.mMenSep3.Name = "mMenSep3";
            this.mMenSep3.Size = new System.Drawing.Size(211, 6);
            // 
            // tsContainer
            // 
            // 
            // tsContainer.ContentPanel
            // 
            this.tsContainer.ContentPanel.Controls.Add(this.pnlDock);
            this.tsContainer.ContentPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tsContainer.ContentPanel.Size = new System.Drawing.Size(1288, 594);
            this.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsContainer.Location = new System.Drawing.Point(0, 0);
            this.tsContainer.Margin = new System.Windows.Forms.Padding(4);
            this.tsContainer.Name = "tsContainer";
            this.tsContainer.Size = new System.Drawing.Size(1288, 644);
            this.tsContainer.TabIndex = 17;
            this.tsContainer.Text = "ToolStripContainer1";
            // 
            // tsContainer.TopToolStripPanel
            // 
            this.tsContainer.TopToolStripPanel.Controls.Add(this._externalToolsToolStrip);
            this.tsContainer.TopToolStripPanel.Controls.Add(this._quickConnectToolStrip);
            this.tsContainer.TopToolStripPanel.Controls.Add(this.msMain);
            // 
            // tmrAutoSave
            // 
            this.tmrAutoSave.Interval = 10000;
            this.tmrAutoSave.Tick += new System.EventHandler(this.tmrAutoSave_Tick);
            // 
            // _externalToolsToolStrip
            // 
            this._externalToolsToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this._externalToolsToolStrip.ForeColor = System.Drawing.SystemColors.ControlText;
            this._externalToolsToolStrip.Location = new System.Drawing.Point(271, 0);
            this._externalToolsToolStrip.MaximumSize = new System.Drawing.Size(0, 25);
            this._externalToolsToolStrip.Name = "_externalToolsToolStrip";
            this._externalToolsToolStrip.Size = new System.Drawing.Size(111, 25);
            this._externalToolsToolStrip.TabIndex = 17;
            // 
            // _quickConnectToolStrip
            // 
            this._quickConnectToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this._quickConnectToolStrip.ForeColor = System.Drawing.SystemColors.ControlText;
            this._quickConnectToolStrip.Location = new System.Drawing.Point(3, 25);
            this._quickConnectToolStrip.MaximumSize = new System.Drawing.Size(0, 25);
            this._quickConnectToolStrip.Name = "_quickConnectToolStrip";
            this._quickConnectToolStrip.Size = new System.Drawing.Size(364, 25);
            this._quickConnectToolStrip.TabIndex = 18;
            // 
            // mainFileMenu1
            // 
            this.mainFileMenu1.ConnectionInitiator = null;
            this.mainFileMenu1.Name = "mMenFile";
            this.mainFileMenu1.Size = new System.Drawing.Size(37, 20);
            this.mainFileMenu1.Text = "&File";
            this.mainFileMenu1.TreeWindow = null;
            this.mainFileMenu1.DropDownOpening += new System.EventHandler(this.mainFileMenu1_DropDownOpening);
            // 
            // viewMenu1
            // 
            this.viewMenu1.FullscreenHandler = null;
            this.viewMenu1.MainForm = null;
            this.viewMenu1.Name = "mMenView";
            this.viewMenu1.Size = new System.Drawing.Size(44, 20);
            this.viewMenu1.Text = "&View";
            this.viewMenu1.TsExternalTools = null;
            this.viewMenu1.TsQuickConnect = null;
            this.viewMenu1.DropDownOpening += new System.EventHandler(this.ViewMenu_Opening);
            // 
            // toolsMenu1
            // 
            this.toolsMenu1.CredentialManager = null;
            this.toolsMenu1.MainForm = null;
            this.toolsMenu1.Name = "mMenTools";
            this.toolsMenu1.Size = new System.Drawing.Size(47, 20);
            this.toolsMenu1.Text = "&Tools";
            // 
            // helpMenu1
            // 
            this.helpMenu1.Name = "mMenInfo";
            this.helpMenu1.Size = new System.Drawing.Size(44, 20);
            this.helpMenu1.Text = "&Help";
            this.helpMenu1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 644);
            this.Controls.Add(this.tsContainer);
            this.Icon = global::mRemoteNG.Resources.mRemote_Icon;
            this.MainMenuStrip = this.msMain;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMain";
            this.Opacity = 0D;
            this.Text = "mRemoteNG";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResizeBegin += new System.EventHandler(this.frmMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsContainer.ContentPanel.ResumeLayout(false);
            this.tsContainer.TopToolStripPanel.ResumeLayout(false);
            this.tsContainer.TopToolStripPanel.PerformLayout();
            this.tsContainer.ResumeLayout(false);
            this.tsContainer.PerformLayout();
            this.ResumeLayout(false);

		}
		internal WeifenLuo.WinFormsUI.Docking.DockPanel pnlDock;
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripMenuItem mMenFile;
		internal System.Windows.Forms.ToolStripMenuItem mMenView;
		internal System.Windows.Forms.ToolStripMenuItem mMenTools;
		internal System.Windows.Forms.ToolStripMenuItem mMenInfo;
		internal System.Windows.Forms.ToolStripContainer tsContainer;
		internal System.Windows.Forms.Timer tmrAutoSave;
		internal System.Windows.Forms.ToolStripSeparator mMenSep3;
        private System.ComponentModel.IContainer components;
        private Menu.MainFileMenu mainFileMenu1;
        private Menu.ViewMenu viewMenu1;
        private Menu.ToolsMenu toolsMenu1;
        private Menu.HelpMenu helpMenu1;
        internal mRemoteNG.UI.Controls.QuickConnectToolStrip _quickConnectToolStrip;
        internal mRemoteNG.UI.Controls.ExternalToolsToolStrip _externalToolsToolStrip;
    }
}
