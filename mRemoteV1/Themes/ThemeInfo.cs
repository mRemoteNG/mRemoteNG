using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.Themes
{

    /// <summary>
    /// Container class for all the color and style elements to define a theme
    /// </summary>
	public class ThemeInfo : ICloneable
    {
        #region Private Variables
        private string _name;
        private ThemeBase _theme;
        private String  _URI;
        private VisualStudioToolStripExtender.VsVersion _version;
        private ExtendedColorPalette _extendedPalette;
        private bool _isThemeBase;
        private bool _isExtendable;

        #endregion

        #region Constructors
        public ThemeInfo(string themeName, ThemeBase inTheme, String inURI, VisualStudioToolStripExtender.VsVersion inVersion, ExtendedColorPalette inExtendedPalette)
        {
            _name = themeName;
            _theme = inTheme;
            _URI = inURI;
            _version = inVersion;
            _extendedPalette = inExtendedPalette;
            _isThemeBase = false;
            _isExtendable = false;
        }

        public ThemeInfo(string themeName, ThemeBase inTheme, String inURI, VisualStudioToolStripExtender.VsVersion inVersion)
        {
            _name = themeName;
            _theme = inTheme;
            _URI = inURI;
            _version = inVersion;
            _isThemeBase = false;
            _isExtendable = false;
        }
        #endregion

        #region Public Methods
        public object Clone()
        {
            ThemeInfo clonedObj;
            ExtendedColorPalette extPalette = new ExtendedColorPalette();
            extPalette.ExtColorPalette = _extendedPalette.ExtColorPalette.ToDictionary(entry => entry.Key,entry => entry.Value);
            extPalette.DefaultColorPalette = _extendedPalette.DefaultColorPalette;
            clonedObj = new ThemeInfo(_name, _theme, _URI, _version, extPalette);
            clonedObj.IsExtendable = _isExtendable;
            clonedObj.IsThemeBase = _isThemeBase;

            return clonedObj;
        }

        #endregion


        #region Properties
        [Browsable(false)]
        public string Name
		{
			get { return _name; }
			set
			{
				if (_name == value)
				{
					return;
				}
				_name = value; 
			}
		}

        public ThemeBase Theme
        {
            get { return _theme; }
            set
            {
                if (_theme == value)
                {
                    return;
                }
                _theme = value;
            }
        }

        public string  URI
        {
            get { return _URI; }
            set
            {
                if (_URI == value)
                {
                    return;
                }
                _URI = value;
            }
        }

        public VisualStudioToolStripExtender.VsVersion Version
        {
            get { return _version; }
            set
            {
                if (_version == value)
                {
                    return;
                }
                _version = value;
            }
        }

        public ExtendedColorPalette ExtendedPalette
        {
            get { return _extendedPalette; }
            set
            {
                if (_extendedPalette == value)
                {
                    return;
                }
                _extendedPalette = value;
            }
        }

        public bool IsThemeBase
        {
            get { return _isThemeBase; }
            set
            { 
                _isThemeBase = value;
            }
        }

        public bool IsExtendable
        {
            get { return _isExtendable; }
            set
            {
                _isExtendable = value;
            }
        }
        #endregion
    }
}