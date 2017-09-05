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
    //This class is only minimally themed as textboxes onPaint are hard to theme (system wm paint control most of the drawing process
    //There are some glitches on the initial draw of some controls
    public class NGTextBox : TextBox
    {
        private ThemeManager _themeManager; 

        public  NGTextBox() : base()
        { 
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                if (_themeManager.ThemingActive)
                {
                    ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                    Invalidate();
                }
            }
           
        }
         


        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                if(_themeManager.ThemingActive)
                { 
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
            }                
            base.OnEnabledChanged(e);
            Invalidate();
        }

       
    }
}
