using mRemoteNG.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace mRemoteNG.UI.TaskDialog
{
    [SupportedOSPlatform("windows")]
    public sealed partial class CommandButton : Button
    {
        //--------------------------------------------------------------------------------

        #region PRIVATE MEMBERS

        //--------------------------------------------------------------------------------
        private Image imgArrow1;
        private Image imgArrow2;
        private readonly ThemeManager _themeManager;
        private const int LEFT_MARGIN = 10;
        private const int TOP_MARGIN = 10;
        private const int ARROW_WIDTH = 19;

        enum eButtonState
        {
            Normal,
            MouseOver,
            Down
        }

        eButtonState m_State = eButtonState.Normal;

        #endregion

        //--------------------------------------------------------------------------------

        #region PUBLIC PROPERTIES

        //--------------------------------------------------------------------------------
        // Override this to make sure the control is invalidated (repainted) when 'Text' is changed
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                if (m_autoHeight)
                    Height = GetBestHeight();
                Invalidate();
            }
        }

        // SmallFont is the font used for secondary lines
        private Font SmallFont { get; set; }

        // AutoHeight determines whether the button automatically resizes itself to fit the Text
        bool m_autoHeight = true;

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AutoHeight
        {
            get => m_autoHeight;
            set
            {
                m_autoHeight = value;
                if (m_autoHeight) Invalidate();
            }
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region CONSTRUCTOR

        //--------------------------------------------------------------------------------
        public CommandButton()
        {
            InitializeComponent();
            Font = new Font("Segoe UI", 11.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SmallFont = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _themeManager = ThemeManager.getInstance();
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region PUBLIC ROUTINES

        //--------------------------------------------------------------------------------
        public int GetBestHeight()
        {
            return (TOP_MARGIN * 2) + (int)GetSmallTextSizeF().Height + (int)GetLargeTextSizeF().Height;
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region PRIVATE ROUTINES

        //--------------------------------------------------------------------------------
        string GetLargeText()
        {
            var lines = Text.Split('\n');
            return lines[0];
        }

        string GetSmallText()
        {
            if (Text.IndexOf('\n') < 0)
                return "";

            var s = Text;
            var lines = s.Split('\n');
            s = "";
            for (var i = 1; i < lines.Length; i++)
                s += lines[i] + "\n";
            return s.Trim('\n');
        }

        SizeF GetLargeTextSizeF()
        {
            var x = LEFT_MARGIN + ARROW_WIDTH + 5;
            var mzSize = new SizeF(Width - x - LEFT_MARGIN, 5000.0F); // presume RIGHT_MARGIN = LEFT_MARGIN
            var g = Graphics.FromHwnd(Handle);
            var textSize = g.MeasureString(GetLargeText(), Font, mzSize);
            return textSize;
        }

        SizeF GetSmallTextSizeF()
        {
            var s = GetSmallText();
            if (s == "") return new SizeF(0, 0);
            var x = LEFT_MARGIN + ARROW_WIDTH + 8; // <- indent small text slightly more
            var mzSize = new SizeF(Width - x - LEFT_MARGIN, 5000.0F); // presume RIGHT_MARGIN = LEFT_MARGIN
            var g = Graphics.FromHwnd(Handle);
            var textSize = g.MeasureString(s, SmallFont, mzSize);
            return textSize;
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region OVERRIDES

        //--------------------------------------------------------------------------------
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            imgArrow1 = Properties.Resources.GlyphRight_16x;
            imgArrow2 = Properties.Resources.GlyphRight_16x;
        }

        //--------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(e);
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            const LinearGradientMode mode = LinearGradientMode.Vertical;

            var newRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1,
                                        ClientRectangle.Height - 1);

            var img = imgArrow1;


            Color back;
            Color fore;
            Color border;
            if (Enabled)
            {
                switch (m_State)
                {
                    case eButtonState.MouseOver:
                        back = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Background");
                        fore = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Foreground");
                        border = _themeManager.ActiveTheme.ExtendedPalette.getColor("Button_Hover_Border");
                        break;
                    case eButtonState.Down:
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

            if (Enabled)
            {
                e.Graphics.FillRectangle(new SolidBrush(back), newRect);
                e.Graphics.DrawRectangle(new Pen(border, 1), newRect);
            }
            else
            {
                var brush = new LinearGradientBrush(newRect, back, back, mode);
                e.Graphics.FillRectangle(brush, newRect);
                e.Graphics.DrawRectangle(new Pen(border, 1), newRect);
            }

            var largetext = GetLargeText();
            var smalltext = GetSmallText();

            var szL = GetLargeTextSizeF();
            //e.Graphics.DrawString(largetext, base.Font, new SolidBrush(text_color), new RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN), szL));
            TextRenderer.DrawText(e.Graphics, largetext, Font,
                                  new Rectangle(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN, (int)szL.Width,
                                                (int)szL.Height), fore,
                                  TextFormatFlags.Default);

            if (smalltext != "")
            {
                var szS = GetSmallTextSizeF();
                e.Graphics.DrawString(smalltext, SmallFont, new SolidBrush(fore),
                                      new
                                          RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 8, TOP_MARGIN + (int)szL.Height),
                                                     szS));
            }

            e.Graphics.DrawImage(img, new Point(LEFT_MARGIN, TOP_MARGIN + (int)(szL.Height / 2) - img.Height / 2));
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseLeave(EventArgs e)
        {
            m_State = eButtonState.Normal;
            Invalidate();
            base.OnMouseLeave(e);
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseEnter(EventArgs e)
        {
            m_State = eButtonState.MouseOver;
            Invalidate();
            base.OnMouseEnter(e);
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_State = eButtonState.MouseOver;
            Invalidate();
            base.OnMouseUp(e);
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_State = eButtonState.Down;
            Invalidate();
            base.OnMouseDown(e);
        }

        //--------------------------------------------------------------------------------
        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_autoHeight)
            {
                var h = GetBestHeight();
                if (Height != h)
                {
                    Height = h;
                    return;
                }
            }

            base.OnSizeChanged(e);
        }

        #endregion

        //--------------------------------------------------------------------------------
    }
}