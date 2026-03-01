using mRemoteNG.Themes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    // total replace of RadioButton to avoid disabled state inconsistency on the themes
    // and glyph color inconsistency
    class MrngRadioButton : RadioButton
    {
        private ThemeManager? _themeManager;
        private readonly Rectangle _circle;
        private readonly Rectangle _circleSmall;
        private readonly int _textXCoord;

        // Constructor
        public MrngRadioButton()
        {
            DisplayProperties display = new();

            _circleSmall = new Rectangle(display.ScaleWidth(4), display.ScaleHeight(4), display.ScaleWidth(6),
                                         display.ScaleHeight(6));
            _circle = new Rectangle(display.ScaleWidth(1), display.ScaleHeight(1), display.ScaleWidth(12),
                                    display.ScaleHeight(12));
            _textXCoord = display.ScaleWidth(16);
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
            if (_themeManager is not { ActiveAndExtended: true })
            {
                base.OnPaint(e);
                return;
            }

            // Init
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var palette = _themeManager.ActiveTheme.ExtendedPalette;
            if (palette == null)
            {
                base.OnPaint(e);
                return;
            }

            Color fore = palette.getColor("CheckBox_Text");
            Color outline = palette.getColor("CheckBox_Border");
            Color centerBack = palette.getColor("CheckBox_Background");
            Color center;

            // Overlay Graphic
            Color parentBack = Parent?.BackColor ?? BackColor;
            e.Graphics.Clear(parentBack);
            if (Enabled)
            {
                if (Checked)
                {
                    center = palette.getColor("CheckBox_Glyph");
                }
                else
                {
                    center = Color.Transparent;
                    if (_mice == MouseState.HOVER)
                    {
                        outline = palette.getColor("CheckBox_Border_Hover");
                    }
                }
            }
            else
            {
                center = Color.Transparent;
                fore = palette.getColor("CheckBox_Text_Disabled");
            }

            Rectangle textRect = new(_textXCoord, Padding.Top, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fore, parentBack,
                                  TextFormatFlags.PathEllipsis);

            g.FillEllipse(new SolidBrush(centerBack), _circle);
            g.FillEllipse(new SolidBrush(center), _circleSmall);
            g.DrawEllipse(new Pen(outline), _circle);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NGRadioButton
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
        }
    }
}