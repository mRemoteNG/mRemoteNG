using System.Collections.Generic;
using System.Drawing;

namespace mRemoteNG.Themes
{

    /// <summary>
    /// Class used for the UI to display the color tables,as the Dictionary value keys cannot be directly replaced 
    /// </summary>
    public class PseudoKeyColor
    {
        public PseudoKeyColor(string _key, Color _value)
        {
            Key = _key;
            Value = _value;
        }
        public string Key { get; set; }

        public Color Value { get; set; }
    }


    /// <summary>
    /// Holds the color of a palette that are not included in the dockpanelsuite definition
    /// </summary>
    public class ExtendedColorPalette
    {
        #region Private Variables
        //Collection for color values that are not loaded by dock panels (list, buttons,panel content, etc)

        #endregion

        #region Constructors
        public ExtendedColorPalette()
        {
            ExtColorPalette = new Dictionary<string, Color>();
            DefaultColorPalette = new Dictionary<string, Color>(); // If this is the default palette, it will not have a default-default palette
             
        }
        #endregion

        #region Public Methods
        // Set the default theme, that theme should contain all color values used by the application
        public void setDefault(ExtendedColorPalette inPalettte)
        {
            DefaultColorPalette = inPalettte.ExtColorPalette;
        }
        #endregion
 
        /// <summary>
        /// Obtains a color from the extended palette, if not present obtains it from the default palette, in the extreme case it uses Pink as a signal that a color is missing
        /// </summary>
        /// <param name="colorKey"></param>
        /// <returns></returns>
        public Color getColor(string  colorKey)
        {
            var retColor = ExtColorPalette.ContainsKey(colorKey) ? ExtColorPalette[colorKey]:Color.Empty;
            //Invisible colors are not good, might  indicate missing color from the palette as is represented by 00000000
            if (retColor != Color.Empty && retColor.A != 0) return retColor;
            if(DefaultColorPalette != null)
            {
                retColor = DefaultColorPalette.ContainsKey(colorKey) ? DefaultColorPalette[colorKey] : Color.Empty;  
            }
            //why are we here?, just avoid a crash
            if(retColor == Color.Empty)
            {
                //Fail to pink , because why not
                retColor = Color.Pink;
            }
            return retColor;

        }

        /// <summary>
        /// Add a color to the extended palette
        /// </summary>
        /// <param name="colorKey"></param>
        /// <param name="inColor"></param>
        public void  addColor(string colorKey,Color inColor)
        {
            ExtColorPalette.Add(colorKey, inColor);
        }


        /// <summary>
        /// Replace the value of a color in the palette
        /// </summary>
        /// <param name="colorKey"></param>
        /// <param name="inColor"></param>
        public void replaceColor(string colorKey, Color inColor)
        {
            ExtColorPalette[colorKey]= inColor;
        }

        public Dictionary<string, Color> DefaultColorPalette { get; set; }


        public Dictionary<string, Color> ExtColorPalette { get; set; }
    }
}
 

     