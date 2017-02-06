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
            messageWriterList.Add(BuildDebugConsoleWriter());
#endif
            messageWriterList.Add(BuildTextLogMessageWriter());
            messageWriterList.Add(BuildNotificationPanelMessageWriter());
            messageWriterList.Add(BuildPopupMessageWriter());
        }

        private static IMessageWriter BuildDebugConsoleWriter()
        {
            return new DebugConsoleMessageWriter();
        }

        private static IMessageWriter BuildTextLogMessageWriter()
        {
            return new MessageTypeFilterDecorator(
                new MessageTypeFilteringOptions
                {
                    AllowDebugMessages = Settings.Default.TextLogMessageWriterWriteDebugMsgs,
                    AllowInfoMessages = Settings.Default.TextLogMessageWriterWriteInfoMsgs,
                    AllowWarningMessages = Settings.Default.TextLogMessageWriterWriteWarningMsgs,
                    AllowErrorMessages = Settings.Default.TextLogMessageWriterWriteErrorMsgs
                },
                new TextLogMessageWriter(Logger.Instance)
            );
        }

        private static IMessageWriter BuildNotificationPanelMessageWriter()
        {
            
            return new MessageTypeFilterDecorator(
                new MessageTypeFilteringOptions
                {
                    AllowDebugMessages = Settings.Default.NotificationPanelWriterWriteDebugMsgs,
                    AllowInfoMessages = Settings.Default.NotificationPanelWriterWriteInfoMsgs,
                    AllowWarningMessages = Settings.Default.NotificationPanelWriterWriteWarningMsgs,
                    AllowErrorMessages = Settings.Default.NotificationPanelWriterWriteErrorMsgs
                },
                new MessageFocusDecorator(
                    Windows.ErrorsForm,
                    new MessageTypeFilteringOptions
                    {
                        AllowInfoMessages = Settings.Default.SwitchToMCOnInformation,
                        AllowWarningMessages = Settings.Default.SwitchToMCOnWarning,
                        AllowErrorMessages = Settings.Default.SwitchToMCOnError
                    },
                    new NotificationPanelMessageWriter(Windows.ErrorsForm)
                )
            );
        }

        private static IMessageWriter BuildPopupMessageWriter()
        {
            return new MessageTypeFilterDecorator(
                new MessageTypeFilteringOptions
                {
                    AllowDebugMessages = Settings.Default.PopupMessageWriterWriteDebugMsgs,
                    AllowInfoMessages = Settings.Default.PopupMessageWriterWriteInfoMsgs,
                    AllowWarningMessages = Settings.Default.PopupMessageWriterWriteWarningMsgs,
                    AllowErrorMessages = Settings.Default.PopupMessageWriterWriteErrorMsgs
                },
                new PopupMessageWriter()
            );
        }
    }
}