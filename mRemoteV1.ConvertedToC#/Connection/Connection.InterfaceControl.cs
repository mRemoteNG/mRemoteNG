using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Connection
{
	public partial class InterfaceControl
	{
		#region "Properties"
		private Connection.Protocol.Base _Protocol;
		public Connection.Protocol.Base Protocol {
			get { return this._Protocol; }
			set { this._Protocol = value; }
		}

		private Connection.Info _Info;
		public Connection.Info Info {
			get { return this._Info; }
			set { this._Info = value; }
		}
		#endregion

		#region "Methods"
		public InterfaceControl(Control Parent, Connection.Protocol.Base Protocol, Connection.Info Info)
		{
			try {
				this._Protocol = Protocol;
				this._Info = Info;
				this.Parent = Parent;
				this.Location = new Point(0, 0);
				this.Size = this.Parent.Size;
				this.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
				InitializeComponent();
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "Couldn't create new InterfaceControl" + Constants.vbNewLine + ex.Message);
			}
		}
		#endregion
	}
}


