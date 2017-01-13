using BrightIdeasSoftware;
using mRemoteNG.Connection;


namespace mRemoteNG.UI.Controls
{
    public class NameColumn : OLVColumn
    {
        public NameColumn(ImageGetterDelegate imageGetterDelegate)
        {
            AspectName = "Name";
            FillsFreeSpace = true;
            IsButton = true;
            AspectGetter = item => ((ConnectionInfo) item).Name;
            ImageGetter = imageGetterDelegate;
        }
    }
}