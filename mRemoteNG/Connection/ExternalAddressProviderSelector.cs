using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
    public enum ExternalAddressProvider
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.EAPNone))]
        None = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.EAPAmazonWebServices))]
        AmazonWebServices = 1
    }
}
