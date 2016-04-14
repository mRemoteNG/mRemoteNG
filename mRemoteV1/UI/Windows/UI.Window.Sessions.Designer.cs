

namespace mRemoteNG.UI.Window
{
	public partial class Sessions
	{
        #region  Windows Form Designer generated code
		private System.ComponentModel.Container components = null;
		internal System.Windows.Forms.ToolStripMenuItem sessionMenuRetrieve;
		internal System.Windows.Forms.ToolStripMenuItem sessionMenuLogoff;
		internal System.Windows.Forms.ListView sessionList;
				
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Load += new System.EventHandler(Sessions_Load);
			System.Windows.Forms.ContextMenuStrip sessionMenu = default(System.Windows.Forms.ContextMenuStrip);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sessions));
			this.sessionMenuRetrieve = new System.Windows.Forms.ToolStripMenuItem();
			this.sessionMenuRetrieve.Click += new System.EventHandler(this.sessionMenuRetrieve_Click);
			this.sessionMenuLogoff = new System.Windows.Forms.ToolStripMenuItem();
			this.sessionMenuLogoff.Click += new System.EventHandler(this.sessionMenuLogoff_Click);
			this.sessionList = new System.Windows.Forms.ListView();
			this.sessionUsernameColumn = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.sessionActivityColumn = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.sessionTypeColumn = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			sessionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			sessionMenu.SuspendLayout();
			this.SuspendLayout();
			//
			//sessionMenu
			//
			sessionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.sessionMenuRetrieve, this.sessionMenuLogoff});
			sessionMenu.Name = "cMenSession";
			sessionMenu.Size = new System.Drawing.Size(153, 70);
			sessionMenu.Opening += this.menuSession_Opening;
			//
			//sessionMenuRetrieve
			//
			this.sessionMenuRetrieve.Image = My.Resources.Refresh;
			this.sessionMenuRetrieve.Name = "sessionMenuRetrieve";
			this.sessionMenuRetrieve.Size = new System.Drawing.Size(152, 22);
			this.sessionMenuRetrieve.Text = "Retrieve";
			//
			//sessionMenuLogoff
			//
			this.sessionMenuLogoff.Image = My.Resources.Session_LogOff;
			this.sessionMenuLogoff.Name = "sessionMenuLogoff";
			this.sessionMenuLogoff.Size = new System.Drawing.Size(152, 22);
			this.sessionMenuLogoff.Text = My.Language.strLogOff;
			//
			//sessionList
			//
			this.sessionList.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.sessionList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.sessionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.sessionUsernameColumn, this.sessionActivityColumn, this.sessionTypeColumn});
			this.sessionList.ContextMenuStrip = sessionMenu;
			this.sessionList.FullRowSelect = true;
			this.sessionList.GridLines = true;
			this.sessionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.sessionList.Location = new System.Drawing.Point(0, -1);
			this.sessionList.MultiSelect = false;
			this.sessionList.Name = "sessionList";
			this.sessionList.ShowGroups = false;
			this.sessionList.Size = new System.Drawing.Size(242, 174);
			this.sessionList.TabIndex = 0;
			this.sessionList.UseCompatibleStateImageBehavior = false;
			this.sessionList.View = System.Windows.Forms.View.Details;
			//
			//sessionUsernameColumn
			//
			this.sessionUsernameColumn.Text = My.Language.strColumnUsername;
			this.sessionUsernameColumn.Width = 80;
			//
			//sessionActivityColumn
			//
			this.sessionActivityColumn.Text = My.Language.strActivity;
			//
			//sessionTypeColumn
			//
			this.sessionTypeColumn.Text = My.Language.strType;
			this.sessionTypeColumn.Width = 80;
			//
			//Sessions
			//
			this.ClientSize = new System.Drawing.Size(242, 173);
			this.Controls.Add(this.sessionList);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (8.25F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.HideOnClose = true;
			this.Icon = (System.Drawing.Icon) (resources.GetObject("$this.Icon"));
			this.Name = "Sessions";
			this.TabText = My.Language.strMenuSessions;
			this.Text = "Sessions";
			sessionMenu.ResumeLayout(false);
			this.ResumeLayout(false);
					
		}
		internal System.Windows.Forms.ColumnHeader sessionUsernameColumn;
		internal System.Windows.Forms.ColumnHeader sessionActivityColumn;
		internal System.Windows.Forms.ColumnHeader sessionTypeColumn;
        #endregion
	}
}
