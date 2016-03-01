using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI
{
	namespace Window
	{
		public class Base : DockContent
		{

			#region "Public Properties"
			private UI.Window.Type _WindowType;
			public UI.Window.Type WindowType {
				get { return this._WindowType; }
				set { this._WindowType = value; }
			}

			private DockContent _DockPnl;
			public DockContent DockPnl {
				get { return this._DockPnl; }
				set { this._DockPnl = value; }
			}
			#endregion

			#region "Public Methods"
			public void SetFormText(string Text)
			{
				this.Text = Text;
				this.TabText = Text;
			}
			#endregion

			#region "Private Methods"
			private void Base_Load(System.Object sender, System.EventArgs e)
			{
				My.MyProject.Forms.frmMain.ShowHidePanelTabs();
			}

			private void Base_FormClosed(System.Object sender, System.Windows.Forms.FormClosedEventArgs e)
			{
				My.MyProject.Forms.frmMain.ShowHidePanelTabs(this);
			}
			public Base()
			{
				FormClosed += Base_FormClosed;
				Load += Base_Load;
			}
			#endregion
		}
	}
}
