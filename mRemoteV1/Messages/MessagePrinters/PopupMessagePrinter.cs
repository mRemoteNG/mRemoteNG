using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mRemoteNG.Messages.MessagePrinters
{
    public class PopupMessagePrinter : IMessagePrinter
    {
        public bool PrintDebugMessages { get; set; } = true;
        public bool PrintInfoMessages { get; set; } = true;
        public bool PrintWarningMessages { get; set; } = true;
        public bool PrintErrorMessages { get; set; } = true;

        public void Print(IMessage message)
        {
            if (!WeShouldPrint(message.Class))
                return;

            switch (message.Class)
            {
                case MessageClass.DebugMsg:
                    MessageBox.Show(message.Text, string.Format(Language.strTitleInformation, message.Date), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageClass.InformationMsg:
                    MessageBox.Show(message.Text, string.Format(Language.strTitleInformation, message.Date), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageClass.WarningMsg:
                    MessageBox.Show(message.Text, string.Format(Language.strTitleWarning, message.Date), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case MessageClass.ErrorMsg:
                    MessageBox.Show(message.Text, string.Format(Language.strTitleError, message.Date), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool WeShouldPrint(MessageClass msgClass)
        {
            switch (msgClass)
            {
                case MessageClass.InformationMsg:
                    if (PrintInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (PrintWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (PrintErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (PrintDebugMessages) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(msgClass), msgClass, null);
            }
            return false;
        }

        public void Print(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
                Print(message);
        }
    }
}