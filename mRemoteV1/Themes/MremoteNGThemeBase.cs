namespace mRemoteNG.Themes
{

    using WeifenLuo.WinFormsUI.ThemeVS2015;


    /// <summary>
    /// Visual Studio 2015 Light theme.
    /// </summary>
    public class MremoteNGThemeBase : VS2015ThemeBase
    {
        public MremoteNGThemeBase(byte[] themeResource)
            : base(themeResource)
        {
            Measures.SplitterSize = 3;
            Measures.AutoHideSplitterSize = 3;
            Measures.DockPadding = 2;
            ShowAutoHideContentOnHover = false;
        }
    }
}