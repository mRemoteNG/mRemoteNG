using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace mRemoteNG.Themes
{

    public class BasicPalette
    {
        public Color Background { get; set; }
        public Color Foreground { get; set; }
    }
    public class TreeViewPalette : BasicPalette
    {
        public BasicPalette SelectedItemActive { get; set; }
        public BasicPalette SelectedItemInactive { get; set; }
    }

    public class ListPalette : BasicPalette
    {
        public BasicPalette ListItem { get; set; }
        public BasicPalette ListHeader { get; set; }
        public Color ListItemBorder { get; set; }
        public Color ListItemSelectedBorder { get; set; }
        public BasicPalette ListItemSelected { get; set; }
        public BasicPalette ListItemDisabled { get; set; }
        public Color ListItemDisabledBorder { get; set; }

    }

    public class ButtonPalette : BasicPalette
    {
        public Color ButtonBorder { get; set; }
        public BasicPalette ButtonPressed { get; set; }
        public BasicPalette ButtonHover { get; set; }
        
    }

    
    public class ExtendedColorPalette
    {
        public TreeViewPalette TreeViewPalette { get; set; } 
        public ListPalette ListPalette { get; set; }
        public ButtonPalette ButtonPalette { get; set; }
        public BasicPalette ErrorText { get; set; }
        public BasicPalette WarningText { get; set; }

        public ExtendedColorPalette()
        {
            TreeViewPalette = new TreeViewPalette();
            TreeViewPalette.SelectedItemActive = new BasicPalette();
            TreeViewPalette.SelectedItemInactive = new BasicPalette();
            ListPalette = new ListPalette();
            ListPalette.ListItem = new BasicPalette();
            ListPalette.ListHeader = new BasicPalette();
            ListPalette.ListItemSelected = new BasicPalette();
            ListPalette.ListItemDisabled = new BasicPalette();
            ButtonPalette = new ButtonPalette();
            ButtonPalette.ButtonPressed = new BasicPalette();
            ButtonPalette.ButtonHover = new BasicPalette();
            ErrorText = new BasicPalette();
            WarningText = new BasicPalette();
        }
    }

}

