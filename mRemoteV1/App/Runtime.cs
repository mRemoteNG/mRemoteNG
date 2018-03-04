using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;

namespace mRemoteNG.App
{
	public class Runtime
    {
        public static bool IsPortableEdition
        {
            get
            {
#if PORTABLE
                return true;
#else
                return false;
#endif
            }
        }

        public static MessageCollector MessageCollector { get; } = new MessageCollector();
        public static NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static ConnectionsService ConnectionsService { get; set; }
    }
}