using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;
using mRemoteNG.Messages.WriterDecorators;
using mRemoteNG.Tools;
using mRemoteNG.UI.Window;

namespace mRemoteNG.App.Initialization
{
	public class MessageCollectorSetup
    {
        public static void SetupMessageCollector(MessageCollector messageCollector, IList<IMessageWriter> messageWriterList)
        {
            messageCollector.CollectionChanged += (o, args) =>
            {
                var messages = args.NewItems.Cast<IMessage>().ToArray();
                foreach (var printer in messageWriterList)
                    foreach (var message in messages)
                        printer.Write(message);
            };
        }

        public static void BuildMessageWritersFromSettings(IList<IMessageWriter> messageWriterList, ErrorAndInfoWindow errorAndInfoWindow)
        {
#if DEBUG
            messageWriterList.Add(BuildDebugConsoleWriter());
#endif
            messageWriterList.Add(BuildTextLogMessageWriter());
            messageWriterList.Add(BuildNotificationPanelMessageWriter(errorAndInfoWindow));
            messageWriterList.Add(BuildPopupMessageWriter());
        }

        private static IMessageWriter BuildDebugConsoleWriter()
        {
            return new DebugConsoleMessageWriter();
        }

        private static IMessageWriter BuildTextLogMessageWriter()
        {
            return new MessageTypeFilterDecorator(
                new LogMessageTypeFilteringOptions(),
                new TextLogMessageWriter(Logger.Instance)
            );
        }

        private static IMessageWriter BuildNotificationPanelMessageWriter(ErrorAndInfoWindow errorAndInfoWindow)
        {
	        errorAndInfoWindow.ThrowIfNull(nameof(errorAndInfoWindow));
			return new OnlyLogMessageFilter(
                new MessageTypeFilterDecorator(
                    new NotificationPanelMessageFilteringOptions(),
                    new MessageFocusDecorator(
	                    errorAndInfoWindow,
                        new NotificationPanelSwitchOnMessageFilteringOptions(),
                        new NotificationPanelMessageWriter(errorAndInfoWindow)
                    )
                )
            );
        }

        private static IMessageWriter BuildPopupMessageWriter()
        {
            return new OnlyLogMessageFilter(
                new MessageTypeFilterDecorator(
                    new PopupMessageFilteringOptions(),
                    new PopupMessageWriter()
                )
            );
        }
    }
}