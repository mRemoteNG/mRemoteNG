using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq; 

namespace mRemoteNG.Themes 
{
    //Class to extract the rest of the required theme colors for MremoteNG from the vstheme file
    public class MremoteNGPaletteLoader 
    {
        private XDocument _xml;

        public MremoteNGPaletteLoader(byte[] file)
        {
            _xml = XDocument.Load(new StreamReader(new MemoryStream(file)));
        }

        public ExtendedColorPalette getColors()
        {
            ExtendedColorPalette palette = new ExtendedColorPalette();
            //List tree colors
            palette.TreeViewPalette.Background= ColorTranslatorFromHtml("TreeView", "Background",false);
            palette.TreeViewPalette.Foreground= ColorTranslatorFromHtml("TreeView", "Background",true);
            palette.TreeViewPalette.SelectedItemActive.Background= ColorTranslatorFromHtml("TreeView", "SelectedItemActive", false);
            palette.TreeViewPalette.SelectedItemActive.Foreground= ColorTranslatorFromHtml("TreeView", "SelectedItemActive", true);
            palette.TreeViewPalette.SelectedItemInactive.Background= ColorTranslatorFromHtml("TreeView", "SelectedItemInactive", false);
            palette.TreeViewPalette.SelectedItemInactive.Foreground= ColorTranslatorFromHtml("TreeView", "SelectedItemInactive", true);
            //List items colors, used for config and external tools
            palette.ListPalette.Background= ColorTranslatorFromHtml("Cider", "ListBackground", false);
            palette.ListPalette.ListItem.Foreground= ColorTranslatorFromHtml("Cider", "ListItem", true);
            palette.ListPalette.ListItem.Background= ColorTranslatorFromHtml("Cider", "ListItem", false);
            palette.ListPalette.ListHeader.Foreground= ColorTranslatorFromHtml("Cider", "ListHeader", true);
            palette.ListPalette.ListHeader.Background= ColorTranslatorFromHtml("Cider", "ListHeader", false);
            palette.ListPalette.ListItemBorder= ColorTranslatorFromHtml("Cider", "ListItemBorder", false);
            palette.ListPalette.ListItemSelectedBorder= ColorTranslatorFromHtml("Cider", "ListItemSelectedBorder", false);
            palette.ListPalette.ListItemSelected.Foreground= ColorTranslatorFromHtml("Cider", "ListItemSelected", true);
            palette.ListPalette.ListItemSelected.Background= ColorTranslatorFromHtml("Cider", "ListItemSelected", false);
            palette.ListPalette.ListItemDisabled.Foreground= ColorTranslatorFromHtml("Cider", "ListItemDisabled", true);
            palette.ListPalette.ListItemDisabled.Background= ColorTranslatorFromHtml("Cider", "ListItemDisabled", false);
            palette.ListPalette.ListItemDisabledBorder = ColorTranslatorFromHtml("Cider", "ListItemDisabledBorder", false);
            //Button colors
            palette.ButtonPalette.Background = ColorTranslatorFromHtml("CommonControls", "Button", false);
            palette.ButtonPalette.Foreground = ColorTranslatorFromHtml("CommonControls", "Button", true);
            palette.ButtonPalette.ButtonBorder = ColorTranslatorFromHtml("CommonControls", "ButtonBorder", false);
            palette.ButtonPalette.ButtonPressed.Background = ColorTranslatorFromHtml("CommonControls", "ButtonPressed", false);
            palette.ButtonPalette.ButtonPressed.Foreground= ColorTranslatorFromHtml("CommonControls", "ButtonPressed", true);
            palette.ButtonPalette.ButtonHover.Background = ColorTranslatorFromHtml("CommonControls", "ButtonHover", false);
            palette.ButtonPalette.ButtonHover.Foreground = ColorTranslatorFromHtml("CommonControls", "ButtonHover", true);
            //Button colors
            palette.WarningText.Background = ColorTranslatorFromHtml("Text Editor Text Marker Items", "compiler warning", false);
            palette.WarningText.Foreground = ColorTranslatorFromHtml("Text Editor Text Marker Items", "compiler warning", true);
            palette.ErrorText.Background = ColorTranslatorFromHtml("Text Editor Text Marker Items", "compiler error", false);
            palette.ErrorText.Foreground = ColorTranslatorFromHtml("Text Editor Text Marker Items", "compiler error", true);

            return palette;
        }

        //This code is taken from VS2012PaletteFactory WeifenLuo
        private Color ColorTranslatorFromHtml(string category,string name, bool foreground = false)
        {
            var color = _xml.Root.Element("Theme")
                .Elements("Category").FirstOrDefault(item => item.Attribute("Name").Value == category)?
                .Elements("Color").FirstOrDefault(item => item.Attribute("Name").Value == name)?
                .Element(foreground ? "Foreground" : "Background").Attribute("Source").Value;
            if (color == null)
            {
                return Color.Transparent;
            }

            return ColorTranslator.FromHtml($"#{color}");
        }
    }
}
