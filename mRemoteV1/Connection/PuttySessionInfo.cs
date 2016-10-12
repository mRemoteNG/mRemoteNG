using System;
using System.ComponentModel;
using mRemoteNG.App;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Root.PuttySessions;
using mRemoteNG.Tools;
using mRemoteNG.Tree;

namespace mRemoteNG.Connection
{
    public class PuttySessionInfo : ConnectionInfo, IComponent
    {
        [Command]
        [LocalizedAttributes.LocalizedDisplayNameAttribute("strPuttySessionSettings")]
        public void SessionSettings()
        {
            try
            {
                var puttyProcess = new PuttyProcessController();
                if (!puttyProcess.Start())
                    return;
                if (puttyProcess.SelectListBoxItem(PuttySession))
                    puttyProcess.ClickButton("&Load");
                puttyProcess.SetControlText("Button", "&Cancel", "&Close");
                puttyProcess.SetControlVisible("Button", "&Open", false);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    Language.strErrorCouldNotLaunchPutty + Environment.NewLine + ex.Message);
            }
        }

        public override TreeNodeType GetTreeNodeType()
        {
            return TreeNodeType.PuttySession;
        }

        #region Properties

        [Browsable(false)]
        public RootPuttySessionsNodeInfo RootRootPuttySessionsInfo { get; set; }

        [ReadOnly(true)]
        public override string PuttySession { get; set; }

        [ReadOnly(true)]
        public override string Name { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string Description { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string Icon
        {
            get { return "PuTTY"; }
            set { }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string Panel
        {
            get { return Parent?.Panel; }
            set { }
        }

        [ReadOnly(true)]
        public override string Hostname { get; set; }

        [ReadOnly(true)]
        public override string Username { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string Password { get; set; }

        [ReadOnly(true)]
        public override ProtocolType Protocol { get; set; }

        [ReadOnly(true)]
        public override int Port { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string PreExtApp { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string PostExtApp { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string MacAddress { get; set; }

        [ReadOnly(true)]
        [Browsable(false)]
        public override string UserField { get; set; }

        #endregion

        #region IComponent

        [Browsable(false)]
        public ISite Site
        {
            get { return new PropertyGridCommandSite(this); }
            set { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Disposed;

        #endregion
    }
}