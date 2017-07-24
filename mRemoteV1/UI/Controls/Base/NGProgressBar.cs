using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    class NGProgressBar : ProgressBar
    {
        private ThemeManager _themeManager;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                 SetStyle(ControlStyles.UserPaint, true);
                 SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        { 
            Color progressFill = _themeManager.ActiveTheme.ExtendedPalette.getColor("ProgressBar_Fill");
            Color back = _themeManager.ActiveTheme.ExtendedPalette.getColor("ProgressBar_Background");
            var doneProgress = (int)(e.ClipRectangle.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(new SolidBrush(progressFill), 0, 0, doneProgress, e.ClipRectangle.Height);
            e.Graphics.FillRectangle(new SolidBrush(back), doneProgress, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
        }
    }
}
