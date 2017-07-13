using mRemoteNG.Themes;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    [ToolboxBitmap(typeof(Button))]
    [DefaultEvent("Click")]
    class NGButton : Button
    {
        private ThemeManager _themeManager ;

        public enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }

        public MouseState _mice { get; set; }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;
            _themeManager = ThemeManager.getInstance();
            _mice = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                _mice = MouseState.HOVER; 
                Invalidate();
            };
            MouseLeave += (sender, args) =>
            {
                _mice = MouseState.OUT; 
                Invalidate();
            };
            MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    _mice = MouseState.DOWN; 
                    Invalidate();
                }
            };
            MouseUp += (sender, args) =>
            {
                _mice = MouseState.OUT;

                Invalidate();
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (DesignMode) return;
            Color back;
            Color fore;
            Color border;
            if (Enabled)
            {

                switch (_mice)
                {
                    case  MouseState.HOVER:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Border");
                        break;
                    case MouseState.DOWN:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Pressed_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Pressed_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Pressed_Border");
                        break;
                    default:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Border");
                        break;
                } 
            }
            else
            {
                back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Disabled_Background"); 
                fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Disabled_Foreground");
                border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Disabled_Border");
            }
            e.Graphics.FillRectangle(new SolidBrush(back), e.ClipRectangle);
            e.Graphics.DrawRectangle(new Pen(border, 1), 0, 0, base.Width - 1, base.Height - 1);
            TextRenderer.DrawText(e.Graphics, this.Text, Font, ClientRectangle, fore, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
