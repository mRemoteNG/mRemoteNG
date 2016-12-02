namespace mRemoteNG.App.Info
{
    public static class UpdateChannelInfo
    {
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
                    case "Final":
                        return "update-portable.txt";
                    case "Beta":
                        return "beta-update-portable.txt";
                    case "Pre-Release":
                        return "prere-update-portable.txt";
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
                    case "Final":
                        return "update.txt";
                    case "Beta":
                        return "beta-update.txt";
                    case "Pre-Release":
                        return "prere-update.txt";
                    default:
                        return "update.txt";
                }
            }
#endif //endif for PORTABLE
        }
    }
}