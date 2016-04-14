using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace mRemoteNG.Connection
{
	public partial class InterfaceControl
    {
        #region Private Variables
        private ProtocolBase _Protocol;
        private ConnectionInfo _Info;
        #endregion

        #region Public Properties
        public ProtocolBase Protocol
		{
			get { return this._Protocol; }
			set { this._Protocol = value; }
		}
		
        public ConnectionInfo Info
		{
			get { return this._Info; }
			set { this._Info = value; }
		}
        #endregion
			
        #region Constructors
		public InterfaceControl(Control Parent, ProtocolBase Protocol, ConnectionInfo Info)
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Couldn\'t create new InterfaceControl" + Environment.NewLine + ex.Message);
			}
		}
        #endregion
	}
}