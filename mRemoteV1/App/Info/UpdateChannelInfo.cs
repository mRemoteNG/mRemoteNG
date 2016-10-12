namespace mRemoteNG.App.Info
{
    public class UpdateChannelInfo
    {
        public static string FileName
        {
            get
            {
#if DEBUG
				return "update-debug.txt";
                #else
                if (Settings.Default.UpdateChannel.ToLowerInvariant() == "debug")
                    return "update-debug.txt";
                return "update.txt";
#endif
            }
        }
    }
}