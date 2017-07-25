using BrightIdeasSoftware;
using mRemoteNG.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mRemoteNG.UI.Controls.Base
{
    //Simple coloring of ObjectListView
    //This is subclassed to avoid repeating the code in multiple places
    class NGListView : ObjectListView
    {

        private CellBorderDecoration deco;
        //Control if the gridlines are styled, must be set before the OnCreateControl is fired
        public bool DecorateLines { get; set; } = true;
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                ThemeManager _themeManager = ThemeManager.getInstance();
                //List back color
                BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Background");
                ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Foreground");
                //Selected item
                SelectedBackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Background");
                SelectedForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Selected_Foreground");
                
                //Header style
                HeaderUsesThemes = false;
                HeaderFormatStyle headerStylo = new HeaderFormatStyle();
                headerStylo.Normal.BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Header_Background");
                headerStylo.Normal.ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("List_Header_Foreground"); 
                HeaderFormatStyle = headerStylo;
                //Border style
                if(DecorateLines)
                {
                    UseCellFormatEvents = true;
                    GridLines = false;
                    deco = new CellBorderDecoration();
                    deco.BorderPen = new Pen(_themeManager.ActiveTheme.ExtendedPalette.getColor("List_Item_Border"));
                    deco.FillBrush = null;
                    deco.BoundsPadding = Size.Empty;
                    deco.CornerRounding = 0;
                    FormatCell += NGListView_FormatCell;
                }  
            }

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
