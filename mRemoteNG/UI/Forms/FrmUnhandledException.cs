using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class FrmUnhandledException : Form
    {
        private readonly bool _isFatal;
        private readonly Exception? _exception;
        private bool _submitted;

        public FrmUnhandledException()
            : this(null, false)
        {
        }

        public FrmUnhandledException(Exception? exception, bool isFatal)
        {
            _isFatal = isFatal;
            _exception = exception;
            InitializeComponent();
            SetLanguage();

            if (exception == null)
                return;

            textBoxExceptionMessage.Text = exception.Message;
            textBoxStackTrace.Text = exception.Demystify().StackTrace;
            SetEnvironmentText();
        }

        private void SetEnvironmentText()
        {
            textBoxEnvironment.Text = new StringBuilder()
                .AppendLine(CultureInfo.InvariantCulture, $"OS: {Environment.OSVersion}")
                .AppendLine(CultureInfo.InvariantCulture, $"{GeneralAppInfo.ProductName} Version: {GeneralAppInfo.ApplicationVersion}")
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
            buttonCreateBug.Text = Language.SubmitErrorReport;
            buttonCopyAll.Text = Language.CopyAll;
            buttonClose.Text = _isFatal
                ? Language.Exit
                : Language._Close;
        }

        private void buttonCopyAll_Click(object sender, EventArgs e)
        {
            string text = new StringBuilder()
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

        private async void buttonCreateBug_Click(object sender, EventArgs e)
        {
            if (_submitted)
                return;

            string? token = GetCrashReportToken();
            if (!string.IsNullOrEmpty(token))
            {
                await SubmitViaApiAsync(token);
            }
            else
            {
                OpenPreFilledIssueUrl();
            }
        }

        private static string? GetCrashReportToken()
        {
            return Assembly.GetExecutingAssembly()
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => string.Equals(a.Key, "CrashReportToken", StringComparison.Ordinal))?.Value;
        }

        private async Task SubmitViaApiAsync(string token)
        {
            buttonCreateBug.Enabled = false;
            buttonCreateBug.Text = "Submitting...";

            try
            {
                string title = BuildIssueTitle();
                string body = BuildIssueBody();

                var payload = new
                {
                    title,
                    body,
                    labels = new[] { "bug", "crash-report", "auto-submitted" }
                };

                string json = JsonSerializer.Serialize(payload);
                string apiUrl = $"https://api.github.com/repos/{GeneralAppInfo.CrashReportOwner}/{GeneralAppInfo.CrashReportRepo}/issues";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"{GeneralAppInfo.ProductName}/{GeneralAppInfo.ApplicationVersion}");

                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _submitted = true;
                    string responseBody = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseBody);
                    string issueUrl = doc.RootElement.GetProperty("html_url").GetString() ?? string.Empty;
                    buttonCreateBug.Text = "Submitted!";
                    MessageBox.Show(this,
                        $"Error report submitted successfully.\n\n{issueUrl}",
                        GeneralAppInfo.ProductName,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    // API call failed — fallback to browser
                    buttonCreateBug.Text = Language.SubmitErrorReport;
                    buttonCreateBug.Enabled = true;
                    OpenPreFilledIssueUrl();
                }
            }
            catch
            {
                // Network error — fallback to browser
                buttonCreateBug.Text = Language.SubmitErrorReport;
                buttonCreateBug.Enabled = true;
                OpenPreFilledIssueUrl();
            }
        }

        private void OpenPreFilledIssueUrl()
        {
            string url = BuildPreFilledIssueUrl();
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        }

        private string BuildIssueTitle()
        {
            string exceptionType = _exception?.GetType().FullName ?? "Unknown";
            string exceptionMessage = _exception?.Message ?? "No message";
            return $"[Crash] {exceptionType}: {Truncate(exceptionMessage, 80)}";
        }

        private string BuildIssueBody()
        {
            string exceptionType = _exception?.GetType().FullName ?? "Unknown";
            string exceptionMessage = _exception?.Message ?? "No message";
            string stackTrace = textBoxStackTrace.Text ?? "";
            string environment = textBoxEnvironment.Text ?? "";

            var body = new StringBuilder();
            body.AppendLine("## Crash Report");
            body.AppendLine();
            body.AppendLine("### Exception");
            body.AppendLine(CultureInfo.InvariantCulture, $"**Type:** `{exceptionType}`");
            body.AppendLine(CultureInfo.InvariantCulture, $"**Message:** {exceptionMessage}");
            body.AppendLine();
            body.AppendLine("### Stack Trace");
            body.AppendLine("```");
            body.AppendLine(Truncate(stackTrace, 4000));
            body.AppendLine("```");
            body.AppendLine();
            body.AppendLine("### Environment");
            body.AppendLine("```");
            body.AppendLine(environment);
            body.AppendLine("```");
            body.AppendLine();
            body.AppendLine(_isFatal ? "> **Fatal:** This exception forced the application to close." : "> **Non-fatal:** The application continued running.");
            body.AppendLine();
            body.AppendLine("---");
            body.AppendLine("*Auto-generated crash report from mRemoteNG*");

            return body.ToString();
        }

        private string BuildPreFilledIssueUrl()
        {
            string title = BuildIssueTitle();
            string issueBody = BuildIssueBody();

            string encodedTitle = Uri.EscapeDataString(title);
            string encodedBody = Uri.EscapeDataString(issueBody);
            string encodedLabels = Uri.EscapeDataString("bug,crash-report");

            string url = $"{GeneralAppInfo.UrlBugs}?title={encodedTitle}&body={encodedBody}&labels={encodedLabels}";
            if (url.Length > 8000)
            {
                int excess = url.Length - 8000;
                if (issueBody.Length > excess + 100)
                    issueBody = issueBody[..^(excess + 100)] + "\n\n*(truncated due to URL length limit)*";
                encodedBody = Uri.EscapeDataString(issueBody);
                url = $"{GeneralAppInfo.UrlBugs}?title={encodedTitle}&body={encodedBody}&labels={encodedLabels}";
            }

            return url;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value ?? "";
            return value.Length <= maxLength ? value : value[..maxLength] + "...";
        }
    }
}
