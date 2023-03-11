using System;
using System.Runtime.Versioning;
using mRemoteNG.App.Info;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Interaction logic for FrmSplashScreenNew.xaml
    /// </summary>
    public partial class FrmSplashScreenNew
    {
        static FrmSplashScreenNew instance = null;
        public FrmSplashScreenNew()
        {
            InitializeComponent();
            LoadFont();
            lblLogoPartD.Content = $@"v. {GeneralAppInfo.ApplicationVersion}";
        }
        public static FrmSplashScreenNew GetInstance()
        {
            if (instance == null)
                instance = new FrmSplashScreenNew();
            return instance;
        }
        void LoadFont()
        {
            lblLogoPartA.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./UI/Font/#HandelGotDBol");
            lblLogoPartB.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./UI/Font/#HandelGotDBol");
        }
    }
}
