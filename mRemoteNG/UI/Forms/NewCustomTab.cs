#region Usings
using System.Runtime.Versioning;
using mRemoteNG.Resources.Language;
using WeifenLuo.WinFormsUI.Docking;
#endregion

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    internal sealed class NewCustomTab : DockContent
    {
        public NewCustomTab()
        {
            HideOnClose = true;
            ShowHint = DockState.DockBottomAutoHide;
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Monitor_16x);
            Text = Language.Screenshots;
            TabText = Language.Screenshots;
        }
    }
}
