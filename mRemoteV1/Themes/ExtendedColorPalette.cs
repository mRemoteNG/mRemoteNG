using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace mRemoteNG.Themes
{
    public class ExtendedColorPalette
    {
        #region Private Variables
        //Collection for color values that are not loaded by dock panels (list, buttons,panel content, etc)
        private Dictionary<String, Color> _extendedColors;
        private Dictionary<String, Color> _default;
        #endregion

        #region Constructors
        public ExtendedColorPalette()
        {
            _extendedColors = new Dictionary<string, Color>();
            _default = new Dictionary<string, Color>(); // If this is the default palette, it will not have a default-default palette
             
        }
        #endregion

        #region Public Methods
        // Set the default theme, that theme should contain all color values used by the application
        public void setDefault(ExtendedColorPalette inPalettte)
        {
            _default = inPalettte._extendedColors;
        }
        #endregion
 
        public Color getColor(String  colorKey)
        {
            Color retColor= Color.Empty;

            retColor = _extendedColors.ContainsKey(colorKey) ? _extendedColors[colorKey]:Color.Empty;
            //Invisible colors are not good, might  indicate missing color from the palette as is represented by 00000000
            if(retColor == Color.Empty || retColor.A == 0)
            {
                if(_default != null)
                {
                    retColor = _default.ContainsKey(colorKey) ? _default[colorKey] : Color.Empty;  
                }
                //why are we here?, just avoid a crash
                if(retColor == Color.Empty)
                {
                    //Fail to pink , because why not
                    retColor = Color.Pink;
                } 
                    
            }
            return retColor;

        }
        public void  addColor(String colorKey,Color inColor)
        {
            _extendedColors.Add(colorKey, inColor);
        }

    }
}
 

     