using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls.Base
{
    public partial class NGPictureBox : PictureBox
    {
        private ThemeManager _themeManager;

        public NGPictureBox()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }

        public NGPictureBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ActiveAndExtended) return;
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Foreground");
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("TextBox_Background");
            Invalidate();
        }
    }
}
