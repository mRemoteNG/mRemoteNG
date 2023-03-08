using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    [SupportedOSPlatform("windows")]
    internal sealed class MremoteNGAutoHideStrip : AutoHideStripBase
    {
        private class TabNG : Tab
        {
            internal TabNG(IDockContent content)
                : base(content)
            {
            }

            public int TabX { get; set; }

            public int TabWidth { get; set; }
        }

        private const int _ImageHeight = 16;
        private const int _ImageWidth = 16;
        private const int _ImageGapTop = 2;
        private const int _ImageGapLeft = 4;
        private const int _ImageGapRight = 2;
        private const int _ImageGapBottom = 2;
        private const int _TextGapLeft = 0;
        private const int _TextGapRight = 0;
        private const int _TabGapTop = 3;
        private const int _TabGapLeft = 4;
        private const int _TabGapBetween = 10;

        #region Customizable Properties

        public Font TextFont => DockPanel.Theme.Skin.AutoHideStripSkin.TextFont;

        private static StringFormat _stringFormatTabHorizontal;

        private StringFormat StringFormatTabHorizontal
        {
            get
            {
                if (_stringFormatTabHorizontal == null)
                {
                    _stringFormatTabHorizontal = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.None
                    };
                }

                if (RightToLeft == RightToLeft.Yes)
                    _stringFormatTabHorizontal.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                else
                    _stringFormatTabHorizontal.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

                return _stringFormatTabHorizontal;
            }
        }

        private static StringFormat _stringFormatTabVertical;

        private StringFormat StringFormatTabVertical
        {
            get
            {
                if (_stringFormatTabVertical == null)
                {
                    _stringFormatTabVertical = new StringFormat();
                    _stringFormatTabVertical.Alignment = StringAlignment.Near;
                    _stringFormatTabVertical.LineAlignment = StringAlignment.Center;
                    _stringFormatTabVertical.FormatFlags =
                        StringFormatFlags.NoWrap | StringFormatFlags.DirectionVertical;
                    _stringFormatTabVertical.Trimming = StringTrimming.None;
                }

                if (RightToLeft == RightToLeft.Yes)
                    _stringFormatTabVertical.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                else
                    _stringFormatTabVertical.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;

                return _stringFormatTabVertical;
            }
        }

        private static int ImageHeight => _ImageHeight;

        private static int ImageWidth => _ImageWidth;

        private static int ImageGapTop => _ImageGapTop;

        private static int ImageGapLeft => _ImageGapLeft;

        private static int ImageGapRight => _ImageGapRight;

        private static int ImageGapBottom => _ImageGapBottom;

        private static int TextGapLeft => _TextGapLeft;

        private static int TextGapRight => _TextGapRight;

        private static int TabGapTop => _TabGapTop;

        private static int TabGapLeft => _TabGapLeft;

        private static int TabGapBetween => _TabGapBetween;

        private static Pen PenTabBorder => SystemPens.GrayText;

        #endregion

        private static Matrix MatrixIdentity { get; } = new Matrix();

        private static DockState[] _dockStates;

        private static DockState[] DockStates
        {
            get
            {
                if (_dockStates == null)
                {
                    _dockStates = new DockState[4];
                    _dockStates[0] = DockState.DockLeftAutoHide;
                    _dockStates[1] = DockState.DockRightAutoHide;
                    _dockStates[2] = DockState.DockTopAutoHide;
                    _dockStates[3] = DockState.DockBottomAutoHide;
                }

                return _dockStates;
            }
        }

        private static GraphicsPath _graphicsPath;

        internal static GraphicsPath GraphicsPath
        {
            get
            {
                if (_graphicsPath == null)
                    _graphicsPath = new GraphicsPath();

                return _graphicsPath;
            }
        }

        public MremoteNGAutoHideStrip(DockPanel panel)
            : base(panel)
        {
            SetStyle(ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = SystemColors.ControlLight;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            var startColor = DockPanel.Theme.Skin.AutoHideStripSkin.DockStripGradient.StartColor;
            var endColor = DockPanel.Theme.Skin.AutoHideStripSkin.DockStripGradient.EndColor;
            var gradientMode = DockPanel.Theme.Skin.AutoHideStripSkin.DockStripGradient.LinearGradientMode;
            using (var brush = new LinearGradientBrush(ClientRectangle, startColor, endColor, gradientMode))
            {
                g.FillRectangle(brush, ClientRectangle);
            }

            DrawTabStrip(g);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            CalculateTabs();
            base.OnLayout(levent);
        }

        private void DrawTabStrip(Graphics g)
        {
            DrawTabStrip(g, DockState.DockTopAutoHide);
            DrawTabStrip(g, DockState.DockBottomAutoHide);
            DrawTabStrip(g, DockState.DockLeftAutoHide);
            DrawTabStrip(g, DockState.DockRightAutoHide);
        }

        private void DrawTabStrip(Graphics g, DockState dockState)
        {
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            if (rectTabStrip.IsEmpty)
                return;

            var matrixIdentity = g.Transform;
            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
            {
                var matrixRotated = new Matrix();
                matrixRotated.RotateAt(90, new PointF(rectTabStrip.X + (float)rectTabStrip.Height / 2,
                                                      rectTabStrip.Y + (float)rectTabStrip.Height / 2));
                g.Transform = matrixRotated;
            }

            foreach (var pane in GetPanes(dockState))
            {
                foreach (TabNG tab in pane.AutoHideTabs)
                    DrawTab(g, tab);
            }

            g.Transform = matrixIdentity;
        }

        private void CalculateTabs()
        {
            CalculateTabs(DockState.DockTopAutoHide);
            CalculateTabs(DockState.DockBottomAutoHide);
            CalculateTabs(DockState.DockLeftAutoHide);
            CalculateTabs(DockState.DockRightAutoHide);
        }

        private void CalculateTabs(DockState dockState)
        {
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            var imageHeight = rectTabStrip.Height - ImageGapTop - ImageGapBottom;
            var imageWidth = ImageWidth;
            if (imageHeight > ImageHeight)
                imageWidth = ImageWidth * (imageHeight / ImageHeight);

            var x = TabGapLeft + rectTabStrip.X;
            foreach (var pane in GetPanes(dockState))
            {
                foreach (TabNG tab in pane.AutoHideTabs)
                {
                    var width = imageWidth + ImageGapLeft + ImageGapRight +
                                TextRenderer.MeasureText(tab.Content.DockHandler.TabText, TextFont).Width +
                                TextGapLeft + TextGapRight;
                    tab.TabX = x;
                    tab.TabWidth = width;
                    x += width;
                }

                x += TabGapBetween;
            }
        }

        private Rectangle RtlTransform(Rectangle rect, DockState dockState)
        {
            Rectangle rectTransformed;
            if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                rectTransformed = rect;
            else
                rectTransformed = DrawHelper.RtlTransform(this, rect);

            return rectTransformed;
        }

        private GraphicsPath GetTabOutline(TabNG tab, bool transformed, bool rtlTransform)
        {
            var dockState = tab.Content.DockHandler.DockState;
            var rectTab = GetTabRectangle(tab, transformed);
            if (rtlTransform)
                rectTab = RtlTransform(rectTab, dockState);
            var upTab = (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockBottomAutoHide);
            DrawHelper.GetRoundedCornerTab(GraphicsPath, rectTab, upTab);

            return GraphicsPath;
        }

        private void DrawTab(Graphics g, TabNG tab)
        {
            var rectTabOrigin = GetTabRectangle(tab);
            if (rectTabOrigin.IsEmpty)
                return;

            var dockState = tab.Content.DockHandler.DockState;
            var content = tab.Content;

            var path = GetTabOutline(tab, false, true);
            var startColor = DockPanel.Theme.Skin.AutoHideStripSkin.TabGradient.StartColor;
            var endColor = DockPanel.Theme.Skin.AutoHideStripSkin.TabGradient.EndColor;
            var gradientMode = DockPanel.Theme.Skin.AutoHideStripSkin.TabGradient.LinearGradientMode;
            g.FillPath(new LinearGradientBrush(rectTabOrigin, startColor, endColor, gradientMode), path);
            g.DrawPath(PenTabBorder, path);

            // Set no rotate for drawing icon and text
            using (var matrixRotate = g.Transform)
            {
                g.Transform = MatrixIdentity;

                // Draw the icon
                var rectImage = rectTabOrigin;
                rectImage.X += ImageGapLeft;
                rectImage.Y += ImageGapTop;
                var imageHeight = rectTabOrigin.Height - ImageGapTop - ImageGapBottom;
                var imageWidth = ImageWidth;
                if (imageHeight > ImageHeight)
                    imageWidth = ImageWidth * (imageHeight / ImageHeight);
                rectImage.Height = imageHeight;
                rectImage.Width = imageWidth;
                rectImage = GetTransformedRectangle(dockState, rectImage);

                if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                {
                    // The DockState is DockLeftAutoHide or DockRightAutoHide, so rotate the image 90 degrees to the right. 
                    var rectTransform = RtlTransform(rectImage, dockState);
                    Point[] rotationPoints =
                    {
                        new Point(rectTransform.X + rectTransform.Width, rectTransform.Y),
                        new Point(rectTransform.X + rectTransform.Width, rectTransform.Y + rectTransform.Height),
                        new Point(rectTransform.X, rectTransform.Y)
                    };

                    using (var rotatedIcon = new Icon(((Form)content).Icon, 16, 16))
                    {
                        g.DrawImage(rotatedIcon.ToBitmap(), rotationPoints);
                    }
                }
                else
                {
                    // Draw the icon normally without any rotation.
                    g.DrawIcon(((Form)content).Icon, RtlTransform(rectImage, dockState));
                }

                // Draw the text
                var rectText = rectTabOrigin;
                rectText.X += ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft;
                rectText.Width -= ImageGapLeft + imageWidth + ImageGapRight + TextGapLeft;
                rectText = RtlTransform(GetTransformedRectangle(dockState, rectText), dockState);

                var textColor = DockPanel.Theme.Skin.AutoHideStripSkin.TabGradient.TextColor;

                if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
                    g.DrawString(content.DockHandler.TabText, TextFont, new SolidBrush(textColor), rectText,
                                 StringFormatTabVertical);
                else
                    g.DrawString(content.DockHandler.TabText, TextFont, new SolidBrush(textColor), rectText,
                                 StringFormatTabHorizontal);

                // Set rotate back
                g.Transform = matrixRotate;
            }
        }

        private Rectangle GetLogicalTabStripRectangle(DockState dockState)
        {
            return GetLogicalTabStripRectangle(dockState, false);
        }

        private Rectangle GetLogicalTabStripRectangle(DockState dockState, bool transformed)
        {
            if (!DockHelper.IsDockStateAutoHide(dockState))
                return Rectangle.Empty;

            var leftPanes = GetPanes(DockState.DockLeftAutoHide).Count;
            var rightPanes = GetPanes(DockState.DockRightAutoHide).Count;
            var topPanes = GetPanes(DockState.DockTopAutoHide).Count;
            var bottomPanes = GetPanes(DockState.DockBottomAutoHide).Count;

            int x, y, width, height;

            height = MeasureHeight();
            if (dockState == DockState.DockLeftAutoHide && leftPanes > 0)
            {
                x = 0;
                y = (topPanes == 0) ? 0 : height;
                width = Height - (topPanes == 0 ? 0 : height) - (bottomPanes == 0 ? 0 : height);
            }
            else if (dockState == DockState.DockRightAutoHide && rightPanes > 0)
            {
                x = Width - height;
                if (leftPanes != 0 && x < height)
                    x = height;
                y = (topPanes == 0) ? 0 : height;
                width = Height - (topPanes == 0 ? 0 : height) - (bottomPanes == 0 ? 0 : height);
            }
            else if (dockState == DockState.DockTopAutoHide && topPanes > 0)
            {
                x = leftPanes == 0 ? 0 : height;
                y = 0;
                width = Width - (leftPanes == 0 ? 0 : height) - (rightPanes == 0 ? 0 : height);
            }
            else if (dockState == DockState.DockBottomAutoHide && bottomPanes > 0)
            {
                x = leftPanes == 0 ? 0 : height;
                y = Height - height;
                if (topPanes != 0 && y < height)
                    y = height;
                width = Width - (leftPanes == 0 ? 0 : height) - (rightPanes == 0 ? 0 : height);
            }
            else
                return Rectangle.Empty;

            if (width == 0 || height == 0)
            {
                return Rectangle.Empty;
            }

            var rect = new Rectangle(x, y, width, height);
            return transformed ? GetTransformedRectangle(dockState, rect) : rect;
        }

        private Rectangle GetTabRectangle(TabNG tab)
        {
            return GetTabRectangle(tab, false);
        }

        private Rectangle GetTabRectangle(TabNG tab, bool transformed)
        {
            var dockState = tab.Content.DockHandler.DockState;
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);

            if (rectTabStrip.IsEmpty)
                return Rectangle.Empty;

            var x = tab.TabX;
            var y = rectTabStrip.Y +
                    (dockState == DockState.DockTopAutoHide || dockState == DockState.DockRightAutoHide
                        ? 0
                        : TabGapTop);
            var width = tab.TabWidth;
            var height = rectTabStrip.Height - TabGapTop;

            if (!transformed)
                return new Rectangle(x, y, width, height);
            else
                return GetTransformedRectangle(dockState, new Rectangle(x, y, width, height));
        }

        private Rectangle GetTransformedRectangle(DockState dockState, Rectangle rect)
        {
            if (dockState != DockState.DockLeftAutoHide && dockState != DockState.DockRightAutoHide)
                return rect;

            var pts = new PointF[1];
            // the center of the rectangle
            pts[0].X = rect.X + (float)rect.Width / 2;
            pts[0].Y = rect.Y + (float)rect.Height / 2;
            var rectTabStrip = GetLogicalTabStripRectangle(dockState);
            using (var matrix = new Matrix())
            {
                matrix.RotateAt(90, new PointF(rectTabStrip.X + (float)rectTabStrip.Height / 2,
                                               rectTabStrip.Y + (float)rectTabStrip.Height / 2));
                matrix.TransformPoints(pts);
            }

            return new Rectangle((int)(pts[0].X - (float)rect.Height / 2 + .5F),
                                 (int)(pts[0].Y - (float)rect.Width / 2 + .5F),
                                 rect.Height, rect.Width);
        }

        protected override IDockContent HitTest(Point point)
        {
            foreach (var state in DockStates)
            {
                var rectTabStrip = GetLogicalTabStripRectangle(state, true);
                if (!rectTabStrip.Contains(point))
                    continue;

                foreach (var pane in GetPanes(state))
                {
                    foreach (TabNG tab in pane.AutoHideTabs)
                    {
                        var path = GetTabOutline(tab, true, true);
                        if (path.IsVisible(point))
                            return tab.Content;
                    }
                }
            }

            return null;
        }

        protected override Rectangle GetTabBounds(Tab tab)
        {
            var path = GetTabOutline((TabNG)tab, true, true);
            var bounds = path.GetBounds();
            return new Rectangle((int)bounds.Left, (int)bounds.Top, (int)bounds.Width, (int)bounds.Height);
        }

        protected override int MeasureHeight()
        {
            return Math.Max(ImageGapBottom +
                            ImageGapTop + ImageHeight,
                            TextFont.Height) + TabGapTop;
        }

        protected override void OnRefreshChanges()
        {
            CalculateTabs();
            Invalidate();
        }

        protected override Tab CreateTab(IDockContent content)
        {
            return new TabNG(content);
        }
    }
}