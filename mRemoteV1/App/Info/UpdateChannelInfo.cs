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
#if DEBUG
                return "update-portable-debug.txt";
#else
                return Settings.Default.UpdateChannel.ToLowerInvariant() == "debug" ? "update-portable-debug.txt" : "update-portable.txt";
#endif 
            }
#else //NOT portable
            get
            {
                /*                                    */
                /* return INSTALLER update files here */
                /*                                    */
#if DEBUG
				return "update-debug.txt";
#else
                return Settings.Default.UpdateChannel.ToLowerInvariant() == "debug" ? "update-debug.txt" : "update.txt";
#endif
            }
#endif //endif for PORTABLE
        }
    }
}