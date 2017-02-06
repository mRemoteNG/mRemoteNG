using System;
using System.Windows.Forms;

namespace mRemoteNG.Messages.MessageWriters
{
    public class PopupMessageWriter : IMessageWriter
    {
        public bool AllowDebugMessages { get; set; } = true;
        public bool AllowInfoMessages { get; set; } = true;
        public bool AllowWarningMessages { get; set; } = true;
        public bool AllowErrorMessages { get; set; } = true;

        public void Write(IMessage message)
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
                    if (AllowInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (AllowWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (AllowErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (AllowDebugMessages) return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(message.Class), message.Class, null);
            }
            return false;
        }
    }
}