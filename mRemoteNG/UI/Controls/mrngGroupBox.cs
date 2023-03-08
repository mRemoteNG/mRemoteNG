using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    //Groupbox is colored using the innerTab colors as the vstheme doesnt have explicit groupbox palettes (at least completes)
    //This clas completely repaints the control
    public class MrngGroupBox : GroupBox
    {
        private ThemeManager _themeManager;

        public MrngGroupBox()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }


        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.getInstance();
            if (_themeManager.ThemingActive)
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            //Reusing the textbox colors
            var titleColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("GroupBox_Foreground");
            //var backColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("GroupBox_Backgorund");
            var lineColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("GroupBox_Line");

            if (!Enabled)
            {
                titleColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("GroupBox_Disabled_Foreground");
                //backColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("GroupBox_Disabled_Background");
                lineColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("GroupBox_Disabled_Line");
            }


            //var state = Enabled ? GroupBoxState.Normal : GroupBoxState.Disabled;
            var flags = TextFormatFlags.PreserveGraphicsTranslateTransform | TextFormatFlags.PreserveGraphicsClipping |
                        TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak;

            if (!ShowKeyboardCues)
                flags |= TextFormatFlags.HidePrefix;
            if (RightToLeft == RightToLeft.Yes)
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;

            //No clear backgorund, this control is transparently
            //e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, Width, Height);

            var bounds = new Rectangle(0, 0, Width, Height);
            var rectangle = bounds;
            rectangle.Width -= 8;
            var size = TextRenderer.MeasureText(e.Graphics, Text, Font, new Size(rectangle.Width, rectangle.Height),
                                                flags);
            rectangle.Width = size.Width;
            rectangle.Height = size.Height;
            if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
                rectangle.X = (bounds.Right - rectangle.Width) - 8;
            else
                rectangle.X += 8;
            TextRenderer.DrawText(e.Graphics, Text, Font, rectangle, titleColor, flags);

            if (rectangle.Width > 0)
                rectangle.Inflate(2, 0);
            using (var pen = new Pen(lineColor))
            {
                var num = bounds.Top + (Font.Height / 2);
                //Left line
                e.Graphics.DrawLine(pen, bounds.Left + Padding.Left, num - Padding.Top, bounds.Left + Padding.Left,
                                    bounds.Height - Padding.Bottom);
                //Bottom line
                e.Graphics.DrawLine(pen, bounds.Left + Padding.Left, bounds.Height - Padding.Bottom,
                                    bounds.Width - Padding.Right, bounds.Height - Padding.Bottom);
                //Beside text line
                e.Graphics.DrawLine(pen, bounds.Left + Padding.Left, num - Padding.Top, rectangle.X - 3,
                                    num - Padding.Top);
                //Top line cutted
                e.Graphics.DrawLine(pen, rectangle.X + rectangle.Width + 2, num - Padding.Top,
                                    bounds.Width - Padding.Right, num - Padding.Top);
                //Right line
                e.Graphics.DrawLine(pen, bounds.Width - Padding.Right, num - Padding.Top, bounds.Width - Padding.Right,
                                    bounds.Height - Padding.Bottom);
            }

            RaisePaintEvent(this, e);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NGGroupBox
            // 
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeLayout(false);
        }
    }
}