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
                    foreach (var message in messages)
                        printer.Write(message);
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
                AllowDebugMessages = Settings.Default.TextLogMessageWriterWriteDebugMsgs,
                AllowInfoMessages = Settings.Default.TextLogMessageWriterWriteInfoMsgs,
                AllowWarningMessages = Settings.Default.TextLogMessageWriterWriteWarningMsgs,
                AllowErrorMessages = Settings.Default.TextLogMessageWriterWriteErrorMsgs
            };
        }

        private static NotificationPanelMessageWriter BuildNotificationPanelMessageWriter()
        {
            return new NotificationPanelMessageWriter(Windows.ErrorsForm)
            {
                AllowDebugMessages = Settings.Default.NotificationPanelWriterWriteDebugMsgs,
                AllowInfoMessages = Settings.Default.NotificationPanelWriterWriteInfoMsgs,
                AllowWarningMessages = Settings.Default.NotificationPanelWriterWriteWarningMsgs,
                AllowErrorMessages = Settings.Default.NotificationPanelWriterWriteErrorMsgs,
                FocusOnInfoMessages = Settings.Default.SwitchToMCOnInformation,
                FocusOnWarningMessages = Settings.Default.SwitchToMCOnWarning,
                FocusOnErrorMessages = Settings.Default.SwitchToMCOnError
            };
        }

        private static PopupMessageWriter BuildPopupMessageWriter()
        {
            return new PopupMessageWriter
            {
                AllowDebugMessages = Settings.Default.PopupMessageWriterWriteDebugMsgs,
                AllowInfoMessages = Settings.Default.PopupMessageWriterWriteInfoMsgs,
                AllowWarningMessages = Settings.Default.PopupMessageWriterWriteWarningMsgs,
                AllowErrorMessages = Settings.Default.PopupMessageWriterWriteErrorMsgs
            };
        }
    }
}