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
            lblLogoPartD.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            lblLogoPartD.Content = $@"v. {GeneralAppInfo.ApplicationVersion} - 'Libro Ultimo'";
        }
        public static FrmSplashScreenNew GetInstance()
        {
            //instance == null
            instance ??= new FrmSplashScreenNew();
            return instance;
        }

        void LoadFont()
        {
            lblLogoPartA.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./UI/Font/#HandelGotDBol");
            lblLogoPartB.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./UI/Font/#HandelGotDBol");
        }
    }
}
