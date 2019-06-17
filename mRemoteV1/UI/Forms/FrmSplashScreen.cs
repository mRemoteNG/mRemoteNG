using System.Windows.Forms;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmSplashScreen : Form
    {
        static FrmSplashScreen instance = null;

        private FrmSplashScreen() => InitializeComponent();

        public static FrmSplashScreen getInstance()
        {
            if (instance == null)
                instance = new FrmSplashScreen();
            return instance;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
    }
}