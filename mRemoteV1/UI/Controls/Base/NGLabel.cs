using mRemoteNG.Themes;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Themable label to overide the winforms behavior of drawing the forecolor of disabled with a system color
    [ToolboxBitmap(typeof(Label))]
    public class NGLabel : Label
    {
         
        private ThemeManager _themeManager;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;
            _themeManager = ThemeManager.getInstance();
        }

  
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DesignMode) return;
            if (Enabled)
            {
                TextRenderer.DrawText(e.Graphics, this.Text, Font, ClientRectangle, ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            else
            {
                Color disabledtextLabel = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Foreground");
                TextRenderer.DrawText(e.Graphics, this.Text, Font, ClientRectangle, disabledtextLabel, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
                
        }

 
 

         

    }
}
