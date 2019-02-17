using System;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms
{
    public partial class UnhandledExceptionWindow : Form
    {
        private readonly bool _isFatal;

        public UnhandledExceptionWindow()
            : this(null, false)
        {
        }

        public UnhandledExceptionWindow(Exception exception, bool isFatal)
        {
            _isFatal = isFatal;
            InitializeComponent();
            SetLanguage();

            if (exception == null)
                return;

            textBoxExceptionMessage.Text = exception.Message;
            textBoxStackTrace.Text = exception.StackTrace;
        }

        private void SetLanguage()
        {
            Text = Language.mRemoteNGUnhandledException;
            labelExceptionCaught.Text = Language.UnhandledExceptionOccured;

            labelExceptionIsFatalHeader.Text = _isFatal
                ? Language.ExceptionForcesmRemoteNGToClose
                : string.Empty;

            labelExceptionMessageHeader.Text = Language.ExceptionMessage;
            labelStackTraceHeader.Text = Language.StackTrace;
            buttonCopyAll.Text = Language.strMenuNotificationsCopyAll;
            buttonClose.Text = _isFatal
                ? Language.strMenuExit
                : Language.strButtonClose;
        }

        private void buttonCopyAll_Click(object sender, EventArgs e)
        {
            var text = new StringBuilder()
                       .AppendLine(labelExceptionMessageHeader.Text)
                       .AppendLine("\"" + textBoxExceptionMessage.Text + "\"")
                       .AppendLine()
                       .AppendLine(labelStackTraceHeader.Text)
                       .AppendLine(textBoxStackTrace.Text)
                       .ToString();

            Clipboard.SetText(text);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_isFatal)
                Shutdown.Quit();

            Close();
        }
    }
}