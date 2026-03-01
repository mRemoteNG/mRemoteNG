using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    //Extended CheckBox class, the NGCheckBox onPaint completely repaint the control

    //
    // If this causes design issues in the future, may want to think about migrating to
    // CheckBoxRenderer:
    // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.checkboxrenderer?view=netframework-4.6
    //
    public class MrngCheckBox : CheckBox
    {
        private ThemeManager? _themeManager;
        private readonly Size _checkboxSize;
        private readonly int _checkboxYCoord;
        private readonly int _textXCoord;

        public MrngCheckBox()
        {
            InitializeComponent();
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
            DisplayProperties display = new();
            _checkboxSize = new Size(display.ScaleWidth(11), display.ScaleHeight(11));
            _checkboxYCoord = (display.ScaleHeight(Height) - _checkboxSize.Height) / 2 - display.ScaleHeight(5);
            _textXCoord = _checkboxSize.Width + display.ScaleWidth(2);
        }

        public enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }

#pragma warning disable CA1707 // Designer-generated code uses this name; renaming would break .Designer.cs files
        public MouseState _mice { get; set; }
#pragma warning restore CA1707


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


        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (_themeManager is null || !_themeManager.ActiveAndExtended)
            {
                base.OnPaint(pevent);
                return;
            }

            var palette = _themeManager.ActiveTheme.ExtendedPalette;
            if (palette is null)
            {
                base.OnPaint(pevent);
                return;
            }

            //Get the colors
            Color fore;
            Color glyph;
            Color checkBorder;

            Color back = palette.getColor("CheckBox_Background");
            if (Enabled)
            {
                glyph = palette.getColor("CheckBox_Glyph");
                fore = palette.getColor("CheckBox_Text");
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (_mice)
                {
                    case MouseState.HOVER:
                        checkBorder = palette.getColor("CheckBox_Border_Hover");
                        break;
                    case MouseState.DOWN:
                        checkBorder = palette.getColor("CheckBox_Border_Pressed");
                        break;
                    default:
                        checkBorder = palette.getColor("CheckBox_Border");
                        break;
                }
            }
            else
            {
                fore = palette.getColor("CheckBox_Text_Disabled");
                glyph = palette.getColor("CheckBox_Glyph_Disabled");
                checkBorder = palette.getColor("CheckBox_Border_Disabled");
            }

            Color parentBack = Parent?.BackColor ?? BackColor;
            pevent.Graphics.Clear(parentBack);

            using (Pen p = new(checkBorder))
            {
                Rectangle boxRect = new(0, _checkboxYCoord, _checkboxSize.Width, _checkboxSize.Height);
                pevent.Graphics.FillRectangle(new SolidBrush(back), boxRect);
                pevent.Graphics.DrawRectangle(p, boxRect);
            }

            if (Checked)
            {
                // | \uE001 | &#xE001; |  |  is the tick/check mark and it exists in Segoe UI Symbol at least...
                pevent.Graphics.DrawString("\uE001", new Font("Segoe UI Symbol", 7.75f), new SolidBrush(glyph), -4, 0);
            }

            Rectangle textRect = new(_textXCoord, 0, Width - 16, Height);
            TextRenderer.DrawText(pevent.Graphics, Text, Font, textRect, fore, parentBack,
                                  TextFormatFlags.PathEllipsis);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGCheckBox
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResumeLayout(false);
        }
    }
}