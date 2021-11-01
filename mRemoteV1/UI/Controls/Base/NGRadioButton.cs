using mRemoteNG.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    // total replace of RadioButton to avoid disabled state inconsistency on the themes
    // and glyph color inconsistency
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
           ThemeManager.getInstance().ThemeChanged += OnCreateControl; 
        }


        private enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }

        private MouseState _mice { get; set; }


        protected override void OnCreateControl()
        {
            base.OnCreateControl(); 
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            // Allows for Overlaying
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
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


        //This class is painted with the checkbox colors, the glyph color is used for the radio inside
        protected override void OnPaint(PaintEventArgs e)
        {
            if ( !_themeManager.ThemingActive)
            {
                base.OnPaint(e);
                return;
            }
            // Init 
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (_themeManager.ActiveTheme == null)
                return;

            var fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Text");
            var outline = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Border");
            var centerBack = _themeManager.ActiveTheme.ExtendedPalette.getColor("CheckBox_Background");
            Color center;

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

            var textRect = new Rectangle(16, Padding.Top, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, Parent.BackColor, TextFormatFlags.PathEllipsis);
 
            g.FillEllipse(new SolidBrush(centerBack), circle);
            g.FillEllipse(new SolidBrush(center), circle_small); 
            g.DrawEllipse(new Pen(outline), circle);
       
        }

    }
}
