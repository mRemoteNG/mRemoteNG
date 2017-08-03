using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.UI.Forms.OptionsPages
{
	public class OptionsPage : UserControl
	{
	    protected OptionsPage()
		{
            //InitializeComponent();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
                Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;

        }
			
        #region Public Properties
		[Browsable(false)]public virtual string PageName {get; set;}
			
		public virtual Icon PageIcon {get; set;}
        public virtual Image IconImage{
            get
            {
                if (PageIcon != null)
                    return PageIcon.ToBitmap();
                return null;
            }
        }

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OptionsPage
            // 
            this.Name = "OptionsPage";
            this.Size = new System.Drawing.Size(610, 489);
            this.ResumeLayout(false); 
        }

        public virtual void ApplyTheme()
        {
             if (Themes.ThemeManager.getInstance().ThemingActive)
            {
                BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                Invalidate();
            }
        }
    }
}
