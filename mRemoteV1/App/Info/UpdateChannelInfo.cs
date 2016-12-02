namespace mRemoteNG.App.Info
{
    public static class UpdateChannelInfo
    {
        internal const string STABLE = "Stable";
        internal const string BETA = "Beta";
        internal const string DEV = "Development";

        public static string FileName
        {
#if PORTABLE
            get
            {
                /*                                   */
                /* return PORTABLE update files here */
                /*                                   */
                switch (Settings.Default.UpdateChannel)
                {
                    case STABLE:
                        return "update-portable.txt";
                    case BETA:
                        return "beta-update-portable.txt";
                    case DEV:
                        return "dev-update-portable.txt";
                    default:
                        return "update-portable.txt";
                }
            }
#else //NOT portable
            get
            {
                /*                                    */
                /* return INSTALLER update files here */
                /*                                    */
                switch (Settings.Default.UpdateChannel)
                {
                    case STABLE:
                        return "update.txt";
                    case BETA:
                        return "beta-update.txt";
                    case DEV:
                        return "dev-update.txt";
                    default:
                        return "update.txt";
                }
            }
#endif //endif for PORTABLE
        }
    }
}