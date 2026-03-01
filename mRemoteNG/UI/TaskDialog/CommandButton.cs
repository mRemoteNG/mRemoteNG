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
        public override string? Text
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
            imgArrow1 = Properties.Resources.GlyphRight_16x; // Initialize imgArrow1
            imgArrow2 = Properties.Resources.GlyphRight_16x; // Initialize imgArrow2
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
            if (string.IsNullOrEmpty(Text))
                return string.Empty;
            
            string[] lines = Text.Split('\n');
            return lines[0];
        }

        string GetSmallText()
        {
            if (string.IsNullOrEmpty(Text) || Text.IndexOf('\n') < 0)
                return "";

            string s = Text;
            string[] lines = s.Split('\n');
            s = "";
            for (int i = 1; i < lines.Length; i++)
                s += lines[i] + "\n";
            return s.Trim('\n');
        }

        SizeF GetLargeTextSizeF()
        {
            int x = LEFT_MARGIN + ARROW_WIDTH + 5;
            SizeF mzSize = new(Width - x - LEFT_MARGIN, 5000.0F); // presume RIGHT_MARGIN = LEFT_MARGIN
            Graphics g = Graphics.FromHwnd(Handle);
            SizeF textSize = g.MeasureString(GetLargeText(), Font, mzSize);
            return textSize;
        }

        SizeF GetSmallTextSizeF()
        {
            string s = GetSmallText();
            if (s == "") return new SizeF(0, 0);
            int x = LEFT_MARGIN + ARROW_WIDTH + 8; // <- indent small text slightly more
            SizeF mzSize = new(Width - x - LEFT_MARGIN, 5000.0F); // presume RIGHT_MARGIN = LEFT_MARGIN
            Graphics g = Graphics.FromHwnd(Handle);
            SizeF textSize = g.MeasureString(s, SmallFont, mzSize);
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
        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (!_themeManager.ActiveAndExtended)
            {
                base.OnPaint(pevent);
                return;
            }

            // ActiveAndExtended guarantees ExtendedPalette is non-null
            var palette = _themeManager.ActiveTheme.ExtendedPalette;
            if (palette == null)
            {
                base.OnPaint(pevent);
                return;
            }

            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pevent.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            const LinearGradientMode mode = LinearGradientMode.Vertical;

            Rectangle newRect = new(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1,
                                        ClientRectangle.Height - 1);

            Image img = imgArrow1;


            Color back;
            Color fore;
            Color border;
            if (Enabled)
            {
                switch (m_State)
                {
                    case eButtonState.MouseOver:
                        back = palette.getColor("Button_Hover_Background");
                        fore = palette.getColor("Button_Hover_Foreground");
                        border = palette.getColor("Button_Hover_Border");
                        break;
                    case eButtonState.Down:
                        back = palette.getColor("Button_Pressed_Background");
                        fore = palette.getColor("Button_Pressed_Foreground");
                        border = palette.getColor("Button_Pressed_Border");
                        break;
                    default:
                        back = palette.getColor("Button_Background");
                        fore = palette.getColor("Button_Foreground");
                        border = palette.getColor("Button_Border");
                        break;
                }
            }
            else
            {
                back = palette.getColor("Button_Disabled_Background");
                fore = palette.getColor("Button_Disabled_Foreground");
                border = palette.getColor("Button_Disabled_Border");
            }

            if (Enabled)
            {
                pevent.Graphics.FillRectangle(new SolidBrush(back), newRect);
                pevent.Graphics.DrawRectangle(new Pen(border, 1), newRect);
            }
            else
            {
                LinearGradientBrush brush = new(newRect, back, back, mode);
                pevent.Graphics.FillRectangle(brush, newRect);
                pevent.Graphics.DrawRectangle(new Pen(border, 1), newRect);
            }

            string largetext = GetLargeText();
            string smalltext = GetSmallText();

            SizeF szL = GetLargeTextSizeF();
            //e.Graphics.DrawString(largetext, base.Font, new SolidBrush(text_color), new RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN), szL));
            TextRenderer.DrawText(pevent.Graphics, largetext, Font,
                                  new Rectangle(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN, (int)szL.Width,
                                                (int)szL.Height), fore,
                                  TextFormatFlags.Default);

            if (smalltext != "")
            {
                SizeF szS = GetSmallTextSizeF();
                pevent.Graphics.DrawString(smalltext, SmallFont, new SolidBrush(fore),
                                      new
                                          RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 8, TOP_MARGIN + (int)szL.Height),
                                                     szS));
            }

            pevent.Graphics.DrawImage(img, new Point(LEFT_MARGIN, TOP_MARGIN + (int)(szL.Height / 2) - img.Height / 2));
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
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            m_State = eButtonState.MouseOver;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            m_State = eButtonState.Down;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        //--------------------------------------------------------------------------------
        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_autoHeight)
            {
                int h = GetBestHeight();
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