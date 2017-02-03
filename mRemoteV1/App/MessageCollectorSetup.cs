using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Messages;
using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.App
{
    public class MessageCollectorSetup
    {
        public static void SetupMessageCollector(MessageCollector messageCollector, IList<IMessageWriter> messageWriterList)
        {
            messageCollector.CollectionChanged += (o, args) =>
            {
                var messages = args.NewItems.Cast<IMessage>().ToArray();
                foreach (var printer in messageWriterList)
                    printer.Print(messages);
            };
        }

        public static void BuildMessageWritersFromSettings(IList<IMessageWriter> messageWriterList)
        {
#if DEBUG
            messageWriterList.Add(new DebugConsoleMessageWriter());
#endif
            messageWriterList.Add(BuildTextLogMessageWriter());
            messageWriterList.Add(BuildNotificationPanelMessageWriter());
            messageWriterList.Add(BuildPopupMessageWriter());
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
                PrintErrorMessages = Settings.Default.NotificationPanelWriterWriteErrorMsgs,
                FocusOnInfoMessages = Settings.Default.SwitchToMCOnInformation,
                FocusOnWarningMessages = Settings.Default.SwitchToMCOnWarning,
                FocusOnErrorMessages = Settings.Default.SwitchToMCOnError
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