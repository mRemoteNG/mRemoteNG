using mRemoteNG.Themes;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    // Repaint of a ProgressBar on a flat style
    internal class MrngProgressBar : ProgressBar
    {
        private ThemeManager? _themeManager;


        public MrngProgressBar()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_themeManager is null || !_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            var palette = _themeManager.ActiveTheme.ExtendedPalette;
            if (palette is null)
            {
                base.OnPaint(e);
                return;
            }

            Color progressFill = palette.getColor("ProgressBar_Fill");
            Color back = palette.getColor("ProgressBar_Background");
            int doneProgress = (int)(e.ClipRectangle.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(new SolidBrush(progressFill), 0, 0, doneProgress, e.ClipRectangle.Height);
            e.Graphics.FillRectangle(new SolidBrush(back), doneProgress, 0, e.ClipRectangle.Width,
                                     e.ClipRectangle.Height);
        }
    }
}