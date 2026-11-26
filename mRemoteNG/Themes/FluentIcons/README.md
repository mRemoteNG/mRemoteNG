These Microsoft Fluent System Icons are optional and selected by a theme's
`IconStyle="Fluent"` metadata. Existing themes retain the stock images.

Source: https://github.com/microsoft/fluentui-system-icons
Pinned revision: 41089846ad80c5749df505aef35a68712c1561b9
License: MIT; see LICENSE.txt. Per-icon source URLs are in sources.json.

icons.zip contains transparent PNG masks at 16 and 48 pixels for 36 regular
outline glyphs. They were rasterized from the pinned 16-pixel SVG geometry
with 4x supersampling. ThemeIcons resizes their alpha coverage and applies
IconColor or IconDisabledColor at runtime. It does not edit shared resources.
No Notepad++ artwork or branding is included.

The default connection icon is replaced only if Icons/mRemoteNG.ico is still
byte-for-byte equal to the embedded stock reference. Other connection icons
and the green connected-status overlay retain their original appearance.
