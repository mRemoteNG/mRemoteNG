using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //This class is only minimally themed as textboxes onPaint are hard to theme (system controls most of the drawing process
    public class NGTextBox : TextBox
    {
        private ThemeManager _themeManager; 

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            }
           
        }
         


        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                if (base.Enabled)
                {
                    ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                }
                else
                {
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Background");
                }
            }                
            base.OnEnabledChanged(e);
            Invalidate();
        }

       
    }
}
