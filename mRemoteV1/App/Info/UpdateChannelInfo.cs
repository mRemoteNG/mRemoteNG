using System;
// ReSharper disable InconsistentNaming

namespace mRemoteNG.App.Info
{
    public static class UpdateChannelInfo
    {
        public const string STABLE = "Stable";
        public const string BETA = "Beta";
        public const string DEV = "Development";

        /* no #if here since they are used for unit tests as well */
        public const string STABLE_PORTABLE = "update-portable.txt";
        public const string BETA_PORTABLE = "beta-update-portable.txt";
        public const string DEV_PORTABLE = "dev-update-portable.txt";

        public const string STABLE_MSI = "update.txt";
        public const string BETA_MSI = "beta-update.txt";
        public const string DEV_MSI = "dev-update.txt";


        public static Uri GetUpdateChannelInfo()
        {
            var channel = IsValidChannel(Settings.Default.UpdateChannel) ? Settings.Default.UpdateChannel : STABLE;
            return GetUpdateTxtUri(channel);
        }

        private static string GetChannelFileName(string channel)
        {
            return Runtime.IsPortableEdition ? GetChannelFileNamePortableEdition(channel) : GetChannelFileNameNormalEdition(channel);
        }

        private static string GetChannelFileNameNormalEdition(string channel)
        {
            switch (channel)
            {
                case STABLE:
                    return STABLE_MSI;
                case BETA:
                    return BETA_MSI;
                case DEV:
                    return DEV_MSI;
                default:
                    return STABLE_MSI;
            }
        }

        private static string GetChannelFileNamePortableEdition(string channel)
        {
            switch (channel)
            {
                case STABLE:
                    return STABLE_PORTABLE;
                case BETA:
                    return BETA_PORTABLE;
                case DEV:
                    return DEV_PORTABLE;
                default:
                    return STABLE_PORTABLE;
            }
        }

        private static Uri GetUpdateTxtUri(string channel)
        {
            return new Uri(new Uri(Settings.Default.UpdateAddress), new Uri(GetChannelFileName(channel), UriKind.Relative));
        }

        private static bool IsValidChannel(string s)
        {
            return s.Equals(STABLE) || s.Equals(BETA) || s.Equals(DEV);
        }
    }
}