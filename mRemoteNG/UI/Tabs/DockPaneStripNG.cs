using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Properties;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Tabs
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// This class is lifted from VS2013DockPaneStrip from DockPanelSuite and customized for MremoteNG
    /// </summary>
    [ToolboxItem(false)]
    internal class DockPaneStripNG : DockPaneStripBase
    {
        private class MremoteNGTab(IDockContent content) : Tab(content)
        {
            public int TabX { get; set; }

            public int TabWidth { get; set; }

            public int MaxWidth { get; set; }

            protected internal bool Flag { get; set; }
        }

        protected override Tab CreateTab(IDockContent content)
        {
            return new MremoteNGTab(content);
        }

        [ToolboxItem(false)]
        private sealed class InertButton(Bitmap hovered, Bitmap normal, Bitmap pressed) : InertButtonBase
        {
            public override Bitmap Image { get; } = normal;

            public override Bitmap HoverImage { get; } = hovered;

            public override Bitmap PressImage { get; } = pressed;
        }

        #region Constants

        private const int _ToolWindowStripGapTop = 0;
        private const int _ToolWindowStripGapBottom = 0;
        private const int _ToolWindowStripGapLeft = 0;
        private const int _ToolWindowStripGapRight = 0;
        private const int _ToolWindowImageHeight = 16;
        private const int _ToolWindowImageWidth = 0; //16;
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
        private const int _DocumentButtonGapTop = 3;
        private const int _DocumentButtonGapBottom = 3;
        private const int _DocumentButtonGapBetween = 0;
        private const int _DocumentButtonGapRight = 3;
        private const int _DocumentTabGapTop = 0; //3;
        private const int _DocumentTabGapLeft = 0; //3;
        private const int _DocumentTabGapRight = 0; //3;
        private const int _DocumentIconGapBottom = 2; //2;
        private const int _DocumentIconGapLeft = 8;
        private const int _DocumentIconGapRight = 0;
        private const int _DocumentIconHeight = 16;
        private const int _DocumentIconWidth = 16;
        private const int _DocumentTextGapRight = 6;

        #endregion

        #region Members

        private InertButton m_buttonOverflow;
        private InertButton m_buttonWindowList;
        private ToolTip m_toolTip;
        private Font m_font;
        private Font m_boldFont;
        private int m_startDisplayingTab;
        private bool m_documentTabsOverflow;
        private static string m_toolTipSelect;
        private bool m_suspendDrag;

        #endregion

        #region Properties

        private Rectangle TabStripRectangle =>
            Appearance == DockPane.AppearanceStyle.Document
                ? TabStripRectangle_Document
                : TabStripRectangle_ToolWindow;

        private Rectangle TabStripRectangle_ToolWindow
        {
            get
            {
                Rectangle rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + ToolWindowStripGapTop, rect.Width,
                                     rect.Height - ToolWindowStripGapTop - ToolWindowStripGapBottom);
            }
        }

        private Rectangle TabStripRectangle_Document
        {
            get
            {
                Rectangle rect = ClientRectangle;
                return new Rectangle(rect.X, rect.Top + DocumentStripGapTop, rect.Width,
                                     rect.Height + DocumentStripGapTop - DocumentStripGapBottom);
            }
        }

        private Rectangle TabsRectangle
        {
            get
            {
                if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                    return TabStripRectangle;

                Rectangle rectWindow = TabStripRectangle;
                int x = rectWindow.X;
                int y = rectWindow.Y;
                int width = rectWindow.Width;
                int height = rectWindow.Height;

                x += DocumentTabGapLeft;
                width -= DocumentTabGapLeft +
                         DocumentTabGapRight +
                         DocumentButtonGapRight +
                         ButtonOverflow.Width +
                         ButtonWindowList.Width +
                         2 * DocumentButtonGapBetween;

                return new Rectangle(x, y, width, height);
            }
        }

        private ContextMenuStrip SelectMenu { get; }

        public int SelectMenuMargin { get; set; } = 5;

        private InertButton ButtonOverflow
        {
            get
            {
                if (m_buttonOverflow != null) return m_buttonOverflow;
                m_buttonOverflow = new InertButton(
                                                   DockPane.DockPanel.Theme.ImageService.DockPaneHover_OptionOverflow,
                                                   DockPane.DockPanel.Theme.ImageService.DockPane_OptionOverflow,
                                                   DockPane.DockPanel.Theme.ImageService.DockPanePress_OptionOverflow);
                m_buttonOverflow.Click += WindowList_Click;
                Controls.Add(m_buttonOverflow);

                return m_buttonOverflow;
            }
        }

        private InertButton ButtonWindowList
        {
            get
            {
                if (m_buttonWindowList != null) return m_buttonWindowList;
                m_buttonWindowList = new InertButton(
                                                     DockPane.DockPanel.Theme.ImageService.DockPaneHover_List,
                                                     DockPane.DockPanel.Theme.ImageService.DockPane_List,
                                                     DockPane.DockPanel.Theme.ImageService.DockPanePress_List);
                m_buttonWindowList.Click += WindowList_Click;
                Controls.Add(m_buttonWindowList);

                return m_buttonWindowList;
            }
        }

        private static GraphicsPath GraphicsPath => MremoteNGAutoHideStrip.GraphicsPath;

        private IContainer Components { get; }

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
                else if (!Equals(m_font, TextFont))
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

        private int EndDisplayingTab { get; set; }

        private int FirstDisplayingTab { get; set; }

        private bool DocumentTabsOverflow
        {
            set
            {
                if (m_documentTabsOverflow == value)
                    return;

                m_documentTabsOverflow = value;
                SetInertButtons();
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

        private static string ToolTipSelect => m_toolTipSelect ?? (m_toolTipSelect = Language.TabsAndPanels);

        private TextFormatFlags ToolWindowTextFormat
        {
            get
            {
                const TextFormatFlags textFormat = TextFormatFlags.EndEllipsis |
                                                   TextFormatFlags.HorizontalCenter |
                                                   TextFormatFlags.SingleLine |
                                                   TextFormatFlags.VerticalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                return textFormat;
            }
        }

        private static int DocumentStripGapTop => _DocumentStripGapTop;

        private static int DocumentStripGapBottom => _DocumentStripGapBottom;

        private TextFormatFlags DocumentTextFormat
        {
            get
            {
                const TextFormatFlags textFormat = TextFormatFlags.EndEllipsis |
                                                   TextFormatFlags.SingleLine |
                                                   TextFormatFlags.VerticalCenter |
                                                   TextFormatFlags.HorizontalCenter;
                if (RightToLeft == RightToLeft.Yes)
                    return textFormat | TextFormatFlags.RightToLeft;
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

            Components = new System.ComponentModel.Container();
            m_toolTip = new ToolTip(Components);
            SelectMenu = new ContextMenuStrip(Components);
            pane.DockPanel.Theme.ApplyTo(SelectMenu);

            ResumeLayout();
        }

        // This seems like a bogus warning - suppressing since Components is being disposed...
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<Components>k__BackingField")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(Components != null)
                    Components.Dispose();

                if (m_boldFont != null)
                {
                    m_boldFont.Dispose();
                    m_boldFont = null;
                }

            }

            base.Dispose(disposing);
        }

        protected override int MeasureHeight()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return MeasureHeight_ToolWindow();
            return MeasureHeight_Document();
        }

        private int MeasureHeight_ToolWindow()
        {
            if (DockPane.IsAutoHide || Tabs.Count <= 1)
                return 0;

            int height = Math.Max(TextFont.Height + (PatchController.EnableHighDpi == true ? DocumentIconGapBottom : 0),
                                  ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom)
                       + ToolWindowStripGapTop + ToolWindowStripGapBottom;

            return height;
        }

        private int MeasureHeight_Document()
        {
            int height =
                Math.Max(
                         TextFont.Height + DocumentTabGapTop +
                         (PatchController.EnableHighDpi == true ? DocumentIconGapBottom : 0),
                         ButtonOverflow.Height + DocumentButtonGapTop + DocumentButtonGapBottom)
              + DocumentStripGapBottom + DocumentStripGapTop;

            return height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
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
            return Appearance == DockPane.AppearanceStyle.Document
                ? GetOutline_Document(index)
                : GetOutline_ToolWindow(index);
        }

        private GraphicsPath GetOutline_Document(int index)
        {
            Rectangle? rectangle = Tabs[index].Rectangle;
            if (rectangle == null) return null;
            Rectangle rectTab = rectangle.Value;
            rectTab.X -= rectTab.Height / 2;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            Rectangle rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            GraphicsPath path = new();
            GraphicsPath pathTab = GetTabOutline_Document(Tabs[index], true, true, true);
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
            Rectangle? rectangle = Tabs[index].Rectangle;
            if (rectangle == null) return null;
            Rectangle rectTab = rectangle.Value;
            rectTab.Intersect(TabsRectangle);
            rectTab = RectangleToScreen(DrawHelper.RtlTransform(this, rectTab));
            Rectangle rectPaneClient = DockPane.RectangleToScreen(DockPane.ClientRectangle);

            GraphicsPath path = new();
            GraphicsPath pathTab = GetTabOutline(Tabs[index], true, true);
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

            Rectangle rectTabStrip = TabStripRectangle;

            // Calculate tab widths
            int countTabs = Tabs.Count;
            foreach (Tab tab1 in Tabs)
            {
                MremoteNGTab tab = (MremoteNGTab)tab1;
                tab.MaxWidth = GetMaxTabWidth(Tabs.IndexOf(tab));
                tab.Flag = false;
            }

            // Set tab whose max width less than average width
            bool anyWidthWithinAverage;
            int totalWidth = rectTabStrip.Width - ToolWindowStripGapLeft - ToolWindowStripGapRight;
            int totalAllocatedWidth = 0;
            int averageWidth = totalWidth / countTabs;
            int remainedTabs = countTabs;
            for (anyWidthWithinAverage = true; anyWidthWithinAverage && remainedTabs > 0;)
            {
                anyWidthWithinAverage = false;
                foreach (Tab tab1 in Tabs)
                {
                    MremoteNGTab tab = (MremoteNGTab)tab1;
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
                int roundUpWidth = (totalWidth - totalAllocatedWidth) - (averageWidth * remainedTabs);
                foreach (Tab tab1 in Tabs)
                {
                    MremoteNGTab tab = (MremoteNGTab)tab1;
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
            int x = rectTabStrip.X + ToolWindowStripGapLeft;
            foreach (Tab tab1 in Tabs)
            {
                MremoteNGTab tab = (MremoteNGTab)tab1;
                tab.TabX = x;
                x += tab.TabWidth;
            }
        }

        private bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
        {
            bool overflow = false;

            if (!(Tabs[index] is MremoteNGTab tab)) return false;
            tab.MaxWidth = GetMaxTabWidth(index);
            int width = Math.Min(tab.MaxWidth, DocumentTabMaxWidth);
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

            Rectangle rectTabStrip = TabsRectangle;

            int x = rectTabStrip.X; //+ rectTabStrip.Height / 2;
            bool overflow = false;

            // Originally all new documents that were considered overflow
            // (not enough pane strip space to show all tabs) were added to
            // the far left (assuming not right to left) and the tabs on the
            // right were dropped from view. If StartDisplayingTab is not 0
            // then we are dealing with making sure a specific tab is kept in focus.
            if (m_startDisplayingTab > 0)
            {
                int tempX = x;
                if (Tabs[m_startDisplayingTab] is MremoteNGTab tab)
                    tab.MaxWidth = GetMaxTabWidth(m_startDisplayingTab);

                // Add the active tab and tabs to the left
                for (int i = StartDisplayingTab; i >= 0; i--)
                    CalculateDocumentTab(rectTabStrip, ref tempX, i);

                // Store which tab is the first one displayed so that it
                // will be drawn correctly (without part of the tab cut off)
                FirstDisplayingTab = EndDisplayingTab;

                tempX = x; // Reset X location because we are starting over

                // Start with the first tab displayed - name is a little misleading.
                // Loop through each tab and set its location. If there is not enough
                // room for all of them overflow will be returned.
                for (int i = EndDisplayingTab; i < Tabs.Count; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref tempX, i);

                // If not all tabs are shown then we have an overflow.
                if (FirstDisplayingTab != 0)
                    overflow = true;
            }
            else
            {
                for (int i = StartDisplayingTab; i < Tabs.Count; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);
                for (int i = 0; i < StartDisplayingTab; i++)
                    overflow = CalculateDocumentTab(rectTabStrip, ref x, i);

                FirstDisplayingTab = StartDisplayingTab;
            }

            if (!overflow)
            {
                m_startDisplayingTab = 0;
                FirstDisplayingTab = 0;
                x = rectTabStrip.X;
                foreach (Tab tab1 in Tabs)
                {
                    MremoteNGTab tab = (MremoteNGTab)tab1;
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
            int index = Tabs.IndexOf(content);
            if (index == -1) // TODO: should prevent it from being -1;
                return false;

            if (Tabs[index] is MremoteNGTab tab && tab.TabWidth != 0)
                return false;

            StartDisplayingTab = index;
            if (repaint)
                Invalidate();

            return true;
        }

        private int GetMaxTabWidth(int index)
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
                return GetMaxTabWidth_ToolWindow(index);
            return GetMaxTabWidth_Document(index);
        }

        private int GetMaxTabWidth_ToolWindow(int index)
        {
            IDockContent content = Tabs[index].Content;
            Size sizeString = TextRenderer.MeasureText(content.DockHandler.TabText, TextFont);
            return ToolWindowImageWidth + sizeString.Width + ToolWindowImageGapLeft
                 + ToolWindowImageGapRight + ToolWindowTextGapRight;
        }

        private const int TAB_CLOSE_BUTTON_WIDTH = 30;

        private int GetMaxTabWidth_Document(int index)
        {
            IDockContent content = Tabs[index].Content;
            int height = GetTabRectangle_Document(index).Height;
            Size sizeText = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont,
                                                    new Size(DocumentTabMaxWidth, height), DocumentTextFormat);

            int width;
            if (DockPane.DockPanel.ShowDocumentIcon)
                width = sizeText.Width + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight +
                        DocumentTextGapRight;
            else
                width = sizeText.Width + DocumentIconGapLeft + DocumentTextGapRight;

            width += TAB_CLOSE_BUTTON_WIDTH;
            return width;
        }

        private void DrawTabStrip(Graphics g)
        {
            // IMPORTANT: fill background.
            Rectangle rectTabStrip = TabStripRectangle;
            g.FillRectangle(
                            DockPane.DockPanel.Theme.PaintingService.GetBrush(DockPane.DockPanel.Theme.ColorPalette
                                                                                      .MainWindowActive
                                                                                      .Background), rectTabStrip);

            if (Appearance == DockPane.AppearanceStyle.Document)
                DrawTabStrip_Document(g);
            else
                DrawTabStrip_ToolWindow(g);
        }

        private void DrawTabStrip_Document(Graphics g)
        {
            int count = Tabs.Count;
            if (count == 0)
                return;

            Rectangle rectTabStrip = new(TabStripRectangle.Location, TabStripRectangle.Size);
            rectTabStrip.Height += 1;

            // Draw the tabs
            Rectangle rectTabOnly = TabsRectangle;
            Rectangle rectTab;
            MremoteNGTab tabActive = null;
            g.SetClip(DrawHelper.RtlTransform(this, rectTabOnly));
            for (int i = 0; i < count; i++)
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

            if (DockPane.DockPanel.DocumentTabStripLocation != DocumentTabStripLocation.Bottom)
            {
                Color tabUnderLineColor;
                if (tabActive != null && DockPane.IsActiveDocumentPane)
                    tabUnderLineColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Background;
                else
                    tabUnderLineColor = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background;

                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(tabUnderLineColor, 4), rectTabStrip.Left,
                           rectTabStrip.Bottom, rectTabStrip.Right, rectTabStrip.Bottom);
            }

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
            Rectangle rect = TabStripRectangle_ToolWindow;
            Color borderColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowBorder;

            g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Top,
                       rect.Right, rect.Top);

            for (int i = 0; i < Tabs.Count; i++)
            {
                if (!(Tabs[i] is MremoteNGTab tab)) continue;
                tab.Rectangle = GetTabRectangle(i);
                DrawTab(g, tab);
            }
        }

        private Rectangle GetTabRectangle(int index)
        {
            return Appearance == DockPane.AppearanceStyle.ToolWindow
                ? GetTabRectangle_ToolWindow(index)
                : GetTabRectangle_Document(index);
        }

        private Rectangle GetTabRectangle_ToolWindow(int index)
        {
            Rectangle rectTabStrip = TabStripRectangle;

            MremoteNGTab tab = (MremoteNGTab)Tabs[index];
            return new Rectangle(tab.TabX, rectTabStrip.Y, tab.TabWidth, rectTabStrip.Height);
        }

        private Rectangle GetTabRectangle_Document(int index)
        {
            Rectangle rectTabStrip = TabStripRectangle;
            MremoteNGTab tab = (MremoteNGTab)Tabs[index];

            Rectangle rect = new()
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
            return Appearance == DockPane.AppearanceStyle.ToolWindow
                ? GetTabOutline_ToolWindow(tab, rtlTransform, toScreen)
                : GetTabOutline_Document(tab, rtlTransform, toScreen, false);
        }

        private GraphicsPath GetTabOutline_ToolWindow(Tab tab, bool rtlTransform, bool toScreen)
        {
            Rectangle rect = GetTabRectangle(Tabs.IndexOf(tab));
            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            DrawHelper.GetRoundedCornerTab(GraphicsPath, rect, false);
            return GraphicsPath;
        }

        private GraphicsPath GetTabOutline_Document(Tab tab, bool rtlTransform, bool toScreen, bool full)
        {
            GraphicsPath.Reset();
            Rectangle rect = GetTabRectangle(Tabs.IndexOf(tab));

            // Shorten TabOutline so it doesn't get overdrawn by icons next to it
            rect.Intersect(TabsRectangle);
            rect.Width--;

            if (rtlTransform)
                rect = DrawHelper.RtlTransform(this, rect);
            if (toScreen)
                rect = RectangleToScreen(rect);

            GraphicsPath.AddRectangle(rect);
            return GraphicsPath;
        }

        private void DrawTab_ToolWindow(Graphics g, MremoteNGTab tab)
        {
            if (tab.Rectangle == null) return;
            Rectangle rect = tab.Rectangle.Value;
            Rectangle rectIcon = new(
                                         rect.X + ToolWindowImageGapLeft,
                                         rect.Y + rect.Height - ToolWindowImageGapBottom - ToolWindowImageHeight,
                                         ToolWindowImageWidth, ToolWindowImageHeight);
            Rectangle rectText = PatchController.EnableHighDpi == true
                ? new Rectangle(
                                rect.X + ToolWindowImageGapLeft,
                                rect.Y + rect.Height - ToolWindowImageGapBottom - TextFont.Height,
                                ToolWindowImageWidth, TextFont.Height)
                : rectIcon;
            rectText.X += rectIcon.Width + ToolWindowImageGapRight;
            rectText.Width = rect.Width - rectIcon.Width - ToolWindowImageGapLeft -
                             ToolWindowImageGapRight - ToolWindowTextGapRight;

            Rectangle rectTab = DrawHelper.RtlTransform(this, rect);
            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);
            Color borderColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowBorder;

            Color separatorColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowSeparator;
            if (DockPane.ActiveContent == tab.Content)
            {
                Color textColor;
                Color backgroundColor;
                if (DockPane.IsActiveDocumentPane)
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedActive.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedActive.Background;
                }
                else
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedInactive.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabSelectedInactive.Background;
                }

                g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rect);
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Top,
                           rect.Left, rect.Bottom);
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Bottom - 1,
                           rect.Right, rect.Bottom - 1);
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Right - 1, rect.Top,
                           rect.Right - 1, rect.Bottom);
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor,
                                      ToolWindowTextFormat);
            }
            else
            {
                Color textColor;
                Color backgroundColor;
                if (tab.Content == DockPane.MouseOverTab)
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselectedHovered.Background;
                }
                else
                {
                    textColor = DockPane.DockPanel.Theme.ColorPalette.ToolWindowTabUnselected.Text;
                    backgroundColor = DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background;
                }

                g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(backgroundColor), rect);
                g.DrawLine(DockPane.DockPanel.Theme.PaintingService.GetPen(borderColor), rect.Left, rect.Top,
                           rect.Right, rect.Top);
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, textColor,
                                      ToolWindowTextFormat);
            }

            if (rectTab.Contains(rectIcon))
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        private void DrawTab_Document(Graphics g, MremoteNGTab tab)
        {
            if (tab.Rectangle == null) return;
            Rectangle rect = tab.Rectangle.Value;
            if (tab.TabWidth == 0)
                return;

            Rectangle rectCloseButton = GetCloseButtonRect(rect);
            Rectangle rectIcon = new(
                                         rect.X + DocumentIconGapLeft,
                                         rect.Y + rect.Height - DocumentIconGapBottom - DocumentIconHeight,
                                         DocumentIconWidth, DocumentIconHeight);
            Rectangle rectText = PatchController.EnableHighDpi == true
                ? new Rectangle(
                                rect.X + DocumentIconGapLeft,
                                rect.Y + rect.Height - DocumentIconGapBottom - TextFont.Height,
                                DocumentIconWidth, TextFont.Height)
                : rectIcon;
            if (DockPane.DockPanel.ShowDocumentIcon)
            {
                rectText.X += rectIcon.Width + DocumentIconGapRight;
                rectText.Y = rect.Y;
                rectText.Width = rect.Width - rectIcon.Width - DocumentIconGapLeft - DocumentIconGapRight -
                                 DocumentTextGapRight - rectCloseButton.Width;
                rectText.Height = rect.Height;
            }
            else
                rectText.Width = rect.Width - DocumentIconGapLeft - DocumentTextGapRight - rectCloseButton.Width;

            Rectangle rectTab = DrawHelper.RtlTransform(this, rect);
            Rectangle rectBack = DrawHelper.RtlTransform(this, rect);
            rectBack.Width += DocumentIconGapLeft;
            rectBack.X -= DocumentIconGapLeft;

            rectText = DrawHelper.RtlTransform(this, rectText);
            rectIcon = DrawHelper.RtlTransform(this, rectIcon);

            // Get custom tab color if available
            Color? customTabColor = GetCustomTabColor(tab.Content);

            Color activeColor = customTabColor ?? DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Background;
            Color lostFocusColor = customTabColor ?? DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Background;
            Color inactiveColor = DockPane.DockPanel.Theme.ColorPalette.MainWindowActive.Background;
            Color mouseHoverColor = DockPane.DockPanel.Theme.ColorPalette.TabUnselectedHovered.Background;

            Color activeText = DockPane.DockPanel.Theme.ColorPalette.TabSelectedActive.Text;
            Color lostFocusText = DockPane.DockPanel.Theme.ColorPalette.TabSelectedInactive.Text;
            Color inactiveText = DockPane.DockPanel.Theme.ColorPalette.TabUnselected.Text;
            Color mouseHoverText = DockPane.DockPanel.Theme.ColorPalette.TabUnselectedHovered.Text;

            Color text;
            Image image = null;
            Color paint;
            IImageService imageService = DockPane.DockPanel.Theme.ImageService;
            if (DockPane.ActiveContent == tab.Content)
            {
                if (DockPane.IsActiveDocumentPane)
                {
                    paint = activeColor;
                    text = activeText;
                    image = IsMouseDown
                        ? imageService.TabPressActive_Close
                        : rectCloseButton == ActiveClose
                            ? imageService.TabHoverActive_Close
                            : imageService.TabActive_Close;
                }
                else
                {
                    paint = lostFocusColor;
                    text = lostFocusText;
                    image = IsMouseDown
                        ? imageService.TabPressLostFocus_Close
                        : rectCloseButton == ActiveClose
                            ? imageService.TabHoverLostFocus_Close
                            : imageService.TabLostFocus_Close;
                }
            }
            else
            {
                if (tab.Content == DockPane.MouseOverTab)
                {
                    paint = mouseHoverColor;
                    text = mouseHoverText;
                    image = IsMouseDown
                        ? imageService.TabPressInactive_Close
                        : rectCloseButton == ActiveClose
                            ? imageService.TabHoverInactive_Close
                            : imageService.TabInactive_Close;
                }
                else
                {
                    paint = inactiveColor;
                    text = inactiveText;
                }
            }

            g.FillRectangle(DockPane.DockPanel.Theme.PaintingService.GetBrush(paint), rect);
            TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, text, DocumentTextFormat);
            if (image != null)
                g.DrawImage(image, rectCloseButton);

            if (rectTab.Contains(rectIcon) && DockPane.DockPanel.ShowDocumentIcon)
                g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
        }

        private Color? GetCustomTabColor(IDockContent content)
        {
            try
            {
                if (content is ConnectionTab connectionTab)
                {
                    InterfaceControl interfaceControl = InterfaceControl.FindInterfaceControl(connectionTab);
                    if (interfaceControl?.Info != null)
                    {
                        string tabColorStr = interfaceControl.Info.TabColor;
                        if (!string.IsNullOrEmpty(tabColorStr))
                        {
                            ColorConverter converter = new ColorConverter();
                            return (Color)converter.ConvertFromString(tabColorStr);
                        }
                    }
                }
            }
            catch
            {
                // If there's any error parsing the color, just return null to use default
            }
            return null;
        }

        private bool m_isMouseDown;

        protected bool IsMouseDown
        {
            get => m_isMouseDown;
            private set
            {
                if (m_isMouseDown == value)
                    return;

                m_isMouseDown = value;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseDown)
                IsMouseDown = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            // suspend drag if mouse is down on active close button.
            m_suspendDrag = ActiveCloseHitTest(e.Location);
            if (!IsMouseDown)
                IsMouseDown = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!m_suspendDrag)
                base.OnMouseMove(e);

            int index = HitTest(PointToClient(MousePosition));
            string toolTip = string.Empty;

            bool tabUpdate = false;
            bool buttonUpdate = false;
            if (index != -1)
            {
                if (Tabs[index] is MremoteNGTab tab)
                {
                    if (Appearance == DockPane.AppearanceStyle.ToolWindow ||
                        Appearance == DockPane.AppearanceStyle.Document)
                    {
                        tabUpdate = SetMouseOverTab(tab.Content == DockPane.ActiveContent ? null : tab.Content);
                    }

                    if (!string.IsNullOrEmpty(tab.Content.DockHandler.ToolTipText))
                        toolTip = tab.Content.DockHandler.ToolTipText;
                    else if (tab.MaxWidth > tab.TabWidth)
                        toolTip = tab.Content.DockHandler.TabText;

                    Point mousePos = PointToClient(MousePosition);
                    if (tab.Rectangle != null)
                    {
                        Rectangle tabRect = tab.Rectangle.Value;
                        Rectangle closeButtonRect = GetCloseButtonRect(tabRect);
                        Rectangle mouseRect = new(mousePos, new Size(1, 1));
                        buttonUpdate = SetActiveClose(closeButtonRect.IntersectsWith(mouseRect)
                                                          ? closeButtonRect
                                                          : Rectangle.Empty);
                    }
                }
            }
            else
            {
                tabUpdate = SetMouseOverTab(null);
                buttonUpdate = SetActiveClose(Rectangle.Empty);
            }

            if (tabUpdate || buttonUpdate)
                Invalidate();

            if (m_toolTip.GetToolTip(this) == toolTip) return;
            m_toolTip.Active = false;
            m_toolTip.SetToolTip(this, toolTip);
            m_toolTip.Active = true;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button != MouseButtons.Left || Appearance != DockPane.AppearanceStyle.Document)
                return;

            int indexHit = HitTest();
            if (indexHit > -1)
                TabCloseButtonHit(indexHit);
        }

        private void TabCloseButtonHit(int index)
        {
            if (index < 0 || index >= Tabs.Count)
                return;

            Point mousePos = PointToClient(MousePosition);
            Rectangle tabRect = GetTabBounds(Tabs[index]);
            if (!tabRect.Contains(ActiveClose) || !ActiveCloseHitTest(mousePos))
                return;

            IDockContent tabContent = Tabs[index].Content;
            if (tabContent == null || IsDisposed || Disposing || !IsHandleCreated)
                return;

            QueueCloseTab(tabContent);
        }

        private void QueueCloseTab(IDockContent tabContent)
        {
            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    if (IsDisposed || Disposing)
                        return;

                    for (int i = 0; i < Tabs.Count; i++)
                    {
                        if (Tabs[i].Content != tabContent)
                            continue;

                        try
                        {
                            TryCloseTab(i);
                        }
                        catch (NullReferenceException)
                        {
                            // Avoid crash on transient dock layout races during tab-close.
                        }
                        catch (ObjectDisposedException)
                        {
                            // Tab/pane was disposed before queued close executed.
                        }
                        catch (InvalidOperationException)
                        {
                            // Docking state changed before queued close executed.
                        }

                        break;
                    }
                }));
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }

        private Rectangle GetCloseButtonRect(Rectangle rectTab)
        {
            if (Appearance != DockPane.AppearanceStyle.Document)
            {
                return Rectangle.Empty;
            }

            const int gap = 3;
            int imageSize = PatchController.EnableHighDpi == true ? rectTab.Height - gap * 2 : 15;
            return new Rectangle(rectTab.X + rectTab.Width - imageSize - gap - 1, rectTab.Y + gap, imageSize,
                                 imageSize);
        }

        private void WindowList_Click(object sender, EventArgs e)
        {
            SelectMenu.Items.Clear();
            foreach (Tab tab1 in Tabs)
            {
                MremoteNGTab tab = (MremoteNGTab)tab1;
                IDockContent content = tab.Content;
                ToolStripItem item = SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap());
                item.Tag = tab.Content;
                item.Click += ContextMenuItem_Click;
            }

            Rectangle workingArea =
                Screen.GetWorkingArea(ButtonWindowList.PointToScreen(new Point(ButtonWindowList.Width / 2,
                                                                               ButtonWindowList.Height / 2)));
            Rectangle menu = new(
                                     ButtonWindowList.PointToScreen(new Point(0,
                                                                              ButtonWindowList.Location.Y +
                                                                              ButtonWindowList.Height)),
                                     SelectMenu.Size);
            Rectangle menuMargined = new(menu.X - SelectMenuMargin, menu.Y - SelectMenuMargin,
                                             menu.Width + SelectMenuMargin, menu.Height + SelectMenuMargin);
            if (workingArea.Contains(menuMargined))
            {
                SelectMenu.Show(menu.Location);
            }
            else
            {
                Point newPoint = menu.Location;
                newPoint.X = DrawHelper.Balance(SelectMenu.Width, SelectMenuMargin, newPoint.X, workingArea.Left,
                                                workingArea.Right);
                newPoint.Y = DrawHelper.Balance(SelectMenu.Size.Height, SelectMenuMargin, newPoint.Y, workingArea.Top,
                                                workingArea.Bottom);
                Point button = ButtonWindowList.PointToScreen(new Point(0, ButtonWindowList.Height));
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
            IDockContent content = (IDockContent)item.Tag;
            DockPane.ActiveContent = content;
        }

        private void SetInertButtons()
        {
            if (Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                if (m_buttonOverflow != null)
                    m_buttonOverflow.Left = -m_buttonOverflow.Width;

                if (m_buttonWindowList != null)
                    m_buttonWindowList.Left = -m_buttonWindowList.Width;
            }
            else
            {
                ButtonOverflow.Visible = m_documentTabsOverflow;
                ButtonOverflow.RefreshChanges();

                ButtonWindowList.Visible = !m_documentTabsOverflow;
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
            Rectangle rectTabStrip = TabStripRectangle;

            // Set position and size of the buttons
            int buttonWidth = ButtonOverflow.Image.Width;
            int buttonHeight = ButtonOverflow.Image.Height;
            int height = rectTabStrip.Height - DocumentButtonGapTop - DocumentButtonGapBottom;
            if (buttonHeight < height)
            {
                buttonWidth = buttonWidth * height / buttonHeight;
                buttonHeight = height;
            }

            Size buttonSize = new(buttonWidth, buttonHeight);

            int x = rectTabStrip.X + rectTabStrip.Width - DocumentTabGapLeft
                                                        - DocumentButtonGapRight - buttonWidth;
            int y = rectTabStrip.Y + DocumentButtonGapTop;
            Point point = new(x, y);
            ButtonOverflow.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));

            // If the close button is not visible draw the window list button overtop.
            // Otherwise it is drawn to the left of the close button.
            ButtonWindowList.Bounds = DrawHelper.RtlTransform(this, new Rectangle(point, buttonSize));
        }

        private void Close_Click(object sender, EventArgs e)
        {
            CloseProtocol();

            if (PatchController.EnableMemoryLeakFix == true)
            {
                ContentClosed();
            }
        }

        protected override int HitTest(Point point)
        {
            if (!TabsRectangle.Contains(point))
                return -1;

            foreach (Tab tab in Tabs)
            {
                GraphicsPath path = GetTabOutline(tab, true, false);
                if (path.IsVisible(point))
                    return Tabs.IndexOf(tab);
            }

            return -1;
        }

        protected override bool MouseDownActivateTest(MouseEventArgs e)
        {
            bool result = base.MouseDownActivateTest(e);
            if (result && (e.Button == MouseButtons.Left) && (Appearance == DockPane.AppearanceStyle.Document))
            {
                // don't activate if mouse is down on active close button
                result = !ActiveCloseHitTest(e.Location);
            }

            return result;
        }

        private bool ActiveCloseHitTest(Point ptMouse)
        {
            if (ActiveClose.IsEmpty) return false;
            Rectangle mouseRect = new(ptMouse, new Size(1, 1));
            return ActiveClose.IntersectsWith(mouseRect);
        }

        protected override Rectangle GetTabBounds(Tab tab)
        {
            GraphicsPath path = GetTabOutline(tab, true, false);
            RectangleF rectangle = path.GetBounds();
            return new Rectangle((int)rectangle.Left, (int)rectangle.Top, (int)rectangle.Width, (int)rectangle.Height);
        }

        private Rectangle ActiveClose { get; set; }

        private bool SetActiveClose(Rectangle rectangle)
        {
            if (ActiveClose == rectangle)
                return false;

            ActiveClose = rectangle;
            return true;
        }

        private bool SetMouseOverTab(IDockContent content)
        {
            if (DockPane.MouseOverTab == content)
                return false;

            DockPane.MouseOverTab = content;
            return true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            bool tabUpdate = SetMouseOverTab(null);
            bool buttonUpdate = SetActiveClose(Rectangle.Empty);
            if (tabUpdate || buttonUpdate)
                Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            PerformLayout();
        }

        private void CloseProtocol()
        {
            InterfaceControl ic = InterfaceControl.FindInterfaceControl(DockPane.DockPanel);
            ic?.Protocol.Close();
        }

        #region Native Methods
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Msgs.WM_LBUTTONDBLCLK)
            {
                // If the option is not set, do nothing. Do not send the message to base.
                if (!Properties.OptionsTabsPanelsPage.Default.DoubleClickOnTabClosesIt) return;

                // Option is set, close the tab, then send to base.
                //DockPane.CloseActiveContent();
                CloseProtocol();

                if (PatchController.EnableMemoryLeakFix == true)
                {
                    ContentClosed();
                }

                return;
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}
