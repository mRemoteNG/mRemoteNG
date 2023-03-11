using BrightIdeasSoftware;
using mRemoteNG.Connection;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    [SupportedOSPlatform("windows")]
    public class NameColumn : OLVColumn
    {
        public NameColumn(ImageGetterDelegate imageGetterDelegate)
        {
            AspectName = "Name";
            FillsFreeSpace = false;
            AspectGetter = item => ((ConnectionInfo)item).Name;
            ImageGetter = imageGetterDelegate;
            AutoCompleteEditor = false;
        }
    }
}