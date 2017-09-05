using System.Collections.Generic;
using System.Drawing;

namespace mRemoteNG.Themes
{

    /// <summary>
    /// Class used for the UI to display the color tables,as the Dictionary value keys cannot be directly replaced 
    /// </summary>
    public class PseudoKeyColor
    {
        private string key;
        private Color value;
        public PseudoKeyColor(string _key, Color _value)
        {
            key = _key;
            value = _value;
        }
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }
        public Color Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }


    /// <summary>
    /// Holds the color of a palette that are not included in the dockpanelsuite definition
    /// </summary>
    public class ExtendedColorPalette
    {
        #region Private Variables
        //Collection for color values that are not loaded by dock panels (list, buttons,panel content, etc)
        private Dictionary<string, Color> _extendedColors;
        private Dictionary<string, Color> _default;
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
 
        /// <summary>
        /// Obtains a color from the extended palette, if not present obtains it from the default palette, in the extreme case it uses Pink as a signal that a color is missing
        /// </summary>
        /// <param name="colorKey"></param>
        /// <returns></returns>
        public Color getColor(string  colorKey)
        {
            var retColor = _extendedColors.ContainsKey(colorKey) ? _extendedColors[colorKey]:Color.Empty;
            //Invisible colors are not good, might  indicate missing color from the palette as is represented by 00000000
            if (retColor != Color.Empty && retColor.A != 0) return retColor;
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
            return retColor;

        }

        /// <summary>
        /// Add a color to the extended palette
        /// </summary>
        /// <param name="colorKey"></param>
        /// <param name="inColor"></param>
        public void  addColor(string colorKey,Color inColor)
        {
            _extendedColors.Add(colorKey, inColor);
        }


        /// <summary>
        /// Replace the value of a color in the palette
        /// </summary>
        /// <param name="colorKey"></param>
        /// <param name="inColor"></param>
        public void replaceColor(string colorKey, Color inColor)
        {
            _extendedColors[colorKey]= inColor;
        }

        public Dictionary<string, Color> DefaultColorPalette
        {
            get
            {
                return _default;
            }
            set
            {
                _default = value;
            }
        }


        public Dictionary<string, Color> ExtColorPalette
        {
            get
            {
                return _extendedColors;
            }
            set
            {
                _extendedColors = value;
            }
        }
    }
}
 

     