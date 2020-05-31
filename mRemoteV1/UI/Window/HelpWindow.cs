using CefSharp;
using mRemoteNG.Connection.Protocol.Http;
using System;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Window
{
    public partial class HelpWindow : BaseWindow
    {
        public HelpWindow()
        {
            WindowType = WindowType.Help;
            DockPnl = new DockContent();
            InitializeComponent();
        }

        private void HelpWindow_Load(object sender, EventArgs e)
        {
            cefBrwoser.RequestHandler = new RequestHandler();
            cefBrwoser.Load($@"{Cef.CefCommitHash}://help/");
        }
    }
}