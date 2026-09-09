using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid
{
    /// <summary>
    /// Color picker for properties that are stored as a string. The stock
    /// <see cref="ColorEditor"/> hands a <see cref="Color"/> straight to
    /// <see cref="PropertyDescriptor.SetValue"/>, which fails with
    /// "Property value is not valid" on a string property, so the value is
    /// translated in both directions here.
    /// </summary>
    public class ColorStringEditor : ColorEditor
    {
        private static readonly MiscTools.TabColorConverter Converter = new();

        public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
        {
            bool valueIsAColor = TryConvertToColor(value, out Color currentColor);
            object? editedValue = base.EditValue(context, provider, currentColor);

            if (editedValue is not Color pickedColor)
                return value;

            // A value the converter cannot parse - a legacy or hand-edited entry, say -
            // also arrives here as an empty color. Dismissing the picker would then wipe
            // it, so it is kept until a real color is picked.
            if (pickedColor.IsEmpty && !valueIsAColor)
                return value;

            return Converter.ConvertFrom(pickedColor);
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            TryConvertToColor(e.Value, out Color color);
            base.PaintValue(new PaintValueEventArgs(e.Context, color, e.Graphics, e.Bounds));
        }

        /// <summary>
        /// Converts a stored value to the <see cref="Color"/> the wrapped editor works
        /// with, reporting whether it actually describes a color.
        /// </summary>
        private static bool TryConvertToColor(object? value, out Color color)
        {
            color = Color.Empty;

            if (value is Color existingColor)
            {
                color = existingColor;
                return true;
            }

            // Nothing stored means "no color", which is a valid state rather than a
            // failed conversion.
            if (value is null || (value is string text && string.IsNullOrWhiteSpace(text)))
                return true;

            try
            {
                if (Converter.ConvertTo(null, null, value, typeof(Color)) is Color convertedColor)
                    color = convertedColor;
            }
            catch (NotSupportedException)
            {
                return false;
            }

            // TabColorConverter maps text it cannot parse to an empty color instead of
            // throwing, so an empty result here means the conversion failed.
            return !color.IsEmpty;
        }
    }
}
