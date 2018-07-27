using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

namespace mRemoteNG.Themes 
{
    //Class to extract the rest of the required theme colors for MremoteNG from the vstheme file
    public class MremoteNGPaletteManipulator 
    {
        private XmlDocument _xml;
        private ExtendedColorPalette  _defaultPalette;

        

        //warning, defaultpalette should always contain all the values, because when is loaded there is no default palette (parameter is null
        public MremoteNGPaletteManipulator(byte[] file, ExtendedColorPalette defaultPalette = null )
        {
            _xml = new XmlDocument();
            _xml.LoadXml(new StreamReader(new MemoryStream(file)).ReadToEnd());   
            _defaultPalette = defaultPalette ?? new ExtendedColorPalette();
        }
        


        //Load the colors for the mRemoteNG own components as Dockpanel only have a menus and docks palette
        public ExtendedColorPalette getColors()
        {
            var newPalette = new ExtendedColorPalette();
            newPalette.setDefault(_defaultPalette);
            var resourceSet = ColorMapTheme.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            //
            foreach (DictionaryEntry entry in resourceSet)
            {
                var colorName  = entry.Key.ToString();
                var xmlQueryPath = entry.Value.ToString();
                if (_xml.DocumentElement == null) continue;
                var colorNodeList = _xml.DocumentElement.FirstChild.SelectNodes(xmlQueryPath);
                var color = colorNodeList != null && colorNodeList.Count > 0 ? colorNodeList[0].Value : null;
                if (color != null )
                {
                    newPalette.addColor(colorName , ColorTranslator.FromHtml($"#{color}"));
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
            var resourceSet = ColorMapTheme.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            
            foreach (DictionaryEntry entry in resourceSet)
            {
                var colorName = entry.Key.ToString();
                var xmlQueryPath = entry.Value.ToString();
                var colorNodeList = _xml.DocumentElement?.FirstChild.SelectNodes(xmlQueryPath);
                if (colorNodeList == null || colorNodeList.Count <= 0) continue;
                var paletteColor = colorPalette.getColor(colorName);
                colorNodeList[0].Value = $"FF{paletteColor.R:X2}{paletteColor.G:X2}{paletteColor.B:X2}";
            }
            var ms = new MemoryStream();
            _xml.Save(ms);
            var bytes = ms.ToArray();

            return bytes;
        }

    }
}
