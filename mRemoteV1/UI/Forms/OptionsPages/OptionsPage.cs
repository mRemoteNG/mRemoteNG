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
        }

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

        #region Public Properties

        [Browsable(false)]
        public virtual string PageName { get; set; }

        public virtual Icon PageIcon { get; set; }

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
    }
}