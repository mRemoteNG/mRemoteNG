using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.App
{
    public class MessageCollectorSetup
    {
        public static void Setup(IList<IMessageWriter> messageWriterList, MessageCollector2 messageCollector)
        {
            messageWriterList.Add(BuildDebugConsoleMessageWriter());
            messageWriterList.Add(BuildTextLogMessageWriter());
            messageWriterList.Add(BuildNotificationPanelMessageWriter());
            messageWriterList.Add(BuildPopupMessageWriter());

            messageCollector.CollectionChanged += (o, args) =>
            {
                var messages = args.NewItems.Cast<IMessage>().ToArray();
                foreach (var printer in Runtime.MessageWriters)
                    printer.Print(messages);
            };
        }

        private static DebugConsoleMessageWriter BuildDebugConsoleMessageWriter()
        {
            return new DebugConsoleMessageWriter
            {
                PrintDebugMessages = Settings.Default.DebugMessageWriterWriteDebugMsgs,
                PrintInfoMessages = Settings.Default.DebugMessageWriterWriteInfoMsgs,
                PrintWarningMessages = Settings.Default.DebugMessageWriterWriteWarningMsgs,
                PrintErrorMessages = Settings.Default.DebugMessageWriterWriteErrorMsgs
            };
        }

        private static TextLogMessageWriter BuildTextLogMessageWriter()
        {
            return new TextLogMessageWriter(Logger.Instance)
            {
                PrintDebugMessages = Settings.Default.TextLogMessageWriterWriteDebugMsgs,
                PrintInfoMessages = Settings.Default.TextLogMessageWriterWriteInfoMsgs,
                PrintWarningMessages = Settings.Default.TextLogMessageWriterWriteWarningMsgs,
                PrintErrorMessages = Settings.Default.TextLogMessageWriterWriteErrorMsgs
            };
        }

        private static NotificationPanelMessageWriter BuildNotificationPanelMessageWriter()
        {
            return new NotificationPanelMessageWriter(Windows.ErrorsForm)
            {
                PrintDebugMessages = Settings.Default.NotificationPanelWriterWriteDebugMsgs,
                PrintInfoMessages = Settings.Default.NotificationPanelWriterWriteInfoMsgs,
                PrintWarningMessages = Settings.Default.NotificationPanelWriterWriteWarningMsgs,
                PrintErrorMessages = Settings.Default.NotificationPanelWriterWriteErrorMsgs
            };
        }

        private static PopupMessageWriter BuildPopupMessageWriter()
        {
            return new PopupMessageWriter
            {
                PrintDebugMessages = Settings.Default.PopupMessageWriterWriteDebugMsgs,
                PrintInfoMessages = Settings.Default.PopupMessageWriterWriteInfoMsgs,
                PrintWarningMessages = Settings.Default.PopupMessageWriterWriteWarningMsgs,
                PrintErrorMessages = Settings.Default.PopupMessageWriterWriteErrorMsgs
            };
        }
    }
}