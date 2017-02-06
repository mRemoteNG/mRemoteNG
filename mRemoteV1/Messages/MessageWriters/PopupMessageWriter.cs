using System;
using System.Windows.Forms;

namespace mRemoteNG.Messages.MessageWriters
{
    public class PopupMessageWriter : IMessageWriter
    {
        public bool PrintDebugMessages { get; set; } = true;
        public bool PrintInfoMessages { get; set; } = true;
        public bool PrintWarningMessages { get; set; } = true;
        public bool PrintErrorMessages { get; set; } = true;

        public void Print(IMessage message)
        {
            if (!WeShouldPrint(message))
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

        private bool WeShouldPrint(IMessage message)
        {
            if (message.OnlyLog)
                return false;

            switch (message.Class)
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
                    throw new ArgumentOutOfRangeException(nameof(message.Class), message.Class, null);
            }
            return false;
        }
    }
}