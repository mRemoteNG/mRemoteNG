using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI.Controls
{
    public class NameColumn : OLVColumn
    {
        public NameColumn()
        {
            AspectName = "Name";
            FillsFreeSpace = true;
            IsButton = true;
            AspectGetter = item => ((ConnectionInfo) item).Name;
            ImageGetter = ConnectionImageGetter;
        }

        private static object ConnectionImageGetter(object rowObject)
        {
            if (rowObject is RootPuttySessionsNodeInfo) return "PuttySessions";
            if (rowObject is RootNodeInfo) return "Root";
            if (rowObject is ContainerInfo) return "Folder";
            var connection = rowObject as ConnectionInfo;
            if (connection == null) return "";
            return connection.OpenConnections.Count > 0 ? "Play" : "Pause";
        }
    }
}