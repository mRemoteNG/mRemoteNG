using mRemoteNG.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.UI.Tabs
{
    /// <summary>
    /// This class is lifted from VS2005DockPaneStrip from DockPanelSuite and customized for MremoteNG
    /// </summary>
    internal class DockPaneStripNG : DockPaneStripBase
    {
        private class MremoteNGTab : Tab
        {
            public MremoteNGTab(IDockContent content)
                : base(content)
            {
            }

            private int m_tabX;
            public int TabX
            {
                get => m_tabX;
                set => m_tabX = value;
            }

            private int m_tabWidth;
            public int TabWidth
            {
                get => m_tabWidth;
                set => m_tabWidth = value;
            }

            private int m_maxWidth;
            public int MaxWidth
            {
                get => m_maxWidth;
                set => m_maxWidth = value;
            }

            private bool m_flag;
            protected internal bool Flag
            {
                get => m_flag;
                set => m_flag = value;
            }

            private Rectangle? _rect;

            
        }

  
        protected override Tab CreateTab(IDockContent content)
        {
            return new MremoteNGTab(content);
        }

        private sealed class InertButton : InertButtonBase
        {
            private Bitmap m_image0, m_image1;

            public InertButton(Bitmap image0, Bitmap image1)
                : base()
            {
                m_image0 = image0;
                m_image1 = image1;
            }

            private int m_imageCategory = 0;
            public int ImageCategory
            {
                get => m_imageCategory;
                set
                {
                    if (m_imageCategory == value)
                        return;

                    m_imageCategory = value;
                    Invalidate();
                }
            }

            public override Bitmap Image => ImageCategory == 0 ? m_image0 : m_image1;

            public override Bitmap HoverImage => null;

            public override Bitmap PressImage => null;
        }

        #region Constants

        private const int _ToolWindowStripGapTop = 0;
        private const int _ToolWindowStripGapBottom = 1;
        private const int _ToolWindowStripGapLeft = 0;
        private const int _ToolWindowStripGapRight = 0;
        private const int _ToolWindowImageHeight = 16;
        private const int _ToolWindowImageWidth = 16;
        private const int _ToolWindowImageGapTop = 3;
        private const int _ToolWindowImageGapBottom = 1;
        private const int _ToolWindowImageGapLeft = 2;
        private const int _ToolWindowImageGapRight = 0;
        private const int _ToolWindowTextGapRight = 3;
        private const int _ToolWindowTabSeperatorGapTop = 3;
        private const int _ToolWindowTabSeperatorGapBottom = 3;

        private const int _DocumentStripGapTop = 0;
        private const int _DocumentStripGapBottom = 1;
        private const int _DocumentTabMaxWidth = 200;
        private const int _DocumentButtonGapTop = 4;
        private const int _DocumentButtonGapBottom = 4;
        private const int _DocumentButtonGapBetween = 0;
        private const int _DocumentButtonGapRight = 3;
        private const int _DocumentTabGapTop = 3;
        private const int _DocumentTabGapLeft = 3;
        private const int _DocumentTabGapRight = 3;
        private const int _DocumentIconGapBottom = 2;
        private const int _DocumentIconGapLeft = 8;
        private const int _DocumentIconGapRight = 0;
        private const int _DocumentIconHeight = 16;
        private const int _DocumentIconWidth = 16;
        private const int _DocumentTextGapRight = 3;

        #endregion

        #region Members

        private ContextMenuStrip m_selectMenu;
        private static Bitmap m_imageButtonClose;
        private InertButton m_buttonClose;
        private static Bitmap m_imageButtonWindowList;
        private static Bitmap m_imageButtonWindowListOverflow;
        private InertButton m_buttonWindowList;
        private IContainer m_components;
        private ToolTip m_toolTip;
        private Font m_font;
        private Font m_boldFont;
        private int m_startDisplayingTab = 0;
        private int m_endDisplayingTab = 0;
        private int m_firstDisplayingTab = 0;
        private bool m_documentTabsOverflow = false;
        private static string m_toolTipSelect;
        private static string m_toolTipClose;
        private bool m_closeButtonVisible = false;
        private int _selectMenuMargin = 5;

        #endregion

        #region Properties

        private Rectangle TabStripRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.Document)
                    return TabStripRectangle_Document;
                else
                    return TabStripRectangle_ToolWindow;
            }
        }

        private Rectangle TabStripRectangle_ToolWindow
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + ToolWindowStripGapTop, rect.Width, rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom);
            }
        }

        private Rectangle TabStripRectangle_Document
        {
            get
            {
                var rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + DocumentStripGapTop, rect.Width, rect.Height - DocumentStripGapTop - ToolWindowStripGapBottom);
            }
        }

        private Rectangle TabsRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                    return TabStripRectangle;

                var rectWindow = TabStripRectangle;
                var x = rectWindow.X;
                var y = rectWindow.Y;
                var width = rectWindow.Width;
                var height = rectWindow.Height;

                x += DocumentTabGapLeft;
                width -= DocumentTabGapLeft +
                    DocumentTabGapRight +
                    DocumentButtonGapRight +
                    ButtonClose.Width +
                    ButtonWindowList.Width +
                    2 * DocumentButtonGapBetween;

                return new Rectangle(x, y, width, height);
            }
        }

        private ContextMenuStrip SelectMenu => m_selectMenu;

        public int SelectMenuMargin
        {
            get => _selectMenuMargin;
            set => _selectMenuMargin = value;
        }

        private static Bitmap ImageButtonClose
        {
            get
            {
                if (m_imageButtonClose == null)
                    m_imageButtonClose = Resources.TabExit;

                return m_imageButtonClose;
            }
        }

        private InertButton ButtonClose
        {
            get
            {
                if (m_buttonClose != null) return m_buttonClose;
                m_buttonClose = new InertButton(ImageButtonClose, ImageButtonClose);
                m_toolTip.SetToolTip(m_buttonClose, ToolTipClose);
                m_buttonClose.Click += Close_Click;
                Controls.Add(m_buttonClose);

                return m_buttonClose;
            }
        }

        private static Bitmap ImageButtonWindowList => m_imageButtonWindowList ?? (m_imageButtonWindowList = Resources.TabOption);

        private static Bitmap ImageButtonWindowListOverflow
        {
            get
            {
                if (m_imageButtonWindowListOverflow == null)
                    m_imageButtonWindowListOverflow = Resources.TabOverflow;

                return m_imageButtonWindowListOverflow;
            }
        }

        private InertButton ButtonWindowList
        {
            get
            {
                if (m_buttonWindowList == null)
                {
                    m_buttonWindowList = new InertButton(ImageButtonWindowList, ImageButtonWindowListOverflow);
                    m_toolTip.SetToolTip(m_buttonWindowList, ToolTipSelect);
                    m_buttonWindowList.Click += WindowList_Click;
                    Controls.Add(m_buttonWindowList);
                }

                return m_buttonWindowList;
            }
        }

        private static GraphicsPath GraphicsPath => MremoteNGAutoHideStrip.GraphicsPath;

        private IContainer Components => m_components;

        public Font TextFont => DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.TextFont;

        private Font BoldFont
        {
            get
            {
                if (IsDisposed)
                    return null;

                if (m_boldFont == null)
                {
                    m_font = TextFont;
                    m_boldFont = new Font(TextFont, FontStyle.Bold);
                }
                else if (m_font != TextFont)
                {
                    m_boldFont.Dispose();
                    m_font = TextFont;
                    m_boldFont = new Font(TextFont, FontStyle.Bold);
                }

                return m_boldFont;
            }
        }

        private int StartDisplayingTab
        {
            get => m_startDisplayingTab;
            set
            {
                m_startDisplayingTab = value;
                Invalidate();
            }
        }

        private int EndDisplayingTab
        {
            get => m_endDisplayingTab;
            set => m_endDisplayingTab = value;
        }

        private int FirstDisplayingTab
        {
            get => m_firstDisplayingTab;
            set => m_firstDisplayingTab = value;
        }

        private bool DocumentTabsOverflow
        {
            set
            {
                if (m_documentTabsOverflow == value)
                    return;

                m_documentTabsOverflow = value;
                if (value)
                    ButtonWindowList.ImageCategory = 1;
                else
                    ButtonWindowList.ImageCategory = 0;
            }
        }

        #region Customizable Properties

        private static int ToolWindowStripGapTop => _ToolWindowStripGapTop;

        private static int ToolWindowStripGapBottom => _ToolWindowStripGapBottom;

        private static int ToolWindowStripGapLeft => _ToolWindowStripGapLeft;

        private static int ToolWindowStripGapRight => _ToolWindowStripGapRight;

        private static int ToolWindowImageHeight => _ToolWindowImageHeight;

        private static int ToolWindowImageWidth => _ToolWindowImageWidth;

        private static int ToolWindowImageGapTop => _ToolWindowImageGapTop;

        private static int ToolWindowImageGapBottom => _ToolWindowImageGapBottom;

        private static int ToolWindowImageGapLeft => _ToolWindowImageGapLeft;

        private static int ToolWindowImageGapRight => _ToolWindowImageGapRight;

        private static int ToolWindowTextGapRight => _ToolWindowTextGapRight;

        private static int ToolWindowTabSeperatorGapTop => _ToolWindowTabSeperatorGapTop;

        private static int ToolWindowTabSeperatorGapBottom => _ToolWindowTabSeperatorGapBottom;

        private static string ToolTipClose => m_toolTipClose ?? (m_toolTipClose = Language.strRadioCloseWarnExit);

        private static string ToolTipSelect => m_toolTipSelect ?? (m_toolTipSelect = Language.strTabsAndPanels);

        private TextFormatFlags ToolWindowTextFormat
        {
            get
            {
                var textFormat = TextFormatFlags.EndEllipsis |
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.SingleLine |
                    TextFormatFlags.VerticalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                else
                    return textFormat;
            }
        }

        private static int DocumentStripGapTop => _DocumentStripGapTop;

        private static int DocumentStripGapBottom => _DocumentStripGapBottom;

        private TextFormatFlags DocumentTextFormat
        {
            get
            {
                var textFormat = TextFormatFlags.EndEllipsis |
                    TextFormatFlags.SingleLine |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.HorizontalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft;
                else
                    return textFormat;
            }
        }

        private static int DocumentTabMaxWidth => _DocumentTabMaxWidth;

        private static int DocumentButtonGapTop => _DocumentButtonGapTop;

        private static int DocumentButtonGapBottom => _DocumentButtonGapBottom;

        private static int DocumentButtonGapBetween => _DocumentButtonGapBetween;

        private static int DocumentButtonGapRight => _DocumentButtonGapRight;

        private static int DocumentTabGapTop => _DocumentTabGapTop;

        private static int DocumentTabGapLeft => _DocumentTabGapLeft;

        private static int DocumentTabGapRight => _DocumentTabGapRight;

        private static int DocumentIconGapBottom => _DocumentIconGapBottom;

        private static int DocumentIconGapLeft => _DocumentIconGapLeft;

        private static int DocumentIconGapRight => _DocumentIconGapRight;

        private static int DocumentIconWidth => _DocumentIconWidth;

        private static int DocumentIconHeight => _DocumentIconHeight;

        private static int DocumentTextGapRight => _DocumentTextGapRight;

        private static Pen PenToolWindowTabBorder => SystemPens.GrayText;

        private static Pen PenDocumentTabActiveBorder => SystemPens.ControlDarkDark;

        private static Pen PenDocumentTabInactiveBorder => SystemPens.GrayText;

        #endregion

        #endregion

        public DockPaneStripNG(DockPane pane)
            : base(pane)
        {
            SetStyle(ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            SuspendLayout();

             

            m_components = new System.ComponentModel.Container();
            m_toolTip = new ToolTip(Components);
            m_selectMenu = new ContextMenuStrip(Components);
            pane.DockPanel.Theme.ApplyTo(m_selectMenu); 

            ResumeLayout();
        } 
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Components.Dispose();
                if (m_boldFont != null)
                {
                    m_boldFont.Dispose();
                    m_boldFont = null;
                }
            }
            base.Dispose(disposing);
        }

        protected  override int MeasureHeight()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return MeasureHeight_ToolWindow();
            else
                return MeasureHeight_Document();
        }

        private int MeasureHeight_ToolWindow()
        {
            if (DockPane.IsAutoHide || Tabs.Count <= 1)
                return 0;

            var height = Math.Max(TextFont.Height + (PatchController.EnableHighDpi == true ? DocumentIconGapBottom : 0),
                ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom)
                + ToolWindowStripGapTop + ToolWindowStripGapBottom;

            return height;
        }

        private int MeasureHeight_Document()
        {
            var height = Math.Max(TextFont.Height + DocumentTabGapTop + (PatchController.EnableHighDpi == true ? DocumentIconGapBottom : 0),
                ButtonClose.Height + DocumentButtonGapTop + DocumentButtonGapBottom)
                + DocumentStripGapBottom + DocumentStripGapTop;

            return height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = TabsRectangle;
            var gradient = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient;
            if (Appearance == DockPane.AppearanceStyle.Document)
            {
                rect.X -= DocumentTabGapLeft;

                // Add these values back in so that the DockStrip color is drawn
                // beneath the close button and window list button.
                // It is possible depending on the DockPanel DocumentStyle to have
                // a Document without a DockStrip.
                rect.Width += DocumentTabGapLeft +
                    DocumentTabGapRight +
                    DocumentButtonGapRight +
                    ButtonClose.Width +
                    ButtonWindowList.Width;
            }
            else
            {
                gradient = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient;
            }
            //Fix MagicRemove , missing gradient implementation in themes
            //Also coloring in tabs in not correct in some themes
            var startColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Background");  
            var endColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Background");  
            var gradientMode = gradient.LinearGradientMode;

            DrawingRoutines.SafelyDrawLinearGradient(rect, startColor, endColor, gradientMode, e.Graphics);
            base.OnPaint(e);
            CalculateTabs();
            if (Appearance == DockPane.AppearanceStyle.Document && DockPane.ActiveContent != null)
            {
                if (EnsureDocumentTabVisible(DockPane.ActiveContent, false))
                    CalculateTabs();
            }

            DrawTabStrip(e.Graphics);
        }

        protected override void OnRefreshChanges()
        {
            SetInertButtons();
            Invalidate();
        }

        public override GraphicsPath GetOutline(int index)
        {

            if (Appearance == DockPane.AppearanceStyle.Document)
                return GetOutline_Document(index);
            else
                return GetOutline_ToolWindow(index);

        }

        private GraphicsPath GetOutline_Document(int index)
        {
            var rectangle = Tabs[index].Rectangle;
            if (rectangle == null) return null;
            var rectTab = rectangle.Value;
            rectTab.X -= rectTab.Height / 2;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            var path = new GraphicsPath();
            var pathTab = GetTabOutline_Document(Tabs[index], true, true, true);
            path.AddPath(pathTab, true);

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
            {
                path.AddLine(rectTab.Right, rectTab.Top, rectPaneClient.Right, rectTab.Top);
                path.AddLine(rectPaneClient.Right, rectTab.Top, rectPaneClient.Right, rectPaneClient.Top);
                path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Left, rectPaneClient.Top);
                path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Left, rectTab.Top);
                path.AddLine(rectPaneClient.Left, rectTab.Top, rectTab.Right, rectTab.Top);
            }
            else
            {
                path.AddLine(rectTab.Right, rectTab.Bottom, rectPaneClient.Right, rectTab.Bottom);
                path.AddLine(rectPaneClient.Right, rectTab.Bottom, rectPaneClient.Right, rectPaneClient.Bottom);
                path.AddLine(rectPaneClient.Right, rectPaneClient.Bottom, rectPaneClient.Left, rectPaneClient.Bottom);
                path.AddLine(rectPaneClient.Left, rectPaneClient.Bottom, rectPaneClient.Left, rectTab.Bottom);
                path.AddLine(rectPaneClient.Left, rectTab.Bottom, rectTab.Right, rectTab.Bottom);
            }
            return path;

        }

        private GraphicsPath GetOutline_ToolWindow(int index)
        {
            var rectangle = Tabs[index].Rectangle;
            if (rectangle == null) return null;
            var rectTab = rectangle.Value;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            var rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            var path = new GraphicsPath();
            var pathTab = GetTabOutline(Tabs[index], true, true);
            path.AddPath(pathTab, true);
            path.AddLine(rectTab.Left, rectTab.Top, rectPaneClient.Left, rectTab.Top);
            path.AddLine(rectPaneClient.Left, rectTab.Top, rectPaneClient.Left, rectPaneClient.Top);
            path.AddLine(rectPaneClient.Left, rectPaneClient.Top, rectPaneClient.Right, rectPaneClient.Top);
            path.AddLine(rectPaneClient.Right, rectPaneClient.Top, rectPaneClient.Right, rectTab.Top);
            path.AddLine(rectPaneClient.Right, rectTab.Top, rectTab.Right, rectTab.Top);
            return path;

        }

        private void CalculateTabs()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                CalculateTabs_ToolWindow();
            else
                CalculateTabs_Document();
        }

        private void CalculateTabs_ToolWindow()
        {
            if (Tabs.Count <= 1 || DockPane.IsAutoHide)
                return;

            var rectTabStrip = TabStripRectangle;

            // Calculate tab widths
            var countTabs = Tabs.Count;
            foreach (MremoteNGTab tab in Tabs)
            {
                tab.MaxWidth = GetMaxTabWidth(Tabs.IndexOf(tab));
                tab.Flag = false;
            }

            // Set tab whose max width less than average width
            var anyWidthWithinAverage = true;
            var totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight;
            var totalAllocatedWidth = 0;
            var averageWidth = totalWidth / countTabs;
            var remainedTabs = countTabs;
            for (anyWidthWithinAverage = true; anyWidthWithinAverage && remainedTabs > 0;)
            {
                anyWidthWithinAverage = false;
                foreach (MremoteNGTab tab in Tabs)
                {
                    if (tab.Flag)
                        continue;

                    if (tab.MaxWidth > averageWidth) continue;
                    tab.Flag = true;
                    tab.TabWidth = tab.MaxWidth;
                    totalAllocatedWidth += tab.TabWidth;
                    anyWidthWithinAverage = true;
                    remainedTabs--;
                }
                if (remainedTabs != 0)
                    averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs;
            }

            // If any tab width not set yet, set it to the average width
            if (remainedTabs > 0)
            {
                var roundUpWidth = totalWidth - totalAllocatedWidth - averageWidth * remainedTabs;
                foreach (MremoteNGTab tab in Tabs)
                {
                    if (tab.Flag)
                        continue;

                    tab.Flag = true;
                    if (roundUpWidth > 0)
                    {
                        tab.TabWidth = averageWidth + 1;
                        roundUpWidth--;
                    }
                    else
                        tab.TabWidth = averageWidth;
                }
            }

            // Set the X position of the tabs
            var x = rectTabStrip.X + ToolWindowStripGapLeft;
            foreach (MremoteNGTab tab in Tabs)
            {
                tab.TabX = x;
                x += tab.TabWidth;
            }
        }

        private bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
        {
            var overflow = false;

            if (!(Tabs[index] is MremoteNGTab tab)) return false;
            tab.MaxWidth = GetMaxTabWidth(index);
            var width = Math.Min(tab.MaxWidth, DocumentTabMaxWidth);
            if (x + width < rectTabStrip.Right || index == StartDisplayingTab)
            {
                tab.TabX = x;
                tab.TabWidth = width;
                EndDisplayingTab = index;
            }
            else
            {
                tab.TabX = 0;
                tab.TabWidth = 0;
                overflow = true;
            }

            x += width;

            return overflow;
        }

        /// <summary>
        /// Calculate which tabs are displayed and in what order.
        /// </summary>
        private void CalculateTabs_Document()
        {
            if (m_startDisplayingTab >= Tabs.Count)
                m_startDisplayingTab = 0;

            var rectTabStrip = TabsRectangle;

            var x = rectTabStrip.X + rectTabStrip.Height / 2;
            var overflow = false;

            // Originally all new documents that were considered overflow
            // (not enough pane strip space to show all tabs) were added to
            // the far left (assuming not right to left) and the tabs on the
            // right were dropped from view. If StartDisplayingTab is not 0
            // then we are dealing with making sure a specific tab is kept in focus.
            if (m_startDisplayingTab > 0)
            {
                var tempX = x;
                if (Tabs[m_startDisplayingTab] is MremoteNGTab tab) tab.MaxWidth = GetMaxTabWidth(m_startDisplayingTab);

                // Add the active tab and tabs to the left
                for (var i = StartDisplayingTab; i >= 0; i--)
                    CalculateDocumentTab(rectTabStrip, ref tempX, i);

                // Store which tab is the first one displayed so that it
                // will be drawn correctly (without part of the tab cut off)
                FirstDisplayingTab = EndDisplayingTab;

                tempX = x; // Reset X location because we are starting over

                // Start with the first tab displayed - name is a little misleading.
                // Loop through each tab and set its location. If there is not enough
                // room for all of them overflow will be returned.
                for (var i = EndDisplayingTab; i < Tabs.Count; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref tempX, i);

                // If not all tabs are shown then we have an overflow.
                if (FirstDisplayingTab != 0)
                    overflow = true;
            }
            else
            {
                for (var i = StartDisplayingTab; i < Tabs.Count; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);
                for (var i = 0; i < StartDisplayingTab; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);

                FirstDisplayingTab = StartDisplayingTab;
            }

            if (!overflow)
            {
                m_startDisplayingTab = 0;
                FirstDisplayingTab = 0;
                x = rectTabStrip.X + rectTabStrip.Height / 2;
                foreach (MremoteNGTab tab in Tabs)
                {
                    tab.TabX = x;
                    x += tab.TabWidth;
                }
            }
            DocumentTabsOverflow = overflow;
        }

        protected override void EnsureTabVisible(IDockContent content)
        {
            if (Appearance != DockPane.AppearanceStyle.Document || !Tabs.Contains(content))
                return;

            CalculateTabs();
            EnsureDocumentTabVisible(content, true);
        }

        private bool EnsureDocumentTabVisible(IDockContent content, bool repaint)
        {
            var index = Tabs.IndexOf(content);
            if (index == -1)
            {
                //somehow we've lost the content from the Tab collection
                return false;
            }

            if (Tabs[index] is MremoteNGTab tab && tab.TabWidth != 0)
                return false;

            StartDisplayingTab = index;
            if (repaint)
                Invalidate();

            return true;
        }

        private int GetMaxTabWidth(int index)
        {
            return Appearance == DockPane.AppearanceStyle.ToolWindow ? GetMaxTabWidth_ToolWindow(index) : GetMaxTabWidth_Document(index);
        }

        private int GetMaxTabWidth_ToolWindow(int index)
        {
            var content = Tabs[index].Content;
            var sizeString = TextRenderer.MeasureText(content.DockHandler.TabText, TextFont);
            return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft
                + ToolWindowImageGapRight + ToolWindowTextGapRight;
        }

        private int GetMaxTabWidth_Document(int index)
        {
            var content = Tabs[index].Content;

            var height = GetTabRectangle_Document(index).Height;

            var sizeText = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont, new Size(DocumentTabMaxWidth, height), DocumentTextFormat);

            return DockPane.DockPanel.ShowDocumentIcon
                ? sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight + DocumentTextGapRight
                : sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight;
        }

        private void DrawTabStrip(Graphics g)
        {
            if (Appearance == DockPane.AppearanceStyle.Document)
                DrawTabStrip_Document(g);
            else
                DrawTabStrip_ToolWindow(g);
        }

        private void DrawTabStrip_Document(Graphics g)
        {
            var count = Tabs.Count;
            if (count == 0)
                return;

            var rectTabStrip = TabStripRectangle;

            // Draw the tabs
            var rectTabOnly = TabsRectangle;
            Rectangle rectTab;
            MremoteNGTab tabActive = null;
            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            for (var i = 0; i < count; i++)
            {
                rectTab = GetTabRectangle(i);
                if (Tabs[i].Content == DockPane.ActiveContent)
                {
                    tabActive = Tabs[i] as MremoteNGTab;
                    if (tabActive != null) tabActive.Rectangle = rectTab;
                    continue;
                }

                if (!rectTab.IntersectsWith(rectTabOnly)) continue;
                if (!(Tabs[i] is MremoteNGTab tab)) continue;
                tab.Rectangle = rectTab;
                DrawTab(g, tab);
            }

            g.SetClip(rectTabStrip);

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                g.DrawLine(PenDocumentTabActiveBorder, rectTabStrip.Left, rectTabStrip.Top + 1,
                    rectTabStrip.Right, rectTabStrip.Top + 1);
            else
                g.DrawLine(PenDocumentTabActiveBorder, rectTabStrip.Left, rectTabStrip.Bottom - 1,
                    rectTabStrip.Right, rectTabStrip.Bottom - 1);

            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            if (tabActive == null) return;
            rectTab = tabActive.Rectangle.Value;
            if (!rectTab.IntersectsWith(rectTabOnly)) return;
            rectTab.Intersect(rectTabOnly);
            tabActive.Rectangle = rectTab;
            DrawTab(g, tabActive);
        }

        private void DrawTabStrip_ToolWindow(Graphics g)
        {
            var rectTabStrip = TabStripRectangle;

            g.DrawLine(PenToolWindowTabBorder, rectTabStrip.Left, rectTabStrip.Top,
                rectTabStrip.Right, rectTabStrip.Top);

            for (var i = 0; i < Tabs.Count; i++)
            {
                if (!(Tabs[i] is MremoteNGTab tab)) continue;
                tab.Rectangle = GetTabRectangle(i);
                DrawTab(g, tab);
            }
        }

        private Rectangle GetTabRectangle(int index)
        {
            return Appearance == DockPane.AppearanceStyle.ToolWindow ? GetTabRectangle_ToolWindow(index) : GetTabRectangle_Document(index);
        }

        private Rectangle GetTabRectangle_ToolWindow(int index)
        {
            var rectTabStrip = TabStripRectangle;

            var tab = (MremoteNGTab)Tabs[index];
            return new Rectangle(tab.TabX, rectTabStrip.Y, tab.TabWidth, rectTabStrip.Height);
        }

        private Rectangle GetTabRectangle_Document(int index)
        {
            var rectTabStrip = TabStripRectangle;
            var tab = (MremoteNGTab)Tabs[index];

            var rect = new Rectangle
            {
                X = tab.TabX, Width = tab.TabWidth, Height = rectTabStrip.Height - DocumentTabGapTop
            };

            if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                rect.Y = rectTabStrip.Y + DocumentStripGapBottom;
            else
                rect.Y = rectTabStrip.Y + DocumentTabGapTop;

            return rect;
        }

        private void DrawTab(Graphics g, MremoteNGTab tab)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                DrawTab_ToolWindow(g, tab);
            else
                DrawTab_Document(g, tab);
        }

        private GraphicsPath GetTabOutline(Tab tab, bool rtlTransform, bool toScreen)
        {
            return Appearance == DockPane.AppearanceStyle.ToolWindow ? GetTabOutline_ToolWindow(tab, rtlTransform, toScreen) : GetTabOutline_Document(tab, rtlTransform, toScreen, false);
        }

        private GraphicsPath GetTabOutline_ToolWindow(Tab tab, bool rtlTransform, bool toScreen)
        {
            var rect = GetTabRectangle(Tabs.IndexOf(tab));
            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            DrawHelper.GetRoundedCornerTab(GraphicsPath, rect, false);
            return GraphicsPath;
        }

        private GraphicsPath GetTabOutline_Document(Tab tab, bool rtlTransform, bool toScreen, bool full)
        {
            const int curveSize = 6;

            GraphicsPath.Reset();
            var rect = GetTabRectangle(Tabs.IndexOf(tab));

            // Shorten TabOutline so it doesn't get overdrawn by icons next to it
            rect.Intersect(TabsRectangle);
            rect.Width--;

            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            // Draws the full angle piece for active content (or first tab)
            if (tab.Content == DockPane.ActiveContent || full || Tabs.IndexOf(tab) == FirstDisplayingTab)
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        // For some reason the next line draws a line that is not hidden like it is when drawing the tab strip on top.
                        // It is not needed so it has been commented out.
                        //GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right + rect.Height / 2, rect.Bottom);
                        GraphicsPath.AddLine(rect.Right + rect.Height / 2, rect.Top, rect.Right - rect.Height / 2 + curveSize / 2, rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right + rect.Height / 2, rect.Bottom);
                        GraphicsPath.AddLine(rect.Right + rect.Height / 2, rect.Bottom, rect.Right - rect.Height / 2 + curveSize / 2, rect.Top + curveSize / 2);
                    }
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        // For some reason the next line draws a line that is not hidden like it is when drawing the tab strip on top.
                        // It is not needed so it has been commented out.
                        //GraphicsPath.AddLine(rect.Left, rect.Top, rect.Left - rect.Height / 2, rect.Top);
                        GraphicsPath.AddLine(rect.Left - rect.Height / 2, rect.Top, rect.Left + rect.Height / 2 - curveSize / 2, rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left - rect.Height / 2, rect.Bottom);
                        GraphicsPath.AddLine(rect.Left - rect.Height / 2, rect.Bottom, rect.Left + rect.Height / 2 - curveSize / 2, rect.Top + curveSize / 2);
                    }
                }
            }
            // Draws the partial angle for non-active content
            else
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Top, rect.Right, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Top + rect.Height / 2, rect.Right - rect.Height / 2 + curveSize / 2, rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - rect.Height / 2, rect.Right - rect.Height / 2 + curveSize / 2, rect.Top + curveSize / 2);
                    }
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Top, rect.Left, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Top + rect.Height / 2, rect.Left + rect.Height / 2 - curveSize / 2, rect.Bottom - curveSize / 2);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - rect.Height / 2, rect.Left + rect.Height / 2 - curveSize / 2, rect.Top + curveSize / 2);
                    }
                }
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                {
                    // Draws the bottom horizontal line (short side)
                    GraphicsPath.AddLine(rect.Right - rect.Height / 2 - curveSize / 2, rect.Bottom, rect.Left + curveSize / 2, rect.Bottom);

                    // Drawing the rounded corner is not necessary. The path is automatically connected
                    //GraphicsPath.AddArc(new Rectangle(rect.Left, rect.Top, curveSize, curveSize), 180, 90);
                }
                else
                {
                    // Draws the bottom horizontal line (short side)
                    GraphicsPath.AddLine(rect.Right - rect.Height / 2 - curveSize / 2, rect.Top, rect.Left + curveSize / 2, rect.Top);
                    GraphicsPath.AddArc(new Rectangle(rect.Left, rect.Top, curveSize, curveSize), 180, 90);
                }
            }
            else
            {
                if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                {
                    // Draws the bottom horizontal line (short side)
                    GraphicsPath.AddLine(rect.Left + rect.Height / 2 + curveSize / 2, rect.Bottom, rect.Right - curveSize / 2, rect.Bottom);

                    // Drawing the rounded corner is not necessary. The path is automatically connected
                    //GraphicsPath.AddArc(new Rectangle(rect.Right - curveSize, rect.Bottom, curveSize, curveSize), 90, -90);
                }
                else
                {
                    // Draws the top horizontal line (short side)
                    GraphicsPath.AddLine(rect.Left + rect.Height / 2 + curveSize / 2, rect.Top, rect.Right - curveSize / 2, rect.Top);

                    // Draws the rounded corner oppposite the angled side
                    GraphicsPath.AddArc(new Rectangle(rect.Right - curveSize, rect.Top, curveSize, curveSize), -90, 90);
                }
            }

            if (Tabs.IndexOf(tab) != EndDisplayingTab && Tabs.IndexOf(tab) != Tabs.Count - 1 && Tabs[Tabs.IndexOf(tab) + 1].Content == DockPane.ActiveContent && !full)
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - curveSize / 2, rect.Left, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - rect.Height / 2, rect.Left + rect.Height / 2, rect.Top);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Left, rect.Top + curveSize / 2, rect.Left, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Left, rect.Top + rect.Height / 2, rect.Left + rect.Height / 2, rect.Bottom);
                    }
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - curveSize / 2, rect.Right, rect.Bottom - rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - rect.Height / 2, rect.Right - rect.Height / 2, rect.Top);
                    }
                    else
                    {
                        GraphicsPath.AddLine(rect.Right, rect.Top + curveSize / 2, rect.Right, rect.Top + rect.Height / 2);
                        GraphicsPath.AddLine(rect.Right, rect.Top + rect.Height / 2, rect.Right - rect.Height / 2, rect.Bottom);
                    }
                }
            }
            else
            {
                // Draw the vertical line opposite the angled side
                if (RightToLeft == RightToLeft.Yes)
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                        GraphicsPath.AddLine(rect.Left, rect.Bottom - curveSize / 2, rect.Left, rect.Top);
                    else
                        GraphicsPath.AddLine(rect.Left, rect.Top + curveSize / 2, rect.Left, rect.Bottom);
                }
                else
                {
                    if (DockPane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                        GraphicsPath.AddLine(rect.Right, rect.Bottom - curveSize / 2, rect.Right, rect.Top);
                    else
                        GraphicsPath.AddLine(rect.Right, rect.Top + curveSize / 2, rect.Right, rect.Bottom);
                }
            }

            return GraphicsPath;
        }

        private void DrawTab_ToolWindow(Graphics g, MremoteNGTab tab)
        {
            if (tab.Rectangle == null) return;
            var rect = tab.Rectangle.Value;
            var rectIcon = new Rectangle(
                rect.X + ToolWindowImageGapLeft,
                rect.Y + rect.Height - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight,
                ToolWindowImageWidth, ToolWindowImageHeight);
            var rectText = PatchController.EnableHighDpi == true
                ? new Rectangle(
                    rect.X + ToolWindowImageGapLeft,
                    rect.Y - 1 + rect.Height - ToolWindowImageGapBottom - TextFont.Height,
                    ToolWindowImageWidth, TextFont.Height)
                : rectIcon;
            rectText.X += rectIcon.Width + ToolWindowImageGapRight;
            rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft -
                             ToolWindowImageGapRight - ToolWindowTextGapRight;

            var rectTab = DrawHelper.RtlTransform(this, rect);
            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);
            var path = GetTabOutline(tab, true, false);
            if (DockPane.ActiveContent == tab.Content)
            {
                var startColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor;
                var endColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor;
                var gradientMode = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectTab, startColor, endColor, gradientMode), path);
                g.DrawPath(PenToolWindowTabBorder, path);

                var textColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.TextColor;
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, ToolWindowTextFormat);
            }
            else
            {
                var startColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor;
                var endColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor;
                var gradientMode = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectTab, startColor, endColor, gradientMode), path);

                if (Tabs.IndexOf(DockPane.ActiveContent) != Tabs.IndexOf(tab) + 1)
                {
                    var pt1 = new Point(rect.Right, rect.Top + ToolWindowTabSeperatorGapTop);
                    var pt2 = new Point(rect.Right, rect.Bottom - ToolWindowTabSeperatorGapBottom);
                    g.DrawLine(PenToolWindowTabBorder, DrawHelper.RtlTransform(this, pt1), DrawHelper.RtlTransform(this, pt2));
                }

                var textColor = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.TextColor;
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, ToolWindowTextFormat);
            }

            if (rectTab.Contains(rectIcon))
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        private void DrawTab_Document(Graphics g, MremoteNGTab tab)
        {
            if (tab.Rectangle == null) return;
            var rect = tab.Rectangle.Value;
            if (tab.TabWidth == 0)
                return;

            var rectIcon = new Rectangle(
                rect.X + DocumentIconGapLeft,
                rect.Y + rect.Height - 1 - DocumentIconGapBottom - DocumentIconHeight,
                DocumentIconWidth, DocumentIconHeight);
            var rectText = PatchController.EnableHighDpi == true
                ? new Rectangle(
                    rect.X + DocumentIconGapLeft,
                    rect.Y + rect.Height - DocumentIconGapBottom - TextFont.Height,
                    DocumentIconWidth, TextFont.Height)
                : rectIcon;
            if (DockPane.DockPanel.ShowDocumentIcon)
            {
                rectText.X += rectIcon.Width + DocumentIconGapRight;
                rectText.Y = rect.Y;
                rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft -
                                 DocumentIconGapRight - DocumentTextGapRight;
                rectText.Height = rect.Height;
            }
            else
                rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight;

            var rectTab = DrawHelper.RtlTransform(this, rect);
            var rectBack = DrawHelper.RtlTransform(this, rect);
            rectBack.Width += DocumentIconGapLeft;
            rectBack.X -= DocumentIconGapLeft;

            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);
            var path = GetTabOutline(tab, true, false);
            if (DockPane.ActiveContent == tab.Content)
            {
                var startColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Background");
                var endColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Background");
                var gradientMode = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectBack, startColor, endColor, gradientMode), path);
                g.DrawPath(PenDocumentTabActiveBorder, path);

                var textColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Foreground");
                if (DockPane.IsActiveDocumentPane)
                    TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, BoldFont, rectText, textColor, DocumentTextFormat);
                else
                    TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, DocumentTextFormat);
            }
            else
            {
                var startColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Disabled_Background");
                var endColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Disabled_Background");
                var gradientMode = DockPane.DockPanel.Theme.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.LinearGradientMode;
                g.FillPath(new LinearGradientBrush(rectBack, startColor, endColor, gradientMode), path);
                g.DrawPath(PenDocumentTabInactiveBorder, path);

                var textColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Tab_Item_Disabled_Foreground");
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor, DocumentTextFormat);
            }

            if (rectTab.Contains(rectIcon) && DockPane.DockPanel.ShowDocumentIcon)
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        private void WindowList_Click(object sender, EventArgs e)
        {
            SelectMenu.Items.Clear();
            foreach (MremoteNGTab tab in Tabs)
            {
                var content = tab.Content;
                var item = SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap());
                item.Tag = tab.Content;
                item.Click += ContextMenuItem_Click;
            }

            var workingArea = Screen.GetWorkingArea(ButtonWindowList.PointToScreen(new Point(ButtonWindowList.Width / 2, ButtonWindowList.Height / 2)));
            var menu = new Rectangle(ButtonWindowList.PointToScreen(new Point(0, ButtonWindowList.Location.Y + ButtonWindowList.Height)), SelectMenu.Size);
            var menuMargined = new Rectangle(menu.X - SelectMenuMargin, menu.Y - SelectMenuMargin, menu.Width + SelectMenuMargin, menu.Height + SelectMenuMargin);
            if (workingArea.Contains(menuMargined))
            {
                SelectMenu.Show(menu.Location);
            }
            else
            {
                var newPoint = menu.Location;
                newPoint.X = DrawHelper.Balance(SelectMenu.Width, SelectMenuMargin, newPoint.X, workingArea.Left, workingArea.Right);
                newPoint.Y = DrawHelper.Balance(SelectMenu.Size.Height, SelectMenuMargin, newPoint.Y, workingArea.Top, workingArea.Bottom);
                var button = ButtonWindowList.PointToScreen(new Point(0, ButtonWindowList.Height));
                if (newPoint.Y < button.Y)
                {
                    // flip the menu up to be above the button.
                    newPoint.Y = button.Y - ButtonWindowList.Height;
                    SelectMenu.Show(newPoint, ToolStripDropDownDirection.AboveRight);
                }
                else
                {
                    SelectMenu.Show(newPoint);
                }
            }
        }

        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem item)) return;
            var content = (IDockContent)item.Tag;
            DockPane.ActiveContent = content;
        }

        private void SetInertButtons()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                if (m_buttonClose != null)
                    m_buttonClose.Left = -m_buttonClose.Width;

                if (m_buttonWindowList != null)
                    m_buttonWindowList.Left = -m_buttonWindowList.Width;
            }
            else
            {
                ButtonClose.Enabled = DockPane.ActiveContent == null || DockPane.ActiveContent.DockHandler.CloseButton;
                m_closeButtonVisible = DockPane.ActiveContent == null || DockPane.ActiveContent.DockHandler.CloseButtonVisible;
                ButtonClose.Visible = m_closeButtonVisible;
                ButtonClose.RefreshChanges();
                ButtonWindowList.RefreshChanges();
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (Appearance == DockPane.AppearanceStyle.Document)
            {
                LayoutButtons();
                OnRefreshChanges();
            }

            base.OnLayout(levent);
        }

        private void LayoutButtons()
        {
            var rectTabStrip = TabStripRectangle;

            // Set position and size of the buttons
            var buttonWidth = ButtonClose.Image.Width;
            var buttonHeight = ButtonClose.Image.Height;
            var height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom;
            if (buttonHeight < height)
            {
                buttonWidth = buttonWidth * height / buttonHeight;
                buttonHeight = height;
            }
            var buttonSize = new Size(buttonWidth, buttonHeight);

            var x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft
                - DocumentButtonGapRight - buttonWidth;
            var y = rectTabStrip.Y + DocumentButtonGapTop;
            var point = new Point(x, y);
            ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

            // If the close button is not visible draw the window list button overtop.
            // Otherwise it is drawn to the left of the close button.
            if (m_closeButtonVisible)
                point.Offset(-(DocumentButtonGapBetween + buttonWidth), 0);

            ButtonWindowList.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
        }

        private void Close_Click(object sender, EventArgs e)
        {
            DockPane.CloseActiveContent();
            if (PatchController.EnableMemoryLeakFix == true)
            {
                ContentClosed();
            }
        }

        protected  override int HitTest(Point point)
        {
            if (!TabsRectangle.Contains(point))
                return -1;

            foreach (var tab in Tabs)
            {
                var path = GetTabOutline(tab, true, false);
                if (path.IsVisible(point))
                    return Tabs.IndexOf(tab);
            }
            return -1;
        }

        protected override Rectangle GetTabBounds(Tab tab)
        {
            var path = GetTabOutline(tab, true, false);
            var rectangle = path.GetBounds();
            return new Rectangle((int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Width, (int)rectangle.Height);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            var index = HitTest(PointToClient(MousePosition));
            var toolTip = string.Empty;

            base.OnMouseHover(e);

            if (index != -1)
            {
                if (!(Tabs[index] is MremoteNGTab tab)) return;
                if (!string.IsNullOrEmpty(tab.Content.DockHandler.ToolTipText))
                    toolTip = tab.Content.DockHandler.ToolTipText;
                else if (tab.MaxWidth > tab.TabWidth)
                    toolTip = tab.Content.DockHandler.TabText;
            }

            if (m_toolTip.GetToolTip(this) != toolTip)
            {
                m_toolTip.Active = false;
                m_toolTip.SetToolTip(this, toolTip);
                m_toolTip.Active = true;
            }

            // requires further tracking of mouse hover behavior,
            ResetMouseEventArgs();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            PerformLayout();
        }
    }
}
