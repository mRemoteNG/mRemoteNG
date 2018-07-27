using System;
using System.ComponentModel;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNG.Themes
{

    /// <inheritdoc />
    /// <summary>
    /// Container class for all the color and style elements to define a theme
    /// </summary>
	public class ThemeInfo : ICloneable
    {
        #region Private Variables
        private string _name;
        private ThemeBase _theme;
        private string  _URI;
        private VisualStudioToolStripExtender.VsVersion _version;
        private ExtendedColorPalette _extendedPalette;

        #endregion

        #region Constructors
        public ThemeInfo(string themeName, ThemeBase inTheme, string inURI, VisualStudioToolStripExtender.VsVersion inVersion, ExtendedColorPalette inExtendedPalette)
        {
            _name = themeName;
            _theme = inTheme;
            _URI = inURI;
            _version = inVersion;
            _extendedPalette = inExtendedPalette;
            IsThemeBase = false;
            IsExtendable = false;
        }

        public ThemeInfo(string themeName, ThemeBase inTheme, string inURI, VisualStudioToolStripExtender.VsVersion inVersion)
        {
            _name = themeName;
            _theme = inTheme;
            _URI = inURI;
            _version = inVersion;
            IsThemeBase = false;
            IsExtendable = false;
        }
        #endregion

        #region Public Methods
        public object Clone()
        {
            var extPalette = new ExtendedColorPalette
            {
                ExtColorPalette =
                    _extendedPalette.ExtColorPalette.ToDictionary(entry => entry.Key, entry => entry.Value),
                DefaultColorPalette = _extendedPalette.DefaultColorPalette
            };
            var clonedObj = new ThemeInfo(_name, _theme, _URI, _version, extPalette)
            {
                IsExtendable = IsExtendable,
                IsThemeBase = IsThemeBase
            };

            return clonedObj;
        }

        #endregion


        #region Properties
        [Browsable(false)]
        public string Name
		{
			get => _name;
            set
			{
				if (string.Equals(_name, value, StringComparison.InvariantCulture))
				{
					return;
				}
				_name = value; 
			}
		}

        public ThemeBase Theme
        {
            get => _theme;
            set
            {
                if (value != null && _theme == value)
                {
                    return;
                }
                _theme = value;
            }
        }

        public string  URI
        {
            get => _URI;
            set
            {
                if (value != null && _URI == value)
                {
                    return;
                }
                _URI = value;
            }
        }

        public VisualStudioToolStripExtender.VsVersion Version
        {
            get => _version;
            set
            {
                if (Equals(_version, value))
                {
                    return;
                }
                _version = value;
            }
        }

        public ExtendedColorPalette ExtendedPalette
        {
            get => _extendedPalette;
            set
            {
                if (_extendedPalette != null && _extendedPalette == value)
                {
                    return;
                }
                _extendedPalette = value;
            }
        }

        public bool IsThemeBase { get; set; }

        public bool IsExtendable { get; set; }

        #endregion
    }
}