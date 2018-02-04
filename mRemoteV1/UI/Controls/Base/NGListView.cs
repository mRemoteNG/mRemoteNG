using BrightIdeasSoftware;
using mRemoteNG.Themes;
using System.Drawing;

namespace mRemoteNG.UI.Controls.Base
{
    //Simple coloring of ObjectListView
    //This is subclassed to avoid repeating the code in multiple places
    internal class NGListView : ObjectListView
    {

        private CellBorderDecoration deco;
        //Control if the gridlines are styled, must be set before the OnCreateControl is fired
        public bool DecorateLines { get; set; } = true;


        public NGListView()
        {
            ThemeManager.getInstance().ThemeChanged += OnCreateControl;
        }




        protected override void OnCreateControl()
        {
            base.OnCreateControl(); 
            var _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            //List back color
            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Foreground");
            //Selected item
            SelectedBackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Background");
            SelectedForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Foreground");
                
            //Header style
            HeaderUsesThemes = false;
            var headerStylo = new HeaderFormatStyle();
            headerStylo.Normal.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Header_Background");
            headerStylo.Normal.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Header_Foreground"); 
            HeaderFormatStyle = headerStylo;
            //Border style
            if(DecorateLines)
            {
                UseCellFormatEvents = true;
                GridLines = false;
                deco = new CellBorderDecoration
                {
                    BorderPen = new Pen(_themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Border")),
                    FillBrush = null,
                    BoundsPadding = Size.Empty,
                    CornerRounding = 0
                };
                FormatCell += NGListView_FormatCell;
            }
            if (Items != null && Items.Count != 0)
                BuildList();
            Invalidate();
        }

        private void NGListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Column.IsVisible)
            {
                e.SubItem.Decoration = deco;
            }
        }
    }
}
