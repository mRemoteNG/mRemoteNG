using System;
using System.Drawing;
using System.Runtime.Versioning;
using mRemoteNG.Resources.Language;
using mRemoteNG.Themes;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    /// <summary>
    /// A dockable window for the connection search bar, so it can be placed apart from the
    /// connection tree. While this window is showing it holds the search bar of
    /// <see cref="ConnectionTreeWindow"/>; when it is hidden or closed the search bar goes back there.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class ConnectionSearchWindow : BaseWindow
    {
        // Room for the search bar plus the pane's caption, used for the first split under the tree
        private const int PreferredPaneHeight = 60;

        private readonly ConnectionTreeWindow _treeWindow;
        private ThemeManager? _themeManager;

        public ConnectionSearchWindow(ConnectionTreeWindow treeWindow)
        {
            _treeWindow = treeWindow ?? throw new ArgumentNullException(nameof(treeWindow));
            WindowType = WindowType.ConnectionSearch;
            DockPnl = new DockContent();
            InitializeWindow();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Search_16x);
            ApplyLanguage();
            Load += ConnectionSearchWindow_Load;
            DockStateChanged += (sender, args) => UpdateSearchBarHost();
        }

        /// <summary>True while the window is open in a dock panel, including auto-hidden.</summary>
        internal bool IsShowing => !IsDisposed && DockPanel != null &&
                                   DockState != DockState.Unknown && DockState != DockState.Hidden;

        /// <summary>
        /// Shows the window where it was last, or the first time, docked under the connection tree.
        /// </summary>
        internal void ShowIn(DockPanel dockPanel)
        {
            if (DockPanel == dockPanel && DockState != DockState.Unknown)
            {
                Show(dockPanel);
                Activate();
                return;
            }

            DockPane? treePane = _treeWindow.Pane;
            if (_treeWindow.DockPanel == dockPanel && treePane != null && !_treeWindow.IsHidden &&
                IsSplittable(_treeWindow.DockState) && treePane.Height > 0)
            {
                double proportion = Math.Clamp((double)PreferredPaneHeight / treePane.Height, 0.05, 0.5);
                Show(treePane, DockAlignment.Bottom, proportion);
            }
            else
            {
                Show(dockPanel, DockState.DockLeft);
            }
        }

        internal void RefreshUiLanguage()
        {
            if (!IsDisposed)
                ApplyLanguage();
        }

        private static bool IsSplittable(DockState dockState)
        {
            return dockState is DockState.DockLeft or DockState.DockRight or DockState.DockTop or
                                DockState.DockBottom or DockState.Float;
        }

        private void InitializeWindow()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new Size(204, 21);
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop |
                        DockAreas.DockBottom | DockAreas.Float;
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HideOnClose = true;
            Name = "ConnectionSearchWindow";
            ResumeLayout(false);
        }

        private void ApplyLanguage()
        {
            Text = Language.ConnectionSearch;
            TabText = Language.ConnectionSearch;
        }

        private void ConnectionSearchWindow_Load(object? sender, EventArgs e)
        {
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            ApplyTheme();
        }

        private void UpdateSearchBarHost()
        {
            if (IsShowing)
                _treeWindow.MoveSearchBarTo(this);
            else
                _treeWindow.ReturnSearchBar();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // The search bar belongs to the tree window; don't dispose it along with this one
                _treeWindow.ReturnSearchBar();
                if (_themeManager != null)
                    _themeManager.ThemeChanged -= ApplyTheme;
            }

            base.Dispose(disposing);
        }
    }
}
