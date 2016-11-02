namespace mRemoteNG.App.Info
{
    public static class UpdateChannelInfo
    {
        public static string FileName
        {
            get
            {
#if DEBUG
				return "update-debug.txt";
#else
                return Settings.Default.UpdateChannel.ToLowerInvariant() == "debug" ? "update-debug.txt" : "update.txt";
#endif
            }
        }
    }
}