

using mRemoteNG.My;

namespace mRemoteNG.UI.Window
{
	public partial class AnnouncementWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.WebBrowser webBrowser;
				
		private void InitializeComponent()
		{
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.Load += new System.EventHandler(Announcement_Load);
			this.SuspendLayout();
			//
			//webBrowser
			//
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(0, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new System.Drawing.Size(549, 474);
			this.webBrowser.TabIndex = 0;
			//
			//Announcement
			//
			this.ClientSize = new System.Drawing.Size(549, 474);
			this.Controls.Add(this.webBrowser);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.Icon = Resources.News_Icon;
			this.Name = "Announcement";
			this.TabText = "Announcement";
			this.Text = "Announcement";
			this.ResumeLayout(false);
					
		}
        #endregion
	}
}
