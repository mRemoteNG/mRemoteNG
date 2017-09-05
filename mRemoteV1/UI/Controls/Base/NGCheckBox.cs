using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Extended CheckBox class, the NGCheckBox onPaint completely repaint the control
    public class NGCheckBox : CheckBox
    {
        private ThemeManager _themeManager;

        public NGCheckBox() : base()
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
            if (Tools.DesignModeTest.IsInDesignMode(this)) return;
            _themeManager = ThemeManager.getInstance();
            if (_themeManager.ThemingActive)
            {
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

                Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (Tools.DesignModeTest.IsInDesignMode(this) || !_themeManager.ThemingActive)
            {
                base.OnPaint(e);
                return;
            }
            //Get the colors
            Color back;
            Color fore;
            Color glyph;
            Color checkBack;
            Color checkBorder;
 
            back = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Background");
            if (Enabled)
            {
                glyph = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Glyph");
                fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Text");
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

            using (Pen p = new Pen(checkBorder))
            {
                Rectangle boxRect = new Rectangle(0, Height / 2 - 7, 11, 11);
                e.Graphics.FillRectangle(new SolidBrush(back), boxRect);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            if (Checked)
            {
                e.Graphics.DrawString("ü", new Font("Wingdings", 9f), new SolidBrush(glyph), -1, 1);
            }

            Rectangle textRect = new Rectangle(16, 0, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, Parent.BackColor, TextFormatFlags.PathEllipsis);

        
        }

    }
}

