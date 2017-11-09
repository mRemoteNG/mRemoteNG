using System.Windows.Forms;
using mRemoteNG.App.Info;

namespace mRemoteNG.UI
{
    public class DialogFactory
    {
        public static OpenFileDialog BuildLoadConnectionsDialog()
        {
            return new OpenFileDialog
            {
                Title = "",
                CheckFileExists = true,
                InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath,
                Filter = Language.strFiltermRemoteXML + @"|*.xml|" + Language.strFilterAll + @"|*.*"
            };
        }
    }
}