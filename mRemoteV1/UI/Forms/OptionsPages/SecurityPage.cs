using System.ComponentModel;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class SecurityPage : OptionsPage
    {
        public SecurityPage()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        public override string PageName
        {
            get { return Language.strTabSecurity; }
            set { }
        }

        public override void ApplyLanguage()
        {

        }

        public override void LoadSettings()
        {

        }

        public override void SaveSettings()
        {

        }

        public override void RevertSettings()
        {

        }
    }
}