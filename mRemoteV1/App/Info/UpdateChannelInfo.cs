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
                if ((string)(mRemoteNG.Settings.Default.UpdateChannel.ToLowerInvariant()) == "debug")
                    return "update-debug.txt";
                else
                    return "update.txt";
                #endif
            }
        }
    }
}