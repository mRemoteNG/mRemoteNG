using mRemoteNG.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Themable label to overide the winforms behavior of drawing the forecolor of disabled with a system color
    //This class repaints the control to avoid Disabled state mismatch of the theme
    [ToolboxBitmap(typeof(Label))]
    public class NGLabel : Label
    {
         
        private ThemeManager _themeManager;

        public NGLabel()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            // Use the Dialog_* colors since Labels generally have the same colors as panels/dialogs/windows/etc...
            if (_themeManager.ActiveTheme != null)
            {
                BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                FontOverrider.FontOverride(this);
                Invalidate();
            }
        }

  
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ThemingActive)
            {
                base.OnPaint(e);
                return;
            }

            // let's use the defaults - this looks terrible in my testing....
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            if (Enabled)
            {
                TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            else
            {
                var disabledtextLabel = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Foreground");
                TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, disabledtextLabel, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
                
        } 
    }
}
