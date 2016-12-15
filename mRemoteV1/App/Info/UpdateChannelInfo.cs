using System;

namespace mRemoteNG.App.Info
{
    public class UpdateChannelInfo
    {
        internal const string STABLE = "Stable";
        internal const string BETA = "Beta";
        internal const string DEV = "Development";

        /* no #if here since they are used for unit tests as well */
        public const string STABLE_PORTABLE = "update-portable.txt";
        public const string BETA_PORTABLE = "beta-update-portable.txt";
        public const string DEV_PORTABLE = "dev-update-portable.txt";

        public const string STABLE_MSI = "update.txt";
        public const string BETA_MSI = "beta-update.txt";
        public const string DEV_MSI = "dev-update.txt";

        private readonly string channel;

        public UpdateChannelInfo()
        {
            channel = IsValidChannel(Settings.Default.UpdateChannel) ? Settings.Default.UpdateChannel : STABLE;
        }

        public UpdateChannelInfo(string s)
        {
            channel = IsValidChannel(s) ? s : STABLE;
        }

        private string FileName
        {
#if PORTABLE
            get
            {
                /*                                   */
                /* return PORTABLE update files here */
                /*                                   */
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
#else //NOT portable
            get
            {
                /*                                    */
                /* return INSTALLER update files here */
                /*                                    */
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
#endif //endif for PORTABLE
        }

        public Uri GetUpdateTxtUri()
        {
            return new Uri(new Uri(Settings.Default.UpdateAddress), new Uri(FileName, UriKind.Relative));
        }

        private static bool IsValidChannel(string s)
        {
            return s.Equals(STABLE) || s.Equals(BETA) || s.Equals(DEV);
        }
    }
}