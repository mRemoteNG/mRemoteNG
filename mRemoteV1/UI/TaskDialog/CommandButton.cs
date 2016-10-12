using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.TaskDialog
{
    public sealed partial class CommandButton : Button
    {
        //--------------------------------------------------------------------------------

        #region CONSTRUCTOR

        //--------------------------------------------------------------------------------
        public CommandButton()
        {
            InitializeComponent();
            Font = new Font("Segoe UI", 11.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SmallFont = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region PUBLIC ROUTINES

        //--------------------------------------------------------------------------------
        public int GetBestHeight()
        {
            return TOP_MARGIN*2 + (int) GetSmallTextSizeF().Height + (int) GetLargeTextSizeF().Height;
        }

        #endregion

        //--------------------------------------------------------------------------------

        #region PRIVATE MEMBERS

        //--------------------------------------------------------------------------------
        private Image imgArrow1;
        private Image imgArrow2;

        private const int LEFT_MARGIN = 10;
        private const int TOP_MARGIN = 10;
        private const int ARROW_WIDTH = 19;

        private enum eButtonState
        {
            Normal,
            MouseOver,
            Down
        }

        private eButtonState m_State = eButtonState.Normal;

        #endregion

        //--------------------------------------------------------------------------------

        #region PUBLIC PROPERTIES

        //--------------------------------------------------------------------------------
        // Override this to make sure the control is invalidated (repainted) when 'Text' is changed
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                if (m_autoHeight)
                    Height = GetBestHeight();
                Invalidate();
            }
        }

        // SmallFont is the font used for secondary lines
        private Font SmallFont { get; }

        // AutoHeight determines whether the button automatically resizes itself to fit the Text
        private bool m_autoHeight = true;

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AutoHeight
        {
            get { return m_autoHeight; }
            set
            {
                m_autoHeight = value;
                if (m_autoHeight) Invalidate();
            }
        }

        #endregion

        //--------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------

        #region PRIVATE ROUTINES

        //--------------------------------------------------------------------------------
        private string GetLargeText()
        {
            var lines = Text.Split('\n');
            return lines[0];
        }

        private string GetSmallText()
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

        private SizeF GetLargeTextSizeF()
        {
            var x = LEFT_MARGIN + ARROW_WIDTH + 5;
            var mzSize = new SizeF(Width - x - LEFT_MARGIN, 5000.0F); // presume RIGHT_MARGIN = LEFT_MARGIN
            var g = Graphics.FromHwnd(Handle);
            var textSize = g.MeasureString(GetLargeText(), Font, mzSize);
            return textSize;
        }

        private SizeF GetSmallTextSizeF()
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

        #region OVERRIDEs

        //--------------------------------------------------------------------------------
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            imgArrow1 = Resources.green_arrow1;
            imgArrow2 = Resources.green_arrow2;
        }

        //--------------------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            LinearGradientBrush brush;
            var mode = LinearGradientMode.Vertical;

            var newRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1,
                ClientRectangle.Height - 1);
            var text_color = SystemColors.WindowText;

            var img = imgArrow1;

            if (Enabled)
            {
                switch (m_State)
                {
                    case eButtonState.Normal:
                        e.Graphics.FillRectangle(SystemBrushes.Control, newRect);
                        e.Graphics.DrawRectangle(Focused ? new Pen(Color.Silver, 1) : new Pen(SystemColors.Control, 1),
                            newRect);
                        text_color = Color.DarkBlue;
                        break;

                    case eButtonState.MouseOver:
                        brush = new LinearGradientBrush(newRect, SystemColors.Control, SystemColors.Control, mode);
                        e.Graphics.FillRectangle(brush, newRect);
                        e.Graphics.DrawRectangle(new Pen(Color.Silver, 1), newRect);
                        img = imgArrow2;
                        text_color = Color.Blue;
                        break;

                    case eButtonState.Down:
                        brush = new LinearGradientBrush(newRect, SystemColors.Control, SystemColors.Control, mode);
                        e.Graphics.FillRectangle(brush, newRect);
                        e.Graphics.DrawRectangle(new Pen(Color.DarkGray, 1), newRect);
                        text_color = Color.DarkBlue;
                        break;
                }
            }
            else
            {
                brush = new LinearGradientBrush(newRect, SystemColors.Control, SystemColors.Control, mode);
                e.Graphics.FillRectangle(brush, newRect);
                e.Graphics.DrawRectangle(new Pen(Color.DarkGray, 1), newRect);
                text_color = Color.DarkBlue;
            }

            var largetext = GetLargeText();
            var smalltext = GetSmallText();

            var szL = GetLargeTextSizeF();
            //e.Graphics.DrawString(largetext, base.Font, new SolidBrush(text_color), new RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN), szL));
            TextRenderer.DrawText(e.Graphics, largetext, Font,
                new Rectangle(LEFT_MARGIN + imgArrow1.Width + 5, TOP_MARGIN, (int) szL.Width, (int) szL.Height),
                text_color, TextFormatFlags.Default);

            if (smalltext != "")
            {
                var szS = GetSmallTextSizeF();
                e.Graphics.DrawString(smalltext, SmallFont, new SolidBrush(text_color),
                    new RectangleF(new PointF(LEFT_MARGIN + imgArrow1.Width + 8, TOP_MARGIN + (int) szL.Height), szS));
            }

            e.Graphics.DrawImage(img, new Point(LEFT_MARGIN, TOP_MARGIN + (int) (szL.Height/2) - img.Height/2));
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
    }
}