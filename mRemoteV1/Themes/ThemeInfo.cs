using mRemoteNG.Tools;
using System;
using System.ComponentModel;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.Themes
{
	public class ThemeInfo 
    {
        #region Private Variables
        private string _name;
        private ThemeBase _theme;
        private String  _URI;
        private VisualStudioToolStripExtender.VsVersion _version;
        private ExtendedColorPalette _extendedPalette;

        #endregion

        #region Constructors
        public ThemeInfo(string themeName, ThemeBase inTheme, String inURI, VisualStudioToolStripExtender.VsVersion inVersion, ExtendedColorPalette inExtendedPalette)
        {
            _name = themeName;
            _theme = inTheme;
            _URI = inURI;
            _version = inVersion;
            _extendedPalette = inExtendedPalette;
        }

        public ThemeInfo(string themeName, ThemeBase inTheme, String inURI, VisualStudioToolStripExtender.VsVersion inVersion)
        {
            _name = themeName;
            _theme = inTheme;
            _URI = inURI;
            _version = inVersion; 
        }
        #endregion

        #region Public Methods
        public object Clone()
        {
            return MemberwiseClone();
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
        #endregion
    }
}