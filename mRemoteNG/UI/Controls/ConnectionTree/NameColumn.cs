using BrightIdeasSoftware;
using mRemoteNG.Connection;
using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;
using System.Runtime.Versioning;
using System.Linq;
using mRemoteNG.Container;
using mRemoteNG.Tree;

namespace mRemoteNG.UI.Controls.ConnectionTree
{
    [SupportedOSPlatform("windows")]
    public class NameColumn : OLVColumn
    {
        public NameColumn(ImageGetterDelegate imageGetterDelegate)
        {
            Text = Language.Name;
            AspectName = "Name";
            FillsFreeSpace = false;
            AspectGetter = item =>
            {
                var ci = (ConnectionInfo)item;
                string name = ConnectionNameFormatter.FormatName(ci);
                if (ci is ContainerInfo container)
                {
                    int count = container.GetRecursiveChildList()
                        .Count(c => c.GetTreeNodeType() == TreeNodeType.Connection || 
                                    c.GetTreeNodeType() == TreeNodeType.PuttySession);
                    
                    if (count > 0)
                        return $"{name} ({count})";
                }
                return name;
            };
            ImageGetter = imageGetterDelegate;
            AutoCompleteEditor = false;
        }
    }
}
