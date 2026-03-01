using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using mRemoteNG.Security;
using mRemoteNG.Themes;

namespace mRemoteNG.Themes
{
    //Class to extract the rest of the required theme colors for MremoteNG from the vstheme file
    public class MremoteNGPaletteManipulator
    {
        private XmlDocument _xml;
        private ExtendedColorPalette _defaultPalette;


        //warning, defaultpalette should always contain all the values, because when is loaded there is no default palette (parameter is null
        public MremoteNGPaletteManipulator(byte[] file, ExtendedColorPalette? defaultPalette = null)
        {
            _xml = SecureXmlHelper.LoadXmlFromString(new StreamReader(new MemoryStream(file)).ReadToEnd());
            _defaultPalette = defaultPalette ?? new ExtendedColorPalette();
        }


        //Load the colors for the mRemoteNG own components as Dockpanel only have a menus and docks palette
        public ExtendedColorPalette getColors()
        {
            ExtendedColorPalette newPalette = new();
            newPalette.setDefault(_defaultPalette);
            System.Resources.ResourceSet? resourceSet = ColorMapTheme.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            if (resourceSet == null) return newPalette;
            //
            foreach (DictionaryEntry entry in resourceSet)
            {
                string? colorName = entry.Key.ToString();
                string? xmlQueryPath = entry.Value?.ToString();
                if (_xml.DocumentElement?.FirstChild == null || colorName == null || xmlQueryPath == null) continue;
                XmlNodeList? colorNodeList = _xml.DocumentElement.FirstChild.SelectNodes(xmlQueryPath);
                string? color = colorNodeList != null && colorNodeList.Count > 0 ? colorNodeList[0]?.Value : null;
                if (color != null)
                {
                    newPalette.addColor(colorName, ColorTranslator.FromHtml($"#{color}"));
                }
            }

            return newPalette;
        }


        /// <summary>
        /// Takes a palette from memory and update the xml elements in disk
        /// </summary>
        /// <param name="colorPalette"></param>
        /// <returns></returns>
        public byte[] mergePalette(ExtendedColorPalette colorPalette)
        {
            System.Resources.ResourceSet? resourceSet = ColorMapTheme.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            if (resourceSet != null)
            foreach (DictionaryEntry entry in resourceSet)
            {
                string? colorName = entry.Key.ToString();
                string? xmlQueryPath = entry.Value?.ToString();
                if (colorName == null || xmlQueryPath == null) continue;
                XmlNodeList? colorNodeList = _xml.DocumentElement?.FirstChild?.SelectNodes(xmlQueryPath);
                if (colorNodeList == null || colorNodeList.Count <= 0) continue;
                Color paletteColor = colorPalette.getColor(colorName);
                XmlNode? node = colorNodeList[0];
                if (node != null)
                    node.Value = $"FF{paletteColor.R:X2}{paletteColor.G:X2}{paletteColor.B:X2}";
            }

            MemoryStream ms = new();
            _xml.Save(ms);
            byte[] bytes = ms.ToArray();

            return bytes;
        }
    }
}