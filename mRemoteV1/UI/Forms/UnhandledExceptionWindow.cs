using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;

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
            SetEnvironmentText();
        }

        private void SetEnvironmentText()
        {
            textBoxEnvironment.Text = new StringBuilder()
                .AppendLine($"OS: {Environment.OSVersion}")
                .AppendLine($"{GeneralAppInfo.ProductName} Version: {GeneralAppInfo.ApplicationVersion}")
                .AppendLine("Edition: " + (Runtime.IsPortableEdition ? "Portable" : "MSI"))
                .AppendLine("Cmd line args: " + string.Join(" ", Environment.GetCommandLineArgs().Skip(1)))
                .ToString();
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
            labelEnvironment.Text = Language.Environment;
            buttonCreateBug.Text = Language.strMenuReportBug;
            buttonCopyAll.Text = Language.strMenuNotificationsCopyAll;
            buttonClose.Text = _isFatal
                ? Language.strMenuExit
                : Language.strButtonClose;
        }

        private void buttonCopyAll_Click(object sender, EventArgs e)
        {
            var text = new StringBuilder()
               .AppendLine("```")
               .AppendLine(labelExceptionMessageHeader.Text)
               .AppendLine("\"" + textBoxExceptionMessage.Text + "\"")
               .AppendLine()
               .AppendLine(labelStackTraceHeader.Text)
               .AppendLine(textBoxStackTrace.Text)
               .AppendLine()
               .AppendLine(labelEnvironment.Text)
               .AppendLine(textBoxEnvironment.Text)
               .AppendLine("```")
               .ToString();

            Clipboard.SetText(text);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (_isFatal)
                Shutdown.Quit();

            Close();
        }

        private void buttonCreateBug_Click(object sender, EventArgs e)
        {
            Process.Start(GeneralAppInfo.UrlBugs);
        }
    }
}