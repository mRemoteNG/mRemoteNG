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
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }
			
        #region Public Properties
	    // ReSharper disable once UnusedAutoPropertyAccessor.Global
	    [Browsable(false)]public virtual string PageName {get; set;}
			
		public virtual Icon PageIcon {get; protected set;}
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

        /*
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // OptionsPage
            // 
            Name = "OptionsPage";
            Size = new Size(610, 489);
            ResumeLayout(false);

        }
        */

	    protected virtual void ApplyTheme()
        {
            if (!Themes.ThemeManager.getInstance().ThemingActive) return;
            BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OptionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OptionsPage";
            this.ResumeLayout(false);

        }
    }
}
