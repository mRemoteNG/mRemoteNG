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
            object? editedValue = base.EditValue(context, provider, ToColor(value));
            return editedValue is Color color ? Converter.ConvertFrom(color) : value;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(new PaintValueEventArgs(e.Context, ToColor(e.Value), e.Graphics, e.Bounds));
        }

        private static object? ToColor(object? value)
        {
            if (value is Color)
                return value;

            try
            {
                return Converter.ConvertTo(null, null, value, typeof(Color));
            }
            catch (NotSupportedException)
            {
                return Color.Empty;
            }
        }
    }
}
