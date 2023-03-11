using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.App;


namespace mRemoteNG.Credential
{
    [SupportedOSPlatform("windows")]
    public class CredentialRecordTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Guid) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Guid) || destinationType == typeof(ICredentialRecord) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is ICredentialRecord && destinationType == typeof(Guid))
                return ((ICredentialRecord)value).Id;
            if (value is ICredentialRecord && destinationType == typeof(ICredentialRecord))
                return value;
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is Guid)) return base.ConvertFrom(context, culture, value);
            var matchedCredentials = Runtime.CredentialProviderCatalog.GetCredentialRecords()
                                            .Where(record => record.Id.Equals(value)).ToArray();
            return matchedCredentials.Any() ? matchedCredentials.First() : null;
        }
    }
}