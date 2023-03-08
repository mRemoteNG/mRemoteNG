using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    [SupportedOSPlatform("windows")]
    //This class is only minimally themed as textboxes onPaint are hard to theme (system wm paint control most of the drawing process
    //There are some glitches on the initial draw of some controls
    public class MrngTextBox : TextBox
    {
        private ThemeManager _themeManager;

        public MrngTextBox()
        {
            InitializeComponent();
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor(ReadOnly
                                                                               ? "TextBox_Disabled_Background"
                                                                               : "TextBox_Background");
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            _themeManager = ThemeManager.getInstance();
            _themeManager = ThemeManager.getInstance();
            if (_themeManager.ActiveAndExtended)
            {
                if (Enabled)
                {
                    ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
                }
                else
                {
                    BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Disabled_Background");
                }
            }

            base.OnEnabledChanged(e);
            Invalidate();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // NGTextBox
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResumeLayout(false);
        }
    }
}