using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mRemoteNG.Messages.MessagePrinters
{
    public class PopupMessagePrinter : IMessagePrinter
    {
        public void Print(IMessage message)
        {
            switch (message.Class)
            {
                case MessageClass.ReportMsg:
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

        public void Print(IEnumerable<IMessage> messages)
        {
            foreach (var message in messages)
                Print(message);
        }
    }
}