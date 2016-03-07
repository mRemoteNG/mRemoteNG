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
using mRemoteNG.App;


namespace mRemoteNG.Connection
{
	public partial class InterfaceControl
    {
        #region Public Properties
        private Connection.Protocol.Base _Protocol;
        public Connection.Protocol.Base Protocol
		{
			get { return this._Protocol; }
			set { this._Protocol = value; }
		}
			
		private Info _Info;
        public Info Info
		{
			get { return this._Info; }
			set { this._Info = value; }
		}
        #endregion
			
        #region Methods
		public InterfaceControl(Control Parent, Connection.Protocol.Base Protocol, Connection.Info Info)
		{
			try
			{
				this._Protocol = Protocol;
				this._Info = Info;
                this.Parent = Parent;
                this.Location = new Point(0, 0);
                this.Size = this.Parent.Size;
                this.Anchor = (System.Windows.Forms.AnchorStyles)(AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
				InitializeComponent();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t create new InterfaceControl" + Constants.vbNewLine + ex.Message);
			}
		}
        #endregion
	}
}
