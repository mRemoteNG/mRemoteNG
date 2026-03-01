using System;
using System.Drawing;
using mRemoteNG.Properties;

namespace mRemoteNG.UI.Tabs
{
    internal static class ConnectionTabAppearanceSettings
    {
        private static Font? _cachedTabFont;
        private static string _cachedFontKey = string.Empty;

        public static Font GetTabFont(Font themeFont)
        {
            if (!OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont)
                return themeFont;

            string fontName = OptionsTabsPanelsPage.Default.ConnectionTabFontName?.Trim() ?? string.Empty;
            float fontSize = OptionsTabsPanelsPage.Default.ConnectionTabFontSize;

            if (string.IsNullOrWhiteSpace(fontName) || fontSize <= 0)
                return themeFont;

            string requestedKey = $"{fontName}|{fontSize}|{themeFont.Style}|{themeFont.Unit}";
            if (_cachedTabFont != null && string.Equals(_cachedFontKey, requestedKey, StringComparison.Ordinal))
                return _cachedTabFont;

            ResetCache();

            try
            {
                _cachedTabFont = new Font(fontName, fontSize, themeFont.Style, themeFont.Unit, themeFont.GdiCharSet);
                _cachedFontKey = requestedKey;
                return _cachedTabFont;
            }
            catch
            {
                return themeFont;
            }
        }

        public static Color? GetTabColorOverride(Color? connectionTabColor)
        {
            if (connectionTabColor.HasValue)
                return connectionTabColor;

            if (!OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor)
                return null;

            string customColor = OptionsTabsPanelsPage.Default.ConnectionTabColor?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(customColor))
                return null;

            try
            {
                ColorConverter converter = new();
                object? converted = converter.ConvertFromString(customColor);
                return converted is Color color && !color.IsEmpty
                    ? color
                    : null;
            }
            catch
            {
                return null;
            }
        }

        public static void ResetCache()
        {
            if (_cachedTabFont != null)
            {
                _cachedTabFont.Dispose();
                _cachedTabFont = null;
            }

            _cachedFontKey = string.Empty;
        }
    }
}
