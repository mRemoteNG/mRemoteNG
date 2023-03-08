using System.ComponentModel;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public class OptionsPage : UserControl
    {
        protected OptionsPage()
        {
            InitializeComponent();
            //ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }

        #region Public Properties

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Browsable(false)] public virtual string PageName { get; set; }

        public virtual Icon PageIcon { get; protected set; }
        public virtual Image IconImage => PageIcon?.ToBitmap();

        #endregion

        #region Public Methods

        public virtual void ApplyLanguage()
        {
        }

        public virtual void LoadSettings()
        {
        }

        public virtual void SaveSettings()
        {
        }

        public virtual void RevertSettings()
        {
        }

        #endregion

        protected virtual void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            Invalidate();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // OptionsPage
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "OptionsPage";
            ResumeLayout(false);
        }
    }
	internal class DropdownList
    {
        public int Index { get; set; }
        public string DisplayString { get; set; }

        public DropdownList(int argIndex, string argDisplay)
        {
            Index = argIndex;
            DisplayString = argDisplay;
        }
    }
}