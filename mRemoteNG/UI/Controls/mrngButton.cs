using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    [ToolboxBitmap(typeof(Button))]
    //Extended button class, the button onPaint completely repaint the control
    public class MrngButton : Button
    {
        private ThemeManager _themeManager;

        /// <summary>
        /// Store the mouse state, required for coloring the component according to the mouse state
        /// </summary>
        public enum MouseState
        {
            HOVER,
            DOWN,
            OUT
        }

        public MrngButton()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        public MouseState _mice { get; set; }

        /// <summary>
        /// Rewrite the function to allow for coloring the component depending on the mouse state
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
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


        /// <summary>
        /// Repaint the componente, the elements considered are the clipping rectangle, text and an icon
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            Color back;
            Color fore;
            Color border;
            if (Enabled)
            {
                switch (_mice)
                {
                    case MouseState.HOVER:
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
            e.Graphics.DrawRectangle(new Pen(border, 1), 0, 0, Width - 1, Height - 1);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            //Warning. the app doesnt use many images in buttons so this positions are kinda tailored just for the used by the app
            //not by general usage of iamges in buttons
            if (Image != null)
            {
                SizeF stringSize = e.Graphics.MeasureString(Text, Font);

                e.Graphics.DrawImageUnscaled(Image, Width / 2 - (int)stringSize.Width / 2 - Image.Width,
                                             Height / 2 - Image.Height / 2);
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, fore,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NGButton
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
        }
    }
}