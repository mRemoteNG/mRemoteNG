using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Themable label to overide the winforms behavior of drawing the forecolor of disabled with a system color
    class NGLabel : Label
    {

        private bool _Enabled = false;
        private ThemeManager _themeManager = ThemeManager.getInstance();


        protected override void OnPaint(PaintEventArgs e)
        {
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
