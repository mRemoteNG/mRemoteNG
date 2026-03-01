using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Window
{
    [SupportedOSPlatform("windows")]
    public class KeyboardShortcutsWindow : BaseWindow
    {
        private ListView _listView = null!;
        private readonly ThemeManager _themeManager;

        public KeyboardShortcutsWindow()
        {
            WindowType = WindowType.KeyboardShortcuts;
            DockPnl = this;
            InitializeComponent();
            PopulateShortcuts();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyThemeHandler;
            ApplyTheme();
        }

        private void ApplyThemeHandler()
        {
            ApplyTheme();
            if (!_themeManager.ActiveAndExtended) return;
            _listView.BackColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("List_Background") ?? SystemColors.Window;
            _listView.ForeColor = _themeManager.ActiveTheme.ExtendedPalette?.getColor("List_Foreground") ?? SystemColors.WindowText;
        }

        private void InitializeComponent()
        {
            _listView = new ListView();
            SuspendLayout();

            // listView
            _listView.AccessibleName = "Keyboard Shortcuts";
            _listView.AccessibleDescription = "List of keyboard shortcuts and their actions";
            _listView.Columns.AddRange(new[]
            {
                new ColumnHeader { Text = "Shortcut", Width = 180 },
                new ColumnHeader { Text = "Action", Width = 300 }
            });
            _listView.Dock = DockStyle.Fill;
            _listView.FullRowSelect = true;
            _listView.GridLines = true;
            _listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            _listView.Name = "lvShortcuts";
            _listView.View = View.Details;

            // KeyboardShortcutsWindow
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(500, 400);
            Controls.Add(_listView);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            HideOnClose = true;
            Name = "KeyboardShortcutsWindow";
            TabText = "Keyboard Shortcuts";
            Text = "Keyboard Shortcuts";
            ResumeLayout(false);
        }

        private void PopulateShortcuts()
        {
            var shortcuts = new[]
            {
                ("Ctrl+N", "New connection file"),
                ("Ctrl+O", "Open connection file"),
                ("Ctrl+S", "Save connection file"),
                ("Ctrl+Shift+S", "Save connection file as"),
                ("F2", "Rename selected connection/folder"),
                ("Delete", "Delete selected connection/folder"),
                ("Ctrl+D", "Duplicate selected connection"),
                ("Ctrl+Shift+C", "Copy hostname to clipboard"),
                ("Enter", "Open selected connection"),
                ("Ctrl+Up", "Move selected item up"),
                ("Ctrl+Down", "Move selected item down"),
                ("Ctrl+Tab", "Next connection tab"),
                ("Ctrl+Shift+Tab", "Previous connection tab"),
                ("Ctrl+PageDown", "Next connection tab"),
                ("Ctrl+PageUp", "Previous connection tab"),
                ("Ctrl+Right", "Next session"),
                ("Ctrl+Left", "Previous session"),
                ("Ctrl+1 … Ctrl+9", "Jump to session by number"),
                ("Ctrl+F", "Find in session output (SSH/Telnet)"),
                ("F11", "Toggle fullscreen"),
                ("F1", "Help contents"),
                ("Alt+F4", "Exit application"),
            };

            _listView.BeginUpdate();
            foreach (var (key, action) in shortcuts)
            {
                _listView.Items.Add(new ListViewItem(new[] { key, action }));
            }
            _listView.EndUpdate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _themeManager.ThemeChanged -= ApplyThemeHandler;
            }
            base.Dispose(disposing);
        }
    }
}
