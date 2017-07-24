using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    class NGRadioButton : RadioButton
    {
        private ThemeManager _themeManager;
        private Rectangle circle;
        private Rectangle circle_small; 
        // Constructor
        public NGRadioButton()
        {
            // Init
            circle_small = new Rectangle(4, 4, 6, 6 );
            circle = new Rectangle(1, 1, 12, 12 );
 

            // Allows for Overlaying
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
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


        //This class is painted with the checkbox colors, the glyph color is used for the radio inside
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Tools.DesignModeTest.IsInDesignMode(this))
            {
                base.OnPaint(e);
                return;
            }
            // Init 
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Text");
            Color outline = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border");
            Color centerBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Background");
            Color center = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Glyph");

            // Overlay Graphic
            e.Graphics.Clear(Parent.BackColor);
            if (Enabled)
            {
                 
                if(Checked)
                { 
                    center = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Glyph");
                }
                else
                {
                    center = Color.Transparent;
                    if (_mice == MouseState.HOVER)
                    {
                        outline = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border_Hover");
                    }
                }
                
            }
            else
            {
                center = Color.Transparent;
                fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Text_Disabled");
            }

            Rectangle textRect = new Rectangle(16, Padding.Top, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, Parent.BackColor, TextFormatFlags.PathEllipsis);
 
            g.FillEllipse(new SolidBrush(centerBack), circle);
            g.FillEllipse(new SolidBrush(center), circle_small); 
            g.DrawEllipse(new Pen(outline), circle);
       
        }

    }
}
