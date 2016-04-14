using mRemoteNG.App;
using mRemoteNG.Messages;
using mRemoteNG.My;
using mRemoteNG.Tools;
using System;
using System.ComponentModel;


namespace mRemoteNG.Connection
{
	public class PuttySessionInfo : ConnectionInfo, IComponent
	{
		[Command(),LocalizedAttributes.LocalizedDisplayName("strPuttySessionSettings")]
        public void SessionSettings()
		{
			try
			{
				PuttyProcessController puttyProcess = new PuttyProcessController();
				if (!puttyProcess.Start())
				{
					return ;
				}
				if (puttyProcess.SelectListBoxItem(PuttySession))
				{
					puttyProcess.ClickButton("&Load");
				}
				puttyProcess.SetControlText("Button", "&Cancel", "&Close");
				puttyProcess.SetControlVisible("Button", "&Open", false);
				puttyProcess.WaitForExit();
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorCouldNotLaunchPutty + Environment.NewLine + ex.Message, false);
			}
	    }
				
        #region Properties
		[Browsable(false)]
        public Root.PuttySessions.Info RootPuttySessionsInfo { get; set; }
				
		[ReadOnly(true)]
        public override string PuttySession { get; set; }
				
		[ReadOnly(true)]
        public override string Name { get; set; }
				
		[ReadOnly(true), Browsable(false)]
        public override string Description { get; set; }
				
        [ReadOnly(true), Browsable(false)]
        public override string Icon
		{
			get { return "PuTTY"; }
			set { }
		}
				
        [ReadOnly(true), Browsable(false)]
        public override string Panel
		{
			get { return RootPuttySessionsInfo.Panel; }
			set { }
		}
				
		[ReadOnly(true)]
        public override string Hostname { get; set; }
				
		[ReadOnly(true)]
        public override string Username { get; set; }
				
		[ReadOnly(true), Browsable(false)]
        public override string Password { get; set; }
				
		[ReadOnly(true)]
        public override Protocol.ProtocolType Protocol { get; set; }
				
		[ReadOnly(true)]
        public override int Port { get; set; }
				
		[ReadOnly(true), Browsable(false)]
        public override string PreExtApp { get; set; }
				
		[ReadOnly(true), Browsable(false)]
        public override string PostExtApp { get; set; }
				
		[ReadOnly(true), Browsable(false)]
        public override string MacAddress { get; set; }
				
		[ReadOnly(true), Browsable(false)]
        public override string UserField { get; set; }
        #endregion
				
        #region IComponent
        [Browsable(false)]
        public ISite Site
		{
			get { return new PropertyGridCommandSite(this); }
			set { throw (new NotImplementedException()); }
		}
				
		public void Dispose()
		{
			if (Disposed != null)
				Disposed(this, EventArgs.Empty);
		}
				
		public event EventHandler Disposed;
        #endregion
	}
}