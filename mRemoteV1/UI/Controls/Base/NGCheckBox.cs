using mRemoteNG.Themes;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Extended CheckBox class, the NGCheckBox onPaint completely repaint the control
    public class NGCheckBox : CheckBox
    {
        private ThemeManager _themeManager;

        public NGCheckBox()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

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
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
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
                if (args.Button != MouseButtons.Left) return;
                _mice = MouseState.DOWN;
                Invalidate();
            };
            MouseUp += (sender, args) =>
            {
                _mice = MouseState.OUT;

                Invalidate();
            };

            Invalidate();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if ( !_themeManager.ThemingActive)
            {
                base.OnPaint(e);
                return;
            }
            //Get the colors
            Color fore;
            Color glyph;
            Color checkBorder;
            if (_themeManager.ActiveTheme == null)
                return;
            var back = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Background");
            if (Enabled)
            {
                glyph = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Glyph");
                fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Text");
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_mice)
                {
                    case MouseState.HOVER:
                        checkBorder = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border_Hover");
                        break;
                    case MouseState.DOWN:
                        checkBorder = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border_Pressed");
                        break;
                    default:
                        checkBorder = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border");
                        break;
                }
            }
            else
            {
                
                fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Text_Disabled");
                glyph = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Glyph_Disabled");
                checkBorder = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border_Disabled");
            } 

            e.Graphics.Clear(Parent.BackColor);

            using (var p = new Pen(checkBorder))
            {
                var boxRect = new Rectangle(0, Height / 2 - 7, 11, 11);
                e.Graphics.FillRectangle(new SolidBrush(back), boxRect);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            if (Checked)
            {
                e.Graphics.DrawString("\u2714", new Font(Font.FontFamily, 7f), new SolidBrush(glyph), -1, 1);
            }

            var textRect = new Rectangle(16, 0, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, Parent.BackColor, TextFormatFlags.PathEllipsis);

        
        }

    }
}

