using mRemoteNG.Themes;
using System;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //This class is only minimally themed as textboxes onPaint are hard to theme (system wm paint control most of the drawing process
    //There are some glitches on the initial draw of some controls
    public class NGTextBox : TextBox
    {
        private ThemeManager _themeManager; 

        public  NGTextBox()
        { 
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl(); 
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            if (_themeManager.ActiveTheme != null)
            {
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                Invalidate();
            }
        }
         


        protected override void OnEnabledChanged(EventArgs e)
        {
            _themeManager = ThemeManager.getInstance();
            if (_themeManager.ThemingActive)
            {
                _themeManager = ThemeManager.getInstance();
                if(_themeManager.ThemingActive)
                { 
                    if (Enabled)
                    {
                        ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                        BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                    }
                    else
                    {
                        BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Background");
                    }
                }
            }                
            base.OnEnabledChanged(e);
            Invalidate();
        }

       
    }
}
