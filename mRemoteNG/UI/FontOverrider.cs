using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI
{
    [SupportedOSPlatform("windows")]
    public class FontOverrider
    {
        public static void FontOverride(Control ctlParent)
        {
            // Override the font of all controls in a container with the default font based on the OS version
            foreach (Control tempLoopVarCtlChild in ctlParent.Controls)
            {
                var ctlChild = tempLoopVarCtlChild;
                ctlChild.Font = new Font(SystemFonts.MessageBoxFont.Name, ctlChild.Font.Size, ctlChild.Font.Style,
                                         ctlChild.Font.Unit, ctlChild.Font.GdiCharSet);
                if (ctlChild.Controls.Count > 0)
                {
                    FontOverride(ctlChild);
                }
            }
        }
    }
}