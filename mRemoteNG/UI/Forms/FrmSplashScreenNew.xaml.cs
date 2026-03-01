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
        static FrmSplashScreenNew? instance;
        public FrmSplashScreenNew()
        {
            InitializeComponent();
            LoadFont();
            lblLogoPartD.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            lblLogoPartD.Content = $@"v. {GeneralAppInfo.ApplicationVersion} - 'Fructus temporum'";
            SourceInitialized += OnSourceInitialized;
        }

        // Fix #2685: CenterScreen uses system DPI which is wrong with PerMonitorV2.
        // SourceInitialized fires after HWND creation but before ShowWindow, so we
        // can set Left/Top using the actual window DPI (from PresentationSource) and
        // physical screen pixels (from Screen.WorkingArea) without any visible flash.
        private void OnSourceInitialized(object? sender, EventArgs e)
        {
            var source = System.Windows.PresentationSource.FromVisual(this);
            if (source?.CompositionTarget == null) return;

            double dpiX = source.CompositionTarget.TransformToDevice.M11;
            double dpiY = source.CompositionTarget.TransformToDevice.M22;

            var workArea = System.Windows.Forms.Screen.PrimaryScreen?.WorkingArea;
            if (workArea == null) return;

            // workArea is in physical pixels; convert to WPF logical units using actual window DPI
            Left = workArea.Value.Left / dpiX + (workArea.Value.Width / dpiX - Width) / 2;
            Top  = workArea.Value.Top  / dpiY + (workArea.Value.Height / dpiY - Height) / 2;
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
