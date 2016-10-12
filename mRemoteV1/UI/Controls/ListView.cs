using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls
{
    public class ListView : System.Windows.Forms.ListView
    {
        private Brush backColorBrush;
        private Pen borderPen;
        private Brush foreColorBrush;

        #region Constructors

        public ListView()
        {
            ShowFocusCues = true;
            LabelAlignment = new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left);
            HighlightForeColor = SystemColors.HighlightText;
            HighlightBackColor = SystemColors.Highlight;
            HighlightBorderColor = SystemColors.HotTrack;
            InactiveHighlightForeColor = SystemColors.ControlText;
            InactiveHighlightBackColor = SystemColors.Control;
            InactiveHighlightBorderColor = SystemColors.ControlDark;
            OwnerDraw = true;
        }

        #endregion

        #region Private Methods

        private StringFormat GetStringFormat()
        {
            var format = StringFormat.GenericDefault;
            switch (LabelAlignment.Vertical)
            {
                case VerticalAlignment.Top:
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case VerticalAlignment.Middle:
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case VerticalAlignment.Bottom:
                    format.LineAlignment = StringAlignment.Far;
                    break;
            }

            switch (LabelAlignment.Horizontal)
            {
                case HorizontalAlignment.Left:
                    format.Alignment = StringAlignment.Near;
                    break;
                case HorizontalAlignment.Center:
                    format.Alignment = StringAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    format.Alignment = StringAlignment.Far;
                    break;
            }

            if (RightToLeftLayout)
                format.FormatFlags = format.FormatFlags | StringFormatFlags.DirectionRightToLeft;

            if (LabelWrap)
                format.FormatFlags = format.FormatFlags & ~StringFormatFlags.NoWrap;
            else
                format.FormatFlags = format.FormatFlags | StringFormatFlags.NoWrap;

            return format;
        }

        #endregion

        #region Public Properties

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "HighlightText")]
        public Color HighlightForeColor { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Highlight")]
        public Color HighlightBackColor { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "HotTrack")]
        public Color HighlightBorderColor { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ControlText")]
        public Color InactiveHighlightForeColor { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Control")]
        public Color InactiveHighlightBackColor { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color InactiveHighlightBorderColor { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public new bool ShowFocusCues { get; set; }

        [Category("Appearance")]
        public Alignment LabelAlignment { get; set; }

        #endregion

        #region Protected Methods

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            if ((View != View.Tile) || (e.ItemIndex < 0))
                base.OnDrawItem(e);
            else
                CustomDraw(e);
        }

        private void CustomDraw(DrawListViewItemEventArgs e)
        {
            try
            {
                ResetBrushesAndPens();
                BuildBorderPen();
                BuildBrushes(e);
                FillBackgroundColor(e);
                DrawItemBorder(e);
                DrawImageAndText(e);
            }
            finally
            {
                DisposeBrushesAndPens();
            }
        }

        private void ResetBrushesAndPens()
        {
            foreColorBrush = null;
            backColorBrush = null;
            borderPen = null;
        }

        private void BuildBorderPen()
        {
            if (Focused)
                borderPen = new Pen(HighlightBorderColor);
            borderPen = new Pen(InactiveHighlightBorderColor);
        }

        private void BuildBrushes(DrawListViewItemEventArgs e)
        {
            if (e.Item.Selected)
            {
                if (Focused)
                {
                    foreColorBrush = new SolidBrush(HighlightForeColor);
                    backColorBrush = new SolidBrush(HighlightBackColor);
                }
                else
                {
                    foreColorBrush = new SolidBrush(InactiveHighlightForeColor);
                    backColorBrush = new SolidBrush(InactiveHighlightBackColor);
                }
            }
            else
            {
                foreColorBrush = new SolidBrush(e.Item.ForeColor);
                backColorBrush = new SolidBrush(BackColor);
            }
        }

        private void FillBackgroundColor(DrawListViewItemEventArgs e)
        {
            e.Graphics.FillRectangle(backColorBrush, e.Bounds);
        }

        private void DrawImageAndText(DrawListViewItemEventArgs e)
        {
            var imageBounds = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 6, 16, 16);
            var textBounds = e.Bounds;
            if (e.Item.ImageList != null)
            {
                var image = GetItemImage(e);
                if (image != null)
                {
                    DrawImage(e, imageBounds, image);
                    textBounds = UpdateTextBoundsToLeaveRoomForImage(textBounds);
                }
            }
            DrawText(e, textBounds);
        }

        private static Image GetItemImage(DrawListViewItemEventArgs e)
        {
            Image image = null;
            if (!string.IsNullOrEmpty(e.Item.ImageKey) && e.Item.ImageList.Images.ContainsKey(e.Item.ImageKey))
                image = e.Item.ImageList.Images[e.Item.ImageKey];
            else if (!(e.Item.ImageIndex < 0) & (e.Item.ImageList.Images.Count > e.Item.ImageIndex))
                image = e.Item.ImageList.Images[e.Item.ImageIndex];
            return image;
        }

        private static void DrawImage(DrawListViewItemEventArgs e, Rectangle imageBounds, Image image)
        {
            e.Graphics.DrawImageUnscaledAndClipped(image, imageBounds);
        }

        private static Rectangle UpdateTextBoundsToLeaveRoomForImage(Rectangle textBounds)
        {
            textBounds.X = textBounds.Left + 20;
            textBounds.Width = textBounds.Width - 20;
            return textBounds;
        }

        private void DrawText(DrawListViewItemEventArgs e, Rectangle textBounds)
        {
            e.Graphics.DrawString(e.Item.Text, e.Item.Font, foreColorBrush, textBounds, GetStringFormat());
        }

        private void DrawItemBorder(DrawListViewItemEventArgs e)
        {
            if (Focused && ShowFocusCues)
                e.DrawFocusRectangle();
            else if (e.Item.Selected)
                e.Graphics.DrawRectangle(borderPen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
        }

        private void DisposeBrushesAndPens()
        {
            foreColorBrush?.Dispose();
            backColorBrush?.Dispose();
            borderPen?.Dispose();
        }

        #endregion
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Alignment
    {
        [DefaultValue(HorizontalAlignment.Left)] private HorizontalAlignment _Horizontal = HorizontalAlignment.Left;

        [DefaultValue(VerticalAlignment.Top)] private VerticalAlignment _Vertical = VerticalAlignment.Top;


        public Alignment()
        {
        }


        public Alignment(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment)
        {
            Vertical = verticalAlignment;
            Horizontal = horizontalAlignment;
        }

        public VerticalAlignment Vertical
        {
            get { return _Vertical; }
            set { _Vertical = value; }
        }

        public HorizontalAlignment Horizontal
        {
            get { return _Horizontal; }
            set { _Horizontal = value; }
        }

        public override string ToString()
        {
            return $"{Vertical}, {Horizontal}";
        }
    }

    #region Enums

    public enum VerticalAlignment
    {
        Top,
        Middle,
        Bottom
    }

    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    #endregion
}